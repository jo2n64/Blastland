using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Healthbar : GameObject
{
    private Boss boss;
    private Sprite healthbar;
    private Sprite health;
    private int segments;
    public Healthbar(float x, float y, Boss boss) : base()
    {
        SetXY(x, y);
        healthbar = new Sprite("healthbar2.png");
        AddChildAt(healthbar, 1);
        segments = 15;
        health = new Sprite("health.png");
        health.SetXY(50, healthbar.height/2 - 30);
        health.scaleX = segments;
        AddChildAt(health, 0);
        this.boss = boss;
    }

    public Sprite Health { get => health; set => health = value; }

    private void Update() { 
        
    }

    public void setBoss(Boss boss)
    {
        this.boss = boss;
        health.scaleX = segments;
    }

}

