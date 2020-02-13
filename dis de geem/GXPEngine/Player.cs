using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
class Player : GameObject
{
    private float speedX, speedY;
    private int left, right;
    private int up;
    private int shootDelay, shootTimer;
    private Sprite sprite;
    private Vector2 direction = new Vector2(0.0f, 0.0f);

    //need to add a sprite object to be rotated

    public int ShootDelay { get => shootDelay; set => shootDelay = value; }
    public int ShootTimer { get => shootTimer; set => shootTimer = value; }
    public float SpeedX { get => speedX; set => speedX = value; }
    public float SpeedY { get => speedY; set => speedY = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public Vector2 Direction { get => direction; set => direction = value; }

    public Player(float speedX, float speedY) : base()
    {
        sprite = new Sprite("square.png");
        sprite.SetOrigin(sprite.width / 2.0f, sprite.height / 2.0f);
        shootDelay = 100;
        shootTimer = Time.time;
        this.speedX = speedX;
        this.speedY = speedY;
        left = 0;
        right = 0;
        up = 0;
        AddChild(sprite);
    }

    private void Update()
    {
        move();
        handleInput();
        checks();
    }

    private void setVector(float dirX, float dirY)
    {
        direction.x = dirX;
        direction.y = dirY;
    }

    private void move()
    {
        Move(speedX * direction.x, speedY * direction.y);
    }

    public void reposition(float x, float y)
    {
        SetXY(x, y);
    }

    private void checks()
    {
        foreach (GameObject g in sprite.GetCollisions())
        {
            if (g is Wall)
            {
                Wall wall = g as Wall;
                if (x < wall.x + wall.width + sprite.width / 2) x = wall.x + wall.width + sprite.width / 2;
                if (x + sprite.width > wall.x) x = wall.x - sprite.width / 2;
            }

            if (g is Enemy)
            {
                reposition(game.width / 2, game.height - 100);
            }

            //if enemy is reappearing spikes, when the enemy's alpha is 1(anim frame for the future) apply collision effect
        }


        if (x < 0) x = 0;
        if (x + sprite.width > game.width) x = game.width - sprite.width;
        if (y + sprite.height / 2 > game.height) y = game.height - sprite.height / 2;
    }
   

    private void handleInput()
    {
        if (Input.GetKey(Key.LEFT))
        {
            setVector(-1.0f, 0.0f);
            sprite.rotation = -90.0f;
        }
        
        if (Input.GetKey(Key.RIGHT))
        {
            setVector(1.0f, 0.0f);
            sprite.rotation = 90.0f;
        }
        if (Input.GetKey(Key.UP))
        {
            setVector(0.0f, -1.0f);
            sprite.rotation = 0.0f;
        }
        if (Input.GetKey(Key.UP) && Input.GetKey(Key.LEFT))
        {
            setVector(-0.7f, -0.7f);
            sprite.rotation = -45.0f;
        }
        if (Input.GetKey(Key.UP) && Input.GetKey(Key.RIGHT))
        {
            setVector(0.7f, -0.7f);
            sprite.rotation = 45.0f;
        }
        if (Input.GetKey(Key.DOWN))
        {
            setVector(0.0f, 1.0f);
            sprite.rotation = 180.0f;
        }
        if (Input.GetKey(Key.DOWN) && Input.GetKey(Key.LEFT))
        {
            setVector(-0.7f, 1.0f);
            sprite.rotation = -90f - 45f;
        }
        if (Input.GetKey(Key.DOWN) && Input.GetKey(Key.RIGHT))
        {
            setVector(0.7f, 1.0f);
            sprite.rotation = 90f + 45f;
        }
        if (Input.GetKeyUp(Key.LEFT) || Input.GetKeyUp(Key.RIGHT) || Input.GetKeyUp(Key.UP) || Input.GetKeyUp(Key.DOWN))
        {
            setVector(0f, 0f);
            sprite.rotation = 0f;
        }

    }
}

