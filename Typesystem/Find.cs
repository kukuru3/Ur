using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ur.Typesystem {

    public class Finder {

        #region Fields
        List<Assembly> assemblies; 
        #endregion

        #region Factories
        static public Finder FromAllAssemblies() {
            return new Finder(AppDomain.CurrentDomain.GetAssemblies());
        }

        static public Finder FromAssemblies(IEnumerable<Assembly> assemblies) {
            return new Finder(assemblies);
        } 

        static public Finder FromCurrentAssembly() {
            var a = Assembly.GetCallingAssembly();
            return new Finder(new[] { a });
        }

        #endregion

        private Finder(IEnumerable<Assembly> assemblies ) {
            this.assemblies = new List<Assembly>(assemblies);
        }

        public IEnumerable<Type> ImplementingTypes(Type tt) {
            var l = new List<Type>();
            foreach (var ass in this.assemblies) 
                foreach (var type in ass.GetTypes()) 
                    if (!type.IsAbstract)
                        if (tt.IsAssignableFrom(type))
                            l.Add(type);
            return l;
        }
        
        public IEnumerable<Type> ImplementingTypes<T>() where T : class {
            var tt = typeof(T);
            return ImplementingTypes(tt);
        }

        public (T attribute, Type type)[] GetTypesWithAttributes<T>() where T: Attribute {
            var l = new List<(T, Type)>();

            foreach (var ass in assemblies)
                foreach (var type in ass.GetTypes())
                    if (!type.IsAbstract) {
                        var attribs = type.GetCustomAttributes(true);
                        foreach (var attrib in attribs) if (attrib is T typedAttrib) l.Add((typedAttrib, type));
                    }
            return l.ToArray();
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

        public T GetAttribute<T>(object @object) where T : Attribute => @object.GetType().GetCustomAttribute<T>(true);

    }
}
