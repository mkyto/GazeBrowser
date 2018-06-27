using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myRayCastListener : MonoBehaviour {
    //private MeshRenderer myMeshRenderer;
    //private Renderer myRenderer;
    //GameObject child;
    Transform child;
    Transform grandChild;
    Transform grandGrandChild;

    Transform[] firstChildren;
    List<Transform> children;
    List<Transform> grandChildren;
    List<Transform> grandGrandChildren;

    bool TimerStarted;
    float currentTime;
    float triggerTime;
    float timeLimit; //Trigger time limit
    float dwellTime;

    GameObject nextButton;
    GameObject nextButton2;
    GameObject backButton2;
    GameObject backButton3;
    //Hashtable hierChild;
    Transform[] hierChild;
    public int[] hierarchy;
    bool isFreezed;
    int a;
    int i;
    Color color;
    Vector3 worldPos;
    Vector3 screenPos;
    int currentID;

    Animation incBar_next1;
    Animation incBar_freeze1;
    public bool objectIsSelected;
  
    // Use this for initialization
    void Start() {

        objectIsSelected = false;
        isFreezed = false;
        TimerStarted = false;
        firstChildren = GetComponentsInChildren<Transform>();
        worldPos = this.transform.position;

        hierChild = new Transform[GetComponentsInChildren<Transform>().Length];
        hierarchy = new int[GetComponentsInChildren<Transform>().Length];

        currentID = this.GetComponentInParent<ThingProperties>().ID;

        Transform currentParent = this.gameObject.transform.parent;
        a = 0;
        i = 0;
        dwellTime = 2.0f;


        foreach (Transform trans in firstChildren)

        {

            if (trans.name == "NextButton" || trans.name == "NextButton2" || trans.name == "BackButton2" || trans.name == "BackButton3" || trans.name == "freezeButton" || trans.name == "unFreezeButton")
            {
                color = trans.GetComponent<Renderer>().material.color;
                color.a = 0.0f;
                trans.GetComponent<Renderer>().material.color = color;

            }

            if (trans.parent == currentParent) //Check here if the parent has emerged before, then set a to correspond that.
            {
                hierarchy[i] = a;
                currentParent = trans.parent;
            }
            else
            {
                a++;
                hierarchy[i] = a;
                currentParent = trans.parent;
            }
            i++;

        }



        children = new List<Transform>();
        grandChildren = new List<Transform>();
        grandGrandChildren = new List<Transform>();

        i = 0;

        foreach (Transform trans in firstChildren)
        {
            if (hierarchy[i] == 1)
            {
                children.Add(trans);
            }

            if (hierarchy[i] == 2)
            {
                grandChildren.Add(trans);
            }

            if (hierarchy[i] == 3)
            {
                grandGrandChildren.Add(trans);
            }
            i++;

        }
        //this.gameObject.active = false;
        Debug.Log("mycast kaynnistyi");

        foreach (Transform trans in children)
        {
            trans.GetComponent<MeshRenderer>().enabled = false;
            //Debug.Log("Child:" + trans.name);
            if (trans.GetComponent<BoxCollider>() != null)
                trans.GetComponent<BoxCollider>().enabled = false;
        }

        foreach (Transform trans2 in grandChildren)
        {
            trans2.GetComponent<MeshRenderer>().enabled = false;
            if (trans2.GetComponent<BoxCollider>() != null)
                trans2.GetComponent<BoxCollider>().enabled = false;
        }

        foreach (Transform trans3 in grandGrandChildren)
        {
            trans3.GetComponent<MeshRenderer>().enabled = false;
            if (trans3.GetComponent<BoxCollider>() != null)
                trans3.GetComponent<BoxCollider>().enabled = false;
        }


        timeLimit = 2.0f;

        Debug.Log("Startti kutsuttiin");
        isFreezed = false;
        foreach (Transform trans in children)
        {
            //Debug.Log("nykyiset lapset:" + trans.name);
            if (trans.name == "loadingObject_next1")
            {
                incBar_next1 = trans.GetComponent<Animation>();
                incBar_next1.Rewind("loadinObject_next1");
            }
        }

    }

    // Update is called once per frame
    void Update() {
        this.currentTime = Time.time;

        if (isFreezed == true && GameObject.Find("pinningHandler").GetComponent<pinningManagement>().pinMode == "screen")
        {

            //screenPos = GameObject.Find("pinningHandler").GetComponent<pinningManagement>().setUIPosition(this.transform.position, currentID);
            //Debug.Log("Uusi positio" + pos);
            this.transform.position = screenPos;

        }
    }

    void setTriggerTime(float trigTime)
    {
        this.triggerTime = trigTime;

    }

    float getTriggerTime()
    {
        return this.triggerTime;

    }


    void setWorldPosition()
    {
        this.transform.position = worldPos;
    }


    void showHierarchy(int h)
    {
        if (h == 1)
        {
            foreach (Transform trans in children)
            {
                trans.GetComponent<Renderer>().enabled = true;
                if (trans.GetComponent<BoxCollider>() != null && trans.tag != "loadAnimation")
                {
                    trans.GetComponent<BoxCollider>().enabled = true;
                }

                togglePin(isFreezed);

            }

            foreach (Transform trans in grandChildren)
            {
                if (trans.GetComponent<BoxCollider>() != null)
                {
                    trans.GetComponent<BoxCollider>().enabled = false;
                }
                trans.GetComponent<Renderer>().enabled = false;


            }

            foreach (Transform trans in grandGrandChildren)
            {
                if (trans.GetComponent<BoxCollider>() != null)
                {
                    trans.GetComponent<BoxCollider>().enabled = false;
                }
                trans.GetComponent<Renderer>().enabled = false;
            }

        }

        if (h == 2)
        {
            foreach (Transform trans in children)
            {
                if (trans.GetComponent<BoxCollider>() != null)
                {
                    trans.GetComponent<BoxCollider>().enabled = false;
                }

                if (trans.tag == "freezeButton" || trans.tag == "unFreezeButton")
                {
                    if (trans.GetComponent<BoxCollider>() != null)
                    {
                        trans.GetComponent<BoxCollider>().enabled = true;
                    }
                }
                else
                {
                    trans.GetComponent<Renderer>().enabled = false;
                }

            }


            foreach (Transform trans in grandChildren)
            {
                trans.GetComponent<Renderer>().enabled = true;
                if (trans.GetComponent<BoxCollider>() != null && trans.tag != "loadAnimation")
                {
                    trans.GetComponent<BoxCollider>().enabled = true;
                }

            }

            foreach (Transform trans in grandGrandChildren)
            {
                if (trans.GetComponent<BoxCollider>() != null)
                {
                    trans.GetComponent<BoxCollider>().enabled = false;
                }
                trans.GetComponent<Renderer>().enabled = false;


            }
        }

        if (h == 3)
        {
            foreach (Transform trans in children)
            {

                if (trans.GetComponent<BoxCollider>() != null)
                {
                    trans.GetComponent<BoxCollider>().enabled = false;
                }

                if (trans.tag == "freezeButton" || trans.tag == "unFreezeButton")
                {
                    if (trans.GetComponent<BoxCollider>() != null)
                    {
                        trans.GetComponent<BoxCollider>().enabled = true;
                    }
                }
                else
                {
                    trans.GetComponent<Renderer>().enabled = false;
                }

            }

            foreach (Transform trans in grandChildren)
            {
                trans.GetComponent<Renderer>().enabled = false;

                if (trans.GetComponent<BoxCollider>() != null)
                {
                    trans.GetComponent<BoxCollider>().enabled = false;
                }
            }

            foreach (Transform trans in grandGrandChildren)
            {
                trans.GetComponent<Renderer>().enabled = true;
                if (trans.GetComponent<BoxCollider>() != null && trans.tag != "loadAnimation")
                {
                    trans.GetComponent<BoxCollider>().enabled = true;
                }
            }


        }


    }


    void showText(float rayCastOnObject)
    {
        setTriggerTime(currentTime);

        if (rayCastOnObject < 2.0f)
            showHierarchy(1);

        else if (rayCastOnObject >= 2.0f && rayCastOnObject < 4.0f)
            showHierarchy(2);

        else if (rayCastOnObject >= 4.0f)
            showHierarchy(3);
    }





    bool openInfo(float rayCastOnObject)
    {
        //if (isFreezed == true && GameObject.Find("pinningHandler").GetComponent<pinningManagement>().pinMode == "world")
        //{
            setWorldPosition();
       // }

        setTriggerTime(currentTime);
        if (rayCastOnObject > 1.0f) {
            showHierarchy(1);
            return true;
        }
        else {
            return false;
        }
    }


    bool selectNext(float raycastInObject)
    {


        if (!incBar_next1.IsPlaying("loadingObject"))
        {
            Debug.Log("Yritettiin animoida");
            incBar_next1.Play("loadingObject", PlayMode.StopSameLayer);
        }


        /* if (incBar_next1 != null)
         {
             incBar_next1.Rewind("loadingObject");
             incBar_next1.Stop();
         }*/

        if (raycastInObject > dwellTime) {
            showHierarchy(2);
            //incBar_next1.Play("loadingObject");
            //incBar_next1["loadingObject"].speed = -1;
            //incBar_next1["loadingObject"].time = 0;

            //incBar_next1.Rewind();
            //incBar_next1.Stop();
            return true;
        }
        else
            return false;
    }


    bool selectNext2(float raycastInObject)
    {
        if (raycastInObject > dwellTime)
        {
            showHierarchy(3);
            return true;
        }
        else
            return false;
    }

    bool selectPrevious2(float raycastInObject)
    {
        if (raycastInObject > dwellTime)
        {
            showHierarchy(1);
            return true;
        }
        else
            return false;
    }

    bool selectPrevious3(float raycastInObject)
    {
        if (raycastInObject > dwellTime)
        {
            showHierarchy(2);
            return true;
        }
        else
            return false;
    }

    bool freezeWindow(float raycastInObject) {

        foreach (Transform trans in children)
        {
            //Debug.Log("nykyiset lapset:" + trans.name);
            if (trans.name == "loadingObject_freeze1")
            {
                incBar_freeze1 = trans.GetComponent<Animation>();
            }
        }
        if (!incBar_freeze1.IsPlaying("loadingObject"))
        {
            Debug.Log("Yritettiin animoida");
            incBar_freeze1.Play("loadingObject");
        }
        if (raycastInObject > dwellTime)
        {
            isFreezed = true;

            incBar_freeze1.Stop();
            togglePin(true);
            return true;
        }
        else
            return false;
    }


    bool unFreezeWindow(float raycastInObject)
    {
        foreach (Transform trans in children)
        {
            //Debug.Log("nykyiset lapset:" + trans.name);
            if (trans.name == "loadingObject_unFreeze1")
            {
                incBar_freeze1 = trans.GetComponent<Animation>();
            }
        }
        if (!incBar_freeze1.IsPlaying("loadingObject"))
        {
            Debug.Log("Yritettiin animoida");
            incBar_freeze1.Play("loadingObject");
        }



        {
            if (raycastInObject > dwellTime)
            {
                isFreezed = false;
                incBar_freeze1.Stop();
                togglePin(false);
                return true;
            }
            else
                return false;
        }
    }

    //Billboard
    //child.transform.LookAt(Camera.main.transform);
    //child.transform.LookAt(child.transform.position + Camera.main.transform.rotation * Vector3.forward,
    //Camera.main.transform.rotation * Vector3.up);


    void togglePin(bool pinned)
    {

        if (pinned == true)
        {
            foreach (Transform trans in children)
            {
                if (trans.tag == "freezeButton" || trans.name == "loadingObject_freeze1")
                {
                    trans.GetComponent<MeshRenderer>().enabled = false;

                    if (trans.GetComponent<BoxCollider>() != null)
                    {
                        trans.GetComponent<BoxCollider>().enabled = false;

                    }

                }

                if (trans.tag == "unFreezeButton")
                {
                    trans.GetComponent<MeshRenderer>().enabled = true;

                    if (trans.GetComponent<BoxCollider>() != null)
                    {
                        trans.GetComponent<BoxCollider>().enabled = true;

                    }

                }

            }
        }
        else
        {
            foreach (Transform trans in children)
            {
                if (trans.tag == "freezeButton")
                {
                    trans.GetComponent<MeshRenderer>().enabled = true;

                    if (trans.GetComponent<BoxCollider>() != null)
                    {
                        trans.GetComponent<BoxCollider>().enabled = true;

                    }

                }

                if (trans.tag == "unFreezeButton" || trans.name == "loadingObject_unFreeze1")
                {
                    trans.GetComponent<Renderer>().enabled = false;
                    //trans.GetComponent<MeshRenderer>().enabled = false;

                    //if (trans.GetComponent<BoxCollider>() != null)
                    //{
                    //    trans.GetComponent<BoxCollider>().enabled = false;

                    // }

                }

            }

        }
    }

    void removeText()
    {
        float curTime = this.currentTime;
        float trTime = getTriggerTime();
        
        //this.gameObject.GetComponent<MeshRenderer>().enabled = false;

        if ((curTime - trTime) > timeLimit && isFreezed == false)
        {

            //Debug.Log("kutsuttiin remove text");
            foreach (Transform trans in firstChildren)
            {
                trans.GetComponent<MeshRenderer>().enabled = false;
                if (trans.GetComponent<BoxCollider>() != null)
                    trans.GetComponent<BoxCollider>().enabled = false;

            }
        }
    }

    void removeFreezedText()
    {
        //currentTime = Time.time;

        //Debug.Log()


        foreach (Transform trans in firstChildren)
        {

            if (trans.GetComponent<Renderer>() != null) { 
                trans.GetComponent<Renderer>().enabled = false;
            }
            if (trans.GetComponent<BoxCollider>() != null)
                trans.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void removeTextOverlap()
    {
        
        //this.gameObject.GetComponent<MeshRenderer>().enabled = false;

        if (isFreezed == false)
        {

            Debug.Log("kutsuttiin remove text overlap");
            foreach (Transform trans in firstChildren)
            {
                trans.GetComponent<MeshRenderer>().enabled = false;
                if (trans.GetComponent<BoxCollider>() != null)
                    trans.GetComponent<BoxCollider>().enabled = false;

            }
        }
    }

}
/*
            foreach (Transform trans in grandChildren)
            {
                trans.GetComponent<MeshRenderer>().enabled = false;
            if (trans.GetComponent<BoxCollider>() != null)
                trans.GetComponent<BoxCollider>().enabled = false;
        
    }

            foreach (Transform trans in grandGrandChildren)
            {
                trans.GetComponent<MeshRenderer>().enabled = false;
                if (trans.GetComponent<BoxCollider>() != null)
                    trans.GetComponent<BoxCollider>().enabled = false;
            
        }
        }
    }

}*/
