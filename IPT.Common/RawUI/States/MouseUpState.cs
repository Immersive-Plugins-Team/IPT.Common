using System.Linq;
using IPT.Common.API;

namespace IPT.Common.RawUI.States
{
    /// <summary>
    /// Represents the mouse in an up state.
    /// </summary>
    public class MouseUpState : MouseState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseUpState"/> class.
        /// </summary>
        /// <param name="canvas">The context for the state.</param>
        public MouseUpState(Canvas canvas)
            : base(canvas)
        {
        }

        /// <inheritdoc/>
        public override void UpdateWidgets()
        {
            if (this.Canvas.Cursor.MouseStatus != MouseStatus.Up)
            {
                this.PressMouse();
            }
            else
            {
                this.UpdateHoveredWidget();
            }
        }

        private void PressMouse()
        {
            Logging.Debug("the mouse has been pressed");
            this.Canvas.ActiveWidget = this.Canvas.HoveredWidget;
            Logging.Debug($"the active widget is {(this.Canvas.ActiveWidget == null ? string.Empty : "not ")}null");
            Logging.Debug("setting mouse state to down");
            this.Canvas.SetMouseState(new MouseDownState(this.Canvas));
        }

        private void UpdateHoveredWidget()
        {
            bool hoveredItemFound = false;
            for (int i = this.Canvas.Items.Count - 1; i >= 0; i--)
            {
                var item = this.Canvas.Items[i];
                if (this.Canvas.Items[i] is IWidget widget)
                {
                    if (!hoveredItemFound && widget.Contains(this.Canvas.Cursor))
                    {
                        Logging.Debug($"found hovered widget at {this.Canvas.Cursor.Bounds}");
                        widget.IsHovered = true;
                        hoveredItemFound = true;
                        this.Canvas.HoveredWidget = widget;
                        Logging.Info("searching hovered widget to see if it contains any controls");
                        var controls = ControlFinder.FindControls(widget).ToList();
                        Logging.Info($"we found {controls.Count} controls");
                        if (controls.Count > 0)
                        {
                            Logging.Info("checking to see if any contain the cursor");
                            var hoveredControl = controls.Where(x => x.Contains(this.Canvas.Cursor)).ToList().LastOrDefault();
                            if (hoveredControl != null)
                            {
                                Logging.Info("we found one, setting it to the hoveredControl");
                                this.Canvas.HoveredControl = hoveredControl;
                            }
                            else
                            {
                                Logging.Info("none of the controls are being hovered over");
                                this.Canvas.HoveredControl = null;
                            }
                        }
                    }
                    else
                    {
                        widget.IsHovered = false;
                    }
                }
            }

            if (!hoveredItemFound)
            {
                this.Canvas.HoveredWidget = null;
            }

            this.Canvas.Cursor.SetCursorType(this.Canvas.HoveredControl == null ? CursorType.Default : CursorType.Pointing);
        }
    }
}
