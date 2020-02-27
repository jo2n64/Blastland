using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
class Boss3 : Boss
{
    private Vector2 position, target;
    private Player player;
    private int timer, delay;
    private List<Missile> missiles;
    public Boss3(float x, float y, float scale, Player player, int delay, int health) : base(x, y, scale, player, health)
    {
        missiles = new List<Missile>();
        this.player = player;
        this.health = health;
        position = new Vector2(x, y);
        target = new Vector2(player.x, player.y);
        timer = Time.time;
        this.delay = delay;
    }

    private void Update() {
        if (anim.y + anim.height > 0)
        {
            position.x = x;
            position.y = y;
            target.x = player.x;
            target.y = player.y;
            shoot();
            if (health <= 0) removeMissiles();
        }
    }

    public void removeMissiles() { 
        for(int i = 0; i < missiles.Count; i++)
        {
            Missile m = missiles[i];
            m.LateDestroy();
            missiles.Remove(m);          
        }
    }

    private void shoot() { 
        if(Time.time >= timer + delay)
        {
            Vector2 diff = new Vector2(target.x - position.x, target.y - position.y);
            float length = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y);
            diff.x /= length;
            diff.y /= length;
            Missile m = new Missile(x,y,diff.x * 5, diff.y * 5,player);
            missiles.Add(m);
            m.SetScaleXY(4);
            game.AddChild(m);
            timer = Time.time;
        }
    }

}

