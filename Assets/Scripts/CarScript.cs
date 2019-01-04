using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    bool isStopped;

    public float CarSpeed = 10f;
    public float DirectionModifier = 1;
    public Vector3 TargetPosition;
    public RoadScript road;
    Vector3 startPos;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name.StartsWith("Car"))// == "CarSafeZone")
        {
            isStopped = true;
            road.StopSpawn = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.StartsWith("Pen") || collision.gameObject.name.StartsWith("Play"))// == "Player")
        {
            isStopped = true;
            road.StopSpawn = true;
        }
       
    }
    private void OnCollisionExit(Collision collision)
    {
        
    }
}
