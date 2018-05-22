using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sha256Hash
{
    static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> collection, int size)
        {
            for (var i = 0; i < (float)collection.Count() / size; i++)
            {
                yield return collection.Skip(i * size).Take(size);
            }
        }

        public static string ToHexString(this byte[] array)
        {
            StringBuilder s = new StringBuilder();
            foreach (byte b in array)
                s.AppendFormat("{0:x2}", b);
            return s.ToString();
        }

        public static uint RR(this uint value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }

        public static uint RS(this uint value, int shift)
        {
            return value >> shift;
        }

        public static byte[] ReverseToByte(this IEnumerable<byte> array)
        {

            return array.Reverse().ToArray();
        }

        public static byte[] ReverseToByte(this byte[] array)
        {
            Array.Reverse(array);
            return array;
        }

        public static void AddRangeToFront(this uint[] dest, uint[] source)
        {
            if (source.Length <= dest.Length)
                for (int i = 0; i < source.Length; i++)
                    dest[i] = source[i];
        }
    }
}
