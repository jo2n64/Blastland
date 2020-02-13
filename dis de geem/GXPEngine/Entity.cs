using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
abstract class Entity : AnimationSprite
{
    protected int time, delay;
    public Entity(string path, int cols, int rows, int delay) : base(path, cols, rows)
    {
        time = Time.time;
        this.delay = delay;
    }

    private void Update() {
        handleAnimation();
    }

    protected abstract void handleAnimation();
}
