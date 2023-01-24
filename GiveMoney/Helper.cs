using GTA.Native;
using GTA;
using System;
using System.Drawing;

namespace Givemoney
{
    public sealed class Helper
    {
        public static void SetCarColor(Vehicle veh, int col1, int col2)
        {
            Function.Call("CHANGE_CAR_COLOUR", new Parameter[]
            {
                veh,
                col1,
                col2
            });
        }

        public static Point GetCarColor(Vehicle veh)
        {
            Pointer pointer = new Pointer(typeof(int));
            Pointer pointer2 = new Pointer(typeof(int));
            Function.Call("GET_CAR_COLOURS", new Parameter[]
            {
                veh,
                pointer,
                pointer2
            });
            return new Point(pointer, pointer2);
        }

        public static void SetExtraCarColor(Vehicle veh, int col1, int col2)
        {
            Function.Call("SET_EXTRA_CAR_COLOURS", new Parameter[]
            {
                veh,
                col1,
                col2
            });
        }

        public static Point GetExtraCarColor(Vehicle veh)
        {
            Pointer pointer = new Pointer(typeof(int));
            Pointer pointer2 = new Pointer(typeof(int));
            Function.Call("GET_EXTRA_CAR_COLOURS", new Parameter[]
            {
                veh,
                pointer,
                pointer2
            });
            return new Point(pointer, pointer2);
        }

        public static int RandomColor()
        {
            int min = 0;
            int max = 136;
            Random random = new Random();
            int result = random.Next(min, max);
            return result;
        }

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
    }
}
