using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

class EnemyCollider : Sprite
{
    public bool isOn;
    public EnemyCollider(float x, float y) : base("assets/sprite_3.png")
    {
        SetXY(x, y);
        alpha = 0f;
        isOn = true;
    }
}

