using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTarget : MonoBehaviour {

    GameObject sceneCreation;

    // Use this for initialization
    void Start()
    {

        //sceneCreation = GameObject.Find("sceneCreation");
        //this.transform.position = sceneCreation.GetComponent<sceneCreationTargetSelection>().currentPosition;
    }
	// Update is called once per frame
	void Update () {
		
	}

    void setPosition(Vector3 position) {
        this.transform.position = position;
    }

}
