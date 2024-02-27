using UnityEngine;

namespace GlassyCode
{
    public static class RandomUtils
    {
        public static bool GetRandomBool => Random.Range(0, 2) == 0;
    }
}