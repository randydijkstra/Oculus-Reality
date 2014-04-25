The plugin consists of three scripts located in the project Plugins/TrackerWrapper directory; TrackerMarker.cs, TrackerManager.cs and TrackerMap.cs.
Add the TrackerManager.cs to a game object in the scene to register and track markers.
Each instance of TrackerMarker.cs added to a game object allows a specific type of marker and estimator to be registered with the TrackerManager script.
Marker objects with id -1 will be populated with the location and information from the largest new marker found.

There are four demo scenes included in the TrackerProject directory.
Print fit to a normal page both SingleMarkers.png and Markerboard481-498.png images in the Assets/Resources directory.
The SingleMarkerDemo tracks each of the markers from the SingleMarkers page.
The split markers are currently not working.
The MultiMarkerDemo tracks the Markerboard481-498 board in addition to the single markers.
The MapBuildingDemo shows how a marker can learn the id of unregistered markers found.
The DropMakerDemo demonstrates using corner information to remap the video onto the marker.
Mouse click on the red target to add either the 302 or 303 SimpleID markers to the map.

This project can be run on OSX and Windows using Unity 3.1 and on iPhone 3GS using IOS 4.2.
When building for OSX, copy the Plugins directory to the Contents directory within the OSX application.
When building for WIN32, copy the Plugins directory to the Data directory next to the WIN32 application.
Rename the Editor directory to Editor_ before building executables on WIN32 machines.
Go to Edit->Project Settings->Player and setup the bundle identifier before attempting to build the project.
Change the calibration file to no_distortion_256x384.cal and the camera FOV to 42 in each scene for iPhone 3GS.