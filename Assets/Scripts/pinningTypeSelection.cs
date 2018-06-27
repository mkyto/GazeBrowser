using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinningTypeSelection : MonoBehaviour
{
    bool type1Selected;
    bool type2Selected;
    bool type3Selected;
    Material newMat;
    Material oldMat;
    public string pinMode;
    // Use this for initialization
    void Start()
    {
        type1Selected = false;
        type2Selected = false;
        type3Selected = false;

        pinMode = "world";

        newMat = Resources.Load("green", typeof(Material)) as Material;
        oldMat = Resources.Load("darkGrey", typeof(Material)) as Material;
    }

    // Update is called once per frame
    void Update()
    {



    }


    void selectPinningTypeWorld(float raycastInsideObject)
    {

        if (raycastInsideObject > 1.0f && type1Selected == false)
        {

            GameObject.Find("pinningHandler").SendMessage("setPinningMode","world");
            pinMode = "world";

            GameObject.Find("pinningTypeWorld").GetComponent<Renderer>().material = newMat;
            GameObject.Find("pinningTypeBody").GetComponent<Renderer>().material = oldMat;
            GameObject.Find("pinningTypeScreen").GetComponent<Renderer>().material = oldMat;
            type1Selected = true;
            type2Selected = false;
            type3Selected = false;
        }


    }

    void selectPinningTypeBody(float raycastInsideObject)
    {

        if (raycastInsideObject > 1.0f && type2Selected == false)
        {


            pinMode = "body";
            GameObject.Find("pinningHandler").SendMessage("setPinningMode", "body");
            GameObject.Find("pinningTypeWorld").GetComponent<Renderer>().material = oldMat;
            GameObject.Find("pinningTypeBody").GetComponent<Renderer>().material = newMat;
            GameObject.Find("pinningTypeScreen").GetComponent<Renderer>().material = oldMat;
            type1Selected = false;
            type2Selected = true;
            type3Selected = false;
        }


    }

    void selectPinningTypeScreen(float raycastInsideObject)
    {

        if (raycastInsideObject > 1.0f && type3Selected == false)
        {
            pinMode = "screen";
            GameObject.Find("pinningHandler").SendMessage("setPinningMode", "screen");
            GameObject.Find("pinningTypeWorld").GetComponent<Renderer>().material = oldMat;
            GameObject.Find("pinningTypeBody").GetComponent<Renderer>().material = oldMat;
            GameObject.Find("pinningTypeScreen").GetComponent<Renderer>().material = newMat;

            type1Selected = false;
            type2Selected = false;
            type3Selected = true;
        }


    }


}

