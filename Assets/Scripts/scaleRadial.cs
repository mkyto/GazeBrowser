using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleRadial : MonoBehaviour {

    public float accLimit = 3.0f; //This should come from validation
    Vector3 currentPosition;
    GameObject referenceObject;
    // Use this for initialization
    void Start () {


       // accLimit = GameObject.Find("Canvas").GetComponent<validationScript>().accuracyLimit;
        
    }
	
	// Update is called once per frame
	void Update () {
        currentPosition = Camera.main.transform.position;
        float distance = Vector3.Magnitude(this.transform.position - currentPosition);
        float scaleFactor = Mathf.Abs(distance * Mathf.Tan(accLimit / 2 * 180 / Mathf.PI))*0.21f;

        float temp = this.transform.localScale.x;
        this.transform.localScale = new Vector3(temp, scaleFactor, scaleFactor);
        //this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + this.transform.localScale.y / 2, this.transform.position.z);

       // this.transform.Translate(0.0f, this.transform.localScale.y / 2, 0.0f);
    }
}
