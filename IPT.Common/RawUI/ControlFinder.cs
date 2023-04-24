using System.Collections.Generic;

namespace IPT.Common.RawUI
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
        /// <typeparam name="T">The type of objects in the container.</typeparam>
        public static IEnumerable<IControl> FindControls<T>(IContainer<T> container)
            where T : IDrawable
        {
            foreach (var item in container.Items)
            {
                if (item is IControl control && control.IsEnabled)
                {
                    yield return control;
                }

                if (item is IContainer<T> childContainer)
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
