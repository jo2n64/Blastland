using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Boss2 : Boss
{
    private float timer, delay, timer2, delay2;
    private Sprite[] shootPositions;
    public Boss2(float x, float y, float scale, int health) : base(x, y, scale, null, health)
    {
        shootPositions = new Sprite[4];
        for(int i = 0; i < shootPositions.Length; i++)
        {
            shootPositions[i] = new Sprite("collidePoint.png");
            shootPositions[i].SetXY(Anim.x - 16 + i * 32, 0);
            AddChild(shootPositions[i]);
        }
        timer = Time.time;
        timer2 = Time.time;
        delay = 2000;
        delay2 = 6000;
    }

    private void Update() {
        if (anim.y + anim.height > 0)
        {
            EnemyBullet b, b1;
            if (Time.time > timer + delay)
            {
                if (y > 0)
                {
                    b = new EnemyBullet(shootPositions[1].x, shootPositions[1].y, 0, 10, 0);
                    b1 = new EnemyBullet(shootPositions[2].x, shootPositions[2].y, 0, 10, 0);
                    AddChild(b);
                    AddChild(b1);
                    timer = Time.time;
                }
            }
            if (Time.time > timer2 + delay2)
            {
                if (y > 0)
                {
                    b = new EnemyBullet(shootPositions[0].x, shootPositions[0].y, 0, 10, 0);
                    b1 = new EnemyBullet(shootPositions[3].x, shootPositions[2].y, 0, 10, 0);
                    AddChild(b);
                    AddChild(b1);
                    timer2 = Time.time;
                }
            }
        }
    }
}
