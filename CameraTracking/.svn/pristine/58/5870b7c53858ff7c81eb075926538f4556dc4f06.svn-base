/* ========================================================================
 * PROJECT: Unity AR Tookit (UART)
 * ========================================================================
 *
 * (C) 2010 by Blair MacIntyer and Georgia Tech Research Corporation
 *
 * ========================================================================
 ** @author   Alex Hill (ahill@gatech.edu)
 *
 * This software is licensed for commercial use.
 *
 * For licensing and support contact Blair MacIntyer (blair@cc.gatech.edu)
 *
 * ========================================================================
 *
 * VideoTexture.cs
 *
 * Usage: Add this script to a GameObject to receive video texture
 * 
 *
 * Notes:
 *
 **********************************************************************************/

using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class VideoTexture : MonoBehaviour {

    public VideoCamera cam;
    private Texture2D m_Texture;
            
    void Start () 
    {  	
    	Debug.Log("Creating Texture " + cam.m_width + " x " + cam.m_height);
	    // Create texture that will be updated with video frames
	    m_Texture = new Texture2D (cam.m_width, cam.m_height, TextureFormat.ARGB32, false);
       	
        // Assign texture to the renderer
        if (renderer)
            renderer.material.mainTexture = m_Texture;
        // or gui texture
        else if (GetComponent(typeof(GUITexture)))
        {
            GUITexture gui = GetComponent(typeof(GUITexture)) as GUITexture;
            gui.texture = m_Texture;
        }
        else
        {
            Debug.Log("Game object has no renderer or gui texture to assign the generated texture to!");
        }
    }
    
    // update the texture from the frame available from the camera
    void LateUpdate () 
    {
   		//Debug.Log("VideoTexture.cs");

   		if(cam.m_vidframe.Length > 0)
   		{
        	m_Texture.SetPixels (cam.m_vidframe, 0);
        	m_Texture.Apply ();
   		}else{
			Debug.Log("VideoTexture.cs Short");
		}
 
    }
}