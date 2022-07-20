using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Whiteboard.Misc;
using Whiteboard.Render;
using Texture = Whiteboard.Render.Texture;

namespace Whiteboard.Whiteboard; 

public class Spot {
    private Point PSize;
    
    public Guid Uid { get; }
    public Point Position { get; }
    public float Size { get; }
    public Color Color { get; }

    public Spot(Vector2 position, int size, Color color, Guid uid) {
        Position = position.ToPoint();
        PSize = new Point(size);
        Size = size;
        Color = color;
        Uid = uid;
    }

    public void Draw() {
        if (!Camera.VisibleArea.Contains(Position)) return;
        
        Texture2D circle = Assets.GetTexture(Texture.Circle);

        ARender.Draw(
            circle, 
            new Rectangle(Position - PSize.Div(2), PSize),
            new Rectangle(Point.Zero, circle.Bounds.Size),
            Color
        );
    }
}