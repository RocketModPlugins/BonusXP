using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using Rocket.API.Extensions;

namespace fr34kyn01535.BonusXP
{
    internal class BonusXPCommand : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>()
                {
                    "xp"
                };
            }
        }

        public string Help
        {
            get
            {
                return "Give or Remove XP to players.";
            }
        }

        public string Name
        {
            get
            {
                return "bonusxp";
            }
        }

        public string Syntax
        {
            get
            {
                return "<reset> | <give |remove> <player>";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "bonusxp.xp" };
            }
        }

        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Both;
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            string stringParameter = command.GetStringParameter(0);
            UnturnedPlayer rocketPlayerParameter = command.GetUnturnedPlayerParameter(1);
            uint? uInt32Parameter = command.GetUInt32Parameter(2);
            if (string.IsNullOrEmpty(stringParameter) || rocketPlayerParameter == null || stringParameter != "reset" && !uInt32Parameter.HasValue)
            {
                UnturnedChat.Say(caller, BonusXP.Instance.Translations.Instance.Translate("command_generic_invalid_parameter", new object[0]), BonusXP.Instance.Configuration.Instance.Color);
                return;
            }
            BonusXPComponent component = rocketPlayerParameter.GetComponent<BonusXPComponent>();
            string lower = stringParameter.ToLower();
            string str = lower;
            if (lower != null)
            {
                if (str == "give")
                {
                    BonusXP instance = BonusXP.Instance;
                    object[] objArray = new object[] { component.GiveXP(uInt32Parameter.Value) };
                    UnturnedChat.Say(rocketPlayerParameter, BonusXP.Instance.Translations.Instance.Translate("bonusxp_reward_no_multiplier", objArray), BonusXP.Instance.Configuration.Instance.Color);
                    return;
                }
                if (str == "remove")
                {
                    component.RemoveXP(uInt32Parameter.Value);
                    return;
                }
                if (str != "reset")
                {
                    return;
                }
                component.ResetXP();
            }
        }
    }
}