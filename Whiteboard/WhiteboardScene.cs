using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Whiteboard.Misc;
using Whiteboard.Render;
using Keyboard = Adofai.Render.Keyboard;
using Mouse = Adofai.Render.Mouse;

namespace Whiteboard.Whiteboard; 

public class WhiteboardScene : IScene {
    public List<Spot> Spots = new List<Spot>();
    private List<Guid> History = new List<Guid>();
    private Guid drawGuid = Guid.NewGuid();

    private Vector2 mouseDragStart;
    private Vector2 cameraPosStart;

    private Vector2 drawPosStart;
    private int drawSize = 32;

    private bool showHelp = false;

    public void LoadScene() {
        Main.UpdateEvent += Update;
        Main.DrawEvent += Draw;
        Main.DrawHUDEvent += DrawHud;

        Spots = new List<Spot>();
    }

    private void Update() {
        bool spaceHeld = Keyboard.IsKeyHeld(Keys.Space);

        // Dragging the Camera
        if (Mouse.MiddleButtonPressed || (spaceHeld && Mouse.LeftButtonPressed)) {
            mouseDragStart = Mouse.WindowPosition.ToVector2();
            cameraPosStart = Camera.Position;
        } 
        else if (Mouse.MiddleButton || (spaceHeld && Mouse.LeftButton)) {
            Vector2 mousePos = Mouse.WindowPosition.ToVector2();
            Vector2 drag = mousePos - mouseDragStart;
            Camera.Position = cameraPosStart - drag / Camera.Zoom;
        }

        // Camera Zoom
        Camera.Zoom = Math.Max(Camera.Zoom + Mouse.ScrollFrame / 1024f, 0.05f);
        if (Keyboard.IsKeyPressed(Keys.D0, Keys.A)) Camera.Zoom = 1f;
        
        // Drawing
        // Drawing
        // Drawing
        if (!spaceHeld && Mouse.LeftButtonPressed) {
            drawPosStart = Mouse.Position.ToVector2();
            Spots.Add(new Spot(Mouse.Position.ToVector2(), drawSize, Color.Black, drawGuid));
        } 
        else if (!spaceHeld && Mouse.LeftButton) {
            Vector2 mousePos = Mouse.Position.ToVector2();
            float distance = Vector2.Distance(drawPosStart, mousePos);
            if (distance > drawSize / 2f) {
                float spacing = drawSize / 4f;
                int numSpots = (int) (distance / spacing);

                for (int i = 0; i < numSpots; i++) {
                    Spots.Add(new Spot(
                        Vector2.Lerp(drawPosStart, mousePos, i / (float) numSpots), 
                        drawSize, Color.Black, drawGuid)
                    );
                }

                drawPosStart = mousePos;
            }
        }

        // When released
        if (Mouse.LastLeftButton && !Mouse.LeftButton) {
            History.Add(drawGuid);
            drawGuid = Guid.NewGuid();
        }
        
        // Deleting
        // Deleting
        // Deleting
        if (!spaceHeld && Mouse.RightButton) {
            foreach (Spot spot in Spots) {
                if (Vector2.Distance(spot.Position.ToVector2(), Mouse.Position.ToVector2()) < spot.Size) {
                    Spots.Remove(spot);
                    break;
                }
            }
        }
        
        // Undo
        // Undo
        // Undo
        if (Keyboard.IsKeyPressed(Keys.Z) && History.Count > 0) {
            Guid uid = History[^1];
            History.RemoveAt(History.Count - 1);

            Spots.RemoveAll(spot => spot.Uid == uid);
        }
        
        // Help
        // Help
        // Help
        if (Keyboard.IsKeyPressed(Keys.H)) {
            showHelp = !showHelp;
        }
    }

    private void Draw() {
        foreach (Spot spot in Spots) {
            spot.Draw();
        }
    }

    private void DrawHud() {
        if (showHelp) {
            string[] lines = new string[] {
                "Left Click: Draw\n", 
                "Right Click: Delete\n",
                "Middle Click Drag: Move Camera\n",
                "Space + Left Click Drag: Move Camera\n",
                "Scroll: Zoom\n",
                "Z: Undo\n",
                "H: Show / Hide Help\n",
                "A or 0: Reset Zoom"
            };

            for (int i = 0; i < lines.Length; i++) {
                ARender.DrawString(lines[i], Align.Centre, Align.Centre, 
                    Camera.WindowBounds.Size.Div(2) + new Point(0, (i - 4) * 48), 4, Color.Black);
            }
        }
        else {
            ARender.DrawString("Press H for help", Align.Left, new Point(10), 5, Color.Black);
        }
    }
}