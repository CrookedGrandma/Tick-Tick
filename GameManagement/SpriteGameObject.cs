﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteGameObject : GameObject
{
    protected SpriteSheet sprite;
    protected Vector2 origin;
    public bool PerPixelCollisionDetection = true;
    protected int patrolShotCount = 1;
    protected int sparkyShotCount = 1;

    public SpriteGameObject(string assetName, int layer = 0, string id = "", int sheetIndex = 0)
        : base(layer, id)
    {
        if (assetName != "")
        {
            sprite = new SpriteSheet(assetName, sheetIndex);
        }
        else
        {
            sprite = null;
        }
    }    

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!visible || sprite == null)
        {
            return;
        }
        if (layer < 5) {
            sprite.Draw(spriteBatch, this.GlobalPosition - Camera.CamPos * layer / 4, origin);
        }
        else {
            sprite.Draw(spriteBatch, this.GlobalPosition, origin);
        }
    }

    public SpriteSheet Sprite
    {
        get { return sprite; }
    }

    public Vector2 Center
    {
        get { return new Vector2(Width, Height) / 2; }
    }

    public int Width
    {
        get
        {
            return sprite.Width;
        }
    }

    public int Height
    {
        get
        {
            return sprite.Height;
        }
    }

    public bool Mirror
    {
        get { return sprite.Mirror; }
        set { sprite.Mirror = value; }
    }

    public Vector2 Origin
    {
        get { return origin; }
        set { origin = value; }
    }

    public override Rectangle BoundingBox
    {
        get
        {
            int left = (int)(GlobalPosition.X - origin.X);
            int top = (int)(GlobalPosition.Y - origin.Y);
            return new Rectangle(left, top, Width, Height);
        }
    }

    public bool CollidesWith(SpriteGameObject obj)
    {
        if (!visible || !obj.visible || !BoundingBox.Intersects(obj.BoundingBox))
        {
            return false;
        }
        if (!PerPixelCollisionDetection)
        {
            return true;
        }
        Rectangle b = Collision.Intersection(BoundingBox, obj.BoundingBox);
        for (int x = 0; x < b.Width; x++)
        {
            for (int y = 0; y < b.Height; y++)
            {
                int thisx = b.X - (int)(GlobalPosition.X - origin.X) + x;
                int thisy = b.Y - (int)(GlobalPosition.Y - origin.Y) + y;
                int objx = b.X - (int)(obj.GlobalPosition.X - obj.origin.X) + x;
                int objy = b.Y - (int)(obj.GlobalPosition.Y - obj.origin.Y) + y;
                if (sprite.IsTranslucent(thisx, thisy) && obj.sprite.IsTranslucent(objx, objy))
                {
                    return true;
                }
            }
        }
        return false;
    }
    protected bool Dorito() {
        Weapon dorito = GameWorld.Find("weapon") as Weapon;
        if (dorito != null) {
            if (CollidesWith(dorito) && visible) {
                dorito.Reset();
                return true;
            }
        }
        return false;
    }

    protected void Die() {
        if (this is PatrollingEnemy) {
            PatrollingEnemy current = this as PatrollingEnemy;
            if (patrolShotCount == 5) {
                velocity.Y = 1000f;
                current.ResetToo();
            }
            else {
                patrolShotCount++;
            }
        }
        else if (this is Sparky) {
            Sparky current = this as Sparky;
            if (sparkyShotCount == 10) {
                velocity.Y = 1000f;
                current.ResetToo();
            }
            else {
                sparkyShotCount++;
            }
        }
        else {
            velocity.Y = 1000f;
        }
    }
}

