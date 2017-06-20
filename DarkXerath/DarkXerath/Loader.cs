using System;
using HesaEngine.SDK;

namespace DarkXerath
{
    public class MyLoader : IScript
    {
        public string Name
        {
            get { return "DarkXerath"; }
        }

        public string Version
        {
            get { return ScriptVersion; }
        }

        public string Author
        {
            get { return "Ryzuki"; }
        }              

        public void OnInitialize()
        {
            Game.OnGameLoaded += OnGameLoaded;
        }

        internal static void OnGameLoaded()
        {
            if (ObjectManager.Me.ChampionName == "Xerath")
            {
                MyScript.LoadData();
                PrintChat("Successfully Loaded Version " + ScriptVersion);
            }
        }

        internal static string ScriptVersion
        {
            get { return "1.0.0"; }
        }

        internal static void PrintChat(string text) { Chat.Print(String.Format("<font color=\"#4169E1\"><b>[DarkXerath]:</b></font><font color=\"#FFFFFF\"> {0}</font>", text)); }
    }
}
