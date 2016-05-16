using UnityEngine;
using System.Collections;

public class ViveSelection : MonoBehaviour {

    // Use this for initialization
    private SteamVR_TrackedController trackedController;


    [Tooltip("Select the layers raycast should target.")]
    public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

    void Start()
    {
        trackedController = GetComponent<SteamVR_TrackedController>();
        if (trackedController == null)
        {
            trackedController = gameObject.AddComponent<SteamVR_TrackedController>();
        }

        trackedController.TriggerClicked += new ClickedEventHandler(DoClick);
    }

    void DoClick(object sender, ClickedEventArgs e)
    {

        //SteamVR_TrackedController controller = GetComponent<SteamVR_TrackedController>();
        Debug.Log("trigger clicked: raycasting");

        Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit, RaycastLayerMask);

        Debug.Log(bHit);
        if (bHit)
        {
            Debug.Log("obj hit");
            Debug.Log(hit.collider.gameObject.tag);
            hit.collider.gameObject.SendMessage("OnSelect");
        }

    }

}
