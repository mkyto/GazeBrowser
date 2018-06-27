using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleRadial : MonoBehaviour
{

    GameObject background;
    // Use this for initialization
    void Start()
    {
        //background = GameObject.Find("Background");
        if(this.transform.FindChild("Background") != null) { 
        background = this.transform.FindChild("Background").gameObject;
        }
        foreach (Transform trans in this.transform)
            {
            trans.GetComponent<Collider>().enabled = false;
            trans.GetComponent<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //if eye gaze hits, display the objects on level 1 

        //copy from target selection

        // display cursor when clicker down (condition 4)


    }
   
        public void showChildren()
        {
        foreach (Transform trans in this.transform)
        {
            trans.GetComponent<Collider>().enabled = true;
            trans.GetComponent<Renderer>().enabled = true;
        }
        if(background != null)
        {
            scaleBackgroundSize();
        }
       

    }

        public void hideChildren()
    {
            foreach (Transform trans in this.transform)
            {
                trans.GetComponent<Collider>().enabled = false;
                trans.GetComponent<Renderer>().enabled = false;
            }
    }

    void scaleBackgroundSize()
    {
       // background.transform.sc* 0.75f;

    }

}


