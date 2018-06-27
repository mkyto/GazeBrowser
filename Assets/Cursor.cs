using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    Vector3 headPosition;
    Vector3 gazeDirection;
    GameObject cursor;
    Vector3 cursorOrigPosition;
    GameObject[] radialObjects;
    // Use this for initialization
    void Start()
    {
        radialObjects = GameObject.FindGameObjectsWithTag("hasRadialChildren");
        cursor = GameObject.Find("Cursor");
        cursorOrigPosition = cursor.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        headPosition = Camera.main.transform.position;
        gazeDirection = Camera.main.transform.forward;

        


        RaycastHit hit;

        if (Physics.Raycast(headPosition, gazeDirection, out hit))
        {
             
            cursor.transform.position = hit.point;
            transform.LookAt(cursor.transform.position + Camera.main.transform.rotation * Vector3.forward,
                   Vector3.up);


            if(hit.collider.tag == "hasRadialChildren")
            {
                hit.collider.SendMessage("showChildren");

            }
        }
        else
        {
            cursor.transform.localPosition = cursorOrigPosition;     
            foreach (GameObject radialObject in radialObjects) { 
            radialObject.SendMessage("hideChildren");
            }
        }

        
    }
}
