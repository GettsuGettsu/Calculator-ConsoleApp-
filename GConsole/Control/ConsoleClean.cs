using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.GConsole.Control
{
    public class ConsoleClean
    {
        static void ClearLine()
        {
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.BufferWidth));
            Console.CursorLeft = 0;
            Console.CursorTop--;
        }

        public static (string, int) DeleteChar(string str, int cursorLeft)
        {
            if (str.Length > 0)
            {
                str = str.Substring(0, str.Length - 1);
                ClearLine();
                Console.Write(str);
                cursorLeft -= 1;
            }
            return (str, cursorLeft);
        }
    }
}
