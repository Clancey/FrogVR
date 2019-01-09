using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeadTracker : MonoBehaviour
{
    public GameObject cameraToFollow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraToFollow == null)
            return;
        this.transform.SetPositionAndRotation(cameraToFollow.transform.position, cameraToFollow.transform.rotation);
    }
}
