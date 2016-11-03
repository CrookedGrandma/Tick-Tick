using Microsoft.Xna.Framework;

class Weapon : AnimatedGameObject {
    protected double spawnTime;
    protected Vector2 idlePosition;
    protected Vector2 startPosition;
    protected float horSpeed;

    public Weapon(bool moveToLeft, Vector2 startPosition) {
        LoadAnimation("Sprites/Weapon/spr_weapon@11", "weapon", true);
        PlayAnimation("weapon");
        Mirror = moveToLeft;
        idlePosition = new Vector2(-200, -200);
        this.startPosition = startPosition;
        Start();
    }

    public void Start() {
        position = startPosition;
        horSpeed = 400f;
    }

    public override void Reset() {
        visible = false;
        position = idlePosition;
        horSpeed = 0f;
        velocity = Vector2.Zero;
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        visible = true;
        velocity.X = horSpeed;
        if (Mirror) {
            this.velocity.X *= -1;
        }
        CheckCollision();
        // check if we are outside the screen
        Rectangle screenBox = new Rectangle(0, 0, GameEnvironment.Screen.X, GameEnvironment.Screen.Y);
        if (!screenBox.Intersects(this.BoundingBox)) {
            Reset();
        }
    }

    public void CheckCollision() {
        Player player = GameWorld.Find("player") as Player;
        if (CollidesWith(player) && visible) {
            if (this.GlobalPosition.Y > (player.GlobalPosition.Y + 20) && player.IsAlive) {
                player.Jump();
                velocity.Y = 1000f;
            }
            else {
                player.Die(false);
            }
        }
    }
}