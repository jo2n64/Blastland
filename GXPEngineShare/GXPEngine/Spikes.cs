using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Spikes : Sprite
{
    private AnimationSprite anim;
    private int timer, delay;
    public Spikes(int x, int y) : base("assets/sprite_3.png")
    {
        alpha = 0f;
        SetXY(x, y);
        timer = Time.time;
        delay = 500;
        
        anim = new AnimationSprite("spikes.png", 2, 1);
        anim.SetOrigin(0, height * 2);
        anim.SetXY(0, 30);
        anim.SetScaleXY(0.5f, 0.5f);
        AddChild(anim);
    }

    private void Update() { 
        if(Time.time > timer + delay)
        {
            anim.NextFrame();
            timer = Time.time;
        }
    }
}

