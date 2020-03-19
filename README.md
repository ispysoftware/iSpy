# Overview

https://www.ispyconnect.com/

iSpy is the world’s most popular open source video surveillance application. It's compatible with the the vast majority of consumer webcams and IP cameras. With more than 2 million users worldwide, iSpy works with more cameras and devices than anything else on the market.

![iSpyInterface](https://www.ispyconnect.com/content/ebook/ispysurface.jpg)

## Agent
We have a new platform called Agent which runs as a service and has a full local and remote UI that works on all devices and doesn't require port forwarding for remote access. We've added everything in iSpy plus a lot more to Agent. Check it out on our downloads page

https://www.ispyconnect.com/download.aspx

## About iSpy

Started back in 2007 the software has continually evolved and improved to become a robust, feature rich solution.

The number one use of iSpy is small business security, but home monitoring, neighborhood watch, checking in on the kids, desktop monitoring and mobile access through a iSpyConnect.com are valued features.

Facial recognition and detection of changes in lighting and audio offer the subtleties that set the software apart from competitors.

Getting started with iSpy is easy: all you need is a webcam or IP camera connected to your computer or network.

iSpy connects to the camera and shows the live view. You can then define specific areas of the video that iSpy should watch for movement, and set a threshold value for the amount of motion that would trigger automatic recording. iSpy can also operate in always-recording or manual-recording modes and supports scheduling and remote access (with an iSpyConnect subscription)

iSpy was designed to provide a low-cost alternative to expensive surveillance systems. It has become a highly scalable application that can be tailored to record and take actions on specific incidents as defined by the user either locally or remotely.

## Installing iSpy

https://www.ispyconnect.com/download.aspx

## Compiling iSpy
The solution requires **Visual Studio 2019** to build. Choose 32 or 64 bit version to build.

For building the Setup project [Wix Toolset 3.11](http://wixtoolset.org/) must be installed. (Make sure you restart Visual Studio after installing)

To build the full installer select x86 or x64 Release mode and compile the Bootstrap project. Installer will be generated in  
Wix\Bootstrap\bin\Release

***Remove the signing post build event command as the code signing certificate is not part of the source code of the project.***

If you have dll reference errors when building you may need to go into the DLLS folder and right-click - unblock the DLLs. (Windows Security issue)


