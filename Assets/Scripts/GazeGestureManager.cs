using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GazeGestureManager : MonoBehaviour
{
    public static GazeGestureManager Instance { get; private set; }

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }

    GestureRecognizer recognizer;
    RaycastHit hit;
    GameObject focusedMoveButton;

    // Use this for initialization
    void Start()
    {
        Instance = this;
        //focusedMoveButton = new GameObject();
        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.NavigationUpdatedEvent += NavigationUpdatedEvent;
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            // Send an OnSelect message to the focused object and its ancestors.
            if (FocusedObject != null)
            {

                Debug.Log("Klikkaus lahetetty: " + FocusedObject.name);
                if (FocusedObject.tag == "moveButton")
                {
                    focusedMoveButton = FocusedObject;
                    Debug.Log("Painallus lahetetty");
                    focusedMoveButton.SendMessage("OnSelect");
                }
                else
                {
                    if (focusedMoveButton != null)
                    {
                        focusedMoveButton.SendMessage("deSelect");
                    }
                }
            }
            else
            {
                if (focusedMoveButton != null)
                {
                    focusedMoveButton.SendMessage("deSelect");
                }
                else
                {
                    Debug.Log("focusedMoveButton is null");
                }
            }

        };
        recognizer.StartCapturingGestures();
    }

    // Update is called once per frame
    void Update()
    {

        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;
            //Debug.Log("Osui objektiin: " + hitInfo.collider.gameObject.name);
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
        }

        // If the focused object changed this frame,
        // start detecting fresh gestures again.
        if (FocusedObject != oldFocusObject)
        {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }

#if UNITY_EDITOR
        //Debug.Log("Editor");

if (Input.GetMouseButtonDown(0))
{
    if(FocusedObject != null) { 

        Debug.Log("Klikkaus lahetetty: " + FocusedObject.name);
        if (FocusedObject.tag == "moveButton")
        {
            focusedMoveButton = FocusedObject;
            Debug.Log("Painallus lahetetty");
            focusedMoveButton.SendMessage("OnSelect");
        }
        else
        {
            if (focusedMoveButton != null)
            {
                focusedMoveButton.SendMessage("deSelect");
            }
        }
    }
    else
    {
        if (focusedMoveButton != null)
        {
            focusedMoveButton.SendMessage("deSelect");
        }
        else
            {
                Debug.Log("focusedMoveButton on null");
            }
        }
    }
    #endif
}

void NavigationUpdatedEvent(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
{
float t = normalizedOffset.x * 0.5f + 0.5f;
Debug.Log("Jotain tapahtui");
}
}
 