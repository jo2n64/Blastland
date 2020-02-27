using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
class Missile : Sprite
{
    private Player player;
    private Vector2 position, target;
    private float speedX, speedY;
    public Missile(float x, float y, float speedX, float speedY, Player player) : base("0.png")
    {
        SetXY(x, y);
        this.player = player;
        this.speedX = speedX;
        this.speedY = speedY;
        position = new Vector2(x, y);
        target = new Vector2(player.x, player.y);
    }

    public void Update() {
        position.x = x;
        position.y = y;
        target.x = player.x;
        target.y = player.y;
        follow();
        checks();
    }

    private void checks() {
        foreach (GameObject g in GetCollisions())
        {
            if (g is Bullet)
            {
                LateDestroy();
                g.LateDestroy();
            }
        }
    }

    private void follow() {
        float dX = target.x - position.x;
        float dY = target.y - position.y;
        float normal = Mathf.Sqrt(dX * dX + dY * dY);
        Move(dX / normal * speedX, dY / normal * speedY);
    }
}

