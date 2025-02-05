using Unity.VisualScripting;
using UnityEngine;

public class Vehicle : MonoBehaviour
{   
    private Vector2 direction;
    public string vehicleClass;
    public float speed = 1f;
    private float currentSpeed;

    private bool hasPassed = false;
    public float stopRange = .1f;  // Range to detect other cars
    private bool isStopped = false;  // Flag to check if the car has stopped

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.currentSpeed = this.speed;
    }

    // Init method to set direction and speed for the car
    public void Init(Vector2 direction)
    {
        this.direction = direction;

        if (direction == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Default (Up)
        }
        else if (direction == Vector2.right)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90); // Right
        }
        else if (direction == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180); // Down
        }
        else if (direction == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90); // Left
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isStopped){
            return;
        }
        // Check for another car in front of the vehicle within the stopRange
        else if(DetectOtherCarInFront())
        {
            return;
        }

        else{
            // Move the car in the given direction if it's not stopped
            transform.Translate(this.currentSpeed * Time.deltaTime * Vector2.up);
        }

        
    }

    private void OnTriggerStay2D(Collider2D  collision)
    {
        if (collision.gameObject.CompareTag("Stop"))
        {
            if(hasPassed) return;
            StopsManger stopsManger = collision.gameObject.GetComponent<StopsManger>();
            TrafficLight trafficLight = stopsManger.signal;
            if (trafficLight != null && (trafficLight.GetCurrentLightState() == TrafficLight.LightState.Red || trafficLight.GetCurrentLightState() == TrafficLight.LightState.Yellow))
            {
                isStopped = true;
                this.currentSpeed = 0;
            }
            else{
                isStopped = false;
                this.currentSpeed = this.speed;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Stop")){
            if(hasPassed) return;
            StopsManger stopsManger = collision.gameObject.GetComponent<StopsManger>();
            TrafficLight trafficLight = stopsManger.signal;
            hasPassed = true;

            trafficLight.vehicles.Remove(this);
        }
    }


    // Detect if another car is in front of the vehicle within the stopRange
    private bool DetectOtherCarInFront()
    {
        float offsetDistance = 1f; 

        Vector2 rayOrigin = (Vector2)transform.position + direction.normalized * offsetDistance;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction.normalized, stopRange);

        Debug.DrawRay(rayOrigin, direction.normalized * stopRange, Color.red);


        if (hit.collider != null && hit.collider.CompareTag("Vehicle")) 
        {
            print("Car collition detected");
            print(hit.collider.tag);
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * stopRange);
    }
}
