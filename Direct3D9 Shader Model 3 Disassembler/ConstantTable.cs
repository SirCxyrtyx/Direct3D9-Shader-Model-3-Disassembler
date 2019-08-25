using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct3D9_Shader_Model_3_Disassembler
{
    public class ShaderSignature
    {
        public ConstantInfo[] Constants;
    }

    public class ConstantInfo
    {
        public readonly string Name;
        public readonly D3DXREGISTER_SET RegisterSet;
        public readonly int RegisterIndex;
        public readonly int RegisterCount;
        public readonly uint DefaultValue;
        public readonly D3DXPARAMETER_CLASS ParameterClass;
        public readonly D3DXPARAMETER_TYPE ParameterType;
        public readonly int Rows;
        public readonly int Columns;
        public readonly int Elements;
        public readonly StructMember[] StructMembers;

        public ConstantInfo(string name, D3DXREGISTER_SET registerSet, int registerIndex, int registerCount, uint defaultValue, D3DXPARAMETER_CLASS parameterClass,
                            D3DXPARAMETER_TYPE parameterType, int rows, int columns, int elements, StructMember[] structMembers)
        {
            Name = name;
            RegisterSet = registerSet;
            RegisterIndex = registerIndex;
            RegisterCount = registerCount;
            DefaultValue = defaultValue;
            ParameterClass = parameterClass;
            ParameterType = parameterType;
            Rows = rows;
            Columns = columns;
            Elements = elements;
            StructMembers = structMembers;
        }
    }

    public class StructMember
    {
        public string Name;
        public ConstantInfo TypeInfo;
    }
}
