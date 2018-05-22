using System;
using System.Collections.Generic;
using System.Linq;

namespace Sha256Hash
{
    class Hash256
    {
        private static readonly bool ILE = BitConverter.IsLittleEndian;

        private static readonly uint[] K = new uint[] { 0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
                                                        0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
                                                        0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
                                                        0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
                                                        0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
                                                        0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
                                                        0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
                                                        0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2 };


        public static byte[] GenerateHash(byte[] byteMessage)
        {
            uint[] H = new uint[] { 0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a, 0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19 }; // Base rests array

            byte [] preparedArray = PrepareByteArray(byteMessage); // Array with paddings

            List<uint[]> uintPartsList = SplitAndConvert(preparedArray); // Split list of arrays converted to uint

            foreach (uint[] uintPart in uintPartsList)
            {
                uint[] W = new uint[64];
                W.AddRangeToFront(uintPart);

                ExpandTo64Uints(ref W); // Expand from 16 to 64 uints

                uint a = H[0];
                uint b = H[1];
                uint c = H[2];
                uint d = H[3];
                uint e = H[4];
                uint f = H[5];
                uint g = H[6];
                uint h = H[7];

                uint s0, s1, t1, t2, ch, maj;
                for (int i = 0; i < 64; i++)
                {
                    s1 = e.RR(6) ^ e.RR(11) ^ e.RR(25);
                    ch = (e & f) ^ ((~e) & g);
                    t1 = h + s1 + ch + K[i] + W[i];

                    s0 = a.RR(2) ^ a.RR(13) ^ a.RR(22);
                    maj = (a & b) ^ (a & c) ^ (b & c);
                    t2 = s0 + maj;

                    h = g;
                    g = f;
                    f = e;
                    e = d + t1;
                    d = c;
                    c = b;
                    b = a;
                    a = t1 + t2;
                }

                H[0] += a;
                H[1] += b;
                H[2] += c;
                H[3] += d;
                H[4] += e;
                H[5] += f;
                H[6] += g;
                H[7] += h;
            }

            if(ILE)
                return H.SelectMany(sm => BitConverter.GetBytes(sm).ReverseToByte()).ToArray();
            else
                return H.SelectMany(sm => BitConverter.GetBytes(sm)).ToArray();
        }


        private static byte[] PrepareByteArray(byte[] byteArray)
        {
            byte[] tempLen = BitConverter.GetBytes((long)byteArray.Length*8);
            if (ILE)
                tempLen = tempLen.Reverse().ToArray();

            var temp = byteArray.ToList();
            temp.Add(0x80);
            while (temp.Count % 64 != 56)
                temp.Add(0x00);
            temp.AddRange(tempLen);

            return temp.ToArray();
        }

        private static List<uint[]> SplitAndConvert(byte[] byteArray)
        {
            List<uint[]> output = new List<uint[]>();
            var temp = byteArray.Split(64); 
            foreach (var part in temp) // for each 64 byte part
            {
                uint[] uPart = new uint[16];
                var numPart = part.Split(4).ToArray();
                for (int i = 0; i < 16; i++) // for each 4 byte part
                {
                    if (ILE)
                        uPart[i] = BitConverter.ToUInt32(numPart[i].ReverseToByte(),0);
                    else
                        uPart[i] = BitConverter.ToUInt32(numPart[i].ToArray(), 0);
                }
                output.Add(uPart);
            }
            return output;
        }

        private static void ExpandTo64Uints(ref uint[] W)
        {
            uint s0, s1;
            for (int i = 16; i < 64; i++)
            {
                s0 = W[i - 15].RR(7) ^ W[i - 15].RR(18) ^ W[i - 15].RS(3);
                s1 = W[i - 2].RR(17) ^ W[i - 2].RR(19) ^ W[i - 2].RS(10);
                W[i] = W[i - 16] + s0 + W[i - 7] + s1;
            }
        }
    }
}
