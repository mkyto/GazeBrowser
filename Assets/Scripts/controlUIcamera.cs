using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlUIcamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        this.transform.localRotation = Quaternion.identity;
    }
}
