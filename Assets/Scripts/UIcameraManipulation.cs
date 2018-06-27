using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcameraManipulation : MonoBehaviour
{

    float speed = 10.0f;
    // Use this for initialization
    void Start()
    {
        Debug.Log("Kaynnistyi2");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.H))
        {
           this.transform.Rotate(Vector3.up * speed * Time.deltaTime);
            //Debug.Log("A painettiin");

        }
        if (Input.GetKey(KeyCode.K))
        {
            this.transform.Rotate(-Vector3.up * speed * Time.deltaTime);
            //Debug.Log("D painettiin");
        }

        if (Input.GetKey(KeyCode.U))
        {
            this.transform.Rotate(-Vector3.right * speed * Time.deltaTime);
            //Debug.Log("W painettiin");
        }

        if (Input.GetKey(KeyCode.J))
        {
            this.transform.Rotate(Vector3.right * speed * Time.deltaTime);
            //Debug.Log("S painettiin");
        }


    }
}


