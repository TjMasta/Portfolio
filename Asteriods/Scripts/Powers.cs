using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: TJ Driscoll
/// Manages the powers that are available in arcade mode.
/// </summary>
public class Powers : MonoBehaviour
{
    public bool hitAll;
    public bool hitAllUsed;

    public bool freeze;
    public bool freezeUsed;
    public float freezeTimer;

    public bool invicble;
    public bool invicbleUsed;
    public float invicleTimer;

	// Use this for initialization
	void Start ()
    {
        hitAll = false;
        hitAllUsed = false;

        freeze = false;
        freezeUsed = false;
        freezeTimer = 0;

        invicble = false;
        invicbleUsed = false;
        invicleTimer = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(gameObject.GetComponent<GameController>().arcadeMode)
        {
            if(hitAll)
            {
                hitAll = false;
            }
            else if(Input.GetKeyDown(KeyCode.Z) && !hitAll && !hitAllUsed) // Hits all asteroids if Z is pressed.
            {
                hitAll = true;
                hitAllUsed = true;
            }

            if(freeze && freezeTimer > 0 && freezeTimer < 2)
            {
                freezeTimer += Time.deltaTime;
            }
            else if (freeze && freezeTimer >= 2)
            {
                freeze = false;
            }
            else if (Input.GetKeyDown(KeyCode.X) && !freeze && !freezeUsed && freezeTimer == 0) // Freezes all asteroids if 'X' is pressed.
            {
                freeze = true;
                freezeUsed = true;
                freezeTimer += Time.deltaTime;
            }

            if (invicble && invicleTimer > 0 && invicleTimer < 2)
            {
                invicleTimer += Time.deltaTime;
            }
            else if(invicble && invicleTimer >= 2)
            {
                invicble = false;
            }
            else if (Input.GetKeyDown(KeyCode.C) && !invicble && !invicbleUsed && invicleTimer == 0) // Gives invinciblitilty when 'C' is pressed.
            {
                invicble = true;
                invicbleUsed = true;
                invicleTimer += Time.deltaTime;
            }
        }
	}

    /// <summary>
    /// Resests the powers for a new game or when the next wave of ateroids spawns.
    /// </summary>
    public void Reset()
    {
        hitAll = false;
        hitAllUsed = false;

        freeze = false;
        freezeUsed = false;
        freezeTimer = 0;

        invicble = false;
        invicbleUsed = false;
        invicleTimer = 0;
    }

    /// <summary>
    /// Hits all the asteriods once.
    /// </summary>
    /// <param name="asteriods"></param>
    public void HitAll(List<GameObject> asteriods)
    {
        for(int i = 0; i < asteriods.Count; i++)
        {
            if(asteriods[i].GetComponent<SpriteInfo>().state != 2)
            {
                asteriods[i].GetComponent<SpriteRenderer>().sprite = null;
                asteriods[i].gameObject.GetComponent<AsteriodMove>().Death();

                if (asteriods[i].GetComponent<SpriteInfo>().state == 0)
                {
                    GameObject.Find("Spawner").GetComponent<AsteriodManager>().Split(asteriods[i].transform.position.x, asteriods[i].transform.position.y);
                    GameObject.Find("Spawner").GetComponent<AsteriodManager>().Split(asteriods[i].transform.position.x, asteriods[i].transform.position.y);
                    GameObject.Find("GameManager").GetComponent<GameController>().score += 20;
                }
                else
                {
                    GameObject.Find("GameManager").GetComponent<GameController>().score += 50;
                }

                asteriods[i] = null;

                GameObject.Find("GameManager").GetComponent<GameController>().score += 10;
            }
        }
    }
}
