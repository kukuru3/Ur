using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ur.Typesystem {

    public class Finder {

        List<Assembly> assemblies;
        
        public Finder(IEnumerable<Assembly> assemblies = null) {
            if (assemblies == null ) assemblies = AppDomain.CurrentDomain.GetAssemblies();
            this.assemblies = new List<Assembly>(assemblies);            
        }
        
        public IEnumerable<Type> ImplementingTypes<T>() where T : class {

            var l = new List<Type>();
            var tt = typeof(T);
            foreach (var ass in this.assemblies) 
                foreach (var type in ass.GetTypes()) 
                    if (!type.IsAbstract) if (tt.IsAssignableFrom(type))  l.Add(type);                        
            return l;
            
        }

        public Dictionary<T, Type> GetTypesWithAttribute<T>() where T : Attribute {
            var lookup = new Dictionary<T, Type>();
            foreach (var ass in assemblies)
                foreach (var type in ass.GetTypes()) 
                    if (!type.IsAbstract) {
                        var attribs = type.GetCustomAttributes(true);
                        foreach (var attrib in attribs) if (attrib is T) lookup[(T)attrib] = type;
                    }
            return lookup;
        }

    }
}
