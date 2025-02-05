using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public enum LightState { Red, Yellow, Green }

    public List<Vehicle> vehicles = new List<Vehicle>{};

    public TextMeshPro text;

    public SpriteRenderer[] lights;

    public float minGreenDuration = 2.5f;
    public float currentGreenDuration = 1f;
    public float maxGreenDuration = 10f;
    public float yellowDuration = 4f;
    public LightState currentLightState;
    

    public float currentYellowDuration = 0f;

    private void Start()
    {
     foreach (SpriteRenderer  light in lights){
        light.enabled = false;
     }
     currentGreenDuration = minGreenDuration;
    }


    public void Update(){
        text.text = currentGreenDuration.ToString("F1");
        if(currentLightState == LightState.Green){
            lights[0].enabled = true;
            lights[1].enabled = false;
            lights[2].enabled = false;
        }
        else if(currentLightState == LightState.Yellow){            
            lights[0].enabled = false;
            lights[1].enabled = true;
            lights[2].enabled = false;
        }
        else if(currentLightState == LightState.Red){
            lights[0].enabled = false;
            lights[1].enabled = false;
            lights[2].enabled = true;
        }
    }


    public LightState GetCurrentLightState(){
        return currentLightState;
    }
    
}
