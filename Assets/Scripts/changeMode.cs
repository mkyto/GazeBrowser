using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeMode : MonoBehaviour {
    bool type1Selected;
    bool type2Selected;
    bool type3Selected;
    Material newMat;
    Material oldMat;
    float dwellTime;

    GameObject[] things;
    // Use this for initialization
    void Start () {
        dwellTime =0.2f;
        type1Selected = false;
      type2Selected = false;
       type3Selected = false;

       newMat  = Resources.Load("green", typeof(Material)) as Material;
        oldMat = Resources.Load("darkGrey", typeof(Material)) as Material;


    }
	
	// Update is called once per frame
	void Update () {


		
	}


    void selectVisualisationType1(float raycastInsideObject)
    {

        if(raycastInsideObject > dwellTime && type1Selected == false) { 
        GameObject.Find("sceneCreation").SendMessage("selectType", 1);

            things = GameObject.FindGameObjectsWithTag("Thing");

            for (int i =0; i < things.Length; i++)
            {

                things[i].SendMessage("removeFreezedText");
            
            }

            
            GameObject.Find("Type1").GetComponent<Renderer>().material = newMat;
            GameObject.Find("Type2").GetComponent<Renderer>().material = oldMat;
            GameObject.Find("Type3").GetComponent<Renderer>().material = oldMat;
            type1Selected = true;
            type2Selected = false;
            type3Selected = false;
        }


    }

    void selectVisualisationType2(float raycastInsideObject)
    {

        if (raycastInsideObject > dwellTime && type2Selected == false)
        {
            things = GameObject.FindGameObjectsWithTag("Thing");

            for (int i=0; i < things.Length; i++)
            {

                things[i].SendMessage("removeFreezedText");

            }
            GameObject.Find("sceneCreation").SendMessage("selectType", 2);
            GameObject.Find("Type1").GetComponent<Renderer>().material = oldMat;
            GameObject.Find("Type2").GetComponent<Renderer>().material = newMat;
            GameObject.Find("Type3").GetComponent<Renderer>().material = oldMat;


            Color color = GameObject.Find("Type1").GetComponent<Renderer>().material.color;
            color.a = 0.5f;
            GameObject.Find("Type1").GetComponent<Renderer>().material.color = color;


            type1Selected = false;
            type2Selected = true;
            type3Selected = false;
        }


    }

    void selectVisualisationType3(float raycastInsideObject)
    {

        if (raycastInsideObject > dwellTime && type3Selected == false)
        {
            things = GameObject.FindGameObjectsWithTag("Thing");

            for (int i=0; i < things.Length; i++)
            {

                things[i].SendMessage("removeFreezedText");

            }
            GameObject.Find("sceneCreation").SendMessage("selectType", 3);
            GameObject.Find("Type1").GetComponent<Renderer>().material = oldMat;
            GameObject.Find("Type2").GetComponent<Renderer>().material = oldMat;
            GameObject.Find("Type3").GetComponent<Renderer>().material = newMat;

            type1Selected = false;
            type2Selected = false;
            type3Selected = true;
        }


    }


}
