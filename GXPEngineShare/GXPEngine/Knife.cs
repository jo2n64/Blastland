using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Knife : Weapon
{
    public Knife(float x, float y) : base(x, y, "knife.png")
    {
        SetOrigin(width / 2, height);
    }
}
