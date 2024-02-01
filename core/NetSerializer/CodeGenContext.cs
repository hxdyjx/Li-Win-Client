using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ClientWindows.Core.NetSerializer
{
    public sealed class TypeData
    {
        public readonly ushort TypeID;
        
        public readonly IDynamicTypeSerializer TypeSerializer;
        
        public bool IsGenerated { get { return this.TypeSerializer != null; } }   
        
        public MethodInfo WriterMethodInfo;

        public MethodInfo ReaderMethodInfo;

        public bool NeedsInstanceParameter { get; private set; }

        public TypeData(ushort typeID,IDynamicTypeSerializer serializer) 
        {
            this.TypeID = typeID;
            this.TypeSerializer = serializer;
            this.NeedsInstanceParameter = true;

        }

        public TypeData(ushort typeID, MethodInfo writer, MethodInfo reader)
        {
            this.TypeID = typeID;
            this.WriterMethodInfo = writer;
            this.ReaderMethodInfo = reader;

            this.NeedsInstanceParameter |= writer.GetParameters().Length == 3;
        }                        
    }
    public sealed class CodeGenContext
    {
        readonly Dictionary<Type, TypeData> m_typeMap;
        public MethodInfo SerializerSwitchMethodInfo { get; private set; }

        public MethodInfo GetWriterMethodInfo(Type type)
        {
            return m_typeMap[type].WriterMethodInfo;
        }

        public MethodInfo DeserializerSwitchMethodInfo { get; private set; }

        public MethodInfo GetReaderMethodInfo(Type type)
        {
            return m_typeMap[type].ReaderMethodInfo;
        }

        public bool IsGenerated(Type type)
        {
            return m_typeMap[type].IsGenerated;
        }


        public CodeGenContext(Dictionary<Type, TypeData> typeMap) 
        {                
            m_typeMap = typeMap;

            var td = m_typeMap[typeof(object)];
            this.SerializerSwitchMethodInfo = td.WriterMethodInfo;
            this.DeserializerSwitchMethodInfo = td.ReaderMethodInfo;
        }

        public IDictionary<Type,TypeData> TypeMap { get { return m_typeMap; } }
        
        bool CanCallDirect(Type type)
        {
            bool direct;
            if (type.IsValueType || type.IsArray)
                direct = false;
            else if (type.IsSealed && IsGenerated(type) == false)
                direct = true;
            else
                direct = false;
            return direct;
        }
       
    }
}
