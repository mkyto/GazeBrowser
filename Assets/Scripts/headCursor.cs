using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headCursor : MonoBehaviour {

    private MeshRenderer meshRenderer;
    GameObject thing;

    GameObject previousGameObject;
    GameObject currentGameObject;
    GameObject currentThing;
    GameObject previousThing;

    GameObject[] things;
    //GameObject[] 
    //GameObject sphere_larger_collider;
    float rayCastInsideObject;
    bool textFlag;
    Animation incBar_next1;
    Animation incBar_freeze1;
    Animation incBar_next2;
    Animation incBar_back2;
    Animation incBar_back3;

    GameObject loadingObject1;
    int dummyAnimationState;
    int dummyVariable;
    Transform[] currentChildren;
    /*bool isFreezed;
    bool freezeTriggered;
    float freezeCurrentTime;
    float freezeTriggerTime;
    float freezeTimeLimit;*/

    int previd;
    int currentid;
    RaycastHit hit;
    public BuildCircleMesh circle;
    float dwellTime;
    private float prog;
    public bool startAngleInsteadOfEndAngle;
    public bool bothAngles;



    Vector3 gazeDirection;

    Vector3 headPosition;
    bool isReady;
    public int gazeMode;
    List<RaycastHit> previousHitInfos;
    bool hitIsOn;
    int frameSample;
    RaycastHit dummyHit;
    string objectName;
    GameObject[] thingSymbols;
    RaycastHit hitInfo;
    RaycastHit[] hitInfos;
  



    float[] thingDistancesToRay;
    // Use this for initialization
    void Start()
    {
        thingSymbols = GameObject.FindGameObjectsWithTag("dublicateThingSymbol");
        //gazeMode = 1;
        isReady = false;
        dwellTime = 0.5f;
        prog = 0f;
        startAngleInsteadOfEndAngle = false;
        bothAngles = false;
        resetCircle();

        // Grab the mesh renderer that's on the same object as this script.
        meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
  

        /*for (int i = 0; i < thingSymbols.Length; i++) { 
        Color color = thingSymbols[i].GetComponent<Renderer>().material.color;
       
            color.a = 0.5f;
            thingSymbols[i].GetComponent<Renderer>().material.color = color;
        }*/



        //incBar_next1 = GameObject.Find("loadingObject_next1").GetComponent<Animation>();
        //incBar_next2 = GameObject.Find("loadingObject_next2").GetComponent<Animation>();
        //incBar_back2 = GameObject.Find("loadingObject_back2").GetComponent<Animation>();
        //incBar_back3 = GameObject.Find("loadingObject_back3").GetComponent<Animation>();

        dummyAnimationState = 0;

        //incBar_next2.Rewind();
        rayCastInsideObject = 0.0f;
        //sphere_larger_collider = GameObject.Find("Sphere1_larger_collider");
        previousGameObject = null;
        previousThing = null;

        //currentGameObject = GameObject.Find("DummyGameObject");
        currentGameObject = GameObject.Find(null);
        dummyVariable = 0;
        previd = 0;
        currentid = 0;

        frameSample = 30;
        previousHitInfos = new List<RaycastHit>();
        hitIsOn = false;
        headPosition = Camera.main.transform.position;
        Physics.Raycast(headPosition, Vector3.up, out dummyHit);
        for (int i = 0; i < frameSample; i++)
        {
            previousHitInfos.Add(dummyHit);
        }


    }



    // Update is called once per frame
    void Update()
    {
        things = GameObject.FindGameObjectsWithTag("Thing");
        thingDistancesToRay = new float[thingSymbols.Length];
        /* if (Input.GetMouseButton(0))
         {

             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             if (Physics.Raycast(ray, out hit))
                 if (hit.collider != null)
                 {
                     if(hit.collider.tag == "moveButton")
                     {
                         currentThing = resolveCurrentThing(hit.collider.gameObject);
                         //if (things[i].GetComponent<myRayCastListenerTab>() != null)
                         //{
                             currentThing.SendMessage("enableMove");
                         //}
                     }

                 }
         }/*

        /* else {
             for(int i = 0; i < things.Length; i ++)
             {
                 if(things[i].GetComponent<myRayCastListenerTab>() != null) { 
                 things[i].SendMessage("disableMove");
                 }
             }
         }*/
        if (dummyVariable < 500)
        {

            for (int i = 0; i < things.Length; i++)
            {
                things[i].SendMessage("removeText");
            }
            dummyVariable = dummyVariable + 1;
        }


        headPosition = Camera.main.transform.position;
        gazeDirection = Camera.main.transform.forward;


        Debug.DrawRay(headPosition, gazeDirection);


        hitInfos = Physics.RaycastAll(headPosition, gazeDirection);
        // hitInfosEyeGaze = Physics.RaycastAll(headPosition, eyeGazeDirection);
        // hitInfosHeadGaze = Physics.RaycastAll(headPosition, headGazeDirection);

        //Physics.Raycast(headPosition, gazeDirection, out hitInfo);
        if (previd != currentid && previousThing != null)
        {
            Debug.Log("previousTunnus: " + previd);
            Debug.Log("currentTunnus: " + currentid);
            previousThing.SendMessage("removeTextOverlap");

        }


        previd = currentid;
        previousThing = currentThing;


        if (hitInfos.Length > 0)
        {
            this.transform.position = hitInfos[0].point;
            meshRenderer.enabled = true;
            float smallestDepth;
            //smallestDepth = hitInfos[0].collider.gameObject.transform.position.z;
            smallestDepth = Vector3.Distance(hitInfos[0].point, headPosition);
            hitInfo = hitInfos[0];
            for (int i = 0; i < hitInfos.Length; i++)
            {
                //if(hitInfos[i].collider.gameObject.transform.position.z  < smallestDepth)
                float distance = Vector3.Distance(hitInfos[i].point, headPosition);
                //Debug.Log("Object: " + hitInfos[i].collider.gameObject.name);
                //Debug.Log("Depth: " + distance);
                if (Vector3.Distance(hitInfos[i].point, headPosition) < smallestDepth)
                {
                    //smallestDepth = hitInfos[i].collider.gameObject.transform.position.z;
                    smallestDepth = Vector3.Distance(hitInfos[i].point, headPosition);
                    hitInfo = hitInfos[i];
                }
            }

            this.transform.position = hitInfo.point;



            //this.transform.rotation = Quaternion.FromToRotation(Vector3.up, gazeDirection.normal);
            this.transform.LookAt(Camera.main.transform);
            previousGameObject = currentGameObject;

            currentGameObject = hitInfo.collider.gameObject;
            objectName = currentGameObject.name;

            GameObject currentThing = resolveCurrentThing(currentGameObject);

            if (currentThing != null)
            {
                Debug.Log("Current thing:" + currentThing.name);
            }


            if (currentChildren != null)
            {
                currentChildren = currentThing.GetComponentsInChildren<Transform>();
            }

            if (currentGameObject == previousGameObject)
            {
                rayCastInsideObject = rayCastInsideObject + Time.deltaTime;
                //Debug.Log("Kaytiin textFlag");
            }

            else
            {
                //if(gazeMode == 1) { 
                rayCastInsideObject = 0.0f;
                resetCircle();
                //}
            }

            //FOR DEBUG
            Debug.Log("Osui objektiin: " + objectName + ", RayCastin kesto: " + rayCastInsideObject);

            if (hitInfo.collider.tag == "dublicateThingSymbol")
            {

                //if(currentThing.tag == "Thing")
                //{
                //Debug.Log("Open info kutsuttiin: " + rayCastInsideObject);
                if (currentThing != null)
                {
                    currentThing.SendMessage("openInfo", rayCastInsideObject);
                }
                //}

                startCircleProgress(true);


            }


            if (hitInfo.collider.tag == "nextButton")
            {
                if (currentThing != null)
                {
                    currentThing.SendMessage("selectNext", rayCastInsideObject);
                    startCircleProgress(true);

                }
            }


            if (hitInfo.collider.tag == "freezeButton")
            {
                currentThing.SendMessage("freezeWindow", rayCastInsideObject);
                startCircleProgress(true);
            }

            if (hitInfo.collider.tag == "unFreezeButton")
            {
                currentThing.SendMessage("unFreezeWindow", rayCastInsideObject);
                startCircleProgress(true);
            }



            if (hitInfo.collider.tag == "pin")
            {
                currentThing.SendMessage("freezeWindow", rayCastInsideObject);
                startCircleProgress(true);
            }

            if (hitInfo.collider.tag == "close")
            {
                currentThing.SendMessage("unFreezeWindow", rayCastInsideObject);
                startCircleProgress(true);
            }



            if (hitInfo.collider.tag == "nextButton2")
            {
                //incBar_next2.Rewind();
                // incBar_next1.Play("loadingObject");
                // incBar_next1["loadingObject"].speed = -1;
                // incBar_next1["loadingObject"].time = 0;
                //Debug.Log("Animaatio 1n pitaisi kelautua");

                if (currentChildren == null)
                {
                    currentChildren = currentThing.GetComponentsInChildren<Transform>();
                }

                if (currentThing != null)
                {
                    currentThing.SendMessage("selectNext2", rayCastInsideObject);
                    startCircleProgress(true);

                    foreach (Transform trans in currentChildren)
                    {

                        //Debug.Log("nykyiset lapset:" + trans.name);
                        if (trans.name == "loadingObject_next2")
                        {
                            incBar_next2 = trans.GetComponent<Animation>();
                            //incBar_next2.Rewind();
                        }
                    }

                }
                if (!incBar_next2.IsPlaying("loadingObject2Next"))
                {
                    incBar_next2.Stop();
                    Debug.Log("Yritettiin animoida2");
                    incBar_next2.Play("loadingObject2Next", PlayMode.StopSameLayer);
                }
            }
            else
            {
                if (incBar_next2 != null)
                {
                    //incBar_next2.Rewind();
                    //incBar_next2.Stop();
                }
            }


            if (hitInfo.collider.tag == "backbutton2")
            {
                //incBar_next2.Play("loadingObject2");
                //incBar_next2["loadingObject2"].speed = -1;
                //incBar_next2["loadingObject2"].time = 0;
                currentChildren = currentThing.GetComponentsInChildren<Transform>();
                startCircleProgress(true);
                Debug.Log("Osui backiin2");
                if (currentThing != null)
                {
                    currentThing.SendMessage("selectPrevious2", rayCastInsideObject);
                    //incBar_back2 = currentThing.transform.Find("loadingObject_back2").GetComponent<Animation>();

                    foreach (Transform trans in currentChildren)
                    {

                        if (trans.name == "loadingObject_back2")
                        {
                            incBar_back2 = trans.GetComponent<Animation>();
                            //incBar_back2.Rewind();
                        }
                    }
                }

                if (!incBar_back2.IsPlaying("loadingObject2"))
                {
                    Debug.Log("Yritettiin animoida");
                    incBar_back2.Play("loadingObject2", PlayMode.StopSameLayer);
                }
            }
            else
            {
                if (incBar_back2 != null)
                {
                    //incBar_back2.Rewind();
                    //incBar_back2.Stop();
                }

            }

            if (hitInfo.collider.tag == "backButton3")
            {
                incBar_next2.Rewind();
                currentChildren = currentThing.GetComponentsInChildren<Transform>();
                startCircleProgress(true);
                //incBar_back2.Rewind();
                Debug.Log("Osui backiin3");

                if (currentThing != null)
                {
                    currentThing.SendMessage("selectPrevious3", rayCastInsideObject);

                    foreach (Transform trans in currentChildren)
                    {

                        if (trans.name == "loadingObject_back3")
                            incBar_back3 = trans.GetComponent<Animation>();
                    }

                    //incBar_back3 = currentThing.transform.Find("loadingObject_back3").GetComponent<Animation>();

                }

                if (!incBar_back3.IsPlaying("loadingObject3"))
                {
                    Debug.Log("Yritettiin animoida");
                    incBar_back3.Play("loadingObject3", PlayMode.StopSameLayer);
                }
            }
            else
            {
                if (incBar_back3 != null)
                {
                    //incBar_back3.Rewind();
                    //incBar_back3.Stop();
                }

            }

            if (hitInfo.collider.tag == "status")
            {

                currentThing.SendMessage("openStatus", rayCastInsideObject);
                startCircleProgress(true);
            }

            if (hitInfo.collider.tag == "diagnostics")
            {

                currentThing.SendMessage("openDiagnostics", rayCastInsideObject);
                startCircleProgress(true);
            }

            if (hitInfo.collider.tag == "datalog")
            {

                currentThing.SendMessage("openDatalog", rayCastInsideObject);
                startCircleProgress(true);
            }


            if (hitInfo.collider.name == "Type1")
            {

                GameObject.Find("TypeSelection").SendMessage("selectVisualisationType1", rayCastInsideObject);
                startCircleProgress(true);
            }

            if (hitInfo.collider.name == "Type2")
            {

                GameObject.Find("TypeSelection").SendMessage("selectVisualisationType2", rayCastInsideObject);
                startCircleProgress(true);

            }

            if (hitInfo.collider.name == "Type3")
            {

                GameObject.Find("TypeSelection").SendMessage("selectVisualisationType3", rayCastInsideObject);
                startCircleProgress(true);

            }

            if (hitInfo.collider.name == "pinningTypeWorld")
            {

                GameObject.Find("pinningSelection").SendMessage("selectPinningTypeWorld", rayCastInsideObject);
                startCircleProgress(true);
            }

            if (hitInfo.collider.name == "pinningTypeBody")
            {

                GameObject.Find("pinningSelection").SendMessage("selectPinningTypeBody", rayCastInsideObject);
                startCircleProgress(true);
            }

            if (hitInfo.collider.name == "pinningTypeScreen")
            {

                GameObject.Find("pinningSelection").SendMessage("selectPinningTypeScreen", rayCastInsideObject);
                startCircleProgress(true);
            }



            if (incBar_next1 != null)
            {
                incBar_next1.Rewind();
                incBar_next1.Stop();

            }
            if (incBar_next2 != null)
            {
                incBar_next2.Rewind();
                incBar_next2.Stop();
            }
            if (incBar_back2 != null)
            {
                incBar_back2.Rewind();
                incBar_back2.Stop();
            }
            if (incBar_back3 != null)
            {
                incBar_back3.Rewind();
                incBar_back3.Stop();
            }


        }

        else
        {

                rayCastInsideObject = 0.0f;
                resetCircle();
                this.transform.position = headPosition + gazeDirection * 2;
                clearText();
            
        }
    }

    void clearText()
    {

        for (int i = 0; i < things.Length; i++)
        {
            things[i].SendMessage("removeText");
        }

    }




    GameObject resolveCurrentThing(GameObject currentGameObject)
    {
        Transform t = currentGameObject.transform;


        while (t.parent != null)
        {
            if (t.GetComponent<SymbolProperties>() != null)
            {
                currentid = t.GetComponent<SymbolProperties>().ID;
            }

            if (t.GetComponent<ThingProperties>() != null)
            {
                currentid = t.GetComponent<ThingProperties>().ID;
            }

            t = t.parent.transform;
        }


        GameObject[] allThings = GameObject.FindGameObjectsWithTag("Thing");

        for (int i = 0; i < allThings.Length; i++)
        {
            if (allThings[i].GetComponent<thingID>().ID == currentid)
            {
                currentThing = allThings[i];
            }

        }

        return currentThing;

    }

    void startCircleProgress(bool inProgress)
    {

        if (!isReady)
        {

            prog += Time.deltaTime / dwellTime * 360f;

            prog += Time.deltaTime * 30f;
            if (prog > 380f)
            {
                prog = 0f;
                isReady = true;

            }
            if (startAngleInsteadOfEndAngle) circle.startAngle = prog;
            else if (bothAngles)
            {
                float tProg = prog;
                if (tProg > 180f) tProg = 360f - prog;
                circle.startAngle = tProg;
                circle.endAngle = 360f - tProg;
            }
            else circle.endAngle = prog;
        }
    }
    void resetCircle()
    {
        circle.endAngle = 0f;
        isReady = false;
    }

    void setTextFlag(bool textFlag)
    {
        this.textFlag = textFlag;
    }
}


