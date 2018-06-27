using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PupilCalibMarker : MonoBehaviour {

#if UNITY_EDITOR

    RectTransform _transform;
	Image _image;
	bool _started=false;
	float x,y;

	// Use this for initialization
	void Start () {

		_transform = GetComponent<RectTransform> ();
		_image = GetComponent<Image> ();
		_image.enabled = false;

		PupilGazeTracker.Instance.OnCalibrationStarted += OnCalibrationStarted;
		PupilGazeTracker.Instance.OnCalibrationDone += OnCalibrationDone;
		PupilGazeTracker.Instance.OnCalibData += OnCalibData;
	}

	void OnCalibrationStarted(PupilGazeTracker m)
	{
		_started = true;
	}

	void OnCalibrationDone(PupilGazeTracker m)
	{
		_started = false;
	}

	void OnCalibData(PupilGazeTracker m,float x,float y)
	{
		this.x = x;
		this.y = y;
	}

	void _SetLocation(float x,float y)
	{
		Canvas c = _transform.GetComponentInParent<Canvas> ();
		if (c == null)
			return;

        GameObject go = GameObject.Find("EyeCanvas");
        float w = go.GetComponent<RectTransform>().rect.width;
        float h = go.GetComponent<RectTransform>().rect.height;
        //Vector3 pos=new Vector3 ((x-0.5f)*c.pixelRect.width,(y-0.5f)*c.pixelRect.height,0);
        Vector3 pos = new Vector3((x - 0.5f) * w, (y - 0.5f) * h, 0);
        _transform.localPosition = pos;
        //print("x = " + x + ", y = " + y);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space))
			PupilGazeTracker.Instance.StartCalibration ();
		if (Input.GetKeyDown (KeyCode.S))
			PupilGazeTracker.Instance.StopCalibration ();/**/
		_image.enabled = _started;
		if(_started)
			_SetLocation (x, y);
	}

#endif

}
