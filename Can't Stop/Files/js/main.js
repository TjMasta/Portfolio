"use strict";
const app = new PIXI.Application(1024,512);
document.body.querySelector("article").appendChild(app.view);

const sceneWidth = app.view.width;
const sceneHeight = app.view.height;

let stage;

const storedTime = localStorage.getItem("txd7163-time"); // Stores the fastest time in local storage
let fastestTime = Number.MAX_SAFE_INTEGER;

if(storedTime)
{
    fastestTime = storedTime;
}

// Set ups variables for the game
let bullets = [];
let enemies = [];
let checkpoint;
let terrain;
let levels = [level1, level2, level3, level4, level5, level6, level7];
let currentLevelNum = 0;
let startScene;
let levelSelector;
let gameScene, player, levelEnd, lives, shootSound, hitSound, currentLevel, lifeLabel, timeLabel, inGameTime;
let gameOverScene;
let gameEndScene;
let backScene;
let paused = true;
lives = 0;

PIXI.loader.
add(["images/player.png", "images/tile.png", "images/enemy.png", "images/level-end.png", "images/background.png"]).
on("progress",e=>{console.log(`progress=${e.progress}`)}).
load(setup);



function setup()
{
    stage = app.stage;
    
    // Gives the game a background image
    backScene = new PIXI.Container();
    backScene.visible = true;
    stage.addChild(backScene);
    let backgroundImage = new PIXI.Sprite(PIXI.loader.resources["images/background.png"].texture);
    backScene.addChild(backgroundImage);
    
    // Beginning screen
    startScene = new PIXI.Container();
    stage.addChild(startScene);
    
    // Game Screen
    gameScene = new PIXI.Container();
    gameScene.visible = false;
    stage.addChild(gameScene);
    
    // Game over screen
    gameOverScene = new PIXI.Container();
    gameOverScene.visible = false;
    stage.addChild(gameOverScene);
    
    // Win screen
    gameEndScene = new PIXI.Container();
    gameEndScene.visible = false;
    stage.addChild(gameEndScene);
    
    createLabelsAndButtons();
    
    app.ticker.add(gameLoop);
}

function createLabelsAndButtons()
{
    let buttonStyle = new PIXI.TextStyle({
        fill: 0x00FF00,
        fontSize: 48,
        fontFamily: "Futura"
    });
    
    let textStyle = new PIXI.TextStyle({
        fill: 0xFFFFFF,
        fontSize: 24,
        fontFamily: "Futura",
        strokeThickness: 4
    });
    
    let startLabel1 = new PIXI.Text("Can't Stop");
    startLabel1.style = new PIXI.TextStyle({
        fill: 0xFFFFFF,
        fontSize: 80,
        fontFamily: 'Futura',
        stroke: 0x0000FF,
        strokeThickness: 6
    });
    startLabel1.x = 350;
    startLabel1.y = 50;
    startScene.addChild(startLabel1);
    
    let highScoreLabel = new PIXI.Text();
    highScoreLabel.style = textStyle;
    highScoreLabel.x = 80;
    highScoreLabel.y = 300;
    if(fastestTime == Number.MAX_SAFE_INTEGER)
    {
        highScoreLabel.text = "Fastest time: none";
    }
    else
    {
        highScoreLabel.text = `Fastest time: ${fastestTime}s`;
    }
    startScene.addChild(highScoreLabel);
    
    let directions = new PIXI.Text("Use W, A, S, D, to move.\nUse the arrow keys to fire.\nKill all enemies to move to the next area.\nOnce you start moving you can't stop.");
    directions.style = textStyle;
    directions.x = 600;
    directions.y = 250;
    startScene.addChild(directions);
    
    let startButton = new PIXI.Text("Start");
    startButton.style = buttonStyle;
    startButton.x = 450;
    startButton.y = sceneHeight - 80;
    startButton.interactive = true;
    startButton.buttonMode = true;
    startButton.on("pointerup", startGame);
    startButton.on('pointerover', e=>e.target.alpha = 0.7);
    startButton.on('pointerout', e=>e.currentTarget.alpha = 1.0);
    startScene.addChild(startButton);
    
    let gameOverLabel = new PIXI.Text("Game Over");
    gameOverLabel.style = new PIXI.TextStyle({
        fill: 0xFFFFFF,
        fontSize: 80,
        fontFamily: 'Futura',
        stroke: 0x0000FF,
        strokeThickness: 6
    });
    gameOverLabel.x = 350;
    gameOverLabel.y = 50;
    gameOverScene.addChild(gameOverLabel);
    
    let playAgainButton = new PIXI.Text("Play Again?");
    playAgainButton.style = buttonStyle;
    playAgainButton.x = 450;
    playAgainButton.y = sceneHeight - 80;
    playAgainButton.interactive = true;
    playAgainButton.buttonMode = true;
    playAgainButton.on("pointerup",startGame);
    playAgainButton.on('pointerover',e=>e.target.alpha = 0.7);
    playAgainButton.on('pointerout',e=>e.currentTarget.alpha = 1.0);
    gameOverScene.addChild(playAgainButton);
    
    let gameWinLabel = new PIXI.Text("You Win!");
    gameWinLabel.style = new PIXI.TextStyle({
        fill: 0xFFFFFF,
        fontSize: 80,
        fontFamily: 'Futura',
        stroke: 0x0000FF,
        strokeThickness: 6
    });
    gameWinLabel.x = 350;
    gameWinLabel.y = 50;
    gameEndScene.addChild(gameWinLabel);
    
    lifeLabel = new PIXI.Text();
    lifeLabel.style = textStyle;
    lifeLabel.x = 5;
    lifeLabel.y = 4;
    gameScene.addChild(lifeLabel);
    decreaseLifeBy(0);
    
    timeLabel = new PIXI.Text();
    timeLabel.style = textStyle;
    timeLabel.x = 880;
    timeLabel.y = 4;
    gameScene.addChild(timeLabel);
    
    let startAgainButton = new PIXI.Text("Main Menu");
    startAgainButton.style = buttonStyle;
    startAgainButton.x = 400;
    startAgainButton.y = sceneHeight - 80;
    startAgainButton.interactive = true;
    startAgainButton.buttonMode = true;
    startAgainButton.on("pointerup",restartGame);
    startAgainButton.on('pointerover',e=>e.target.alpha = 0.7);
    startAgainButton.on('pointerout',e=>e.currentTarget.alpha = 1.0);
    gameEndScene.addChild(startAgainButton);
}

// Sets up the start of the game
function startGame()
{
    startScene.visible = false;
    gameOverScene.visible = false;
    gameScene.visible = true;
    currentLevelNum = 0;
    inGameTime = 0;
    decreaseLifeBy(-3);
    player = new Player();
    gameScene.addChild(player);
    loadLevel();
    paused = false;
}

// Restarts game on the game over screen
function restartGame()
{
    startScene.visible = true;
    gameEndScene.visible = false;
}

// Loads each level
function loadLevel()
{
    gameScene.removeChild(lifeLabel);
    gameScene.addChild(timeLabel);
    
    currentLevel = new Level(levels[currentLevelNum], 46, 46);
    
    // Puts the player in position
    player.x = currentLevel.playerX;
    player.y = currentLevel.playerY;
    
    enemies = currentLevel.enemies;
    checkpoint = currentLevel.checkpoint;
    terrain = currentLevel.terrain;
    
    // Puts terrain in position
    for(let i = 0; i < terrain.length; i++)
    {
        for(let j = 0; j < terrain[i].length; j++)
        {
            if(terrain[i][j] != 0)
            {
                gameScene.addChild(terrain[i][j]);
            }
        }
    }
    
    // Puts enemies in position
    for(let en of enemies)
    {
        gameScene.addChild(en);
    }
    
    // Puts checkpoint in position
    gameScene.addChild(checkpoint);
    
    gameScene.addChild(currentLevel);
    
    gameScene.addChild(lifeLabel);
    gameScene.addChild(timeLabel);
}

function gameLoop()
{   
    if(paused) return;
    
    let dt = 1/app.ticker.FPS;
    if(dt > 1/12) dt = 1/12;
    increaseTime(dt);
    
    let amt = 6 * dt;
    
    if(player.firingSpeed < 0.5)
    {
        player.firingSpeed += dt;
    }
    
    window.onkeydown = (e) =>
    {
        checkKeys(e);
    }
    
    // Checks wall collisions with player
    if(!currentLevel.checkCollisions(player))
    {
        player.move(player.direction);
    }
    
    // Enemies fire
    for(let i = 0; i < enemies.length; i++)
    {
        enemies[i].fire();    
    }
    
    // Moves bullets and checks collisions with player
    for(let i = 0; i < bullets.length; i++)
    {
        bullets[i].move();
        
        if(rectsIntersect(player, bullets[i]) && !bullets[i].friendly)
        {
            gameScene.removeChild(bullets[i]);
            player.x = 46;
            player.y = 46;
            player.changeDirection("none");
            decreaseLifeBy(1);
            if(lives == 0)
                {
                    end();
                    return;
                }
        }
    }
    
    // Checks player bullest on enemies
    for(let i = 0; i < bullets.length; i++)
    {
        for(let j = 0; j < enemies.length; j++)
        {
            if(rectsIntersect(bullets[i], enemies[j]) && bullets[i].friendly)
            {
                bullets[i].isAlive = false;
                enemies[j].isAlive = false;
            }
        }
    }
    
    bulletWallColision();
    
    // Checks to see if all enemies are dead and the player has reached the checkpoint
    if(rectsIntersect(checkpoint, player) && enemies.length == 0)
    {
        currentLevelNum++;
        gameScene.removeChild(checkpoint)
        gameScene.removeChild(currentLevel);
        if(currentLevelNum >= levels.length) // If its the last level, show the win screen
        {
            win();
            return;
        }
        removeTerrain();
        loadLevel();
        return;
    }
    
    removeNotAlive(); // Gets rid of all enemies and bullets from their lists
    
    bullets = bullets.filter(b=>b.isAlive);
    enemies = enemies.filter(en=>en.isAlive);
}

// Shows the game over screen and resets the game
function end()
{
    gameScene.removeChild(player);
    gameScene.removeChild(currentLevel);
    
    gameScene.visible = false;
    gameOverScene.visible = true;
    
    bullets.forEach(b=>gameScene.removeChild(b));
    bullets = [];
    
    removeTerrain();
    
    currentLevel = null;
    
    paused = true;
}

// Shows the win screen and sets the new fastest time, if there is one
function win()
{
    gameScene.visible = false;
    gameEndScene.visible = true;
    
    gameScene.removeChild(player);
    gameScene.removeChild(currentLevel);
    
    bullets.forEach(b=>gameScene.removeChild(b));
    bullets = [];
    
    removeTerrain();
    
    currentLevel = null;
    
    if(inGameTime < fastestTime)
    {
        fastestTime = inGameTime;
        localStorage.setItem("txd7163-time", fastestTime)
    }
    
    paused = true;
}

// Looks for input
function checkKeys(e)
{
    switch(e.keyCode)
        {
            case 87:
                player.changeDirection("up");
                break;
                
            case 68:
                player.changeDirection("right");
                break;
                
            case 83:
                player.changeDirection("down");
                break;
                
            case 65:
                player.changeDirection("left");
                break;
                
            case 39:
                player.fire("right");
                break;
                
            case 38:
                player.fire("up");
                break;
                
            case 37:
                player.fire("left");
                break;
                
            case 40:
                player.fire("down");
                break;
        }
}