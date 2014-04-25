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
 * VideoPlane.cs
 *
 * Usage: Add this script to a plane GameObject below the camera
 * 
 *
 * Notes:
 *
 **********************************************************************************/
 
using UnityEngine;
using System;

public class VideoPlane : MonoBehaviour {

    public VideoCamera cam;
    private Texture2D m_Texture;
    private float fov;
            
    void Start () 
    {  	
		#if UNITY_IPHONE
		int textureWidth = NextPowerOfTwo(cam.m_width);
		int textureHeight = NextPowerOfTwo(cam.m_height);
		#else
		int textureWidth = cam.m_width;
		int textureHeight = cam.m_height;
		#endif

		Debug.Log("Creating Plane Texture " + textureWidth + " x " + textureHeight);
		// Create textures that will be updated in the plugin
		m_Texture = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);
		m_Texture.Apply();
		
        // Assign texture to the renderer
        renderer.material.mainTexture = m_Texture;
		float xScale = cam.m_width / (float) textureWidth;
		float yScale = cam.m_height / (float) textureHeight;
		//renderer.material.mainTextureScale = new Vector2(-1, 1);
		renderer.material.mainTextureScale = new Vector2(-xScale, yScale);
		renderer.material.mainTextureOffset = new Vector2(1.0f - xScale, 0.0f);
				
    }
    
    // update the texture from the frame available from the camera
    void LateUpdate () 
    {
    	if (Camera.main.fieldOfView != fov)
    	{
			fov = Camera.main.fieldOfView;
			float planeDist = Camera.main.farClipPlane - 10.0f;
			float halfFOV = (fov / 2.0f) * Mathf.Deg2Rad;
			float planeHeight = Mathf.Tan(halfFOV) * planeDist * 2.0f;
			float planeWidth = planeHeight * Camera.main.aspect;
			transform.localPosition = new Vector3(0, 0, planeDist);
			transform.localScale = new Vector3(planeWidth / 10.0f, 1, planeHeight / 10.0f);
    	}
		
		#if UNITY_IPHONE		
			cam.SetPixelsByID(m_Texture.GetNativeTextureID());		
		#else		
	   		if(cam.m_vidframe.Length > 0)
	   		{
	        	m_Texture.SetPixels (cam.m_vidframe, 0);
	        	m_Texture.Apply ();
	   		}else{
				Debug.Log("VideoTexture.cs Short");
			}		
		#endif

    }
    
    int NextPowerOfTwo (int val)
    {
		val--;
		val = (val >> 1) | val;
		val = (val >> 2) | val;
		val = (val >> 4) | val;
		val = (val >> 8) | val;
		val = (val >> 16) | val;
		val++;
		return val;
    }
}