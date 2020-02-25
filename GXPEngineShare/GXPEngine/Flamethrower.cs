using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Flamethrower : Weapon
{
    Player player;
    public Flamethrower(float x, float y, Player player) : base(x, y, "flames.png")
    {
        this.player = player;
        SetOrigin(width / 2, height);
        alpha = 0f;
    }

    private void Update()
    {
        base.Update();
        if (player.Fuel <= 0) alpha = 0f;
        handleInput();
    }

    private void handleInput()
    {
        if (isSelected && Input.GetKey(Key.SPACE) && player.Fuel > 0)
        {
            alpha = 1f;
            player.Fuel--;
            Console.WriteLine(player.Fuel);
        }
        //if (Input.GetKeyUp(Key.SPACE))
        //{
        //    alpha = 0f;
        //}

    }
}

