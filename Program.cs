using System;
using NovaScript;

namespace novascript
{
    class Program
    {
        static void Main(string[] args)
        {
            // currently only lexer is implemented.
            // parser will be implemented next.
            // below is a sample client to test lexer.
            string testScript =
             "if (1+0==5+1 && 2==2 || 1==1) " +
            "{ thing = otherThing; }" +
            " func test {" +
            " " +
            "}";
            string testScript2 =
            "if (1+0==5+1 && 2==2 || 1==1) " +
           "{ thing = otherThing; }" +
           " func test {" +
           " // this should log an exception at index 66 (comments not implemented, yet so it should be unrecognized." +
           "}";

            Lexer lex = new Lexer(testScript);
            foreach (var token in lex.Lex())
                Console.WriteLine("Token:" + token.Key + " - Token Value: " + token.Value);
            Console.WriteLine("---------");
            Console.WriteLine("---------");
            lex = new Lexer(testScript2);
            foreach (var token in lex.Lex())
                Console.WriteLine("Token:" + token.Key + " - Token Value: " + token.Value); // should log exception
        }
    }
}
