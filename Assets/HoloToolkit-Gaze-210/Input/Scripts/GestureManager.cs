using UnityEngine;
using UnityEngine.VR.WSA.Input;

namespace Academy.HoloToolkit.Unity
{
    public class GestureManager : Singleton<GestureManager>
    {
        //GameObject cube1;
       // GameObject cube2;

        // Tap and Navigation gesture recognizer.
        public GestureRecognizer NavigationRecognizer { get; private set; }

        // Manipulation gesture recognizer.
        public GestureRecognizer ManipulationRecognizer { get; private set; }

        // Currently active gesture recognizer.
        public GestureRecognizer ActiveRecognizer { get; private set; }

        public bool IsNavigating { get; private set; }

        public Vector3 NavigationPosition { get; private set; }

        public bool IsManipulating { get;  set; }

        public Vector3 ManipulationPosition { get; private set; }

        GameObject cursorObject;
       public bool isTapped;

        void Awake()
        {
            /* TODO: DEVELOPER CODING EXERCISE 2.b */

            cursorObject = GameObject.Find("Cursor");
            //cube1 = GameObject.Find("TestCube1");
            //cube2 = GameObject.Find("TestCube2");


            // Instantiate the ManipulationRecognizer.
            ManipulationRecognizer = new GestureRecognizer(); //For hand gesture

            // Add the ManipulationTranslate GestureSetting to the ManipulationRecognizer's RecognizableGestures.
            ManipulationRecognizer.SetRecognizableGestures(
                GestureSettings.ManipulationTranslate);

            // Register for the Manipulation events on the ManipulationRecognizer.
            ManipulationRecognizer.ManipulationStartedEvent += ManipulationRecognizer_ManipulationStartedEvent;
            ManipulationRecognizer.ManipulationUpdatedEvent += ManipulationRecognizer_ManipulationUpdatedEvent;
            ManipulationRecognizer.ManipulationCompletedEvent += ManipulationRecognizer_ManipulationCompletedEvent;
            ManipulationRecognizer.ManipulationCanceledEvent += ManipulationRecognizer_ManipulationCanceledEvent;

            // 2.b: Instantiate the NavigationRecognizer.
            NavigationRecognizer = new GestureRecognizer(); // For clicker

            // 2.b: Add Tap and NavigationX GestureSettings to the NavigationRecognizer's RecognizableGestures.
            // NavigationRecognizer.SetRecognizableGestures(
            //     GestureSettings.Tap |
            //    GestureSettings.NavigationX);
            NavigationRecognizer.SetRecognizableGestures(GestureSettings.NavigationX | GestureSettings.NavigationY);

            // 2.b: Register for the TappedEvent with the NavigationRecognizer_TappedEvent function.
            //NavigationRecognizer.TappedEvent += NavigationRecognizer_TappedEvent;
            // 2.b: Register for the NavigationStartedEvent with the NavigationRecognizer_NavigationStartedEvent function.
            NavigationRecognizer.NavigationStartedEvent += NavigationRecognizer_NavigationStartedEvent;
            // 2.b: Register for the NavigationUpdatedEvent with the NavigationRecognizer_NavigationUpdatedEvent function.
            NavigationRecognizer.NavigationUpdatedEvent += NavigationRecognizer_NavigationUpdatedEvent;
            // 2.b: Register for the NavigationCompletedEvent with the NavigationRecognizer_NavigationCompletedEvent function. 
            NavigationRecognizer.NavigationCompletedEvent += NavigationRecognizer_NavigationCompletedEvent;
            // 2.b: Register for the NavigationCanceledEvent with the NavigationRecognizer_NavigationCanceledEvent function. 
            NavigationRecognizer.NavigationCanceledEvent += NavigationRecognizer_NavigationCanceledEvent;




            isTapped = false;

            ResetGestureRecognizers();

            //if(GameObject.Find("sceneCreation").GetComponent<sceneCreationTargetSelection>().condition == 4) {

               // Transition(NavigationRecognizer);
            //}

            //Transition(ManipulationRecognizer);
        }

        private void Update()
        {
           // Debug.Log("Gesture Manager, Is manipulating:" + IsManipulating);
        }

        void OnDestroy()
        {
            // 2.b: Unregister the Tapped and Navigation events on the NavigationRecognizer.
            //NavigationRecognizer.TappedEvent -= NavigationRecognizer_TappedEvent;

            NavigationRecognizer.NavigationStartedEvent -= NavigationRecognizer_NavigationStartedEvent;
            NavigationRecognizer.NavigationUpdatedEvent -= NavigationRecognizer_NavigationUpdatedEvent;
            NavigationRecognizer.NavigationCompletedEvent -= NavigationRecognizer_NavigationCompletedEvent;
            NavigationRecognizer.NavigationCanceledEvent -= NavigationRecognizer_NavigationCanceledEvent;

            // Unregister the Manipulation events on the ManipulationRecognizer.
            ManipulationRecognizer.ManipulationStartedEvent -= ManipulationRecognizer_ManipulationStartedEvent;
            ManipulationRecognizer.ManipulationUpdatedEvent -= ManipulationRecognizer_ManipulationUpdatedEvent;
            ManipulationRecognizer.ManipulationCompletedEvent -= ManipulationRecognizer_ManipulationCompletedEvent;
            ManipulationRecognizer.ManipulationCanceledEvent -= ManipulationRecognizer_ManipulationCanceledEvent;
        }

        /// <summary>
        /// Revert back to the default GestureRecognizer.
        /// </summary>
        public void ResetGestureRecognizers()
        {
            // Default to the manipulation gestures.
            //Transition(ManipulationRecognizer);
        }

        /// <summary>
        /// Transition to a new GestureRecognizer.
        /// </summary>
        /// <param name="newRecognizer">The GestureRecognizer to transition to.</param>
        public void Transition(GestureRecognizer newRecognizer)
        {
            if (newRecognizer == null)
            {
                return;
            }

            if (ActiveRecognizer != null)
            {
                if (ActiveRecognizer == newRecognizer)
                {
                    return;
                }

                ActiveRecognizer.CancelGestures();
                ActiveRecognizer.StopCapturingGestures();
            }

            newRecognizer.StartCapturingGestures();
            ActiveRecognizer = newRecognizer;
        }
        //public void Transition(GestureRecognizer newRecognizer)
        //{
        //    if (newRecognizer != null)
        //    {

        //        if (ActiveRecognizer != null)
        //        {
        //            if (ActiveRecognizer != newRecognizer)
        //            {

        //                ActiveRecognizer.CancelGestures();
        //                ActiveRecognizer.StopCapturingGestures();
        //            }
        //            else
        //            {
        //                //Debug.Log("Active is the same as old one");
        //            }
        //        }
        //        else
        //        {
        //            //Debug.Log("Old rec is null");
        //        }

        //        if (!newRecognizer.IsCapturingGestures())
        //        {
        //            newRecognizer.StartCapturingGestures();
        //        }
        //        ActiveRecognizer = newRecognizer;
        //    }
        //    else
        //    {
        //        //Debug.Log("New rec is null");
        //    }


        //    //Debug.Log("ActiveRecognizer: " + ActiveRecognizer == NavigationRecognizer + 
        //    //    "\nNavigation Caputuring: " + NavigationRecognizer.IsCapturingGestures() +
        //    //    "\nManipulation Caputuring: " + ManipulationRecognizer.IsCapturingGestures());
        //}




        /*void NavigationUpdatedEvent(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            Debug.Log("ClickerPosition updated");
            GameObject.Find("ClickerObject").transform.Translate(normalizedOffset);
            // 2.b: Set IsNavigating to be true.
            IsNavigating = true;

            // 2.b: Set NavigationPosition to be relativePosition.
            NavigationPosition = normalizedOffset;

        }*/

        private void NavigationRecognizer_NavigationStartedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            // 2.b: Set IsNavigating to be true.
            //IsNavigating = true;

            // 2.b: Set NavigationPosition to be relativePosition.
            //NavigationPosition = relativePosition;
            //cube1.transform.position = cursorObject.transform.position;

            if (source == InteractionSourceKind.Controller)
            {


                IsManipulating = true;
                //GameObject.Find("ClickerObject").transform.Translate(-0.3f, 0.0f, 0.0f);
                ManipulationPosition = relativePosition;

                Debug.Log("Clicker navigation started event");
                cursorObject.SendMessage("PerformManipulationStartClicker", relativePosition);
            }
        }

        private void NavigationRecognizer_NavigationUpdatedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            //Debug.Log("Clicker updated");
            // 2.b: Set IsNavigating to be true.
            //IsNavigating = true;
            // 2.b: Set NavigationPosition to be relativePosition.
            //NavigationPosition = relativePosition;
            //cube2.transform.position = cursorObject.transform.position;

            if (source == InteractionSourceKind.Controller)
            {
                IsManipulating = true;

                ManipulationPosition = relativePosition;

                Debug.Log("Clicker navigation updated event");

                cursorObject.SendMessage("PerformManipulationUpdateClicker", relativePosition);
            }
            
        }

        private void NavigationRecognizer_NavigationCompletedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            // 2.b: Set IsNavigating to be false.
            //IsNavigating = false;


            if (source == InteractionSourceKind.Controller)
            {

                Debug.Log("Clicker navigation completed event");
                IsManipulating = false;
            }
        }

        private void NavigationRecognizer_NavigationCanceledEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
        {
            // 2.b: Set IsNavigating to be false.
            // IsNavigating = false;

            if (source == InteractionSourceKind.Controller)
            {
                IsManipulating = false;
            }
        }

        private void ManipulationRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
          //  if (HandsManager.Instance.FocusedGameObject != null)
            //{
                IsManipulating = true;
            //GameObject.Find("ClickerObject").transform.Translate(-0.3f, 0.0f, 0.0f);
            ManipulationPosition = position;

                Debug.Log("Manipulation started event");
            cursorObject.SendMessage("PerformManipulationStart", position);

            //HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationStart", position);
            //}
        }

        private void ManipulationRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            //  if (HandsManager.Instance.FocusedGameObject != null)
            //{
            IsManipulating = true;

            ManipulationPosition = position;

            Debug.Log("Manipulation updated event");

            cursorObject.SendMessage("PerformManipulationUpdate", position);
           
            //HandsManager.Instance.FocusedGameObject.SendMessageUpwards("PerformManipulationUpdate", position);
            //  }
            //}
        }
        private void ManipulationRecognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            IsManipulating = false;
        }

        private void ManipulationRecognizer_ManipulationCanceledEvent(InteractionSourceKind source, Vector3 position, Ray ray)
        {
            IsManipulating = false;
        }

        //private void NavigationRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray ray)
        //{
        //    isTapped = true;
        //    // if (isClicked)
        //    // {
        //   // GameObject.Find("ClickerObject").transform.Translate(0.1f, 0.0f, 0.0f);
        //    //}
        //        /*GameObject focusedObject = InteractibleManager.Instance.FocusedGameObject;

        //        if (focusedObject != null)
        //        {
        //            focusedObject.SendMessageUpwards("OnSelect");
        //        }*/



        //    }
    }
}