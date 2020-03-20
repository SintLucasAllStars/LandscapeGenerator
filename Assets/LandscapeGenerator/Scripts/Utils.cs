public class Utils
{
    public static float Map(float value, float valueMin, float valueMax, float resultMin, float resultMax)
    {
        return resultMin + (value-valueMin)*(resultMax-resultMin)/(valueMax-valueMin);
    }
}