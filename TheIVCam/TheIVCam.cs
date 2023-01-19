using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using System.Windows.Forms;
using System.IO;
using System.Timers;

namespace TheIVCam
{
    public class TheIVCam : Script
    {

        public TheIVCam()
        {
            Tick += StaticCamera_Tick;
            KeyDown += StaticCamera_KeyDown;
            PerFrameDrawing += TheIVCam_PerFrameDrawing;
            Helper.SpeedTextFont = Helper.FontStyle();
            BindConsoleCommand("ivcamreload", new ConsoleCommandDelegate(ConsoleSettingsReload), "- Reload TheIVCam mod settings.");
            LoadSettings();
            Game.DisplayText("TheIVCam Mod Loaded");
        }

        private void TheIVCam_PerFrameDrawing(object sender, GraphicsEventArgs e)
        {
            if (Player.Character.isInVehicle())
            {
                if (Helper._modEnabled)
                {
                    if (Helper._speedHUD)
                    {
                        e.Graphics.Scaling = FontScaling.Pixel;
                        if (Helper.MPH)
                        {
                            float speed = Helper.VehSpeed * 2.2f;

                            e.Graphics.DrawText("Speed: " + speed.ToString("00") + " MPH" , Helper.TextPosition(0, 0).X, Helper.TextPosition(0, 0).Y, Helper.FontColor(), Helper.SpeedTextFont);
                            Helper.KPH = false;
                        }
                        if (Helper.KPH)
                        {
                            float speed = Helper.ConvertMPHToKPH(Helper.VehSpeed);
                            e.Graphics.DrawText("Speed: " + speed.ToString("00") + " KM/H", Helper.TextPosition(0, 0).X, Helper.TextPosition(0, 0).Y, Helper.FontColor(), Helper.SpeedTextFont);
                            Helper.MPH = false;
                        }
                    }
                }
            }
        }

        private void ConsoleSettingsReload(ParameterCollection Parameter)
        {
            LoadSettings();
        }

        private void StaticCamera_Tick(object sender, EventArgs e)
        {
            if (Player.Character.isInVehicle())
            {
                if (Helper._modEnabled)
                {
                    Helper.VehSpeed = Player.Character.CurrentVehicle.Speed;
                    if (Helper.DynamicCam)
                    {
                        if (Helper.VehSpeed > 20f && Helper.camX > -7f && Helper.camY > -7f)
                        {
                            Helper.camX = Helper.camX - 0.01f;
                            Helper.camY = Helper.camY - 0.01f;
                        }
                        else if (Helper.VehSpeed > 0f && Helper.VehSpeed < 70f && Helper.camX < -5f && Helper.camY < -5f)
                        {
                            Helper.camX = Helper.camX + 0.02f;
                            Helper.camY = Helper.camY + 0.02f;
                        }
                    }
                    UpdateModCam();
                }
            }
            else
            {
                if (Helper.ModCamActive)
                {
                    DeleteModCam();
                }
            }
        }

        private void StaticCamera_KeyDown(object sender, GTA.KeyEventArgs e)
        {
            if (e.Key == Helper.ToggleKey)
            {
                if (Player.Character.isInVehicle())
                {
                    Helper._modEnabled = !Helper._modEnabled;
                    if (Helper._modEnabled)
                    {
                        SetupModCam();

                    }
                    else
                    {
                        DeleteModCam();
                    }
                }
                else
                {
                    Helper.ShowSubtitle("TheIVCam: You need to be in vehicle in order to activate the camera.");
                }
            }
        }

        private void UpdateModCam()
        {
            Vector3 direction;
            Vector3 position;
            direction.X = Player.Character.CurrentVehicle.Direction.X + Helper.camDX;
            direction.Y = Player.Character.CurrentVehicle.Direction.Y + Helper.camDY;
            direction.Z = Player.Character.CurrentVehicle.Direction.Z + Helper.camDZ;
            position.X = Player.Character.CurrentVehicle.Position.X + Helper.camX * Player.Character.CurrentVehicle.Direction.X;
            position.Y = Player.Character.CurrentVehicle.Position.Y + Helper.camY * Player.Character.CurrentVehicle.Direction.Y;
            position.Z = Player.Character.CurrentVehicle.Position.Z + Helper.camZ;
            Helper.ModCamera.Direction = direction;
            Helper.ModCamera.Position = position;
        }

        private void SetupModCam()
        {
            try
            {
                Helper.ModCamera = new Camera();
                Helper.ModCamera.Position = Player.Character.Position;
                Helper.ModCamera.Direction = Player.Character.Direction;
                Helper.ModCamera.Activate();
                Helper.ModCamActive = true;
                Helper._modEnabled = true;
                Helper.ShowSubtitle("TheIVCam: Enabled");
            }
            catch (Exception ex)
            {
                Game.DisplayText(ex.Message);
            }
        }

        private void DeleteModCam()
        {
            if (Helper.ModCamera.isActive)
            { 
                Helper.ModCamera.Deactivate();
            }
            Helper.ModCamActive = false;
            Helper._modEnabled =false;
            Helper.ShowSubtitle("TheIVCam: Disabled");
        }

        private void LoadSettings()
        {
            if (File.Exists(Settings.Filename))
            {
                Settings.Load();
                Helper.ToggleKey = Settings.GetValueKey("ToggleMod", "HOTKEYS", Keys.F7);
                Helper.camX = Settings.GetValueFloat("CamLeftRight", "SETTINGS", -5f);
                Helper.camY = Settings.GetValueFloat("CamForwardBack", "SETTINGS", -5f);
                Helper.camZ = Settings.GetValueFloat("CamUpDown", "SETTINGS", 1.5f);
                Helper._speedHUD = Settings.GetValueBool("SpeedHUD", "SETTINGS", true);
                Helper.MPH = Settings.GetValueBool("MPH", "SETTINGS", false);
                Helper.KPH = Settings.GetValueBool("KMH", "SETTINGS", true);
                Helper.DynamicCam =  Settings.GetValueBool("DynamicCam", "SETTINGS", true);
                Helper.ShowSubtitle("TheIVCam: Settings Loaded");
            }
            else
            {
                SaveSettings();
            }
        }
        private void SaveSettings()
        {
            Settings.SetValue("ToggleMod", "HOTKEYS", Keys.F7);
            Settings.SetValue("CamLeftRight", "SETTINGS", -5f);
            Settings.SetValue("CamForwardBack", "SETTINGS", -5f);
            Settings.SetValue("CamUpDown", "SETTINGS", 1.5f);
            Settings.SetValue("SpeedHUD", "SETTINGS", true);
            Settings.SetValue("MPH", "SETTINGS", false);
            Settings.SetValue("KMH", "SETTINGS", true);
            Settings.SetValue("DynamicCam", "SETTINGS", true);
            Settings.Save();
            Helper.ShowSubtitle("TheIVCam: Settings Saved");
        }
    }
}
