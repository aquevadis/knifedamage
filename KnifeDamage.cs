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


namespace EnvSkyChanger;

[MinimumApiVersion(142)]


public class EnvSkyChanger : BasePlugin
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
            var info = hook.GetParam<CTakeDamageInfo>(1);
            
            SendMessageToSpecificChat(msg: $"======> BitsDamageType: {(int)info.BitsDamageType}", print: PrintTo.ChatAll);

            // if (info.BitsDamageType is not 256) return HookResult.Continue;
            // info.Damage = 0f;
            //return HookResult.Changed;

            return HookResult.Continue;
        }, HookMode.Pre);
        
        Console.WriteLine("Hooked Take Damage Func! \n!");


    }

    public override void Unload(bool hotReload)
    {
        //
        
    }

   
    //helpers
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
