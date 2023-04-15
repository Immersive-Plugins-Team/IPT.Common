using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using IPT.Common.API;
using Rage;
using Rage.Native;

/// <summary>
/// A class for managing one or more texture frames.
/// </summary>
public class BaseFrame : IPT.Common.Fibers.GenericFiber
{
    private readonly List<TextureFrame> frames = new List<TextureFrame>();
    private readonly Cursor cursor;

    private Size resolution;
    private bool isEditing;
    private bool isPaused;
    private TextureFrame mousedFrame;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseFrame"/> class.
    /// </summary>
    public BaseFrame()
        : base("baseframe", 100)
    {
        this.mousedFrame = null;
        this.resolution = default;
        this.isEditing = false;
        this.isPaused = false;
        this.cursor = new Cursor();
    }

    /// <summary>
    /// Adds a texture frame to the base frame.
    /// </summary>
    /// <param name="frame">The frame to add.</param>
    public void AddFrame(TextureFrame frame)
    {
        this.frames.Add(frame);
    }

    /// <summary>
    /// Moves any frames matching the given name.
    /// </summary>
    /// <param name="name">The name of the frame.</param>
    /// <param name="position">The new position of the frame.</param>
    public void MoveFrame(string name, Point position)
    {
        foreach (var entry in this.frames)
        {
            if (entry.Name == name)
            {
                entry.Position = position;
            }
        }
    }

    /// <summary>
    /// Rescales any frames matching the given name.
    /// </summary>
    /// <param name="name">The name of the frame.</param>
    /// <param name="scale">The new scale of the frame.</param>
    public void RescaleFrame(string name, int scale)
    {
        foreach (var entry in this.frames)
        {
            if (entry.Name == name)
            {
                entry.Scale = scale;
            }
        }
    }

    /// <summary>
    /// Changes into an edit mode where the textures can be respositioned and resized.
    /// </summary>
    public void EditMode()
    {
        this.isEditing = true;
        Game.IsPaused = true;
    }

    /// <summary>
    /// Starts the fiber.
    /// </summary>
    public override void Start()
    {
        Logging.Info("Starting base frame");
        if (this.frames.Count == 0)
        {
            Logging.Warning("there are no texture frames to manage!");
        }
        else
        {
            Logging.Info("Starting base frame");
            Game.FrameRender += this.Game_FrameRender;
            Game.RawFrameRender += this.Game_RawFrameRender;
            base.Start();
        }
    }

    /// <summary>
    /// Stops the fiber.
    /// </summary>
    public override void Stop()
    {
        Logging.Info("stopping base frame");
        Game.FrameRender -= this.Game_FrameRender;
        Game.RawFrameRender -= this.Game_RawFrameRender;
        base.Stop();
    }

    /// <summary>
    /// Fired during every fiber tick.
    /// </summary>
    protected override void DoSomething()
    {
        this.isPaused = IPT.Common.API.Functions.IsGamePaused();
        if (this.resolution != Game.Resolution)
        {
            this.resolution = Game.Resolution;
            foreach (var frame in this.frames)
            {
                frame.Refresh();
            }
        }
    }

    private void DrawBorder(Rage.Graphics g)
    {
        var scale = Game.Resolution.Height / 1080f;
        var width = scale * 10f;
        g.DrawRectangle(new RectangleF(0, 0, Game.Resolution.Width, width), Color.Green);
        g.DrawRectangle(new RectangleF(0, Game.Resolution.Height - width + 2, Game.Resolution.Width, width), Color.Green);
        g.DrawRectangle(new RectangleF(0, 0, width, Game.Resolution.Height), Color.Green);
        g.DrawRectangle(new RectangleF(Game.Resolution.Width - width + 1, 0, width, Game.Resolution.Height), Color.Green);
        g.DrawText("**EDIT MODE - RIGHT-CLICK ANYWHERE TO EXIT**", "Consolas", 50f * scale, new PointF(20, 20), Color.White);
    }

    private void ProcessEditingControls()
    {
        this.cursor.Update();
        this.UpdateMousedFrame();
        this.RescaleFrames();
    }

    private void Game_FrameRender(object sender, GraphicsEventArgs e)
    {
        this.UpdateEditingStatus();
        if (this.isEditing)
        {
            this.ProcessEditingControls();
        }
    }

    private void Game_RawFrameRender(object sender, GraphicsEventArgs e)
    {
        if (this.isEditing)
        {
            this.DrawBorder(e.Graphics);
            this.frames.ForEach(x => x.Draw(e.Graphics));
            this.cursor.Draw(e.Graphics);
        }
        else if (!this.isPaused)
        {
            this.frames.Where(x => x.IsVisible).ToList().ForEach(x => x.Draw(e.Graphics));
        }
    }

    private void RescaleFrames()
    {
        if (this.cursor.Scale != 0)
        {
            var frame = this.frames.LastOrDefault(x => x.Contains(this.cursor));
            if (frame != null)
            {
                frame.Rescale(this.cursor.Scale);
            }
        }
    }

    private void UpdateEditingStatus()
    {
        if (this.isEditing)
        {
            if (Game.Console.IsOpen)
            {
                this.isEditing = false;
            }
            else if (NativeFunction.Natives.IS_DISABLED_CONTROL_PRESSED<bool>(0, (int)GameControl.CellphoneCancel))
            {
                this.isEditing = false;
                Game.IsPaused = false;
            }

            if (!this.isEditing)
            {
                // drop all frames just in case
                this.frames.ForEach(x => x.Drop());
            }
        }
    }

    private void UpdateMousedFrame()
    {
        if (this.mousedFrame != null)
        {
            if (this.cursor.IsMouseDown)
            {
                this.mousedFrame.Move(this.cursor);
            }
            else
            {
                this.mousedFrame.Drop();
                this.mousedFrame = null;
            }
        }
        else if (this.cursor.IsMouseDown)
        {
            var frame = this.frames.LastOrDefault(x => x.Contains(this.cursor));
            if (frame != null)
            {
                this.mousedFrame = frame;
                this.mousedFrame.Lift(this.cursor);
            }
        }
    }
}
