using Rocket.Unturned.Player;
using Rocket.Unturned.Plugins;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace fr34kyn01535.BonusXP
{
    internal class BonusXPComponent : UnturnedPlayerComponent
    {
        private Timer timer;

        public uint BonusExperience
        {
            get;
            set;
        }

        public uint CurrentExperience
        {
            get;
            set;
        }

        public bool RecievingExperience
        {
            get;
            set;
        }

        public BonusXPComponent()
        {
        }

        internal void AddBonusExperience(uint experience)
        {
            BonusXPComponent bonusExperience = this;
            bonusExperience.BonusExperience = bonusExperience.BonusExperience + experience;
            this.CurrentExperience = Player.Experience;
            this.ResetTimer();
        }

        public uint GiveXP(uint experience)
        {
            this.RecievingExperience = true;
            Player.Experience = experience;
            this.RecievingExperience = false;
            return experience;
        }

        public void RemoveXP(uint experience)
        {
            this.RecievingExperience = true;
            Player.Experience -= experience;
            this.RecievingExperience = false;
        }

        private void ResetTimer()
        {
            this.timer = new Timer((object obj) => {
                this.RecievingExperience = true;
                Player.Experience += BonusExperience;
                this.BonusExperience = 0;
                this.timer.Dispose();
            }, null, (long)BonusXP.Instance.Configuration.Instance.RewardDelay, (long)-1);
        }

        public void ResetXP()
        {
            this.RecievingExperience = true;
            Player.Experience -= Player.Experience;
            this.RecievingExperience = false;
        }

        public void Start()
        {
            this.BonusExperience = 0;
            this.CurrentExperience = Player.Experience;
        }
    }
}