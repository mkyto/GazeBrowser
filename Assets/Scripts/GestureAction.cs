using Academy.HoloToolkit.Unity;
using UnityEngine;

/// <summary>
/// GestureAction performs custom actions based on
/// which gesture is being performed.
/// </summary>
public class GestureAction : MonoBehaviour
{
    [Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 10.0f;
    public float movementGain;
    public float movementGainClicker;
    public float movementGainHead;
    private Vector3 manipulationPreviousPosition;

    GameObject sceneCreation;
    WorldCursorTargetSelection worldCursor;
    int condition;
    //GameObject cube1;
    //GameObject cube2;


    private float rotationFactor;
    Vector3 cameraDirection;

    private void Start()
    {
        //GestureManager.Instance.Transition(GestureManager.Instance.ManipulationRecognizer);
        sceneCreation = GameObject.Find("sceneCreation");
        worldCursor = this.gameObject.GetComponent<WorldCursorTargetSelection>();

        //cube1 = GameObject.Find("TestCube1");
        //cube2 = GameObject.Find("TestCube2");

        movementGain = 1.4f;
        movementGainClicker = 0.35f;
        movementGainHead = 0.5f;
    }


    void Update()
    {

        condition = sceneCreation.GetComponent<simpleRadialSceneCreation>().condition;
        //PerformRotation();
        //cameraDirection = Camera.main.t
    }

    private void PerformRotation()
    {
        /*  if (GestureManager.Instance.IsNavigating &&
              (!ExpandModel.Instance.IsModelExpanded ||
              (ExpandModel.Instance.IsModelExpanded && HandsManager.Instance.FocusedGameObject == gameObject)))*/

          if (GestureManager.Instance.IsNavigating)
        {

            
            /* TODO: DEVELOPER CODING EXERCISE 2.c */

            // 2.c: Calculate rotationFactor based on GestureManager's NavigationPosition.X and multiply by RotationSensitivity.
            // This will help control the amount of rotation.
            // rotationFactor = GestureManager.Instance.NavigationPosition.x * RotationSensitivity;

            // 2.c: transform.Rotate along the Y axis using rotationFactor.


            //transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));




            Debug.Log("Cursor Rotated");
        }
    }

    void PerformManipulationStart(Vector3 position)
    {

        if(condition == 3 || condition == 6) { 
        manipulationPreviousPosition = position;
        }
    }

    void PerformManipulationUpdate(Vector3 position)
    {
        if (condition == 3 || condition == 6)
        {
            //Debug.Log("Cursor moved");
            if (GestureManager.Instance.IsManipulating)
            {
                /* TODO: DEVELOPER CODING EXERCISE 4.a */

                Vector3 moveVector = Vector3.zero;

                // 4.a: Calculate the moveVector as position - manipulationPreviousPosition.
                moveVector = position - manipulationPreviousPosition;

                // 4.a: Update the manipulationPreviousPosition with the current position.
                manipulationPreviousPosition = position;
                Vector3 movement = movementGain * moveVector;
                Debug.Log("Cursor moved by Hand: " + movement);
                // 4.a: Increment this transform's position by the moveVector.
                transform.position += movement;
            }
        }
    }


    void PerformManipulationStartClicker(Vector3 position)
    {
        Debug.Log("StartClickerNav: " + position);
        if (condition == 4 || condition == 7)
        {
            manipulationPreviousPosition = position;
        }
    }

    void PerformManipulationStartHead()
    { 
        if (condition == 5 || condition == 8)
        {
            manipulationPreviousPosition = Camera.main.transform.position + Camera.main.transform.forward * 2;
        }
    }

    void PerformManipulationUpdateClicker(Vector3 position)
    {
        Debug.Log("UpdateClickerNav: " + position);
        if (condition == 4 || condition == 7)
        {
            //Debug.Log("Cursor moved");
            if (GestureManager.Instance.IsManipulating)
            {
                /* TODO: DEVELOPER CODING EXERCISE 4.a */

                Vector3 moveVector = Vector3.zero;

                // 4.a: Calculate the moveVector as position - manipulationPreviousPosition.
                moveVector = position - manipulationPreviousPosition;

                // 4.a: Update the manipulationPreviousPosition with the current position.
                manipulationPreviousPosition = position;
                Vector3 movement = movementGainClicker * moveVector;
                Debug.Log("Cursor moved by Clicker: " + movement);
                // 4.a: Increment this transform's position by the moveVector.
                transform.position += movement;
            }
        }
    }

    internal void PerformManipulationUpdateHead()
    {
        if (condition == 5 || condition == 8)
        {
            if (GestureManager.Instance.IsManipulating)
            {
                Debug.Log("Cursor moved by head");
                Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * 2;

                Vector3 moveVector = position - manipulationPreviousPosition;
                manipulationPreviousPosition = position;
                Vector3 movement = movementGainHead * moveVector;
                transform.position += movement;

                //cube1.transform.position = transform.position;
                //cube2.transform.position = manipulationPreviousPosition;
            }
        }

    }

}