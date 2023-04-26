using System.Linq;
using IPT.Common.API;
using IPT.Common.RawUI.Interfaces;
using IPT.Common.RawUI.Util;

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
            if (this.Canvas.HoveredControl != null)
            {
                if (this.Canvas.HoveredControl.Contains(this.Canvas.Cursor))
                {
                    if (this.Canvas.HoveredControl is IScrollable scrollable)
                    {
                        scrollable.Scroll(this.Canvas.Cursor.ScrollWheelStatus);
                    }
                }
                else
                {
                    this.Canvas.HoveredControl = null;
                    this.Canvas.Cursor.SetCursorType(CursorType.Default);
                }
            }
            else if (this.Canvas.HoveredWidget != null)
            {
                if (this.Canvas.HoveredWidget.Contains(this.Canvas.Cursor))
                {
                    var controls = ControlFinder.FindControls(this.Canvas.HoveredWidget).ToList();
                    if (controls.Count > 0)
                    {
                        var hoveredControl = controls.Where(x => x.Contains(this.Canvas.Cursor)).ToList().LastOrDefault();
                        if (hoveredControl != null)
                        {
                            if (hoveredControl is IClickable)
                            {
                                this.Canvas.Cursor.SetCursorType(CursorType.Pointing);
                            }

                            this.Canvas.HoveredControl = hoveredControl;
                        }
                    }
                }
                else
                {
                    this.Canvas.HoveredWidget.IsHovered = false;
                    this.Canvas.HoveredWidget = null;
                }
            }
            else
            {
                for (int i = this.Canvas.Items.Count - 1; i >= 0; i--)
                {
                    if (this.Canvas.Items[i] is IWidget widget && widget.Contains(this.Canvas.Cursor))
                    {
                        widget.IsHovered = true;
                        this.Canvas.HoveredWidget = widget;
                        break;
                    }
                }
            }
        }
    }
}
