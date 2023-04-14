using System.Drawing;
using Rage;
using Rage.Native;

/// <summary>
/// A class for displaying and monitoring an on-screen cursor for editing a base frame.
/// </summary>
public class Cursor
{
    /// <summary>
    /// Gets the location of the cursor on a 1920 x 1080 canvas.
    /// </summary>
    public Point Position { get; private set; } = new Point(0, 0);

    /// <summary>
    /// Gets a value indicating whether or not the mouse is currently down.
    /// </summary>
    public bool IsMouseDown { get; private set; } = false;

    /// <summary>
    /// Gets the scaling factor applied by the user.
    /// </summary>
    public int Scale { get; private set; }

    /// <summary>
    /// Draws the cursor.
    /// </summary>
    /// <param name="g">The Rage.Graphics object to draw against.</param>
    public void Draw(Rage.Graphics g)
    {
        var xScale = Game.Resolution.Width / 1920f;
        var yScale = Game.Resolution.Height / 1080f;
        g.DrawFilledCircle(new Vector2(this.Position.X * xScale, this.Position.Y * yScale), 10f, Color.Green);
    }

    /// <summary>
    /// Updates the cursor's location, click status, and scroll wheel status.
    /// </summary>
    public void Update()
    {
        var x = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.CursorX);
        var y = NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(0, (int)GameControl.CursorY);
        this.Position = new Point((int)System.Math.Round(x * 1920), (int)System.Math.Round(y * 1080));

        if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.Attack) != this.IsMouseDown)
        {
            this.IsMouseDown = !this.IsMouseDown;
        }

        if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.WeaponWheelNext))
        {
            this.Scale = -1;
        }
        else if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.WeaponWheelPrev))
        {
            this.Scale = 1;
        }
        else
        {
            this.Scale = 0;
        }
    }
}
