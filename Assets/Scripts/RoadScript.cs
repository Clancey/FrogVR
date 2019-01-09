using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScript : MonoBehaviour
{
    public GameObject[] VehiclePrefabs = new GameObject[0];
    public GameObject[] EmergencyVehiclePrefabs = new GameObject[0];
    int directionModifier;
    public float RoadHalfLength = 35;
    Vector3 RoadStart;
    Vector3 RoadEnd;
    public float MaxSPawnTime = 5;
    float minSpawnTime = 1;
    float nextActionTime;
    public bool StopSpawn;
    int maxActiveCars = 5;
    List<GameObject> activeCars = new List<GameObject>();

    float carSpeed;

    // Start is called before the first frame update
    void Start()
    {
        MaxSPawnTime = Random.Range(2, 8);
        maxActiveCars = Random.Range(5, 20);
        nextActionTime = Time.time + Random.Range(minSpawnTime, MaxSPawnTime);
        directionModifier = (Random.Range(0, 2) == 0) ? 1 : -1;
        if(directionModifier > 0)
        {
            GetComponentsInChildren<RoadTriggerSpawnExit>()[0].IsActive = true ;
        }
        else
        {
            GetComponentsInChildren<RoadTriggerSpawnExit>()[1].IsActive = true;
        }
        RoadEnd = RoadStart = gameObject.transform.position;
        RoadStart.y = RoadEnd.y = .85f;
        RoadStart.x += RoadHalfLength * directionModifier;
        RoadEnd.x -= RoadHalfLength * directionModifier;
        carSpeed = Random.Range(30f, 40f);

        //Spawn cars to Start;
        var carCount = Random.Range(0, 6);
        float perc = 1f / (carCount + 3);
        float halfPerc = perc / 2;
        for (var i = 1; i <= carCount; i++)
        {
            var start = (i * perc) + halfPerc;
            var car = SpawnCar();
            car.SetPositionPercent(start);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (StopSpawn || isSpawnZoneFull)
            return;
        if(Time.time > nextActionTime)
        {
            nextActionTime += Random.Range(minSpawnTime, MaxSPawnTime);
            if(maxActiveCars > activeCars.Count && Random.Range(0,2) != 0)
                SpawnCar();
        }
    }

    CarScript SpawnCar()
    {
        var carType = VehiclePrefabs[Random.Range(0, VehiclePrefabs.Length)];
        var car = Instantiate(carType);
        var carScript = car.GetComponent<CarScript>();
        carScript.TargetPosition = RoadEnd;
        carScript.DirectionModifier = directionModifier;
        carScript.startPos = carScript.transform.position = RoadStart;
        carScript.CarSpeed = carSpeed;
        carScript.road = this;
        car.transform.rotation = Quaternion.Euler(0,directionModifier == 1 ? 0 : 180,0);
        activeCars.Add(car);
        return carScript;
    }
    private void OnDestroy()
    {
        activeCars.ForEach(Destroy);
    }
    public void RemoveCar(GameObject gameObject)
    {
        activeCars.Remove(gameObject);
    }
    bool isSpawnZoneFull => carSpawnCount > 0;
    int carSpawnCount = 0;
    public void SpawnerOnEnter()
    {
        carSpawnCount++;
    }
    public void SpawnerOnExit()
    {
        carSpawnCount--;
    }
}
