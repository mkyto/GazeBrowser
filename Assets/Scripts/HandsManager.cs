using UnityEngine.VR.WSA.Input;
using UnityEngine;
using HoloToolkit.Unity;

namespace Academy.HoloToolkit.Unity
{
    /// <summary>
    /// HandsManager keeps track of when a hand is detected.
    /// </summary>
    public class HandsManager : Singleton<HandsManager>
    {
        [Tooltip("Audio clip to play when Finger Pressed.")]
        public AudioClip FingerPressedSound;
        private AudioSource audioSource;

        public float HandGuidanceThreshold = 0.2f;

        GameObject target_vertical;
        GameObject target_horisontal;
        GameObject sceneCreation;
        sceneCreationTargetSelection targetSelection;
        GameObject cursor;

        /// <summary>
        /// Tracks the hand detected state.
        /// </summary>
        public bool HandDetected
        {
            get;
            private set;
        }

        // Keeps track of the GameObject that the hand is interacting with.
        public GameObject FocusedGameObject { get; private set; }

        void Awake()
        {
            EnableAudioHapticFeedback();

            InteractionManager.SourceDetected += InteractionManager_SourceDetected;
            //InteractionManager.SourceUpdated += InteractionManager_SourceUpdated;
            InteractionManager.SourceLost += InteractionManager_SourceLost;

            /* TODO: DEVELOPER CODE ALONG 2.a */

            // 2.a: Register for SourceManager.SourcePressed event.
            InteractionManager.SourcePressed += InteractionManager_SourcePressed;

            // 2.a: Register for SourceManager.SourceReleased event.
            InteractionManager.SourceReleased += InteractionManager_SourceReleased;

            // 2.a: Initialize FocusedGameObject as null.
            FocusedGameObject = null;

            target_vertical = GameObject.Find("VerticalComponent");
            target_horisontal = GameObject.Find("HorisontalComponent");
            sceneCreation = GameObject.Find("sceneCreation");
            //mikko commented out
            //targetSelection = sceneCreation.GetComponent<sceneCreationTargetSelection>();
            targetSelection = sceneCreation.GetComponent<sceneCreationTargetSelection>();
            cursor = GameObject.Find("Cursor");


            Debug.Log("HandManagerStarted");
        }

        private void EnableAudioHapticFeedback()
        {
            // If this hologram has an audio clip, add an AudioSource with this clip.
            if (FingerPressedSound != null)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                }

                audioSource.clip = FingerPressedSound;
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 1;
                audioSource.dopplerLevel = 0;
            }
        }

        private void InteractionManager_SourceDetected(InteractionSourceState hand)
        {
            HandDetected = true;
            Debug.Log("Hand detected");
        }

        private void InteractionManager_SourceLost(InteractionSourceState hand)
        {
            HandDetected = false;

            // 2.a: Reset FocusedGameObject.
            ResetFocusedGameObject();
        }


        //private void InteractionManager_SourceUpdated(InteractionSourceState hand)
        //{
        //    Debug.Log("Source Updated");
        //    /* Debug.Log("Source updated");
        //      Start showing an indicator to move your hand toward the center of the view.

        //     if (hand.properties.sourceLossRisk > HandGuidanceThreshold)
        //     {
        //         Debug.Log("Hand within range");
        //         HandDetected = true;
        //     }
        //     else
        //     {
        //         Debug.Log("Hand Out of range");
        //         HandDetected = false;
        //     }
        //     */

        //    /*
        //     Only display hand indicators when we are in a holding state, since hands going out of view will affect any active gestures.
        //    if (!hand.pressed)
        //    {
        //        return;
        //    }

        //     Only track a new hand if are not currently tracking a hand.
        //    if (!currentlyTrackedHand.HasValue)
        //    {
        //        currentlyTrackedHand = hand.source.id;
        //    }
        //    else if (currentlyTrackedHand.Value != hand.source.id)
        //    {
        //         This hand is not the currently tracked hand, do not drawn a guidance indicator for this hand.
        //        return;
        //    }*/


        //}





        private void InteractionManager_SourcePressed(InteractionSourceState hand)
        {
            //GestureManager.Instance.Transition(GestureManager.Instance.ManipulationRecognizer);
            Debug.Log("Pressed");

            GestureManager.Instance.IsManipulating = true;
            //mikko commented out
            //targetSelection.setPreRefinementTime();
           // targetSelection.checkPreRefinementAccuracy();

            cursor.SendMessage("PerformManipulationStartHead");
        }

        private void InteractionManager_SourceReleased(InteractionSourceState hand)
        {
            Debug.Log("Unpressed");

            GestureManager.Instance.IsManipulating = false;

            target_horisontal.GetComponentInChildren<Renderer>().enabled = false;
            target_vertical.GetComponentInChildren<Renderer>().enabled = false;


            //sceneCreation.GetComponent<sceneCreationTargetSelection>().isLogging = false;
            //sceneCreation.SendMessage("checkAccuracyRequirement");
            //sceneCreation.SendMessage("stopLogging");
            //mikko commented out
            //targetSelection.checkAccuracyRequirement();

            //mikko commented out
            //cursor.GetComponent<WorldCursorTargetSelection>().isReady = false;
            cursor.GetComponent<simpleRadialCursor>().isReady = false;
            //sceneCreation.SendMessage("nextTrial");

            // 2.a: Reset FocusedGameObject.
            // ResetFocusedGameObject();
        }

        private void ResetFocusedGameObject()
        {
            // 2.a: Set FocusedGameObject to be null.
            FocusedGameObject = null;

            // 2.a: On GestureManager call ResetGestureRecognizers
            // to complete any currently active gestures.
            GestureManager.Instance.ResetGestureRecognizers();
        }

        void OnDestroy()
        {
            InteractionManager.SourceDetected -= InteractionManager_SourceDetected;
            InteractionManager.SourceLost -= InteractionManager_SourceLost;
           // InteractionManager.SourceUpdated -= InteractionManager_SourceUpdated;
            // 2.a: Unregister the SourceManager.SourceReleased event.
            InteractionManager.SourceReleased -= InteractionManager_SourceReleased;

            // 2.a: Unregister for SourceManager.SourcePressed event.
            InteractionManager.SourcePressed -= InteractionManager_SourcePressed;
        }
    }
}