using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Ur.Typesystem {
    static public class TypeUtility {
        public static int GetManagedSize(Type type) {
            // all this just to invoke one opcode with no arguments!
            var method = new DynamicMethod("GetManagedSizeImpl", typeof(uint), new Type[0], typeof(TypeExtensions), false);

            ILGenerator gen = method.GetILGenerator();

            gen.Emit(OpCodes.Sizeof, type);
            gen.Emit(OpCodes.Ret);

            var func = (Func<uint>)method.CreateDelegate(typeof(Func<uint>));
            return checked((int)func());
        }

        public static int GetManagedSize<T>() => GetManagedSize(typeof(T));
    }
}
