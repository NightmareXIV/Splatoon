namespace Splatoon.External
{
    internal static class Maths
    {
        internal const float PI = MathF.PI;
        internal const float TAU = PI * 2f;
        private const int CIRCLE_SEGMENTS = 180;

        // epsilon error value when comparing radian floats
        internal const float Epsilon = PI * 0.01f;

        static internal float Radians(float degrees)
        {
            return PI * degrees / 180.0f;
        }

        static internal bool BetweenAngles(float test, float start, float end)
        {
            // check if the angle between START and TEST is between 0 and the angle between START and END
            var toEnd = NormalizeRadians(end - start);
            var toTest = NormalizeRadians(test - start);

            return toTest > 0 && toTest < toEnd;
        }

        static private float NormalizeRadians(float radians)
        {
            return (radians + TAU) % TAU;
        }

        static internal float DistanceXZ(Vector3 a, Vector3 b)
        {
            var dx = b.X - a.X;
            var dz = b.Z - a.Z;
            return MathF.Sqrt(dx * dx + dz * dz);
        }

        static internal float AngleXZ(Vector3 a, Vector3 b)
        {
            return MathF.Atan2(b.X - a.X, b.Z - a.Z);
        }

        // how many segments to split an arc up into when rendering
        static internal int ArcSegments(float startRads, float endRads)
        {
            return (int)(MathF.Abs(endRads - startRads) * (CIRCLE_SEGMENTS / TAU)) + 1;
        }
    }
}