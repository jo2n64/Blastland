using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class BounceEnemy : Enemy
{
    private Sprite collisionChecker;
    private int timer, delay;
    public BounceEnemy (float x, float y, float speedX, float speedY, float scale) : base(x, y, speedX, speedY, scale, "crawlingthing.png", 6, 1)
    {
        timer = Time.time;
        delay = 100;
        anim.SetOrigin(width / 2, height / 2);
        anim.SetXY(-80, 0);
        AddChild(anim);
        anim.Mirror(true, false);
       // AddChild(collisionChecker);
    }

    private void Update() {
        move();
        if(Time.time >= timer + delay)
        {
            anim.NextFrame();
            timer = Time.time;
        }
        bounce();
    }

    private void bounce() { 
        foreach(GameObject g in anim.GetCollisions())
        {
            if (g is Wall)
            {
                speedX = -speedX;
                anim.Mirror(true, false);
            }
            if (g is Wall2)
            {
                speedX = -speedX;
                anim.Mirror(false, false);
            }
        }
    }

}

