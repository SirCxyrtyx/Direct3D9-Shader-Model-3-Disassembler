using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct3D9_Shader_Model_3_Disassembler
{
    static class Extensions
    {
        public static uint ReadUInt(this Stream stream)
        {
            var bytes = new byte[4];
            stream.Read(bytes, 0, 4);
            return BitConverter.ToUInt32(bytes, 0);
        }
        public static ushort ReadUShort(this Stream stream)
        {
            var bytes = new byte[2];
            stream.Read(bytes, 0, 2);
            return BitConverter.ToUInt16(bytes, 0);
        }

        public static string ReadString(this Stream stream)
        {

            long startPos = stream.Position;
            int length = 0;
            while (stream.ReadByte() != 0)
            {
                length++;
            }

            stream.Seek(startPos, SeekOrigin.Begin);
            byte[] buff = new byte[length];
            stream.Read(buff, 0, length);
            return Encoding.ASCII.GetString(buff);
        }

        public static int[] Unpack(this int word, params (short end, short start)[] ranges)
        {
            Contract.Assert(ranges.Length > 0, "need to provide ranges");
            short prev = -1;
            foreach (var range in ranges)
            {
                if (range.end > 32)
                {
                    throw new ArgumentOutOfRangeException(nameof(ranges), "arguments must be <= 32");
                }
                if (range.start <= prev || range.end < range.start)
                {
                    throw new ArgumentException("arguments must be in order");
                }

                prev = range.end;
            }
            BitVector32 bitVec = new BitVector32(word);
            var sections = new List<BitVector32.Section>();
            BitVector32.Section prevSection = BitVector32.CreateSection((short)((1 << (ranges[0].end - ranges[0].start)) - 1));
            sections.Add(prevSection);
            for (int i = 1; i < ranges.Length; i++)
            {
                var (end, start) = ranges[i];
                {
                    short maxValue = (short)((1 << (end - start)) - 1);
                    prevSection = BitVector32.CreateSection(maxValue, prevSection);
                    sections.Add(prevSection);
                }
            }

            return sections.Select(section => bitVec[section]).ToArray();
        }

        public static uint bits(this uint word, byte from, byte to)
        {
            Contract.Assert(from < 32);
            Contract.Assert(to < 32);
            Contract.Assert(to < from);

            return (word << (31 - from)) >> (31 - from + to);
        }

        public static bool bit(this uint word, byte index)
        {
            Contract.Assert(index < 32);

            return (word << (31 - index)) >> 31 == 1;
        }
    }
}
