using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: TJ Driscoll
/// Moves the bullet and wraps it around the screen
/// </summary>
public class BulletMove : MonoBehaviour
{
    private GameObject ship;
    private Vector3 velocity;
    private Vector3 position;
	// Use this for initialization
	void Start ()
    {
        ship = GameObject.Find("Ship");

        velocity = ship.GetComponent<Vehicle>().direction.normalized / 5;
        position = ship.GetComponent<Vehicle>().vehiclePosition + velocity;

        GetComponent<Transform>().position = position;

        Destroy(this.gameObject, 0.6f);  //Keeps the bullet alive for a short time.
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(this.gameObject != null)
        {
            position += velocity;

            GetComponent<Transform>().position = position;

            ScreenWrap();
        }
    }

    /// <summary>
    /// Wraps the bullet around the screen when it hits one side.
    /// </summary>
    void ScreenWrap()
    {
        if (position.x > Camera.main.orthographicSize * Camera.main.aspect)
        {
            position = new Vector3(-(Camera.main.orthographicSize * Camera.main.aspect), position.y, position.z);
        }
        else if (position.x < -(Camera.main.orthographicSize * Camera.main.aspect))
        {
            position = new Vector3(Camera.main.orthographicSize * Camera.main.aspect, position.y, position.z);
        }

        if (position.y > Camera.main.orthographicSize)
        {
            position = new Vector3(position.x, -Camera.main.orthographicSize, position.z);
        }
        else if (position.y < -(Camera.main.orthographicSize))
        {
            position = new Vector3(position.x, Camera.main.orthographicSize, position.z);
        }
    }
}
