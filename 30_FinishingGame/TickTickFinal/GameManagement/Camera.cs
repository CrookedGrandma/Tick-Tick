using Microsoft.Xna.Framework;
class Camera {
    static Vector2 CameraPos = Vector2.Zero;
    public static int CamPosX {
        get { return (int)CameraPos.X; }
        set { CameraPos.X = value; }
    }
    public static Vector2 CamPos {
        get { return CameraPos; }
        set { CameraPos = value; }
    }
}