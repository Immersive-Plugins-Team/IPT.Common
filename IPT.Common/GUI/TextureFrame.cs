using System.Collections.Generic;
using System.Drawing;
using IPT.Common;
using IPT.Common.GUI;
using Rage;

/// <summary>
/// A frame for drawing a single texture.
/// </summary>
public class TextureFrame : TextureItem
{
    private Point position;
    private int scale;
    private bool isLifted = false;
    private Point cursorOffset = new Point(0, 0);
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
        this.Texture = texture;
        this.position = position;
        this.scale = scale;
        this.RectF = default;
        this.IsVisible = false;
        this.Refresh();
    }

    /// <inheritdoc/>
    public override Point Position
    {
        get
        {
            return this.position;
        }

        protected set
        {
            this.position = value;
            this.Refresh();
        }
    }

    /// <summary>
    /// Gets or sets the scale.
    /// </summary>
    public int Scale
    {
        get
        {
            return this.scale;
        }

        set
        {
            this.scale = IPT.Common.API.Math.Clamp(value, Constants.MinScale, Constants.MaxScale);
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
    /// Draws the texture to the graphics object.
    /// </summary>
    /// <param name="g">The Rage Graphics object.</param>
    public override void Draw(Rage.Graphics g)
    {
        if (this.isLifted)
        {
            this.DrawBorder(g, Color.LimeGreen);
        }

        g.DrawTexture(this.Texture, this.RectF);
        this.sprites.ForEach(x => x.Draw(g));
    }

    /// <summary>
    /// Gets a value indicating whether or not a cursor is positioned within a frame.
    /// </summary>
    /// <param name="cursor">The cursor to evaluate.</param>
    /// <returns>A value indicating whether or not the cursor is positioned within the frame.</returns>
    public bool Contains(Cursor cursor)
    {
        var resolution = Game.Resolution;
        var cursorPosition = new PointF(cursor.Position.X * (resolution.Width / Constants.CanvasWidth), cursor.Position.Y * (resolution.Height / Constants.CanvasHeight));
        return this.RectF.Contains(cursorPosition);
    }

    /// <summary>
    /// Drops the frame.
    /// </summary>
    public void Drop()
    {
        this.isLifted = false;
    }

    /// <summary>
    /// Lifts the frame.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    public void Lift(Cursor cursor)
    {
        this.cursorOffset = new Point(cursor.Position.X - this.Position.X, cursor.Position.Y - this.Position.Y);
        this.isLifted = true;
    }

    /// <summary>
    /// Moves the frame to the cursor.
    /// </summary>
    /// <param name="cursor">The cursor.</param>
    public void MoveTo(Cursor cursor)
    {
        this.Position = new Point(cursor.Position.X - this.cursorOffset.X, cursor.Position.Y - this.cursorOffset.Y);
        this.Refresh();
    }

    /// <summary>
    /// Moves the frame to the given position which should be coordinates on the base canvas.
    /// </summary>
    /// <param name="position">The position.</param>
    public override void MoveTo(Point position)
    {
        this.Position = new Point(position.X, position.Y);
        this.Refresh();
    }

    /// <summary>
    /// Refreshes the underlying frame rectangle based on resolution and scale.  This should be called when the resolution changes.
    /// </summary>
    public void Refresh()
    {
        var resolution = Game.Resolution;
        var xScale = resolution.Width / Constants.CanvasWidth;
        var yScale = resolution.Height / Constants.CanvasHeight;
        var size = new SizeF(this.Texture.Size.Width * yScale, this.Texture.Size.Height * yScale);
        var xOffset = Constants.CanvasWidth - this.Position.X - this.Texture.Size.Width;
        var x = xOffset > (Constants.CanvasWidth / 2) ? this.Position.X * xScale : resolution.Width - (xOffset * xScale) - size.Width;
        var y = this.Position.Y * yScale;
        var scale = this.Scale / 100f;
        this.RectF = new RectangleF(new PointF(x, y), new SizeF(size.Width * scale, size.Height * scale));
        this.sprites.ForEach(sprite => sprite.Refresh(this.RectF.Location, scale));
    }

    /// <summary>
    /// Rescales the frame based on the given offset.
    /// </summary>
    /// <param name="offset">The amount to offset the scale.</param>
    public void Rescale(int offset)
    {
        this.Scale += offset;
    }

    /// <summary>
    /// Draws a border around the texture.
    /// </summary>
    /// <param name="g">The Rage.Graphics object to draw on.</param>
    /// <param name="color">The color of the border.</param>
    protected void DrawBorder(Rage.Graphics g, Color color)
    {
        var borderRect = this.RectF;
        borderRect.Inflate(5f, 5f);
        g.DrawRectangle(borderRect, color);
    }
}
