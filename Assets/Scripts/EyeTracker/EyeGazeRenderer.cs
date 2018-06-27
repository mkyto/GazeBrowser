
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class EyeGazeRenderer : MonoBehaviour
{
#if UNITY_EDITOR
    public PupilGazeTracker.GazeSource Gaze;
#endif
    private GameObject eyeTracker;
    private RectTransform m_Trans;
    private Vector2 m_dim = new Vector2(1, 1);
    private EyeInfoSync eyeInfoSync;
    public int m_participantNum;
    public bool m_head;
    public int m_conditionNum;
    public bool m_sync;
    public bool m_showReticle;
    GameObject sceneCreation;

    void Awake() { 
        m_Trans = GetComponent<RectTransform>();
        GameObject go = GameObject.Find("EyeCanvas");
        sceneCreation = GameObject.Find("sceneCreation");

        //sceneCreation.GetComponent<simpleRadialSceneCreation>().condition = 1;
        m_dim = new Vector2(go.GetComponent<RectTransform>().rect.width, go.GetComponent<RectTransform>().rect.height);

        eyeTracker = GameObject.Find("EyeTrackerHandler(Clone)");
        if (eyeTracker != null)
            eyeInfoSync = eyeTracker.GetComponent<EyeInfoSync>();

        m_sync = false;
        m_showReticle = false;
    }

	void Update() {
        Vector2 g;
#if UNITY_EDITOR
        g = PupilGazeTracker.Instance.GetEyeGaze (Gaze);
        m_Trans.localPosition = new Vector3((g.x - 0.5f) * m_dim.x, (g.y - 0.5f) * m_dim.y, 0);
#else
        if(eyeTracker != null) {
            RectTransform rect = eyeTracker.GetComponent<RectTransform>();
            m_Trans.localPosition = rect.position;                   
        } else {
            eyeTracker = GameObject.Find("EyeTrackerHandler(Clone)");
        }

        if (eyeInfoSync != null) {
            m_participantNum = eyeInfoSync.m_participantNum;
            m_head = eyeInfoSync.m_head;
            m_conditionNum = eyeInfoSync.m_conditionNum;
        sceneCreation.GetComponent<simpleRadialSceneCreation>().condition = m_conditionNum;
        sceneCreation.SendMessage("setRecognizer", m_conditionNum);
            m_sync = eyeInfoSync.m_sync;
            m_showReticle = eyeInfoSync.m_showReticle;
        } else {
            if (eyeTracker != null)
            {
                eyeInfoSync = eyeTracker.GetComponent<EyeInfoSync>();
            }
        }
#endif

    }
}