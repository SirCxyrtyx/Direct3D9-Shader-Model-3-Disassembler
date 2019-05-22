using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct3D9_Shader_Model_3_Disassembler
{
    class Program
    {
        private const uint PIXELSHADER = 0xFFFF;
        private const uint VERTEXSHADER = 0xFFFE;
        private const uint END_TOKEN = 0x0000FFFF;
        private const uint CTAB = 0x42415443;

        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: Shader3DeComp.exe infile outfile");
                return 1;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Input file does not exist!");
                return 1;
            }

            using (FileStream inFile = new FileStream(args[0], FileMode.Open, FileAccess.Read))
            using (FileStream outFile = new FileStream(args[1], FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(outFile))
            {
                uint vers = inFile.ReadUInt();
                var minor = vers.bits(7, 0);
                var major = vers.bits(15, 8);
                var shaderType = vers.bits(31, 16);
                if (shaderType != PIXELSHADER && shaderType != VERTEXSHADER)
                {
                    Console.WriteLine("Not a valid Shader!");
                    return 1;
                }
                writer.WriteLine($"{(shaderType == PIXELSHADER ? "ps" : "vs")}_{major}_{minor}");

                uint instructionSize = 0;
                while (inFile.Position + 4 <= inFile.Length)
                {
                    uint token = inFile.ReadUInt();
                    uint opcode = token.bits(15, 0);
                    if (opcode == (uint)OpcodeType.D3DSIO_END)
                    {
                        writer.WriteLine("END");
                        return 0;
                    }

                    if (opcode == (uint)OpcodeType.D3DSIO_COMMENT)
                    {
                        //writer.WriteLine("Begin COMMENT");
                        uint length = token.bits(30, 16);
                        long commentEnd = length * 4 + inFile.Position;
                        if (inFile.ReadUInt() == CTAB)
                        {
                            long ctabPos = inFile.Position;
                            var header = new CTHeader(inFile);
                            inFile.Seek(ctabPos + header.ConstantInfo, SeekOrigin.Begin);
                            var constantInfos = new CTInfo[header.Constants];
                            for (int i = 0; i < header.Constants; i++)
                            {
                                constantInfos[i] = new CTInfo(inFile);
                            }

                            inFile.Seek(ctabPos + header.Creator, SeekOrigin.Begin);
                            string creator = inFile.ReadString();
                            writer.WriteLine(creator);

                            writer.WriteLine();

                            //writer.WriteLine("BEGIN Constant Table");
                            foreach (CTInfo info in constantInfos)
                            {
                                inFile.Seek(ctabPos + info.TypeInfo, SeekOrigin.Begin);
                                var type = new CTType(inFile);
                                inFile.Seek(ctabPos + info.Name, SeekOrigin.Begin);
                                string name = inFile.ReadString();
                                writer.WriteLine();
                                writer.WriteLine(name);
                                writer.WriteLine($"RegisterSet: {(EREGISTER_SET)info.RegisterSet}, RegisterIndex: {info.RegisterIndex}, RegisterCount: {info.RegisterCount}");
                            }
                            writer.WriteLine();
                            //writer.WriteLine("END Constant Table");


                        }
                        inFile.Seek(commentEnd, SeekOrigin.Begin);
                        //writer.WriteLine("End COMMENT");
                        writer.WriteLine();
                        continue;
                    }

                    if (instructionSize == 0)
                    {
                        instructionSize = token.bits(27, 24);
                        writer.WriteLine($"{d3d9types.opcodeNames[(OpcodeType)opcode]} (instruction size: {instructionSize})");

                        for (int i = 0; i < instructionSize; i++)
                        {
                            
                        }
                        continue;
                    }

                    instructionSize--;
                }
            }
            Console.WriteLine("No End Token found!");
            return 1;
        }

         
    }
}
