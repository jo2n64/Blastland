using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Statsbar : GameObject
{
    Sprite bar, amount;
    private int thingToAmount;
    Player player;
    public Statsbar(float x, float y, Player player, string barPath, string amountPath, int thingToAmount) : base()
    {
        SetXY(x, y);
        this.thingToAmount = thingToAmount;
        bar = new Sprite(barPath);
        amount = new Sprite(amountPath);
        amount.SetXY(50, bar.height / 2 - 30);
        AddChild(amount);
        AddChild(bar);
        
        this.player = player;
        amount.scaleX = thingToAmount;
    }

    public int ThingToAmount { get => thingToAmount; set => thingToAmount = value; }

    private void Update() {
        amount.scaleX = thingToAmount;
    }

}

