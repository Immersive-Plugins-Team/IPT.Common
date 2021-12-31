using System;
using IPT.Common.User.Settings;
using RAGENativeUI;

namespace IPT.Common.UX
{
    /// <summary>
    /// Extends the UIMenuListItem class to incorporate a Setting object.
    /// </summary>
    public class UXMenuItem : RAGENativeUI.Elements.UIMenuListItem
    {
        private readonly Setting setting;

        /// <summary>
        /// Initializes a new instance of the <see cref="UXMenuItem"/> class.
        /// </summary>
        /// <param name="setting">The Setting object to associate with this menu item.</param>
        public UXMenuItem(Setting setting)
            : base(setting.Name, setting.Description)
        {
            this.setting = setting;
            if (this.setting is SettingBool settingBool)
            {
                this.Collection = new DisplayItemsCollection
                {
                    { false, "False" },
                    { true, "True" },
                };
                this.Index = settingBool.Value ? 1 : 0;
            }
            else if (this.setting is SettingFloat settingFloat)
            {
                this.Collection = new DisplayItemsCollection();
                string format = settingFloat.Increment < 1 ? "0.00" : "0";
                for (float i = settingFloat.Min; i <= settingFloat.Max; i += settingFloat.Increment)
                {
                    this.Collection.Add(i, i.ToString(format));
                }

                this.Index = 0;
                for (int i = 0; i < this.Collection.Count; i++)
                {
                    if ((float)this.Collection[i].Value >= settingFloat.Value)
                    {
                        this.Index = i;
                        break;
                    }
                }
            }
            else if (this.setting is SettingInt settingInt)
            {
                this.Collection = new DisplayItemsCollection();
                for (int i = settingInt.Min; i <= settingInt.Max; i += settingInt.Increment)
                {
                    this.Collection.Add(i, i.ToString());
                }

                this.Index = 0;
                for (int i = 0; i < this.Collection.Count; i++)
                {
                    if ((int)this.Collection[i].Value >= settingInt.Value)
                    {
                        this.Index = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the given type can be used with UXMenuItem.
        /// </summary>
        /// <param name="type">The desired type.</param>
        /// <returns>True if supported.</returns>
        public static bool IsSupportedType(Type type)
        {
            return type == typeof(SettingBool) || type == typeof(SettingFloat) || type == typeof(SettingInt);
        }

        /// <summary>
        /// Updates the Setting value based on the selected value.
        /// </summary>
        public void Update()
        {
            this.setting.SetValue(this.SelectedValue);
        }
    }
}
