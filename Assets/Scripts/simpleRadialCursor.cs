using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Academy.HoloToolkit.Unity;

public class simpleRadialCursor : MonoBehaviour {

    const int filterSamples = 4;
    public Image calibReticle;

    RectTransform gaze;
    EyeGazeRenderer eyeGazeRenderer;

    MovingAverage cursorAvgX;
    MovingAverage cursorAvgY;
    MovingAverage cursorAvgZ;
    Vector3 filteredCursorPos;

    Renderer noHand1Renderer;
    Renderer noHand2Renderer;
    Renderer staticCircleRenderer;

    public bool isReady;

    Renderer progressCircleRenderer;
    Renderer isManipCursor3Renderer;
    Renderer isManipCursor4Renderer;
    Renderer isManipCursor5Renderer;
    Renderer isManipCursor6Renderer;
    Renderer isManipCursor7Renderer;
    Renderer isManipCursor8Renderer;
    Renderer isManipCursor9Renderer;
    Renderer isManipCursor10Renderer;

    GameObject sceneCreation;
    simpleRadialSceneCreation simpleRadial;
    int gazeMode;
    GameObject cursorPlaneIntersect;

    RaycastHit[] hitInfos;
    GameObject eyeReticle;
    Transform eyeGazeTransform;
    GameObject headPointer;
    GameObject eyePointer;
    Vector3 cursorScale;

    Vector3 headPosition;
    Vector3 headGazeDirection;
    Vector3 eyeGazeDirection;
    Vector3 gazeDirection;

    GestureAction gestureAction;
    Vector3 planeCentre;

    Plane targetPlane;
GameObject[] radialObjects;

    Vector3 pointerScale;

    Color originalColor;


    float timeLastHit;
    float timeRemove;
    float timeLimit;

    // Use this for initialization
    void Start () {
        timeLastHit = 0.0f;
        timeRemove = 0.0f;
        timeLimit = 5.0f;

        isReady = true;
radialObjects = GameObject.FindGameObjectsWithTag("hasRadialChildren");
        pointerScale = new Vector3(0.005f, 0.005f, 0.005f);
        GameObject nohand1 = GameObject.Find("noHandCross1");
        GameObject nohand2 = GameObject.Find("noHandCross2");
        noHand1Renderer = nohand1.GetComponent<Renderer>();
        noHand2Renderer = nohand2.GetComponent<Renderer>();

        originalColor = GameObject.Find("RadialLevel2_1_2 (9)").GetComponent<Renderer>().material.color;
        isManipCursor3Renderer = GameObject.Find("isManipulatingObject3").GetComponent<Renderer>();
        isManipCursor4Renderer = GameObject.Find("isManipulatingObject4").GetComponent<Renderer>();
        isManipCursor5Renderer = GameObject.Find("isManipulatingObject5").GetComponent<Renderer>();
        isManipCursor6Renderer = GameObject.Find("isManipulatingObject6").GetComponent<Renderer>();
        isManipCursor7Renderer = GameObject.Find("isManipulatingObject7").GetComponent<Renderer>();
        isManipCursor8Renderer = GameObject.Find("isManipulatingObject8").GetComponent<Renderer>();
        isManipCursor9Renderer = GameObject.Find("isManipulatingObject9").GetComponent<Renderer>();
        isManipCursor10Renderer = GameObject.Find("isManipulatingObject10").GetComponent<Renderer>();
        progressCircleRenderer = GameObject.Find("ProgressCirlce").GetComponent<Renderer>();
        staticCircleRenderer = GameObject.Find("staticCircle").GetComponent<Renderer>();
        staticCircleRenderer.enabled = false;


        sceneCreation = GameObject.Find("sceneCreation");
        simpleRadial = sceneCreation.GetComponent<simpleRadialSceneCreation>();
        gazeMode = simpleRadial.condition;

        cursorPlaneIntersect = GameObject.Find("CursorPlaneIntersect");

        gestureAction = gameObject.GetComponent<GestureAction>();

        cursorScale = transform.localScale;

        cursorAvgX = new MovingAverage(filterSamples);
        cursorAvgY = new MovingAverage(filterSamples);
        cursorAvgZ = new MovingAverage(filterSamples);
        setCursorManipEnabled(true);
        eyeReticle = GameObject.Find("EyeReticle");
        if (eyeReticle != null)
        {
            eyeGazeTransform = eyeReticle.transform;
            eyeGazeRenderer = eyeReticle.GetComponent<EyeGazeRenderer>();
            //targetSelection.setParticipant(eyeGazeRenderer.m_participantNum, eyeGazeRenderer.m_conditionNum);
        }

        //Mikko disabled

        //#if WINDOWS_UWP
        //hide reticle
        //      calibReticle.enabled = false;
        //#endif

        progressCircleRenderer.enabled = false;
        headPointer = GameObject.Find("HeadPointer");
        eyePointer = GameObject.Find("EyePointer");
    }
	
	// Update is called once per frame
	void Update () {

        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
          Vector3.up);
        headPosition = Camera.main.transform.position;


        headGazeDirection = Camera.main.transform.forward;
       
        if(eyeGazeRenderer.m_conditionNum != 0) { 
            gazeMode = eyeGazeRenderer.m_conditionNum;
            }
            else
            {
                gazeMode = simpleRadial.condition;
            }
        

        /*    

     CONDITIONS

1. Head pointing
2. Eye pointing
3. Head pointing + gesture refinement
4. Head pointing + device refinement
5. Eye pointing + head refinement
6. Eye pointing + gesture refinement
7. Eye pointing + device refinement
8. Head pointing + head refinement


     */

        Debug.Log("Gaze mode:" + gazeMode);


        

        if (gazeMode == 3 || gazeMode == 4 || gazeMode == 5 || gazeMode == 6 || gazeMode == 7 || gazeMode == 8)
        {

            if (gazeMode == 3 || gazeMode == 6)
            {
                if (HandsManager.Instance.HandDetected && gazeMode == 3)
                {
                    staticCircleRenderer.enabled = true;
                    noHand1Renderer.enabled = false;
                    noHand2Renderer.enabled = false;
                }
                else if (HandsManager.Instance.HandDetected && gazeMode == 6)
                {
                    staticCircleRenderer.enabled = false;
                    noHand1Renderer.enabled = false;
                    noHand2Renderer.enabled = false;
                }
                else
                {

                    staticCircleRenderer.enabled = false;
                    noHand1Renderer.enabled = true;
                    noHand2Renderer.enabled = true;


                }
            }

            if (gazeMode == 5 || gazeMode == 6 || gazeMode == 7)
            {
                //eye conditions - use head pointer for start trial
                //if (targetSelection.showEyePointer || eyeGazeRenderer.m_showReticle)
                if (simpleRadial.showEyePointer || eyeGazeRenderer.m_showReticle)
                    eyePointer.SetActive(true);
                else
                    eyePointer.SetActive(false);
            }
            else
            {
                eyePointer.SetActive(false);
            }

    
            if (gazeMode == 4 || gazeMode == 8)
            {
                //staticCircleRenderer.enabled = true;
                staticCircleRenderer.enabled = false;
                noHand1Renderer.enabled = false;
                noHand2Renderer.enabled = false;
            }
            else if (gazeMode == 5 || gazeMode == 7)
            {
                staticCircleRenderer.enabled = false;
                noHand1Renderer.enabled = false;
                noHand2Renderer.enabled = false;
            }

            bool isManipulating = GestureManager.Instance.IsManipulating;
            // Debug.Log("Is Manipulating: " + isManipulating);

            if (isManipulating)
            {

                headPointer.GetComponent<Renderer>().enabled = false;
                //staticCircleRenderer.enabled = true;
                noHand1Renderer.enabled = false;
                noHand2Renderer.enabled = false;
                //ismanipulatingCursor1.GetComponent<Renderer>().enabled = true;
                //ismanipulatingCursor2.GetComponent<Renderer>().enabled = true;
                setCursorManipEnabled(true);
                Debug.Log("isManipulating is true");
                //this.gameObject.GetComponent<Renderer>().material.color = manipulationColor;
                gazeDirection = this.transform.position - headPosition;

                if (gazeMode == 5 || gazeMode == 8)
                {
                    gestureAction.PerformManipulationUpdateHead();
                    gazeDirection = transform.position - headPosition;
                }


                //hitInfos = Physics.RaycastAll(headPosition, gazeDirection);
                RaycastHit hitPointing;



                // if ((Physics.Raycast(headPosition, eyeGazeDirection, out hitEyePointing)) || (Physics.Raycast(headPosition, headGazeDirection, out hitHeadPointing)) || (Physics.Raycast(headPosition, cursorDirection, out hitCursorRefinement)))

                if (Physics.Raycast(headPosition, gazeDirection, out hitPointing))
                {
                    //print("Found an object - distance: " + hit.distance);

                    //this.transform.position = hitEyePointing.point;

                    this.transform.position = hitPointing.point;
                    //headPointer.transform.position = 

                    transform.LookAt(this.transform.position + Camera.main.transform.rotation * Vector3.forward,
                           Vector3.up);

                    Debug.Log("Osui objectiin" + hitPointing.collider.name);
                    if (hitPointing.collider.tag == "hasRadialChildren")
                    {
                        hitPointing.collider.SendMessage("showChildren");
                        //Debug.Log("Message sent");

                    }



                    if (hitPointing.collider.tag == "selectableObject_radioButton" || hitPointing.collider.tag == "selectableObject_controlParent")
                    {
                        // Color color = hit.collider.gameObject.GetComponent<Renderer>().material.color;
                        //Color color = GameObject.Find("Type1").GetComponent<Renderer>().material.color;
                        Color color = originalColor;
                        color.g = 1.0f;
                        hitPointing.collider.gameObject.GetComponent<Renderer>().material.color = color;
                        if (hitPointing.collider.tag == "selectableObject_controlParent")
                        {
                            hitPointing.collider.transform.parent.GetComponent<Renderer>().material.color = color;
                        }
                        Transform[] trans = hitPointing.collider.gameObject.GetComponentsInParent<Transform>();
                        //hit.collider.gameObject.GetComponent<selectableObject>().isSelected = true;

                        foreach (Transform tr in hitPointing.collider.transform.parent)
                        {
                            if (tr != hitPointing.collider.gameObject.GetComponent<Transform>())
                            //if(tr.gameObject.GetComponent<selectableObject>().isSelected == false)
                            {
                                tr.gameObject.GetComponent<Renderer>().material.color = originalColor;

                            }

                        }
                    }

                }
                else
                {
                    //mikko is this preventing refinement
                    this.transform.localPosition = Camera.main.transform.position + Camera.main.transform.forward * 2;
                    //headPointer.transform.localPosition = cursorOrigPosition;     
                    foreach (GameObject radialObject in radialObjects)
                    {
                        radialObject.SendMessage("hideChildren");
                        Debug.Log("hideChildren called");

                    }
                }




            }

            else
            {
                //ismanipulatingCursor1.GetComponent<Renderer>().enabled = false;
                //ismanipulatingCursor2.GetComponent<Renderer>().enabled = false;
                headPointer.GetComponent<Renderer>().enabled = true;

                setCursorManipEnabled(false);


                //head conditions - use head gaze direction
                if (gazeMode == 3 || gazeMode == 4 || gazeMode == 8)
                {
                    gazeDirection = headGazeDirection;
                    //headPointer.transform.position = headPosition + gazeDirection * 2;


                    RaycastHit hitHeadPointing;

                    if (Physics.Raycast(headPosition, headGazeDirection, out hitHeadPointing))
                    {
                        //print("Found an object - distance: " + hit.distance);

                        this.transform.position = hitHeadPointing.point;

                        headPointer.transform.position = hitHeadPointing.point;
                        //headPointer.transform.position = 

                        transform.LookAt(this.transform.position + Camera.main.transform.rotation * Vector3.forward,
                               Vector3.up);

                        Debug.Log("Head hit object: " + hitHeadPointing.collider.name);
                        if (hitHeadPointing.collider.tag == "hasRadialChildren")
                        {
                            hitHeadPointing.collider.SendMessage("showChildren");
                            //Debug.Log("Message sent");

                        }



                        if (hitHeadPointing.collider.tag == "selectableObject_radioButton" || hitHeadPointing.collider.tag == "selectableObject_controlParent")
                        {
                            // Color color = hit.collider.gameObject.GetComponent<Renderer>().material.color;
                            //Color color = GameObject.Find("Type1").GetComponent<Renderer>().material.color;
                            Color color = originalColor;
                            color.g = 1.0f;
                            hitHeadPointing.collider.gameObject.GetComponent<Renderer>().material.color = color;
                            if (hitHeadPointing.collider.tag == "selectableObject_controlParent")
                            {
                                hitHeadPointing.collider.transform.parent.GetComponent<Renderer>().material.color = color;
                            }
                            Transform[] trans = hitHeadPointing.collider.gameObject.GetComponentsInParent<Transform>();
                            //hit.collider.gameObject.GetComponent<selectableObject>().isSelected = true;

                            foreach (Transform tr in hitHeadPointing.collider.transform.parent)
                            {
                                if (tr != hitHeadPointing.collider.gameObject.GetComponent<Transform>())
                                //if(tr.gameObject.GetComponent<selectableObject>().isSelected == false)
                                {
                                    tr.gameObject.GetComponent<Renderer>().material.color = originalColor;

                                }

                            }
                        }

                    }
                    else
                    {
                        //mikko is this preventing refinement
                        headPointer.transform.position = headPosition + gazeDirection * 2;
                        //this.transform.localPosition = Camera.main.transform.position + Camera.main.transform.forward * 2;
                        //headPointer.transform.localPosition = cursorOrigPosition;     
                        foreach (GameObject radialObject in radialObjects)
                        {

                            radialObject.SendMessage("hideChildren");
                            //Debug.Log("Head called hide children");
                        }
                    }



                }
                else //use eye gaze
                {
                    //gazeDirection = GameObject.Find("Left").GetComponent<EyeGazeRenderer>().eyeGazeDirection;               

                    if (eyeGazeTransform != null)
                    {
                        //Vector3 p1 = Camera.main.transform.position;
                        Vector3 p1 = headPosition;
                        Vector3 p2 = eyeGazeTransform.position;
                        
                        eyeGazeDirection = p2 - p1;
                        //gazeDirection = eyeGazeDirection;
                    }


                    RaycastHit hitEyePointing;

                    if (Physics.Raycast(headPosition, eyeGazeDirection, out hitEyePointing))
                    {
                        //print("Found an object - distance: " + hit.distance);

                        timeLastHit = Time.time;// 0.0f;
                        timeRemove = 0.0f;
                        timeLimit = 5.0f;
                       

                        Debug.Log("Eye hit object: " + hitEyePointing.collider.name);
                        this.transform.position = hitEyePointing.point;
                        //eyePointer.transform.position = hitEyePointing.point;

                        /* 
                                   float eyePointerDistance =  Vector3.Distance(headPosition, hitEyePointing.point);

                                    headPointer.transform.position = hitEyePointing.point;
                                    headPointer.transform.localScale = pointerScale * eyePointerDistance;
                                    transform.LookAt(this.transform.position + Camera.main.transform.rotation * Vector3.forward,
                                           Vector3.up);

                          */
                        if (hitEyePointing.collider.gameObject.transform.parent != null)
                        {
                            if (hitEyePointing.collider.tag == "hasRadialChildren" && hitEyePointing.collider.gameObject.transform.parent.tag != "hasRadialChildren")
                            {
                                hitEyePointing.collider.SendMessage("showChildren");
                                Debug.Log("Eye called showChildren");
                            }
                        }

                    }
                    else
                    {
                        timeRemove = Time.time;

                        if(timeRemove-timeLastHit>timeLimit) { 
                        //mikko is this preventing refinement          
                        //headPointer.transform.localPosition = cursorOrigPosition;     
                        foreach (GameObject radialObject in radialObjects)
                        {
                            radialObject.SendMessage("hideChildren");
                            Debug.Log("Eye called hide children");

                        }
                        }
                    }



                    //The values from the eye-tracking server should come here///
                    //     gazeDirection = gz.eyeGazeDirection;

                    //this.transform.position = headPosition + gazeDirection; //WHY NOT DOUBLE
                    //Debug.Log("Eye gaze direction: x: " + gazeDirection.x + ", y:" + gazeDirection.y + ", z:" + gazeDirection.z);
                }

            }

        }
        Ray gazeRay;
        //project cursor onto target plane

        Vector3 cursorDirection = headPosition - this.transform.position;
       
        gazeRay = new Ray(headPosition, gazeDirection);

        
        // int layerMaski = 1 << 0;
        //layerMaski = layerMaski;


        
       






        //planeCentre.z = planeCentre.z - 0.02f;

        //if(targetPlane != null) { 
        /*Mikko commented out this
         * 
         * float gazeDistance;
            targetPlane.Raycast(gazeRay, out gazeDistance);
        Vector3 gazeIntersect = gazeRay.GetPoint(gazeDistance); //actual gaze intersecton plane
        cursorPlaneIntersect.transform.position = gazeIntersect;

        Vector3 cursorLocation = gazeRay.GetPoint(gazeDistance - 0.01f); // draw cursor slightly closer
        AddCursorPos(cursorLocation);
        this.transform.position = filteredCursorPos;
        this.transform.localScale = cursorScale * (gazeDistance / 2.0f); //scale is 1 at 2m distance
        */



        //Debug.Log("cursorLocation: " + cursorLocation + "\nfilteredLocation: " + filteredCursorPos);

        //project head pointer onto target plane
        /* Mikko commented out this
        Vector3 pointerScale = new Vector3(0.005f, 0.005f, 0.005f);
        float headPointerDistance;
        Ray headRay = new Ray(headPosition, headGazeDirection);
        targetPlane.Raycast(headRay, out headPointerDistance);
        Vector3 headIntersect = headRay.GetPoint(headPointerDistance);
        headPointer.transform.position = headIntersect;
        headPointer.transform.localScale = pointerScale * headPointerDistance;
     and added this
        */


        //project eye pointer onto target plane

        //mikko commented out this
        /*
        float eyePointerDistance;
        Vector3 eyePointerDirection = eyeGazeTransform.position - headPosition;
        Ray eyeRay = new Ray(headPosition, eyePointerDirection);
        targetPlane.Raycast(eyeRay, out eyePointerDistance);
        Vector3 eyeIntersect = eyeRay.GetPoint(eyePointerDistance);
        eyePointer.transform.position = eyeIntersect;
        eyePointer.transform.localScale = pointerScale * eyePointerDistance;
        */

        Debug.DrawRay(headPosition, gazeDirection);
            //gazeRay.transform.position = headPosition + gazeDirection;
            //gazeRay.transform.up = gazeDirection;

            //Physics.Raycast(headPosition, gazeDirection, out hitInfo_gaze);
            //Physics.Raycast(headPosition, headGazeDirection, out hitInfo_head);


        //}


    }

public Vector3 AddCursorPos(Vector3 newPos)
{
    filteredCursorPos.x = cursorAvgX.AddSample(newPos.x);
    filteredCursorPos.y = cursorAvgY.AddSample(newPos.y);
    filteredCursorPos.z = cursorAvgZ.AddSample(newPos.z);
    return filteredCursorPos;
}

void setCursorManipEnabled(bool isEnabled)
    {
        isManipCursor3Renderer.enabled = isEnabled;
        isManipCursor4Renderer.enabled = isEnabled;
        isManipCursor5Renderer.enabled = isEnabled;
        isManipCursor6Renderer.enabled = isEnabled;
        isManipCursor7Renderer.enabled = isEnabled;
        isManipCursor8Renderer.enabled = isEnabled;
        isManipCursor9Renderer.enabled = isEnabled;
        isManipCursor10Renderer.enabled = isEnabled;
}
}

  class MovingAverage
{
    List<float> samples = new List<float>();
    int length = 5;

    public MovingAverage(int len)
    {
        length = len;
    }
    public float AddSample(float v)
    {
        samples.Add(v);
        while (samples.Count > length)
        {
            samples.RemoveAt(0);
        }
        float s = 0;
        for (int i = 0; i < samples.Count; ++i)
            s += samples[i];

        return s / (float)samples.Count;

    }
}
