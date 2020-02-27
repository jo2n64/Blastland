using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
class Player : GameObject
{
    private bool isHit;
    private float speed;
    private float startSpeed;
    private float rotValue;
    private int health, maxHealth;
    private int slowAmount;
    private int fuel;
    private int weaponNum;
    private int score;
    private int lives;
    private int damage, maxDamage;         
    private int damageReceived;
    private int damagePowerupsCollected;
    private float xModifier, yModifier;
    private int fireRate, maxFireRate;
    private int shootDelay, shootTimer;
    private int walkDelay, walkTimer;
    private int hitTimer, hitDelay;
    private int frameCount, startFrame;
    private string weaponName;
    private Sprite sprite;
    private Weapon[] weapons;
    private AnimationSprite animationSprite;
    private Sound hurtSound, pickupSound, walkSound;
    private SoundChannel channel;
    private Vector2 direction = new Vector2(0.0f, 0.0f);

    public int ShootDelay { get => shootDelay; set => shootDelay = value; }
    public int ShootTimer { get => shootTimer; set => shootTimer = value; }
    public float Speed { get => speed; set => speed = value; }
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
        startFrame = 0;
        frameCount = 4;
        isHit = false;
        weaponNum = 0;
        maxDamage = 4;
        maxFireRate = 4;
        hurtSound = new Sound("sounds/PlayerGettingHurt_sound416839__alineaudio__grunt1-death-pain.wav");
        pickupSound = new Sound("sounds/PickingUpSomething422651__trullilulli__sfx-player-action-phone-pick-up.wav");
        walkSound = new Sound("sounds/FootstepSoundsCS_1.6_Sounds.wav");
        sprite = new Sprite("square.png");
        weapons = new Weapon[3];
        weapons[0] = new Weapon(sprite.x, sprite.y, "triangle.png");
        weapons[1] = new Flamethrower(sprite.x, sprite.y, this);
        weapons[2] = new Knife(sprite.x, sprite.y);
        animationSprite = new AnimationSprite("player.png", 4, 4);
        sprite.SetOrigin(sprite.width / 2.0f, sprite.height / 2.0f);
        animationSprite.SetScaleXY(0.5f, 0.5f);
        animationSprite.SetOrigin(sprite.width, sprite.height * 2);
        shootDelay = 800;
        shootTimer = Time.time;
        hitTimer = Time.time;
        walkTimer = Time.time;
        hitDelay = 2000;
        walkDelay = 276;
        fireRate = 1;
        startSpeed = speed;
        speed = startSpeed;
        xModifier = 0;
        yModifier = 0;
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
        sprite.alpha = 0f;
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

        //if(Time.time >= animTimer + animDelay)
        //{
        //    animationSprite.NextFrame();
        //    animTimer = Time.time;
        //}
        if (direction.x != 0 || direction.y != 0)
        {
            if (Time.time >= walkDelay + walkTimer)
            {
                walkSound.Play();
                walkTimer = Time.time;
            }
        }
        if (direction.x == 0 || direction.y == 0) walkSound.Play(true);
        if (isHit)
        {
            if (Time.time > hitTimer + hitDelay)
            {
                isHit = false;
                animationSprite.alpha = 1f;
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

    private void setVector(float dirX, float dirY)
    {
        direction.x = dirX;
        direction.y = dirY;
    }

    private void move()
    {
        xModifier = speed * direction.x * Time.deltaTime / 60f;
        yModifier = speed * direction.y * Time.deltaTime / 60f;
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
        fireRate = 1;
        speed = startSpeed;
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
        foreach (GameObject g in animationSprite.GetCollisions())
        {
            switch (g)
            {
                case Wall w:
                    if (x - sprite.width / 2 <= w.x + w.width)
                    {
                        x = w.x + w.width + sprite.width / 2;
                    }
                    break;
                case Wall2 w2:
                    if (x + sprite.width/2 >= w2.x)
                    {
                        x = w2.x - sprite.width / 2;
                    }
                    break;
                case MovementPowerup mP:
                    mP.LateDestroy();
                    if (speed < 20)
                    {
                        speed += 2;
                        fireRate--;
                        shootDelay *= 2;       
                    }
                    if (damage > 1)
                    {
                        damage--;
                    }
                    break;
                case HealthPowerup h:                    
                    h.LateDestroy();
                    pickupSound.Play();
                    if (health < maxHealth) health++;
                    Console.WriteLine(health);
                    break;
                case FuelPowerup f:
                    f.LateDestroy();
                    pickupSound.Play();
                    if (fuel < 75)
                    {
                        fuel += 20;
                    }
                    break;
                case DamagePowerup d:
                    d.LateDestroy();
                    pickupSound.Play();
                    if (damage < maxDamage)
                    {
                        damage++;
                    }
                    if (speed > 10f)
                    {
                        speed -= 2f;
                    }
                    break;
                case FireRatePowerup fr:
                    fr.LateDestroy();
                    if (fireRate < maxFireRate)
                    {
                        fireRate++;
                    }
                    pickupSound.Play();
                    shootDelay /= 2;
                    break;
                case Enemy e:
                    if (!isHit)
                    {
                        isHit = true;
                        animationSprite.alpha = 0.5f;
                        health -= damageReceived;
                        hurtSound.Play();
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
                        animationSprite.alpha = 0.5f;
                        hurtSound.Play();
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
                        animationSprite.alpha = 0.5f;
                        m.LateDestroy();
                        hurtSound.Play();
                        if (health <= 0)
                        {
                            lives--;
                            health = 3;
                            reposition(game.width / 2, game.height - 100);
                        }
                    }
                    break;
                case EnemyBullet e:
                    //make the player invulnerable for a bit
                    health -= damageReceived;
                    e.LateDestroy();
                    isHit = true;
                    sprite.alpha = 0.5f;
                    hurtSound.Play();
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

    private void setRange(int count, int start)
    {
        frameCount = count;
        startFrame = start;
    }

    private void walkXAnim() {
        int frame = Mathf.Floor(x / 15) % frameCount + startFrame;
        animationSprite.SetFrame(frame);
       // if (xModifier == 0) animationSprite.SetFrame(0);
    }

    private void walkYAnim()
    {
        int frame = Mathf.Floor(y / 15) % frameCount + startFrame;
        animationSprite.SetFrame(frame);
        //if (yModifier == 0) animationSprite.SetFrame(0);
    }

    private void checkWeaponSwitch() {
        switch (weaponNum)
        {
            case 0:
                //Console.WriteLine("weapon is a pistol");
                weaponName = "Pistol";
                weapons[0].isSelected = true;
                weapons[1].isSelected = false;
                break;
            case 1:
                //Console.WriteLine("weapon is a flamethrower");
                weaponName = "Flamethrower";
                weapons[1].isSelected = true;
                weapons[0].isSelected = false;
                break;

        }
    }


    private void handleInput()
    {
        
        if (Input.GetKey(Key.A))
        {
            setVector(-1.0f, 0.0f);
            rotValue = -90.0f;
            setRange(4, 8);
            walkXAnim();
        }

        if (Input.GetKey(Key.D))
        {
            setVector(1.0f, 0.0f);
            rotValue = 90.0f;
            setRange(4, 4);
            walkXAnim();
        }
        if (Input.GetKey(Key.W))
        { 
            setVector(0.0f, -1.0f);
            rotValue = 0.0f;
            setRange(4, 0);
            walkYAnim();
            //walkYAnim();
            // animationSprite.SetFrame(4);
        }
        if (Input.GetKey(Key.W) && Input.GetKey(Key.A))
        {
            setVector(-0.7f, -0.7f);
            rotValue = -45.0f;
            setRange(4, 0);
            walkYAnim();
            // animationSprite.SetFrame(2);
        }
        if (Input.GetKey(Key.W) && Input.GetKey(Key.D))
        {
            setVector(0.7f, -0.7f);
            rotValue = 45.0f;
            setRange(4, 0);
            walkYAnim();
            //animationSprite.SetFrame(6);
        }
        if (Input.GetKey(Key.S))
        {
            setVector(0.0f, 1.0f);
            rotValue = 180f;
            setRange(4, 12);
            walkYAnim();
        }
        if (Input.GetKey(Key.S) && Input.GetKey(Key.A))
        {
            setVector(-0.7f, 1.0f);
            rotValue = -135f;
            setRange(4, 12);
            walkYAnim();
        }
        if (Input.GetKey(Key.S) && Input.GetKey(Key.D))
        {
            setVector(0.7f, 1.0f);
            rotValue = 135f;
            setRange(4, 12);
            walkYAnim();
        }
        if (Input.GetKeyUp(Key.A) || Input.GetKeyUp(Key.D) || Input.GetKeyUp(Key.W) || Input.GetKeyUp(Key.S))
        {
            setVector(0f, 0f);

            //sprite.rotation = 0f;
        }

        if (Input.GetKeyDown(Key.ONE))
        {
            weaponNum++;
            if (weaponNum >= 2) weaponNum = 0;
        }
        
        

        sprite.rotation = rotValue;
        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i].rotation = rotValue;
        }
    }
}

