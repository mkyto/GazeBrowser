using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class myRayCastListenerRadial : MonoBehaviour
{
    Transform[] firstChildren2;
    List<Transform> menuObjects;
    List<Transform> statusObjects;
    List<Transform> diagnosticObjects;
    List<Transform> datalogObjects;
    bool isFreezed;
    float currentTime;
    float triggerTime;
    float timeLimit;
    Vector3 worldPos;
    float dwellTime;
    public bool movementEnabled;
    Transform groundConnector;
    Texture2D moveSelected;
    Texture2D moveSymbol;
    
    // Use this for initialization

    private void Awake()
    {
        /*
        moveSelected = new Texture2D(1, 1);
        moveSymbol = new Texture2D(1, 1);
        string moveFileName = "moveSelected.png";
        string moveFilePath = Path.Combine(Application.streamingAssetsPath, moveFileName);

        string moveSymbolName = "movesymbol.png";
        string moveSymbolPath = Path.Combine(Application.streamingAssetsPath, moveSymbolName);


        byte[] bytes = UnityEngine.Windows.File.ReadAllBytes(moveFilePath);
        byte[] bytes2 = UnityEngine.Windows.File.ReadAllBytes(moveSymbolPath);

        moveSelected.LoadImage(bytes);
        moveSymbol.LoadImage(bytes2);
        this.gameObject.transform.FindChild("moveButton").GetComponent<Renderer>().material.mainTexture = moveSymbol;
        */
    }

    void Start()
    {
        firstChildren2 = this.gameObject.GetComponentsInChildren<Transform>();
        menuObjects = new List<Transform>();
        statusObjects = new List<Transform>();
        diagnosticObjects = new List<Transform>();
        datalogObjects = new List<Transform>();
        Transform t = this.transform;
        this.gameObject.GetComponent<Transform>().GetComponent<Renderer>().enabled = false;
        //Getrenderer.enabled = false;
        //this.gameObject.active = false;
        this.transform.GetComponent<Renderer>().enabled = false;
        isFreezed = false;
        worldPos = this.transform.parent.transform.position;
        currentTime = Time.time;
        timeLimit = 2.0f;
        dwellTime = 1.0f;
        movementEnabled = false;
        groundConnector = this.gameObject.transform.FindChild("connectorGround");
        foreach (Transform trans in firstChildren2)
        {
            //trans.gameObject.GetComponent<Renderer>().enabled = false;
            //trans.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //Debug.Log("Lapsi" + trans.name);
            if (trans.parent == t)
            {
                menuObjects.Add(trans);
                //Debug.Log("Menu" + trans.name);
            }

            if(trans.parent.name == "Status")
            {
                statusObjects.Add(trans);
                //Debug.Log("Status: " + trans.name);
            }

            if (trans.parent.name == "Diagnostics")
            {
                diagnosticObjects.Add(trans);
            }

            if (trans.parent.name == "Datalog")
            {
               datalogObjects.Add(trans);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;
        Vector3 upDirection = Camera.main.transform.up;
        if (movementEnabled == true)
        {
           // this.gameObject.transform.FindChild("moveButton").GetComponent<Renderer>().material.mainTexture = moveSelected;
            float distanceToObject = Vector3.Distance(this.transform.position, headPosition);
            groundConnector.GetComponent<Renderer>().enabled = false;
            groundConnector.GetComponent<BoxCollider>().enabled = false;
            //Vector3 directionDifference = headPosition + headPosition - this.transform.position;
            //this.transform.position = headPosition + (gazeDirection-angle) * distanceToObject;
            this.transform.parent.transform.position = headPosition + gazeDirection * distanceToObject;
            //this.transform.position = directionDifference * distanceToObject;

        }
        else
        {
           // this.gameObject.transform.FindChild("moveButton").GetComponent<Renderer>().material.mainTexture = moveSymbol;

        }
    }

    void togglePin()
    {

        foreach (Transform tr in menuObjects)
        {

            if (tr.tag == "close" || tr.tag == "moveButton")
            {
                if (isFreezed)
                {
                    tr.GetComponent<Renderer>().enabled = true;
                    tr.GetComponent<Collider>().enabled = true;
                }
                else
                {
                    tr.GetComponent<Renderer>().enabled = false;
                    tr.GetComponent<Collider>().enabled = false;
                }

            }
            if (tr.tag == "pin")
            {
                if (isFreezed)
                {
                    tr.GetComponent<Renderer>().enabled = false;
                    tr.GetComponent<Collider>().enabled = false;
                }
                else
                {
                    tr.GetComponent<Renderer>().enabled = true;
                    tr.GetComponent<Collider>().enabled = true;
                }
            }

        }
    } 


    void openInfo(float rayCastInsideObject)
    {
        //setWorldPosition();
        Debug.Log("openMenu kutsuttiin");
        triggerTime = Time.time;
        if (isFreezed)
        {
            //t.transform.FindChild("Thing")
            if (!this.transform.FindChild("pinnedID").GetComponent<Animation>().IsPlaying("colorChange"))
            {
                Debug.Log("Yritettiin animoida");
                //incBar_next1.Play("loadingObject", PlayMode.StopSameLayer);
                this.transform.FindChild("pinnedID").GetComponent<Animation>().Play("colorChange");
            }



        }

        if (rayCastInsideObject > dwellTime) { 
        foreach (Transform tr in menuObjects)
        {
                        tr.GetComponent<Renderer>().enabled = true;

                if (tr.GetComponent<Collider>() != null)
                {
                    tr.GetComponent<Collider>().enabled = true;
                }
            }
            togglePin();
            }
        }

    void openStatus(float rayCastInsideObject)
    {
        triggerTime = Time.time;
        Debug.Log("openStatus kutsuttiin");

        if (rayCastInsideObject > dwellTime)
        {
            foreach (Transform tr in statusObjects)
            {
                tr.GetComponent<Renderer>().enabled = true;
                if (tr.GetComponent<Collider>() != null)
                {
                    tr.GetComponent<Collider>().enabled = true;
                }

            }
        }

    }


    void openDiagnostics(float rayCastInsideObject)
    {
        triggerTime = Time.time;
        Debug.Log("openMenu kutsuttiin");

        if (rayCastInsideObject > dwellTime)
        {
            foreach (Transform tr in diagnosticObjects)
            {
                tr.GetComponent<Renderer>().enabled = true;

                if (tr.GetComponent<Collider>() != null)
                {
                    tr.GetComponent<Collider>().enabled = true;
                }
            }
        }

    }

    void openDatalog(float rayCastInsideObject)
    {
        triggerTime = Time.time;
        if (rayCastInsideObject > dwellTime)
        {
            Debug.Log("openMenu kutsuttiin");

            foreach (Transform tr in datalogObjects)
            {
                tr.GetComponent<Renderer>().enabled = true;
                if (tr.GetComponent<Collider>() != null)
                {
                    tr.GetComponent<Collider>().enabled = true;
                }

            }
        }
    }

    bool freezeWindow(float raycastInObject)
    {
        if (raycastInObject > dwellTime)
        {
            isFreezed = true;

            togglePin();
            return true;
        }
        else
            return false;
    }


    bool unFreezeWindow(float raycastInObject)
    {
       
        if (raycastInObject > dwellTime)
        {
            this.transform.position = worldPos;
            isFreezed = false;
            groundConnector.GetComponent<Renderer>().enabled = true;
            groundConnector.GetComponent<BoxCollider>().enabled = true;
            togglePin();
            return true;
        }
        else
            return false;
    }


    void removeText()
    {
        currentTime = Time.time;
        if ((currentTime - triggerTime > timeLimit) && isFreezed==false)
        {
            Debug.Log("kutsuttiin remove text");
            foreach (Transform trans in firstChildren2)
            {
                //trans.gameObject.GetComponent<Renderer>().enabled = false;
                //trans.gameObject.GetComponent<MeshRenderer>().enabled = false;
                trans.GetComponent<Renderer>().enabled = false;
                if (trans.GetComponent<Collider>() != null)
                    trans.GetComponent<Collider>().enabled = false;
        }
            //this.transform.GetComponent<Renderer>().enabled = false;
            //this.gameObject.GetComponent<Transform>().GetComponent<Renderer>().enabled = false;
            //this.gameObject.GetComponent<Renderer>().enabled = false;
            /*float curTime = this.currentTime;
            float trTime = getTriggerTime();


            if ((curTime - trTime) > timeLimit && isFreezed == false)
            {
                foreach (Transform trans in children)
                {
                    trans.GetComponent<MeshRenderer>().enabled = false;
                }

                foreach (Transform trans in grandChildren)
                {
                    trans.GetComponent<MeshRenderer>().enabled = false;
                }

                foreach (Transform trans in grandGrandChildren)
                {
                    trans.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }*/
        }
        
    }

    void removeFreezedText()
    {
        //currentTime = Time.time;

        //Debug.Log()


        foreach (Transform trans in firstChildren2)
        {
            trans.GetComponent<Renderer>().enabled = false;
            if (trans.GetComponent<Collider>() != null)
                trans.GetComponent<Collider>().enabled = false;
        }
    }

    void setWorldPosition()
    {
        this.transform.position = worldPos;
    }

    void removeTextOverlap()
    {

        //this.gameObject.GetComponent<MeshRenderer>().enabled = false;

        if (isFreezed == false)
        {

            Debug.Log("kutsuttiin remove text overlap");
            foreach (Transform trans in firstChildren2)
            {
                trans.GetComponent<MeshRenderer>().enabled = false;
                if (trans.GetComponent<Collider>() != null)
                    trans.GetComponent<Collider>().enabled = false;

            }
        }
    }

    void enableMove()
    {

        movementEnabled = true;
        Debug.Log("Move enabloitu");

    }


    void disableMove()
    {
        movementEnabled = false;
        //Debug.Log("Move disabloitu");

    }

}

