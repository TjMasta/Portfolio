using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: TJ Driscoll
/// Detects collisions for ship on asteroid and bullet on asteroid.
/// </summary>
public class ColisionDetection : MonoBehaviour
{
    /// <summary>
    /// Checks for collision between the ship and the asteroid.
    /// </summary>
    /// <param name="ship"></param>
    /// <param name="asteriod"></param>
    /// <returns></returns>
    public bool CircleCollision(GameObject ship, GameObject asteriod)
    {
        if(Mathf.Pow(ship.GetComponent<SpriteInfo>().Radius + asteriod.GetComponent<SpriteInfo>().Radius, 2) > 
            Mathf.Pow(ship.GetComponent<Renderer>().bounds.center.x - asteriod.GetComponent<Renderer>().bounds.center.x ,2) + 
            Mathf.Pow(ship.GetComponent<Renderer>().bounds.center.y - asteriod.GetComponent<Renderer>().bounds.center.y, 2))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks collisions between a bullet and an asteroid.
    /// </summary>
    /// <param name="bullet"></param>
    /// <param name="asteriod"></param>
    /// <returns></returns>
    public bool BulletCollision(GameObject bullet, GameObject asteriod)
    {
        if(Mathf.Pow(bullet.GetComponent<Renderer>().bounds.center.x - asteriod.GetComponent<Renderer>().bounds.center.x, 2) +
            Mathf.Pow(bullet.GetComponent<Renderer>().bounds.center.y - asteriod.GetComponent<Renderer>().bounds.center.y, 2) < asteriod.GetComponent<SpriteInfo>().Radius)
        {
            return true;
        }

        return false;
    }
}
