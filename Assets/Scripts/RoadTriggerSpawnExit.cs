using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTriggerSpawnExit : MonoBehaviour
{
    public bool IsActive;
    // Start is called before the first frame update
    RoadScript road;
    void Start()
    {
        road = GetComponentInParent<RoadScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (!IsActive || other.gameObject.tag != "Car")
            return;
        road.SpawnerOnExit();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (!IsActive || other.gameObject.tag != "Car")
            return;
       road.SpawnerOnEnter();
    }
}
