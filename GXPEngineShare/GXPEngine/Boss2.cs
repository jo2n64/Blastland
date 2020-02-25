using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Boss2 : Boss
{
    private float timer, delay, timer2, delay2;
    private Sprite[] shootPositions;
    public Boss2(float x, float y, float scale) : base(x, y, 0, 0, scale, null)
    {
        shootPositions = new Sprite[3];
        for(int i = 0; i < shootPositions.Length; i++)
        {
            shootPositions[i] = new Sprite("collidePoint.png");
            shootPositions[i].SetXY(i * 32, 0);
            AddChild(shootPositions[i]);
        }
        timer = Time.time;
        timer2 = Time.time;
        delay = 2000;
        delay2 = 6000;
    }

    private void Update() {
        if (y + height > 0)
        {
            EnemyBullet b, b1;
            if (Time.time > timer + delay)
            {
                if (y > 0)
                {
                    b = new EnemyBullet(shootPositions[1].x, shootPositions[1].y, 0, 10, 0);
                    AddChild(b);
                    timer = Time.time;
                }
            }
            if (Time.time > timer2 + delay2)
            {
                if (y > 0)
                {
                    b = new EnemyBullet(shootPositions[0].x, shootPositions[0].y, 0, 10, 0);
                    b1 = new EnemyBullet(shootPositions[2].x, shootPositions[2].y, 0, 10, 0);
                    AddChild(b);
                    AddChild(b1);
                    timer2 = Time.time;
                }
            }
        }
    }
}
