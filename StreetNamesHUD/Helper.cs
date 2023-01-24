using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using System.Drawing;
using GTA.Native;

namespace StreetNamesHUD
{
    public sealed class Helper
    {
        public static Point TextPosition(int TextPosX, int TextPosY)
        {
            Point pos= new Point(Game.Resolution.Width - Game.Resolution.Width + 50, Game.Resolution.Height - 30);
            pos.Offset(TextPosX, TextPosY);
            return pos;
        }

        public static Vector3 GetCharPosition()
        {
            Vector3 pos = Game.LocalPlayer.Character.Position;
            return pos;
        }

        public static GTA.Font FontStyle()
        {
            GTA.Font font = new GTA.Font("Arial", 22f, FontScaling.Pixel, true, false);
            font.Effect = FontEffect.Shadow;
            font.EffectSize = 1;
            font.EffectColor = Color.Black;
            return font;
        }

        public static Color FontColor()
        {
            return Color.FromArgb(255, 255, 175, 122);
        }

        public static bool CheckHUDToggle()
        {
            return Function.Call<bool>("IS_HUD_PREFERENCE_SWITCHED_ON", Array.Empty<Parameter>());
        }
    }
}
