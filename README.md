# UniverseUGUI

UniverseLib is a library for making plugins which target IL2CPP and Mono Unity games, with a focus on UI-driven plugins.

UniverseUGUI is a fork of UniverseLib that adds support for skins & styles, provides 
new methods for easily creating field-backed UI controls, and streamlines repetitive
layout and skin configuration with disposable context objects.

There is also the `UniverseLib.UGUI` namesace which provides an additional framework similar to IMGUI. 
The goal is to allow easy porting of IMGUI plugins to the more performant UGUI.


## NuGet

[![](https://img.shields.io/nuget/v/LuVoid.UniverseUGUI.Mono?label=UniverseUGUI.Mono)](https://www.nuget.org/packages/LuVoid.UniverseUGUI.Mono)

[![](https://img.shields.io/nuget/v/LuVoid.UniverseUGUI.UnityEditor.Legacy?label=UniverseUGUI.UnityEditor.Legacy)](https://www.nuget.org/packages/LuVoid.UniverseUGUI.UnityEditor.Legacy)  

## Documentation

There is no documentation for UniverseUGUI yet.

Legacy documentation and usage guides can currently be found on the [Wiki](https://github.com/sinai-dev/UniverseLib/wiki).

## UniverseLib.Analyzers

[![](https://img.shields.io/nuget/v/UniverseLib.Analyzers)](https://www.nuget.org/packages/UniverseLib.Analyzers) 
[![](https://img.shields.io/badge/-source-blue?logo=github)](https://github.com/sinai-dev/UniverseLib.Analyzers)

The Analyzers package contains IDE analyzers for using UniverseLib and avoiding common mistakes when making universal Unity mods and tools.

## Acknowledgements

* [Geoffrey Horsington](https://github.com/ghorsington) and [BepInEx](https://github.com/BepInEx) for [ManagedIl2CppEnumerator](https://github.com/BepInEx/BepInEx/blob/master/BepInEx.IL2CPP/Utils/Collections/Il2CppManagedEnumerator.cs) \[[license](https://github.com/BepInEx/BepInEx/blob/master/LICENSE)\], included for IL2CPP coroutine support.
* [Sinai], the original developer of [UniverseLib](https://github.com/sinai-dev/UniverseLib)
