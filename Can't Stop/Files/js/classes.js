class Player extends PIXI.Sprite 
{
    constructor(x = 80, y = 80, direction = "none")
    {
        super(PIXI.loader.resources["images/player.png"].texture);
        this.anchor.set(0.5);
        this.x = x;
        this.y = y;
        this.firingSpeed = 0.5;
        this.direction = direction;
    }
    
    move(direction) // Moves the player based on the direction it has
    {
        if(direction == "right")
            {
                this.x += 2;
            }
        else if(direction == "left")
            {
                this.x -= 2;
            }
        else if(direction == "up")
            {
                this.y -= 2;
            }
        else if(direction =="down")
            {
                this.y +=2;
            }
    }
    
    changeDirection(newDirection) // Changes the player's direction and rotation
    {
        this.direction = newDirection;
        if(newDirection == "right")
            {
                this.rotation = 0.5 * 3.14;
            }
        else if(newDirection == "left")
            {
                this.rotation = 1.5 * 3.14;
            }
        else if(newDirection == "up")
            {
                this.rotation = 0;
            }
        else if(newDirection =="down")
            {
                this.rotation = 3.14;
            }
    }
    
    reverse() // Makes the player go the other way when it runs into a wall
    {
        if(this.direction == "right")
            {
                this.direction = "none";
                this.x -= 3;
            }
        else if(this.direction == "left")
            {
                this.direction = "none";
                this.x += 3;
            }
        else if(this.direction == "up")
            {
                this.direction = "none";
                this.y += 3;
                this.rotation = 0;
            }
        else if(this.direction == "down")
            {
                this.direction = "none";
                this.y -= 3;
            }
    }
    
    fire(direction) // Fires a bullet in the specified direction
    {
        let b;
        if(this.firingSpeed < 0.5) return;
        if(direction == "right")
        {
            this.rotation = 0.5 * 3.14;
            b = new Bullet(0x999900, player.x, player.y);
            b.fwd.x = 1;
            b.fwd.y = 0;
            b.friendly = true;
            bullets.push(b);
            gameScene.addChild(b);
            this.firingSpeed = 0;
        }
        else if(direction == "left")
        {
            this.rotation = 1.5 * 3.14;
            b = new Bullet(0x999900, player.x, player.y);
            b.fwd.x = -1;
            b.fwd.y = 0;
            b.friendly = true;
            bullets.push(b);
            gameScene.addChild(b);
            this.firingSpeed = 0;
        }
        else if(direction == "up")
        {
            this.rotation = 0;
            b = new Bullet(0x999900, player.x, player.y);
            b.fwd.x = 0;
            b.fwd.y = -1;
            b.friendly = true;
            bullets.push(b);
            gameScene.addChild(b);
            this.firingSpeed = 0;
        }
        else if(direction == "down")
        {
            this.rotation = 3.14;
            b = new Bullet(0x999900, player.x, player.y);
            b.fwd.x = 0;
            b.fwd.y = 1;
            b.friendly = true;
            bullets.push(b);
            gameScene.addChild(b);
            this.firingSpeed = 0;
        }
    }
}

class Bullet extends PIXI.Graphics
{
    constructor(color=0xFFFF00, x=0, y=0, friendly = false)
    {
        super();
        this.beginFill(color);
        this.drawRect(-2, -3, 4, 6);
        this.endFill();
        this.x = x;
        this.y = y;
        this.isAlive = true;
        this.friendly = friendly;
        this.fwd = {x:0, y:-1};
        this.speed = 400;
        this.isAlive = true;
        Object.seal(this);
    }
    
    move(dt=1/60)
    {
        this.x += this.fwd.x * this.speed * dt;
        this.y += this.fwd.y * this.speed * dt;
    }
}

class Level extends PIXI.Graphics
{
    constructor(levelNum, playerX, playerY)
    {
        super();
        this.enemies = [];
        this.checkpoint;
        this.playerX = playerX;
        this.playerY = playerY;
        this.terrain = this.setTiles(levelNum);
        this.beginFill();
        this.endFill();
    }
    
    setTiles(levelNum) // Sets the tile in an array for the level
    {
        let temp;
        let tempArray =  // This is to keep the original level arrays intact
        [
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
        ];
        for(let i = 0; i < levelNum.length; i++)
        {
            for(let j = 0; j < levelNum[i].length; j++)
            {
                if(levelNum[i][j] == 1) // Wall
                {
                    temp = new Tile((j * 32), (i * 32))
                    tempArray[i][j] = temp;
                }
                else if(levelNum[i][j] == 2) // Checkpoint
                {
                    temp = new LevelEnd((j * 32), (i * 32));
                    tempArray[i][j] = temp;
                    this.checkpoint = temp;
                }
                else if(levelNum[i][j] == 3) // Enemy firing up
                {
                    temp = new Enemy((j * 32) + 16, (i * 32) + 16, {x:0, y:-1});
                    tempArray[i][j] = temp;
                    this.enemies.push(temp);
                }
                else if(levelNum[i][j] == 4) // Enemy firing down
                {
                    temp = new Enemy((j * 32) + 16, (i * 32) + 16, {x:0, y:1});
                    temp.rotation = 3.14;
                    tempArray[i][j] = temp;
                    this.enemies.push(temp);
                }
                else if(levelNum[i][j] == 5) // Enemy firing right
                {
                    temp = new Enemy((j * 32) + 16, (i * 32) + 16, {x:1, y:0});
                    temp.rotation = 0.5 * 3.14;
                    tempArray[i][j] = temp;
                    this.enemies.push(temp);
                }
                else if(levelNum[i][j] == 6) // Enemy firing left
                {
                    temp = new Enemy((j * 32) + 16, (i * 32) + 16, {x:-1, y:0});
                    temp.rotation = 1.5 * 3.14;
                    tempArray[i][j] = temp;
                    this.enemies.push(temp);
                }
                else // Empty space
                {
                    temp = 0;
                    tempArray[i][j] = temp;
                }
            }
        }        
        return tempArray;
    }
    
    checkCollisions(player) // checks collisions with player and takes them out of the wall
    {
        for(let i = 0; i < this.terrain.length; i++)
        {
            for(let j = 0; j < this.terrain[i].length; j++)
            {
                if(this.terrain[i][j] != 0 && this.terrain[i][j].isAlive)
                {
                    if(rectsIntersect(player, this.terrain[i][j]))
                    {
                        player.reverse();
                        return true;
                    }
                }
            }
        }
        
        return false;
    }
}

class Tile extends PIXI.Sprite
{
    constructor(x = 0, y = 0)
    {
        super(PIXI.loader.resources["images/tile.png"].texture);
        this.x = x;
        this.y = y;
        this.isAlive = true;
        this.isTile = true;
    }
}

class Enemy extends PIXI.Sprite
{
    constructor(x = 0, y = 0, bFwd = {x: 0, y: -1})
    {
        super(PIXI.loader.resources["images/enemy.png"].texture)
        this.anchor.set(0.5);
        this.x = x;
        this.y = y;
        this.isAlive = true;
        this.firingSpeed = 60;
        this.bFwd = bFwd;
        this.isTile = false;
    }
    
    fire() // Fires in its set direction
    {
        if(this.firingSpeed >= 60)
        {
            let b = new Bullet(0x999900, this.x, this.y);
            b.fwd = this.bFwd;
            bullets.push(b);
            gameScene.addChild(b);
            this.firingSpeed = 0;
        }
        else
        {
            this.firingSpeed++;    
        }
    }
}

class LevelEnd extends PIXI.Sprite // The checkpoints
{
    constructor(x, y)
    {
        super(PIXI.loader.resources["images/level-end.png"].texture);
        this.x = x;
        this.y = y;
        this.isAlive = true;
        this.isTile = false;
    }
}
