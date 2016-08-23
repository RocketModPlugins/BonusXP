using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System.Linq;

namespace fr34kyn01535.BonusXP
{
    internal class BonusXP : RocketPlugin<BonusXPConfiguration>
    {
        public static BonusXP Instance;

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    { "bonusxp_reward_multiplier", "+ {0} XP from your x{1} Bonus XP." },
                    { "bonusxp_reward_no_multiplier", "You have been awarded {0} XP." },
                    { "bonusxp_reward_player_kill", "You have been awarded {0} XP for killing {1}." },
                    { "bonusxp_reward_reset", "Your XP has been reset to 0." },
                    { "bonusxp_error_target_not_found", "Player {0} not found!" }
                };
            }
        }

        public BonusXP()
        {
        }

        private ushort getBonus(IRocketPlayer caller)
        {
            return (
                from p in caller.GetPermissions()
                where p.Name.ToLower().StartsWith("bonusxp.x")
                select ushort.Parse(p.Name.ToLower().Replace("bonusxp.x", "")) into p
                orderby p
                select p).FirstOrDefault<ushort>();
        }

        protected override void Load()
        {
            BonusXP.Instance = this;
            if (Configuration.Instance.Enabled)
            {
                UnturnedPlayerEvents.OnPlayerUpdateExperience += OnPlayerUpdateExperience;
                UnturnedPlayerEvents.OnPlayerDeath += OnPlayerDeath;
            }
        }

        private void OnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            UnturnedPlayer rocketPlayer = UnturnedPlayer.FromCSteamID(murderer);
            if (rocketPlayer != null && (cause == EDeathCause.GUN || cause == EDeathCause.MELEE || cause == EDeathCause.PUNCH || cause == EDeathCause.ROADKILL))
            {
                BonusXPComponent component = rocketPlayer.GetComponent<BonusXPComponent>();
                component.GiveXP(Configuration.Instance.PlayerKillXP);
            }
        }

        private void OnPlayerUpdateExperience(UnturnedPlayer player, uint experience)
        {
            if (!Configuration.Instance.Enabled)
            {
                return;
            }
            uint bonus = this.getBonus((IRocketPlayer)player);
            if (bonus <= 1)
            {
                return;
            }
            BonusXPComponent component = player.Player.transform.GetComponent<BonusXPComponent>();
            if (experience <= component.CurrentExperience)
            {
                component.CurrentExperience = experience;
                return;
            }
            if (!component.RecievingExperience)
            {
                uint num = experience - component.CurrentExperience;
                component.AddBonusExperience(num * bonus - num);
                return;
            }
            if (component.BonusExperience != 0)
            {
                object[] bonusExperience = new object[] { component.BonusExperience, bonus };
                UnturnedChat.Say(player, Translations.Instance.Translate("bonusxp_reward_multiplier", bonusExperience), BonusXP.Instance.Configuration.Instance.Color);
            }
            component.RecievingExperience = false;
            component.CurrentExperience = experience;
        }

        protected override void Unload()
        {
            if (Configuration.Instance.Enabled)
            {
                UnturnedPlayerEvents.OnPlayerUpdateExperience -= OnPlayerUpdateExperience;
                UnturnedPlayerEvents.OnPlayerDeath -= OnPlayerDeath;
            }
        }
    }
}