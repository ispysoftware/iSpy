# Compiling iSpy
The 32 bit and 64 bit solutions require **Visual Studio 2015** to build.
The FFMPEG project requires that visual studio 2010 is installed.
To build without visual studio 2010 you should:

1) remove the ffmpeg project from the solution
2) install ispy and copy the files:

from the install directory into the DLLS directory in iSpy
3) add a reference in ispy to ispy.video.ffmpeg.dll


For building the Setup project [Wix Toolset 3.10+](http://wixtoolset.org/) must be installed. (Make sure you restart Visual Studio after installing)

