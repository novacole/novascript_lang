namespace NovaScript.Tokens
{
public class StringToken : IToken { }
public class IntegerToken : IToken { }
public class WhiteSpaceToken : IToken { }
public class IdentifierToken : IToken { }
public class SymbolToken : IToken { }
public class ReservedToken : IToken { }
public class ExpressionBlockStartToken : IToken { }
public class ExpressionBlockEndToken : IToken { }
public class ParenStartToken : IToken { }
public class ParenEndToken : IToken { }
public class StatementTerminatorToken : IToken { }
public class BooleanOperatorToken : IToken { }
}