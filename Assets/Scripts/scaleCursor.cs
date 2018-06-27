using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleCursor : MonoBehaviour {

    public float cursorSize; ////In degrees, the diameter of the largest circle
    Vector3 currentPosition;


    // Use this for initialization

    void Start()
    {
        //accLimit = GameObject.Find("Canvas").GetComponent<validationScript>().accuracyLimit;
         


    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = Camera.main.transform.position;
        float distance = Vector3.Magnitude(this.transform.position - currentPosition);

        //Debug.Log("Distance:" + distance);
        this.gameObject.transform.localScale = new Vector3(2 * Mathf.Abs(distance * Mathf.Tan(cursorSize / 2 / 180 * Mathf.PI)), 2 * Mathf.Abs(distance * Mathf.Tan(cursorSize / 2 / 180 * Mathf.PI)), 0.00001f);

        //this.gameObject.GetComponent<SphereCollider>().radius = Mathf.Abs(distance * Mathf.Tan(accLimit / 2 / 180 * Mathf.PI));
        // this.transform.localScale = distance * Mathf.Tan(accuracyLimit / 2 * 180 / Mathf.PI);
    }
}
