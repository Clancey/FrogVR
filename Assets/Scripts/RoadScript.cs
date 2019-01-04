using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScript : MonoBehaviour
{
    public GameObject[] VehiclePrefabs = new GameObject[0];
    int directionModifier;
    public float RoadHalfLength = 30;
    Vector3 RoadStart;
    Vector3 RoadEnd;
    public float MaxSPawnTime = 5;
    float minSpawnTime = 1;
    float nextActionTime;
    public bool StopSpawn;
    List<GameObject> activeCars = new List<GameObject>();

    float carSpeed;

    // Start is called before the first frame update
    void Start()
    {
        nextActionTime = Time.time + Random.Range(minSpawnTime, MaxSPawnTime);
        directionModifier = (Random.Range(0, 2) == 0) ? 1 : -1;
        RoadEnd = RoadStart = gameObject.transform.position;
        RoadStart.y = RoadEnd.y = .85f;
        RoadStart.x += RoadHalfLength * directionModifier;
        RoadEnd.x -= RoadHalfLength * directionModifier;
        carSpeed = Random.Range(10f, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        if (StopSpawn)
            return;
        if(Time.time > nextActionTime)
        {
            nextActionTime += Random.Range(minSpawnTime, MaxSPawnTime);
            SpawnCar();
        }
    }

    void SpawnCar()
    {
        var carType = VehiclePrefabs[Random.Range(0, VehiclePrefabs.Length)];
        var car = Instantiate(carType);
        var carScript = car.GetComponent<CarScript>();
        carScript.TargetPosition = RoadEnd;
        carScript.DirectionModifier = directionModifier;
        carScript.transform.position = RoadStart;
        carScript.CarSpeed = carSpeed;
        carScript.road = this;
        activeCars.Add(car);
    }
    private void OnDestroy()
    {
        activeCars.ForEach(Destroy);
    }
    public void RemoveCar(GameObject gameObject)
    {
        activeCars.Remove(gameObject);
    }
}
