using System;

namespace AE.Unpacker
{
    class ArcHeader
    {
        public UInt32 dwMagic { get; set; } // 0x424154 (TAB\0)
        public Int16 wMajorVersion { get; set; } // 3
        public Int16 wMinorVersion { get; set; } // 1
        public Int32 dwAligment { get; set; } // 4096
        public Int32 dwTotalFiles { get; set; }
        public Int32 dwCacheTableSize { get; set; } // * 8
        public Int32 dwUnknown { get; set; } // 0
        public Int32 dwMaxCompressedBlockSize { get; set; }
        public Int32 dwMaxDecompressedBlockSize { get; set; }
    }
}
