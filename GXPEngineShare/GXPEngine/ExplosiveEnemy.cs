using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class ExplosiveEnemy : Enemy
{
    private int enlargeTimer, enlargeDelay;
    private int delay, timer;
    private AnimationSprite anim;
    private Sound explosionSound;
    public ExplosiveEnemy(float x, float y, float speedX, float speedY) : base(x, y, speedX, speedY, 1)
    {
        anim = new AnimationSprite("plantspriteshee2222t.png", 6, 1);
        explosionSound = new Sound("sounds/Enemy_exploding459973__florianreichelt__huge-explosion.wav");
        AddChild(anim);
        enlargeTimer = Time.time;
        enlargeDelay = 50;
        timer = Time.time;
        delay = 200;
    }

    private void Update() {
        checkIfDead();
        handleAnim();
    }

    private void handleAnim() { 
        if(Time.time > timer + delay)
        {
            anim.NextFrame();
            timer = Time.time;
        }
    }

    private void checkIfDead() {
        if (isDead)
        {
            if (Time.time > enlargeTimer + enlargeDelay && scale < 3)
            {
                scale += 0.1f;
                enlargeTimer = Time.time;
            }
            if (scale >= 3)
            {
                explosionSound.Play();
                LateDestroy();
            }
        }
    }

    
}

