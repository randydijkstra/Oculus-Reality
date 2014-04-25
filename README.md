Oculus-Reality
==============
*Use the Oculus Rift to create true Virtual Reality*


We believe that the Oculus Rift can be used for more than just virtual reality gaming. It can be used to make the next big step in creating Augmented Reality. We hooked up two usb cameras to the Oculus Rift and combined the input to get a view with actual depth. We are also using to the OpenCV/OpenCVSharp library in Unity. This will allow us to create a base for many Augmented Reality applications.


Greetings from the developers of Oculus Reality,

Randy Dijkstra	(<a href="https://twitter.com/Ereinion25" target="_blank">@Ereinion25</a>),
Ricardo Snoek 	(<a href="http://ricardoismy.name" target="_blank">Website</a>)

![alt tag](https://github.com/randydijkstra/Oculus-Reality/blob/master/Photos/Wall-E%20in%20your%20hand.jpg)

HOW DOES IT WORK?
--------------


We want to use OpenCV for camera input analysing. The library contains a wide variation of many nifty features.

*For the current example we don't make use of OpenCV just yet. We have made preparations for the implementation of OpenCV. It will enable to create all sorts of augemented reality features in the future.*


HOW TO SET UP
--------------

**IMPORTANT** *Be sure to follow every step in the description in order to save yourself some unneeded trouble.*

This is a guide to help you set up your project.

**Requirements:**
- Patience 
- Oculus Rift devkit
- 2 USB cameras. We used two disassembled Logitech C270's. Those are very small and thus easy to mount unto the Oculus Rift headset.
- Unity 4 Pro. Oculus Rift support is at the moment only available in the Pro edition.
- OpenCV & OpenCVSharp

**Step 1**
Install Unity Pro 4

**Step 2**
Install OpenCV. 
Guides for platforms can be found in the following links:
- <a href="http://docs.opencv.org/doc/tutorials/introduction/windows_install/windows_install.html">Windows</a>
- <a href="http://docs.opencv.org/doc/tutorials/introduction/linux_install/linux_install.html">Linux<a>
- Mac OSX

**Step 3**
Install OpenCVSharp
If you are on Mac: Copy paste the dylibs from the 'OpenCVSharp' folder into the '/usr/local/lib' folder.

**Step 4**
Open a new project in Unity.