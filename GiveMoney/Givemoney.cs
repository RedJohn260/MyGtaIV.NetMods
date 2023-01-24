using System.Windows.Forms;
using GTA;

namespace Givemoney
{
    public class Givemoney : Script
    {
        public Givemoney()
        {
            KeyDown += Givemoney_KeyDown;
            Game.DisplayText("Givemoney mod loaded");
        }

        private void Givemoney_KeyDown(object sender, GTA.KeyEventArgs e)
        {
            if (e.Key == Keys.F11)
            {
                GiveMoney();
            }
        }

        private void GiveMoney()
        {
            if (Player.isPlaying)
            {
                int money = Player.Money;
                int amount = 1000;
                money = money + amount;
                Player.Money= money;
                Helper.ShowSubtitle("You got $ " + amount.ToString());
            }
            else
            {
                Helper.ShowSubtitle("Player not playing");
            }
        }
    }
}
