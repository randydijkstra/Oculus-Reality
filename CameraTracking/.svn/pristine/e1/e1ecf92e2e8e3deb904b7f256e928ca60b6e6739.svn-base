
function Update()
{
	var markers = FindObjectsOfType(TrackerMarker) as TrackerMarker[];
	for (var marker : TrackerMarker in markers)
		if (marker.visible == false && marker.Behavior == TrackerMarker.Behavior_Flags.CameraRelative)
			marker.transform.position = Vector3(0,1000,0); 
}