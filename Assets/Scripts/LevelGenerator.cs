using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject Water;
    public GameObject Grass;
    public GameObject Road;
    Dictionary<GameObject, int> gameObjectMaxValues = new Dictionary<GameObject, int>();
    GameObject[] gameObjects;
    float distanceToGenerate = 50;
    Vector3 currentPos = new Vector3(0,0,-20);
    FixedSizedQueue<GameObject> currentItems = new FixedSizedQueue<GameObject>(60);
    GameObject player;
    void Start()
    {
        currentItems.OnDequeue = (g) => Destroy(g);
        player = GameObject.FindGameObjectWithTag("Player");
        gameObjects = new[]
        {
            Water,Road,Grass,Road
        };
        gameObjectMaxValues =  new Dictionary<GameObject, int>()
        {
            [Water] = 6,
            [Road] = 5,
            [Grass] = 6
        };
        for (var i = currentPos.z; i < 1; i++)
            generateNext(Grass);
    }
    void Update()
    {
        var distance = player.transform.position.z + distanceToGenerate;
        while (currentPos.z < distance)
            generateBlock();
    }
    void generateBlock()
    {
        var type = gameObjects[Random.Range(0, gameObjects.Length)];
        if (!gameObjectMaxValues.TryGetValue(type, out var foo))
            System.Console.WriteLine(type);
        var amount = Random.Range(1, gameObjectMaxValues[type]);
        for (var i = 0; i < amount; i++)
            generateNext(type);
    }
    void generateNext(GameObject type)
    {
        currentPos.z += 1;
        var obj = Instantiate(type);
        obj.transform.position = currentPos;
        currentItems.Enqueue(obj);
    }
}
