using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Boss : Enemy
{
    private int delay, timer;
    private int animDelay, animTimer;
    private AnimationSprite sprite;
    private List<Follower> spawns;
    private Player player;
    public Boss(float x, float y, float scale, Player player, int health) : base(x, y, 0, 0, scale, "boss1.png", 3, 1)
    {
        //sprite = new AnimationSprite("boss1.png", 3, 1);
        //base.scaleX = 6f;
        this.health = health;
        spawns = new List<Follower>();
        anim.SetScaleXY(0.20f, 0.20f);
        anim.SetOrigin(width, height / 2);
        this.health = health;
        delay = 2000;
        timer = Time.time;
        animDelay = 500;
        animTimer = Time.time;
        this.player = player;
    }

    private void Update() {
        if (anim.y + anim.height > 0)
        {
            spawnEnemy();
            handleAnim();
            if (health <= 0) removeSpawns();
        }
    }

    public void removeSpawns() { 
        for(int i = 0; i < spawns.Count; i++)
        {
            Follower e = spawns[i];
            e.LateDestroy();
            spawns.Remove(e);
        }
    }

    private void handleAnim() {
        if (Time.time >= animTimer + animDelay)
        {
            anim.NextFrame();
            animTimer = Time.time;
        }
    }
    
    private void spawnEnemy() {
        if (Time.time > timer + delay)
        {
            if (y > 0)
            {
                Follower e = new Follower(x, y, 3, 3, 0.25f, player);
                spawns.Add(e);
                AddChild(e);
                timer = Time.time;
            }
        }
    }
}
