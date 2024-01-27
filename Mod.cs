using KitchenLib;
using KitchenLib.Logging;
using KitchenMods;
using System.Reflection;
using CreativeModeReborn.Components;
using CreativeModeReborn.Views;
using KitchenLib.Utils;

namespace CreativeModeReborn
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.starfluxgames.creativemodereborn";
        public const string MOD_NAME = "Creative Mode Reborn";
        public const string MOD_VERSION = "0.1.1";
        public const string MOD_AUTHOR = "StarFluxGames";
        public const string MOD_GAMEVERSION = ">=1.1.7";

        public static KitchenLogger Logger;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            Logger.LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            Logger = InitLogger();
            ViewUtils.RegisterView("CreativeModeView", typeof(SCreativeView), typeof(SpawnAppliancesView));
            ViewUtils.RegisterView("ApplianceMenuView", typeof(SApplianceMenuView), typeof(ApplianceMenuView));
        }
    }
}

