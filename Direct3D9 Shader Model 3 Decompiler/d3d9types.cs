using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct3D9_Shader_Model_3_Decompiler
{
    //D3DSHADER_INSTRUCTION_OPCODE_TYPE
    public enum OpcodeType : uint
    {
        D3DSIO_NOP = 0,
        D3DSIO_MOV = 1,
        D3DSIO_ADD = 2,
        D3DSIO_SUB = 3,
        D3DSIO_MAD = 4,
        D3DSIO_MUL = 5,
        D3DSIO_RCP = 6,
        D3DSIO_RSQ = 7,
        D3DSIO_DP3 = 8,
        D3DSIO_DP4 = 9,
        D3DSIO_MIN = 10,
        D3DSIO_MAX = 11,
        D3DSIO_SLT = 12,
        D3DSIO_SGE = 13,
        D3DSIO_EXP = 14,
        D3DSIO_LOG = 15,
        D3DSIO_LIT = 16,
        D3DSIO_DST = 17,
        D3DSIO_LRP = 18,
        D3DSIO_FRC = 19,
        D3DSIO_M4x4 = 20,
        D3DSIO_M4x3 = 21,
        D3DSIO_M3x4 = 22,
        D3DSIO_M3x3 = 23,
        D3DSIO_M3x2 = 24,
        D3DSIO_CALL = 25,
        D3DSIO_CALLNZ = 26,
        D3DSIO_LOOP = 27,
        D3DSIO_RET = 28,
        D3DSIO_ENDLOOP = 29,
        D3DSIO_LABEL = 30,
        D3DSIO_DCL = 31,
        D3DSIO_POW = 32,
        D3DSIO_CRS = 33,
        D3DSIO_SGN = 34,
        D3DSIO_ABS = 35,
        D3DSIO_NRM = 36,
        D3DSIO_SINCOS = 37,
        D3DSIO_REP = 38,
        D3DSIO_ENDREP = 39,
        D3DSIO_IF = 40,
        D3DSIO_IFC = 41,
        D3DSIO_ELSE = 42,
        D3DSIO_ENDIF = 43,
        D3DSIO_BREAK = 44,
        D3DSIO_BREAKC = 45,
        D3DSIO_MOVA = 46,
        D3DSIO_DEFB = 47,
        D3DSIO_DEFI = 48,
        D3DSIO_TEXCOORD = 64,
        D3DSIO_TEXKILL = 65,
        D3DSIO_TEX = 66,
        D3DSIO_TEXBEM = 67,
        D3DSIO_TEXBEML = 68,
        D3DSIO_TEXREG2AR = 69,
        D3DSIO_TEXREG2GB = 70,
        D3DSIO_TEXM3x2PAD = 71,
        D3DSIO_TEXM3x2TEX = 72,
        D3DSIO_TEXM3x3PAD = 73,
        D3DSIO_TEXM3x3TEX = 74,
        D3DSIO_RESERVED0 = 75,
        D3DSIO_TEXM3x3SPEC = 76,
        D3DSIO_TEXM3x3VSPEC = 77,
        D3DSIO_EXPP = 78,
        D3DSIO_LOGP = 79,
        D3DSIO_CND = 80,
        D3DSIO_DEF = 81,
        D3DSIO_TEXREG2RGB = 82,
        D3DSIO_TEXDP3TEX = 83,
        D3DSIO_TEXM3x2DEPTH = 84,
        D3DSIO_TEXDP3 = 85,
        D3DSIO_TEXM3x3 = 86,
        D3DSIO_TEXDEPTH = 87,
        D3DSIO_CMP = 88,
        D3DSIO_BEM = 89,
        D3DSIO_DP2ADD = 90,
        D3DSIO_DSX = 91,
        D3DSIO_DSY = 92,
        D3DSIO_TEXLDD = 93,
        D3DSIO_SETP = 94,
        D3DSIO_TEXLDL = 95,
        D3DSIO_BREAKP = 96,
        D3DSIO_PHASE = 0xFFFD,
        D3DSIO_COMMENT = 0xFFFE,
        D3DSIO_END = 0xFFFF
    }

    //_D3DSHADER_PARAM_REGISTER_TYPE
    public enum RegisterType
    {
        D3DSPR_TEMP = 0,
        D3DSPR_INPUT = 1,
        D3DSPR_CONST = 2,
        D3DSPR_ADDR = 3,
        D3DSPR_TEXTURE = 3,
        D3DSPR_RASTOUT = 4,
        D3DSPR_ATTROUT = 5,
        D3DSPR_TEXCRDOUT = 6,
        D3DSPR_OUTPUT = 6,
        D3DSPR_CONSTINT = 7,
        D3DSPR_COLOROUT = 8,
        D3DSPR_DEPTHOUT = 9,
        D3DSPR_SAMPLER = 10,
        D3DSPR_CONST2 = 11,
        D3DSPR_CONST3 = 12,
        D3DSPR_CONST4 = 13,
        D3DSPR_CONSTBOOL = 14,
        D3DSPR_LOOP = 15,
        D3DSPR_TEMPFLOAT16 = 16,
        D3DSPR_MISCTYPE = 17,
        D3DSPR_LABEL = 18,
        D3DSPR_PREDICATE = 19
    }

    enum EREGISTER_SET
    {
        RS_BOOL,
        RS_INT4,
        RS_FLOAT4,
        RS_SAMPLER
    }

    public struct CTHeader
    {
        public uint Size;
        public uint Creator;
        public uint Version;
        public uint Constants;
        public uint ConstantInfo;
        public uint Flags;
        public uint Target;

        public CTHeader(Stream stream)
        {
            Size = stream.ReadUInt();
            Creator = stream.ReadUInt();
            Version = stream.ReadUInt();
            Constants = stream.ReadUInt();
            ConstantInfo = stream.ReadUInt();
            Flags = stream.ReadUInt();
            Target = stream.ReadUInt();
        }
    }

    public struct CTInfo
    {
        public uint Name;
        public ushort RegisterSet;
        public ushort RegisterIndex;
        public ushort RegisterCount;
        public ushort Reserved;
        public uint TypeInfo;
        public uint DefaultValue;

        public CTInfo(Stream stream)
        {
            Name = stream.ReadUInt();
            RegisterSet = stream.ReadUShort();
            RegisterIndex = stream.ReadUShort();
            RegisterCount = stream.ReadUShort();
            Reserved = stream.ReadUShort();
            TypeInfo = stream.ReadUInt();
            DefaultValue = stream.ReadUInt();
        }
    }

    struct CTType
    {
        ushort Class;
        ushort Type;
        ushort Rows;
        ushort Columns;
        ushort Elements;
        ushort StructMembers;
        uint StructMemberInfo;

        public CTType(Stream stream)
        {
            Class = stream.ReadUShort();
            Type = stream.ReadUShort();
            Rows = stream.ReadUShort();
            Columns = stream.ReadUShort();
            Elements = stream.ReadUShort();
            StructMembers = stream.ReadUShort();
            StructMemberInfo = stream.ReadUInt();
        }
    }

    public static class d3d9types
    {
        public static Dictionary<OpcodeType, string> opcodeNames = new Dictionary<OpcodeType, string>
        {
            {OpcodeType.D3DSIO_NOP, "nop"},
            {OpcodeType.D3DSIO_MOV, "mov"},
            {OpcodeType.D3DSIO_ADD, "add"},
            {OpcodeType.D3DSIO_SUB, "sub"},
            {OpcodeType.D3DSIO_MAD, "mad"},
            {OpcodeType.D3DSIO_MUL, "mul"},
            {OpcodeType.D3DSIO_RCP, "rcp"},
            {OpcodeType.D3DSIO_RSQ, "rsq"},
            {OpcodeType.D3DSIO_DP3, "dp3"},
            {OpcodeType.D3DSIO_DP4, "dp4"},
            {OpcodeType.D3DSIO_MIN, "min"},
            {OpcodeType.D3DSIO_MAX, "max"},
            {OpcodeType.D3DSIO_SLT, "slt"},
            {OpcodeType.D3DSIO_SGE, "sge"},
            {OpcodeType.D3DSIO_EXP, "exp"},
            {OpcodeType.D3DSIO_LOG, "log"},
            {OpcodeType.D3DSIO_LIT, "lit"},
            {OpcodeType.D3DSIO_DST, "dst"},
            {OpcodeType.D3DSIO_LRP, "lrp"},
            {OpcodeType.D3DSIO_FRC, "frc"},
            {OpcodeType.D3DSIO_M4x4, "m4x4"},
            {OpcodeType.D3DSIO_M4x3, "m4x3"},
            {OpcodeType.D3DSIO_M3x4, "m3x4"},
            {OpcodeType.D3DSIO_M3x3, "m3x3"},
            {OpcodeType.D3DSIO_M3x2, "m3x2"},
            {OpcodeType.D3DSIO_CALL, "call"},
            {OpcodeType.D3DSIO_CALLNZ, "callnz"},
            {OpcodeType.D3DSIO_LOOP, "loop"},
            {OpcodeType.D3DSIO_RET, "ret"},
            {OpcodeType.D3DSIO_ENDLOOP, "endloop"},
            {OpcodeType.D3DSIO_LABEL, "label"},
            {OpcodeType.D3DSIO_DCL, "dcl"},
            {OpcodeType.D3DSIO_POW, "pow"},
            {OpcodeType.D3DSIO_CRS, "crs"},
            {OpcodeType.D3DSIO_SGN, "sgn"},
            {OpcodeType.D3DSIO_ABS, "abs"},
            {OpcodeType.D3DSIO_NRM, "nrm"},
            {OpcodeType.D3DSIO_SINCOS, "sincos"},
            {OpcodeType.D3DSIO_REP, "rep"},
            {OpcodeType.D3DSIO_ENDREP, "endrep"},
            {OpcodeType.D3DSIO_IF, "if"},
            {OpcodeType.D3DSIO_IFC, "ifc"},
            {OpcodeType.D3DSIO_ELSE, "else"},
            {OpcodeType.D3DSIO_ENDIF, "endif"},
            {OpcodeType.D3DSIO_BREAK, "break"},
            {OpcodeType.D3DSIO_BREAKC, "breakc"},
            {OpcodeType.D3DSIO_MOVA, "mova"},
            {OpcodeType.D3DSIO_DEFB, "defb"},
            {OpcodeType.D3DSIO_DEFI, "defi"},
            {OpcodeType.D3DSIO_TEXCOORD, "texcoord"},
            {OpcodeType.D3DSIO_TEXKILL, "texkill"},
            {OpcodeType.D3DSIO_TEX, "tex"},
            {OpcodeType.D3DSIO_TEXBEM, "texbem"},
            {OpcodeType.D3DSIO_TEXBEML, "texbeml"},
            {OpcodeType.D3DSIO_TEXREG2AR, "texreg2ar"},
            {OpcodeType.D3DSIO_TEXREG2GB, "texreg2gb"},
            {OpcodeType.D3DSIO_TEXM3x2PAD, "texm3x2pad"},
            {OpcodeType.D3DSIO_TEXM3x2TEX, "texm3x2tex"},
            {OpcodeType.D3DSIO_TEXM3x3PAD, "texm3x3pad"},
            {OpcodeType.D3DSIO_TEXM3x3TEX, "texm3x3tex"},
            {OpcodeType.D3DSIO_RESERVED0, ""},
            {OpcodeType.D3DSIO_TEXM3x3SPEC, "texm3x3spec"},
            {OpcodeType.D3DSIO_TEXM3x3VSPEC, "texm3x3vspec"},
            {OpcodeType.D3DSIO_EXPP, "expp"},
            {OpcodeType.D3DSIO_LOGP, "logp"},
            {OpcodeType.D3DSIO_CND, "cnd"},
            {OpcodeType.D3DSIO_DEF, "def"},
            {OpcodeType.D3DSIO_TEXREG2RGB, "texreg2rgb"},
            {OpcodeType.D3DSIO_TEXDP3TEX, "texdp3tex"},
            {OpcodeType.D3DSIO_TEXM3x2DEPTH, "texm3x2depth"},
            {OpcodeType.D3DSIO_TEXDP3, "texdp3"},
            {OpcodeType.D3DSIO_TEXM3x3, "texm3x3"},
            {OpcodeType.D3DSIO_TEXDEPTH, "texdepth"},
            {OpcodeType.D3DSIO_CMP, "cmp"},
            {OpcodeType.D3DSIO_BEM, "bem"},
            {OpcodeType.D3DSIO_DP2ADD, "dp2add"},
            {OpcodeType.D3DSIO_DSX, "dsx"},
            {OpcodeType.D3DSIO_DSY, "dsy"},
            {OpcodeType.D3DSIO_TEXLDD, "texldd"},
            {OpcodeType.D3DSIO_SETP, "setp"},
            {OpcodeType.D3DSIO_TEXLDL, "texldl"},
            {OpcodeType.D3DSIO_BREAKP, "break pred"},
            {OpcodeType.D3DSIO_PHASE, "phase"},
            {OpcodeType.D3DSIO_COMMENT, ""},
            {OpcodeType.D3DSIO_END, ""},
        };
    }
}
