using IPT.Common.API;

namespace IPT.Common.RawUI.Canvas
{
    /// <summary>
    /// Represents the mouse in a down state.
    /// </summary>
    public class MouseDownState : MouseState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseDownState"/> class.
        /// </summary>
        /// <param name="canvas">The context canvas for the state.</param>
        public MouseDownState(Canvas canvas)
            : base(canvas)
        {
        }

        /// <inheritdoc/>
        public override void UpdateWidgets()
        {
            if (this.Canvas.Cursor.MouseStatus != MouseStatus.Down)
            {
                this.ReleaseMouse();
            }
            else if (this.Canvas.ActiveWidget != null)
            {
                this.UpdateActiveWidget(this.Canvas.ActiveWidget, this.Canvas.Cursor);
            }
        }

        private void ReleaseMouse()
        {
            Logging.Debug("mouse has been released");
            if (this.Canvas.ActiveWidget != null)
            {
                if (this.Canvas.ActiveWidget.IsDragging)
                {
                    Logging.Debug("stopping drag");
                    this.Canvas.ActiveWidget.StopDrag();
                }
                else
                {
                    if (this.Canvas.HoveredControl != null)
                    {
                        Logging.Debug("registered click on hovered control");
                        this.Canvas.HoveredControl.Click();
                    }
                }

                this.Canvas.ActiveWidget = null;
            }

            Logging.Debug("setting mouse state to up");
            this.Canvas.SetMouseState(new MouseUpState(this.Canvas));
        }

        private void UpdateActiveWidget(IWidget widget, Cursor cursor)
        {
            if (widget.IsDragging)
            {
                widget.Drag(cursor.Position);
                if (cursor.ScrollWheelStatus != ScrollWheelStatus.None)
                {
                    widget.SetWidgetScale(widget.WidgetScale + (cursor.ScrollWheelStatus == ScrollWheelStatus.Up ? 0.05f : -0.05f));
                }
            }
            else
            {
                if (widget.Contains(cursor))
                {
                    if (cursor.ClickDuration > Constants.LongClick)
                    {
                        Logging.Debug("starting drag...");
                        Logging.Debug($"widget position: {widget.Position}");
                        Logging.Debug($"cursor position: {cursor.Position}");
                        widget.StartDrag(cursor.Position);
                        this.Canvas.BringToFront(widget);
                        Logging.Debug($"drag offset    : {widget.DragOffset}");
                    }
                }
                else
                {
                    this.Canvas.ActiveWidget = null;
                }
            }
        }
    }
}
