using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
class Boss3 : Boss
{
    private Player player;
    private Vector2 playerPos;
    private int timer, delay;
    public Boss3(float x, float y, float scale, Player player, int delay) : base(x, y, 0, 0, scale, player)
    {
        this.player = player;
        playerPos = new Vector2(player.x, player.y);
        timer = Time.time;
        this.delay = delay;
    }

    private void Update() {
        if (y + height > 0)
        {
            playerPos.x = player.x;
            playerPos.y = player.y;
            shoot();
        }
    }

    private void shoot() { 
        if(Time.time >= timer + delay)
        {
            Missile m = new Missile(0, 0, 3, 3, player);
            AddChild(m);
            timer = Time.time;
        }
    }
}

