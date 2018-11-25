using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: TJ Driscoll
/// Controls the flow of the game.
/// </summary>
public class GameController : MonoBehaviour
{
    public int lives;
    public GameObject lifeImage;
    public List<GameObject> images;
    public bool startGame;
    public Sprite image;

    public GameObject text;
    private GameObject textImage;
    public Sprite gameStart;
    public Sprite gameContinue;
    public Sprite gameOver;

    public bool arcadeMode;

    public int score;

	// Use this for initialization
	void Start ()
    {
        DisplayLives();
        score = 0;

        textImage = Instantiate(text, new Vector3(0, 2, 0), Quaternion.identity);

        startGame = false;
        arcadeMode = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(!startGame && Input.anyKeyDown) // Starts the game if any key is pressed
        {
            startGame = true;
            if(Input.GetKeyDown(KeyCode.G)) // If 'G' is pressed the game starts in arcade mode
            {
                arcadeMode = true;
            }
        }

        if(lives <= 0) // Resets the game if lives are 0 or somehow end up being less than 0.
        {
            startGame = false;
            arcadeMode = false;
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                lives = 3;
                score = 0;
                gameObject.GetComponent<Powers>().Reset();
                GameObject.Find("Spawner").GetComponent<AsteriodManager>().Reset();
            }
        }

        AdjustLives();
        ChangeText();
    }

    /// <summary>
    /// Shows the 3 lives in the top right of the screen.
    /// </summary>
    void DisplayLives()
    {
        for(int i = 0; i < lives; i++)
        {
            images.Add(Instantiate(lifeImage));

            images[i].GetComponent<Transform>().position = new Vector3(Camera.main.orthographicSize * Camera.main.aspect - 0.5f - (0.6f * i), Camera.main.orthographicSize - 0.5f, 0);
        }
    }

    /// <summary>
    /// Changes the text on the screen to show what game state the player is in.
    /// The start, continue, game over, or currently playing.
    /// </summary>
    void ChangeText()
    {
        if (!startGame && lives == 3)
        {
            textImage.GetComponent<SpriteRenderer>().sprite = gameStart;
        }
        else if (lives == 0)
        {
            textImage.GetComponent<SpriteRenderer>().sprite = gameOver;
        }
        else if (!startGame && lives < 3)
        {
            textImage.GetComponent<SpriteRenderer>().sprite = gameContinue;
        }
        else
        {
            textImage.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    /// <summary>
    /// Shows the score and whether arcade mode is on.
    /// </summary>
    private void OnGUI()
    {
        GUIStyle thing = new GUIStyle("box");
        thing.fontSize = 25;

        GUI.Box(new Rect(-Camera.main.orthographicSize * Camera.main.aspect + 0.2f, Camera.main.orthographicSize - 2, 150, 100), "Score: " + score, thing);

        if(arcadeMode)
        {
            GUI.Box(new Rect(-Camera.main.orthographicSize * Camera.main.aspect + 2, 30, 160, 100), "Arcade mode: On");
        }
        else
        {
            GUI.Box(new Rect(-Camera.main.orthographicSize * Camera.main.aspect + 2, 30, 160, 100), "Arcade mode: Off");
        }
    }

    /// <summary>
    /// Changes the lives display to show how many lives are left.
    /// </summary>
    void AdjustLives()
    {
        for (int i = images.Count - 1; i >= 0; i--)
        {
            if (lives - 1 < i)
            {
                images[i].GetComponent<SpriteRenderer>().sprite = null;
            }
            else
            {
                images[i].GetComponent<SpriteRenderer>().sprite = image;
            }
        }
    }
}
