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
 * Plugins.cs
 *
 * Usage: Add this script to the main camera in loading scene and all others
 * 
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;

public class Plugins : MonoBehaviour {

    public string[] PluginNames = {"VideoWrapper","TrackerWrapper"};

	// Use this for initialization
	void Awake () {
		AddToPATH("", true);
        for (int i=0; i<PluginNames.Length; i++) 
			AddToPATH(PluginNames[i], true);	
	}
		
	void AddToPATH(String subpath, bool useProjectDir)
	{	

		//.NET 1.1 has no string.Contains method or System.Environment class
		#if !UNITY_IPHONE

		string cwd = ApplicationDataPath();
		Debug.Log("[CWD]"+cwd);
		string dataDirName = "Plugins";
		String dataDirectory = Path.Combine(cwd,dataDirName);
			
		string separator;
		string environVar;
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
		{
			environVar = "PATH";
			separator = ";";
		}
		else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
		{
			environVar = "DYLD_LIBRARY_PATH";
			separator = ":";
		}
		else
		{
			Debug.Log("Not using native platform app");
			return;
		}
					
		string oldPath = System.Environment.GetEnvironmentVariable(environVar);
		Debug.Log("[Old Path]"+oldPath);
		String pluginPath;
		if (useProjectDir)
			pluginPath = Path.Combine(dataDirectory,subpath);
		else
			pluginPath = subpath;

		if(oldPath == null || !oldPath.Contains(pluginPath))
		{
			string newPath = oldPath + separator + pluginPath +"/";
			Debug.Log("[New Path]"+newPath);
			System.Environment.SetEnvironmentVariable(environVar,newPath);
			System.Environment.SetEnvironmentVariable("PATH",newPath);
		}else{
			Debug.Log("Path already contains plugin dir");
		}
				
		#endif
	}

	public static string ApplicationDataPath()
	{    
		#if UNITY_IPHONE
		// Application.dataPath returns something like "/var/mobile/Applications/30B51836-D2DD-43AA-BCB4-9D4DADFED6A2/Data"
		// Unity 1.6 dataPath returns something like "/var/mobile/Applications/XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX/myappname.app/Data"
		// Strip the "Data" part and add "Documents" instead
		string path = Application.dataPath.Substring(0,Application.dataPath.Length-5);
		if (path.Substring(path.Length-4) == ".app") //Unity 1.6
                path = path.Substring(0, path.LastIndexOf('/'));
        return path + "/Documents";
		#else
		return Application.dataPath;
		#endif
	}
	
	public static string CreateFileFromAsset(string filename)
	{
		TextAsset textAsset = (TextAsset)UnityEngine.Resources.Load(filename);
		string filepath = ApplicationDataPath() + "/" + filename;
		BinaryWriter binWriter = new BinaryWriter(File.Open(filepath, FileMode.Create));
		binWriter.Write(textAsset.bytes);
		binWriter.Close();
		return filepath;
	}
	
}
