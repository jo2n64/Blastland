using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
class Follower : Enemy
{
    private Player targetPlayer;
    private Vector2 position;
    private Vector2 playerPos;
    private int delay, timer;
    public Follower(float x, float y, float speedX, float speedY, float scale, Player targetPlayer) : base(x, y, speedX, speedY, scale, "plantspritesheet.png", 6, 1)
    {
        this.targetPlayer = targetPlayer;
        delay = 150;
        timer = Time.time;
        AddChild(colliderSprite);
        position = new Vector2(x, y);
        playerPos = new Vector2(targetPlayer.x, targetPlayer.y);
        anim.SetOrigin(width, height);
        anim.SetScaleXY(0.25f);
    }

    private void Update() {
        if (y + height > 0)
        {
            position.x = x;
            position.y = y;
            playerPos.x = targetPlayer.x;
            playerPos.y = targetPlayer.y;
            follow();
            handleAnim();
        }
    }

    private void handleAnim() { 
        if(Time.time >= timer + delay)
        {
            anim.NextFrame();
            timer = Time.time;
        }
    }

    private void follow() {
        float dX = playerPos.x - position.x;
        float dY = playerPos.y - position.y;
        float normal = Mathf.Sqrt(dX * dX + dY * dY);
        //dX += (normal / speedX);
        //dY += (normal / speedY);
        Move(dX/normal * speedX, dY/normal * speedY);
    }
}

