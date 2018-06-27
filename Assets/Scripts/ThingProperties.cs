using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingProperties : MonoBehaviour {
    public int ID;
    public int type;
	// Use this for initialization
	void Start () {
        //ID = 1;

        

    }

    // Update is called once per frame
    void Update () {
        
        //transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
       //   Vector3.up);
       
        
        
        //Vector3 targetPosition = new Vector3(transform.position.x + Camera.main.transform.position.x,
        //                              transform.position.y + Camera.main.transform.position.y,
        //                              transform.position.z + Camera.main.transform.position.z);
        //this.transform.LookAt(targetPosition, Camera.main.transform.rotation * Vector3.up);

        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
           Vector3.up);

        transform.Rotate(new Vector3(-90.0f, 0.0f, 90.0f));

    }
}
