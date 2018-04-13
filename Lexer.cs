
using System;
using System.Collections.Generic;
using System.Linq;
using NovaScript.Tokens;

namespace NovaScript
{
    partial class Lexer
    {
        public Dictionary<IToken, string> _tokenPair { get; set; }
        string _source { get; set; }
        int _index { get; set; }
        string _currentWord { get; set; }
        public Lexer(string source)
        {
            _source = source;
            _tokenPair = new Dictionary<IToken, string>();
        }
        public Dictionary<IToken, string> Lex()
        {
            for (_index = 0; _index < _source.Length;)
            {
                if (!IsOutOfRange(1))
                {
                    if (IsWhiteSpace(_source[_index].ToString()))
                    {
                        //just consume and ignore whitespace
                    }
                    else if (IsString(_source[_index].ToString()))
                    {
                        _tokenPair.Add(new StringToken(), _currentWord.Trim());
                        _currentWord = string.Empty;
                    }
                    else if (IsInteger(_source[_index].ToString()))
                    {
                        _tokenPair.Add(new IntegerToken(), _currentWord.Trim());
                        _currentWord = string.Empty;
                    }
                    else if (IsIdentifier(_source[_index].ToString()))
                    {
                        if (IsReservedWord(_currentWord.Trim()))
                        {
                            _tokenPair.Add(new ReservedToken(), _currentWord.Trim());
                            _currentWord = string.Empty;
                        }
                        else
                        {
                            _tokenPair.Add(new IdentifierToken(), _currentWord.Trim());
                            _currentWord = string.Empty;
                        }
                    }
                    else if (IsSymbol(_source[_index].ToString()))
                    {
                        _tokenPair.Add(new SymbolToken(), _currentWord.Trim());
                        _currentWord = string.Empty;
                    }
                    else if (IsBooleanOperator(_source[_index].ToString()))
                    {
                        _tokenPair.Add(new BooleanOperatorToken(), _currentWord.Trim());
                        _currentWord = string.Empty;
                    }
                    else if (IsBlockExpressionStart(_source[_index].ToString()))
                    {
                        _tokenPair.Add(new ExpressionBlockStartToken(), _currentWord.Trim());
                        _currentWord = string.Empty;
                    }
                    else if (IsBlockExpressionEnd(_source[_index].ToString()))
                    {
                        _tokenPair.Add(new ExpressionBlockEndToken(), _currentWord.Trim());
                        _currentWord = string.Empty;
                    }
                    else if (IsParenStart(_source[_index].ToString()))
                    {
                        _tokenPair.Add(new ParenStartToken(), _currentWord.Trim());
                        _currentWord = string.Empty;
                    }
                    else if (IsParenEnd(_source[_index].ToString()))
                    {
                        _tokenPair.Add(new ParenEndToken(), _currentWord.Trim());
                        _currentWord = string.Empty;
                    }
                    else if (IsStatementTerminator(_source[_index].ToString()))
                    {
                        _tokenPair.Add(new StatementTerminatorToken(), _currentWord.Trim());
                        _currentWord = string.Empty;
                    }
                    else
                    {
                        // lexer failure, unrecognized symbol.
                        new LexerException(_source[_index].ToString(), _index);
                        break;
                    }
                }
            }
            return _tokenPair;
        }
        public bool IsInteger(string symbol)
        {
            int lastCheckedIndex = 0;
            if (Char.IsDigit(_source[_index]))
            {
                _currentWord += _source[_index++].ToString();
                while (!IsOutOfRange(1) && !IsWhiteSpace(_source[_index].ToString()) && Char.IsDigit(_source[_index]))
                {
                    _currentWord += _source[_index++].ToString();
                }
                return true;
            }
            if (lastCheckedIndex > 0)
            {
                _index = lastCheckedIndex;
            }
            return false;
        }
        public bool IsIdentifier(string word)
        {
            int lastCheckedIndex = 0;

            if (Char.IsLetter(_source[_index]) || _source[_index].ToString() == "_")
            {
                _currentWord += _source[_index++].ToString();
                while (!IsOutOfRange(1) && !IsWhiteSpace(_source[_index].ToString()) && Char.IsLetterOrDigit(_source[_index]))
                {
                    _currentWord += _source[_index].ToString();
                    _index++;
                }
                return true;
            }
            if (lastCheckedIndex > 0)
            {
                _index = lastCheckedIndex;
            }
            return false;
        }
        public bool IsString(string symbol)
        {
            int lastCheckedIndex = 0;
            if (symbol == "\"")
            {
                _index++;
                while (!IsOutOfRange(1)
                && !(IsStringTerminator(_source[_index].ToString())
                && !(IsStringTerminator(_source[_index].ToString())
                && (Char.IsLetterOrDigit(_source[_index])
                || IsWhiteSpace(_source[_index].ToString())))))
                {
                    _currentWord += _source[_index].ToString();
                    lastCheckedIndex = _index;
                    _index++;
                }
                return true;
            }
            if (lastCheckedIndex > 0)
            {
                _index = lastCheckedIndex;
            }
            return false;
        }
        public bool IsWhiteSpace(string symbol)
        {
            if (symbol == " " || symbol == "\n" || symbol == "\t")
            {
                _currentWord += _source[_index++].ToString();
                return true;
            }
            return false;
        }
        public bool IsSymbol(string symbol)
        {
            if (Char.IsSymbol(symbol.ToCharArray()[0])
            || symbol.ToCharArray()[0] == '-' && symbol != "|")
            // symbol for us just means =,<,>
            {
                _currentWord += _source[_index++].ToString();
                return true;
            }

            return false;
        }
        public bool IsBooleanOperator(string symbol)
        {
            if (Char.IsPunctuation(symbol.ToCharArray()[0]) && (symbol == "&") || symbol == "|")
            {
                _currentWord += _source[_index++].ToString();
                return true;
            }

            return false;
        }
        public bool IsReservedWord(string symbol)
        {
            bool isKeyword = Enum.GetValues(typeof(ReservedWords)).Cast<ReservedWords>()
                .ToList().Select(x => x.ToString().ToLower())
                .Where(x => x == symbol).Any();
            if (isKeyword == true)
            {
                _currentWord = symbol;
                return true;
            }
            return false;
        }
        public bool IsStringTerminator(string symbol)
        {
            if (symbol == "\"")
            {
                if (!IsOutOfRange(1))
                {
                    _index++;
                }
                return true;
            }
            return false;
        }
        public bool IsBlockExpressionStart(string symbol)
        {
            if (symbol == "{")
            {
                if (!IsOutOfRange(1))
                {
                    _currentWord = _source[_index++].ToString();
                }
                return true;
            }
            return false;
        }
        public bool IsBlockExpressionEnd(string symbol)
        {
            if (symbol == "}")
            {
                if (!IsOutOfRange(1))
                {
                    _currentWord = _source[_index++].ToString();
                }
                return true;
            }
            return false;
        }
        public bool IsParenStart(string symbol)
        {
            if (symbol == "(")
            {
                if (!IsOutOfRange(1))
                {
                    _currentWord = _source[_index++].ToString();
                }
                return true;
            }
            return false;
        }
        public bool IsParenEnd(string symbol)
        {
            if (symbol == ")")
            {
                if (!IsOutOfRange(1))
                {
                    _currentWord = _source[_index++].ToString();
                }
                return true;
            }
            return false;
        }
        public bool IsStatementTerminator(string symbol)
        {
            if (symbol == ";")
            {
                if (!IsOutOfRange(1))
                {
                    _currentWord = _source[_index++].ToString();
                }
                return true;
            }
            return false;
        }
        public bool IsOutOfRange(int peek)
        {
            if (_index + peek > _source.Length || _index + peek < 0)
                return true;
            return false;
        }
    }
}