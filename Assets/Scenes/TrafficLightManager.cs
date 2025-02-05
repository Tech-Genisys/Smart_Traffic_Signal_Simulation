using System.Collections;
using System.Xml.Schema;
using TMPro;
using UnityEngine;


public class TrafficLightManager : MonoBehaviour
{

    public TrafficLight[] signals;

    public TextMeshPro emergancyVhLocation;

    private int currentSignal = 0; 
    private int nextSignal = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        TrafficLight activeSignal = signals[currentSignal];
        print($"Current signal: {activeSignal}");

        if (activeSignal.currentGreenDuration > 0)
        {
            activeSignal.currentLightState = TrafficLight.LightState.Green;
            activeSignal.currentYellowDuration = activeSignal.yellowDuration;
            activeSignal.currentGreenDuration -= Time.deltaTime;
            return;
        }
        else if (activeSignal.currentYellowDuration > 0)
        {
            activeSignal.currentLightState = TrafficLight.LightState.Yellow;
            activeSignal.currentYellowDuration -= Time.deltaTime;
            return;
        }
        else
        {
            activeSignal.currentLightState = TrafficLight.LightState.Red;
        }

        float optimalTime;

        int priorityLane = PriorityVehicles();

        if (priorityLane != -1)
        {
            if (currentSignal != priorityLane)
            {
                currentSignal = priorityLane;
                optimalTime = FindOptimalTime(priorityLane);
            }
            else
            {
                if (activeSignal.currentGreenDuration <= 0)
                {
                    currentSignal = nextSignal;
                    nextSignal = (currentSignal + 1) % signals.Length;
                    optimalTime = FindOptimalTime(nextSignal);
                }
                else
                {
                    return;
                }
            }
        }
        else
        {
            currentSignal = nextSignal;
            nextSignal = (currentSignal + 1) % signals.Length;
            optimalTime = FindOptimalTime(nextSignal);
        }
        if(optimalTime < activeSignal.minGreenDuration){
            optimalTime = activeSignal.minGreenDuration;
        }
        else if (optimalTime > activeSignal.maxGreenDuration)
        {
            optimalTime = activeSignal.maxGreenDuration;
        }

        signals[currentSignal].currentGreenDuration = optimalTime;
        print($"Next signal: {currentSignal}, emergency: {priorityLane}, green Time: {optimalTime}");
    }


    public int PriorityVehicles()
    {
        for (int i = 0; i < signals.Length; i++)
        {
            foreach (Vehicle vh in signals[i].vehicles)
            {
                if (vh.vehicleClass == "ambulance" && i != currentSignal)
                {
                    print("Priority vh found");
                    print(i);
                    emergancyVhLocation.text = i.ToString();
                    return i;
                }
            }
        }
        emergancyVhLocation.text = "None";
        return -1; // Return -1 if no ambulance is found
    }


    public float FindOptimalTime(int index){

        TrafficLight signal = signals[index];

        return signal.vehicles.Count * 2f;
    }

}
