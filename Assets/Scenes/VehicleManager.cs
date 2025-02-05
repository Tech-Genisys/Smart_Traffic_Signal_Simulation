using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
   
    public Transform[] spawnLocations;

    public TrafficLight[] trafficLights;


    private List<Vector2> directions;


    public GameObject[] vehicles;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
         directions = new List<Vector2>{Vector2.right, Vector2.down, Vector2.left, Vector2.up};
        InvokeRepeating(nameof(SpawnVehicle), 0f, 1f);
    }

    // Define rarity levels (higher value = more common)
    private Dictionary<string, float> vehicleRarity = new Dictionary<string, float>()
    {
        { "car", 0.5f },         // 50% chance
        { "truck", 0.3f },       // 30% chance
        { "bus", 0.15f },        // 15% chance
        { "ambulance", 0.05f }   // 5% chance (rarest)
    };

    void SpawnVehicle()
    {
        // Select a random spawn point and direction
        int random = Random.Range(0, spawnLocations.Length);
        Transform spawnPoint = spawnLocations[random];
        Vector2 direction = directions[random];

        // Get a random vehicle using weighted selection
        GameObject vehicle = GetRandomVehicleByRarity();

        // Instantiate the vehicle at the spawn point
        GameObject vehicleInstance = Instantiate(vehicle, spawnPoint.position, Quaternion.identity);
        Vehicle vh = vehicleInstance.GetComponent<Vehicle>();

        // Assign the vehicle to the traffic light
        trafficLights[random].vehicles.Add(vh);
        vh.Init(direction);
    }

    // Weighted random selection of vehicles
    private GameObject GetRandomVehicleByRarity()
    {
        // Create a weighted list of vehicles
        List<GameObject> weightedList = new List<GameObject>();

        foreach (GameObject vehicle in vehicles)
        {
            Vehicle vh = vehicle.GetComponent<Vehicle>();
            if (vh != null && vehicleRarity.ContainsKey(vh.vehicleClass))
            {
                float weight = vehicleRarity[vh.vehicleClass] * 100; // Scale up to avoid small float precision issues
                for (int i = 0; i < weight; i++)
                {
                    weightedList.Add(vehicle);
                }
            }
        }

        // Select a vehicle from the weighted list
        return weightedList[Random.Range(0, weightedList.Count)];
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
