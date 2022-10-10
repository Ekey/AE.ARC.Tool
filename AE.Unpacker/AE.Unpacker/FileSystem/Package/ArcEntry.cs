using System;

namespace AE.Unpacker
{
    class ArcEntry
    {
        public UInt64 dwHashName { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwCompressedSize { get; set; }
        public Int32 dwDecompressedSize { get; set; }
        public UInt16 wUnknown { get; set; } // ID ?
        public UInt16 wCompressionType { get; set; } // 3 (ZSTD)
    }
}
