using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
class GameManager : GameObject
{
    public enum GameState
    {
        Start,
        Level1,
        Level2,
        Level3,
        Win,
        GameOver
    }
    private GameState state;
    private Level level;
    private Player player;
    private Game game;
    private string path;

    public GameManager() : base()
    {
        state = GameState.Level1;
        setLevel(state);
    }

    private void Update() {
        setLevel(state);
    }

    public void setLevel(GameState state)
    {

        switch (state)
        {
            case GameState.Level1:
                path = "assets/samplelevel.tmx";
                break;
            case GameState.Level2:
                path = "assets/level2.tmx";
                break;
            case GameState.Level3:
                break;
        }
        if (path != null)
        {
            level = new Level(player, path, game);
            AddChild(level);
        }
    }

    public void SetState(GameState state) { this.state = state; }

    public Level GetLevel() { return level; }
}