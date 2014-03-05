using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MineDefine.Lexer;
using MineDefine.Parser.AST;
using Tests.Parser.AST;

namespace MineDefine.Parser
{
    public class MineDefineParser
    {
        private readonly LookAheadStream<LexToken> _tokens;

        private static ISet<TokenKind> preBuild = new HashSet<TokenKind>() {
            TokenKind.Identifier,
            TokenKind.Operation,
            TokenKind.Dimension,
            TokenKind.Location
        };

        private static ISet<TokenKind> preStatement = new HashSet<TokenKind>(preBuild
            .Concat(TokenKindExtensions.abosluteDirections)
            .Concat(TokenKindExtensions.relativeDirections)
            );

    public MineDefineParser(IEnumerable<LexToken> tokens)
        {
            _tokens = new LookAheadStream<LexToken>(tokens);
        }

        private LexToken Current { get { return _tokens.Current; } }

        public MineDefineAST Parse()
        {
            GuaranteedMoveNext();
            var statements = ParseStatements().ToList();
            if(!_tokens.IsEOF) throw new ParsingException("Expected end of file, but not reached");
            return new MineDefineAST(statements);
        }

        private IStatement ParseStatement()
        {
            if (Current.Kind.IsTransformDirection())
            {
                 return ParseTransform();
            }
            else if (Current.Kind == TokenKind.Identifier && LookAheadIs(1, TokenKind.Colon))
            {
                 return ParseDefinition();
            }
            else if (preBuild.Contains(Current.Kind))
            {
                 return ParseBuild();
            }
            else throw new UnexpectedToken(Current, "Transform, Identifier, or Build");
        }

        private IEnumerable<IStatement> ParseStatements()
        {
            while(!_tokens.IsEOF && preStatement.Contains(Current.Kind))
            {
                yield return ParseStatement();
            }
        }

        private BuildShape ParseBuildShape(string value)
        {
            return (BuildShape) Enum.Parse(typeof (BuildShape), value.Replace("-",""), true);
        }

        private BuildStatement ParseBuild()
        {
            var shape = BuildShape.Box;
            var dimension = Dimension.Default;
            var location = new Location();
            if (Current.Kind == TokenKind.Operation)
            {
                shape = ParseBuildShape(Current.Value);
                GuaranteedMoveNext();
            }

            if (Current.Kind == TokenKind.Dimension)
            {
                dimension = Dimension.Parse(Current.Value);
                GuaranteedMoveNext();
            }

            if (Current.Kind == TokenKind.Location)
            {
                location = Location.Parse(Current.Value);
                GuaranteedMoveNext();
            }

            if (Current.Kind == TokenKind.Identifier)
            {
                var ident = Current;
                GuaranteedMoveNext(TokenKind.EndOfLine);
                _tokens.MoveNext();
                return new BuildByIdentifier(shape, dimension, location, ident.Value.Substring(1));
            }
            else
            {
                var statements = ParseStatementBlock();
                return new BuildByStatements(shape, dimension, location, statements);
            }
        }

        private ElementDefinition ParseDefinition()
        {
            var name = Current.Value.Substring(1); //Truncate the @
            GuaranteedMoveNext(TokenKind.Colon);
            GuaranteedMoveNext();
            var statements = ParseStatementBlock();
            return new ElementDefinition(name,statements);
        }

        private IList<IStatement> ParseStatementBlock()
        {
            IList<IStatement> stats = null;
            if (Current.Kind == TokenKind.OpenBrace)
            {
                GuaranteedMoveNext();
                stats = ParseStatements().ToList();
                AssertIs(TokenKind.CloseBrace);
                _tokens.MoveNext();
            }
            else
            {
                stats = new [] { ParseStatement() };
            }
            return stats;
        }

        private IEnumerable<IStatement> ParseStatementBlockOptional()
        {
            if (Current.Kind == TokenKind.EndOfLine)
            {
                _tokens.MoveNext();
                return null;
            }
            return ParseStatementBlock();
        }

        private OriginTransformStatement ParseTransform()
        {
            var instructions = ParseTransformInstructions();
            var statements = ParseStatementBlockOptional();
            return new OriginTransformStatement(instructions, statements);
        }

        private OriginTransformInstructions ParseTransformInstructions()
        {
            var transformDir = Current;
            var degree = 1;
            GuaranteedMoveNext();

            if (Current.Kind == TokenKind.Numeral)
            {
                if (transformDir.Kind.IsAbsoulteTransformDirection())
                {
                    degree = int.Parse(Current.Value);
                    GuaranteedMoveNext();
                }
                else { throw new ParsingException("Relative transforms may not have numeric offsets"); }
            }

            if (transformDir.Kind.IsAbsoulteTransformDirection())
            {
                var dir = (AbsoluteOriginTransform) Enum.Parse(typeof (AbsoluteOriginTransform), transformDir.Value, true);
                return new AbsoluteTransformInstructions(dir, degree);
            }
            else
            {
                var dir = (RelativeOriginTransform) Enum.Parse(typeof (RelativeOriginTransform), transformDir.Value, true);
                return new RelativeTransformInstructions(dir);
            }

        }

        private bool LookAheadIs(int i, TokenKind kind)
        {
            var ll = _tokens.LookAhead(i);
            return ll.HasCharactersUpTo && ll.Value.Kind == kind;
        }

        private void AssertIs(TokenKind kind)
        {
            if(kind != Current.Kind) throw new UnexpectedToken(Current,kind);
        }

        private void GuaranteedMoveNext()
        {
            if(!_tokens.MoveNext()) throw new UnexpectedEndOfStream();
        }

        private void GuaranteedMoveNext(TokenKind kind)
        {
            GuaranteedMoveNext();
            AssertIs(kind);
        }
    }
}
