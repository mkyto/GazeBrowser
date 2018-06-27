using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveToStartingPosition : MonoBehaviour {
    public Vector3 targetOff;
    float distanceToTargets;
    // Use this for initialization
    void Start () {
        targetOff = GameObject.Find("sceneCreation").GetComponent<sceneCreationTargetSelection>().targetOffset;
        distanceToTargets = GameObject.Find("sceneCreation").GetComponent<sceneCreationTargetSelection>().targetDistance;
        this.transform.position = new Vector3(targetOff.x, targetOff.y, targetOff.z + distanceToTargets);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
