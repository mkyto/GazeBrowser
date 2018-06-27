using System.Collections;
using System.Collections.Generic;
using Image = UnityEngine.UI.Image;
using UnityEngine;
using Academy.HoloToolkit.Unity;

public class WorldCursorTargetSelection : MonoBehaviour
{
    const int filterSamples = 4;
    public Image calibReticle;

    MovingAverage cursorAvgX;
    MovingAverage cursorAvgY;
    MovingAverage cursorAvgZ;
    Vector3 filteredCursorPos;
    Vector3 gazeDirection;
    Vector3 headGazeDirection;
    Vector3 eyeGazeDirection;

    Vector3 headPosition;

    int gazeMode;
    RectTransform gaze;
    EyeGazeRenderer eyeGazeRenderer;

    Color manipulationColor;
    Color originalColor;

    //GameObject nohand1;
    //GameObject nohand2;

    //GameObject ismanipulatingCursor1;
    //GameObject ismanipulatingCursor2;

    //RaycastHit hitInfo_gaze;
    //RaycastHit hitInfo_head;
    RaycastHit hitInfo;

    public BuildCircleMesh circle;
    float dwellTime;
    private float prog;
    public bool startAngleInsteadOfEndAngle;
    public bool bothAngles;
    float rayCastInsideObject;
    public bool isReady;
   // GameObject staticCircle;
    GameObject sceneCreation;
    sceneCreationTargetSelection targetSelection;
    RaycastHit[] hitInfos;
    GameObject eyeReticle;
    Transform eyeGazeTransform;
    GameObject headPointer;
    GameObject eyePointer;
    //GameObject gazeRay;
    GestureAction gestureAction;
    GameObject cursorPlaneIntersect;
    Vector3 cursorScale;
    Renderer staticCircleRenderer;
    Renderer noHand1Renderer;
    Renderer noHand2Renderer;
    Renderer isManipCursor3Renderer;
    Renderer isManipCursor4Renderer;
    Renderer isManipCursor5Renderer;
    Renderer isManipCursor6Renderer;
    Renderer isManipCursor7Renderer;
    Renderer isManipCursor8Renderer;
    Renderer isManipCursor9Renderer;
    Renderer isManipCursor10Renderer;

    private void Awake()
    {

    }

    void Start()
    {
        isReady = false;
        GameObject nohand1 = GameObject.Find("noHandCross1");
        GameObject nohand2 = GameObject.Find("noHandCross2");
        noHand1Renderer = nohand1.GetComponent<Renderer>();
        noHand2Renderer = nohand2.GetComponent<Renderer>();

        //ismanipulatingCursor1 = GameObject.Find("isManipulatingObject1");
        //ismanipulatingCursor2 = GameObject.Find("isManipulatingObject2");
        //ismanipulatingCursor1.GetComponent<Renderer>().enabled = false;
        //ismanipulatingCursor2.GetComponent<Renderer>().enabled = false;
        isManipCursor3Renderer = GameObject.Find("isManipulatingObject3").GetComponent<Renderer>();
        isManipCursor4Renderer = GameObject.Find("isManipulatingObject4").GetComponent<Renderer>();
        isManipCursor5Renderer = GameObject.Find("isManipulatingObject5").GetComponent<Renderer>();
        isManipCursor6Renderer = GameObject.Find("isManipulatingObject6").GetComponent<Renderer>();
        isManipCursor7Renderer = GameObject.Find("isManipulatingObject7").GetComponent<Renderer>();
        isManipCursor8Renderer = GameObject.Find("isManipulatingObject8").GetComponent<Renderer>();
        isManipCursor9Renderer = GameObject.Find("isManipulatingObject9").GetComponent<Renderer>();
        isManipCursor10Renderer = GameObject.Find("isManipulatingObject10").GetComponent<Renderer>();
        setCursorManipEnabled(false);

        //staticCircle = GameObject.Find("staticCircle");
        staticCircleRenderer = GameObject.Find("staticCircle").GetComponent<Renderer>();
        staticCircleRenderer.enabled = false;

        gestureAction = gameObject.GetComponent<GestureAction>();

        cursorPlaneIntersect = GameObject.Find("CursorPlaneIntersect");

        //staticCircle.GetComponent<Renderer>().enabled = false;

        this.gameObject.GetComponent<Renderer>().enabled = false;

        cursorScale = transform.localScale;
        originalColor = this.gameObject.GetComponent<Renderer>().material.color;
        manipulationColor = new Color(0.0f, 0.6f, 1.0f);
        startAngleInsteadOfEndAngle = false;
        bothAngles = false;
        resetCircle();
        sceneCreation = GameObject.Find("sceneCreation");
        targetSelection = sceneCreation.GetComponent<sceneCreationTargetSelection>();
        gazeMode = targetSelection.condition;
        rayCastInsideObject = 0.0f;
        //dwellTime = 2.0f;
        dwellTime = 1.0f;
        prog = 0f;

        cursorAvgX = new MovingAverage(filterSamples);
        cursorAvgY = new MovingAverage(filterSamples);
        cursorAvgZ = new MovingAverage(filterSamples);

        eyeReticle = GameObject.Find("EyeReticle");
        if (eyeReticle != null)
        {
            eyeGazeTransform = eyeReticle.transform;
            eyeGazeRenderer = eyeReticle.GetComponent<EyeGazeRenderer>();
            //targetSelection.setParticipant(eyeGazeRenderer.m_participantNum, eyeGazeRenderer.m_conditionNum);
        }


#if WINDOWS_UWP
        //hide reticle
        calibReticle.enabled = false;
#endif


        headPointer = GameObject.Find("HeadPointer");
        eyePointer = GameObject.Find("EyePointer");

        //gazeRay = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        //gazeRay.transform.localScale = new Vector3(0.01f, 2.0f, 0.01f);
        //gazeRay.transform.position = new Vector3(0, 0, -1);

        // Grab the mesh renderer that's on the same object as this script.

        //  gz = GameObject.Find("Left").GetComponent<EyeGazeRenderer>();

        //  gaze = gz.gaze;
    }


    // Update is called once per frame
    void Update()
    {
        if (eyeGazeRenderer.m_sync)
        {
            if (eyeGazeRenderer.m_head)
                targetSelection.headConditions = true;
            else
                targetSelection.headConditions = false;


            if (eyeGazeRenderer.m_participantNum != targetSelection.userID || eyeGazeRenderer.m_conditionNum != targetSelection.conditionIndex)
            {
                targetSelection.setParticipant(eyeGazeRenderer.m_participantNum, eyeGazeRenderer.m_conditionNum);
            }
        }

        // Debug.Log("Cursors are ready: " + isReady);
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
           Vector3.up);
        headPosition = Camera.main.transform.position;


        headGazeDirection = Camera.main.transform.forward;
        gazeMode = targetSelection.condition;
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

        // Debug.Log("World cursor, Is manipulating:" + GestureManager.Instance.IsManipulating);

        Vector3 planeCentre = targetSelection.targetCentre.transform.position;
        //planeCentre.z = planeCentre.z - 0.02f;
        Plane targetPlane = new Plane(targetSelection.referenceObject.transform.forward, planeCentre);
        Ray gazeRay;

        if (gazeMode == 1) //Head-point
        {
            staticCircleRenderer.enabled = true;
            noHand1Renderer.enabled = false;
            noHand2Renderer.enabled = false;
            //ismanipulatingCursor1.GetComponent<Renderer>().enabled = true;
            //ismanipulatingCursor2.GetComponent<Renderer>().enabled = true;
            setCursorManipEnabled(true);

            gazeDirection = headGazeDirection;
            eyePointer.SetActive(false);
            //this.transform.position = headPosition + gazeDirection * 2;
        }


        if (gazeMode == 2) //Eye-gaze
        {
            //staticCircle.GetComponent<Renderer>().enabled = true;
            staticCircleRenderer.enabled = false;
            noHand1Renderer.enabled = false;
            noHand2Renderer.enabled = false;
            //ismanipulatingCursor1.GetComponent<Renderer>().enabled = true;
            //ismanipulatingCursor2.GetComponent<Renderer>().enabled = true;
            //ismanipulatingCursor1.GetComponent<Renderer>().enabled = false;
            //ismanipulatingCursor2.GetComponent<Renderer>().enabled = false;
            setCursorManipEnabled(false);

            if (targetSelection.showEyePointer || eyeGazeRenderer.m_showReticle)
                eyePointer.SetActive(true);
            else
                eyePointer.SetActive(false);

            if (eyeGazeTransform != null)
            {
                //Vector3 p1 = Camera.main.transform.position;
                Vector3 p1 = headPosition;
                Vector3 p2 = eyeGazeTransform.position;
                gazeDirection = p2 - p1;
            }

            //The values from the eye-tracking server should come here///

            //     gazeDirection = gz.eyeGazeDirection;
            //this.transform.position = headPosition + gazeDirection;

            //gazeDirection = Camera.main.transform.forward;
            //this.transform.position = headPosition + gazeDirection * 2;
            //this.transform.position = headPosition + gazeDirection;
        }




        if (gazeMode == 3 || gazeMode == 4 || gazeMode == 5 || gazeMode == 6 || gazeMode == 7 || gazeMode == 8)
        {
            if (gazeMode == 5 || gazeMode == 6 || gazeMode == 7)
            {
                //eye conditions - use head pointer for start trial
                if (targetSelection.showEyePointer || eyeGazeRenderer.m_showReticle)
                    eyePointer.SetActive(true);
                else
                    eyePointer.SetActive(false);
            }
            else
            {
                eyePointer.SetActive(false);
            }

            //Non-gesture conditions - disable hand feedback
            //if (gazeMode == 4 || gazeMode == 5 || gazeMode == 7 || gazeMode == 8)
            //{
            //    staticCircle.GetComponent<Renderer>().enabled = true;
            //    nohand1.GetComponent<Renderer>().enabled = false;
            //    nohand2.GetComponent<Renderer>().enabled = false;
            //}
            if (gazeMode == 4 || gazeMode == 8)
            {
                staticCircleRenderer.enabled = true;
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
                staticCircleRenderer.enabled = true;
                noHand1Renderer.enabled = false;
                noHand2Renderer.enabled = false;
                //ismanipulatingCursor1.GetComponent<Renderer>().enabled = true;
                //ismanipulatingCursor2.GetComponent<Renderer>().enabled = true;
                setCursorManipEnabled(true);

                //this.gameObject.GetComponent<Renderer>().material.color = manipulationColor;
                gazeDirection = this.transform.position - headPosition;

                if (gazeMode == 5 || gazeMode == 8)
                {
                    gestureAction.PerformManipulationUpdateHead();
                    gazeDirection = transform.position - headPosition;
                }
            }

            else
            {
                //ismanipulatingCursor1.GetComponent<Renderer>().enabled = false;
                //ismanipulatingCursor2.GetComponent<Renderer>().enabled = false;
                setCursorManipEnabled(false);


                //head conditions - use head gaze direction
                if (gazeMode == 3 || gazeMode == 4 || gazeMode == 8)
                {
                    gazeDirection = headGazeDirection;
                    //this.transform.position = headPosition + gazeDirection * 2;
                }
                else //use eye gaze
                {
                    //gazeDirection = GameObject.Find("Left").GetComponent<EyeGazeRenderer>().eyeGazeDirection;               

                    if (eyeGazeTransform != null)
                    {
                        //Vector3 p1 = Camera.main.transform.position;
                        Vector3 p1 = headPosition;
                        Vector3 p2 = eyeGazeTransform.position;
                        gazeDirection = p2 - p1;
                    }

                    //The values from the eye-tracking server should come here///
                    //     gazeDirection = gz.eyeGazeDirection;

                    //this.transform.position = headPosition + gazeDirection; //WHY NOT DOUBLE
                    //Debug.Log("Eye gaze direction: x: " + gazeDirection.x + ", y:" + gazeDirection.y + ", z:" + gazeDirection.z);
                }

            }

        }

        //project cursor onto target plane
        gazeRay = new Ray(headPosition, gazeDirection);
        float gazeDistance;
        targetPlane.Raycast(gazeRay, out gazeDistance);
        Vector3 gazeIntersect = gazeRay.GetPoint(gazeDistance); //actual gaze intersecton plane
        cursorPlaneIntersect.transform.position = gazeIntersect;

        Vector3 cursorLocation = gazeRay.GetPoint(gazeDistance - 0.01f); // draw cursor slightly closer
        AddCursorPos(cursorLocation);
        this.transform.position = filteredCursorPos;
        this.transform.localScale = cursorScale * (gazeDistance / 2.0f); //scale is 1 at 2m distance

        //Debug.Log("cursorLocation: " + cursorLocation + "\nfilteredLocation: " + filteredCursorPos);

        //project head pointer onto target plane
        Vector3 pointerScale = new Vector3(0.005f, 0.005f, 0.005f);
        float headPointerDistance;
        Ray headRay = new Ray(headPosition, headGazeDirection);
        targetPlane.Raycast(headRay, out headPointerDistance);
        Vector3 headIntersect = headRay.GetPoint(headPointerDistance);
        headPointer.transform.position = headIntersect;
        headPointer.transform.localScale = pointerScale * headPointerDistance;

        //project eye pointer onto target plane
        float eyePointerDistance;
        Vector3 eyePointerDirection = eyeGazeTransform.position - headPosition;
        Ray eyeRay = new Ray(headPosition, eyePointerDirection);
        targetPlane.Raycast(eyeRay, out eyePointerDistance);
        Vector3 eyeIntersect = eyeRay.GetPoint(eyePointerDistance);
        eyePointer.transform.position = eyeIntersect;
        eyePointer.transform.localScale = pointerScale * eyePointerDistance;

        Debug.DrawRay(headPosition, gazeDirection);
        //gazeRay.transform.position = headPosition + gazeDirection;
        //gazeRay.transform.up = gazeDirection;

        //Physics.Raycast(headPosition, gazeDirection, out hitInfo_gaze);
        //Physics.Raycast(headPosition, headGazeDirection, out hitInfo_head);




        hitInfos = Physics.RaycastAll(headPosition, headGazeDirection);
        //hitInfos = Physics.RaycastAll(headPosition, gazeDirection);
        bool isHit;
        isHit = false;
        if (hitInfos.Length > 0)
        {
            //use head pointer in small circle for all conditions
            for (int i = 0; i < hitInfos.Length; i++)
            {
                if (hitInfos[i].collider.tag == "referenceObjectHead")
                {
                    //head pointing mode must hit smaller target
                    //hitInfo_head = hitInfos[i];
                    hitInfo = hitInfos[i];
                    isHit = true;
                }
            }
            //this.transform.position = hitInfos[0].point; //This was lastly commented
            //smallestDepth = hitInfos[0].collider.gameObject.transform.position.z;

                //for (int i = 0; i < hitInfos.Length; i++)
                //{
                //    if ((gazeMode == 1 || gazeMode == 3 || gazeMode == 4) && hitInfos[i].collider.tag == "referenceObjectHead") 
                //    {
                //        //head pointing mode must hit smaller target
                //        //hitInfo_head = hitInfos[i];
                //        hitInfo = hitInfos[i];
                //        isHit = true;
                //    }
                //    else if (hitInfos[i].collider.tag == "referenceObjectHead" || hitInfos[i].collider.tag == "referenceObject")
                //    {
                //        //hitInfo_head = hitInfos[i];
                //        hitInfo = hitInfos[i];
                //        isHit = true;
                //    }
                //}
        }


        /*  if(headIsHit)
          {
              GameObject.Find("ClickerObject").GetComponent<Renderer>().enabled = true;

          }
          else
          {
              GameObject.Find("ClickerObject").GetComponent<Renderer>().enabled = false;
          }*/

        //if (hitInfo_gaze.collider != null && hitInfo_head.collider != null) {

        if (hitInfo.collider != null && isHit)
        {
            //Debug.Log("Gaze osui objektiin: " + hitInfo_gaze.collider.gameObject.name);
            //Debug.Log("Head osui objektiin: " + hitInfo_head.collider.gameObject.name);
            //Debug.Log("Hit info object: " + hitInfo.collider.gameObject.name);

            if (hitInfo.collider.tag == "referenceObject" || hitInfo.collider.tag == "referenceObjectHead")
            {

                if (gazeMode == 3 || gazeMode == 6)
                {
                    if (HandsManager.Instance.HandDetected)
                    {
                        startCircleProgress(true);
                        GameObject.Find("sceneCreation").SendMessage("HideTargets");
                        rayCastInsideObject = rayCastInsideObject + Time.deltaTime;

                    }
                    else
                    {
                        // Debug.Log("Pitaisi resetoitua");
                        rayCastInsideObject = 0.0f;
                        resetCircle();

                    }

                }
                else
                {
                    startCircleProgress(true);
                    GameObject.Find("sceneCreation").SendMessage("HideTargets");
                    rayCastInsideObject = rayCastInsideObject + Time.deltaTime;
                }
            }

            else
            {
                // Debug.Log("Pitaisi resetoitua");
                rayCastInsideObject = 0.0f;
                resetCircle();

            }

        }

        else
        {
            // Debug.Log("Pitaisi resetoitua");
            rayCastInsideObject = 0.0f;
            resetCircle();

        }
        //Ray

        //gaze.localPosition = new Vector3((g.x - 0.5f) * c.pixelRect.width, (g.y - 0.5f) * c.pixelRect.height, 0);

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

    void startCircleProgress(bool inProgress)
    {
        dwellTime = 0.75f + (Random.value * 0.5f); //from 0.75 to 1.25 seconds
        if (!isReady)
        {

            prog += Time.deltaTime / dwellTime * 360f;

            prog += Time.deltaTime * 30f;
            if (prog > 380f)
            {
                prog = 0f;
                isReady = true;
                GameObject.Find("sceneCreation").SendMessage("startTrial");


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
        prog = 0f;
        circle.endAngle = 0f;
        isReady = false;
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
}


