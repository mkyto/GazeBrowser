using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinningManagement : MonoBehaviour {

    public string pinMode;
    Vector3 headPosition;
    Vector3 gazeDirection;
    Vector3 newPosition;
    Vector3 offSetFromCenter;
    int numberOfPinnedScreens;
    int currentThingID;
    string previousPinMode;
   Vector3[] positionsForPinnedScreens;
    Vector3[] positionsForPinnedBodys;
    List<int> screenThings;
    List<int> bodyThings;
    Vector3 upDirection;
    Vector3 rightDirection;
    Vector3 initialBodyDirection;
    Vector3 initialBodyPosition;
    Camera UIcam;
    GameObject mainCamera;

    // Use this for initialization
    void Start () {
        previousPinMode = "";
        pinMode = "world";
        currentThingID = -1;
        offSetFromCenter = new Vector3(0.2f, 0.2f, 0.0f);
        numberOfPinnedScreens = 0;
        screenThings = new List<int>();
        bodyThings = new List<int>();
        positionsForPinnedScreens = new Vector3[4];
        positionsForPinnedScreens[0] = new Vector3(-0.12f, -0.135f, 0.0f);
        positionsForPinnedScreens[1] = new Vector3(0.16f, -0.135f, 0.0f);
        positionsForPinnedScreens[2] = new Vector3(0.1f, 0.1f, 0.0f);
        positionsForPinnedScreens[3] = new Vector3(0.3f, 0.1f, 0.0f);

        mainCamera = GameObject.Find("HoloLensCamera");

        positionsForPinnedBodys = new Vector3[4];
        positionsForPinnedBodys[0] = new Vector3(1.2f, 0.1f, 0.0f);
        positionsForPinnedBodys[1] = new Vector3(1.5f, 0.1f, 0.0f);
        positionsForPinnedBodys[2] = new Vector3(1.2f, -0.1f, 0.0f);
        positionsForPinnedBodys[3] = new Vector3(1.5f, -0.1f, 0.0f);

        //UIcam = GameObject.Find("UICamera").GetComponent<Camera>();

        //initialBodyDirection = UIcam.transform.forward = Camera.main.transform.forward;
        //initialBodyPosition = UIcam.transform.position = Camera.main.transform.position;
        

        //initialBodyDirection = Camera.main.transform.forward;
        //initialBodyPosition = Camera.main.transform.position;


        initialBodyDirection = mainCamera.transform.forward;
        initialBodyPosition = mainCamera.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //headPosition = UIcam.transform.position = Camera.main.transform.position;
        //gazeDirection = UIcam.transform.forward = Camera.main.transform.forward;
        //rightDirection = UIcam.transform.right = Camera.main.transform.right;
        //upDirection = UIcam.transform.up =  Camera.main.transform.up;
        //UIcam.fieldOfView = Camera.main.fieldOfView;

        //headPosition = Camera.main.transform.position;
        //gazeDirection = Camera.main.transform.forward;
        //rightDirection = Camera.main.transform.right;
        //upDirection = Camera.main.transform.up;


        headPosition = mainCamera.transform.position;
        gazeDirection = mainCamera.transform.forward;
        rightDirection = mainCamera.transform.right;
        upDirection = mainCamera.transform.up;

        //UIcam.fieldOfView = Camera.main.fieldOfView;


        //positionsForPinnedScreens[0] = headPosition + gazeDirection - 0.2f * rightDirection + upDirection * 0.2f;

    }


    public void setPinningMode(string mode)
    {
        if(pinMode != mode)
        {
            previousPinMode = pinMode;
            pinMode = mode;
            Debug.Log("Current pin mode: " + pinMode);
            resetPinnedThings();

        }
        else
        {
            previousPinMode = pinMode;
            Debug.Log("Previous pin mode: " + pinMode);
        }


    }



    void resetPinnedThings()
    {
        GameObject[] pinnedThings = GameObject.FindGameObjectsWithTag("Thing");

        for (int i = 0; i < pinnedThings.Length;i++)
        {
            //pinnedThings[i].GetComponentInChildren<Renderer>().enabled = false;
            pinnedThings[i].SendMessage("removeFreezedText");
            pinnedThings[i].SendMessage("setWorldPosition");
            //Debug.Log("yritettiin poistaa" + pinnedThings[i].name);

            //if(pinnedThings[i].GetComponentInChildren<BoxCollider>() != null) { 
            //pinnedThings[i].GetComponentInChildren<BoxCollider>().enabled = false;
            //}
        }          
    }


    public void setAsCameraChild(int thingID)
    {
        int a = 0;
        GameObject[] things = GameObject.FindGameObjectsWithTag("Thing");
        GameObject UIcam = GameObject.Find("UICamera");

        if (screenThings.Count < 3)
        {
            //foreach (int id in screenThings)
            // {
            for (int i = 0; i < things.Length; i++)
            {
                if (things[i].transform.parent.GetComponent<ThingProperties>() != null)
                {
                    if (things[i].transform.parent.GetComponent<ThingProperties>().ID == thingID)
                    {
                        //things[i].transform.parent = UIcam.transform;
                        things[i].transform.parent = Camera.main.transform;
                        //things[i].transform.position = new Vector3(positionsForPinnedScreens[a].x, positionsForPinnedScreens[a].y, 1.0f);
                        things[i].transform.position = headPosition + gazeDirection + positionsForPinnedScreens[screenThings.Count].x * rightDirection + positionsForPinnedScreens[screenThings.Count].y * upDirection;
                        Debug.Log("Asetettiin vanhempi");
                    }
                }
            }
            if (!screenThings.Contains(thingID))
            {
                screenThings.Add(thingID);
            }
        }
        else
        {
            screenThings.Clear();
        }
    }

         
    /*public Vector3 setUIPosition(Vector3 oldPosition, int thingID)
    {
        int a = 0;
        if (pinMode == "screen")
        {
            if(screenThings.Count < 5) {

                foreach (int id in screenThings) {

                    if (thingID == id)
                    {
                        newPosition = headPosition + gazeDirection + positionsForPinnedScreens[a].x * rightDirection + positionsForPinnedScreens[a].y * upDirection;
                        //newPosition = headPosition + gazeDirection + positionsForPinnedScreens[a];
                    }
                    a++;
                }

                if (!screenThings.Contains(thingID))
                {
                        screenThings.Add(thingID);
                    
                }

                }
    
            else
            {
                a = 0;
                newPosition = headPosition + gazeDirection + positionsForPinnedScreens[a].x * rightDirection + positionsForPinnedScreens[a].y * upDirection;
                
            }

            
            return newPosition;
        }
        else if(pinMode == "body")
        {
            int b = 0;
            if (bodyThings.Count < 5)
            {

                foreach (int id in bodyThings)
                {

                    if (thingID == id)
                    {
                        //newPosition = initialBodyPosition + initialBodyDirection + positionsForPinnedBodys[b].x * rightDirection + positionsForPinnedBodys[b].y * upDirection;
                        newPosition = initialBodyPosition + initialBodyDirection + positionsForPinnedBodys[b].x * new Vector3(1.0f,0.0f,0.0f) + positionsForPinnedBodys[b].y * new Vector3(0.0f, 1.0f, 0.0f);


                        //CHANGE: rightDirection and upDirection, now from camera
                    }
                    b++;
                }

                if (!bodyThings.Contains(thingID))
                {
                    bodyThings.Add(thingID);

                }

            }

            else
            {
                b = 0;
                newPosition = initialBodyPosition + initialBodyDirection + positionsForPinnedBodys[b].x * rightDirection + positionsForPinnedBodys[b].y * upDirection;
                //newPosition = headPosition + gazeDirection + offSetFromCenter + positionsForPinnedScreens[0];
                
            }
            return newPosition;
        }
        
            
            //if (pinMode == "world")
        else
        {
            return oldPosition;
        }

       

    }*/
}
