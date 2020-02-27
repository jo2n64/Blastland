using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Timer : GameObject
{
    private Action action;
    private int time;
    private int millis;
    public Timer(int millis, Action action)
    {
        time = Time.time;
        this.millis = millis;
        this.action = action;
    }

    public void Update() {
        if(Time.time > time + millis)
        {
            action();
            LateDestroy();
        }
    }
}

