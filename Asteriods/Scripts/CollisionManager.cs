using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: TJ  Driscoll
/// Deals with all the collisions in the game.
/// </summary>
public class CollisionManager : MonoBehaviour
{
    private List<GameObject> asteriods;
    public GameObject ship;
    private List<GameObject> bullets;

    public GameObject asteriod;

    private ColisionDetection cd;

    private float ghostTime;

	// Use this for initialization
	void Start ()
    {
        cd = GetComponent<ColisionDetection>();
        bullets = new List<GameObject>();
        asteriods = new List<GameObject>();

        ghostTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Gets the lists for the managers into this class.
        bullets = GameObject.Find("Spawner").GetComponent<BulletManager>().bullets;
        asteriods = GameObject.Find("Spawner").GetComponent<AsteriodManager>().asteriods;

        if(GameObject.Find("GameManager").GetComponent<GameController>().startGame)
        {
            if (ghostTime == 0 && !GameObject.Find("GameManager").GetComponent<Powers>().invicble) // Ghost time is similar to invincible, but only occurs after being hit.
            {
                CollisionCircle();
            }
            else if(ghostTime >= 1) // Resets the time ghost time and makes the ship white again.
            {
                ship.GetComponent<SpriteRenderer>().color = Color.white;
                ghostTime = 0;
            }
            else if(ghostTime > 0) // Makes the ship yellow and adds time to the ghost time.
            {
                ship.GetComponent<SpriteRenderer>().color = Color.yellow;
                ghostTime += Time.deltaTime;
            }
            else
            {
                ship.GetComponent<SpriteRenderer>().color = Color.yellow; // If the invincible power is active, the ship becomes yellow.
            }

            CollisionBullet();

            if (GameObject.Find("GameManager").GetComponent<Powers>().hitAll)
            {
                GameObject.Find("GameManager").GetComponent<Powers>().HitAll(asteriods);
            }
        }
    }

    /// <summary>
    /// Checks for a collision between the ship and an asteroid. If the ship is hit, it loses a life and sets its position
    /// back to the start position, makes you have to press a key to resume, and gives some time to be invulnerable.
    /// </summary>
    void CollisionCircle()
    {
        ship.GetComponent<SpriteRenderer>().color = Color.white;
        for (int i = 0; i < asteriods.Count; i++)
        {
            if (cd.CircleCollision(ship, asteriods[i]))
            {
                GameObject.Find("GameManager").GetComponent<GameController>().lives--; // Removes a life
                GameObject.Find("GameManager").GetComponent<GameController>().startGame = false;  // Pauses the game
                GameObject.Find("Ship").GetComponent<Vehicle>().vehiclePosition = Vector3.zero;  // Resets the position of the ship to the start

                if(GameObject.Find("GameManager").GetComponent<GameController>().lives > 0) // Starts the ghost time
                {
                    ghostTime += 0.01f;
                }
            }
            else
            {
                asteriods[i].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    /// <summary>
    /// Checks for collisions between bullets and asteroids.
    /// If an asteroid is hit, the bullet and asteroid lose their sprites and become null.
    /// Then, if the asteroid is a big one it spawns two smaller ones at its position.
    /// Finally, it adds 10 points to the score.
    /// </summary>
    void CollisionBullet()
    {
        for(int i = 0; i < bullets.Count; i++)
        {
            for(int j = 0; j < asteriods.Count; j++)
            {
                if(bullets[i] != null && asteriods[j] != null) // Checks to make sure the asteroid or the bullet isn't null.
                {
                    if (cd.BulletCollision(bullets[i], asteriods[j]))
                    {
                        // Removes the bullet from the game.
                        bullets[i].GetComponent<SpriteRenderer>().sprite = null;
                        bullets[i] = null;

                        // Removes the asteroid from the game.
                        asteriods[j].GetComponent<SpriteRenderer>().sprite = null;
                        asteriods[j].gameObject.GetComponent<AsteriodMove>().Death();

                        if (asteriods[j].GetComponent<SpriteInfo>().state == 0) // Spawns two small asteroids if a big one is hit.
                        {
                            GameObject.Find("Spawner").GetComponent<AsteriodManager>().Split(asteriods[j].transform.position.x, asteriods[j].transform.position.y);
                            GameObject.Find("Spawner").GetComponent<AsteriodManager>().Split(asteriods[j].transform.position.x, asteriods[j].transform.position.y);
                            GameObject.Find("GameManager").GetComponent<GameController>().score += 20; // Gives player some score.
                        }
                        else
                        {
                            GameObject.Find("GameManager").GetComponent<GameController>().score += 50; // Gives player some score.
                        }

                        asteriods[j] = null;
                    }
                }
            }
        }
    }
}
