using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
class Shooter : Enemy
{
    private Player player;
    private Vector2 start, target;
    private int timer, delay;
    private Sound shootSound;
    public Shooter(float x, float y, float speedX, float speedY, Player player, int delay) : base(x, y, speedX, speedY, 1, "crawlingthing.png", 6, 1)
    {
        this.player = player;
        start = new Vector2(x, y);
        target = new Vector2(player.x, player.y);
        timer = Time.time;
        this.delay = delay;
        this.player = player;
        shootSound = new Sound("sounds/Crazy Old Lady, XBOX-style!.mp3");
    }

    private void Update() {
        shoot();
        start.x = x;
        start.y = y;
        target.x = player.x;
        target.y = player.y;
        
    }

    private void shoot() {
        if (y > 0 && y < game.height)
        {
            if (Time.time > timer + delay)
            {
                Vector2 diff = new Vector2(target.x - start.x, target.y - start.y);
                float length = Mathf.Sqrt((float)Mathf.Pow(diff.x, 2) + (float)Math.Pow(diff.y, 2));
                //Console.WriteLine(diff);
                diff.x /= length;
                diff.y /= length;
                EnemyBullet b = new EnemyBullet(width / 2, 0, diff.x * 5, diff.y * 5, 0);
                AddChild(b);
                timer = Time.time;
                shootSound.Play();
            }
        }
    }
}

