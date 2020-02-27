using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class Flamethrower : Weapon
{
    Player player;
    private AnimationSprite anim;
    private SoundChannel ch;
    private bool isUsed;
    private int damage;
    private int delay, timer;
    private int soundDelay, soundTimer;
    private Sound flamethrowerSound;
    private int animDelay, animTimer;

    public int Damage { get => damage; set => damage = value; }
    public bool IsUsed { get => isUsed; set => isUsed = value; }
    public AnimationSprite Anim { get => anim; set => anim = value; }

    public Flamethrower(float x, float y, Player player) : base(x, y)
    {
        this.player = player;
        isUsed = false;
        damage = 3;
        timer = Time.time;
        soundTimer = Time.time;
        soundDelay = 1254;
        delay = 50;
        animTimer = Time.time;
        animDelay = 160;
        anim = new AnimationSprite("flames.png", 6, 1);
        flamethrowerSound = new Sound("sounds/FlameTrowerLongWithFadeOut.wav");
        anim.SetOrigin(anim.width / 2, anim.height + 32);
        AddChildAt(anim, 0);
    }

    private void Update()
    {
        base.Update();
        if (player.Fuel <= 0 || !isUsed)
        {
            anim.alpha = 0f;
            ch = flamethrowerSound.Play(true, 0, 0);
        }
        if (player.Fuel > 0 && isUsed)
        {
            if (Time.time >= soundDelay + soundTimer)
            {
                flamethrowerSound.Play();
                soundTimer = Time.time;
            }
        }
        handleInput();
    }

    private void handleInput()
    {
        if (isSelected && Input.GetKey(Key.TWO) && player.Fuel > 0)
        {
            if (Time.time >= timer + delay)
            {
                anim.alpha = 1f;
                isUsed = true;
                player.Fuel--;
                timer = Time.time;
            }

            if (Time.time >= animTimer + animDelay)
            {
                anim.NextFrame();
                animTimer = Time.time;
            }
        }
        if (Input.GetKeyUp(Key.TWO) && isUsed) {
            isUsed = false;
            ch.Stop();
        }
        //if (Input.GetKeyUp(Key.SPACE))
        //{
        //    alpha = 0f;
        //}

    }
}

