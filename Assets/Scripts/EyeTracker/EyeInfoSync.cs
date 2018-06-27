using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EyeInfoSync : NetworkBehaviour {

     
#if UNITY_EDITOR
    public PupilGazeTracker.GazeSource Gaze;
#endif
    private TextMesh DebugText;
    private RectTransform m_Trans;
    private Vector2 m_dim = new Vector2(1, 1);
    private GameObject PupilGaze;
    [SyncVar]
    public int m_participantNum;
    
    [SyncVar]
    public bool m_head;

    [SyncVar]
    public int m_conditionNum;

    [SyncVar]
    public bool m_sync;

    [SyncVar]
    public bool m_showReticle;

    private void Awake()
    {
        m_Trans = GetComponent<RectTransform>();
#if UNITY_EDITOR
        GameObject go = GameObject.Find("EyeCanvas");
        if(go != null)
            m_dim = new Vector2(go.GetComponent<RectTransform>().rect.width, go.GetComponent<RectTransform>().rect.height);

        m_participantNum = PupilGazeTracker.Instance.Participant;
        m_head = PupilGazeTracker.Instance.Head;
        m_conditionNum = PupilGazeTracker.Instance.Condition;
        m_sync = PupilGazeTracker.Instance.Sync;
#else
        GameObject dt = GameObject.Find("DebugText");
        if (dt != null) {
            DebugText = dt.GetComponent<TextMesh>();
            DebugText.text = "Ready";
        }

#endif
    }

    void Update()
    {
#if UNITY_EDITOR
        Vector2 g = PupilGazeTracker.Instance.GetEyeGaze(Gaze);
            m_Trans.localPosition = new Vector3((g.x - 0.5f) * m_dim.x, (g.y - 0.5f) * m_dim.y, 0);

        m_participantNum = PupilGazeTracker.Instance.Participant;
        m_head = PupilGazeTracker.Instance.Head;
        m_conditionNum = PupilGazeTracker.Instance.Condition;
        m_sync = PupilGazeTracker.Instance.Sync;
        m_showReticle = PupilGazeTracker.Instance.ShowReticle;
#else
        if (DebugText != null)
        {
            DebugText.text = m_Trans.position.x.ToString() + ", " + m_Trans.position.y.ToString();
        }
        //Debug.Log("m_Trans: " + m_Trans.position);
#endif
    }



}
