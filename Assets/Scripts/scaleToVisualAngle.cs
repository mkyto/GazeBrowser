using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleToVisualAngle : MonoBehaviour {

    public float accLimit; //This should originate from validation
    Vector3 currentPosition;

    
    // Use this for initialization

	void Start () {
        accLimit = GameObject.Find("Canvas").GetComponent<validationScript>().accuracyLimit;
       

	}
	
	// Update is called once per frame
	void Update () {
        currentPosition = Camera.main.transform.position;
        float distance = Vector3.Magnitude(this.transform.position - currentPosition);
        this.gameObject.GetComponent<SphereCollider>().radius = Mathf.Abs(distance * Mathf.Tan(accLimit / 2 / 180 * Mathf.PI));
       // this.transform.localScale = distance * Mathf.Tan(accuracyLimit / 2 * 180 / Mathf.PI);
    }
}




