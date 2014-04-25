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
 * DropMarker.js
 *
 * Usage: Add this script to a GameObject
 * 
 *
 * Notes:
 * This script implements the drop marker effect
 *
 * ========================================================================*/
 
var cam : VideoCamera;
var Marker : GameObject;
var depth = 0.02;
var duration = 1.0;
private var animate = false;
private var timer = 0.0;
private var m_Texture : Texture2D;
private var textureWidth = 0;
private var textureHeight = 0;

function Start()
{
	if (Application.platform == RuntimePlatform.OSXPlayer || 
		Application.platform == RuntimePlatform.OSXEditor ||
		Application.platform == RuntimePlatform.WindowsPlayer ||
		Application.platform == RuntimePlatform.WindowsEditor ) // Unity 2.5 doesn't know about IPhonePlayer
	{
		textureWidth = cam.m_width;
		textureHeight = cam.m_height;
		//textureWidth = NextPowerOfTwo(cam.m_width);
		//textureHeight = NextPowerOfTwo(cam.m_height);
	}
	else
	{
		textureWidth = NextPowerOfTwo(cam.m_width);
		textureHeight = NextPowerOfTwo(cam.m_height);
	}

	// Create textures that will be updated in the plugin
	m_Texture = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);
	m_Texture.Apply();
	
    // Assign texture to the renderer
    Marker.renderer.material.mainTexture = m_Texture;
	var xScale = cam.m_width / parseFloat(textureWidth);
	var yScale = cam.m_height / parseFloat(textureHeight);
	System.Console.WriteLine("xScale " + xScale + " yScale " + yScale);
	System.Console.WriteLine("m_width " + cam.m_width + " m_height " + textureWidth);
	Marker.renderer.material.mainTextureScale = new Vector2(xScale, yScale);
	Marker.renderer.material.mainTextureOffset = new Vector2(1.0 - xScale, 0.0);
	
}

function Update()
{
	if (animate)
	{
		var pos = Marker.transform.localPosition;
		pos.z -= Time.deltaTime*depth/duration;
		if (pos.z <= -depth)
			pos.z = -depth;
		timer += Time.deltaTime;
		if (timer > 5.0)
		{
			animate = false;
			pos.z = 0.0;
			timer = 0.0;
		}
		Marker.transform.localPosition = pos;
	}
	else
	{
		var marker = GetComponent(TrackerSingleMarker) as TrackerSingleMarker;
		if (marker.visible == false)
		{
			transform.localPosition = new Vector3(0,0,100);
			transform.localRotation = Quaternion.identity;
		}
		else 
		{  
			if (Application.platform == RuntimePlatform.OSXPlayer || 
				Application.platform == RuntimePlatform.OSXEditor ||
				Application.platform == RuntimePlatform.WindowsPlayer ||
				Application.platform == RuntimePlatform.WindowsEditor ) // Unity 2.5 doesn't know about IPhonePlayer
			{
	        	m_Texture.SetPixels(cam.m_vidframe,0);
	        	m_Texture.Apply();
			}
			else
				cam.SetPixelsByID(m_Texture.GetNativeTextureID());		
			var corn0 = marker.corners[0];
			corn0.x /= cam.m_width;
			corn0.y /= cam.m_height;
			var corn1 = marker.corners[1];
			corn1.x /= cam.m_width;
			corn1.y /= cam.m_height;
			var corn2 = marker.corners[2];
			corn2.x /= cam.m_width;
			corn2.y /= cam.m_height;
			var corn3 = marker.corners[3];
			corn3.x /= cam.m_width;
			corn3.y /= cam.m_height;
			var filter = Marker.GetComponent(MeshFilter) as MeshFilter;
			var uvArray = filter.mesh.uv;
			var index = 0;
			for (var y=0;y<11;y++)
				for (var x=0;x<11;x++)
				{
					var uvLow = corn0 + x*(corn3-corn0)/10.0;
					var uvHigh = corn1 + x*(corn2-corn1)/10.0;
					uvArray[index++] = uvLow + y*(uvHigh-uvLow)/10.0;
				}
			filter.mesh.uv = uvArray;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var hit : RaycastHit;
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, hit, Mathf.Infinity))
			{
				if (hit.transform.gameObject == Marker)
					animate = true;				
			}
		}
	}
}
	
function NextPowerOfTwo (num : int) : int
{
	num--;
	num = (num >> 1) | num;
	num = (num >> 2) | num;
	num = (num >> 4) | num;
	num = (num >> 8) | num;
	num = (num >> 16) | num;
	num++;
	return num;
}
