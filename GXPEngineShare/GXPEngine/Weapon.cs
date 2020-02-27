using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Weapon : GameObject
{
    public bool isSelected;
    public Weapon(float x, float y) : base()
    {
        SetXY(x, y);
    }

    protected void Update() {
    }
}

