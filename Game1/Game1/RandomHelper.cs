using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public static class RandomHelper
    {
        static Random random = new Random();

        public static int Next() => random.Next();

        public static int Next(int maxValue) => random.Next(maxValue);

        public static int Next(int minValue, int maxValue) => random.Next(minValue, maxValue + 1);
    }
}
