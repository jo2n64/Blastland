using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class AppearingSpikes : Enemy
{
    private int appearDelay, appearTimer;
    private int delay, timer;
    private AnimationSprite anim;
    private bool hasAppeared;
    public AppearingSpikes(float x, float y, int appearDelay) : base(x, y, 0, 0, 1)
    {
        this.appearDelay = appearDelay;
        this.appearDelay = appearDelay;
        delay = 100;
        timer = Time.time;
        anim = new AnimationSprite("appearingspikes.png", 18, 1);
        anim.SetOrigin(anim.width - width, anim.height - height);
        AddChild(anim);
        appearTimer = Time.time;
        hasAppeared = false;
    }

    private void Update() {
        appear();
        if(Time.time >= timer + delay)
        {
            anim.NextFrame();
            timer = Time.time;
        }
        if (hasAppeared) alpha = 1f;
        if (!hasAppeared) alpha = 0f;
    }

    private void appear() {
        if (Time.time > appearDelay + appearTimer)
        {
            hasAppeared = !hasAppeared;
            appearTimer = Time.time;
        }
    }
}
