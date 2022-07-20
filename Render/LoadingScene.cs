using Microsoft.Xna.Framework;
using Whiteboard.Misc;
using Whiteboard.Whiteboard;

namespace Whiteboard.Render; 

public class LoadingScene : IScene {
    public void LoadScene() {
        Main.UpdateEvent += Update;
        Main.DrawEvent += Draw;
    }

    private void Update() {
        Assets.Load();
        SceneLoader.LoadScene(new WhiteboardScene());
    }

    private void Draw() {
        ARender.DrawString("Loading...", Align.Centre, Point.Zero, 1);
    }
}