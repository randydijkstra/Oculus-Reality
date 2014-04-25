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
 * TrackerManager.cs
 *
 * Usage: Add this script to the main camera
 * 
 *
 * Notes:
 * Only a single learned marker is tracked at this time
 *
 * ========================================================================*/

using UnityEngine;
using System.Collections; 
using System;
using System.Runtime.InteropServices;
using System.IO;

public class TrackerManager : MonoBehaviour 
{    
	[ StructLayout( LayoutKind.Sequential, Pack=1)]
	public struct TrackerReport
	{
     	public int id;
     	public int type;
    	[ MarshalAs( UnmanagedType.ByValArray, SizeConst=3 )]
    	public Vector3 pos;
    	[ MarshalAs( UnmanagedType.ByValArray, SizeConst=4 )]
    	public Quaternion quat;
    	public float conf;
    	[ MarshalAs( UnmanagedType.ByValArray, SizeConst=2 )]
    	public Vector2 corner0;
    	[ MarshalAs( UnmanagedType.ByValArray, SizeConst=2 )]
    	public Vector2 corner1;
    	[ MarshalAs( UnmanagedType.ByValArray, SizeConst=2 )]
    	public Vector2 corner2;
    	[ MarshalAs( UnmanagedType.ByValArray, SizeConst=2 )]
    	public Vector2 corner3;
    	public int area;
    	public IntPtr data;
 	}

    public VideoCamera CameraNode;
    public string CalibrationFile;
    public int Threshold = 0;
    public Tracker_Flags[] options;
    public static bool debug_flag = false;
    public bool ShowDebug = false;
    private int handle;
    private StringWriter debug_writer;
	private string[] debug_text;
	private int debug_index = 0;
	private int debug_buffer_pos = 0;
	private int debug_buffer_max = 500;
	private int debug_lines = 8;
    private int MaxReports = 50;
    private IntPtr[] repsPtr;
    private Hashtable markers = new Hashtable();
    private TrackerMarker learningMarker = null;
          
	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("TrackerWrapper/TrackerWrapper")]
    #endif
    private static extern int TRACKER_createTracker(string calibration, int flags, [In,Out] ref int handle);

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("TrackerWrapper/TrackerWrapper")]
    #endif
    private static extern int TRACKER_setThreshold(int handle, float threshold);

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("TrackerWrapper/TrackerWrapper")]
    #endif
    private static extern int TRACKER_registerSingleMarker(int handle, string name, int id, string data, float width, float height, int flags);

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("TrackerWrapper/TrackerWrapper")]
    #endif
    private static extern int TRACKER_registerMultiMarker(int handle, string name, string filename, int flags);

    void Awake() 
    {
    	//allocate unmanaged memory for tracker reports
		repsPtr = new IntPtr[MaxReports];
		TrackerReport report = new TrackerReport();
     	report.pos = new Vector3();
     	report.quat = new Quaternion();
     	report.corner0 = new Vector2();
     	report.corner1 = new Vector2();
     	report.corner2 = new Vector2();
     	report.corner3 = new Vector2();
     	report.conf = 0.0f;
     	report.area = 0;
		for(int i=0; i<MaxReports ; i++)
		{
			repsPtr[i] = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TrackerReport)));
			report.data = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte))*256);
			Marshal.WriteIntPtr((IntPtr)((int)repsPtr[i]+76),report.data);
			//Marshal.StructureToPtr(report, repsPtr[i], true); not working in .NET 1.1
		}
		int config_flags = 0;
		for (int i=0; i<options.Length; i++)
			config_flags |= (int)options[i];
  		foreach (TrackerMarker marker in FindObjectsOfType(typeof(TrackerMarker)))
   			if (marker.enabled)
  				config_flags |= marker.Config();
   		string filepath = Plugins.CreateFileFromAsset(CalibrationFile);
		TRACKER_createTracker(filepath,config_flags,ref handle);
		if (handle >= 0)
		{
			if (Threshold > 0)
				TRACKER_setThreshold(handle,Threshold);
			if (ShowDebug) 
			{
	         	#if !UNITY_IPHONE
	         	debug_text = new string[debug_buffer_max];
	        	debug_writer = new StringWriter();
	         	Console.SetOut(debug_writer);
	         	#endif
	         	Console.WriteLine("Tracker Initialized");
			}
		}
     	debug_flag = ShowDebug;
    }
    
    public void RegisterSingleMarker(TrackerSingleMarker marker)
    {
 		if (marker.enabled)
  		{
 			String uniqueName;
			int config_flags = 0;
  			config_flags |= marker.Config();
  			for (int j=0; j<marker.options.Length; j++)
  				config_flags |= (int)marker.options[j];
			if (marker.id >= 0)
				uniqueName = marker.id.ToString();
			else
			{
				uniqueName = marker.MarkerType.ToString();
				learningMarker = marker;
			}
			TRACKER_registerSingleMarker(handle,uniqueName,marker.id,marker.data,marker.Size.x,marker.Size.y,config_flags);
  			if (ShowDebug)
  				Console.WriteLine("Registering marker " + marker.name + " as " + uniqueName);
   			markers[uniqueName] = marker;
  		}
    }
    
    public void RegisterMultiMarker(TrackerMultiMarker marker)
    {
 		if (marker.enabled)
  		{
 			String uniqueName;
			int config_flags = 0;
  			config_flags |= marker.Config();
  			for (int j=0; j<marker.options.Length; j++)
  				config_flags |= (int)marker.options[j];
 			uniqueName = "_" + marker.GetInstanceID().ToString();
			string filepath = "";
			if (marker.Filename.Length > 0)
			{
				filepath = Plugins.CreateFileFromAsset(marker.Filename);
				TRACKER_registerMultiMarker(handle,uniqueName,filepath,config_flags);
			}
			else
				Console.WriteLine("Error: TrackerMultiMarker requires Filename");
  			if (ShowDebug)
  				Console.WriteLine("Registering marker " + marker.name + " as " + uniqueName);
   			markers[uniqueName] = marker;
  		}
    }

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("TrackerWrapper/TrackerWrapper")]
    #endif
    private static extern int TRACKER_deleteTracker(int handle);

    void OnDisable() 
    {
        //delete the tracker
        TRACKER_deleteTracker(handle);
    	if (ShowDebug) 
    	{
        	StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
        	Console.SetOut(standardOutput);
    	}
    }

    void LateUpdate () 
    {
    	if (CameraNode.m_vidpointer_byte != IntPtr.Zero)
    		UpdateMarkers();
     }

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("TrackerWrapper/TrackerWrapper")]
    #endif
    private static extern int TRACKER_getReports(int handle, IntPtr src, int w, int h, int f, [In,Out] IntPtr[] p, [In,Out] ref int n);
        
	void UpdateMarkers ()
	{
        foreach( DictionaryEntry marker in markers )
            ((TrackerMarker)marker.Value).visible = false;
		int num = MaxReports;
		Pixel_Format format = Pixel_Format.PIXEL_FORMAT_RGB;
		if (CameraNode.m_depth == 32 && CameraNode.m_pixelFormat == 32993)
			format = Pixel_Format.PIXEL_FORMAT_BGRA;
		TRACKER_getReports(handle,CameraNode.m_vidpointer_byte,CameraNode.m_width,CameraNode.m_height,(int)format,repsPtr,ref num);
		if (num > 0)
		{
			TrackerReport[] reps = new TrackerReport[num];
			for(int i=0; i<num ; i++)
			{
				#if UNITY_IPHONE
					reps[i] = MarshalPtrToTrackerReport(repsPtr[i]); // PtrToStructure not working in .NET 1.1
				#else
					reps[i] = (TrackerReport)Marshal.PtrToStructure(repsPtr[i],typeof(TrackerReport)); 
				#endif
				//Convert to left-handed coordinates
				Console.WriteLine("x:" + reps[i].quat[0] + " y:" + reps[i].quat[1] + " z:" + reps[i].quat[2] + " w:" + reps[i].quat[3]);
				reps[i].pos[1] = -reps[i].pos[1];
				reps[i].quat[0] = -reps[i].quat[0];
				reps[i].quat[2] = -reps[i].quat[2];
				TrackerMarker marker = null;
				if (reps[i].type > (int)TrackerMarker.Marker_Type.Split)
					marker = (TrackerMarker)markers[Marshal.PtrToStringAnsi(reps[i].data)];
				else
					marker = (TrackerMarker)markers[reps[i].id.ToString()];
	  			if (ShowDebug)
	  			{
					string debug_line = "Target type: ";
					if (reps[i].type > (int)TrackerMarker.Marker_Type.Split)
						debug_line += "Multi data:" + Marshal.PtrToStringAnsi(reps[i].data);
					else
					{
						debug_line += ((TrackerMarker.Marker_Type)reps[i].type).ToString();
						debug_line += " id:" + reps[i].id;
					} 
					debug_line += " conf:" + reps[i].conf + " area:" + reps[i].area;
					debug_line += " pos:" + reps[i].pos;
					debug_line += " ori:" + reps[i].quat.eulerAngles;
    				Console.WriteLine(debug_line);
 				}	  							
				if (marker != null && marker.Behavior == TrackerMarker.Behavior_Flags.WorldFixed)
				{
  					Camera.main.transform.localPosition = marker.transform.rotation*Quaternion.Inverse(reps[i].quat)*(-reps[i].pos) + marker.transform.position;
  					Camera.main.transform.localRotation = marker.transform.rotation*Quaternion.Inverse(reps[i].quat);
					if (reps[i].type <= (int)TrackerMarker.Marker_Type.Split)
					{
  						TrackerSingleMarker singleMarker = marker as TrackerSingleMarker;
  						singleMarker.confidence = reps[i].conf;
  						singleMarker.corners[0] = reps[i].corner0;
  						singleMarker.corners[1] = reps[i].corner1;
  						singleMarker.corners[2] = reps[i].corner2;
  						singleMarker.corners[3] = reps[i].corner3;
  						singleMarker.area = reps[i].area;
					}
  					marker.visible = true;
  					reps[i].type = 0;
  				}
  			}
  			float maxArea = 0.0f;
  			int learningNum = -1;
			for(int i=0; i<num ; i++)
			{
				if (reps[i].type != 0)
				{
					TrackerMarker marker = null;
					if (reps[i].type > (int)TrackerMarker.Marker_Type.Split)
						marker = (TrackerMarker)markers[Marshal.PtrToStringAnsi(reps[i].data)];
					else
						marker = (TrackerMarker)markers[reps[i].id.ToString()];
					if (marker == null && learningMarker != null)
					{
						if (reps[i].conf*reps[i].area > maxArea)
						{
							maxArea = reps[i].conf*reps[i].area;
							learningNum = i;
						}
					}
	  				if (marker != null && marker.enabled)
	  				{
	  					marker.transform.localPosition = Camera.main.transform.TransformPoint(reps[i].pos);
	  					marker.transform.localRotation = Camera.main.transform.rotation*reps[i].quat;
						if (reps[i].type <= (int)TrackerMarker.Marker_Type.Split)
						{
	  						TrackerSingleMarker singleMarker = marker as TrackerSingleMarker;
	  						singleMarker.confidence = reps[i].conf;
	  						singleMarker.corners[0] = reps[i].corner0;
	  						singleMarker.corners[1] = reps[i].corner1;
	  						singleMarker.corners[2] = reps[i].corner2;
	  						singleMarker.corners[3] = reps[i].corner3;
	  						singleMarker.area = reps[i].area;
						}
	  					marker.visible = true;
	  				}
				}
  			}
  			if (learningNum >= 0)
  			{
	  			learningMarker.transform.localPosition = Camera.main.transform.TransformPoint(reps[learningNum].pos);
	  			learningMarker.transform.localRotation = Camera.main.transform.rotation*reps[learningNum].quat;
				if (reps[learningNum].type <= (int)TrackerMarker.Marker_Type.Split)
				{
 					TrackerSingleMarker singleMarker = learningMarker as TrackerSingleMarker;
					singleMarker.confidence = reps[learningNum].conf;
					singleMarker.corners[0] = reps[learningNum].corner0;
					singleMarker.corners[1] = reps[learningNum].corner1;
					singleMarker.corners[2] = reps[learningNum].corner2;
					singleMarker.corners[3] = reps[learningNum].corner3;
					singleMarker.area = reps[learningNum].area;
	  				singleMarker.id = reps[learningNum].id;
	  				singleMarker.data = Marshal.PtrToStringAnsi(reps[learningNum].data);
	  				singleMarker.confidence = reps[learningNum].conf;
	  				singleMarker.area = reps[learningNum].area;
				}
	  			learningMarker.visible = true;
  			}
  				
		}
		#if !UNITY_IPHONE
    	if (ShowDebug) 
    	{
    		debug_writer.Flush();
    		string buffer = debug_writer.ToString();
    		if (buffer.Length > 0) 
    		{
   		     	StringReader strReader = new StringReader(buffer);
   		     	string line;
    	    	while(true) 
    	    	{
    	    		line = strReader.ReadLine();
    	    		if (line == null || line.Length == 0)
    	    			break;
	        		debug_text[debug_buffer_pos] = line;
	        		debug_buffer_pos++;
	        		if (debug_index < 0 && debug_index > debug_lines - debug_buffer_max)
	        			debug_index--;
	        		if (debug_buffer_pos >= debug_buffer_max)
	        			debug_buffer_pos = 0;
    	    	}
    			debug_writer.Close();
    			debug_writer = new StringWriter();
    			Console.SetOut(debug_writer);
    		}
    	}
    	#endif
    }

	#if !UNITY_IPHONE
	
	void OnGUI () 
	{
		if (ShowDebug) 
		{

			GUI.skin.box.alignment = TextAnchor.LowerLeft;
			debug_index = (int)(GUI.VerticalSlider(new Rect(595, 10, 10, (debug_lines-1)*15+10),
									(float)debug_index, debug_lines-debug_buffer_max, 0));
			string line = "";
			for (int i=debug_lines; i>0; i--) 
			{
				int index = debug_buffer_pos + debug_index - i;
				if (index < 0)
					index += debug_buffer_max;
				line += debug_text[index];
				if (i > 1)
					line += "\n";
			}
			GUI.Box(new Rect(10, 10, 600, (debug_lines-1)*15+10), line);	
				
		}
	}
	
	#endif
	
	TrackerReport MarshalPtrToTrackerReport(IntPtr ptr)
	{
		int[] intVals = new int[2];
		float[] floatVals = new float[8];
		TrackerReport result = new TrackerReport();
		Marshal.Copy(ptr,intVals,0,2);
		Marshal.Copy((IntPtr)((int)ptr+8),floatVals,0,8);
		result.id = intVals[0];
		result.type = intVals[1];
		result.pos.x = floatVals[0];
		result.pos.y = floatVals[1];
		result.pos.z = floatVals[2];
		result.quat.x = floatVals[3];
		result.quat.y = floatVals[4];
		result.quat.z = floatVals[5];
		result.quat.w = floatVals[6];
		result.conf = floatVals[7];
		Marshal.Copy((IntPtr)((int)ptr+40),floatVals,0,8);
		result.corner0.x = floatVals[0];
		result.corner0.y = floatVals[1];
		result.corner1.x = floatVals[2];
		result.corner1.y = floatVals[3];
		result.corner2.x = floatVals[4];
		result.corner2.y = floatVals[5];
		result.corner3.x = floatVals[6];
		result.corner3.y = floatVals[7];
		result.area = Marshal.ReadInt32((IntPtr)((int)ptr+72));
		result.data = Marshal.ReadIntPtr((IntPtr)((int)ptr+76));
		return result;
	}
    
	public enum Pixel_Format {
		PIXEL_FORMAT_UNKNOWN = 0,
		PIXEL_FORMAT_ABGR = 1,
		PIXEL_FORMAT_BGRA = 2,
		PIXEL_FORMAT_BGR = 3,
		PIXEL_FORMAT_RGBA = 4,
		PIXEL_FORMAT_RGB = 5,
		PIXEL_FORMAT_RGB565 = 6,
		PIXEL_FORMAT_LUM = 7,
		PIXEL_FORMAT_RGBA5551 = 8,
		PIXEL_FORMAT_RGBA4444 = 9,
		PIXEL_FORMAT_PALETTED = 10
	};
	public enum Tracker_Flags {
		NonUniform = 1048576,
		PoseFilter = 2097152,
		CornerFilter =  4194304,
		DumpImages = 8388608,
		Profiler = 16777216,
		HalfImage = 33554432,
		FastMode = 67108864
	}    
}
