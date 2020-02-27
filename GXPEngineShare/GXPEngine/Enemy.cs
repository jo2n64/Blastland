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

    protected AnimationSprite anim;

    public bool isDead;
    public int Health { get => health; set => health = value; }
    public AnimationSprite Anim { get => anim; set => anim = value; }

    //when enemy explodes make the radius bigger (scale increased), change animation to explosion, wait for 500ms or smth, then destroy enemy
    public Enemy(float x, float y, float speedX, float speedY, float scale, string animPath, int cols, int rows) : base("colors.png")
    {
        alpha = 0f;
        isDead = false;
        health = 9;
        SetXY(x, y);
        SetScaleXY(scale);
        anim = new AnimationSprite(animPath, cols, rows);
        AddChild(anim);
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

