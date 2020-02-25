using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Enemy : Sprite
{
    protected float speedX, speedY;
    protected Level level;
    protected int health;

    public bool isDead;
    private AnimationSprite anim;
    public int Health { get => health; set => health = value; }

    //when enemy explodes make the radius bigger (scale increased), change animation to explosion, wait for 500ms or smth, then destroy enemy
    public Enemy(float x, float y, float speedX, float speedY, float scale) : base("colors.png")
    {
        isDead = false;
        health = 3;
        SetXY(x, y);
        SetScaleXY(scale);

        this.speedX = speedX;
        this.speedY = speedY;
    }

    private void Update()
    {
        move();
        //moveY();
    }

    protected void move()
    {
        float xModifier = speedX * Time.deltaTime / 60f;
        float yModifier = speedY * Time.deltaTime / 60f;
        Move(xModifier, yModifier);
    }

}

