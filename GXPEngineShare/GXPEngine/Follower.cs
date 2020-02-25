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
    public Follower(float x, float y, float speedX, float speedY, float scale, Player targetPlayer) : base(x, y, speedX, speedY, scale)
    {
        this.targetPlayer = targetPlayer;
        position = new Vector2(x, y);
        playerPos = new Vector2(targetPlayer.x, targetPlayer.y);
    }

    private void Update() {
        position.x = x;
        position.y = y;
        playerPos.x = targetPlayer.x;
        playerPos.y = targetPlayer.y;
        follow();
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

