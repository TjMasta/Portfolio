function decreaseLifeBy(value) // Changes the value of the lives
{
    lives -= value;
    lives = parseInt(lives);
    lifeLabel.text = `Lives:   ${lives}`;
}

function increaseTime(value) // increases the in game timer
{
    inGameTime += value;
    inGameTime = inGameTime.toFixed(3);
    inGameTime = parseFloat(inGameTime);
    timeLabel.text = `Time:  ${inGameTime}`
}

function removeTerrain() // Removes the terrain from the level
{
    for(let i = 0; i < terrain.length; i++)
    {
        for(let j = 0; j < terrain[i].length; j++)
        {
            if(terrain[i][j] != 0)
            {
                gameScene.removeChild(terrain[i][j]);
            }
        }
    }
}

function bulletWallColision() // Checks for the bullet on wall collision
{
    for(let i = 0; i < terrain.length; i++)
    {
        for(let j = 0; j < terrain[i].length; j++)
        {
            for(let b = 0; b < bullets.length; b++)
            {
                if(terrain[i][j] != 0 && terrain[i][j].isTile)
                {
                    if(rectsIntersect(bullets[b], terrain[i][j]))
                    {
                        bullets[b].isAlive = false;
                        gameScene.removeChild(bullets[b]);
                    }
                }
            }
        }
    }
}

function removeNotAlive() // Remvoes things that are not alive from the scene
{
    for(let i = 0; i < bullets.length; i++)
    {
        if(!bullets[i].isAlive)
        {
            gameScene.removeChild(bullets[i]);
        }
    }
    
    for(let i = 0; i < enemies.length; i++)
    {
        if(!enemies[i].isAlive)
        {
            gameScene.removeChild(enemies[i]);
        }
    }
}

function rectsIntersect(a,b) // Checks for square collisions
{
    var ab = a.getBounds();
    var bb = b.getBounds();
    return ab.x + ab.width > bb.x && ab.x < bb.x + bb.width && ab.y + ab.height > bb.y && ab.y < bb.y + bb.height;
}