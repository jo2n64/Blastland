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
    protected Sprite blink;
    protected EnemyCollider colliderSprite;

    public bool isDead;
    public bool isHit;
    private int hitTimer, hitDelay;
    public int Health { get => health; set => health = value; }
    public AnimationSprite Anim { get => anim; set => anim = value; }
    public EnemyCollider ColliderSprite { get => colliderSprite; set => colliderSprite = value; }

    //when enemy explodes make the radius bigger (scale increased), change animation to explosion, wait for 500ms or smth, then destroy enemy
    public Enemy(float x, float y, float speedX, float speedY, float scale, string animPath, int cols, int rows) : base("colors.png", false)
    {
        alpha = 0f;
        isDead = false;
        health = 3;
        SetXY(x, y);
        SetScaleXY(scale);
        colliderSprite = new EnemyCollider(0,0);
        blink = new Sprite("health.png");
        //AddChild(collider);
        anim = new AnimationSprite(animPath, cols, rows);
        blink.alpha = 0f;
        AddChildAt(anim, 0);
        AddChildAt(blink,1);
        this.speedX = speedX;
        this.speedY = speedY;
    }

    private void Update()
    {
        move();
    }

    protected void blinkWhenHit() {
        if (Time.time >= hitTimer + hitDelay)
        {
            isHit = false;
        }
    }

    protected void move()
    {
        float xModifier = speedX * Time.deltaTime / 60f;
        float yModifier = speedY * Time.deltaTime / 60f;
        Move(xModifier, yModifier);
    }

}

