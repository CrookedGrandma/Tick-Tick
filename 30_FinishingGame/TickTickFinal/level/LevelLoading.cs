﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{
    public static List<int> levelWidth = new List<int>();
    public static TileField tiles;

    public void LoadTiles(string path, int currLevel)
    {
        for (int i = 0; i < 12; i++) {
            levelWidth.Add(0);
        }
        List<string> textLines = new List<string>();
        StreamReader fileReader = new StreamReader(path);
        string line = fileReader.ReadLine();
        int width = line.Length;
        while (line != null)
        {
            textLines.Add(line);
            line = fileReader.ReadLine();
        }
        tiles = new TileField(textLines.Count - 2, width, 1, "tiles");

        string timerString = textLines[textLines.Count - 2];
        double timerTime = Double.Parse(timerString);
        SpriteGameObject timerBackground = new SpriteGameObject("Sprites/spr_timer", 100, "timerSprite");
        timerBackground.Position = new Vector2(10, 10);
        Add(timerBackground);
        TimerGameObject timer = new TimerGameObject(timerTime, 101, "timer");
        timer.Position = new Vector2(25, 30);
        Add(timer);

        GameObjectList hintField = new GameObjectList(100);
        Add(hintField);
        string hint = textLines[textLines.Count - 1];
        SpriteGameObject hintFrame = new SpriteGameObject("Overlays/spr_frame_hint", 0, "hintFrame");
        hintField.Position = new Vector2((GameEnvironment.Screen.X - hintFrame.Width) / 2, 10);
        hintField.Add(hintFrame);
        TextGameObject hintText = new TextGameObject("Fonts/HintFont", 2);
        hintText.Text = textLines[textLines.Count - 1];
        hintText.Position = new Vector2(120, 25);
        hintText.Color = Color.Black;
        hintField.Add(hintText);
        VisibilityTimer hintTimer = new VisibilityTimer(hintField, 1, "hintTimer");
        Add(hintTimer);

        Add(tiles);
        tiles.CellWidth = 72;
        tiles.CellHeight = 55;
        levelWidth[currLevel] = 0;
        for (int x = 0; x < width; ++x) 
        {
            for (int y = 0; y < textLines.Count - 2; ++y)
            {
                Tile t = LoadTile(textLines[y][x], x, y);
                tiles.Add(t, x, y);
            }
            levelWidth[currLevel]++;
        }
    }

    private Tile LoadTile(char tileType, int x, int y)
    {
        switch (tileType)
        {
            case '.':
                return new Tile();
            case '-':
                return LoadBasicTile("spr_platform", TileType.Platform);
            case '+':
                return LoadBasicTile("spr_platform_hot", TileType.Platform, true, false);
            case '@':
                return LoadBasicTile("spr_platform_ice", TileType.Platform, false, true);
            case 'X':
                return LoadEndTile(x, y);
            case 'W':
                return LoadWaterTile(x, y);
            case '1':
                return LoadStartTile(x, y);
            case '#':
                return LoadBasicTile("spr_wall", TileType.Normal);
            case '^':
                return LoadBasicTile("spr_wall_hot", TileType.Normal, true, false);
            case '*':
                return LoadBasicTile("spr_wall_ice", TileType.Normal, false, true);
            case 'T':
                return LoadTurtleTile(x, y);
            case 'R':
                return LoadRocketTile(x, y, true);
            case 'r':
                return LoadRocketTile(x, y, false);
            case 'S':
                return LoadSparkyTile(x, y);
            case 'A':
            case 'B':
            case 'C':
                return LoadFlameTile(x, y, tileType);
            default:
                return new Tile("");
        }
    }

    private Tile LoadBasicTile(string name, TileType tileType, bool hot = false, bool ice = false)
    {
        Tile t = new Tile("Tiles/" + name, tileType);
        t.Hot = hot;
        t.Ice = ice;
        return t;
    }

    private Tile LoadStartTile(int x, int y)
    {
        TileField tiles = Find("tiles") as TileField;
        Vector2 startPosition = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight);
        Player player = new Player(startPosition);
        Add(player);
        return new Tile("", TileType.Background);
    }

    private Tile LoadFlameTile(int x, int y, char enemyType)
    {
        GameObjectList enemies = Find("enemies") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        PatrollingEnemy enemy = null;
        switch (enemyType)
        {
            case 'A': enemy = new UnpredictableEnemy(); break;
            case 'B': enemy = new PlayerFollowingEnemy(); break;
            case 'C': 
            default:  enemy = new PatrollingEnemy(); break;
        }
        enemy.StartPosition = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight);
        enemies.Add(enemy);
        return new Tile();
    }

    private Tile LoadTurtleTile(int x, int y)
    {
        GameObjectList enemies = Find("enemies") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        Turtle enemy = new Turtle();
        enemy.Position = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight + 25.0f);
        enemies.Add(enemy);
        return new Tile();
    }


    private Tile LoadSparkyTile(int x, int y)
    {
        GameObjectList enemies = Find("enemies") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        Sparky enemy = new Sparky((y + 1) * tiles.CellHeight - 100f);
        enemy.Position = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight - 100f);
        enemies.Add(enemy);
        return new Tile();
    }

    private Tile LoadRocketTile(int x, int y, bool moveToLeft)
    {
        GameObjectList enemies = Find("enemies") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        Vector2 startPosition = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight);
        Rocket enemy = new Rocket(moveToLeft, startPosition);
        enemies.Add(enemy);
        return new Tile();
    }

    private Tile LoadEndTile(int x, int y)
    {
        TileField tiles = Find("tiles") as TileField;
        SpriteGameObject exitObj = new SpriteGameObject("Sprites/spr_goal", 4, "exit");
        exitObj.Position = new Vector2(x * tiles.CellWidth, (y+1) * tiles.CellHeight);
        exitObj.Origin = new Vector2(0, exitObj.Height);
        Add(exitObj);
        return new Tile();
    }

    private Tile LoadWaterTile(int x, int y)
    {
        GameObjectList waterdrops = Find("waterdrops") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        MntDew w = new MntDew();
        w.Origin = w.Center;
        w.Position = new Vector2(x * tiles.CellWidth, y * tiles.CellHeight - 10);
        w.Position += new Vector2(tiles.CellWidth, tiles.CellHeight) / 2;
        waterdrops.Add(w);
        return new Tile();
    }
}