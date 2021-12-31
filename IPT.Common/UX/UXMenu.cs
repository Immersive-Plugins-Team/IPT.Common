namespace IPT.Common.UX
{
    /// <summary>
    /// Extends RageNativeUI's MenuUI class to dynamically adjust width for long menu items.
    /// </summary>
    public class UXMenu : RAGENativeUI.UIMenu
    {
        private int maxItemWidth = 30;

        /// <summary>
        /// Initializes a new instance of the <see cref="UXMenu"/> class.
        /// </summary>
        /// <param name="title">The title of the UIMenu.</param>
        /// <param name="subtitle">The subtitle of the UIMenu.</param>
        public UXMenu(string title, string subtitle)
            : base(title, subtitle)
        {
        }

        /// <summary>
        /// Adds a UIMenuItem to the UXMenu.
        /// </summary>
        /// <param name="item">The UIMenuItem item to be added.</param>
        public new void AddItem(RAGENativeUI.Elements.UIMenuItem item)
        {
            base.AddItem(item);
            if (item.Text.Length > this.maxItemWidth)
            {
                this.maxItemWidth = item.Text.Length;
                this.UpdateWidth();
            }
        }

        private void UpdateWidth()
        {
            this.Width = IPT.Common.API.Math.Clamp(this.maxItemWidth * 0.006f, 0.225f, 0.95f);
        }
    }
}
