﻿using System;
using Rage;

namespace IPT.Common.User.Inputs
{
    /// <summary>
    /// A combination of one or two controller buttons.
    /// </summary>
    public sealed class ButtonCombo : GenericCombo, IEquatable<ButtonCombo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonCombo"/> class.
        /// </summary>
        /// <param name="primary">A controller button that must be pressed.</param>
        /// <param name="secondary">An optional modifier button.</param>
        public ButtonCombo(ControllerButtons primary, ControllerButtons secondary)
            : base(primary, secondary)
        {
        }

        /// <summary>
        /// Gets get the value for the primary button.
        /// </summary>
        public ControllerButtons PrimaryButton
        {
            get
            {
                return (ControllerButtons)this.Primary;
            }
        }

        /// <summary>
        /// Gets the value for the optional second button.
        /// </summary>
        public ControllerButtons SecondaryButton
        {
            get
            {
                return (ControllerButtons)this.Secondary;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the combination contains a secondary button.
        /// </summary>
        public override bool HasSecondary
        {
            get
            {
                return this.SecondaryButton != ControllerButtons.None;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the combination is the same as another combination.
        /// </summary>
        /// <param name="other">The combination to compare against.</param>
        /// <returns>True if the combinations have the same buttons.</returns>
        public bool Equals(ButtonCombo other)
        {
            return this.PrimaryButton == other.PrimaryButton && this.SecondaryButton == other.SecondaryButton;
        }

        /// <summary>
        /// Checks the game to determine the current status of the buttons.
        /// </summary>
        /// <returns>True if the game says they are currently pressed.</returns>
        protected override bool CheckGameIsPressed()
        {
            return Game.IsControllerButtonDownRightNow(this.PrimaryButton) && (!this.HasSecondary || Game.IsControllerButtonDownRightNow(this.SecondaryButton));
        }
    }
}
