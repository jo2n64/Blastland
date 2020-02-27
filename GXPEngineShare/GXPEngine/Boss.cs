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
    private MyGame game;
    public Boss(float x, float y, float scale, Player player, int health, MyGame game) : base(x, y, 0, 0, scale, "boss1.png", 3, 1)
    {
        //sprite = new AnimationSprite("boss1.png", 3, 1);
        //base.scaleX = 6f;
        this.health = health;
        spawns = new List<Follower>();
        anim.SetScaleXY(0.20f, 0.20f);
        anim.SetOrigin(width, height / 2);
        AddChild(colliderSprite);
        colliderSprite.SetOrigin(0, anim.height/2);
        colliderSprite.SetXY(-32f, 0);
        this.health = health;
        delay = 2000;
        timer = Time.time;
        animDelay = 500;
        animTimer = Time.time;
        this.player = player;
        this.game = game;
    }

    private void Update() {
        if (y + anim.height + 100 > 0)
        {
            spawnEnemy();
            handleAnim();
            //Console.WriteLine("u working");
            if (health <= 0 || player.Lives <= 0)
            {
                removeSpawns();
                Console.WriteLine("well yes but no");
            }
        }
    }

    public void removeSpawns() { 
        for(int i = spawns.Count-1; i >= 0; i--)
        {
            Follower e = spawns[i];
            spawns.Remove(e);
            game.levels[game.Level].Enemies.Remove(e);
            e.LateDestroy();
            Console.WriteLine("enemy destroy");
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
            if (y > 0 && player.Lives > 0)
            {
                Follower e = new Follower(x, y, 3, 3, 1f, player);
                spawns.Add(e);
                game.levels[game.Level].Enemies.Add(e);
                game.AddChild(e);
                timer = Time.time;
                Console.WriteLine("spawn");
            }
        }
    }
}
