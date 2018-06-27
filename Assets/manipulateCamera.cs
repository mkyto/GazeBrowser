using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manipulateCamera : MonoBehaviour
{

    float speed = 20.0f;
    float speedTranslate = 1.0f; 
    // Use this for initialization
    void Start()
    {
        Debug.Log("Kaynnistyi");
        //UICam = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A)) { 
            Camera.main.transform.Rotate(Vector3.up * speed * Time.deltaTime);
            //UICam.gameObject.transform.Rotate(Vector3.up * speed * Time.deltaTime);
            //Debug.Log("A painettiin");

        }
        if (Input.GetKey(KeyCode.D)) { 
            Camera.main.transform.Rotate(-Vector3.up * speed * Time.deltaTime);
            //UICam.transform.Rotate(-Vector3.up * speed * Time.deltaTime);
            //Debug.Log("D painettiin");
        }

        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.Rotate(-Vector3.right * speed * Time.deltaTime);
           // UICam.transform.Rotate(-Vector3.right * speed * Time.deltaTime);
            //Debug.Log("W painettiin");
        }

        if (Input.GetKey(KeyCode.X))
        {
            Camera.main.transform.Rotate(Vector3.right * speed * Time.deltaTime);
            //UICam.transform.Rotate(Vector3.right * speed * Time.deltaTime);
            //Debug.Log("S painettiin");
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.transform.Rotate(Vector3.forward * speed * Time.deltaTime);
            //UICam.transform.Rotate(Vector3.right * speed * Time.deltaTime);
            //Debug.Log("S painettiin");
        }

        if (Input.GetKey(KeyCode.E))
        {
            Camera.main.transform.Rotate(-Vector3.forward * speed * Time.deltaTime);
            //UICam.transform.Rotate(Vector3.right * speed * Time.deltaTime);
            //Debug.Log("S painettiin");
        }


        if (Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.transform.Translate(Vector3.up * speedTranslate * Time.deltaTime);
            //UICam.gameObject.transform.Rotate(Vector3.up * speed * Time.deltaTime);
            //Debug.Log("A painettiin");

        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.transform.Translate(-Vector3.up * speedTranslate * Time.deltaTime);
            //UICam.transform.Rotate(-Vector3.up * speed * Time.deltaTime);
            //Debug.Log("D painettiin");
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.Translate(-Vector3.right * speedTranslate * Time.deltaTime);
            // UICam.transform.Rotate(-Vector3.right * speed * Time.deltaTime);
            //Debug.Log("W painettiin");
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.Translate(Vector3.right * speedTranslate * Time.deltaTime);
            //UICam.transform.Rotate(Vector3.right * speed * Time.deltaTime);
            //Debug.Log("S painettiin");
        }

        if (Input.GetKey(KeyCode.Z))
        {
            Camera.main.transform.Translate(Vector3.forward * speedTranslate * Time.deltaTime);
            //UICam.transform.Rotate(Vector3.right * speed * Time.deltaTime);
            //Debug.Log("S painettiin");
        }

        //if (Input.GetKey(KeyCode.C))
       // {
         //   Camera.main.transform.Translate(-Vector3.forward * speedTranslate * Time.deltaTime);
            //UICam.transform.Rotate(Vector3.right * speed * Time.deltaTime);
            //Debug.Log("S painettiin");
        //}


    }
}
