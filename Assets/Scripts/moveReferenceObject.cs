using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveReferenceObject : MonoBehaviour {

    public Vector3 targetOff;
   float distanceToTargets;
    public float accuracyLimit;
    GameObject vuforiaImageTarget;

    // Use this for initialization
    void Start ()
    {
        vuforiaImageTarget = GameObject.Find("ImageTargetChips");
        targetOff = GameObject.Find("sceneCreation").GetComponent<sceneCreationTargetSelection>().targetOffset;
        distanceToTargets = GameObject.Find("sceneCreation").GetComponent<sceneCreationTargetSelection>().targetDistance;

        this.transform.position = new Vector3(targetOff.x, targetOff.y, targetOff.z + distanceToTargets);
        //float x_scale = Mathf.Abs(this.transform.position.z * Mathf.Tan(accuracyLimit / 2 / 180 * Mathf.PI) * 2.0f)/transform.parent.transform.localScale.x;
        //float y_scale = Mathf.Abs(this.transform.position.z * Mathf.Tan(accuracyLimit / 2 / 180 * Mathf.PI) * 2.0f) / transform.parent.transform.localScale.y;
        float x_scale = Mathf.Abs(this.transform.position.z * Mathf.Tan(accuracyLimit / 2 / 180 * Mathf.PI) * 2.0f) / vuforiaImageTarget.transform.localScale.x;
        float y_scale = Mathf.Abs(this.transform.position.z * Mathf.Tan(accuracyLimit / 2 / 180 * Mathf.PI) * 2.0f) / vuforiaImageTarget.transform.localScale.y;
        float z_scale = 0.001f;

        this.transform.localScale = new Vector3(x_scale, y_scale, 0.001f);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
