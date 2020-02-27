using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Bullet : Sprite
{
    private float speedY, speedX;
    private int damage;

    public int Damage { get => damage; set => damage = value; }

    public Bullet(float x, float y, float speedX, float speedY, float rotation) : base("bullet.png")
    {
        damage = 1;
        this.rotation = rotation;
        this.x = x;
        this.y = y;
        SetXY(x, y);
        SetOrigin(width / 2, 0);
        this.speedX = speedX;
        this.speedY = speedY;
    }

    protected void Update() {
        move();
        checks();
    }

    private void move() {
        float yModifier = speedY;
        float xModifier = speedX;
        Move(xModifier, yModifier);
    }

    private void checks() {
        if (y > game.height || y < 0 || x < 0 || x > game.width)
        {
            LateDestroy();
            Console.WriteLine("destroyed");
        }
    }
}

