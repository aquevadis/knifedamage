using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;
using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;


namespace KnifeDamage;

[MinimumApiVersion(142)]


public class KnifeDamage : BasePlugin
{
    public override string ModuleName => "Knife Damage";
    public override string ModuleAuthor => "n/a";
    public override string ModuleVersion => "0.0.1";

    private enum PrintTo
    {
        Chat = 1,
        ChatAll,
        Console,
        ConsoleSucess
    }

    public override void Load(bool hotReload)
    {
        

        VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Hook(hook =>
        {
            var entity = hook.GetParam<CEntityInstance>(0);
            var info = hook.GetParam<CTakeDamageInfo>(1);

            var player = Utilities.GetPlayerFromIndex((int)entity.Index + 1);
            if (player == null || !player.IsValid) 
                return HookResult.Continue;

            CPlayer_WeaponServices? weaponServices = player.PlayerPawn?.Value?.WeaponServices;
            if (weaponServices == null) 
                return HookResult.Continue;

            SendMessageToSpecificChat(msg: $"Player {player.SteamID} fired with weapon ======> BitsDamageType: {(int)info.BitsDamageType}", print: PrintTo.ChatAll);

            if (info.BitsDamageType is not 4) return HookResult.Continue;
            info.Damage = 0f;
            
            return HookResult.Changed;
            
            
        }, HookMode.Pre);
        
        Console.WriteLine("Hooked Take Damage Func! \n!");


    }

    public override void Unload(bool hotReload)
    {
        //
        
    }

    //helpers

    internal static Dictionary<int, string> WeaponDefIndex { get; } = new Dictionary<int, string>
	{
		{ 1, "weapon_deagle" },
		{ 2, "weapon_elite" },
		{ 3, "weapon_fiveseven" },
		{ 4, "weapon_glock" },
		{ 7, "weapon_ak47" },
		{ 8, "weapon_aug" },
		{ 9, "weapon_awp" },
		{ 10, "weapon_famas" },
		{ 11, "weapon_g3sg1" },
		{ 13, "weapon_galilar" },
		{ 14, "weapon_m249" },
		{ 16, "weapon_m4a1" },
		{ 17, "weapon_mac10" },
		{ 19, "weapon_p90" },
		{ 23, "weapon_mp5sd" },
		{ 24, "weapon_ump45" },
		{ 25, "weapon_xm1014" },
		{ 26, "weapon_bizon" },
		{ 27, "weapon_mag7" },
		{ 28, "weapon_negev" },
		{ 29, "weapon_sawedoff" },
		{ 30, "weapon_tec9" },
		{ 31, "weapon_taser" },
		{ 32, "weapon_hkp2000" },
		{ 33, "weapon_mp7" },
		{ 34, "weapon_mp9" },
		{ 35, "weapon_nova" },
		{ 36, "weapon_p250" },
		{ 38, "weapon_scar20" },
		{ 39, "weapon_sg556" },
		{ 40, "weapon_ssg08" },
		{ 60, "weapon_m4a1_silencer" },
		{ 61, "weapon_usp_silencer" },
		{ 63, "weapon_cz75a" },
		{ 64, "weapon_revolver" },
		{ 500, "weapon_bayonet" },
		{ 503, "weapon_knife_css" },
		{ 505, "weapon_knife_flip" },
		{ 506, "weapon_knife_gut" },
		{ 507, "weapon_knife_karambit" },
		{ 508, "weapon_knife_m9_bayonet" },
		{ 509, "weapon_knife_tactical" },
		{ 512, "weapon_knife_falchion" },
		{ 514, "weapon_knife_survival_bowie" },
		{ 515, "weapon_knife_butterfly" },
		{ 516, "weapon_knife_push" },
		{ 517, "weapon_knife_cord" },
		{ 518, "weapon_knife_canis" },
		{ 519, "weapon_knife_ursus" },
		{ 520, "weapon_knife_gypsy_jackknife" },
		{ 521, "weapon_knife_outdoor" },
		{ 522, "weapon_knife_stiletto" },
		{ 523, "weapon_knife_widowmaker" },
		{ 525, "weapon_knife_skeleton" },
		{ 526, "weapon_knife_kukri" }
	};

    private void SendMessageToSpecificChat(CCSPlayerController handle = null!, string msg = "",
        PrintTo print = PrintTo.Chat)
    {
        var colorText = ReplaceColorTags(" {GREEN}[BG-KoKa]{RED}[RS]{DEFAULT} \u226B");

        switch (print)
        {
            case PrintTo.Chat:
                if (!handle.IsValid) return;
                handle.PrintToChat($"{colorText} {msg}");
                return;
            case PrintTo.ChatAll:
                Server.PrintToChatAll($"{colorText} {msg}");
                return;
            case PrintTo.Console:
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{colorText} {msg}");
                Console.ResetColor();
                return;
            case PrintTo.ConsoleSucess:
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{colorText} {msg}");
                Console.ResetColor();
            return;
        }
    }

    private string ReplaceColorTags(string input)
    {
        string[] colorPatterns =
        {
            "{DEFAULT}", "{WHITE}", "{GREEN}", "{LIGHTYELLOW}", "{LIGHTBLUE}", "{OLIVE}", "{LIME}",
            "{RED}", "{LIGHTPURPLE}", "{PURPLE}", "{GREY}", "{YELLOW}", "{GOLD}", "{SILVER}", "{BLUE}", "{DARKBLUE}",
            "{BLUEGREY}", "{MAGENTA}", "{LIGHTRED}", "{ORANGE}"
        };

        string[] colorReplacements =
        {
            $"{ChatColors.Default}", $"{ChatColors.White}", $"{ChatColors.Green}",
            $"{ChatColors.LightYellow}", $"{ChatColors.LightBlue}", $"{ChatColors.Olive}", $"{ChatColors.Lime}",
            $"{ChatColors.Red}", $"{ChatColors.LightPurple}", $"{ChatColors.Purple}", $"{ChatColors.Grey}",
            $"{ChatColors.Yellow}", $"{ChatColors.Gold}", $"{ChatColors.Silver}", $"{ChatColors.Blue}",
            $"{ChatColors.DarkBlue}", $"{ChatColors.BlueGrey}", $"{ChatColors.Magenta}", $"{ChatColors.LightRed}",
            $"{ChatColors.Orange}"
        };

        for (var i = 0; i < colorPatterns.Length; i++)
            input = input.Replace(colorPatterns[i], colorReplacements[i]);

        return input;
    }
}
