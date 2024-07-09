using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HocTiengAnh.Helpers
{
    public static class RandomHelper
    {
        private static Random random = new Random();

        public static List<T> GetRandomObject<T>(List<T> list, int numberOfObjects)
        {
            if (list.Count < numberOfObjects)
            {
                throw new ArgumentException("The list contains fewer elements than the requested number of objects.");
            }
            List<T> result = new List<T>();
            List<T> copy = new List<T>(list);

            for (int i = 0; i < numberOfObjects; i++)
            {
                int index = random.Next(copy.Count);
                result.Add(copy[index]);
                copy.RemoveAt(index);
            }
            return result;
        }

        public static void ShuffleList<T>(List<T> list)
        {
            Random random = new Random();
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                // Hoán đổi vị trí phần tử tại i và j
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
