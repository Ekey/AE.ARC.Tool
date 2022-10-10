using System;
using System.Text;

namespace AE.Unpacker
{
    class ArcHash
    {
        // MurMur 3
        private static UInt64 C1 = 0x87c37b91114253d5L;
        private static UInt64 C2 = 0x4cf5ad432745937fL;

        public static UInt64 RotateLeft(UInt64 dwValue, Int32 dwShift)
        {
            return (dwValue << dwShift) | (dwValue >> (64 - dwShift));
        }

        private static UInt64 MixFinal(UInt64 dwValue)
        {
            dwValue ^= dwValue >> 33;
            dwValue *= 0xff51afd7ed558ccdL;
            dwValue ^= dwValue >> 33;
            dwValue *= 0xc4ceb9fe1a85ec53L;
            dwValue ^= dwValue >> 33;
            return dwValue;
        }

        private static UInt64 ProcessBytes(Byte[] lpBuffer)
        {
            UInt64 dwHash1 = 0;
            UInt64 dwHash2 = 0;
            UInt64 dwLength = 0;

            Int32 dwOffset = 0;

            UInt64 dwRemaining = (UInt64)lpBuffer.Length;

            while (dwRemaining >= 16)
            {
                UInt64 dwData1 = BitConverter.ToUInt64(lpBuffer, dwOffset);
                dwOffset += 8;

                UInt64 dwData2 = BitConverter.ToUInt64(lpBuffer, dwOffset);
                dwOffset += 8;

                dwLength += 16;
                dwRemaining -= 16;

                dwData1 *= C1;
                dwData1 = RotateLeft(dwData1, 31);
                dwData1 *= C2;
                dwHash1 ^= dwData1;

                dwHash1 = RotateLeft(dwHash1, 27);
                dwHash1 += dwHash2;
                dwHash1 = dwHash1 * 5 + 0x52dce729;

                dwData2 *= C2;
                dwData2 = RotateLeft(dwData2, 33);
                dwData2 *= C1;
                dwHash2 ^= dwData2;

                dwHash2 = RotateLeft(dwHash2, 31);
                dwHash2 += dwHash1;
                dwHash2 = dwHash2 * 5 + 0x38495ab5;
            }

            if (dwRemaining > 0)
            {
                UInt64 dwData1 = 0;
                UInt64 dwData2 = 0;
                dwLength += dwRemaining;

                switch (dwRemaining)
                {
                    case 15: dwData2 ^= (UInt64)lpBuffer[dwOffset + 14] << 48; goto case 14;
                    case 14: dwData2 ^= (UInt64)lpBuffer[dwOffset + 13] << 40; goto case 13;
                    case 13: dwData2 ^= (UInt64)lpBuffer[dwOffset + 12] << 32; goto case 12;
                    case 12: dwData2 ^= (UInt64)lpBuffer[dwOffset + 11] << 24; goto case 11;
                    case 11: dwData2 ^= (UInt64)lpBuffer[dwOffset + 10] << 16; goto case 10;
                    case 10: dwData2 ^= (UInt64)lpBuffer[dwOffset + 9] << 8; goto case 9;
                    case 9: dwData2 ^= (UInt64)lpBuffer[dwOffset + 8]; goto case 8;
                    case 8: dwData1 ^= BitConverter.ToUInt64(lpBuffer, dwOffset); break;
                    case 7: dwData1 ^= (UInt64)lpBuffer[dwOffset + 6] << 48; goto case 6;
                    case 6: dwData1 ^= (UInt64)lpBuffer[dwOffset + 5] << 40; goto case 5;
                    case 5: dwData1 ^= (UInt64)lpBuffer[dwOffset + 4] << 32; goto case 4;
                    case 4: dwData1 ^= (UInt64)lpBuffer[dwOffset + 3] << 24; goto case 3;
                    case 3: dwData1 ^= (UInt64)lpBuffer[dwOffset + 2] << 16; goto case 2;
                    case 2: dwData1 ^= (UInt64)lpBuffer[dwOffset + 1] << 8; goto case 1;
                    case 1: dwData1 ^= (UInt64)lpBuffer[dwOffset]; break;
                }

                dwData1 *= C1;
                dwData1 = RotateLeft(dwData1, 31);
                dwData1 *= C2;
                dwHash1 ^= dwData1;

                dwData2 *= C2;
                dwData2 = RotateLeft(dwData2, 33);
                dwData2 *= C1;
                dwHash2 ^= dwData2;
            }

            dwHash1 ^= dwLength;
            dwHash2 ^= dwLength;

            dwHash1 += dwHash2;
            dwHash2 += dwHash1;

            dwHash1 = MixFinal(dwHash1);
            dwHash2 = MixFinal(dwHash2);

            dwHash1 += dwHash2;
            dwHash2 += dwHash1;

            return dwHash1;
        }

        public static UInt64 iGetHash(String m_String)
        {
            return ProcessBytes(Encoding.ASCII.GetBytes(m_String));
        }
    }
}
