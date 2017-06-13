using System;
using HesaEngine.SDK;

namespace Xerath
{
    public class MyLoader : IScript
    {
        public string Name
        {
            get { return "Xerath"; }
        }

        public string Version
        {
            get { return "1.0.0"; }
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
                PrintChat("Successfully Loaded Version 0.0.1");
            }
        }

        internal static void PrintChat(string text) { Chat.Print(String.Format("<font color=\"#4169E1\"><b>[Xerath]:</b></font><font color=\"#FFFFFF\"> {0}</font>", text)); }
    }
}
