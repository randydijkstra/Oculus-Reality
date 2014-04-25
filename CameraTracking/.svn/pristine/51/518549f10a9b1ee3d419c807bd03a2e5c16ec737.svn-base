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
 * VideoCamera.cs
 *
 * Usage: Add this script to the main camera
 * 
 *
 * Notes:
 *
 **********************************************************************************/
 
using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;

public class MarshallCommon
{
   public static string PtrToString (IntPtr p)
     {
        // TODO: deal with character set issues.  Will PtrToStringAnsi always
        // "Do The Right Thing"?
        if (p == IntPtr.Zero)
          return null;
        return Marshal.PtrToStringAnsi (p);
     }

   public static string[] PtrToStringArray (IntPtr stringArray)
     {
        if (stringArray == IntPtr.Zero)
          return new string[]{};


        int argc = CountStrings (stringArray);
        return PtrToStringArray (argc, stringArray);
     }

   private static int CountStrings (IntPtr stringArray)
     {
        int count = 0;
        while (Marshal.ReadIntPtr (stringArray, count*IntPtr.Size) != IntPtr.Zero)
          ++count;
        return count;
     }


   public static string[] PtrToStringArray (int count, IntPtr stringArray)
     {
        if (count < 0)
          throw new ArgumentOutOfRangeException ("count", "< 0");
        if (stringArray == IntPtr.Zero)
          return new string[count];


        string[] members = new string[count];
        for (int i = 0; i < count; ++i) {
           IntPtr s = Marshal.ReadIntPtr (stringArray, i * IntPtr.Size);
           members[i] = PtrToString (s);
        }


        return members;
     }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
     
public class VideoCamera : MonoBehaviour {	
	public string m_configString;

    public Color[] m_vidframe = new Color[0];
	public Byte[] m_vidframe_byte = new Byte[0];
	public IntPtr m_vidpointer_byte = IntPtr.Zero;
	
	private int m_cameraHandle = -1;
	public int m_width = 0;
	public int m_height = 0;
	public int m_depth = 0;
	public int m_pixelFormat = 0;
	private GCHandle m_vidframeHandle;
	private GCHandle m_vidframeHandle_byte;
	
	#if !UNITY_IPHONE	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void VWPrintFcn(IntPtr msg);
	public static void DebugPrint(IntPtr msg) {
		
		Debug.Log("VW Callback::" + MarshallCommon.PtrToString(msg));
	}
    [DllImport("VideoWrapper/VideoWrapper")]
    private static extern int VIDEO_setPrintFunction(VWPrintFcn func); 
	#endif
		
	public static void Initialize()
	{
		Debug.Log("Video Wrapper Initialize\n");
		
		#if !UNITY_IPHONE	
		VWPrintFcn myCallBack = new VWPrintFcn(VideoCamera.DebugPrint);
		if(VIDEO_setPrintFunction(myCallBack) != 0)
		{
			Debug.Log("Unable To Setup Print Function");
		}
		#endif	
	}	
			
	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
	[DllImport ("VideoWrapper/VideoWrapper")]
	#endif
	private static extern int VIDEO_getAvailablePropertyStrings(ref int propertyStringsCount, ref IntPtr propertyStrings);
    public static string[] getAvailablePropertyStrings ()
    {
    	//get the property strings
		int count = 0;
		IntPtr strings = IntPtr.Zero;
		
		if(VIDEO_getAvailablePropertyStrings(ref count,ref strings)==0)
		{
			string[] propStrings = MarshallCommon.PtrToStringArray(count,strings);
    	
	    	Debug.Log("listPropertyStrings BEGIN");
	    	for(int i=0;i<count;i++)
	    	{
	    		Debug.Log(propStrings[i]);
	    	}
	    	Debug.Log("listPropertyStrings END");
	    	return propStrings;
		}else{
			return new string[0];
		}
		
    }
    
	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("VideoWrapper/VideoWrapper")]
	#endif
    private static extern int VIDEO_openVideo([In]string szInit, ref int phVideo);

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("VideoWrapper/VideoWrapper")]
	#endif
    private static extern int VIDEO_startVideo([In]int hVideo);
   
	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("VideoWrapper/VideoWrapper")]
	#endif
    private static extern int VIDEO_getWidth([In]int hVideo, ref int pWidth);
    
	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("VideoWrapper/VideoWrapper")]
	#endif
    private static extern int VIDEO_getHeight([In]int hVideo, ref int pHeight);

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("VideoWrapper/VideoWrapper")]
	#endif
    private static extern int VIDEO_getDepth([In]int hVideo, ref int pDepth);

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("VideoWrapper/VideoWrapper")]
	#endif
    private static extern int VIDEO_getPixelFormat([In]int hVideo, ref int pFormat);
	
    void Awake () 
    {
    	Initialize();
    	Application.runInBackground = true;
    	
		if(m_configString.Equals(""))
		{
			m_configString = VideoWrapperConfigGUI.DropDownEntry;
    		VideoWrapperConfigGUI.WindowVisible = false;
		}
		
		if(m_configString.Equals(""))
		{			
			m_configString = "qt: 0 320 30 rgb 0";
        }
        
		//open the camera
		Debug.Log("Using Config String \"" + m_configString + "\"");
		if(VIDEO_openVideo(m_configString,ref m_cameraHandle)!=0)
		{
			Debug.Log("VIDEO_openVideo failed");
		}
		
        //start the camera
		if(VIDEO_startVideo(m_cameraHandle)!=0)
		{
			Debug.Log("VIDEO_startVideo failed");
		}
		
		VIDEO_getWidth(m_cameraHandle, ref m_width);
		VIDEO_getHeight(m_cameraHandle, ref m_height);
		VIDEO_getDepth(m_cameraHandle, ref m_depth);
		VIDEO_getPixelFormat(m_cameraHandle, ref m_pixelFormat);
		
		Debug.Log("Video Depth = " + m_depth + " Format = " + m_pixelFormat);
	
		m_vidframe_byte = new Byte[m_width * m_height * (int)(m_depth/8.0f)];	
		m_vidframeHandle_byte = GCHandle.Alloc(m_vidframe_byte, GCHandleType.Pinned);
		m_vidpointer_byte = m_vidframeHandle_byte.AddrOfPinnedObject();
		
        m_vidframe = new Color[m_width * m_height]; //this buffer will contain the frame of video       	
		m_vidframeHandle = GCHandle.Alloc(m_vidframe, GCHandleType.Pinned);
    }
    
	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("VideoWrapper/VideoWrapper")]
	#endif
    private static extern int VIDEO_stopVideo([In]int hVideo);

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("VideoWrapper/VideoWrapper")]
	#endif
    private static extern int VIDEO_close([In]int hVideo);
        
    void OnDisable()
    {
        //stop the camera
		if(VIDEO_stopVideo(m_cameraHandle)!=0)
		{
			Debug.Log("VIDEO_stopVideo failed");
		}
		
        //close the camera
		if(VIDEO_close(m_cameraHandle)!=0)
		{
			Debug.Log("VIDEO_close failed");
		}
   		Debug.Log("close");
    }
    
	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("VideoWrapper/VideoWrapper")]
	#endif
    private static extern int VIDEO_getFrame([In]int hVideo, ref IntPtr img, IntPtr ts);
    
	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("VideoWrapper/VideoWrapper")]
	#endif
    private static extern int VIDEO_releaseFrame([In]int  hVideo);    
    
    #if !UNITY_IPHONE    
	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("VideoWrapper/VideoWrapper")]
	#endif
    private static extern void HELPER_ByteXYZAToFloatZYXA(IntPtr src, IntPtr dst, int width, int height);

	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("VideoWrapper/VideoWrapper")]
	#endif
    private static extern void HELPER_ByteXYZToFloatXYZA(IntPtr src, IntPtr dst, int width, int height);
	
	#if UNITY_IPHONE
	[DllImport ("__Internal")]
	#else
    [DllImport ("VideoWrapper/VideoWrapper")]
	#endif
    private static extern void HELPER_TestPaternFloatXYZA(IntPtr dst, int width, int height,float time);
    #endif
	
	void Update () {
    	//Lock the handle
		IntPtr address = m_vidframeHandle.AddrOfPinnedObject();
		
		if( address == IntPtr.Zero)
			return;
		
		IntPtr img = IntPtr.Zero;
  
  		try{
			if( VIDEO_getFrame(m_cameraHandle, ref img, IntPtr.Zero) != 0) 
				return;
			if( img == IntPtr.Zero)
				return;
			Marshal.Copy(img, m_vidframe_byte,0,m_vidframe_byte.Length);
			
			#if !UNITY_IPHONE
			if (m_depth == 32 && m_pixelFormat == 32993){
				HELPER_ByteXYZAToFloatZYXA(img,address,m_width,m_height);
			}else if(m_depth == 24 && m_pixelFormat == 6407){
				HELPER_ByteXYZToFloatXYZA(img,address,m_width,m_height);
			}else{
				HELPER_TestPaternFloatXYZA(address,m_width,m_height,Time.time);
			}			
			#endif
			
  		}catch (Exception ex) 
    	{
     		Console.WriteLine("Exception: " + ex.Message);
     		Console.Write(ex.StackTrace);
  		}
  		
		VIDEO_releaseFrame(m_cameraHandle);
    }
	
	#if UNITY_IPHONE
	[DllImport ("__Internal")]
    private static extern int VIDEO_updateTextureByID(IntPtr img, int id);
    #endif

	public void SetPixelsByID(int id)
	{
		#if UNITY_IPHONE
		VIDEO_updateTextureByID(m_vidpointer_byte,id);
		#endif
	}
}
