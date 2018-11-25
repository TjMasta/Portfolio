using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: TJ Driscoll
/// Holds the radius for the ship and asteriods. Also, holds states for the asteroids.
/// </summary>
public class SpriteInfo : MonoBehaviour
{
    private float radius;

    public int state;

    public float Radius { get { return radius; } }

	// Use this for initialization
	void Start ()
    {
        radius = GetComponent<Renderer>().bounds.size.x / 2;
	}
	
	// Update is called once per frame
	void Update ()
    {
    }
}
