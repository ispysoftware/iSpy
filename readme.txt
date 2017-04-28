# Compiling iSpy
The 32 bit and 64 bit solutions require **Visual Studio 2015** to build.

For building the Setup project [Wix Toolset 3.10+](http://wixtoolset.org/) must be installed. (Make sure you restart Visual Studio after installing)

To build the full installer compile the Bootstrap32 or Bootstrap64 project
If there is an error finding a merge module building the setup project then the merge modules are available in the Merge Modules directory


If you have dll reference errors when building you may need to go into the DLLS folder and right-click - unblock the DLLs. (Windows Security issue)

