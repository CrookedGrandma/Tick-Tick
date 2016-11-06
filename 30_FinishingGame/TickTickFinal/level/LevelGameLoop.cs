using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

partial class Level : GameObjectList
{

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (quitButton.Pressed)
        {
            Reset();
            GameEnvironment.GameStateManager.SwitchTo("levelMenu");
        }
        //CHEAT (TOTALLY ONLY FOR DEBUGGING)
        if (inputHelper.KeyPressed(Keys.F10)) {
            GameObjectList waterdrops = Find("waterdrops") as GameObjectList;
            foreach (GameObject d in waterdrops.Children) {
                d.Visible = false;
            }
            SpriteGameObject exitObj = Find("exit") as SpriteGameObject;
            Player player = Find("player") as Player;
            player.Position = exitObj.Position;
        }
        //END CHEAT
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        TimerGameObject timer = Find("timer") as TimerGameObject;
        Player player = Find("player") as Player;

        // check if we died
        if (!player.IsAlive)
        {
            timer.Running = false;
        }

        // check if we ran out of time
        if (timer.GameOver)
        {
            player.Explode();
        }
                       
        // check if we won
        if (Completed && timer.Running)
        {
            player.LevelFinished();
            timer.Running = false;
        }
    }

    public override void Reset()
    {
        base.Reset();
        VisibilityTimer hintTimer = Find("hintTimer") as VisibilityTimer;
        hintTimer.StartVisible();
    }
}
