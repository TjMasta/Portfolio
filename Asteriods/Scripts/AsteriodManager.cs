using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: TJ Driscoll
/// Controls the asteroids in the game by spawning and spliting them.
/// </summary>
public class AsteriodManager : MonoBehaviour {

    public List<GameObject> asteriods;
    private int amountOfAsteriods;
    public GameObject asteriod;
    public GameObject smallAsteriod;

	// Use this for initialization
	void Start ()
    {
        asteriods = new List<GameObject>();
        amountOfAsteriods = 6;
        Spawn();
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckList();

        if(asteriods.Count == 0)
        {
            Spawn();
        }
	}

    /// <summary>
    /// Randomly spawns asteroids on the screen based on a pre-determined amount. Set to 6.
    /// </summary>
    void Spawn()
    {
        for(int i = 0; i < amountOfAsteriods; i++)
        {
            asteriods.Add(Instantiate(asteriod));

            asteriod.GetComponent<Transform>().position = new Vector3(Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize), 
                Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize), 0);
        }

        GameObject.Find("GameManager").GetComponent<Powers>().Reset(); // Resets the powers for each wave.
        if(GameObject.Find("GameManager").GetComponent<GameController>().lives < 3)
        {
            GameObject.Find("GameManager").GetComponent<GameController>().lives++; // Gives the player another life for each wave they survive up to 3 lives
        }
    }

    /// <summary>
    /// Checks to see if asteroids have been set to null, if they are they are removed from the asteroids list.
    /// </summary>
    void CheckList()
    {
        for (int i = 0; i < asteriods.Count; i++)
        {
            if (asteriods[i] != null)
            {
                if (asteriods[i].GetComponent<SpriteInfo>().state == 2) // State 2 is new spawn, this is so it isn't destroyed by the "Hit All" power. It sets them to normal.
                {
                    asteriods[i].GetComponent<SpriteInfo>().state = 1;
                }
            }
            else
            {
                asteriods.Remove(asteriods[i]);
                i--;
            } 
        }
    }

    /// <summary>
    /// Spawns a small asteroid at the given location of the bigger asteroid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Split(float x, float y)
    {
        asteriods.Add(Instantiate(smallAsteriod, new Vector3(x, y, 0), Quaternion.identity));
        asteriods[asteriods.Count - 1].GetComponent<SpriteInfo>().state = 2; // State 2 is new spawn, this is so it isn't destroyed by the "Hit All" power. It sets them to normal.
    }

    public void Reset()
    {
        for (int i = 0; i < asteriods.Count; i++)
        {
            asteriods[i].GetComponent<AsteriodMove>().Death();
            asteriods[i] = null;
        }
    }
}
