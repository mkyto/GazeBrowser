using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatePendicular2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        Transform child;

       // if(Physics.Raycast(headPosition, gazeDirection,out hitInfo, Mathf.Infinity))
       // {

            // place at hit.position
            //transform.position = hitInfo.point;

            // look in the direction of the hit object
            //+ hitInfo.transform.forward Camera.main.transform.forward
            //transform.LookAt(hitInfo.transform.position + Camera.main.transform.forward, hitInfo.normal);
            //foreach (Transform trans in this.transform) {

            //child = this.gameObject.transform.GetChild(0);
            //child.transform.LookAt(Camera.main.transform);


        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
          Vector3.up);

        //}
        //this.gameObject.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);

        //transform.LookAt(Camera.main.transform);
        // transform.localEulerAngles = new Vector3(0, 0, 0);
        //   Debug.Log("Osui");
        // }


    }
}
