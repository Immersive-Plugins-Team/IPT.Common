﻿using System.Collections.Generic;
using IPT.Common.API;
using IPT.Common.RawUI.Interfaces;
using IPT.Common.RawUI.Widgets;

namespace IPT.Common.RawUI.Util
{
    /// <summary>
    /// Finds controls within a container.
    /// </summary>
    public static class ControlFinder
    {
        /// <summary>
        /// Finds any controls nested within the container.
        /// </summary>
        /// <param name="container">The container to search.</param>
        /// <returns>An enumerable of controls.</returns>
        public static IEnumerable<IControl> FindControls(IContainer container)
        {
            if (container is TabbedWidget tabbedWidget)
            {
                foreach (var item in tabbedWidget.Widgets)
                {
                    if (item is IControl control && control.IsEnabled)
                    {
                        yield return control;
                    }

                    if (item is IContainer childContainer)
                    {
                        foreach (var childControl in FindControls(childContainer))
                        {
                            yield return childControl;
                        }
                    }
                }
            }

            foreach (var item in container.Items)
            {
                if (item is IControl control && control.IsEnabled)
                {
                    yield return control;
                }

                if (item is IContainer childContainer)
                {
                    foreach (var childControl in FindControls(childContainer))
                    {
                        yield return childControl;
                    }
                }
            }
        }
    }
}
