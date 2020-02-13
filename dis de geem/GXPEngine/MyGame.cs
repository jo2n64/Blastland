using System;                                   // System contains a lot of default C# libraries 
using System.Collections.Generic;
using GXPEngine;								// GXPEngine contains the engine

public class MyGame : Game
{
	Player player;
	private Level[] levels;
	private List<Bullet> bullets;
	//private GameManager manager;
	private int level;

	public int Level { get => level; set => level = value; }

	public MyGame() : base(1920, 1080, false)		// Create a window that's 800x600 and NOT fullscreen
	{
		targetFps = 60;
		level = 0;
		player = new Player(5f, 3f);
		levels = new Level[3];
		levels[0] = new Level(player, "assets/samplelevel.tmx", this);
		levels[1] = new Level(player, "assets/level2.tmx", this);
		levels[2] = new Level(player, "assets/level3.tmx", this);
		bullets = new List<Bullet>();
		//manager = new GameManager();
		//manager.SetState(GameManager.GameState.Level2);
		//AddChild(manager);
		AddChild(levels[0]);
		AddChild(player);
	}

	void Update()
	{
		// Empty
		if (level < 3)
		{
			shoot(player);
			destroyBullets(bullets);
		}
	}

	static void Main()							// Main() is the first method that's called when the program is run
	{
		new MyGame().Start();					// Create a "MyGame" and start it
	}

	private void shoot(Player player) {
		if (Input.GetKey(Key.SPACE))
		{
			if (Time.time > player.ShootDelay + player.ShootTimer)
			{
				Bullet b = new Bullet(player.x, player.y, 0, -25, player.Sprite.rotation);
				bullets.Add(b);
				AddChild(b);
				player.ShootTimer = Time.time;
				Console.WriteLine(level);
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

	private void destroyBullets(List<Bullet> buls) {
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
					if (temp is Boss)
					{
						switchLevel(level + 1);
						level++;
					}
					b.LateDestroy();
					buls.Remove(b);
					levels[0].Enemies.Remove(temp);
					temp.LateDestroy();
					temp.isDead = true;
					Console.WriteLine(levels[level].Enemies.Count);
					
				}
			}
		}
	}
}