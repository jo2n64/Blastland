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
    Font font;
    private Sound shootSound, flamethrowerSound, enemyDeathSound;
    private Sound music, gameOver;
    private GameState state;
    private Image img, tutorialImg, buttonImg, endImg, gameOverImg;
    public Level[] levels;
    private List<Bullet> bullets;
    private Statsbar[] bossBars;
    private float shake;
    private bool isShaking, playerIsDead, bossDamaged, isFlashing;
    private Statsbar moveBar, damageBar, fireRateBar, healthBar, fuelBar;
    private Sprite pistol, flamethrower;
    private Sprite avatar;
    //private GameManager manager;
    private int shakeTimer, shakeDelay;
    private int flamethrowerTimer, flamethrowerDelay;
    private int blinkTimer, blinkDelay;
    private int level;

    public int Level { get => level; set => level = value; }

    public MyGame() : base(1920, 1080, false, false)       // Create a window that's NOT 800x600 and fullscreen
    {
        screens = new Canvas(width, height);
        shootSound = new Sound("sounds/Gun_shot_366402__rach-capache__blowing-up-balloon-and-popping.wav");
        font = new Font("Robot_Font", 70f);
        music = new Sound("sounds/Mick Gordon - 02. Rip & Tear.mp3", true, true);
        gameOver = new Sound("sounds/Sega Rally - 15 Game Over Yeah!.mp3", false, true);
        enemyDeathSound = new Sound("sounds/NormalEnemyDeath.wav");
        tutorialImg = Image.FromFile("Instruction_screen.png");
        endImg = Image.FromFile("endgamescreen.png");
        gameOverImg = Image.FromFile("gameOverImg.png");
        buttonImg = Image.FromFile("startc.png");
        avatar = new Sprite("avatar.png");
        avatar.SetXY(40, height - 200);
        targetFps = 60;
        level = 2;
        blinkDelay = 200;
        bossDamaged = false;
        isShaking = false;
        state = GameState.Startup;
        levels = new Level[3];
        player = new Player(15f, 15f);
        loadLevels();
        bullets = new List<Bullet>();
        screens = new Canvas(1920, 1080);
        img = Image.FromFile("startupscreen.png");
        bossBars = new Statsbar[3];
        flamethrowerTimer = Time.time;
        flamethrowerDelay = 300;
        moveBar = new Statsbar(width - 300, 100, player, "statbarmovement.png", "health.png", player.Speed);
        damageBar = new Statsbar(width - 300, 200, player, "statbar.png", "health.png", player.Damage);
        fireRateBar = new Statsbar(width - 300, 300, player, "statbarfirerate.png", "health.png", player.FireRate);
        healthBar = new Statsbar(width - 425, height - 250, player, "healthbarplayer.png", "playerhealth.png", player.Health);
        fuelBar = new Statsbar(width - 425, height - 150, player, "fuelbar.png", "playerfuel.png", player.Fuel);
        flamethrower = new Sprite("gunhudflamethrower.png");
        pistol = new Sprite("gunhudrifle.png");
        pistol.SetXY(width - 300, 400);
        flamethrower.SetXY(width - 300, 400);
        img = Image.FromFile("startupscreen.png");
        //player.SetLevel(levels[0]);
        //manager = new GameManager();
        //manager.SetState(GameManager.GameState.Level2);
        //AddChild(manager);
        shakeTimer = Time.time;
        shakeDelay = 200;
        AddChild(screens);

    }

    private SoundChannel sc;

    void Update()
    {
        // Empty
        //Console.WriteLine(currentFps);
        if (player.Lives <= 0 && !playerIsDead)
        {
            state = GameState.GameOver;
            sc = music.Play(true);
            gameOver.Play();
            playerIsDead = true;
            Console.WriteLine("YOU DIED");
            clearScreen();
        }
        
        switch (state)
        {
            case GameState.Startup:
                screens.graphics.DrawImage(img, 0, 0);
                if (isFlashing)
                {
                    screens.graphics.DrawImage(buttonImg, 0, 0);
                }
                if (Time.time >= blinkDelay + blinkTimer)
                {
                    isFlashing = !isFlashing;
                    blinkTimer = Time.time;
                }
                if (Input.GetKeyDown(Key.ONE) || Input.GetKeyDown(Key.TWO))
                {
                    state = GameState.Tutorial;
                }
                break;
            case GameState.Tutorial:
                screens.graphics.Clear(Color.Empty);
                screens.graphics.DrawImage(tutorialImg, 0, 0);
                
                if (Input.GetKeyDown(Key.ONE) || Input.GetKeyDown(Key.TWO))
                {
                    reset();
                }
                break;
            case GameState.Game:
                screens.graphics.Clear(Color.Empty);
                screens.graphics.DrawString("X", font, Brushes.HotPink, 125, height - 225);
                screens.graphics.DrawString(player.Lives.ToString(),font, Brushes.Black, 225, height - 225);
                screens.graphics.DrawString("Score: " + player.Score.ToString(), font, Brushes.Black, 20, height / 2);
                moveBar.ThingToAmount = -player.Speed / 5;
                damageBar.ThingToAmount = -player.Damage;
                fireRateBar.ThingToAmount = -player.FireRate;
                healthBar.ThingToAmount = -player.Health * 3f;
                fuelBar.ThingToAmount = -player.Fuel;
                bossBars[level].ThingToAmount = levels[level].B.Health;
                checkBossBars();
                checkSelectedWeapon();
                if (player.Weapons[0].isSelected)
                {
                    flamethrower.alpha = 0f;
                    pistol.alpha = 1f;
                }
                if (player.Weapons[1].isSelected)
                {
                    flamethrower.alpha = 1f;
                    pistol.alpha = 0f;
                }
                //Console.WriteLine(levels[level].B.Health);
                Console.WriteLine(player.FireRate);
                shoot(player);
                checkFlamethrowerCollision();
                destroyBullets(bullets);
                bossChecks(bullets);
                break;
            case GameState.End:
                screens.graphics.Clear(Color.Empty);
                screens.graphics.DrawImage(endImg, 0, 0);
                screens.graphics.DrawString("Your score: " + player.Score, SystemFonts.DefaultFont, Brushes.Black, width/2, height/2);
                if (Input.GetKeyDown(Key.ONE) || Input.GetKeyDown(Key.TWO))
                {
                    gameOver.Play(true);
                    //music.Play();
                    reset();
                }
                break;
            case GameState.GameOver:
                screens.graphics.Clear(Color.Empty);
                screens.graphics.DrawImage(gameOverImg, 0, 0);
                if (Input.GetKeyDown(Key.ONE) || Input.GetKeyDown(Key.TWO))
                {
                    gameOver.Play(true, 0, 0);
                    state = GameState.Tutorial;
                }
                break;
        }
        shakeScreen();

        if(sc != null && !sc.IsPlaying)
        {
            Console.WriteLine(" o ");
            sc = music.Play(false, 0, 1, 0);
        }
        //Console.WriteLine("{0} {1}", sc.IsPlaying, sc.IsPaused);
        //sc = music.Play();
    }

    private void checkSelectedWeapon() {
        if (player.Weapons[0].isSelected)
        {
            flamethrower.alpha = 0f;
            pistol.alpha = 1f;
        }
        if (player.Weapons[1].isSelected)
        {
            flamethrower.alpha = 1f;
            pistol.alpha = 0f;
        }
    }
    private void checkBossBars() {
        if (levels[level].B.y + levels[level].B.height > 0)
        {
            AddChild(bossBars[level]);
        }
        if (levels[level].B.y + levels[level].B.height < 0 && bossBars[level].InHierarchy())
        {
            RemoveChild(bossBars[level]);
        }
    }
    private void shakeScreen() {
        if (isShaking)
        {
            shake = Mathf.Sin(Time.time / 10f);
            x = shake;
            y = shake;
        }
        if (Time.time >= shakeTimer + shakeDelay)
        {
            x = 0;
            y = 0;
            isShaking = false;
            shakeTimer = Time.time;
        }
    }

    static void Main()                          // Main() is the first method that's called when the program is run
    {
        new MyGame().Start();                   // Create a "MyGame" and start it
    }

    private void shoot(Player player)
    {
        if (Input.GetKey(Key.TWO) && player.Weapons[0].isSelected)
        {
            if (Time.time > player.ShootDelay + player.ShootTimer)
            {
                Bullet b = new Bullet(player.x, player.y, 0, -25, player.Sprite.rotation);
                shootSound.Play(false);
                bullets.Add(b);
                AddChildAt(b, 1);
                player.ShootTimer = Time.time;
                //Console.WriteLine(level);
            }
        }
    }

    private void reset()
    {
        state = GameState.Game;
        player.Lives = 3;
        player.Health = 3;
        player.Score = 0;
        player.Damage = 1;
        player.FireRate = 1;
        player.ShootDelay = 800;
        playerIsDead = false;
        player = new Player(15f, 15f);
        player.reposition(width / 2, height - 100);
        level = 0;
        loadLevels();
        loadBars();
        switchLevel(level);      
        AddChild(moveBar);
        AddChild(damageBar);
        AddChild(fireRateBar);
        AddChild(healthBar);
        AddChild(fuelBar);
        AddChild(pistol);
        AddChild(flamethrower);
        AddChild(avatar);
        AddChild(player);
       // sc = music.Play(false);
    }

    private void loadLevels()
    {
        levels[0] = new Level(player, "assets/samplelevel.tmx", this);
        levels[1] = new Level(player, "assets/level2.tmx", this);
        levels[2] = new Level(player, "assets/level3.tmx", this);
    }

    private void removeLevels()
    {
        for(int i = 0; i < levels.Length; i++)
        {
            RemoveChild(levels[i]);
        }
    }
    private void loadBars() {
        bossBars[0] = new Statsbar(width / 2 - 300, 20, player, "healthbar.png", "bosshealth.png", levels[0].B.Health);
        bossBars[0].Amount.SetXY(70, 40);
        bossBars[1] = new Statsbar(width / 2 - 300, 20, player, "healthbar.png", "bosshealth2.png", levels[1].B.Health);
        bossBars[1].Amount.SetXY(70, 40);
        bossBars[2] = new Statsbar(width / 2 - 300, 20, player, "healthbar.png", "bosshealth3.png", levels[2].B.Health);
        bossBars[2].Amount.SetXY(70, 40);
    }
    private void switchLevel(int level)
    {
        switch (level)
        {
            case 0:

                AddChildAt(levels[0], 0);
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

    private void bossChecks(List<Bullet> buls)
    {
        for (int i = 0; i < buls.Count; i++)
        {
            Bullet b = buls[i];
            Boss boss = levels[level].B;
            if (b.HitTest(boss.Anim))
            {
                b.LateDestroy();
                buls.Remove(b);
                boss.Health -= player.Damage;
                Console.WriteLine("Boss health is " + boss.Health);
                if (boss.Health <= 0)
                {
                    if (level < 2)
                    {
                        RemoveChild(bossBars[level]);
                        boss.LateDestroy();
                        switchLevel(level + 1);
                        level++;
                    }
                    else
                    {
                        clearScreen();
                        boss.LateDestroy();
                        state = GameState.End;
                        level = 0;
                    }
                }

            }
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
                if (b.HitTest(temp.Anim))
                {
                    b.LateDestroy();
                    buls.Remove(b);
                    shakeScreen();
                    temp.Health -= player.Damage;
                    temp.isHit = true;
                    Console.WriteLine(isShaking);
                    if (temp.Health <= 0)
                    {
                        levels[level].Enemies.Remove(temp);
                        player.Score += 5;
                        temp.isDead = true;
                        enemyDeathSound.Play();
                        if (!(temp is ExplosiveEnemy))
                        {
                            temp.LateDestroy();
                            isShaking = true;
                        }
                    }
                }
            }
        }
    }

    private void clearScreen()
    {
        for (int i = 0; i < bossBars.Length; i++)
        {
            if (bossBars[i].InHierarchy()) RemoveChild(bossBars[i]);
        }
        removeLevels();
        RemoveChild(player);
        RemoveChild(moveBar);
        RemoveChild(damageBar);
        RemoveChild(fireRateBar);
        RemoveChild(healthBar);
        RemoveChild(fuelBar);
        RemoveChild(pistol);
        RemoveChild(flamethrower);
        RemoveChild(avatar);
    }


    private void checkFlamethrowerCollision()
    {
        Flamethrower flamethrower = player.Weapons[1] as Flamethrower;
        for (int i = 0; i < levels[level].Enemies.Count; i++)
        {
            Enemy temp = levels[level].Enemies[i];
            if (flamethrower.Anim.HitTest(temp.Anim) && flamethrower.isSelected && flamethrower.IsUsed)
            {
                temp.Health -= flamethrower.Damage;
                temp.isHit = true;
                if (temp.Health <= 0) temp.LateDestroy();
            }
            Boss b = levels[level].B;
            if (flamethrower.Anim.HitTest(b.Anim) && flamethrower.isSelected && flamethrower.IsUsed && !bossDamaged)
            {
                
                if (Time.time >= flamethrowerDelay + flamethrowerTimer)
                {
                    Console.WriteLine("yey");
                    b.Health -= 5;
                    flamethrowerTimer = Time.time;
                }
                if (b.Health <= 0)
                {
                    if (level < 2)
                    {
                        RemoveChild(bossBars[level]);
                        b.LateDestroy();
                        switchLevel(level + 1);
                        level++;
                    }
                    else
                    {
                        clearScreen();
                        b.LateDestroy();
                        state = GameState.End;
                        level = 0;
                    }
                }
            }
        }
    }
}

