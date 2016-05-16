
using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity;

/// <summary>
/// GestureManager creates a gesture recognizer and signs up for a tap gesture.
/// When a tap gesture is detected, GestureManager uses GazeManager to find the game object.
/// GestureManager then sends a message to that game object.
/// </summary>
[RequireComponent(typeof(PointerManager))]
public class ClickManager : Singleton<ClickManager>
{
    /// <summary>
    /// To select even when a hologram is not being gazed at,
    /// set the override focused object.
    /// If its null, then the gazed at object will be selected.
    /// </summary>
    public GameObject OverrideFocusedObject
    {
        get; set;
    }

    //private GestureRecognizer gestureRecognizer;
    private GameObject focusedObject;

    void Start()
    {
        var trackedController = GetComponent<SteamVR_TrackedController>();
        if (trackedController == null)
        {
            trackedController = gameObject.AddComponent<SteamVR_TrackedController>();
        }

        trackedController.TriggerClicked += new ClickedEventHandler(GestureRecognizer_TappedEvent);
        // Create a new GestureRecognizer. Sign up for tapped events.
        //gestureRecognizer = new GestureRecognizer();
        //gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);

        //gestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;


        // Start looking for gestures.
        //gestureRecognizer.StartCapturingGestures();
    }

    void GestureRecognizer_TappedEvent(object sender, ClickedEventArgs e)
    {
        if (focusedObject != null)
        {
            // geez what a mess, figure this out later but whatever
            focusedObject.SendMessageUpwards("OnSelect");
        }
    }

    void LateUpdate()
    {
        GameObject oldFocusedObject = focusedObject;

        if (PointerManager.Instance.Hit &&
            OverrideFocusedObject == null &&
            PointerManager.Instance.HitInfo.collider != null)
        {
            // If gaze hits a hologram, set the focused object to that game object.
            // Also if the caller has not decided to override the focused object.
            focusedObject = PointerManager.Instance.HitInfo.collider.gameObject;
        }
        else
        {
            // If our gaze doesn't hit a hologram, set the focused object to null or override focused object.
            focusedObject = OverrideFocusedObject;
        }

        if (focusedObject != oldFocusedObject)
        {
            // If the currently focused object doesn't match the old focused object, cancel the current gesture.
            // Start looking for new gestures.  This is to prevent applying gestures from one hologram to another.
            //gestureRecognizer.CancelGestures();
            //gestureRecognizer.StartCapturingGestures();
        }
    }

    void OnDestroy()
    {
        //gestureRecognizer.StopCapturingGestures();
        //gestureRecognizer.TappedEvent -= GestureRecognizer_TappedEvent;
    }
}