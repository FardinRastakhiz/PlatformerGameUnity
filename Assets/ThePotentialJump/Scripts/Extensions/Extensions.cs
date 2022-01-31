using UnityEngine;

namespace ThePotentialJump.Utilities
{
    public static class Extensions
    {
        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }
        public static float Norm2(this Color color)
        {
            return color.ToVector4().sqrMagnitude;
        }

        public static Color AddColor(this Color self, Color color)
        {
            self += color;
            if (self.r < 0) self.r = 0;
            if (self.g < 0) self.g = 0;
            if (self.b < 0) self.b = 0;
            if (self.a < 0) self.a = 0;
            if (self.r > 1) self.r = 1;
            if (self.g > 1) self.g = 1;
            if (self.b > 1) self.b = 1;
            if (self.a > 1) self.a = 1;
            return self;
        }
    }
}