﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsistentHashing
{
    public static class FnvHash
    {
        public static uint To32BitFnvHash(this string toHash, bool separateUpperByte = false)
        {
            IEnumerable<byte> bytesToHash;

            if (separateUpperByte)
                bytesToHash = toHash.ToCharArray()
                    .Select(c => new[] { (byte)((c - (byte)c) >> 8), (byte)c })
                    .SelectMany(c => c);
            else
                bytesToHash = toHash.ToCharArray()
                    .Select(Convert.ToByte);

            uint hash = FnvConstants.FnvOffset32;

            foreach (var chunk in bytesToHash)
            {
                hash ^= chunk;
                hash *= FnvConstants.FnvPrime32;
            }

            return hash;
        }
    }
    public static class FnvConstants
    {
        public static readonly uint FnvPrime32 = 16777619;
        public static readonly ulong FnvPrime64 = 1099511628211;
        public static readonly uint FnvOffset32 = 2166136261;
        public static readonly ulong FnvOffset64 = 14695981039346656037;
    }
}
