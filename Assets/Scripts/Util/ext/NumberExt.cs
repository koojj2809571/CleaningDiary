using UnityEngine;

namespace Util.ext
{
    public static class NumberExt
    {
        public static int MaxLimit(this int i, int max) => i > max ? max : i;
        public static int MinLimit(this int i, int min) => i > min ? i : min;
        public static int PlusLimit(ref this int i, int add, int max)
        {
            i += add;
            return Mathf.Min(i, max);
        }
        
        public static float MaxLimit(this float i, float max) => i > max ? max : i;
        public static float MinLimit(this float i, float min) => i > min ? i : min;
        public static float PlusLimit(ref this float i, float add, float max)
        {
            i += add;
            return Mathf.Min(i, max);
        }
        
        public static double MaxLimit(this double i, double max) => i > max ? max : i;
        public static double MinLimit(this double i, double min) => i > min ? i : min;
        public static double PlusLimit(ref this double i, double add, double max)
        {
            i += add;
            return Mathf.Min((float)i, (float)max);
        }
    }
}