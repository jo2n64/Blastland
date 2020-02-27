using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Statsbar : GameObject
{
    Sprite bar, amount;
    private float thingToAmount;
    Player player;
    public Statsbar(float x, float y, Player player, string barPath, string amountPath, float thingToAmount) : base()
    {
        SetXY(x, y);
        this.thingToAmount = thingToAmount;
        bar = new Sprite(barPath);
        amount = new Sprite(amountPath);
        amount.SetXY(bar.width - 70, bar.height / 2 - 20);
        AddChild(amount);
        AddChild(bar);
        
        this.player = player;
        amount.scaleX = thingToAmount;
    }

    public float ThingToAmount { get => thingToAmount; set => thingToAmount = value; }
    public Sprite Amount { get => amount; set => amount = value; }

    private void Update() {
        amount.scaleX = thingToAmount;
    }

}

