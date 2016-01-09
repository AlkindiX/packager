# Packager
Packaging programs, binaries and other files into zip file format called release.zip automatically. It is useful for dynamic zipping (Automatic zipping). This program help developers to develop their programs and application and zip them automatically. Moreover, it is help full for daily build.

This program uses zip file compress system, because it is available in many platforms, for example Ubuntu, Windows.

## Download Packager 

* You can download latest stable release of packager from [https://github.com/AlkindiX/packager/releases/latest](https://github.com/AlkindiX/packager/releases/latest)

* Or you can download other releases or prerelease version in [https://github.com/AlkindiX/packager/releases](https://github.com/AlkindiX/packager/releases/latest)

## Install Packager 

When you finished downloading Packager. You can install it by unzip the files into your own directory. Just start up packager by pressing packager.exe to inistliziton   it for first use. This step is optionally
Install Packager by unzip the files into your own directory

## Configure it in easy steps

You can configure packager easily by two methods. The first one is using GUI and the other one is CLI. You can use packager by launching the setting window using command line. On the other side, you can use packager in command line using several command line arguments. Both are fully supported.

### Method 1: Graphical user interface (GUI)

1. Type ``` > packager.exe -s --g ```
2. Add release directory by press add button
3. Add some files if you went to exclude them by click add real file or global file
4. Don't forget to press save changes to save your setting
5. Just click ``` > packager.exe ``` and done

### Method 2: using command line

1. Open cmd by open start menu -> run -> cmd and click run
2. Type ``` > cd <packager path> ``` and press enter 
2. Now you need to add your first directory by type ``` > packager -s --r ---add <Release directory path> ```
Then a message will appear that it added successfully. You can add more directories by using this argument
3. Exclude some files from zipping if you don't went to add them by type ``` > packager -s --e ---add <file path> ```
3. Click ``` > packager.exe ``` and package will perform all the zipping automatically 

# Features
This packager contains several features

## Multiple release directories
You can include more than directory to be built into zip format. Packager will produce release.zip file for each directory automatically  

## Exclude list

You can exclude files that aren't needed or you won't show it to the others. Its contain two features

1. Real filename: so you can chose a filename by browsing the file or add the file using command line. This option let you decide this file will not include in your specific directory
2. Global file: you can exclude file you won't be shown in all release directories

## For Developers

You can easily put this file location into your build project. So after successful build, packager will absolutely will work and you can also view the output. I always use this program for many projects in visual studio and it worked fine. Moreover, you can redistribute packager to be more useful.

## Fully support in command line

You can use the command line to configure the project easily using easy command line arguments

## License
Packager is licensed under GPL v3.0
