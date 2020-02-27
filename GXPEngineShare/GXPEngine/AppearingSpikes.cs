using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
class AppearingSpikes : Enemy
{
    private int appearDelay, appearTimer;
    private int delay, timer;
    //private AnimationSprite anim;
    private bool hasAppeared;

    public bool HasAppeared { get => hasAppeared; set => hasAppeared = value; }

    public AppearingSpikes(float x, float y, int appearDelay) : base(x, y, 0, 0, 1, "appearingspikes.png", 18, 1)
    {
        this.appearDelay = appearDelay;
        this.appearDelay = appearDelay;
        colliderSprite.SetXY(0, height * 2);
        AddChild(colliderSprite);
        colliderSprite.scaleY = -2f;
        delay = 100;
        timer = Time.time;
        anim = new AnimationSprite("appearingspikes.png", 18, 1);
        anim.SetOrigin(anim.width - width, anim.height - height);
        anim.SetXY(120, 40);
        AddChild(anim);
        appearTimer = Time.time;
        hasAppeared = false;
        createCollider();
    }

    private void Update() {
        //appear();
        if(Time.time >= timer + delay)
        {
            anim.NextFrame();
            timer = Time.time;
        }
        if (anim.currentFrame >= 7 && anim.currentFrame <= 14)
        {
            hasAppeared = true;
            colliderSprite.isOn = true;
        }
        else
        {
            hasAppeared = false;
            colliderSprite.isOn = false;
        }
    }


}
