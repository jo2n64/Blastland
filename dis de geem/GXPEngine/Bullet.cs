using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class Bullet : Sprite
{
    private float speedY, speedX;
    public Bullet(float x, float y, float speedX, float speedY, float rotation) : base("bullet.png")
    {
        this.rotation = rotation;
        this.x = x;
        this.y = y;
        SetXY(x, y);
        this.speedX = speedX;
        this.speedY = speedY;
    }

    private void Update() {
        moveY();
        //checks();
    }

    private void moveY() {
        float yModifier = speedY;
        Move(0, yModifier);
    }
}

