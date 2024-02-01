using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ClientWindows.Core.NetSerializer
{
    public interface ITypeSerializer
    {
        bool Handler(Type type);
        IEnumerable<Type> GetSubTypes(Type type);
    }
    public interface IStaticTypeSerializer : ITypeSerializer {
        void GetStaticMethods(Type type, out MethodInfo wreiter, out MethodInfo reader);
    }
    public interface IDynamicTypeSerializer : ITypeSerializer
    {
        void GenerateWriterMethod(Type type,CodeGenContext );
    }
}
