using System;
using System.Collections.Generic;
using System.Linq;

namespace Ur {
    public static class Enums {

        public static IEnumerable<T> IterateValues<T>() => Enum.GetValues(typeof(T)).Cast<T>();

        public static int MaxValue<T>() => IterateValues<T>().Cast<int>().Max();
    }
}
