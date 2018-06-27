using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolProperties : MonoBehaviour {
    public int ID;
    public int type;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
         Vector3.up);
        transform.Rotate(new Vector3(-90.0f, 0.0f, 90.0f));
    }
}
