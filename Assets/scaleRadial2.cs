using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleRadial2 : MonoBehaviour {

    public float accLimit; //This should come from validation
    Vector3 currentPosition;
    GameObject referenceObject;

    float accLimit2;
    float heuristicScaler;
    // Use this for initialization
    void Start () {

        heuristicScaler = 25.0f;
       accLimit2 = 4.0f;

        
        
    }
	
	// Update is called once per frame
	void Update () {
        currentPosition = Camera.main.transform.position;
        float distance = Vector3.Magnitude(this.transform.position - currentPosition);
        float scaleFactor = Mathf.Abs(distance * Mathf.Tan(accLimit2 / 2 / 180 * Mathf.PI))*heuristicScaler;

        float temp = this.transform.localScale.x;
        this.transform.localScale = new Vector3(temp, scaleFactor, scaleFactor);
        //this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + this.transform.localScale.y / 2, this.transform.position.z);

       // this.transform.Translate(0.0f, this.transform.localScale.y / 2, 0.0f);
    }
}
