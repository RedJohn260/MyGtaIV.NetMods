using System.Windows.Forms;
using GTA;

namespace ColorChanger
{
    public class ColorChanger : Script
    {
        public ColorChanger()
        {
            KeyDown += ColorChanger_KeyDown;
            Game.DisplayText("ColorChanger mod loaded");
        }

        private void ColorChanger_KeyDown(object sender, GTA.KeyEventArgs e)
        {
            if (e.Key == Keys.F10)
            {
                ChangeCarColor();
            }
        }

        private void ChangeCarColor()
        {
            if (Player.Character.isInVehicle())
            {
                Vehicle car = Player.Character.CurrentVehicle;
                Helper.SetCarColor(car, Helper.RandomColor(), 0);
                Helper.SetExtraCarColor(car, Helper.RandomColor(), Helper.RandomColor());
                Helper.ShowSubtitle("Vehicle color " + Helper.RandomColor().ToString() + " selected");
            }
            else
            {
                Helper.ShowSubtitle("Player not in vehicle");
            }
        }
    }
}
