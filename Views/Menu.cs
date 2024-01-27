using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Kitchen.NetworkSupport;
using KitchenLib.Utils;
using UnityEngine;

namespace CreativeModeReborn.Views
{
    public class Menu : MonoBehaviour
    {

        public Dictionary<string, int> Appliances = new Dictionary<string, int>();
        private Rect windowRect = new Rect(10, 10, 180, 600);
        private Vector2 scrollPosition;
        private string searchText = string.Empty;
        private string manualIDText = string.Empty;
        private string setPasswordText = string.Empty;
        private bool Display = true;

        private void Awake()
        {
            setPasswordText = SteamPlatform.Steam.Me.ID.ToString();
            SpawnAppliancesView.password = SteamPlatform.Steam.Me.ID.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                Display = !Display;
        }

        private void OnGUI()
        {
            if (Display)
                windowRect = GUILayout.Window(VariousUtils.GetID("Creative Spawn Menu"), windowRect, DraggableWindow, "Spawn Items", GUILayout.Width(250), GUILayout.Height(600));
        }
        
        private void DraggableWindow(int windowID)
        {
            GUILayout.Space(2);
            searchText = GUILayout.TextField(searchText);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            Mod.Logger.LogInfo(Appliances.Count);
            for (int i = 0; i < Appliances.Count; i++)
            {
                if (string.IsNullOrEmpty(searchText) || Appliances.Keys.ToArray()[i].ToLower().Contains(searchText.ToLower()))
                {
                    if (GUILayout.Button(new GUIContent(Appliances.Keys.ToArray()[i])))
                    {
                        SpawnAppliancesView.SpawnID = Appliances.Values.ToArray()[i];
                    }
                }
            }
            GUILayout.Space(2);
            GUILayout.EndScrollView();
            GUILayout.Label("Manual ID");
            manualIDText = GUILayout.TextField(manualIDText);
            if (GUILayout.Button(new GUIContent("Spawn")))
            {
                SpawnAppliancesView.SpawnID = int.Parse(manualIDText);
            }
            GUILayout.Space(2);
            GUILayout.Label("Set Password");
            setPasswordText = GUILayout.PasswordField(setPasswordText, '*');
            if (GUILayout.Button(new GUIContent("Set & Copy")))
            {
                SpawnAppliancesView.password = setPasswordText;
                Clipboard.SetText(setPasswordText);
            }

            GUILayout.Label("Press F1 to toggle this menu.");
            GUI.DragWindow();
        }
    }
}