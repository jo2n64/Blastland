using System;                                   // System contains a lot of default C# libraries 
using System.Collections.Generic;
using System.Drawing;
using GXPEngine;								// GXPEngine contains the engine

public class MyGame : Game
{
    public enum GameState
    {
        Startup,
        Tutorial,
        Game,
        GameOver,
        End
    }
    Player player;
    Canvas screens;
    private Sound shootSound, flamethrowerSound;
    private Sound music;
    private GameState state;
    private Image img, tutorialImg, endImg;
    private HUD hud;
    private Level[] levels;
    private List<Bullet> bullets;
    private Healthbar hpBar;
    private float shake;
    private Statsbar moveBar, damageBar, fireRateBar;
    //private GameManager manager;
    private int shakeTimer, shakeDelay;
    private int level;

    public int Level { get => level; set => level = value; }

    public MyGame() : base(1920, 1080, false, false)       // Create a window that's NOT 800x600 and fullscreen
    {
        screens = new Canvas(width, height);
        shootSound = new Sound("sounds/Gun_shot_366402__rach-capache__blowing-up-balloon-and-popping.wav");
        flamethrowerSound = new Sound("sounds/FlameTrowerLong460570__15f-panska-paril-silvestr__flametrower.wav", true, false);
        //music = new Sound("sounds/Mick Gordon - 02. Rip & Tear.mp3", true, true);
        img = Image.FromFile("startupscreen.png");
        tutorialImg = Image.FromFile("Instruction_screen.png");
        endImg = Image.FromFile("endgamescreen.png");
        targetFps = 60;
        level = 2;
        state = GameState.Startup;
        levels = new Level[3];
        player = new Player(15f, 15f);
        levels[0] = new Level(player, "assets/samplelevel.tmx", this);
        levels[1] = new Level(player, "assets/level2.tmx", this);
        levels[2] = new Level(player, "assets/level3.tmx", this);
        bullets = new List<Bullet>();
        screens = new Canvas(1920, 1080);
        img = Image.FromFile("startupscreen.png");
        hud = new HUD(player);
        hpBar = new Healthbar(width / 2 - 400, 20, levels[level].B);
        moveBar = new Statsbar(width - 450, 100, player, "moveSpeed.png", "health.png", (int)player.SpeedX);
        damageBar = new Statsbar(width - 450, 200, player, "moveSpeed.png", "health.png", player.Damage);
        fireRateBar = new Statsbar(width - 450, 300, player, "moveSpeed.png", "health.png", player.FireRate);
        //player.SetLevel(levels[0]);
        //manager = new GameManager();
        //manager.SetState(GameManager.GameState.Level2);
        //AddChild(manager);
        shakeTimer = Time.time;
        shakeDelay = 500;
        AddChild(screens);

    }

    void Update()
    {
        // Empty
        //Console.WriteLine(currentFps);


        if(levels[level].B.y + levels[level].B.height> 0) AddChild(hpBar);
        switch (state)
        {
            case GameState.Startup:
                screens.graphics.DrawImage(img, 0, 0);
                if (Input.GetKeyDown(Key.SPACE))
                {
                    state = GameState.Tutorial;
                }
                break;
            case GameState.Tutorial:               
                screens.graphics.Clear(Color.Empty);
                screens.graphics.DrawImage(tutorialImg, 0, 0);
                if (Input.GetKeyDown(Key.SPACE))
                {
                    state = GameState.Game;
                    switchLevel(level);
                    AddChild(player);
                    AddChild(hud);
                    AddChild(moveBar);
                    AddChild(damageBar);
                    AddChild(fireRateBar);
                    //music.Play();
                }
                break;
            case GameState.Game:
                screens.graphics.Clear(Color.Empty);
                moveBar.ThingToAmount = (int)player.SpeedX;
                damageBar.ThingToAmount = player.Damage;
                fireRateBar.ThingToAmount = player.FireRate;
                shoot(player);
                checkWeaponCollision(player.Weapons);
                destroyBullets(bullets);
                break;
            case GameState.End:
                screens.graphics.Clear(Color.Empty);
                screens.graphics.DrawImage(endImg, 0, 0);
                if (Input.GetKeyDown(Key.SPACE))
                {
                    level = 0;
                    state = GameState.Game;
                    switchLevel(level);
                    AddChild(player);
                    AddChild(hud);
                    AddChild(moveBar);
                    AddChild(damageBar);
                    AddChild(fireRateBar);
                }
                break;
            case GameState.GameOver:
                break;
        }

        if (Input.GetKeyDown(Key.SPACE) && state == GameState.Tutorial)
        {
            screens.graphics.Clear(Color.Empty);
            shoot(player);
            checkWeaponCollision(player.Weapons);
            destroyBullets(bullets);
        }

    }

    static void Main()                          // Main() is the first method that's called when the program is run
    {
        new MyGame().Start();                   // Create a "MyGame" and start it
    }

    private void shoot(Player player)
    {
        if (Input.GetKey(Key.SPACE) && player.Weapons[0].isSelected)
        {
            if (Time.time > player.ShootDelay + player.ShootTimer)
            {
                Bullet b = new Bullet(player.x, player.y, 0, -25, player.Sprite.rotation);
                shootSound.Play();
                bullets.Add(b);
                AddChild(b);
                player.ShootTimer = Time.time;
                //Console.WriteLine(level);
            }
        }
    }

    private void switchLevel(int level)
    {
        switch (level)
        {
            case 0:
                AddChild(levels[0]);
                break;
            case 1:
                RemoveChild(levels[0]);
                AddChildAt(levels[1], 0);
                break;
            case 2:
                RemoveChild(levels[1]);
                AddChildAt(levels[2], 0);
                break;
        }
    }

    private void destroyBullets(List<Bullet> buls)
    {
        for (int i = 0; i < buls.Count; i++)
        {
            Bullet b = buls[i];
            for (int j = 0; j < levels[level].Enemies.Count; j++)
            {
                Enemy temp = levels[level].Enemies[j];
                if (b.y < 0)
                {
                    b.LateDestroy();
                    buls.Remove(b);
                    break;
                }
                if (b.HitTest(temp))
                {
                    //shakeScreen();
                    b.LateDestroy();
                    buls.Remove(b);
                    temp.Health -= player.Damage;
                    levels[level].shakeScreen();
                    if(temp is Boss)
                    {
                        hpBar.Health.scaleX -= 1.5f * player.Damage;
                    }
                    if (temp.Health <= 0)
                    {
                        levels[level].Enemies.Remove(temp); 
                        player.Score += 5;
                        temp.isDead = true;
                        if (!(temp is ExplosiveEnemy))
                        {
                            temp.LateDestroy();
                        }
                        if (temp is Boss)
                        {
                            if (level < 2)
                            {
                                RemoveChild(hpBar);
                                switchLevel(level + 1);
                                level++;
                                hpBar.setBoss(levels[level].B);
                            }
                            else
                            {
                                RemoveChild(player);
                                RemoveChild(hud);
                                RemoveChild(moveBar);
                                RemoveChild(damageBar);
                                RemoveChild(fireRateBar);
                                state = GameState.End;
                            }
                        }
                    }
                }
            }
        }
    }

    private void shakeScreen() {
        shake = Mathf.Sin(60f);
        x = shake;
        if(Time.time >= shakeTimer + shakeDelay)
        {
            x = 0;
            shakeTimer = Time.time;
        }
    }

    private void checkWeaponCollision(Weapon[] weapons)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            Weapon w = player.Weapons[i];
            for (int j = 0; j < levels[level].Enemies.Count; j++)
            {
                Enemy temp = levels[level].Enemies[j];
                if (w.HitTest(temp))
                {
                }
            }
        }
    }
}

