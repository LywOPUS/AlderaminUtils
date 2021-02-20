using uRandom = UnityEngine.Random;

namespace AlderaminUtils
{
    public static class SimpleShuffleLib
    {
        /// <summary>
        ///  随机一个数组中的元素
        /// </summary>
        /// <param name="array"></param>
        /// <param name="length"></param>
        public static void RandomArray<T>(T[] array, int length)
        {
            for (int i = length - 1; i > 0; i--)
            {
                var index = uRandom.Range(0, i + 1);
                (array[i], array[index]) = (array[index], array[i]);
                // Tool.Swap(ref array[i],ref array[index]);
            }
        }

        public static void Random2DArray<T>(T[,] array)
        {
            var w = array.GetLength(0);
            var h = array.GetLength(0);
            int l = w * h;
            for (int i = l - 1; i >= 0; i--)
            {
                var iX = i / w;
                var iY = i % h;

                var r = uRandom.Range(0, i + 1);
                var riX = r / w;
                var riY = r % h;
                (array[iX, iY], array[riX, riY]) = (array[riX, riY], array[iX, iY]);
            }
        }
    }
}