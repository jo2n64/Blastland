using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
class Level : GameObject
{
    const int levelWidth = 40;
    const int levelHeight = 100;

    private Map map;
    private Game game;
    private int width, height;
    private int shakeTimer, shakeDelay;
    private bool hasReachedEnd;
    private Player player;
    private List<Sprite> sprites;
    private List<Sprite> borderSprites;
    private List<Sprite> walls;
    private List<Powerup> powerups;
    private List<Enemy> enemies;
    private Boss b;

    private int delay, timer;

    private Sprite[,] grid = null;

    internal List<Enemy> Enemies { get => enemies; set => enemies = value; }
    internal Boss B { get => b; set => b = value; }
    public bool HasReachedEnd { get => hasReachedEnd; set => hasReachedEnd = value; }

    public Level(Player player, string mapPath, Game game) : base()
    {
        grid = new Sprite[levelHeight, levelWidth];
        this.game = game;
        this.player = player;
        hasReachedEnd = false;
        delay = 100;
        timer = Time.time;
        map = MapParser.ReadMap(mapPath);
        width = map.Layers[0].Width;
        height = map.Layers[0].Height;
        sprites = new List<Sprite>();
        borderSprites = new List<Sprite>();
        enemies = new List<Enemy>();
        powerups = new List<Powerup>();
        shakeTimer = Time.time;
        shakeDelay = 100;
        renderTiles(map);
        renderObjects(map);
    }

    private void Update()
    {
        if (player.Direction.y <= -0.7f && player.y <= game.height - game.height / 3)
        {
            checkEndOfLevel();
            if (!hasReachedEnd)
            {
                scrollTiles(-20f * player.Direction.y);
                player.y = game.height - game.height / 3;
                destroyTiles();
            }

        }
        if (hasReachedEnd) scrollTiles(0f);
    }

    public void shakeScreen() {
        x = x - 10;
        if (Time.time > timer + delay)
        {
            x = x + 10;
            timer = Time.time;
        }
    }

    private void checkEndOfLevel()
    {
        for (int i = 0; i < borderSprites.Count; i++)
        {
            Sprite spr = borderSprites.ElementAt(i);
            if (spr.y > -spr.height) hasReachedEnd = true;
        }
    }

    //private GameObject getGridItem(int gridX, int gridY)
    //{
    //    if (gridX < 0) return null;
    //    if (gridY < 0) return null;
    //    if (gridX >= levelWidth) return null;
    //    if (gridY >= levelHeight) return null;
    //    return grid[gridY, gridX];
    //}

    //private void setGridItem(int gridX, int gridY, Sprite sprite)
    //{
    //    if (gridX < 0) return;
    //    if (gridY < 0) return;
    //    if (gridX >= levelWidth) return;
    //    if (gridY >= levelHeight) return;
    //    grid[gridY, gridX] = sprite;
    //}

    //public List<GameObject> GetOverLaps(GameObject source)
    //{
    //    List<GameObject> result = new List<GameObject>();
    //    int gridX = Mathf.Floor(source.x / 32);
    //    int gridY = Mathf.Floor(source.y / 32);

    //    GameObject other = getGridItem(gridX, gridY);
    //    if (other != null)
    //    {
    //        result.Add(other);
    //    }

    //    return result;
    //}

    private void destroyTiles()
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            Sprite spr = sprites.ElementAt(i);
            if (spr.y > game.height)
            {
                sprites.Remove(spr);
                RemoveChild(spr);
                //Console.WriteLine("REMOVED");
            }
        }
    }

    private void renderTiles(Map map)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int value = Convert.ToInt16(map.Layers[0].GetTileArray().GetValue(x, y));
                Sprite spr;
                switch (value)
                {
                    case 1:
                        spr = new Wall(x * 128, (y - height) * 128 + game.height);// game.width / 4 + x * 128, (y - height) * 128);
                        AddChild(spr);
                        //setGridItem(x, y, spr);
                        sprites.Add(spr);
                        break;
                    case 2:
                        spr = new Plane(x * 128, (y - height) * 128 + game.height);
                        AddChild(spr);
                        //setGridItem(x, y, spr);
                        sprites.Add(spr);
                        break;
                    case 3:
                        spr = new Border(x * 128, (y - height) * 128 + game.height);
                        AddChild(spr);
                        //setGridItem(x, y, spr);
                        sprites.Add(spr);
                        borderSprites.Add(spr);
                        break;
                    case 4:
                        spr = new Spikes(x * 128, (y - height) * 128 + game.height);
                        AddChild(spr);
                        //setGridItem(x, y, spr);
                        sprites.Add(spr);
                        break;
                    case 367:
                        spr = new Wall2(x * 128, (y - height) * 128 + game.height);
                        AddChild(spr);
                        sprites.Add(spr);
                        break;
                }

            }
        }
    }

    private void scrollTiles(float scrollSpeed)
    {
        float yModifier = scrollSpeed * Time.deltaTime / 60f;
        if (sprites != null)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                Sprite spr = sprites.ElementAt(i);
                spr.Move(0, yModifier);
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy e = enemies.ElementAt(i);
            e.Move(0, yModifier);
        }

        for (int i = 0; i < powerups.Count; i++)
        {
            Powerup p = powerups.ElementAt(i);
            p.Move(0, yModifier);
        }
    }

    private void renderObjects(Map map)
    {
        ObjectGroup objectGroup = map.ObjectGroups[0];
        foreach (TiledObject obj in objectGroup.Objects)
        {
            string name = obj.Name;
            Enemy e;
            Powerup powerup;
            switch (name)
            {
                case "PlayerSpawn":
                    player.reposition(game.width / 2, game.height - 200);
                    player.SpeedX = obj.GetFloatProperty("moveX");
                    player.SpeedY = obj.GetFloatProperty("moveY");
                    //Console.WriteLine(player.y);
                    break;
                case "Enemy":
                    e = new Enemy(obj.X, obj.Y - height * 128 + game.height, obj.GetFloatProperty("moveX"), obj.GetFloatProperty("moveY"), obj.GetFloatProperty("scale"));
                    enemies.Add(e);
                    AddChild(e);
                    break;
                case "BounceEnemy":
                    e = new BounceEnemy(obj.X, obj.Y - height * 128 + game.height, obj.GetFloatProperty("moveX"), obj.GetFloatProperty("moveY"), obj.GetFloatProperty("scale"));
                    enemies.Add(e);
                    AddChild(e);
                    break;
                case "Follower":
                    e = new Follower(obj.X, obj.Y - height * 128 + game.height, obj.GetFloatProperty("moveX"), obj.GetFloatProperty("moveY"), obj.GetFloatProperty("scale"), player);
                    AddChild(e);
                    enemies.Add(e);
                    break;
                case "AppearingSpikes":
                    e = new AppearingSpikes(obj.X, obj.Y - height * 128 + game.height, obj.GetIntProperty("appearDelay"));
                    AddChild(e);
                    enemies.Add(e);
                    break;
                case "Shooter":
                    e = new Shooter(obj.X, obj.Y - height * 128 + game.height, obj.GetFloatProperty("moveX"), obj.GetFloatProperty("moveY"), player, obj.GetIntProperty("shootDelay"));
                    AddChild(e);
                    enemies.Add(e);
                    break;
                case "ExplosiveEnemy":
                    e = new ExplosiveEnemy(obj.X, obj.Y - height * 128 + game.height, obj.GetFloatProperty("moveX"), obj.GetFloatProperty("moveY"));
                    AddChild(e);
                    enemies.Add(e);
                    break;
                case "Boss":
                    b = new Boss(obj.X, obj.Y - height * 128 + game.height, obj.GetFloatProperty("moveX"), obj.GetFloatProperty("moveY"), obj.GetFloatProperty("scale"), player);
                    AddChild(b);
                    enemies.Add(b);
                    break;
                case "Boss2":
                    b = new Boss2(obj.X, obj.Y - height * 128 + game.height, obj.GetFloatProperty("scale"));
                    AddChild(b);
                    enemies.Add(b);
                    break;
                case "Boss3":
                    b = new Boss3(obj.X, obj.Y - height * 128 + game.height, obj.GetFloatProperty("scale"), player, obj.GetIntProperty("shootDelay"));
                    AddChild(b);
                    enemies.Add(b);
                    break;
                case "Heart":
                    powerup = new HealthPowerup(obj.X, obj.Y - height * 128 + game.height);
                    AddChild(powerup);
                    powerups.Add(powerup);
                    break;
                case "Fuel":
                    powerup = new FuelPowerup(obj.X, obj.Y - height * 128 + game.height);
                    AddChild(powerup);
                    powerups.Add(powerup);
                    break;
                case "FireRate":
                    powerup = new FireRatePowerup(obj.X, obj.Y - height * 128 + game.height);
                    AddChild(powerup);
                    powerups.Add(powerup);
                    break;
                case "DamagePowerup":
                    powerup = new DamagePowerup(obj.X, obj.Y - height * 128 + game.height);
                    AddChild(powerup);
                    powerups.Add(powerup);
                    break;

            }
        }
    }
}

