# Contributing

This is a guide to contributing code to Suzerain Modding Kit.

## Coding Guidelines and Info

Please read and follow the [coding guidelines](CODING_GUIDELINES.md) when contributing to Suzerain Modding Kit.

Reading the [Suzerain Implementation Info](suzerain_implementation_info.md) document will also be helpful in understanding how Suzerain works.

## Basic Setup

**Important Note:** Suzerain Modding Kit currently only supports Windows. This guide is intended for the Steam version of Suzerain.

1. Install Visual Studio.
2. Follow the instructions on the [MelonLoader Wiki](https://melonwiki.xyz/#/README) to download and install MelonLoader.
3. Install a .NET decompiler like [dotPeek](https://www.jetbrains.com/decompiler/) or similar program of your choice. Note that the assemblies that MelonLoader generates only contain the type names (class names, method names, etc), not the actual code. In most cases, this is sufficient. If actual code is required, see the [Decompiling with Ghidra](#decompiling-with-ghidra) section.
4. Launch Suzerain so MelonLoader can create the game assembly. Launching the game normally from Steam should launch the MelonLoader console before launching the game. Ensure everything runs successfully. Close the game before continuing.
5. Open `C:\Program Files (x86)\Steam\steamapps\common\Suzerain\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll` in the decompiler to view Suzerain's types. Suzerain types are under the `Il2Cpp` namespace (the other namespaces are Suzerain's dependencies).
6. Fork and clone this repository and open it in Visual Studio.
7. Build the solution and it will automatically be copied to the `Mods` folder (ensure Suzerain is closed). Launch Suzerain to test.

## Decompiling with Ghidra

The standard decompiling method explained above only allows you to see type names. This should be sufficient in most cases, but we can also decompile the actual code using Ghidra.

See [this gist](https://gist.github.com/BadMagic100/47096cbcf64ec0509cf75d48cfbdaea5) which explains how to decompile IL2CPP games (like Suzerain) with Ghidra. **You must make the following changes to the guide to use it in newer versions of Ghidra:**

- The guide tells you to use OpenJDK 17, but newer versions of Ghidra require a different version. See [the latest requirements](https://github.com/NationalSecurityAgency/ghidra/blob/master/GhidraDocs/GettingStarted.md#minimum-requirements).
- Add `# @runtime Jython` to the first line of `ghidra_with_struct.py` before running it in Ghidra.
