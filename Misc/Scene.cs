using Microsoft.Xna.Framework;
using Whiteboard.Render;

namespace Whiteboard.Misc;

public static class SceneLoader {
    public static IScene currentScene;

    public static void LoadScene(IScene scene) {
        // Unload scene and remove subscribers from events (leaving UpdateEvent)
        currentScene?.UnloadScene();
        Main.DrawEvent = null;
        Main.UpdateEvent = null;
        Main.DrawHUDEvent = null;

        // Load new scene
        currentScene = scene;
        
        // Finish loading scene next frame to make sure that things running this frame wont interfere with the new scene
        Main.UpdateEvent += AddSubscribers;
    }
    
    private static void AddSubscribers() {
        Camera.Reset();
        Main.BackgroundColor = Color.CornflowerBlue;
        currentScene.LoadScene();
        Main.UpdateEvent -= AddSubscribers;
    }
}

public interface IScene {
    public void LoadScene() { }
    public void UnloadScene() { }
}
