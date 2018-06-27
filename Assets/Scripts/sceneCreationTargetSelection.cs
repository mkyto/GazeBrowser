using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Academy.HoloToolkit.Unity;
using UnityEngine.Windows.Speech;

public class sceneCreationTargetSelection : MonoBehaviour
{
    const float HeadOffset = -0.1f;
    const float angleIncrement = 45.0f;

    public int condition;
    List<Vector3> positions;
    // Use this for initialization
    GameObject target;
    GameObject target_horisontal;
    GameObject target_vertical;

    float targetArrangementRadius;
    float currentTime;
    float stage1Time;
    int targetOrder;
    List<int> usedLocations;
    int repetition;
    int position;
    int[] experimentConditions;

    bool nextConditionFlag;
    bool nextRepetitionFlag;
    bool selectionMade;
    string result = "";

    int block;
    internal GameObject referenceObject;

    GameObject cursor;

    //List<float> x_values;
    //List<float> y_values;

    GameObject dataLog;
    public bool isLogging;
    public float targetDistance;
    public int userID = 0;
    public int conditionIndex;
    public bool headConditions;
    float pauseBeforeStimulus;
    bool positionIsSet;
    bool measurementStarted;
    public Vector3 targetOffset;
    public bool trialIsDone;
    float preRefineAccuracy = 0;
    float accuracyInDegrees = 0;

    //GameObject validationTarget;
    //GameObject gazeCheckObject;

    float runTime;

    GameObject vuforiaImageTarget;
    GameObject vuforiaObject;
    Transform refImage;

    // KeywordRecognizer object.
    KeywordRecognizer keywordRecognizer;

    // Defines which function to call when a keyword is recognized.
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;

    GameObject conditionVis;
    GameObject trainingVis;
    GameObject trialVis;
    GameObject participantIDVis;
    GameObject arrow;

    GameObject infoForUser;
    List<GameObject> targets;
    internal GameObject targetCentre;
    internal float targetAngle;
    internal int targetDistCategory;
    GameObject headPointer;
    GameObject eyePointer;
    GameObject cursorPlaneIntersect;
    internal bool showEyePointer;

    float speedLimit = 5.0f; // in seconds
    float accuracyLimit = 5.0f; // in degrees
    float minRefinementTime = 0.2f; //min hold time for refinement phase in s
    float refinementAccuracyLimit = 2.0f; //min accuracy for refinement trials;


    GameObject slowNotification;
    //public GameObject inaccurateNotification;
    // bool trainingIsOn;
    int trainingTargetOrder;
    int numberOfTrainingStimuli;
    List<int> usedTrainingLocations;
    //int[] trainingConditions;


    List<string> conditionNames;
    GameObject dummyObject;

    public Vector3 currentPosition;
    int[,] allTrialOrders;
    Vector3 headPosition;
    GameObject currentTarget;
    Vector3 targetScale;

    public const int NumReps = 1;//4;
    public const int NumTargets = 16;
    public const int NumConditions = 4;
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

    private void Awake()
    {

        currentPosition = new Vector3(0.0f, 0.0f, 0.0f);
        trialIsDone = false;
        selectionMade = false;
        slowNotification = GameObject.Find("SlowNotification");
        //inaccurateNotification = GameObject.Find("InaccurateNotification");
        nextConditionFlag = false;
        nextRepetitionFlag = false;
        //dummyObject = GameObject.Find("dummyLog");
        //dummyObject.SetActive(false);
        slowNotification.SetActive(false);
        //inaccurateNotification.SetActive(false);

        isLogging = false;
        positionIsSet = false;
        conditionIndex = 0;
        trainingTargetOrder = 0;
        numberOfTrainingStimuli = 2;
        //trainingIsOn = true;
        // trainingIsOn = false;
        //usedTrainingLocations = new List<int>();
        //trainingConditions = new int[3];
        //trainingConditions[0] = 1;
        //trainingConditions[1] = 3;
        //trainingConditions[2] = 4;

        conditionNames = new List<string>();

        conditionNames.Add("Head only");
        conditionNames.Add("Eye only");
        conditionNames.Add("Head + Gesture");
        conditionNames.Add("Head + Clicker");
        conditionNames.Add("Eye + Head");
        conditionNames.Add("Eye + Gesture");
        conditionNames.Add("Eye + Clicker");
        conditionNames.Add("Head + Head");

        /*experimentConditions = new int[3];
        experimentConditions[0] = 3;
        experimentConditions[1] = 4;
        experimentConditions[2] = 1;*/





        target = GameObject.Find("TargetObject");
        targetScale = target.transform.localScale;
        referenceObject = GameObject.Find("CircularReferenceObject");
        target_horisontal = GameObject.Find("HorisontalComponent");
        target_vertical = GameObject.Find("VerticalComponent");
        positions = new List<Vector3>();
        dataLog = GameObject.Find("Datalogger");

        vuforiaImageTarget = GameObject.Find("ImageTargetChips");
        vuforiaObject = GameObject.Find("teapot");
        cursor = GameObject.Find("Cursor");
        //validationTarget = GameObject.Find("ValidationTarget");
        //validationTarget.SetActive(false);
        usedLocations = new List<int>();
        measurementStarted = false;
        //x_values = new List<float>();
        //y_values = new List<float>();
        runTime = 0.0f;
        float angle;

        conditionVis = GameObject.Find("ConditionDisplay");
        trainingVis = GameObject.Find("TrainingDisplay");
        trialVis = GameObject.Find("TrialDisplay");
        participantIDVis = GameObject.Find("ParticipantIdDisplay");
        arrow = GameObject.Find("Arrow");
        arrow.SetActive(false);
        headPointer = GameObject.Find("HeadPointer");
        eyePointer = GameObject.Find("EyePointer");

        cursorPlaneIntersect = GameObject.Find("CursorPlaneIntersect");

        //infoForUser = GameObject.Find("InfoDisplayForUser");
        
        infoForUser = GameObject.Find("InaccurateNotification");
        infoForUser.GetComponent<TextMesh>().text = "test notification";
        refImage = GameObject.Find("ImageTargetChips").transform;
        targetCentre = GameObject.Find("TargetCentre");
        //gazeCheckObject = GameObject.Find("GazeCheckObject");

        pauseBeforeStimulus = 0.0f;
        targetDistance = 2.0f;
        float targetOffsetAngle = 7.0f; //targetOffsetRadiusAngle in degrees
        repetition = 1;
        //targetOffset = new Vector3(-0.585f, 0.73f, 0.0f);
        targetOffset = new Vector3(-0.6f, 0.0f, 0.0f); // offset to side of refrence image

        targetOrder = 1;

        targetArrangementRadius = targetDistance * Mathf.Atan(targetOffsetAngle / 180.0f * Mathf.PI);

        for (int i = 0; i < 8; i++)
        {

            angle = i * angleIncrement;
            //positions.Add(new Vector3(Mathf.Cos(angle / 180.0f * Mathf.PI) * targetArrangementRadius, Mathf.Sin(angle / 180.0f * Mathf.PI) * targetArrangementRadius, targetDistance) + targetOffset);
            positions.Add(new Vector3(Mathf.Cos(angle / 180.0f * Mathf.PI) * targetArrangementRadius, Mathf.Sin(angle / 180.0f * Mathf.PI) * targetArrangementRadius, 0)); //offset relative to target centre
        }

        // outer ring
        for (int i = 0; i < 8; i++)
        {

            angle = i * angleIncrement;
            positions.Add(new Vector3(Mathf.Cos(angle / 180.0f * Mathf.PI) * targetArrangementRadius * 3, Mathf.Sin(angle / 180.0f * Mathf.PI) * targetArrangementRadius * 3, 0)); //offset relative to target centre
        }
    }

    void Start()
    {


        //float s_z = Mathf.Sign(-0.5f + Random.value);
        keywordCollection = new Dictionary<string, KeywordAction>();
        target.SetActive(false);
        referenceObject.SetActive(false);
        //vuforiaImageTarget.GetComponent<MeshRenderer>().enabled = true;

        keywordCollection.Add("Condition one", selectCondition1);
        keywordCollection.Add("Condition two", selectCondition2);
        keywordCollection.Add("Condition three", selectCondition3);
        keywordCollection.Add("Condition four", selectCondition4);
        keywordCollection.Add("Condition five", selectCondition5);
        keywordCollection.Add("Condition six", selectCondition6);
        keywordCollection.Add("Condition seven", selectCondition7);
        keywordCollection.Add("Condition eight", selectCondition8);
        //keywordCollection.Add("Next Trial", nextTrial);



        keywordCollection.Add("Participant one", selectParticipant1);
        keywordCollection.Add("Participant two", selectParticipant2);
        keywordCollection.Add("Participant three", selectParticipant3);
        keywordCollection.Add("Participant four", selectParticipant4);
        keywordCollection.Add("Participant five", selectParticipant5);
        keywordCollection.Add("Participant six", selectParticipant6);
        keywordCollection.Add("Participant seven", selectParticipant7);
        keywordCollection.Add("Participant eight", selectParticipant8);
        keywordCollection.Add("Participant nine", selectParticipant9);
        keywordCollection.Add("Participant ten", selectParticipant10);
        keywordCollection.Add("Plus one", participantPlusOne);
        keywordCollection.Add("Minus one", participantMinusOne);


        keywordCollection.Add("Continue training", continueTraining);

        // Initialize KeywordRecognizer with the previously added keywords.
        keywordRecognizer = new KeywordRecognizer(keywordCollection.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();


        vuforiaImageTarget.GetComponent<MeshRenderer>().enabled = true;
        vuforiaObject.GetComponent<MeshRenderer>().enabled = true;

        targets = new List<GameObject>();
        //create targets
        for (int i = 0; i < positions.Count; i++)
        {

            GameObject targetObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            targetObject.name = "target" + i;
            targetObject.transform.localScale = new Vector3(0.07f, 0.07f, 0.001f);
            //targetObject.transform.parent = GameObject.Find("ImageTargetChips").transform;
            //targetObject.transform.parent = refImage;
            //targetObject.transform.position = positions[i];
            targetObject.transform.position = targetCentre.transform.position + positions[i];
            targetObject.GetComponent<Renderer>().material.color = Color.green;
            targetObject.GetComponent<MeshRenderer>().enabled = true;

            targetObject.transform.parent = targetCentre.transform;
            targets.Add(targetObject);

        }

        headConditions = true;
        //infoForUser.transform.position = positions[4] - new Vector3(0.0f, 0.0f, 0.5f);
        //infoForUser.transform.position = targetCentre.transform.position - new Vector3(0.0f, -0.5f, 0.5f);

    }


    private void HideTargets()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].GetComponent<Renderer>().material.color = Color.black;
            targets[i].GetComponent<MeshRenderer>().enabled = false;
        }
    }


    private void setRecognizer(int condition)
    {
        if (condition == 1 || condition == 2 || condition == 4 || condition == 5 || condition == 7 || condition == 8)
        {
            GestureManager.Instance.Transition(GestureManager.Instance.NavigationRecognizer); //clicker
        }

        if (condition == 3 || condition == 6)
        {
            GestureManager.Instance.Transition(GestureManager.Instance.ManipulationRecognizer); //gesture
            Debug.Log("Detecting gestures");
        }

    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (keywordCollection.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }

    private void selectCondition1(PhraseRecognizedEventArgs args)
    {
        conditionIndex = 0;
        goToCondition(conditionIndex);
    }

    private void selectCondition2(PhraseRecognizedEventArgs args)
    {
        conditionIndex = 1;
        goToCondition(conditionIndex);
    }

    private void selectCondition3(PhraseRecognizedEventArgs args)
    {
        conditionIndex = 2;
        goToCondition(conditionIndex);
    }


    private void selectCondition4(PhraseRecognizedEventArgs args)
    {
        conditionIndex = 3;
        goToCondition(conditionIndex);
    }

    private void selectCondition5(PhraseRecognizedEventArgs args)
    {
        conditionIndex = 4;
        goToCondition(conditionIndex);
    }

    private void selectCondition6(PhraseRecognizedEventArgs args)
    {
        conditionIndex = 5;
        goToCondition(conditionIndex);
    }


    private void selectCondition7(PhraseRecognizedEventArgs args)
    {
        conditionIndex = 6;
        goToCondition(conditionIndex);
    }

    private void selectCondition8(PhraseRecognizedEventArgs args)
    {
        conditionIndex = 7;
        goToCondition(conditionIndex);
    }



    private void selectParticipant1(PhraseRecognizedEventArgs args)
    {
        setParticipant(1);
    }

    private void selectParticipant2(PhraseRecognizedEventArgs args)
    {
        setParticipant(2);
    }

    private void selectParticipant3(PhraseRecognizedEventArgs args)
    {
        setParticipant(3);
    }

    private void selectParticipant4(PhraseRecognizedEventArgs args)
    {
        setParticipant(4);
    }

    private void selectParticipant5(PhraseRecognizedEventArgs args)
    {
        setParticipant(5);
    }

    private void selectParticipant6(PhraseRecognizedEventArgs args)
    {
        setParticipant(6);
    }

    private void selectParticipant7(PhraseRecognizedEventArgs args)
    {
        setParticipant(7);
    }

    private void selectParticipant8(PhraseRecognizedEventArgs args)
    {
        setParticipant(8);
    }

    private void selectParticipant9(PhraseRecognizedEventArgs args)
    {
        setParticipant(9);
    }

    private void selectParticipant10(PhraseRecognizedEventArgs args)
    {
        setParticipant(10);
    }

    private void participantPlusOne(PhraseRecognizedEventArgs args)
    {
        setParticipant(userID + 1);
    }

    private void participantMinusOne(PhraseRecognizedEventArgs args)
    {
        setParticipant(userID - 1);
    }

    public void setParticipant(int id, int conditionIndex)
    {
        this.conditionIndex = conditionIndex;
        setParticipant(id);
    }

    private void setParticipant(int id)
    {
        //if (userID == 0)
        //{ 
        //disable eye tracking feedback
        GameObject dt = GameObject.Find("DebugText");
        if (dt != null)
        {
            dt.SetActive(false);
        }
        //hide teapot
        vuforiaObject.GetComponent<MeshRenderer>().enabled = true;

        userID = id;
        dataLog.SendMessage("setParticipantID", userID);
        if (userID > 0)
        {
            selectExperimentalConditions(userID);
        }
        else
        {
            Debug.Log("Participant ID not set - can't set conditions");
        }
        participantIDVis.GetComponent<TextMesh>().text = "Participant ID: " + userID;

        headPosition = Camera.main.transform.position;
        float headHeight = headPosition.y + HeadOffset;
        //headHeight 
        Vector3 newCentre = new Vector3(targetCentre.transform.position.x, headHeight, targetCentre.transform.position.z);
        targetCentre.transform.position = newCentre;
        //newCentre = new Vector3(gazeCheckObject.transform.position.x, headHeight, gazeCheckObject.transform.position.z);
        //gazeCheckObject.transform.position = newCentre;
        targetCentre.transform.LookAt(targetCentre.transform.position + targetCentre.transform.rotation * Vector3.forward, Vector3.up);

        referenceObject.transform.position = newCentre;
        //infoForUser.transform.position = targetCentre.transform.position + new Vector3(-0.5f, 0.25f, -0.25f);
        infoForUser.GetComponent<TextMesh>().text = "Participant: " + userID + "\nCondition: " + conditionIndex + "\n" + conditionNames[mapCondition(experimentConditions[conditionIndex]) - 1];

        Debug.Log("Participant ID set: " + userID);
        //}
        //else
        //{
        //    Debug.Log("Participant ID already set");
        //}
    }


    private void selectExperimentalConditions(int userID)
    {

        /*
        1.Head pointing
        2.Eye pointing
        3.Head pointing + gesture refinement
        4.Head pointing + device refinement
        5.Eye pointing + head refinement
        6.Eye pointing + gesture refinement
        7.Eye pointing + device refinement
                    */
        allTrialOrders = new int[NumConditions, NumConditions] {
            {1, 2, 4, 3},
            {4, 3, 1, 2},
            {2, 4, 3, 1},
            {3, 1, 2, 4}
        };
//        allTrialOrders = new int[NumConditions, NumConditions] {
// { 1, 2, 8, 3, 7, 4, 6, 5 },
// { 2, 3, 1, 4, 8, 5, 7, 6 },
// { 3, 4, 2, 5, 1, 6, 8, 7 },
// { 4, 5, 3, 6, 2, 7, 1, 8 },
// { 5, 6, 4, 7, 3, 8, 2, 1 },
// { 6, 7, 5, 8, 4, 1, 3, 2 },
// { 7, 8, 6, 1, 5, 2, 4, 3 },
// { 8, 1, 7, 2, 6, 3, 5, 4 }
//};
//        allTrialOrders = new int[14, NumConditions] {
//{ 1, 2, 7, 3, 6, 4, 5 },
//{ 2, 3, 1, 4, 7, 5, 6 },
//{ 3, 4, 2, 5, 1, 6, 7 },
//{ 4, 5, 3, 6, 2, 7, 1 },
//{ 5, 6, 4, 7, 3, 1, 2 },
//{ 6, 7, 5, 1, 4, 2, 3 },
//{ 7, 1, 6, 2, 5, 3, 4 },
//{ 5, 4, 6, 3, 7, 2, 1 },
//{ 6, 5, 7, 4, 1, 3, 2 },
//{ 7, 6, 1, 5, 2, 4, 3 },
//{ 1, 7, 2, 6, 3, 5, 4 },
//{ 2, 1, 3, 7, 4, 6, 5 },
//{ 3, 2, 4, 1, 5, 7, 6 },
//{ 4, 3, 5, 2, 6, 1, 7 }

        //};

        //       allTrialOrders = new int[6, 3] {
        //{ 1, 3, 4},
        //{ 4, 1, 3},
        //{ 3, 1, 4},
        //{ 1, 4, 3},
        //{ 4, 3, 1},
        //{ 3, 4, 1},
        // };


        block = ((userID - 1) % allTrialOrders.GetLength(0));


        /*     
                experimentConditions = new int[]{allTrialOrders[block,0],

        allTrialOrders[block,1],

        allTrialOrders[block,2],

        allTrialOrders[block,3],

        allTrialOrders[block,4],

        allTrialOrders[block,5],

        allTrialOrders[block,6]};*/


        /*experimentConditions = new int[]{allTrialOrders[block,0],
allTrialOrders[block,1],
allTrialOrders[block,2]};*/
        experimentConditions = new int[NumConditions];
        for (int i = 0; i < NumConditions; i++)
        {
            experimentConditions[i] = allTrialOrders[block, i];
        }

        //condition = experimentConditions[conditionIndex];
        setCondition(conditionIndex);
        repetition = 1;
        nextConditionFlag = true;

       // infoForUser.GetComponent<TextMesh>().text = " Next condition: \n" + conditionNames[condition - 1];

    }

    private void goToCondition(int index)
    {
        repetition = 1;
        nextConditionFlag = true;
        setCondition(index);
    }

    private void setCondition(int index)
    {
        if (userID == 0)
        {
            infoForUser.GetComponent<TextMesh>().text = "Set Participant";
        }
        else
        {
            int conditionID = experimentConditions[index];
            condition = mapCondition(conditionID);
            setRecognizer(condition);

            conditionVis.GetComponent<TextMesh>().text = "Condition: " + conditionNames[condition - 1];
            Debug.Log("Condition set: " + condition);
        }
    }

    private int mapCondition(int conditionID)
    {
        //condition ID is value taken from latin square
        int condition = 0;

        if (headConditions)
        {
            if (conditionID == 1)
                condition = 1;
            else if (conditionID == 2)
                condition = 3;
            else if (conditionID == 3)
                condition = 4;
            else if (conditionID == 4)
                condition = 8;
        }
        else //eye conditions
        {
            if (conditionID == 1)
                condition = 2;
            else if (conditionID == 2)
                condition = 6;
            else if (conditionID == 3)
                condition = 7;
            else if (conditionID == 4)
                condition = 5;
        }

        return condition;
    }

    private void continueTraining(PhraseRecognizedEventArgs args)
    {
        repetition = 1;
        nextConditionFlag = true;
        usedLocations.Clear();
        nextTrial();
    }


    private void nextTrial()
    {
        if (conditionIndex < experimentConditions.Length)
        {
            setCondition(conditionIndex);
          
            trialVis.GetComponent<TextMesh>().text = "Trial: " + targetOrder.ToString();
            

            Debug.Log("Measurered target number:" + targetOrder);

            if (nextConditionFlag == true)
            {
                showEyePointer = true;
                infoForUser.SetActive(true);
                //infoForUser.GetComponent<TextMesh>().text = " Next condition: \n" + conditionNames[experimentConditions[conditionIndex] - 1] + "\nBegin training";
                infoForUser.GetComponent<TextMesh>().text = " Next condition: \n" + conditionNames[mapCondition(experimentConditions[conditionIndex]) - 1] + "\nBegin training";
                nextConditionFlag = false;
                nextRepetitionFlag = false;
            }
            else if (nextRepetitionFlag == true)
            {
                if (repetition == 2)
                {
                    infoForUser.GetComponent<TextMesh>().text = "End of training\nNext block: \n" + repetition + " / " + NumReps;
                }
                else
                {
                    infoForUser.GetComponent<TextMesh>().text = " Next block: \n" + repetition + " / " + NumReps;
                }

                showEyePointer = true;
                infoForUser.SetActive(true);                
                nextRepetitionFlag = false;
            }

            slowNotification.SetActive(false);
            //inaccurateNotification.SetActive(false);
            referenceObject.SetActive(true);
            headPointer.SetActive(true);
            //selectionMade = false;

            //if (condition == 2 || condition == 5 || condition == 6 || condition == 7)
            //{
            //    validationTarget.SetActive(true);
            //}


        }
        else
        {
            showEyePointer = true;
            infoForUser.SetActive(true);
            infoForUser.GetComponent<TextMesh>().text = "Experiment done\nThank you.";
            Debug.Log("Experiment is finished:");
        }

    }

    private void nextTrialUnityEditor()
    {



    }

    private void startTrial()
    {  //positionIsSet == true &&  runTime > pauseBeforeStimulus &&

        position = Mathf.FloorToInt(Random.value * NumTargets);

        //if (trainingIsOn)
        //{
        //    if (trainingTargetOrder < numberOfTrainingStimuli)
        //    {
        //        while (usedLocations.Contains(position))
        //        {
        //            position = Mathf.FloorToInt(Random.value * NumTargets);
        //        }

        //        usedLocations.Add(position);
        //    }

        //    else
        //    {
        //        trainingIsOn = false;
        //        usedLocations.Clear();
        //        trainingTargetOrder = 0;
        //        Debug.Log("Training is done");
        //    }

        //}
        //else
        {
            //positionIsSet = true;
            //runTime = 0.0f;
            // pauseBeforeStimulus = (100.0f + Mathf.Round(Random.value* 500.0f))/1000;


            while (usedLocations.Contains(position))
            {
                position = Mathf.FloorToInt(Random.value * NumTargets);
            }
            usedLocations.Add(position);

            //if (usedLocations.Count == NumTargets && repetition < NumReps)
            //{
            //    usedLocations.Clear();
            //    //repetition += 1;
            //    nextRepetitionFlag = true;
            //}

            //if (usedLocations.Count == NumTargets && repetition == NumReps)
            //{
            //    usedLocations.Clear();
            //    Debug.Log("Condition done:" + conditionIndex);
            //    conditionIndex = conditionIndex + 1;

            //    nextConditionFlag = true;


            //    //trainingIsOn = true;
            //    repetition = 1;
            //}

            Debug.Log("Starting Trial " + repetition);
            HideTargets();
            //arrow.transform.LookAt(targets[position].transform);
            targetAngle = (position % 8) * angleIncrement;
            Vector3 arrowRot = new Vector3(0, 0, targetAngle);
            arrow.transform.localRotation = Quaternion.Euler(arrowRot);
            arrow.SetActive(true);
            headPointer.SetActive(false);
  

            if (position < 8)
                targetDistCategory = 1;
            else
                targetDistCategory = 2;
        }

        showEyePointer = false;
        infoForUser.SetActive(false);
        referenceObject.SetActive(false);
        Debug.Log("Position: " + position);
        //currentPosition = GameObject.Find("ImageTargetChips").transform.position +  positions[position];
        target.SetActive(true);
        //target.transform.parent = GameObject.Find("ImageTargetChips").transform;
        // target.transform.position = positions[position];

        //target.SendMessage("setPosition", currentPosition);

        //string selectedTarget = "target" + position;
        //target.transform.position = GameObject.Find(selectedTarget).transform.position + new Vector3(0.0f, 0.0f, -0.005f);
        currentTarget = targets[position];
        target.transform.position = currentTarget.transform.position + new Vector3(0, 0, -0.005f);
        float targetDistance = Vector3.Distance(headPosition, target.transform.position);
        target.transform.localScale = targetScale * (targetDistance / 2.0f); // scale is 1 at 2m distance
        

        target_horisontal.GetComponent<Renderer>().enabled = true;
        target_vertical.GetComponent<Renderer>().enabled = true;
        //target.transform.position = new Vector3(targetOffset.x, targetOffset.y, targetOffset.z + targetDistance);
        currentTime = 0.0f;
        trialIsDone = false;
        isLogging = true;
        positionIsSet = false;
        selectionMade = false;
        cursor.GetComponent<WorldCursorTargetSelection>().isReady = false;
        preRefineAccuracy = 0;
        accuracyInDegrees = 0;
        stage1Time = 0;

        //if (trainingIsOn)
        //{

        //    trainingTargetOrder = trainingTargetOrder + 1;
        //}
        //else
        //{
        //    targetOrder = targetOrder + 1;
        //}
    }

    internal void checkPreRefinementAccuracy()
    {
        headPosition = Camera.main.transform.position;
        Vector3 vTarget = target.transform.position - headPosition;
        Vector3 vCursor = cursor.transform.position - headPosition;
        preRefineAccuracy = Vector3.Angle(vTarget, vCursor);

        if (!(condition == 1 || condition == 2) && preRefineAccuracy > accuracyLimit)
        {
            //click down is too far from target - refinement conditions only - abort trial
            result = "preRefineMiss";
            selectionMade = true;
        }
    }

    internal void checkAccuracyRequirement()
    {
        headPosition = Camera.main.transform.position;
        Vector3 vTarget = target.transform.position - headPosition;
        Vector3 vCursor = cursor.transform.position - headPosition;
        accuracyInDegrees = Vector3.Angle(vTarget, vCursor);

        //Debug.Log("Accuracy (deg): " + accuracyInDegrees);
        if (condition == 1 || condition == 2)
        {
            if (accuracyInDegrees <= accuracyLimit)
            {
                result = "hit";
            }
            else
            {
                result = "miss";
            }
        }
        else
        {
            if (accuracyInDegrees <= refinementAccuracyLimit)
            {
                result = "hit";
            }
            else
            {
                result = "refineMiss";
            }
        }

        selectionMade = true;
    }

    void endTrial()
    {
        target_horisontal.GetComponentInChildren<Renderer>().enabled = false;
        target_vertical.GetComponentInChildren<Renderer>().enabled = false;
        cursor.GetComponent<WorldCursorTargetSelection>().isReady = false;
        arrow.SetActive(false);

        
        if (usedLocations.Count == NumTargets && repetition < NumReps)
        {
            usedLocations.Clear();
            repetition += 1;
            nextRepetitionFlag = true;
        }

        if (usedLocations.Count == NumTargets && repetition == NumReps)
        {
            usedLocations.Clear();
            Debug.Log("Condition done:" + conditionIndex);
            conditionIndex = conditionIndex + 1;

            nextConditionFlag = true;
            
            repetition = 1;
        }

        nextTrial();
    }

    // Update is called once per frame
    void Update()
    {

        currentTime = currentTime + Time.deltaTime;
        runTime = runTime + Time.deltaTime;

        target.transform.LookAt(target.transform.position + Camera.main.transform.rotation * Vector3.forward, Vector3.up);

        if (isLogging)
        {
            string trng = repetition == 1 ? "true" : "false";

            Vector3 headRot = Camera.main.transform.rotation.eulerAngles;
            
            //dataLog.SendMessage("writeCalibToLog", userID + ";" + currentTime + ";false;" + condition + ";" + targetOrder + ";" + repetition + ";" + headPosition.x + ";" + headPosition.y + ";" + headPosition.z + ";" + target.transform.position.x + ";" + target.transform.position.y + ";" + target.transform.position.z + ";" + cursor.transform.position.x + ";" + cursor.transform.position.y + ";" + cursor.transform.position.z + ";" + GestureManager.Instance.IsManipulating + "\n");
            dataLog.SendMessage("writeCalibToLog", userID + ";" + currentTime + ";" + trng + ";" + condition + ";" + targetOrder + ";" + repetition + ";" + 
                                headPosition.x + ";" + headPosition.y + ";" + headPosition.z + ";" + headRot.x + ";" + headRot.y + ";" + headRot.z + ";" +
                                headPointer.transform.position.x + ";" + headPointer.transform.position.y + ";" + headPointer.transform.position.z + ";" + eyePointer.transform.position.x + ";" + eyePointer.transform.position.y + ";" + eyePointer.transform.position.z + ";" +
                                target.transform.position.x + ";" + target.transform.position.y + ";" + target.transform.position.z + ";" + 
                                cursor.transform.position.x + ";" + cursor.transform.position.y + ";" + cursor.transform.position.z + ";" +
                                cursorPlaneIntersect.transform.localPosition.x + ";" + cursorPlaneIntersect.transform.localPosition.y + ";" + cursorPlaneIntersect.transform.localPosition.z + ";" +
                                targetCentre.transform.position.x + ";" + targetCentre.transform.position.y  + ";" + targetCentre.transform.position.z + ";" +
                                referenceObject.transform.forward.x + ";" + referenceObject.transform.forward.y + ";" + referenceObject.transform.forward.z + ";" +
                                GestureManager.Instance.IsManipulating + "\n");


            if (currentTime > speedLimit)
            {

                currentTarget.GetComponent<MeshRenderer>().enabled = true;
                currentTarget.GetComponent<Renderer>().material.color = Color.red;

                usedLocations.RemoveAt(usedLocations.Count - 1);
                
                slowNotification.SetActive(true);
                dataLog.SendMessage("writeTrialToLog", userID + ";" + stage1Time + ";" + currentTime + ";" + trng + ";" + condition + ";" + position + ";" + targetAngle + ";" + targetDistCategory + ";" + targetOrder + ";" + repetition + ";timeout;" + preRefineAccuracy + ";0\n");

                endTrial();

                isLogging = false;
                dataLog.SendMessage("writeBuffer");



            }
            else if (selectionMade)
            {
                float holdTime = currentTime - stage1Time;
                if (!(condition == 1 || condition == 2) && holdTime < minRefinementTime)
                {
                    result = "noHold";

                    if (currentTarget != null)
                    {
                        currentTarget.GetComponent<MeshRenderer>().enabled = true;
                        currentTarget.GetComponent<Renderer>().material.color = Color.red;
                    }

                    if (usedLocations.Count > 0)
                    {
                        usedLocations.RemoveAt(usedLocations.Count - 1);
                    }
                }
                else if (result == "hit")
                {                    

                    if (currentTarget != null)
                    {
                        currentTarget.GetComponent<MeshRenderer>().enabled = true;
                        currentTarget.GetComponent<Renderer>().material.color = Color.green;
                    }

                }
                else if (result == "miss" || result == "preRefineMiss" || result == "refineMiss")
                {

                    if (currentTarget != null)
                    {
                        currentTarget.GetComponent<MeshRenderer>().enabled = true;
                        currentTarget.GetComponent<Renderer>().material.color = Color.red;
                    }
                    
                    if (usedLocations.Count > 0)
                    {
                        usedLocations.RemoveAt(usedLocations.Count - 1);
                      
                        //inaccurateNotification.SetActive(true);
                    }


                }
                dataLog.SendMessage("writeTrialToLog", userID + ";" + stage1Time + ";" + currentTime + ";" + trng + ";" + condition + ";" + position + ";" + targetAngle + ";" + targetDistCategory + ";" + targetOrder + ";" + repetition + ";" + result + ";" + preRefineAccuracy + ";" + accuracyInDegrees + "\n");

                Debug.Log("Hit result: " + result);
                endTrial();

                if (result == "hit")
                {
                    targetOrder += 1;
                }

                isLogging = false;
                dataLog.SendMessage("writeBuffer");
                selectionMade = false;
            }
        }


#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("M was pressed");
            nextTrialUnityEditor();

        }

#endif

    }

    //void stopLogging()
    //{
    //    isLogging = false;
    //    dataLog.SendMessage("writeBuffer");

    //}
    internal void setPreRefinementTime()
    {
        stage1Time = currentTime;
    }

}

// CODE FOR SHOWING ALL TRAINING CONDITIONS FIRST
/*
private void nextTrial()
{
    //if (Input.GetKeyDown(KeyCode.M))
    //  {
    //cursor.GetComponent<WorldCursorTargetSelection>().isReady = false;
    slowNotification.SetActive(false);
    Debug.Log("Measurered target number:" + targetOrder);

    if (condition == 2 || condition == 5 || condition == 6 || condition == 7)
    {
        validationTarget.SetActive(true);
    }


    if (trainingIsOn)
    {

        if (trainingTargetOrder < numberOfTrainingStimuli)
        {
            position = (int)Mathf.Round(Random.value * 8.0f);

            while (usedTrainingLocations.Contains(position))
            {
                position = (int)Mathf.Round(Random.value * 8.0f);
            }


            usedTrainingLocations.Add(position);
            referenceObject.SetActive(true);
            positionIsSet = true;
            runTime = 0.0f;

        }

        else
        {
            usedTrainingLocations.Clear();
            trainingTargetOrder = 0;

            referenceObject.SetActive(true);
            positionIsSet = true;
            runTime = 0.0f;

            if (conditionIndex < trainingConditions.Length)
            {
                condition = trainingConditions[conditionIndex];
                Debug.Log("Current condition" + condition);
                conditionIndex = conditionIndex + 1;
            }

            else
            {
                Debug.Log("Training is done");
                trainingIsOn = false;
                conditionIndex = 0;

            }
        }
    }
    else
    {

        if (conditionIndex < experimentConditions.Length)
        {
            condition = experimentConditions[conditionIndex];
            position = (int)Mathf.Round(Random.value * 8.0f);
            pauseBeforeStimulus = (100.0f + Mathf.Round(Random.value * 500.0f)) / 1000;


            while (usedLocations.Contains(position))
            {
                position = (int)Mathf.Round(Random.value * 8.0f);
            }


            usedLocations.Add(position);


            if (usedLocations.Count == positions.Count && repetition == 1)
            {
                usedLocations.Clear();
                repetition = 2;
            }

            if (usedLocations.Count == positions.Count && repetition == 2)
            {
                usedLocations.Clear();
                Debug.Log("Condition done:" + conditionIndex);
                repetition = 1;
                conditionIndex = conditionIndex + 1;
                //Change the location here
            }

            else
            {
                referenceObject.SetActive(true);
                positionIsSet = true;
                runTime = 0.0f;
            }
        }

        else
        {
            Debug.Log("Experiment is finished:");
        }

    }
}*/
