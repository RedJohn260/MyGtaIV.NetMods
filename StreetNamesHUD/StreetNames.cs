using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace StreetNamesHUD.Scripts
{
    public class StreetNames : Script
    {
        private string StreetName;
        private bool _IsHUD_ON;
        private Vector3 CharPos;
        private GTA.Font TextFont;
        public StreetNames()
        {
            Tick += StreetNames_Tick;
            PerFrameDrawing += new GTA.GraphicsEventHandler(StreetNames_PerFrameDrawing);
            TextFont = Helper.FontStyle();
        }

        private void StreetNames_PerFrameDrawing(System.Object sender, GraphicsEventArgs e)
        {
            if (_IsHUD_ON)
            {
                e.Graphics.Scaling = FontScaling.Pixel;
                e.Graphics.DrawText(StreetName, Helper.TextPosition(0, 0).X, Helper.TextPosition(0, 0).Y, Helper.FontColor(), TextFont);
            }
        }

        private void StreetNames_Tick(object sender, EventArgs e)
        {
            if (Helper.CheckHUDToggle())
            {
                _IsHUD_ON = true;
                CharPos = Game.LocalPlayer.Character.Position;
                StreetName = World.GetStreetName(CharPos);
            }
            else
            {
                _IsHUD_ON = false;  
            }
        }
    }
}
