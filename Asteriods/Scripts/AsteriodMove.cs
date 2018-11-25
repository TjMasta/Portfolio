using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: TJ Driscoll
/// Moves the asteroids and makes them wrap around the screen.
/// </summary>
public class AsteriodMove : MonoBehaviour
{
    private Vector3 asteriodPos;
    private float speedX;
    private float speedY;

    // Use this for initialization
    void Start ()
    {
		asteriodPos = GetComponent<Transform>().position;
        speedX = Random.Range(-0.04f, 0.04f);
        speedY = Random.Range(-0.04f, 0.04f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!GameObject.Find("GameManager").GetComponent<Powers>().freeze) // Freezes the asteriods if the freeze power is true.
        {
            Move();
            ScreenWrap();
        }

        SetTransform();
	}

    /// <summary>
    /// Moves the asteroid in one direction.
    /// </summary>
    public void Move()
    {
        asteriodPos = new Vector3(asteriodPos.x + speedX, asteriodPos.y + speedY, 0);
    }

    /// <summary>
    /// Wraps the asteroid around the screen when it hits one side.
    /// </summary>
    void ScreenWrap()
    {
        if (asteriodPos.x > Camera.main.orthographicSize * Camera.main.aspect)
        {
            asteriodPos = new Vector3(-(Camera.main.orthographicSize * Camera.main.aspect), asteriodPos.y, asteriodPos.z);
        }
        else if (asteriodPos.x < -(Camera.main.orthographicSize * Camera.main.aspect))
        {
            asteriodPos = new Vector3(Camera.main.orthographicSize * Camera.main.aspect, asteriodPos.y, asteriodPos.z);
        }

        if (asteriodPos.y > Camera.main.orthographicSize)
        {
            asteriodPos = new Vector3(asteriodPos.x, -Camera.main.orthographicSize, asteriodPos.z);
        }
        else if (asteriodPos.y < -(Camera.main.orthographicSize))
        {
            asteriodPos = new Vector3(asteriodPos.x, Camera.main.orthographicSize, asteriodPos.z);
        }
    }

    /// <summary>
    /// Sets the asteroids new position.
    /// </summary>
    void SetTransform()
    {
        GetComponent<Transform>().position = asteriodPos;
    }

    /// <summary>
    /// Removes the asteroid from the game.
    /// </summary>
    public void Death()
    {
        Destroy(this.gameObject, 0.1f);
    }
}
