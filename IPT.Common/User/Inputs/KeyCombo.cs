using System;
using System.Windows.Forms;

namespace IPT.Common.User.Inputs
{
    public sealed class KeyCombo : GenericCombo, IEquatable<KeyCombo>
    {
        public KeyCombo(Keys primary, Keys secondary)
            : base(primary, secondary)
        {
        }

        public Keys PrimaryKey
        {
            get
            {
                return (Keys)this.Primary;
            }
        }

        public Keys SecondaryKey
        {
            get
            {
                return (Keys)this.Secondary;
            }
        }

        public override bool HasSecondary
        {
            get
            {
                return this.SecondaryKey != Keys.None;
            }
        }

        public bool Equals(KeyCombo other)
        {
            return this.PrimaryKey == other.PrimaryKey && this.SecondaryKey == other.SecondaryKey;
        }
    }
}
