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
 * TrackerMap.cs
 *
 * Usage: Add this script to a GameObject in the scene
 * 
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using System.Collections; 
using System;

public class TrackerMap : MonoBehaviour 
{    
	
	public TrackerMarker.Marker_Type MarkerType = TrackerMarker.Marker_Type.SimpleID;
	public bool AddAllOfType = true;
    private ArrayList markers = new ArrayList();

	public void AddMarker(TrackerMarker marker)
	{
		markers.Add(marker);
	}
	
	void Start()
	{
		if (AddAllOfType)
		{
  			foreach (TrackerSingleMarker marker in FindObjectsOfType(typeof(TrackerSingleMarker)))
  			{
  				if (marker.MarkerType == MarkerType && marker.Behavior == TrackerMarker.Behavior_Flags.WorldFixed && marker.id >= 0)
  					AddMarker(marker);
  			}
		}
	}
	
	void Update()
	{
				
		TrackerSingleMarker maxMarker = null;
		float maxArea = 0.0f;
	  	foreach (TrackerSingleMarker marker in markers)
  		{
  			if (marker.visible && marker.area*marker.confidence > maxArea)
  			{
  				maxArea = marker.area;
  				maxMarker = marker;
  			}
  		}
  		if (maxMarker != null)
  		{
		  	foreach (TrackerSingleMarker marker in markers)
  			{
  				if (marker == maxMarker)
  				{
  					maxMarker.Behavior = TrackerMarker.Behavior_Flags.WorldFixed;
  					Debug.Log("setting " + maxMarker.id + " to WorldFixed"); 
  				}
  				else
  					marker.Behavior = TrackerMarker.Behavior_Flags.CameraRelative;
  			}
  		}
	}
}