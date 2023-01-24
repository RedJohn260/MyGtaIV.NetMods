using System;
using GTA;
using System.Windows.Forms;
using System.IO;

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
                    if (Helper._speedHUD && Player.CanControlCharacter)
                    {
                        e.Graphics.Scaling = FontScaling.Pixel;
                        if (Helper.MPH)
                        {
                            float speed = Helper.VehSpeed;

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
            if (Player.Character.isInVehicle() && Player.CanControlCharacter)
            {
                if (Helper._modEnabled)
                {
                    Helper.VehSpeed = Player.Character.CurrentVehicle.Speed;
                    if (Helper.DynamicCam)
                    {
                        if (Helper.VehSpeed > Helper.vehicleSpeed)
                        {
                            Helper.cameraZoom -= Helper.zoomSpeed;
                        }
                        else if (Helper.VehSpeed < Helper.vehicleSpeed)
                        {
                            Helper.cameraZoom += Helper.zoomSpeed;
                        }
                        Helper.cameraZoom = Helper.cameraZoom < Helper.camMinZoom ? Helper.camMinZoom : (Helper.cameraZoom > Helper.camMaxZoom ? Helper.camMaxZoom : Helper.cameraZoom);
                        Helper.camX = Helper.cameraZoom;
                        Helper.camY = Helper.cameraZoom;
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
            if (e.Key == Helper.MoveFoward)
            {
                if (Helper._modEnabled && !Helper.DynamicCam)
                {
                    Helper.camX += 0.1f;
                    Helper.camY += 0.1f;
                    Helper.camMaxZoom = Helper.camX;
                    Helper.camMinZoom = Helper.camX - 2f;
                    Helper.ShowSubtitle("Camera Forward: " + Helper.camX.ToString());
                }
            }
            if (e.Key == Helper.MoveBackward)
            {
                if (Helper._modEnabled && !Helper.DynamicCam)
                {
                    Helper.camX -= 0.1f;
                    Helper.camY -= 0.1f;
                    Helper.camMaxZoom = Helper.camX;
                    Helper.camMinZoom = Helper.camX - 2f;
                    Helper.ShowSubtitle("Camera Backward: " + Helper.camX.ToString());
                }
            }
            if (e.Key == Helper.MoveUp)
            {
                if (Helper._modEnabled && !Helper.DynamicCam)
                {
                    Helper.camZ += 0.1f;
                    Helper.ShowSubtitle("Camera Up: " + Helper.camZ.ToString());
                }
            }
            if (e.Key == Helper.MoveDown)
            {
                if (Helper._modEnabled && !Helper.DynamicCam)
                {
                    Helper.camZ -= 0.1f;
                    Helper.ShowSubtitle("Camera Down: " + Helper.camZ.ToString());
                }
            }
            if (e.Key == Helper.SaveSettings)
            {
                if (Helper._modEnabled)
                {
                    SaveSettings();
                }
                else
                {
                    Helper.ShowSubtitle("TheIVCam: You need to have mod camera active in order to save the settings.");
                }
            }
            if (e.Key == Helper.ToggleDynamicCam)
            {
                if (Helper._modEnabled)
                {
                    Helper.DynamicCam = !Helper.DynamicCam;
                    if (Helper.DynamicCam)
                    {
                        Helper.ShowSubtitle("TheIVCam: Dynamic Camera enabled");
                    }
                    else
                    {
                        Helper.ShowSubtitle("TheIVCam: Dynamic Camera disabled");
                    }
                }
                else
                {
                    Helper.ShowSubtitle("TheIVCam: You need to have mod camera active in order to activate dynamic camera.");
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
            SaveSettings();
            Helper.ShowSubtitle("TheIVCam: Disabled");
        }

        private void LoadSettings()
        {
            if (File.Exists(Settings.Filename))
            {
                Settings.Load();
                Helper.ToggleKey = Settings.GetValueKey("ToggleMod", "HOTKEYS", Keys.F7);
                Helper.ToggleDynamicCam = Settings.GetValueKey("ToggleDynamicCam", "HOTKEYS", Keys.NumPad0);
                Helper.SaveSettings = Settings.GetValueKey("SaveSettings", "HOTKEYS", Keys.NumPad5);
                Helper.MoveFoward =  Settings.GetValueKey("MoveForward", "HOTKEYS", Keys.NumPad9);
                Helper.MoveBackward = Settings.GetValueKey("MoveBackwards", "HOTKEYS", Keys.NumPad7);
                Helper.MoveUp = Settings.GetValueKey("MoveUp", "HOTKEYS", Keys.NumPad8);
                Helper.MoveDown = Settings.GetValueKey("MoveDown", "HOTKEYS", Keys.NumPad2);
                Helper._speedHUD = Settings.GetValueBool("SpeedHUD", "SETTINGS", Helper._speedHUD);
                Helper.MPH = Settings.GetValueBool("MPH", "SETTINGS", Helper.MPH);
                Helper.KPH = Settings.GetValueBool("KMH", "SETTINGS", Helper.KPH);
                Helper.DynamicCam =  Settings.GetValueBool("DynamicCam", "SETTINGS", Helper.DynamicCam);
                Helper.camX = Settings.GetValueFloat("PosForwardBack", "POSITION", Helper.camX);
                Helper.camY = Settings.GetValueFloat("PosForwardBack", "POSITION", Helper.camY);
                Helper.camZ = Settings.GetValueFloat("PosUpDown", "POSITION", Helper.camZ);
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
            Settings.SetValue("ToggleDynamicCam", "HOTKEYS", Keys.NumPad0);
            Settings.SetValue("SaveSettings", "HOTKEYS", Keys.NumPad5);
            Settings.SetValue("MoveForward", "HOTKEYS", Keys.NumPad9);
            Settings.SetValue("MoveBackwards", "HOTKEYS", Keys.NumPad7);
            Settings.SetValue("MoveUp", "HOTKEYS", Keys.NumPad8);
            Settings.SetValue("MoveDown", "HOTKEYS", Keys.NumPad2);
            Settings.SetValue("SpeedHUD", "SETTINGS", Helper._speedHUD);
            Settings.SetValue("MPH", "SETTINGS", Helper.MPH);
            Settings.SetValue("KMH", "SETTINGS", Helper.KPH);
            Settings.SetValue("DynamicCam", "SETTINGS", Helper.DynamicCam);
            Settings.SetValue("PosForwardBack", "POSITION", Helper.camX);
            Settings.SetValue("PosUpDown", "POSITION", Helper.camZ);
            Settings.Save();
            Helper.ShowSubtitle("TheIVCam: Settings Saved");
        }
    }
}
