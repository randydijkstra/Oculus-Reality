var SceneNames : String[] = [ "mylevel" ];

function OnGUI ()
{
	GUILayout.BeginArea(Rect(0, Screen.height - 30, Screen.width, 30));
	GUILayout.BeginHorizontal();
	
	var level_num = 0;
	for (var level in SceneNames)
	{
		level_num++;
		if (GUILayout.Button(level))
			Application.LoadLevel(level_num);
	}
	GUILayout.FlexibleSpace();
	GUILayout.EndHorizontal();
	GUILayout.EndArea();
}