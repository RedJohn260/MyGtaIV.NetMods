using GTA;
using GTA.Native;
using System.Drawing;
using System.Windows.Forms;

namespace TheIVCam
{
    public sealed class Helper
    {
        //HOTKEYS
        public static Keys ToggleKey = Keys.F7;
        public static Keys ToggleDynamicCam = Keys.NumPad0;
        public static Keys SaveSettings = Keys.NumPad5;
        public static Keys MoveFoward = Keys.NumPad9;
        public static Keys MoveBackward = Keys.NumPad7;
        public static Keys MoveUp = Keys.NumPad8;
        public static Keys MoveDown = Keys.NumPad2;

        //MOD SETTINGS
        public static bool _modEnabled;
        public static float cameraZoom;
        public static float camMinZoom = -9f;
        public static float camMaxZoom = -6f;
        public static float zoomSpeed = 0.02f;
        public static float vehicleSpeed = 20f;
        public static float camX = -5f;
        public static float camY = -5f;
        public static float camZ = 1.5f;
        public static float camDX;
        public static float camDY;
        public static float camDZ;
        public static float VehSpeed;
        public static bool ModCamActive;
        public static Camera ModCamera;
        public static bool _speedHUD = true;
        public static bool MPH = false;
        public static bool KPH = true;
        public static GTA.Font SpeedTextFont;
        public static bool DynamicCam = true;

        public static void ShowSubtitle(string text, int duration = 4000)
        {
            Function.Call("PRINT_STRING_WITH_LITERAL_STRING_NOW", new Parameter[]
            {
                "STRING",
                text,
                duration,
                1
            });
        }
        public static Color FontColor()
        {
            return Color.FromArgb(255, 255, 175, 122);
        }
        public static GTA.Font FontStyle()
        {
            GTA.Font font = new GTA.Font("Arial", 22f, FontScaling.Pixel, true, false);
            font.Effect = FontEffect.Shadow;
            font.EffectSize = 1;
            font.EffectColor = Color.Black;
            return font;
        }
        public static Point TextPosition(int TextPosX, int TextPosY)
        {
            Point pos = new Point(Game.Resolution.Width -300, Game.Resolution.Height - 30);
            pos.Offset(TextPosX, TextPosY);
            return pos;
        }

        public static float ConvertMPHToKPH(float mph)
        {
            //return (float)(mph * 1.60934);
            return mph * 1.609344f * 2.2f;
        }
    }
}
