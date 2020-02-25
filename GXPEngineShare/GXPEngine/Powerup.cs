using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Powerup : Sprite
{
    protected Sprite sprite;
    public Powerup(float x, float y, string spritePath) : base(spritePath)
    {
        SetXY(x, y);
        SetScaleXY(0.3f, 0.3f);
    }

    private void Update() { }


}
