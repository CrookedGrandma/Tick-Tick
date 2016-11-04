using Microsoft.Xna.Framework;

class Sparky : AnimatedGameObject
{
    protected float idleTime;
    protected float yOffset;
    protected float initialY;


    public Sparky(float initialY) : base(4, "enemy") {
        LoadAnimation("Sprites/Sparky/spr_electrocute@6x5", "electrocute", false);
        LoadAnimation("Sprites/Sparky/spr_idle", "idle", true);
        PlayAnimation("idle");
        this.initialY = initialY;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        idleTime = (float)GameEnvironment.Random.NextDouble() * 5;
        position.Y = initialY;
        yOffset = 120;
        velocity = Vector2.Zero;
        patrolShotCount = 1;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (idleTime <= 0)
        {
            PlayAnimation("electrocute");
            if (velocity.Y != 0)
            {
                // falling down
                yOffset -= velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (yOffset <= 0)
                {
                    velocity.Y = 0;
                }
                else if (yOffset >= 120.0f)
                {
                    Reset();
                }
            }
            else if (Current.AnimationEnded)
            {
                velocity.Y = -60;
            }
        }
        else
        {
            PlayAnimation("idle");
            idleTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (idleTime <= 0.0f)
            {
                velocity.Y = 300;
            }
        }

        CheckCollision();
    }

    public void ResetToo() {
        position.Y = initialY;
        velocity.Y = 0f;
        visible = false;
    }

    public void CheckCollision()
    {
        Player player = GameWorld.Find("player") as Player;
        if (CollidesWith(player) && idleTime <= 0)
        {
            player.Die(false);
        }
        if (Dorito()) {
            Die();
        }
    }
}
