using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Wall : Sprite
{
    public Wall(int x, int y) : base("assets/sprite_0.png")
    {
        SetXY(x, y);
    }
}

