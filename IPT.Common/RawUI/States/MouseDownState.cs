﻿using IPT.Common.API;
using IPT.Common.RawUI.Elements;
using IPT.Common.RawUI.Interfaces;
using IPT.Common.RawUI.Util;

namespace IPT.Common.RawUI.States
{
    /// <summary>
    /// Represents the mouse in a down state.
    /// </summary>
    public class MouseDownState : MouseState
    {
        /// <inheritdoc/>
        public override void UpdateWidgets(Cursor cursor, WidgetManager widgetManager)
        {
            if (cursor.MouseStatus != MouseStatus.Down)
            {
                this.ReleaseMouse(widgetManager);
            }
            else
            {
                this.PressMouse(widgetManager, cursor);
            }
        }

        private void ReleaseMouse(WidgetManager widgetManager)
        {
            if (widgetManager.PressedWidget != null)
            {
                if (widgetManager.PressedWidget.IsDragging)
                {
                    widgetManager.PressedWidget.StopDrag();
                }
                else
                {
                    if (widgetManager.HoveredControl != null && widgetManager.HoveredControl is IClickable clickable)
                    {
                        Logging.Debug("registered click on hovered control");
                        clickable.Click();
                    }
                }

                widgetManager.PressedWidget = null;
            }

            widgetManager.SetMouseState(new MouseUpState());
        }

        private void PressMouse(WidgetManager widgetManager, Cursor cursor)
        {
            var widget = widgetManager.PressedWidget;
            if (widget == null)
            {
                return;
            }

            if (widget.IsDragging)
            {
                widget.Drag(cursor.Position);
                if (cursor.ScrollWheelStatus != ScrollWheelStatus.None)
                {
                    widget.SetWidgetScale(widget.WidgetScale + (cursor.ScrollWheelStatus == ScrollWheelStatus.Up ? Constants.RescaleIncrement : -Constants.RescaleIncrement));
                    // widget.SetWidgetScale(widget.WidgetScale + (cursor.ScrollWheelStatus == ScrollWheelStatus.Up ? 0.05f : -0.05f));
                }
            }
            else
            {
                if (widget.Contains(cursor))
                {
                    if (cursor.ClickDuration > Constants.LongClick)
                    {
                        widget.StartDrag(cursor.Position);

                        // todo: this.Canvas.BringToFront(widget);
                    }
                }
                else
                {
                    widgetManager.PressedWidget = null;
                }
            }
        }
    }
}
