# packager
packaging programs, binaries, files and other files into zip file format called release.zip
## In easy steps
1. Configure packager using command line 
> packager.exe -s --g
Or use -h for more informations
2. Click packager.exe and done

# Features
This packager contains several features

## Multiple release directories
You can include more then directory to be build into zip format. Each dir will produce its release.zip file.

## Exclude list

You can exclude files that aren't needed or you won't show it to the others. Its contain two features

1. Real filename : so you can chose a filename by browsing the file or add the file using command line. This option let you decide this file will not include in your specific directory
2. Global file : you can exclude file you won't be shown in all release directories

## For Developers

You can easily put this file location into your build project. So after successful build, packager will absolutely will work and you can also view the output. I always use this program for many projects in visual studio and it worked fine

## Fully support in command line

You can use the command line to configure the project easily using easy command line arguments
