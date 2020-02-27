using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Flamethrower : Weapon
{
    Player player;
    private bool isUsed;
    private int damage;
    private int delay, timer;

    public int Damage { get => damage; set => damage = value; }

    public Flamethrower(float x, float y, Player player) : base(x, y, "flames.png")
    {
        this.player = player;
        SetOrigin(width / 2, height);
        alpha = 0f;
        isUsed = false;
        damage = 3;
        timer = Time.time;
        delay = 50;
    }

    private void Update()
    {
        base.Update();
        if (player.Fuel <= 0 || !isUsed) alpha = 0f;
        handleInput();
    }

    private void handleInput()
    {
        if (isSelected && Input.GetKey(Key.TWO) && player.Fuel > 0)
        {
            if (Time.time >= timer + delay)
            {
                alpha = 1f;
                isUsed = true;
                player.Fuel--;
                timer = Time.time;
            }
        }
        if (Input.GetKeyUp(Key.TWO) && isUsed) { isUsed = false; }
        //if (Input.GetKeyUp(Key.SPACE))
        //{
        //    alpha = 0f;
        //}

    }
}

