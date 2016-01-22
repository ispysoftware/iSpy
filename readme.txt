# Compiling iSpy
The 32 bit and 64 bit solutions require **Visual Studio 2015** to build.
The FFMPEG project requires that visual studio 2010 is installed.

To build without visual studio 2010 you should:

for 32 bit:

1) remove the ffmpeg project from the solution
2) copy the files in /ffmpeg/ffmpeg/bin into /ffmpeg/bin
3) add a reference to the ispy.video.ffmpeg.dll file in /ffmpeg/bin

for 64 bit:

1) remove the ffmpeg project from the solution
2) copy the files in /ffmpeg/ffmpeg64/bin into /ffmpeg/bin64
3) add a reference to the ispy.video.ffmpeg.dll in /ffmpeg/bin64


For building the Setup project [Wix Toolset 3.10+](http://wixtoolset.org/) must be installed. (Make sure you restart Visual Studio after installing)

To build the full installer compile the Bootstrap32 or Bootstrap64 project
If there is an error finding a merge module building the setup project then the merge modules are available in the Merge Modules directory

