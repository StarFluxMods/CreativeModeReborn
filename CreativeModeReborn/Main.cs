using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KitchenLib;
using System.Reflection;
using UnityEngine;
using KitchenLib.Event;
using KitchenLib.References;
using KitchenData;
using KitchenLib.Utils;
using Unity.Entities.UniversalDelegates;
using System.IO;
using Kitchen.Modules;

#if MELONLOADER
using MelonLoader;
#endif
#if BEPINEX
using BepInEx;
#endif
#if MELONLOADER
[assembly: MelonInfo(typeof(CreativeModeReborn.Main), CreativeModeReborn.Main.MOD_NAME, CreativeModeReborn.Main.MOD_VERSION, CreativeModeReborn.Main.MOD_AUTHOR)]
[assembly: MelonGame("It's Happening", "PlateUp")]
#endif
namespace CreativeModeReborn
{
#if BEPINEX
	[BepInProcess("PlateUp.exe")]
	[BepInPlugin(MOD_ID, MOD_NAME, MOD_VERSION)]
#endif
	public class Main : BaseMod
	{
		public const string MOD_ID = "creativemodereborn";
		public const string MOD_NAME = "Creative Mode Reborn";
		public const string MOD_AUTHOR = "StarFluxGames";
		public const string MOD_VERSION = "0.1.3";
		public const string MOD_COMPATIBLE_VERSIONS = "1.1.1";

		public static Rect windowRect = new Rect(10, 10, 180, 600);
		private static Vector2 scrollPosition;
		private static string searchText = string.Empty;
		private static string manualIDText = string.Empty;
		private static Dictionary<string, int> appliances = new Dictionary<string, int>();
		private static List<string> applianceNames = new List<string>();
		public static bool showMenu = true;

		public Main() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_COMPATIBLE_VERSIONS, Assembly.GetExecutingAssembly()) { }

		protected override void OnFrameUpdate()
		{
			if (Input.GetKeyDown(KeyCode.F1))
			{
				showMenu = !showMenu;
			}
		}

		protected override void OnInitialise()
		{
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<MonoGUI>();
			Events.BuildGameDataEvent += (s, args) =>
			{
				foreach (Appliance appliance in args.gamedata.Get<Appliance>())
				{
					if (appliance.Name != "")
					{
						int x = 2;
						if (!applianceNames.Contains(appliance.Name))
						{
							appliances.Add(appliance.Name, appliance.ID);
							applianceNames.Add(appliance.Name);
						}
						else
						{
							while (x != 0)
							{
								if (!applianceNames.Contains(appliance.Name + x))
								{
									appliances.Add(appliance.Name + x, appliance.ID);
									applianceNames.Add(appliance.Name + x);
									x = 0;
								}
								else
									x++;
							}
						}
					}
				}
			};
		}

		public static void DraggableWindow(int windowID)
		{
			GUILayout.Space(2);
			searchText = GUILayout.TextField(searchText);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			for (int i = 0; i < applianceNames.Count; i++)
			{
				if (string.IsNullOrEmpty(searchText) || applianceNames[i].ToLower().Contains(searchText.ToLower()))
				{
					if (GUILayout.Button(new GUIContent(applianceNames[i], $"Click button to spawn {applianceNames[i]}")))
					{
						SpawnUtils.SpawnApplianceBlueprintAtPlayer(appliances[applianceNames[i]]);
					}
				}
			}
			GUILayout.EndScrollView();
			GUILayout.Label("Manual ID");
			manualIDText = GUILayout.TextField(manualIDText);
			if (GUILayout.Button(new GUIContent("Spawn")))
			{
				try
				{
					SpawnUtils.SpawnApplianceBlueprintAtPlayer(int.Parse(manualIDText));
				}
				catch
				{
					Main.instance.Log("Invalid ID");
				}
			}

			GUILayout.Label("Press F1 to toggle this menu.");
			GUI.DragWindow();
		}
	}

	public class MonoGUI : MonoBehaviour
	{
		public void OnGUI()
		{
			if (Main.showMenu)
			{
				Main.windowRect = GUILayout.Window(0, Main.windowRect, Main.DraggableWindow, "Spawn Items", GUILayout.Width(180), GUILayout.Height(600f));
			}
		}
	}
}