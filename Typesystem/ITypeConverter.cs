using System;
using System.Collections.Generic;

namespace Ur.Typesystem {
    public interface ITypeConverter {
        object Convert(object source);
    }
    public interface ITypeConverter<in TSource, out TDestination> {
        TDestination Convert(TSource a);
    }
}
