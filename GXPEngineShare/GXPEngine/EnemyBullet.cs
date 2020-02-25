using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class EnemyBullet : Sprite
{
    private float speedY, speedX;
    private int damage;

    public EnemyBullet(float x, float y, float speedX, float speedY, float rotation) : base("0.png")
    {
        damage = 1;
        this.rotation = rotation;
        this.x = x;
        this.y = y;
        SetXY(x, y);
        this.speedX = speedX;
        this.speedY = speedY;
    }

    protected void Update()
    {
        move();
        checks();
    }

    private void move()
    {
        float yModifier = speedY;
        float xModifier = speedX;
        Move(xModifier, yModifier);
    }

    private void checks()
    {
       //if (y > game.height || y < 0) LateDestroy();
    }
}

