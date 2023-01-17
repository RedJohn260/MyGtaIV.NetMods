using System;
using System.Drawing;
using GTA;
using GTA.Native;

namespace PrivateVehicle
{
    public sealed class Helper
    {
        public static bool SaveSuccesfull()
        {
            return Function.Call<bool>("DID_SAVE_COMPLETE_SUCCESSFULLY", new Parameter[0]);
        }

        public static string VehicleName(Vehicle veh)
		{
			return Function.Call<string>("GET_STRING_FROM_TEXT_FILE", new Parameter[]
			{
				veh.Name
			});
		}

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
