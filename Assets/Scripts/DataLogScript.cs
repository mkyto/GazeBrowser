using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if WINDOWS_UWP
using Windows.Storage;
using System.Threading.Tasks;
using System;
#endif

public class DataLogScript : MonoBehaviour
{
#if WINDOWS_UWP
    StorageFolder selectionResultsPath;
    StorageFile textFileForWrite;
    StorageFile textFileTrialLog;
    //private async void writeToLog(lineToWrite)
#endif
#if UNITY_EDITOR
    string selectionResultsPath;
#endif
    string selectionTaskResultsFolder = "SelectionTaskResults";
    string selectionFileName;
    string trialLogFileName;
    int userID;
    string bufferString;
    string bufferTrialLog;
    //string CalibFolder = "CalibrationResults";
    //string CalibPath;
    //string calibFileName;
    GameObject sceneCreation;


    // Use this for initialization
    void Start()
    {
        //Application.runInBackground = true;
        sceneCreation = GameObject.Find("sceneCreation");
            
            

        //userID = GameObject.Find("sceneCreation").GetComponent<sceneCreationTargetSelection>().userID;
        bufferString = "";
        bufferTrialLog = "";
        

#if UNITY_EDITOR
        selectionResultsPath = Path.Combine(Application.dataPath, selectionTaskResultsFolder);
#endif

        

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setParticipantID(int id)
    {
        userID = id;
        selectionFileName = "User_" + userID + "_selectionTask.txt";
        trialLogFileName = "User_" + userID + "_trialLog.txt";

#if WINDOWS_UWP
        selectionResultsPath = ApplicationData.Current.LocalFolder; 
        initializeFile(selectionFileName);
        initializeTrialLogFile(trialLogFileName);
#endif
        writeTrialToLog("UserID;preRefinementTime;totalTime;trainingIsOn;condition;targetPosition;targetAngle;targetDistance;trialOrder;repetition;result;preRefineAngle;finalAngle\n");
        writeCalibHeaderToLog("userID;timeStamp;trainingIsOn;condition;trialOrder;repetition;headPos_x;headPos_y;headPos_z;headRot_x;headRot_y;headRot_z;headPointer_x;headPointer_y;headPointer_z;eyePointer_x;eyePointer_y;eyePointer_z;targetPos_x;targetPos_y;targetPos_z;" +
            "cursor_x;cursor_y;cursor_z;cursorPlane_x;cursorPlane_y;cursorPlane_z;planeCentre_x;planeCentre_y;planeCentre_z;planeNormal_x;planeNormal_y;planeNormal_z;isRefining\n");
        
        /*
#if WINDOWS_UWP

                //Get local folder
                selectionResultsPath = ApplicationData.Current.LocalFolder;

                //Create file
                  Task task = new Task(
                    async () =>
                    {                              
                       textFileForWrite = await selectionResultsPath.CreateFileAsync(selectionFileName, CreationCollisionOption.OpenIfExists);


                    });
                task.Start();
                task.Wait();
#endif
        */

    }

    public void writeTrialToLog(string lineToWrite)
    {
        bufferTrialLog = bufferTrialLog + lineToWrite;
    }

    public void writeCalibToLog(string lineToWrite)
    {
        if (sceneCreation.GetComponent<sceneCreationTargetSelection>().isLogging) {
            bufferString = bufferString + lineToWrite;
        }
#if UNITY_EDITOR
        string fileToWrite = selectionResultsPath + "/" + selectionFileName;
        using (StreamWriter sw = new StreamWriter(fileToWrite, true))
        {
            //sw.Write("User" + menuScriptRef.userID + " - Logging calibration accuracy:\r\nTime(sec);");
            sw.Write(lineToWrite);
            sw.Write("\r");
        }

#endif
    }

    public void writeCalibHeaderToLog(string lineToWrite)
    {
        bufferString = bufferString + lineToWrite;
    }
    


    public void writeBuffer()
    {
#if WINDOWS_UWP
        //var buffer = Windows.Security.Cryptography.CryptographicBuffer.ConvertStringToBinary(
        //bufferString, Windows.Security.Cryptography.BinaryStringEncoding.Utf8);
        
         Task task = new Task(

                    async () =>
                    {                              
                        //await FileIO.WriteBufferAsync(textFileForWrite, buffer);
                     await FileIO.AppendTextAsync(textFileForWrite, bufferString);    
                    });
                task.Start();
                task.Wait();     
        
        task = new Task(

                    async () =>
                    {                                   
                     await FileIO.AppendTextAsync(textFileTrialLog, bufferTrialLog);
                    });
                task.Start();
                task.Wait();           
#endif
        bufferString = "";
        bufferTrialLog = "";
    }



#if WINDOWS_UWP

    private async void initializeFile(string selectionFileName) {
        textFileForWrite = await selectionResultsPath.CreateFileAsync(selectionFileName, CreationCollisionOption.OpenIfExists);  
  
    } 

   private async void initializeTrialLogFile(string selectionFileName) {
        textFileTrialLog = await selectionResultsPath.CreateFileAsync(trialLogFileName, CreationCollisionOption.OpenIfExists);  
  
    } 


    //private async void writeToLog(string lineToWrite) {    
    //try{
    //await FileIO.AppendTextAsync(textFileForWrite, lineToWrite); }
    //catch (FileNotFoundException){
    //// For example, handle file not found 
    //}
    //} 

#endif

}

