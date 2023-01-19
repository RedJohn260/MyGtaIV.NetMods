using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GTA;
using GTA.Native;

namespace PrivateVehicle.Scripts
{
    public class PrivateVehicle : Script
    {
        private Vehicle vehicle;
        private Blip vehicle_blip;
        private bool vehicle_spawned = false;
        public PrivateVehicle()
        {
            Tick += PrivateVehicle_Tick;
            KeyDown += PrivateVehicle_KeyDown;
            GeneralInfo = "Private Vehicle Mod gives you ability to spawn your desired car with default: F3 key, remove car with default: F4 key and repair the car with default: F5 key. It also includes vehicle icon that you can find it easily.";
            Game.DisplayText("[PVM]: Private Vehicle Mod Loaded!");
            //vehicle.FreezePosition = false;
        }

        private void PrivateVehicle_Tick(object sender, EventArgs e)
        {
            if (vehicle_spawned)
            {
                if (vehicle.Exists() && vehicle != null)
                {
                    if (Game.LocalPlayer.Character.isInVehicle(vehicle))
                    {
                        vehicle_blip.Display = BlipDisplay.Hidden;
                    }
                    else
                    {
                        vehicle_blip.Display = BlipDisplay.MapOnly;
                    }
                }
            }
        }

        private void PrivateVehicle_KeyDown(object sender, GTA.KeyEventArgs e)
        {
            if (e.Key == Settings.GetValueKey("SpawnVehicle", "HOTKEYS", Keys.F3)) // If Key 'F3' is pressed, open or close the menu.
            {
                SpawnVehicle();
            }

            if (e.Key == Settings.GetValueKey("RemoveVehicle", "HOTKEYS", Keys.F4))
            {
               DeleteVehicle();
            }

            if (e.Key == Settings.GetValueKey("RepairVehicle", "HOTKEYS", Keys.F5))
            {
                RepairVehicle();
            }
        }

        private void SpawnVehicle()
        {
            if (!vehicle_spawned)
            {
                Vector3 position = Game.LocalPlayer.Character.Position.Around(2.0f);
                Vector3 position2 = World.GetNextPositionOnPavement(position);
                if (position2.DistanceTo(position) >= 0.5f)
                {
                    position2 = World.GetNextPositionOnStreet(position);
                }
                vehicle = World.CreateVehicle(Settings.GetValueModel("VehicleModel", "SETTINGS"), position2);
                Helper.SetCarColor(vehicle, Settings.GetValueInteger("PrimaryColor", "SETTINGS", 0), Settings.GetValueInteger("SecondaryColor", "SETTINGS", 0));
                Helper.SetExtraCarColor(vehicle, Settings.GetValueInteger("ExtraColor1", "SETTINGS", 0), Settings.GetValueInteger("ExtraColor2", "SETTINGS", 0));
                vehicle.FreezePosition = false;
                vehicle.CanBeDamaged= false;
                vehicle.CanBeVisiblyDamaged= false;
                vehicle.EngineRunning= true;
                vehicle.Visible= true;
                vehicle.SoundHorn(5);
                vehicle.CanTiresBurst = false;
                vehicle.MakeProofTo(true, true, true, true, true);
                vehicle.Dirtyness = 0f;
                vehicle.EngineRunning = false;
                vehicle.isRequiredForMission = true;
                vehicle_blip = vehicle.AttachBlip();
                vehicle_blip.Icon = BlipIcon.Building_Garage;
                vehicle_blip.Name = "Player Vehicle";
                vehicle_blip.Display = BlipDisplay.MapOnly;
                vehicle_blip.SetColorRGB(System.Drawing.Color.White);

                if (vehicle.isAlive)
                {
                    vehicle.Position = position2;
                    vehicle.Repair();
                    vehicle.DoorLock = DoorLock.None;
                }

                vehicle_spawned = true;
                SaveSettings();
                Helper.ShowSubtitle("Your private vehicle spawned");
            }
        }

        private void DeleteVehicle()
        {
            if (vehicle_spawned)
            {
                if (vehicle.Exists())
                {
                    vehicle.Delete();
                    vehicle.NoLongerNeeded();
                    vehicle_spawned = false;
                }
                if (vehicle_blip.Exists())
                {
                    vehicle_blip.Delete();
                }
                Helper.ShowSubtitle("Your private vehicle removed");
            }
        }

        private void RepairVehicle()
        {
            if (vehicle_spawned)
            {
                vehicle.Repair();
                vehicle.Wash();
                Helper.ShowSubtitle("Your private vehicle repaired");
            }
        }

        private void SaveSettings()
        {
            if (!File.Exists(Settings.Filename))
            {
                Settings.SetValue("VehicleModel", "SETTINGS", Helper.VehicleName(vehicle));
                Settings.SetValue("PrimaryColor", "SETTINGS", Helper.GetCarColor(vehicle).X);
                Settings.SetValue("SecondaryColor", "SETTINGS", Helper.GetCarColor(vehicle).Y);
                Settings.SetValue("ExtraColor1", "SETTINGS", Helper.GetExtraCarColor(vehicle).X);
                Settings.SetValue("ExtraColor2", "SETTINGS", Helper.GetExtraCarColor(vehicle).Y);
                Settings.SetValue("SpawnVehicle", "HOTKEYS", Keys.F3);
                Settings.SetValue("RemoveVehicle", "HOTKEYS", Keys.F4);
                Settings.SetValue("RepairVehicle", "HOTKEYS", Keys.F5);
                Settings.SetValue("VehiclePosition", "COORDINATES", vehicle.Position);
                Settings.SetValue("VehicleRotation", "COORDINATES", vehicle.Rotation);
                Settings.SetValue("VehicleHeading", "COORDINATES", vehicle.Heading);
                Settings.Save();
                Game.DisplayText("[PVM]: Your private vehicle settings saved");
            }
        }
    }
}
