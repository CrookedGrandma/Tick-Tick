using Microsoft.Xna.Framework;

class Weapon : AnimatedGameObject {
    protected Vector2 startPosition;

    public Weapon(bool moveToLeft, Vector2 startPosition) : base(4, "weapon") {
        LoadAnimation("Sprites/Weapon/spr_weapon@11", "weapon", true, 0.025f);
        PlayAnimation("weapon");
        Mirror = moveToLeft;
        this.startPosition = startPosition;
        Start();
    }

    public void Start() {
        position = startPosition;
    }

    public override void Reset() {
        GameWorld.RemoveToo(this);
    }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);
        visible = true;
        velocity.X = 800f;
        if (Mirror) {
            this.velocity.X *= -1;
        }
        //CheckCollision();
        // check if we are outside the screen
        Rectangle screenBox = new Rectangle(0, 0, Level.levelWidth[Camera.CurrLevel] * Level.tiles.CellWidth, GameEnvironment.Screen.Y);
        if (!screenBox.Intersects(this.BoundingBox)) {
            Reset();
        }
    }
}