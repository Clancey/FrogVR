using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject playerToFollow;
    Vector3 newPos;
    // Update is called once per frame
    void Update()
    {
        newPos = Vector3.Lerp(gameObject.transform.position, playerToFollow.transform.position, Time.deltaTime);
        newPos.y = 1;
        //Only let the camera move forwards
       // if(gameObject.transform.position.z < newPos.z)
            gameObject.transform.position = newPos;
    }
}
