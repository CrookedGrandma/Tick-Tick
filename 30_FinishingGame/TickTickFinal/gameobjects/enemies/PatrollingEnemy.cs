﻿using Microsoft.Xna.Framework;
using System;

class PatrollingEnemy : AnimatedGameObject
{
    protected float waitTime;
    protected Vector2 startPosition;

    public PatrollingEnemy() : base(4, "enemy") {
        waitTime = 0.0f;
        velocity.X = 120;
        LoadAnimation("Sprites/Flame/spr_flame@9", "default", true);
        PlayAnimation("default");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (waitTime > 0)
        {
            waitTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (waitTime <= 0.0f)
            {
                TurnAround();
            }
        }
        else
        {
            TileField tiles = GameWorld.Find("tiles") as TileField;
            float posX = BoundingBox.Left;
            if (!Mirror)
            {
                posX = BoundingBox.Right;
            }
            int tileX = (int)Math.Floor(posX / tiles.CellWidth);
            int tileY = (int)Math.Floor(position.Y / tiles.CellHeight);
            if (tiles.GetTileType(tileX, tileY - 1) == TileType.Normal ||
                tiles.GetTileType(tileX, tileY) == TileType.Background)
            {
                waitTime = 0.5f;
                velocity.X = 0.0f;
            }
        }
        CheckCollision();
    }

    public Vector2 StartPosition {
        set {
            startPosition = value;
            position = value;
        }
    }

    public override void Reset() {
        base.Reset();
        position = startPosition;
        patrolShotCount = 1;
    }

    public void ResetToo() {
        position = startPosition;
        velocity.Y = 0f;
        visible = false;
    }

    public void CheckCollision()
    {
        Player player = GameWorld.Find("player") as Player;
        if (CollidesWith(player))
        {
            player.Die(false);
        }
        if (Dorito()) {
            Die();
        }
    }

    public void TurnAround()
    {
        Mirror = !Mirror;
        velocity.X = 120;
        if (Mirror)
        {
            velocity.X *= -1;
        }
    }
}
