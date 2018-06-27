using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Academy.HoloToolkit.Unity;
using UnityEngine.Windows.Speech;

public class simpleRadialSceneCreation : MonoBehaviour {

    GameObject vuforiaImageTarget;
    GameObject vuforiaObject;
    GameObject cursor;
    public int condition;
    internal bool showEyePointer;
    GameObject conditionVis;
    List<string> conditionNames;
    // Use this for initialization
    void Awake()
    {
        vuforiaImageTarget = GameObject.Find("ImageTargetChips");
        vuforiaObject = GameObject.Find("teapot");
        cursor = GameObject.Find("Cursor");
        //condition = 8;
        showEyePointer = false;
        conditionVis = GameObject.Find("ConditionDisplay");
        conditionNames = new List<string>();

        conditionNames.Add("Head only");
        conditionNames.Add("Eye only");
        conditionNames.Add("Head + Gesture");
        conditionNames.Add("Head + Clicker");
        conditionNames.Add("Eye + Head");
        conditionNames.Add("Eye + Gesture");
        conditionNames.Add("Eye + Clicker");
        conditionNames.Add("Head + Head");
    }



	// Use this for initialization
	void Start () {
        setRecognizer(condition);

    }
	
	// Update is called once per frame
	void Update () {
        
        conditionVis.GetComponent<TextMesh>().text = "Condition: " + conditionNames[condition - 1];
    }

    private void setRecognizer(int condition)
    {
        if (condition == 1 || condition == 2 || condition == 4 || condition == 5 || condition == 7 || condition == 8)
        {
            GestureManager.Instance.Transition(GestureManager.Instance.NavigationRecognizer); //clicker
            Debug.Log("Recognizer set to clicker");
        }

        if (condition == 3 || condition == 6)
        {
            GestureManager.Instance.Transition(GestureManager.Instance.ManipulationRecognizer); //gesture
            Debug.Log("Recognizer set to gesture");
        }

    }
}
