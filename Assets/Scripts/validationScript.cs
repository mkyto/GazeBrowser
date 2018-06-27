using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class validationScript : MonoBehaviour {


    List<float> x_values;
    List<float> y_values;
    float sampleSizeTime;
    float currentTime;
    GameObject validationTarget;
    List<Vector2> targetPositions;
    int[] x_targetPositions;
    int[] y_targetPositions;
    int targetNumber = 0;
    //EyeGazeRenderer gz;
    RectTransform gaze;
    Vector2 gazePoint;

    float hololensFOV_ver = 17.5f;
    float hololensFOV_hor = 30.0f;

    float res_ver = 300.0f; //455.0f; //540.0f;
    float res_hor = 810.0f; //960.0f;
    float pixelInAngle;

    public float accuracyLimit;

    float accuracyLimitScaler = 1.3f;
    GameObject dataLog;
    // Use this for initialization
    void Start()
    {

        accuracyLimit = accuracyLimit * accuracyLimitScaler;

        dataLog = GameObject.Find("Datalogger");
        targetNumber = 0;

        //gz = GameObject.Find("Left").GetComponent<EyeGazeRenderer>();

        //gaze = gz.gaze;


        pixelInAngle = hololensFOV_hor / res_hor; 


        x_values = new List<float>();
        y_values = new List<float>();
        targetPositions = new List<Vector2>();

        currentTime = 4.0f;
        sampleSizeTime = 3.0f;
        validationTarget = GameObject.Find("ValidationTarget");
        validationTarget.GetComponent<MeshRenderer>().enabled = false;
        validationTarget.GetComponent<Collider>().enabled = false;
        x_targetPositions = new int[3];
        y_targetPositions = new int[3];

        targetPositions.Add(new Vector2(0.0f, 0.0f));
        targetPositions.Add(new Vector2(0.0f, res_ver/2 - 40.0f));
        targetPositions.Add(new Vector2(res_hor / 2 - 20.0f, res_ver / 2 - 40.0f));
        targetPositions.Add(new Vector2(res_hor / 2 - 20.0f, 0.0f));
        targetPositions.Add(new Vector2(res_hor / 2 - 20.0f, -res_ver / 2 + 20.0f));
        targetPositions.Add(new Vector2(0.0f, -res_ver / 2 + 20.0f));
        targetPositions.Add(new Vector2(-res_hor / 2 + 20.0f, -res_ver / 2 + 20.0f));
        targetPositions.Add(new Vector2(-res_hor / 2 + 20.0f, 0.0f));
        targetPositions.Add(new Vector2(-res_hor / 2 + 20.0f, res_ver / 2 - 40.0f));

    }

    
    

    
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("M was pressed");
            Debug.Log("Mesurered target number:" + targetNumber );
            currentTime = 0.0f;
        }

       // Debug.Log("Gaze x: " + gaze.localPosition.x);
        //Debug.Log("Gaze y: " + gaze.localPosition.y);

        if (currentTime < sampleSizeTime && targetNumber < sceneCreationTargetSelection.NumTargets) {
            //x_values.Add(gaze.localPosition.x); // Gaze x value
            //y_values.Add(gaze.localPosition.y); // Gaze y value
            Debug.Log(currentTime + ";" + targetNumber + ";" + targetPositions[targetNumber].x + ";" + targetPositions[targetNumber].y + ";" + gaze.localPosition.x + ";" + gaze.localPosition.y);
            dataLog.SendMessage("writeCalibToLog", currentTime + ";" + targetNumber + ";" + targetPositions[targetNumber].x + ";" + targetPositions[targetNumber].y + ";" + gaze.localPosition.x + ";" + gaze.localPosition.y);
             currentTime = currentTime + Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("N was pressed");
            if (targetNumber == 8)
            {

            }

            else
            {
                targetNumber = targetNumber + 1;
            }

            validationTarget.transform.localPosition = new Vector3(targetPositions[targetNumber].x, targetPositions[targetNumber].y, 0);
            Debug.Log("Current target number: " + targetNumber);



            computeAccuracy(targetNumber);
            x_values.Clear();
            y_values.Clear();

            

            //if (targetNumber > 8) { 
//validationTarget.GetComponent<MeshRenderer>().enabled = false;
            //validationTarget.GetComponent<Collider>().enabled = false;
            //Debug.Log("Validation done");
            //}
        }


        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("V was pressed");

            if(targetNumber == 0)
            {
                validationTarget.GetComponent<MeshRenderer>().enabled = true;
                validationTarget.GetComponent<Collider>().enabled = true;
                
            }


            if (targetNumber >= 8)
            {
                validationTarget.GetComponent<MeshRenderer>().enabled = false;
                validationTarget.GetComponent<Collider>().enabled = false;

            }


        }
    }


    public void computeAccuracy(int targetNumber)
    {
        float temp = 0.0f;
        for (int i = 0; i < x_values.Count; i++)
        {
            temp = temp + x_values[i];

        }

        float x_average = temp / x_values.Count;
        float x_offset = (x_average - targetPositions[targetNumber].x) * pixelInAngle;




        temp = 0.0f;

        for (int i = 0; i < y_values.Count; i++)
        {
            temp = temp + y_values[i];

        }

        float y_average = temp / y_values.Count;
        float y_offset = (y_average - targetPositions[targetNumber].y) * pixelInAngle;


        temp = 0.0f;

        for (int i = 0; i < x_values.Count; i++)
        {
            temp = temp + (x_values[i] - x_average) * (x_values[i] - x_average);

        }

        float x_std = (temp / x_values.Count) * pixelInAngle;



        temp = 0.0f;

        for (int i = 0; i < y_values.Count; i++)
        {
            temp = temp + (y_values[i] - y_average) * (y_values[i] - y_average);

        }

        float y_std = (temp / y_values.Count) * pixelInAngle;



        Debug.Log("x offset: " + x_average);
        Debug.Log("x std: " + x_std);

        Debug.Log("y offset: " + y_average);
        Debug.Log("y std: " + y_std);


    }
        
}


/*
            
            */




