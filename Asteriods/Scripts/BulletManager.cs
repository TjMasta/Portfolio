using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: TJ Driscoll
/// Fires bullets and checks to see if they are still in the game.
/// </summary>
public class BulletManager : MonoBehaviour
{
    public GameObject bullet;
    public List<GameObject> bullets;

	// Use this for initialization
	void Start ()
    {
        bullets = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(GameObject.Find("GameManager").GetComponent<GameController>().startGame)
        {
            Fire();

            CheckList();
        }
	}

    /// <summary>
    /// Fires a bullet when the space bar is pressed.
    /// </summary>
    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bullets.Add(Instantiate(bullet));
        }
    }

    /// <summary>
    /// Removes bullets from the list if they are set to null.
    /// </summary>
    void CheckList()
    {
        for(int i = 0; i < bullets.Count; i++)
        {
            if(bullets[i] == null)
            {
                bullets.Remove(bullets[i]);
            }
        }
    }
}
