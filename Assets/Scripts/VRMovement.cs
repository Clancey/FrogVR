using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class VRMovement : MonoBehaviour
{
    readonly float eyeHeight = 1.75f; // OVRPlugin.eyeHeight
    readonly Vector3 neckToEye = new Vector3(0, 0.075f, 0.08f); // new Vector3(0, OVRPlugin.eyeHeight - OVRManager.profile.neckHeight, OVRPlugin.eyeDepth)

    Transform cameraTransform;
    float LerpSpeed = 3;

    Quaternion lastRotation;
    Vector3 lastPosition;
    float velocity;
    bool isSetup;
    float currentLerpTime;
    float startTime;
    int bounds = 5;
    int maxZ, maxBackTrack = 10;
    void Start()
    {
        startTime = Time.time;
        cameraTransform = GetComponentInChildren<Camera>().transform;
        lastPosition = cameraTransform.position;
        targetVector = transform.position;
    }

    float movementTimeThreshold;
    float jumpThreshold = 2.5f;
    bool didStartJump;
    float peak;
    float perc;
    // Update is called once per frame
    void Update()
    {
        DetectJump();
        UpdateMovement();
        UpdateCollider();
    }


    void DetectJump()
    {
        if (!isSetup)
        {
            isSetup = (Time.time - startTime) > 3;
            return;
        }
        var rotPositionChange = cameraTransform.localRotation * neckToEye - lastRotation * neckToEye;
        var rotCompensation = Mathf.Abs(rotPositionChange.y);

        var yChange = cameraTransform.localPosition.y - lastPosition.y;

        var desiredVelocity = Time.deltaTime > 0 ? (Mathf.Abs(yChange) - rotCompensation) * 5 / Time.deltaTime : 0;
        velocity = Mathf.Lerp(velocity, desiredVelocity, Time.deltaTime * 5);
        peak = Mathf.Max(peak, velocity);
       // Debug.Log($"y: {yChange} : {velocity}, peak: {peak}");
        if(velocity > jumpThreshold)
        {
            if(yChange > 0)
            {
                didStartJump = true;
            } else if(didStartJump)
            {
                CalculateTarget();
                ///Jump!!!
                Debug.Log($"Jumped: {cameraTransform.rotation.eulerAngles}");
                didStartJump = false;
            }
        }

        lastPosition = cameraTransform.localPosition;
        lastRotation = cameraTransform.localRotation;
    }

    void CalculateTarget()
    {
        if (targetVector != transform.position)
            return;
        var x = 0;
        var z = 0;
        var angle = cameraTransform.rotation.eulerAngles.y;
        //Move Forward
        if (angle >= 300 || angle <= 60)
            z = 1;
        //MoveBackwards
        else if (angle >= 120 && angle <= 240)
            z = -1;
        //Looking Right
        if (angle >= 30 && angle <= 150)
            x = 1;
        //Looking Left
        else if (angle >= 210 && angle <= 330)
            x = -1;
        var newPos = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        if (newPos.z >= maxZ - maxBackTrack && Mathf.Abs(newPos.x) < bounds)
        {
            targetVector = newPos;
            currentLerpTime = 0;
            maxZ = Mathf.Max(maxZ, (int)targetVector.z);
        }
    }
    Vector3 targetVector;
    void UpdateMovement()
    {
        if (targetVector == transform.position)
        {
            return;
        }
        currentLerpTime += Time.deltaTime * LerpSpeed;
        perc = currentLerpTime;
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetVector, perc);
    }

    void UpdateCollider()
    {
        var cc = ((CapsuleCollider)GetComponent<Collider>());
        var center = cc.center;

        // if no obstacles on the way move collider together with camera
        //if (Mathf.Abs(GetComponent<Rigidbody>().velocity.x) < 0.001f && Mathf.Abs(GetComponent<Rigidbody>().velocity.z) < 0.001f)
            center = cameraTransform.localPosition;

        var height = cameraTransform.localPosition.y;
        center.y = height / 2;
        cc.center = Vector3.Lerp(cc.center, center, Time.deltaTime);
        cc.height = cc.center.y * 2;
    }
}
