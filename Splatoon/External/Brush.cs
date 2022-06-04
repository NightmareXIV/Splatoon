namespace Splatoon.External
{
    [Serializable]
    public struct Brush
    {
        public float Thickness;
        public Vector4 Color;
        public Vector4 Fill;

        public bool HasFill()
        {
            return Fill.W != 0;
        }
    }
}