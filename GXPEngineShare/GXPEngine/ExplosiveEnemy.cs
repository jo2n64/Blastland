using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class ExplosiveEnemy : Enemy
{
    private int enlargeTimer, enlargeDelay;
    private int delay, timer;
    //private AnimationSprite anim;
    private Sound explosionSound;
    public ExplosiveEnemy(float x, float y, float speedX, float speedY) : base(x, y, speedX, speedY, 1, "explodingenemy.png", 16, 1)
    {
        explosionSound = new Sound("sounds/Enemy_exploding459973__florianreichelt__huge-explosion.wav");
        // AddChild(anim);
        AddChild(colliderSprite);
        colliderSprite.SetXY(0, 32);
        colliderSprite.SetOrigin(width / 2, height / 2);
        enlargeTimer = Time.time;
        enlargeDelay = 50;
        timer = Time.time;
        delay = 62;
        anim.SetOrigin(width * 2, height * 2);
    }

    private void Update() {
        handleAnim();
    }

    private void handleAnim() {
        if (isDead)
        {
            if (Time.time >= delay + timer)
            {
                anim.NextFrame();
                colliderSprite.scaleX += 0.1f;
                colliderSprite.scaleY += 0.1f;
                timer = Time.time;
            }
            if (anim.currentFrame >= 15)
            {
                LateDestroy();
                explosionSound.Play();
            }
        }
    }


    
}

