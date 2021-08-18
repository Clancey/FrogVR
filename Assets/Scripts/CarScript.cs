using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarScript : MonoBehaviour
{
    bool isStopped;

    public float CarSpeed = 10f;
    public float DirectionModifier = 1;
    public Vector3 TargetPosition;
    public RoadScript road;
    public Vector3 startPos;
    float timer;
    GameObject currentModel;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void SetPositionPercent(float percent)
    {
        timer = percent;
        gameObject.transform.position = Vector3.Lerp(startPos, TargetPosition, timer);
    }
    bool isBreaking;
    float breakTime = 1;
    // Update is called once per frame
    void Update()
    {
        if(isBreaking)
        {

            timer += Time.deltaTime / breakTime;
            if(timer >= 1)
            {
                isBreaking = false;
                isStopped = true;
            }

            gameObject.transform.position = Vector3.Lerp(breakStart, breakTarget, timer);
            return;

        }
        if (isStopped)
            return;
        timer += Time.deltaTime / CarSpeed;
        if(gameObject.transform.position != TargetPosition)
        {
            gameObject.transform.position = Vector3.Lerp(startPos, TargetPosition, timer);
        }
        else
        {
            road.RemoveCar(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Car")// == "CarSafeZone")
        {
            var collider = c.gameObject.GetComponent<Collider>();
            var otherCar = c.gameObject.GetComponent<CarScript>();
            if (otherCar.road != road)
                return;
            isStopped = true;
            //if(!isStopped)
              //  ApplyBreaks();
            road.StopSpawn = true;
        }
    }

    Vector3 breakTarget;
    Vector3 breakStart;
    void ApplyBreaks()
    {
        if (isBreaking)
            return;
        isBreaking = true;
        timer = 0;
        breakTime = .5f;
        breakStart = transform.position;
        breakTarget = new Vector3(gameObject.transform.position.x - DirectionModifier, gameObject.transform.position.y, gameObject.transform.position.z);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Player"))// == "Player")
        {
            isStopped = true;
            road.StopSpawn = true;
        }
       
    }
    private void OnCollisionExit(Collision collision)
    {
        
    }
}
