using Microsoft.Xna.Framework;

class Sanic : AnimatedGameObject
{
    protected double spawnTime;
    protected Vector2 startPosition;
    protected double speedMult;

    public Sanic(bool moveToLeft, Vector2 startPosition, double speedMult) : base(4, "enemy")
    {
        if (speedMult == 1) {
            LoadAnimation("Sprites/Rocket/spr_rocket@3", "default", true, 0.2f);
        }
        else {
            LoadAnimation("Sprites/Rocket/spr_rocket_fast@3", "default", true, 0.2f);
        }
        PlayAnimation("default");
        Mirror = moveToLeft;
        this.startPosition = startPosition;
        this.speedMult = speedMult;
        Reset();
    }

    public override void Reset()
    {
        visible = false;
        position = startPosition;
        velocity = Vector2.Zero;
        spawnTime = GameEnvironment.Random.NextDouble() * 5;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (spawnTime > 0)
        {
            spawnTime -= gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }
        visible = true;
        velocity.X = 600 * (float)speedMult;
        if (Mirror)
        {
            this.velocity.X *= -1;
        }
        CheckCollision();
        // check if we are outside the screen
        Rectangle screenBox = new Rectangle(0, 0, Level.levelWidth[Camera.CurrLevel] * Level.tiles.CellWidth, GameEnvironment.Screen.Y);
        if (!screenBox.Intersects(this.BoundingBox))
        {
            Reset();
        }
    }

    public void CheckCollision() {
        Player player = GameWorld.Find("player") as Player;
        if (CollidesWith(player) && visible) {
            if (this.GlobalPosition.Y > (player.GlobalPosition.Y + 20) && player.IsAlive) {
                player.Jump();
                Die();
            }
            else {
                player.Die(false);
            }
        }
        if (Dorito()) {
            Die();
        }
    }
}
