[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug',
    [string]$GamePath,
    [string]$ArtifactsPath,
    [string]$StagingPath,
    [string]$DeploymentPath,
    [switch]$CheckDeploymentAccess
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$failureCount = 0
$warningCount = 0

function Write-Check {
    param([string]$Status, [string]$Message)
    Write-Host "[$Status] $Message"
}

function Confirm-WritableDirectory {
    param([string]$Path)
    $probePath = $null
    try {
        [void][System.IO.Directory]::CreateDirectory($Path)
        $probePath = Join-Path $Path ('.preflight-' + [guid]::NewGuid().ToString('N'))
        [System.IO.File]::WriteAllText($probePath, 'preflight')
        Write-Check 'OK' "Writable directory: $Path"
    }
    finally {
        if ($probePath -and [System.IO.File]::Exists($probePath)) {
            [System.IO.File]::Delete($probePath)
        }
    }
}

try {
    # Keep first-run files in this checkout; these settings affect this process only.
    if (-not $env:DOTNET_CLI_HOME) {
        $env:DOTNET_CLI_HOME = Join-Path $repoRoot 'artifacts/dotnet-home'
    }
    $env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
    $env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH = '0'
    $env:DOTNET_GENERATE_ASPNET_CERTIFICATE = 'false'
    $env:DOTNET_NOLOGO = '1'
    $env:MSBuildEnableWorkloadResolver = 'false'

    if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
        throw 'The dotnet command is missing. Install the SDK specified in global.json.'
    }

    $solution = @(Get-ChildItem -LiteralPath $repoRoot -Filter '*.slnx' -File)
    if ($solution.Count -ne 1) {
        throw 'Expected one .slnx solution in the repository root.'
    }
    $solutionXml = [xml](Get-Content -LiteralPath $solution[0].FullName -Raw)
    $projectPath = Join-Path $repoRoot @($solutionXml.Solution.Project)[0].Path
    $projectDirectory = Split-Path -Parent $projectPath

    $arguments = @(
        'msbuild', $projectPath, '-nologo', '-p:Platform=x64',
        "-p:Configuration=$Configuration",
        '-getProperty:GamePath,ArtifactsPath,StagingPath,DeploymentPath,TargetPath,TargetFramework,LangVersion,NETCoreSdkVersion,ExpectedGameVersion',
        '-getItem:Reference'
    )
    foreach ($name in @('GamePath', 'ArtifactsPath', 'StagingPath', 'DeploymentPath')) {
        $value = Get-Variable -Name $name -ValueOnly
        if ($value) {
            $arguments += "-p:${name}=$value"
        }
    }

    Push-Location -LiteralPath $repoRoot
    try {
        $metadataText = & dotnet @arguments
        if ($LASTEXITCODE -ne 0) {
            throw 'MSBuild could not evaluate the project. Check global.json and local settings.'
        }
    }
    finally {
        Pop-Location
    }
    $metadata = ($metadataText -join [Environment]::NewLine) | ConvertFrom-Json
    $properties = $metadata.Properties
    Write-Check 'OK' "SDK $($properties.NETCoreSdkVersion); C# $($properties.LangVersion); target $($properties.TargetFramework); x64."

    foreach ($name in @('GamePath', 'ArtifactsPath', 'StagingPath', 'DeploymentPath', 'TargetPath')) {
        Write-Check 'INFO' "${name}: $($properties.$name)"
    }

    $runtimes = @(& dotnet --list-runtimes)
    if ($LASTEXITCODE -ne 0) {
        throw 'Could not enumerate installed .NET runtimes.'
    }
    $requiredRuntimes = @('Microsoft.NETCore.App')
    if ([Environment]::OSVersion.Platform -eq [PlatformID]::Win32NT) {
        $requiredRuntimes += 'Microsoft.WindowsDesktop.App'
    }
    foreach ($runtime in $requiredRuntimes) {
        if ($runtimes -match ('^' + [regex]::Escape($runtime) + ' 6\.0\.')) {
            Write-Check 'OK' "$runtime 6.0 is installed."
        }
        else {
            Write-Check 'FAIL' "Missing $runtime 6.0. The SDK does not replace the game's runtime."
            $failureCount++
        }
    }

    if ([string]::IsNullOrWhiteSpace($properties.GamePath)) {
        throw 'Set GamePath in Directory.Build.local.props or pass -GamePath to preflight.'
    }
    $gameExecutable = Join-Path $properties.GamePath 'Suzerain.exe'
    if (-not (Test-Path -LiteralPath $gameExecutable -PathType Leaf)) {
        Write-Check 'FAIL' "Suzerain.exe was not found at $gameExecutable."
        $failureCount++
    }

    $references = @($metadata.Items.Reference | Where-Object { $_.HintPath })
    $foundCount = 0
    foreach ($reference in $references) {
        $referencePath = $reference.HintPath
        if (-not [System.IO.Path]::IsPathRooted($referencePath)) {
            $referencePath = Join-Path $projectDirectory $referencePath
        }
        if (Test-Path -LiteralPath $referencePath -PathType Leaf) {
            $foundCount++
        }
        else {
            Write-Check 'FAIL' "Missing assembly: $referencePath"
            $failureCount++
        }
    }
    Write-Check 'INFO' "Found $foundCount of $($references.Count) game/loader references."
    foreach ($assemblyName in @('MelonLoader', 'Il2CppInterop.Runtime')) {
        $reference = $references | Where-Object { $_.Identity -eq $assemblyName } |
            Select-Object -First 1
        if ($reference -and (Test-Path -LiteralPath $reference.HintPath -PathType Leaf)) {
            $identity = [System.Reflection.AssemblyName]::GetAssemblyName($reference.HintPath)
            Write-Check 'INFO' "$assemblyName assembly version: $($identity.Version)"
        }
    }

    $expectedVersion = $properties.ExpectedGameVersion
    $smkInfo = Join-Path $projectDirectory 'SmkInfo.cs'
    if (-not $expectedVersion -and (Test-Path -LiteralPath $smkInfo)) {
        $match = [regex]::Match((Get-Content -LiteralPath $smkInfo -Raw),
            'TargetSuzerainVersion\s*=\s*"([^"]+)"')
        if ($match.Success) {
            $expectedVersion = $match.Groups[1].Value
        }
    }
    $logPath = Join-Path $properties.GamePath 'MelonLoader/Latest.log'
    if (Test-Path -LiteralPath $logPath -PathType Leaf) {
        $startupLines = Get-Content -LiteralPath $logPath -TotalCount 120
        $gameVersion = $startupLines | Select-String 'Game Version:\s*(.+)' |
            Select-Object -Last 1
        $unityVersion = $startupLines | Select-String 'Unity Version:\s*(.+)' |
            Select-Object -Last 1
        if ($gameVersion) {
            $loggedVersion = $gameVersion.Matches[0].Groups[1].Value.Trim()
            Write-Check 'INFO' "Last logged game version: $loggedVersion ($logPath)."
            if ($expectedVersion -and $loggedVersion -ne $expectedVersion) {
                Write-Check 'WARN' "Expected $expectedVersion. The last launch logged $loggedVersion."
                $warningCount++
            }
        }
        if ($unityVersion) {
            Write-Check 'INFO' "Last logged Unity version: $($unityVersion.Matches[0].Groups[1].Value.Trim())"
        }
        Write-Check 'INFO' 'Startup logs describe the last launch; they do not verify current interop freshness.'
    }
    else {
        Write-Check 'WARN' 'No MelonLoader startup log found. Launch the game once to verify versions.'
        $warningCount++
    }

    foreach ($name in @('ArtifactsPath', 'StagingPath')) {
        Confirm-WritableDirectory -Path $properties.$name
    }
    if ($CheckDeploymentAccess) {
        Confirm-WritableDirectory -Path $properties.DeploymentPath
    }
    else {
        Write-Check 'INFO' 'Deployment write access was not probed. Use -CheckDeploymentAccess explicitly.'
    }

    if (Get-Process -Name Suzerain -ErrorAction SilentlyContinue) {
        Write-Check 'WARN' 'Suzerain is running. Close it before deployment.'
        $warningCount++
    }
    Write-Check 'INFO' 'Restricted shells may not see the game process. Verify the game is closed before deployment.'
    Write-Check 'INFO' 'Game installation/save writes can require separate authorization in a sandbox.'
    if ($failureCount -gt 0) {
        Write-Check 'FAIL' "Preflight found $failureCount problem(s) and $warningCount warning(s)."
        exit 1
    }
    Write-Check 'OK' "Preflight completed with $warningCount warning(s). No build or deployment was run."
}
catch {
    Write-Check 'FAIL' $_.Exception.Message
    exit 1
}
