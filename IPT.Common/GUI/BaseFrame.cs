using System.Collections.Generic;
using System.Drawing;
using IPT.Common.API;
using IPT.Common.GUI;
using Rage;
using Rage.Native;

/// <summary>
/// A class for managing one or more texture frames.
/// </summary>
public class BaseFrame : IPT.Common.Fibers.GenericFiber
{
    private Size resolution;

    private bool isEditing;
    private bool isPaused;

    private List<TextureFrame> frames = new List<TextureFrame>();
    private List<TextureFrameData> frameData;
    private TextureFrame mousedFrame;
    private Cursor cursor;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseFrame"/> class.
    /// </summary>
    /// <param name="frameData">A list of frames data to manage.</param>
    public BaseFrame(List<TextureFrameData> frameData)
        : base("baseframe", 100)
    {
        this.frameData = frameData;
        this.mousedFrame = null;
        this.resolution = default;
        this.isEditing = false;
        this.isPaused = false;
        this.cursor = new Cursor();

        Logging.Info("Loading texture frames");
        this.LoadTextureFrames();
    }

    /// <summary>
    /// Gets or sets a value indicating whether the frame is visible.
    /// </summary>
    public bool IsVisible { get; set; } = false;

    /// <summary>
    /// Changes into an edit mode where the textures can be respositioned and resized.
    /// </summary>
    public void Edit()
    {
        this.isEditing = true;
        Game.IsPaused = true;
    }

    /// <summary>
    /// Starts the fiber.
    /// </summary>
    public override void Start()
    {
        // Logging.Info("Loading texture frames");
        // this.LoadTextureFrames();
        Logging.Info("Starting base frame");
        Game.FrameRender += this.Game_FrameRender;
        Game.RawFrameRender += this.Game_RawFrameRender;
        base.Start();
    }

    /// <summary>
    /// Stops the fiber.
    /// </summary>
    public override void Stop()
    {
        Logging.Info("stopping base frame");
        Game.FrameRender -= this.Game_FrameRender;
        Game.RawFrameRender -= this.Game_RawFrameRender;
    }

    /// <summary>
    /// Toggles the visibility of the frames.
    /// </summary>
    public void Toggle()
    {
        this.IsVisible = !this.IsVisible;
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

    private void DrawFrames(Rage.Graphics g)
    {
        foreach (var frame in this.frames)
        {
            frame.Draw(g);
        }
    }

    private void LoadTextureFrames()
    {
        // we have to load textures at start because they don't load correctly during instantiation
        this.frames.Clear();
        foreach (var frame in this.frameData)
        {
            var texture = Functions.LoadTexture(frame.Filename);
            if (texture != null)
            {
                this.frames.Add(new TextureFrame(texture, frame.Position, frame.Scale));
            }
        }
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
            this.DrawFrames(e.Graphics);
            this.cursor.Draw(e.Graphics);
        }
        else if (!this.isPaused && this.IsVisible)
        {
            this.DrawFrames(e.Graphics);
        }
    }

    private void RescaleFrames()
    {
        if (this.cursor.Scale != 0)
        {
            foreach (var frame in this.frames)
            {
                if (frame.Contains(this.cursor))
                {
                    frame.Rescale(this.cursor.Scale);
                }
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
            foreach (var frame in this.frames)
            {
                if (frame.Contains(this.cursor))
                {
                    this.mousedFrame = frame;
                    this.mousedFrame.Lift(this.cursor);
                }
            }
        }
    }
}
