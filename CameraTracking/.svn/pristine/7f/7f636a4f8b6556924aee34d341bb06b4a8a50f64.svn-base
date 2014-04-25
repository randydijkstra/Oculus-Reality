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
 * TrackerMarker.cs
 *
 * Usage: This is a base class for marker classes
 * 
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using System.Collections; 
using System;
using System.Runtime.InteropServices;
using System.IO;

public class TrackerMarker : MonoBehaviour 
{    

	public Marker_Type MarkerType = Marker_Type.SimpleID;
    public Marker_Flags[] options = {Marker_Flags.GaussNewton};
	public Behavior_Flags Behavior = Behavior_Flags.CameraRelative;
	public bool visible;
    	
	public enum Behavior_Flags { WorldFixed, CameraRelative };
	public enum Marker_Type {
		SimpleID = 1,
		BCH = 2,
		FrameSimpleID = 4,
		Split = 8,
		DataMatrix = 16,
		Template = 32
	};	
	public enum Marker_Flags {
		NullEstimator = 1024,
		GaussNewton = 2048,
		RPP = 4096,
		MultiMarker = 8192,
		FixedPoint = 16384,
		ThickBorder = 32768,
		SingleMarker = 65536
	};
	    	  
	public int Config()
	{
		int config_flags = 0;
  		config_flags |= (int)MarkerType;
		for (int i=0; i<options.Length; i++)
			config_flags |= (int)options[i];
		if (this.GetType() == typeof(TrackerSingleMarker)) //hack because new keyword not working
			config_flags |= (int)Marker_Flags.SingleMarker;
		else if (this.GetType() == typeof(TrackerMultiMarker))
			config_flags |= (int)Marker_Flags.MultiMarker;
		return config_flags;
	}

}