using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
class Level : GameObject
{
    private Map map;
    private Game game;
    private int width, height;
    private bool hasReachedEnd;
    private Player player;
    private List<Sprite> sprites;
    private List<Sprite> borderSprites;
    private List<Enemy> enemies;

    internal List<Enemy> Enemies { get => enemies; set => enemies = value; }

    public Level(Player player, string mapPath, Game game) : base()
    {
        this.game = game;
        this.player = player;
        hasReachedEnd = false;
        map = MapParser.ReadMap(mapPath);
        width = map.Layers[0].Width;
        height = map.Layers[0].Height;
        sprites = new List<Sprite>();
        borderSprites = new List<Sprite>();
        enemies = new List<Enemy>();
        renderTiles(map);
        renderObjects(map);
    }

    private void Update()
    {
        if ((player.Direction.y == -1.0f || player.Direction.y == -0.7f) && player.y <= game.height - 100)
        {
            checkEndOfLevel();
            if (!hasReachedEnd) scrollTiles(50f);
            if (hasReachedEnd) scrollTiles(0f);
            player.y = game.height - 100;
        }
    }

    private void checkEndOfLevel()
    {
        for (int i = 0; i < borderSprites.Count; i++)
        {
            Sprite spr = borderSprites.ElementAt(i);
            if (spr.y > 0) hasReachedEnd = true;
        }
    }

    //private void checkIfBossIsDead() {
    //    if (enemies)) { }
    //}

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
                        spr = new Wall(game.width / 4 + x * 32, (y - height) * 32);
                        AddChild(spr);
                        sprites.Add(spr);
                        break;
                    case 2:
                        spr = new Plane(game.width / 4 + x * 32, (y - height) * 32);
                        AddChild(spr);
                        sprites.Add(spr);
                        break;
                    case 3:
                        spr = new Sprite("assets/sprite_2.png");
                        spr.SetXY(game.width / 4 + x * 32, (y - height) * 32);
                        AddChild(spr);
                        sprites.Add(spr);
                        borderSprites.Add(spr);
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
    }


    private void renderObjects(Map map)
    {
        ObjectGroup objectGroup = map.ObjectGroups[0];
        foreach (TiledObject obj in objectGroup.Objects)
        {
            string name = obj.Name;
            switch (name)
            {
                case "PlayerSpawn":
                    player.reposition(game.width / 2, game.height - 200);
                    player.SpeedX = obj.GetFloatProperty("moveX");
                    Console.WriteLine(player.y);
                    break;
                case "Enemy":
                    Enemy e = new Enemy(game.width / 4 + obj.X, obj.Y - height * 32, obj.GetFloatProperty("moveX"), obj.GetFloatProperty("moveY"), obj.GetFloatProperty("scale"));
                    enemies.Add(e);
                    AddChild(e);
                    break;
                case "Boss":
                    Boss b = new Boss(game.width / 4 + obj.X, obj.Y - height * 32, obj.GetFloatProperty("moveX"), obj.GetFloatProperty("moveY"), obj.GetFloatProperty("scale"));
                    AddChild(b);
                    enemies.Add(b);
                    break;
            }
        }
    }
}

