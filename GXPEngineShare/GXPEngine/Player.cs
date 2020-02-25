using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
class Player : GameObject
{
    private bool isHit;
    private float speedX, speedY;
    private float startSpeedX, startSpeedY;
    private float rotValue;
    private int health, maxHealth;
    private int slowAmount;
    private int fuel;
    private int weaponNum;
    private int score;
    private int lives;
    private int damage;         
    private int damageReceived;
    private int damagePowerupsCollected;
    private float xModifier, yModifier;
    private int fireRate;
    private int shootDelay, shootTimer;
    private int walkDelay, walkTimer;
    private int animTimer, animDelay;
    private int hitTimer, hitDelay;
    private string weaponName;
    private Sprite sprite;
    private Weapon[] weapons;
    private AnimationSprite animationSprite;
    private Sound hurtSound, pickupSound, walkSound;
    private SoundChannel channel;
    private Vector2 direction = new Vector2(0.0f, 0.0f);

    public int ShootDelay { get => shootDelay; set => shootDelay = value; }
    public int ShootTimer { get => shootTimer; set => shootTimer = value; }
    public float SpeedX { get => speedX; set => speedX = value; }
    public float SpeedY { get => speedY; set => speedY = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public Vector2 Direction { get => direction; set => direction = value; }
    public int Health { get => health; set => health = value; }
    public int Lives { get => lives; set => lives = value; }
    public int Damage { get => damage; set => damage = value; }
    internal Weapon[] Weapons { get => weapons; set => weapons = value; }
    public int Fuel { get => fuel; set => fuel = value; }
    public int Score { get => score; set => score = value; }
    public string WeaponName { get => weaponName; set => weaponName = value; }
    public int FireRate { get => fireRate; set => fireRate = value; }

    //private Level level;
    //public void SetLevel(Level level)
    //{
    //    this.level = level;
    //}
    public Player(float speedX, float speedY) : base()
    {
        isHit = false;
        weaponNum = 0;
        hurtSound = new Sound("sounds/classic_hurt.mp3");
        pickupSound = new Sound("sounds/PickingUpSomething422651__trullilulli__sfx-player-action-phone-pick-up.wav");
        walkSound = new Sound("sounds/FootstepSoundsCS_1.6_Sounds.wav");
        sprite = new Sprite("square.png");
        weapons = new Weapon[3];
        weapons[0] = new Weapon(sprite.x, sprite.y, "triangle.png");
        weapons[1] = new Flamethrower(sprite.x, sprite.y, this);
        weapons[2] = new Knife(sprite.x, sprite.y);
        animationSprite = new AnimationSprite("plantspriteshee2222t.png", 6, 1);
        sprite.SetOrigin(sprite.width / 2.0f, sprite.height / 2.0f);
        animationSprite.SetOrigin(sprite.width / 2.0f, sprite.height);
        shootDelay = 500;
        shootTimer = Time.time;
        animDelay = 100;
        animTimer = Time.time;
        hitTimer = Time.time;
        walkTimer = Time.time;
        hitDelay = 2000;
        walkDelay = 276;
        fireRate = 1;
        startSpeedX = speedX;
        this.speedX = startSpeedX;
        xModifier = 0;
        yModifier = 0;
        startSpeedY = speedY;
        this.speedY = startSpeedY;
        damage = 1;
        damageReceived = 1;
        damagePowerupsCollected = 0;
        health = 3;
        maxHealth = 3;
        slowAmount = 0;
        lives = 3;
        fuel = 0;
        rotValue = 0;
        animationSprite.SetFrame(4);
        AddChild(sprite);
        AddChild(animationSprite);
        for(int i = 0; i < weapons.Length; i++)
        {
            AddChild(weapons[i]);
        }
    }

    private void Update()
    {
        move();
        handleInput();
        checks();
        checkWeaponSwitch();
        if (direction.x != 0 || direction.y != 0)
        {
            if (Time.time >= walkDelay + walkTimer)
            {
                walkSound.Play(false);
                walkTimer = Time.time;
            }
        }
        if (direction.x == 0 || direction.y == 0) walkSound.Play(true);
        if (isHit)
        {
            if (Time.time > hitTimer + hitDelay)
            {
                isHit = false;
                sprite.alpha = 1f;
                hitTimer = Time.time;
            }
        }
        //Console.WriteLine(direction.x);
        //if (Time.time > animDelay + animTimer)
        //{
        //    animationSprite.NextFrame();
        //    animTimer = Time.time;
        //}
    }

    private void invulnerable()
    {
        
    }
    private void setVector(float dirX, float dirY)
    {
        direction.x = dirX;
        direction.y = dirY;
    }

    private void move()
    {
        xModifier = speedX * direction.x * Time.deltaTime / 60f;
        yModifier = speedY * direction.y * Time.deltaTime / 60f;
        Move(xModifier, 0);
        Move(0, yModifier);
    }

    public void reposition(float x, float y)
    {
        SetXY(x, y);
    }

    public void reset() {
        damage = 1;
        damageReceived = 1;
        damagePowerupsCollected = 0;
        health = 3;
        maxHealth = 3;
        slowAmount = 0;
        lives = 3;
        fuel = 0;
        rotValue = 0;
    }

    private void checks()
    {
        foreach (GameObject g in sprite.GetCollisions())
        {
            switch (g)
            {
                case Wall w:
                    if (x - sprite.width / 2 <= w.x + w.width)
                    {
                        x = w.x + w.width + sprite.width / 2;
                    }//idfk kms jesus
                    break;
                case Wall2 w2:
                    if (x + sprite.width/2 >= w2.x)
                    {
                        x = w2.x - sprite.width / 2;
                    }
                    break;
                case HealthPowerup h:                    
                    h.LateDestroy();
                    pickupSound.Play();
                    health++;
                    if (health > maxHealth)
                    {
                        slowAmount++;
                        speedX -= slowAmount;
                        speedY -= slowAmount;
                    }
                    break;
                case FuelPowerup f:
                    f.LateDestroy();
                    pickupSound.Play();
                    fuel += 50;
                    break;
                case DamagePowerup d:
                    d.LateDestroy();
                    pickupSound.Play();
                    damage++;
                    break;
                case FireRatePowerup fr:
                    fr.LateDestroy();
                    fireRate++;
                    pickupSound.Play();
                    shootDelay /= 2;
                    break;
                case Enemy e:
                    if (e.alpha == 1f && !isHit)
                    {
                        isHit = true;
                        sprite.alpha = 0.5f;
                        health -= damageReceived;
                        hurtSound.Play();
                        if (health < maxHealth)
                        {
                            Console.WriteLine("prev speedX " + speedX);
                            slowAmount = 0;
                            speedX = startSpeedX;
                            speedY = startSpeedY;
                            Console.WriteLine("after " + speedX);
                        }
                        if (health <= 0)
                        {
                            lives--;
                            health = 3;
                            reposition(game.width / 2, game.height - 100);
                        }
                    }
                    break;
                case Spikes s:
                    //make the player invulnerable for a bit
                    if (!isHit)
                    {
                        health -= damageReceived;
                        isHit = true;
                        sprite.alpha = 0.5f;
                        hurtSound.Play();
                        if (health < maxHealth)
                        {
                            slowAmount = 0;
                            speedX = startSpeedX;
                            speedY = startSpeedY;
                        }
                        if (health <= 0)
                        {
                            lives--;
                            health = 3;
                            reposition(game.width / 2, game.height - 100);
                        }
                    }
                    break;
                case Missile m:
                    if (!isHit)
                    {
                        health -= damageReceived;
                        isHit = true;
                        sprite.alpha = 0.5f;
                        hurtSound.Play();
                        if (health < maxHealth)
                        {
                            slowAmount = 0;
                            speedX = startSpeedX;
                            speedY = startSpeedY;
                        }
                        if (health <= 0)
                        {
                            lives--;
                            health = 3;
                            reposition(game.width / 2, game.height - 100);
                        }
                    }
                    break;
                case EnemyBullet e:
                    EnemyBullet eb = g as EnemyBullet;
                    //make the player invulnerable for a bit
                    health -= damageReceived;
                    eb.LateDestroy();
                    hurtSound.Play();
                    if (health < maxHealth)
                    {
                        slowAmount = 0;
                        speedX = startSpeedX;
                        speedY = startSpeedY;
                    }
                    if (health <= 0)
                    {
                        lives--;
                        health = 3;
                        reposition(game.width / 2, game.height - 100);
                    }
                    break;
            }

            //if enemy is reappearing spikes, when the enemy's alpha is 1(anim frame for the future) apply collision effect
        }


        if (x < 0) x = 0;
        if (x + sprite.width > game.width) x = game.width - sprite.width;
        if (y > game.height - sprite.width/2)
        {
            y = game.height - sprite.width/2;
            Console.WriteLine("zhigubigule");
        }
    }

    private void checkWeaponSwitch() {
        switch (weaponNum)
        {
            case 0:
                //Console.WriteLine("weapon is a pistol");
                weaponName = "Pistol";
                weapons[0].isSelected = true;
                weapons[2].isSelected = false;
                break;
            case 1:
                //Console.WriteLine("weapon is a flamethrower");
                weaponName = "Flamethrower";
                weapons[1].isSelected = true;
                weapons[0].isSelected = false;
                break;
            case 2:
                //Console.WriteLine("weapon is a knife");
                weaponName = "Knife";
                weapons[2].isSelected = true;
                weapons[1].isSelected = false;
                break;

        }
    }


    private void handleInput()
    {
        if (Input.GetKey(Key.LEFT))
        {
            setVector(-1.0f, 0.0f);
            rotValue = -90.0f;
            // animationSprite.SetFrame(0);
        }

        if (Input.GetKey(Key.RIGHT))
        {
            setVector(1.0f, 0.0f);
            rotValue = 90.0f;
            //animationSprite.SetFrame(9);
        }
        if (Input.GetKey(Key.UP))
        {
            setVector(0.0f, -1.0f);
            rotValue = 0.0f;
            // animationSprite.SetFrame(4);
        }
        if (Input.GetKey(Key.UP) && Input.GetKey(Key.LEFT))
        {
            setVector(-0.7f, -0.7f);
            rotValue = -45.0f;
            // animationSprite.SetFrame(2);
        }
        if (Input.GetKey(Key.UP) && Input.GetKey(Key.RIGHT))
        {
            setVector(0.7f, -0.7f);
            rotValue = 45.0f;
            //animationSprite.SetFrame(6);
        }
        if (Input.GetKey(Key.DOWN))
        {
            setVector(0.0f, 1.0f);
            rotValue = 180f;
        }
        if (Input.GetKey(Key.DOWN) && Input.GetKey(Key.LEFT))
        {
            setVector(-0.7f, 1.0f);
            rotValue = -135f;
        }
        if (Input.GetKey(Key.DOWN) && Input.GetKey(Key.RIGHT))
        {
            setVector(0.7f, 1.0f);
            rotValue = 135f;
        }
        if (Input.GetKeyUp(Key.LEFT) || Input.GetKeyUp(Key.RIGHT) || Input.GetKeyUp(Key.UP) || Input.GetKeyUp(Key.DOWN))
        {
            setVector(0f, 0f);
            //sprite.rotation = 0f;
        }

        if (Input.GetKeyDown(Key.C))
        {
            weaponNum++;
            if (weaponNum >= 3) weaponNum = 0;
        }

        if (Input.GetKeyDown(Key.SPACE) && weaponNum == 2)
        {
            weapons[2].y = weapons[2].y - 5;
        }

        if (Input.GetKeyUp(Key.SPACE) && weaponNum == 2)
        {
            weapons[2].y = sprite.y;
        }

        sprite.rotation = rotValue;
        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i].rotation = rotValue;
        }
    }
}

