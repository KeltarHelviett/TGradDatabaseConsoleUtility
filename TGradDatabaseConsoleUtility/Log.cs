using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGradDatabaseConsoleUtility
{
    static class Log
    {
        public static void Warn(string highlightedText, string text)
        {
            LogText(ConsoleColor.Yellow, $"WARN: {highlightedText}", text);
        }

        public static void Error(string highlightedText, string text)
        {
            LogText(ConsoleColor.Red, $"ERROR: {highlightedText}", text);
        }

        public static void LogText(ConsoleColor color, string highlightedText, string text)
        {
            Console.ForegroundColor = color;
            Console.Write(highlightedText);
            Console.ResetColor();
            Console.Write(text + "\n");
        }
    }
}
