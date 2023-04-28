using IPT.Common.API;
using IPT.Common.RawUI.Interfaces;
using IPT.Common.RawUI.Util;

namespace IPT.Common.RawUI.States
{
    /// <summary>
    /// Represents the mouse in an up state.
    /// </summary>
    internal class MouseUpState : MouseState
    {
        /// <inheritdoc/>
        public override void UpdateWidgets(Cursor cursor, WidgetManager widgetManager)
        {
            if (cursor.MouseStatus != MouseStatus.Up)
            {
                this.PressMouse(cursor, widgetManager);
            }
            else
            {
                this.HandleMouse(cursor, widgetManager);
            }
        }

        private void PressMouse(Cursor cursor, WidgetManager widgetManager)
        {
            widgetManager.UpdateHoveredWidget(cursor);
            widgetManager.UpdateHoveredControl(cursor);
            widgetManager.PressedWidget = widgetManager.HoveredWidget;
            widgetManager.SetMouseState(new MouseDownState());
        }

        private void HandleMouse(Cursor cursor, WidgetManager widgetManager)
        {
            widgetManager.UpdateHoveredWidget(cursor);
            widgetManager.UpdateHoveredControl(cursor);

            if (widgetManager.HoveredControl != null)
            {
                if (widgetManager.HoveredControl is IClickable)
                {
                    cursor.SetCursorType(CursorType.Pointing);
                }
                else if (widgetManager.HoveredControl is IScrollable scrollable)
                {
                    scrollable.Scroll(cursor.ScrollWheelStatus);
                }
            }
            else
            {
                cursor.SetCursorType(CursorType.Default);
            }
        }
    }
}
