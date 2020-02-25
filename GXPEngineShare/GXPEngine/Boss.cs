using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Boss : Enemy
{
    private int delay, timer;
    private Player player;
    public Boss(float x, float y, float speedX, float speedY, float scale, Player player) : base(x, y, speedX, speedY, scale)
    {
        health = 10;
        delay = 2000;
        timer = Time.time;
        this.player = player;
    }

    private void Update() {
        if (y + height > 0)
        {
            spawnEnemy();
        }
    }

    private void spawnEnemy() {
        if (Time.time > timer + delay)
        {
            if (y > 0)
            {
                Follower e = new Follower(0, 0, 0, 5, 0.25f, player);
                AddChild(e);
                timer = Time.time;
            }
        }
    }
}
