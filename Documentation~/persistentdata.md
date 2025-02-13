# Documentation

## Overview

A Unity package that makes saving and loading data easier.

## Package contents

- 3 Classes
- 1 Assembly Definition
- 1 Sample
  - 3 Classes
  - 1 Text file

## Installation Instructions

Installing this package via the Unity package manager is the best way to install this package. There are multiple methods to install a third-party package using the package manager, the recommended one is `Install package from Git URL`. The URL for this package is `https://github.com/arwtsh/PersistentData.git`. The Unity docs contains a walkthrough on how to install a package. It also contains information on [specifying a specific branch or release](https://docs.unity3d.com/6000.0/Documentation/Manual/upm-git.html#revision).

Alternatively, you can download directly from this GitHub repo and extract the .zip file into the folder `ProjectRoot/Packages/com.jjasundry.persistentdata`. This will also allow you to edit the contents of the package.

## Requirements

Tested on Unity version 6000.0; will most likely work on older versions, but you will need to manually port it.

## Description of Assets

`PersistentDataBase` is an abstract class that should be inherited by all types that you want to be able to save. This is what a Settings class, GameProgress class, etc should inherit.

`SaveStyle` is an abstract class that contains two methods: `Save` and `Load`. This is what determines how data is saved and loaded. A class that inherits from SaveStyle determines the method of saving and loading, such as PlayerPrefs, to a file, wether or not there are save slots, etc.

`SavableData` is the wrapper data is kept in, along with the `SaveStyle` it will use. This is what should become a singleton in a manager class. All API calls should be made to the `SavableData` class. Although `SaveStyle` has public save and load methods, it should be called from the wrapper. 

When initializing the persistent data, the `SaveStyle` also needs to be initialized. This can be done inline: `new SavableData<Settings>(new PlayerPrefSaver("settings"))`, but then the custom `SaveStyle` API can't be accessed. If a `SaveStyle` has multiple save slots, the save slot will need to be set from the `SaveStyle` API, not the SavableData. `PlayerPrefSaver` in the sample doesn't have an API so it can be inline. Don't share `SaveStyles` instances between `SavableData` instances, unless you want the data to save weirdly.

## Samples

Includes 1 sample. This sample shows how to set up data for settings and includes 2 basic SaveStyles (PlayerPrefs and FileSystem).
