/* ========================================================================
 * PROJECT: UART
 * ========================================================================
 * Portions of this work are built on top of the ARToolkitPlus, 
 * which is in turn based on the original ARToolKit developed by
 *   Hirokazu Kato
 *   Mark Billinghurst
 *   HITLab, University of Washington, Seattle
 * http://www.hitl.washington.edu/artoolkit/
 *
 * Portions of this work are also built on top of the VideoWrapper,
 * a BSD licensed video access library for MacOSX and Windows.
 * VideoWrapper is available at SourceForge via 
 * http://sourceforge.net/projects/videowrapper/
 *
 * Copyright of the ARToolkitPlus is
 *     (C) 2006 Graz University of Technology
 *
 * Copyright of VideoWrapper is
 *     (C) 2003-2010 Georgia Tech Research Corportation
 *
 * Copyright of the new and derived portions of this work
 *     (C) 2010 Georgia Tech Research Corporation
 *
 * This software is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this framework; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 * For further information regarding UART, please contact 
 *   Blair MacIntyre
 *   <blair@cc.gatech.edu>
 *   Georgia Tech, School of Interactive Computing
 *   85 5th Street NW
 *   Atlanta, GA 30308
 *
 * For further information regarding ARToolkitPlus, please contact 
 *   Dieter Schmalstieg
 *   <schmalstieg@icg.tu-graz.ac.at>
 *   Graz University of Technology, 
 *   Institut for Computer Graphics and Vision,
 *   Inffeldgasse 16a, 8010 Graz, Austria.
 *
 * ========================================================================
 ** @author   Alex Hill (ahill@gatech.edu)
 *
 * ========================================================================
 *
 * InitializeMarkers.js
 *
 * Usage: Add this script to the main camera
 * 
 *
 * Notes:
 * This script will sequentially initialize the marker id of 9 markers
 * based on the id of the first marker that becomes visible
 *
 * ========================================================================*/

var InitMarker : TrackerSingleMarker;
var Marker1 : TrackerSingleMarker;
var Marker2 : TrackerSingleMarker;
var Marker3 : TrackerSingleMarker;
var Marker4 : TrackerSingleMarker;
var Marker5 : TrackerSingleMarker;
var Marker6 : TrackerSingleMarker;
var Marker7 : TrackerSingleMarker;
var Marker8 : TrackerSingleMarker;
var Marker9 : TrackerSingleMarker;

var initialized : boolean = false;

function Update()
{
	if (!initialized)
	{
		if (InitMarker.id != -1)
		{
			var tracker : TrackerManager = Component.FindObjectOfType(TrackerManager);
			if (Marker9 != null)
			{
				Marker9.enabled = true;
				Marker9.id = Marker9.id - Marker1.id + InitMarker.id;
				tracker.RegisterSingleMarker(Marker9);
			}
			if (Marker8 != null)
			{
				Marker8.enabled = true;
				Marker8.id = Marker8.id - Marker1.id + InitMarker.id;
				tracker.RegisterSingleMarker(Marker8);
			}
			if (Marker7 != null)
			{
				Marker7.enabled = true;
				Marker7.id = Marker7.id - Marker1.id + InitMarker.id;
				tracker.RegisterSingleMarker(Marker7);
			}
			if (Marker6 != null)
			{
				Marker6.enabled = true;
				Marker6.id = Marker6.id - Marker1.id + InitMarker.id;
				tracker.RegisterSingleMarker(Marker6);
			}
			if (Marker5 != null)
			{
				Marker5.enabled = true;
				Marker5.id = Marker5.id - Marker1.id + InitMarker.id;
				tracker.RegisterSingleMarker(Marker5);
			}
			if (Marker4 != null)
			{
				Marker4.enabled = true;
				Marker4.id = Marker4.id - Marker1.id + InitMarker.id;
				tracker.RegisterSingleMarker(Marker4);
			}
			if (Marker3 != null)
			{
				Marker3.enabled = true;
				Marker3.id = Marker3.id - Marker1.id + InitMarker.id;
				tracker.RegisterSingleMarker(Marker2);
			}
			if (Marker2 != null)
			{
				Marker2.enabled = true;
				Marker2.id = Marker2.id - Marker1.id + InitMarker.id;
				tracker.RegisterSingleMarker(Marker2);
			}
			if (Marker1 != null)
			{
				Marker1.enabled = true;
				Marker1.id = InitMarker.id;
				tracker.RegisterSingleMarker(Marker1);
			}
			//add any other initialization steps here
			initialized = true;
		}
	}
}