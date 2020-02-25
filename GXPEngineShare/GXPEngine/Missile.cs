using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
class Missile : Sprite
{
    private float speedX, speedY;
    private Player player;
    private Vector2 pos;
    private Vector2 target;
    Vector2 playerPos;
    public Missile(float x, float y, float speedX, float speedY, Player player) : base("0.png")
    {
        this.player = player;
        playerPos = new Vector2(player.x, player.y);
        this.speedX = speedX;
        this.speedY = speedY;
        SetXY(x, y);
        pos = new Vector2(x, y);
    }

    private void Update() {
        pos.x = x;
        pos.y = y;
        target.x = playerPos.x;
        target.y = playerPos.y;
        follow();
        checks();
    }
    private void checks() {
        if (y > game.height || y < 0) LateDestroy();
        foreach(GameObject g in GetCollisions())
        {
            if(g is Bullet || g is Player || g is Wall || g is Wall2)
            {
                LateDestroy();
            }
        }
    }
    private void follow() {
        float dX = target.x - pos.x;
        float dY = target.y - pos.y;
        float normal = Mathf.Sqrt(dX * dX + dY * dY);
        Move(dX / normal * speedX, dY / normal * speedY);
    }
}

