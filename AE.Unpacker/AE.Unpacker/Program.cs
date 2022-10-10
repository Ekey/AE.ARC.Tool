using System;
using System.IO;

namespace AE.Unpacker
{
    class Program
    {
        private static String m_Title = "Apex Engine ARC Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2022 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    AE.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of TAB/ARC file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    AE.Unpacker E:\\Games\\SE\\archives_win64\\initial\\game0.tab D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_Input = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists("Zstandard.Net.dll") || !File.Exists("libzstd.dll"))
            {
                Utils.iSetError("[ERROR]: Unable to find ZSTD modules");
                return;
            }

            String m_ArcFile = Path.GetDirectoryName(m_Input) + @"\" + Path.GetFileNameWithoutExtension(m_Input) + ".arc";
            if (!File.Exists(m_ArcFile))
            {
                Utils.iSetError("[ERROR]: Input Arc file -> " + m_ArcFile + " <- does not exist");
                return;
            }

            String m_TabFile = Path.GetDirectoryName(m_Input) + @"\" + Path.GetFileNameWithoutExtension(m_Input) + ".tab";
            if (!File.Exists(m_TabFile))
            {
                Utils.iSetError("[ERROR]: Input Tab file -> " + m_ArcFile + " <- does not exist");
                return;
            }

            ArcUnpack.iDoIt(m_ArcFile, m_TabFile, m_Output);
        }
    }
}
