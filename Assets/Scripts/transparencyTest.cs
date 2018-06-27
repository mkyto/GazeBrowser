using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparencyTest : MonoBehaviour {
    Color color;
    // Use this for initialization
    void Start () {
        color = this.transform.GetComponent<Renderer>().material.color;
        color.a = 0.0f;
        this.transform.GetComponent<Renderer>().material.color = color;
    }
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetKey(KeyCode.O))
        {
            color.a = color.a + 0.1f;
        }
        if (Input.GetKey(KeyCode.P))
        {
            color.a = color.a - 0.1f;
        }

        this.transform.GetComponent<Renderer>().material.color = color;*/
    }
}
