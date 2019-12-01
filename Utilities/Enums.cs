using System;
using System.Collections.Generic;
using System.Linq;

namespace Ur {
    public static class Enums {

        public static IEnumerable<T> IterateValues<T>() {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static int MaxValue<T>() {
            return IterateValues<T>().Cast<int>().Max();
        }
    }
}
