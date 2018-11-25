using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{

    // Unnecessary
    //public float speed;                       // Speed of the vehicle, not needed anymore

    // Necessary
    public float accelRate;                     // Small, constant rate of acceleration
    public Vector3 vehiclePosition;             // Local vector for movement calculation
    public Vector3 direction;                   // Way the vehicle should move
    public Vector3 velocity;                    // Change in X and Y
    public Vector3 acceleration;                // Small accel vector that's added to velocity
    public float angleOfRotation;               // 0 
    public float maxSpeed;                      // 0.5 per frame, limits mag of velocity

	// Use this for initialization
	void Start ()
    {
        vehiclePosition = new Vector3(0, 0, 0);     // Or you could say Vector3.zero
        direction = new Vector3(1, 0, 0);           // Facing right
        velocity = new Vector3(0, 0, 0);            // Starting still (no movement)
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(GameObject.Find("GameManager").GetComponent<GameController>().startGame)
        {
            RotateVehicle();

            Drive();

            ScreenWrap();
        }

        SetTransform();
    }

    /// <summary>
    /// Changes / Sets the transform component
    /// </summary>
    public void SetTransform()
    {
        // Rotate vehicle sprite
        transform.rotation = Quaternion.Euler(0, 0, angleOfRotation);

        // Set the transform position
        transform.position = vehiclePosition;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Drive()
    {
        // Accelerate
        // Small vector that's added to velocity every frame
        if(Input.GetKey(KeyCode.UpArrow))
        {
            acceleration = accelRate * direction;
        }
        else
        {
            acceleration = Vector3.zero;
            velocity /= 1.08f;
        }

        velocity += acceleration;

        // Limit velocity so it doesn't become too large
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Add velocity to vehicle's position
        vehiclePosition += velocity;
    }

    public void RotateVehicle()
    {
        // Player can control direction
        // Left arrow key = rotate left by 2 degrees
        // Right arrow key = rotate right by 2 degrees
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            angleOfRotation += 2;
            direction = Quaternion.Euler(0, 0, 2) * direction;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            angleOfRotation -= 2;
            direction = Quaternion.Euler(0, 0, -2) * direction;
        }
    }

    public void ScreenWrap()
    {
        if(vehiclePosition.x > Camera.main.orthographicSize * Camera.main.aspect)
        {
            vehiclePosition = new Vector3(-(Camera.main.orthographicSize * Camera.main.aspect), vehiclePosition.y, vehiclePosition.z);
        }
        else if(vehiclePosition.x < -(Camera.main.orthographicSize * Camera.main.aspect))
        {
            vehiclePosition = new Vector3(Camera.main.orthographicSize * Camera.main.aspect, vehiclePosition.y, vehiclePosition.z);
        }

        if (vehiclePosition.y > Camera.main.orthographicSize)
        {
            vehiclePosition = new Vector3(vehiclePosition.x, -Camera.main.orthographicSize, vehiclePosition.z);
        }
        else if (vehiclePosition.y < -(Camera.main.orthographicSize))
        {
            vehiclePosition = new Vector3(vehiclePosition.x, Camera.main.orthographicSize, vehiclePosition.z);
        }
    }
}
