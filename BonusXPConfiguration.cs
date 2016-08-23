using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using System.Xml.Serialization;
using UnityEngine;
using System;

namespace fr34kyn01535.BonusXP
{
    public class BonusXPConfiguration : IRocketPluginConfiguration
    {
        [XmlElement(ElementName = "Enabled")]
        public bool Enabled;

        [XmlIgnore]
        public Color Color;

        [XmlIgnore]
        private string _color;

        [XmlElement(ElementName = "PlayerKillXP")]
        public uint PlayerKillXP;

        [XmlElement(ElementName = "RewardDelay")]
        public uint RewardDelay;

        [XmlElement(ElementName = "Color")]
        public string color
        {
            get
            {
                return this._color;
            }
            set
            {
                Logger.Log(value);
                this.Color = UnturnedChat.GetColorFromName(value, Color.green);
                this._color = value;
            }
        }
        
        public BonusXPConfiguration()
        {
        }

        public void LoadDefaults()
        {
            Enabled = true;
            color = "#FFBD52";
            PlayerKillXP = 5;
            RewardDelay = 5000;
        }
    }
}