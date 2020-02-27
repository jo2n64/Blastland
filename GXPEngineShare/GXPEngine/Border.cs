using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Border : Sprite
{
    public Border(int x, int y) : base("assets/sprite_2.png")
    {
        SetXY(x, y);
        alpha = 0f;
    }
}

