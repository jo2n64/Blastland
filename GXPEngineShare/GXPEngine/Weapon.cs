using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Weapon : Sprite
{
    public bool isSelected;
    public Weapon(float x, float y, string path) : base(path)
    {
        SetXY(x, y);
    }

    protected void Update() {
        if (!isSelected) alpha = 0f;
        if (isSelected) alpha = 1f;
    }
}

