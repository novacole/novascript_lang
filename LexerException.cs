using System;
namespace NovaScript
{
    public class LexerException
    {
        public LexerException(string exception, int index)
        {
            Console.Write("Lexer Exception unknown symbol: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception);
            Console.ResetColor();
            Console.WriteLine("Lexer Exception index:" + index);
        }

    }
}