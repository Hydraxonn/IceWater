using System;
using GTA;
using GTA.UI;
using Ini;
namespace IceWater
{
    public class Main : Script
    {
        private readonly IniFile settingsIni;
        public static bool initialized, screenEffectRunning = false;
        public static bool enableCameraShake, enableVFX, enableWhileSnowOnGround = true;
        public static int damageFreqency = 2000;
        public static float damageMultiplier = 1f;
        public Main()
        {
            if (!initialized)
            {
                this.settingsIni = new IniFile("scripts/IceWater.ini");
                LoadExternalSettings(settingsIni);
                initialized = true;
            }
            Tick += HurtMe;
        }
        public static void LoadExternalSettings(IniFile settingsINI)
        {
            damageFreqency = settingsINI.Read<int>("damageFrequency","GENERAL",2000);
            damageMultiplier = settingsINI.Read<float>("damageMultiplier", "GENERAL", 1);
            enableCameraShake = settingsINI.Read("enableCameraShake", "FEATURES", true);
            enableVFX = settingsINI.Read("enableVFX", "FEATURES", true);
            enableWhileSnowOnGround = settingsINI.Read("enableWhileSnowOnGround", "FEATURES", true);
        }
        void ScreenIceEffect(float shakeStrength)
        {
            if (!screenEffectRunning && enableVFX)
            {
                Screen.StartEffect(ScreenEffect.DontTazemeBro, 2, false);
                screenEffectRunning = true;
            }
            if (enableCameraShake)
            {
                GTA.Native.Function.Call(GTA.Native.Hash.SHAKE_GAMEPLAY_CAM, "VIBRATE_SHAKE", shakeStrength);
            }
        }
        private void HurtMe(object sender, EventArgs e)
        {
            Wait(damageFreqency);//Only damage every x amount of ms
            if (Game.Player.Character != null && Game.Player.Character.IsAlive)//ensure we have a valid player to apply this to
            {
                if (Game.Player.Character.IsSwimmingUnderWater)//player is swimming underwater
                {
                    if (World.Weather == Weather.Blizzard || World.Weather == Weather.Christmas || World.Weather == Weather.Snowing || World.Weather == Weather.Snowlight)//currently a snowy weather
                    {
                        switch (World.Weather)//damage player based on which weather
                        {
                            case Weather.Blizzard:
                                Game.Player.Character.Health = (int)(Game.Player.Character.Health - (20 * damageMultiplier));
                                ScreenIceEffect(1.0f);
                                break;
                            case Weather.Snowing:
                                Game.Player.Character.Health = (int)(Game.Player.Character.Health - (16 * damageMultiplier));
                                ScreenIceEffect(1.0f);
                                break;
                            case Weather.Snowlight:
                                Game.Player.Character.Health = (int)(Game.Player.Character.Health - (12 * damageMultiplier));
                                ScreenIceEffect(1.0f);
                                break;
                            case Weather.Christmas:
                                Game.Player.Character.Health = (int)(Game.Player.Character.Health - (16 * damageMultiplier));
                                ScreenIceEffect(1.0f);
                                break;
                        }
                    }
                    else if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.HAS_NAMED_PTFX_ASSET_LOADED, "core_snow") && enableWhileSnowOnGround) // Not in snowy weather, but snow is on ground. 
                    {
                        Game.Player.Character.Health = (int)(Game.Player.Character.Health - (12 * damageMultiplier));//damage player for swimming while snow on ground
                        ScreenIceEffect(1.0f);
                    }
                }
                else if (Game.Player.Character.IsSwimming)//player is actively swimming
                    {
                        if (World.Weather == Weather.Blizzard || World.Weather == Weather.Christmas || World.Weather == Weather.Snowing || World.Weather == Weather.Snowlight)//currently a snowy weather
                        {
                            switch (World.Weather)//damage player based on which weather
                            {
                                case Weather.Blizzard:
                                    Game.Player.Character.Health = (int)(Game.Player.Character.Health - (10 * damageMultiplier));
                                    ScreenIceEffect(0.8f);
                                    break;
                                case Weather.Snowing:
                                    Game.Player.Character.Health = (int)(Game.Player.Character.Health - (8 * damageMultiplier));
                                    ScreenIceEffect(0.8f);
                                    break;
                                case Weather.Snowlight:
                                    Game.Player.Character.Health = (int)(Game.Player.Character.Health - (6 * damageMultiplier));
                                    ScreenIceEffect(0.8f);
                                    break;
                                case Weather.Christmas:
                                    Game.Player.Character.Health = (int)(Game.Player.Character.Health - (8 * damageMultiplier));
                                    ScreenIceEffect(0.8f);
                                    break;
                            }
                        }
                        else if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.HAS_NAMED_PTFX_ASSET_LOADED, "core_snow") && enableWhileSnowOnGround) // Not in snowy weather, but snow is on ground. 
                        {
                        Game.Player.Character.Health = (int)(Game.Player.Character.Health - (6 * damageMultiplier));//damage player for swimming while snow on ground
                        ScreenIceEffect(0.8f);
                        }
                }
                else if (Game.Player.Character.IsInWater) //player is not swimming but is standing in water
                {
                    if (World.Weather == Weather.Blizzard || World.Weather == Weather.Christmas || World.Weather == Weather.Snowing || World.Weather == Weather.Snowlight)//currently a snowy weather
                    {
                        switch (World.Weather)//damage player based on which weather
                        {
                            case Weather.Blizzard:
                                Game.Player.Character.Health = (int)(Game.Player.Character.Health - (5 * damageMultiplier));
                                ScreenIceEffect(0.5f);
                                break;
                            case Weather.Snowing:
                                Game.Player.Character.Health = (int)(Game.Player.Character.Health - (4 * damageMultiplier));
                                ScreenIceEffect(0.5f);
                                break;
                            case Weather.Snowlight:
                                Game.Player.Character.Health = (int)(Game.Player.Character.Health - (3 * damageMultiplier));
                                ScreenIceEffect(0.5f);
                                break;
                            case Weather.Christmas:
                                Game.Player.Character.Health = (int)(Game.Player.Character.Health - (4 * damageMultiplier));
                                ScreenIceEffect(0.5f);
                                break;
                        }
                    }
                    else if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.HAS_NAMED_PTFX_ASSET_LOADED, "core_snow") && enableWhileSnowOnGround) // Not in snowy weather, but snow is on ground.
                    {
                        Game.Player.Character.Health = (int)(Game.Player.Character.Health - (3 * damageMultiplier));//damage player for being in water while snow on ground
                        ScreenIceEffect(0.5f);
                    }
                }
                else//player is not swimming or in water
                {
                    if (screenEffectRunning)
                    {
                        Screen.StopEffect(ScreenEffect.DontTazemeBro);
                        GTA.Native.Function.Call(GTA.Native.Hash.STOP_GAMEPLAY_CAM_SHAKING);
                        screenEffectRunning = false;
                    }
                }
            }
        }
    }
} 