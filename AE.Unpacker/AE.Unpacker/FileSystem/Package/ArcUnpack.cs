using System;
using System.IO;
using System.Collections.Generic;

namespace AE.Unpacker
{
    class ArcUnpack
    {
        static List<ArcEntry> m_EntryTable = new List<ArcEntry>();

        public static void iDoIt(String m_Archive, String m_Index, String m_DstFolder)
        {
            ArcHashList.iLoadProject();

            using (FileStream TIndexStream = File.OpenRead(m_Index))
            {
                var m_Header = new ArcHeader();

                m_Header.dwMagic = TIndexStream.ReadUInt32();
                m_Header.wMajorVersion = TIndexStream.ReadInt16();
                m_Header.wMinorVersion = TIndexStream.ReadInt16();
                m_Header.dwAligment = TIndexStream.ReadInt32();
                m_Header.dwTotalFiles = TIndexStream.ReadInt32();
                m_Header.dwCacheTableSize = TIndexStream.ReadInt32();
                m_Header.dwCacheTableSize *= 8;
                m_Header.dwUnknown = TIndexStream.ReadInt32();
                m_Header.dwMaxCompressedBlockSize = TIndexStream.ReadInt32();
                m_Header.dwMaxDecompressedBlockSize = TIndexStream.ReadInt32();

                if (m_Header.dwMagic != 0x424154)
                {
                    throw new Exception("[ERROR]: Invalid magic of TAB index file!");
                }

                if (m_Header.wMajorVersion != 3)
                {
                    throw new Exception("[ERROR]: Invalid major version of TAB index file!");
                }

                if (m_Header.wMinorVersion != 1)
                {
                    throw new Exception("[ERROR]: Invalid minor version of TAB index file!");
                }

                if (m_Header.dwAligment != 4096)
                {
                    throw new Exception("[ERROR]: Invalid aligment of TAB index file!");
                }

                //Skip cached table
                TIndexStream.Seek(m_Header.dwCacheTableSize, SeekOrigin.Current);

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    var m_Entry = new ArcEntry();

                    m_Entry.dwHashName = TIndexStream.ReadUInt64();
                    m_Entry.dwOffset = TIndexStream.ReadUInt32();
                    m_Entry.dwCompressedSize = TIndexStream.ReadInt32();
                    m_Entry.dwDecompressedSize = TIndexStream.ReadInt32();
                    m_Entry.wUnknown = TIndexStream.ReadUInt16();
                    m_Entry.wCompressionType = TIndexStream.ReadUInt16();
                    m_Entry.wCompressionType &= 3;

                    m_EntryTable.Add(m_Entry);
                }

                TIndexStream.Dispose();

                using (FileStream TArchiveStream = File.OpenRead(m_Archive))
                {
                    foreach (var m_Entry in m_EntryTable)
                    {
                        String m_FileName = ArcHashList.iGetNameFromHashList(m_Entry.dwHashName).Replace("/", @"\");
                        String m_FullPath = m_DstFolder + m_FileName;

                        Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                        Utils.iCreateDirectory(m_FullPath);

                        TArchiveStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                        var lpTemp = TArchiveStream.ReadBytes(m_Entry.dwCompressedSize);

                        if (m_Entry.wCompressionType == 3)
                        {
                            var lpBuffer = ZSTD.iDecompress(lpTemp);
                            File.WriteAllBytes(m_FullPath, lpBuffer);
                        }
                        else
                        {
                            File.WriteAllBytes(m_FullPath, lpTemp);
                        }
                    }

                    TArchiveStream.Dispose();
                }
            }
        }
    }
}
