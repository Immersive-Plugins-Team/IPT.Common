﻿using System;
using System.Collections.Generic;
using System.Linq;
using IPT.Common.API;
using IPT.Common.RawUI.Interfaces;
using IPT.Common.RawUI.States;
using IPT.Common.RawUI.Widgets;

namespace IPT.Common.RawUI.Util
{
    /// <summary>
    /// Used by the canvas to manage the widgets.
    /// </summary>
    internal class WidgetManager
    {
        private readonly List<IWidget> widgets = new List<IWidget>();
        private MouseState mouseState = new MouseUpState();

        /// <summary>
        /// Gets or sets the control that is currently being hovered.
        /// </summary>
        public IControl HoveredControl { get; set; } = null;

        /// <summary>
        /// Gets or sets the widget that is currently being hovered.
        /// </summary>
        public IWidget HoveredWidget { get; set; } = null;

        /// <summary>
        /// Gets or sets the widget that is currently being pressed on by the mouse.
        /// </summary>
        public IWidget PressedWidget { get; set; } = null;

        /// <summary>
        /// Add a widget to the manager.
        /// </summary>
        /// <param name="widget">The widget to be managed.</param>
        public void AddWidget(IWidget widget)
        {
            this.widgets.Add(widget);
        }

        /// <summary>
        /// Moves the widget to the top.
        /// </summary>
        /// <param name="widget">The widget to bring forward.</param>
        public void BringToFront(IWidget widget)
        {
            lock (this.widgets)
            {
                this.widgets.Remove(widget);
                this.AddWidget(widget);
            }
        }

        /// <summary>
        /// Clears all of the widgets.
        /// </summary>
        public void ClearWidgets()
        {
            this.widgets.Clear();
        }

        /// <summary>
        /// Draw the widgets.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        public void Draw(Rage.Graphics g)
        {
            this.widgets.Where(x => x.IsVisible).ToList().ForEach(x => x.Draw(g));
        }

        /// <summary>
        /// Handles mouse input via the cursor.
        /// </summary>
        /// <param name="cursor">The cursor containing the mouse input.</param>
        public void HandleMouseEvents(Cursor cursor)
        {
            this.mouseState.UpdateWidgets(cursor, this);
        }

        /// <summary>
        /// Updates the mouse state.
        /// </summary>
        /// <param name="mouseState">The mouse state.</param>
        public void SetMouseState(MouseState mouseState)
        {
            this.mouseState = mouseState;
        }

        /// <summary>
        /// Updates the Widget Manager's hovered control property.
        /// </summary>
        /// <param name="cursor">The cursor to check.</param>
        public void UpdateHoveredControl(Cursor cursor)
        {
            if (this.HoveredControl == null || !this.HoveredControl.Contains(cursor))
            {
                this.HoveredControl = this.HoveredWidget != null ? this.GetHoveredControl(this.HoveredWidget, cursor) : null;
            }
        }

        /// <summary>
        /// Updates the Widget Manager's hovered widget property.
        /// </summary>
        /// <param name="cursor">The cursor to check.</param>
        public void UpdateHoveredWidget(Cursor cursor)
        {
            if (this.HoveredWidget == null || !this.HoveredWidget.Contains(cursor))
            {
                this.HoveredWidget = this.GetMousedOverWidget(cursor);
            }
        }

        /// <summary>
        /// Updates all of the widgets bounds, usually as a result of a change in resolution.
        /// </summary>
        public void UpdateWidgetBounds()
        {
            this.widgets.ForEach(x => x.UpdateBounds());
        }

        private IControl GetHoveredControl(IWidget widget, Cursor cursor)
        {
            return ControlFinder.FindControls(widget).FirstOrDefault(x => x.Contains(cursor));
        }

        private IWidget GetMousedOverWidget(Cursor cursor)
        {
            for (int i = this.widgets.Count - 1; i >= 0; i--)
            {
                if (this.widgets[i].Contains(cursor))
                {
                    return this.widgets[i];
                }
            }

            return null;
        }
    }
}
