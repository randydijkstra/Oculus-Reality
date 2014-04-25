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
 * MapNewMarker.js
 *
 * Usage: Add this script to a GameObject
 * 
 *
 * Notes:
 * This script will instantiate a new marker of the designated type 
 * when an "OnState" element is clicked
 *
 * ========================================================================*/
 
var MarkerPrefab : GameObject;

function Update()
{
	var ray;
	if (Application.platform == RuntimePlatform.OSXPlayer || 
		Application.platform == RuntimePlatform.OSXEditor ||
		Application.platform == RuntimePlatform.WindowsPlayer ||
		Application.platform == RuntimePlatform.WindowsEditor ) // Unity 2.5 doesn't know about IPhonePlayer
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	else
		ray = Camera.main.ScreenPointToRay(Vector2(Screen.width/2, Screen.height/2));
	var hit : RaycastHit;
	var marker = GetComponent(TrackerSingleMarker) as TrackerSingleMarker;
	if (marker.visible == false)
	{
		transform.localPosition = Vector3(0,0,1000);
		transform.localRotation = Quaternion.identity;
	}
	else if (Physics.Raycast(ray, hit, Mathf.Infinity)) 
	{  
		if (hit.transform == transform.Find("OffState"))
		{
			hit.transform.gameObject.SetActiveRecursively(false);
			hit.transform.parent.Find("OnState").gameObject.SetActiveRecursively(true);
		}
		else if (hit.transform.gameObject.name == "OnState")
		{
			if (Input.GetMouseButtonDown(0))
			{
				var newObject : GameObject;
				newObject = Instantiate(MarkerPrefab,hit.transform.parent.localPosition,hit.transform.parent.localRotation);
				var newMarker = newObject.GetComponent(TrackerSingleMarker) as TrackerSingleMarker;
				newMarker.id = marker.id;
				newMarker.data = marker.data;
				newMarker.Size = marker.Size;
				newMarker.MarkerType = marker.MarkerType;
				newMarker.options = marker.options;
				var materialName : String = marker.MarkerType.ToString();
				materialName += "_";
				materialName += marker.id;
				newObject.transform.Find("Plane").renderer.material = Resources.Load(materialName,Material);
				var map : TrackerMap = Component.FindObjectOfType(TrackerMap) as TrackerMap;
				if (map != null)
					map.AddMarker(newMarker);
				else
					Debug.Log("No TrackerMap found");
			}

		}
		else
		{
			var onObject = GameObject.Find("OnState");
			if (onObject != null)
			{
				onObject.SetActiveRecursively(false);
				onObject.transform.parent.Find("OffState").gameObject.SetActiveRecursively(true);
			}
		}				
	}
}