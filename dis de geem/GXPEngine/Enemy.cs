using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Enemy : Entity
{
    private float speedX, speedY;
    public bool isDead;

    //when enemy explodes make the radius bigger (scale increased), change animation to explosion, wait for 500ms or smth, then destroy enemy
    public Enemy(float x, float y, float speedX, float speedY, float scale) : base("colors.png", 1, 1, 100)
    {
        isDead = false;
        SetXY(x, y);
        SetScaleXY(scale);
        this.speedX = speedX;
        this.speedY = speedY;
    }

    private void Update() {
        moveX();
        moveY();
    }

    private void moveX() {
        float xModifier = speedX * Time.deltaTime;
        Move(xModifier, 0);
    }

    private void moveY() {
        float yModifier = speedY * Time.deltaTime / 60f;
        Move(0, yModifier);
    }

    protected override void handleAnimation()
    {
        throw new NotImplementedException();
    }
}

