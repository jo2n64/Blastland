using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Plane : Sprite
{
    public Plane(int x, int y) : base("ROAD.png")
    {
        SetXY(x, y);
    }
}

