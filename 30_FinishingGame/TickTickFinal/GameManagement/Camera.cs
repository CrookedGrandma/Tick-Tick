using Microsoft.Xna.Framework;
class Camera {
    static Vector2 CameraPos = Vector2.Zero;
    static int levelIndex = 1;
    public static int CamPosX {
        get { return (int)CameraPos.X; }
        set { CameraPos.X = value; }
    }
    public static Vector2 CamPos {
        get { return CameraPos; }
        set { CameraPos = value; }
    }
    public static int CurrLevel {
        get { return levelIndex; }
        set { levelIndex = value + 1; }
    }
}