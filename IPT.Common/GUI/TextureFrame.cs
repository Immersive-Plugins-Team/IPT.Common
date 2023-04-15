using System.Collections.Generic;
using System.Drawing;
using IPT.Common.GUI;
using Rage;

/// <summary>
/// A frame for drawing a single texture.
/// </summary>
public class TextureFrame
{
    private Texture texture;
    private int scale = 100;
    private RectangleF frameRect = default;
    private Point cursorOffset = new Point(0, 0);
    private Point position = new Point(0, 0);
    private bool isLifted = false;
    private List<TextureSprite> sprites = new List<TextureSprite>();

    /// <summary>
    /// Initializes a new instance of the <see cref="TextureFrame"/> class.
    /// </summary>
    /// <param name="name">A name that the frame can be called.</param>
    /// <param name="texture">The underlying texture to be drawn.</param>
    /// <param name="position">The starting position of the texture.</param>
    /// <param name="scale">The scale (25-150).</param>
    public TextureFrame(string name, Texture texture, Point position = default, int scale = 100)
    {
        this.Name = name;
        this.texture = texture;
        this.Position = position;
        this.Scale = scale;
        this.Refresh();
    }

    /// <summary>
    /// Gets a value indicating the name of the frame.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the frame is visible.
    /// </summary>
    public bool IsVisible { get; set; } = false;

    /// <summary>
    /// Gets or sets the coordinates of the frame on a 1920 x 1080 canvas.
    /// </summary>
    public Point Position
    {
        get
        {
            return this.position;
        }

        set
        {
            this.position = value;
            this.Refresh();
        }
    }

    /// <summary>
    /// Gets or sets the scale from 20-200%.
    /// </summary>
    public int Scale
    {
        get
        {
            return this.scale;
        }

        set
        {
            this.scale = IPT.Common.API.Math.Clamp(value, 20, 200);
            this.Refresh();
        }
    }

    /// <summary>
    /// Adds a sprite to the texture frame.
    /// </summary>
    /// <param name="sprite">The sprite to add.</param>
    public void AddSprite(TextureSprite sprite)
    {
        this.sprites.Add(sprite);
    }

    /// <summary>
    /// Toggles the visibility of the frame.
    /// </summary>
    public void Toggle()
    {
        this.IsVisible = !this.IsVisible;
    }

    /// <summary>
    /// Draws the texture to the graphics object.
    /// </summary>
    /// <param name="g">The Rage Graphics object.</param>
    internal void Draw(Rage.Graphics g)
    {
        if (this.isLifted)
        {
            this.DrawBorder(g);
        }

        g.DrawTexture(this.texture, this.frameRect);
        this.sprites.ForEach(x => x.Draw(g));
    }

    /// <summary>
    /// Gets a value indicating whether or not a cursor is positioned within a frame.
    /// </summary>
    /// <param name="cursor">The cursor to evaluate.</param>
    /// <returns>A value indicating whether or not the cursor is positioned within the frame.</returns>
    internal bool Contains(Cursor cursor)
    {
        var resolution = Game.Resolution;
        var cursorPosition = new PointF(cursor.Position.X * (resolution.Width / 1920f), cursor.Position.Y * (resolution.Height / 1080f));
        return this.frameRect.Contains(cursorPosition);
    }

    /// <summary>
    /// Drops the frame.
    /// </summary>
    internal void Drop()
    {
        this.isLifted = false;
    }

    /// <summary>
    /// Lifts the frame.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    internal void Lift(Cursor cursor)
    {
        this.cursorOffset = new Point(cursor.Position.X - this.Position.X, cursor.Position.Y - this.Position.Y);
        this.isLifted = true;
    }

    /// <summary>
    /// Moves the frame.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    internal void Move(Cursor cursor)
    {
        this.Position = new Point(cursor.Position.X - this.cursorOffset.X, cursor.Position.Y - this.cursorOffset.Y);
        this.Refresh();
    }

    /// <summary>
    /// Refreshes the underlying frame rectangle based on resolution and scale.
    /// </summary>
    internal void Refresh()
    {
        var resolution = Game.Resolution;
        var xScale = resolution.Width / 1920f;
        var yScale = resolution.Height / 1080f;
        var size = new SizeF(this.texture.Size.Width * yScale, this.texture.Size.Height * yScale);
        var xOffset = 1920 - this.Position.X - this.texture.Size.Width;
        var x = xOffset > 960 ? this.Position.X * xScale : resolution.Width - (xOffset * xScale) - size.Width;
        var y = this.Position.Y * yScale;
        var scale = this.Scale / 100f;
        this.frameRect = new RectangleF(new PointF(x, y), new SizeF(size.Width * scale, size.Height * scale));
        this.sprites.ForEach(sprite => sprite.Refresh(this.frameRect.Location, scale));
    }

    /// <summary>
    /// Rescales the frame based on the given offset.
    /// </summary>
    /// <param name="offset">The amount to offset the scale.</param>
    internal void Rescale(int offset)
    {
        this.Scale += offset;
    }

    private void DrawBorder(Rage.Graphics g)
    {
        var borderRect = this.frameRect;
        borderRect.Inflate(5f, 5f);
        g.DrawRectangle(borderRect, Color.LimeGreen);
    }
}
