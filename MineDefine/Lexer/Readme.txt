Lexing is implemented using gplex. This auto-generates RawLexer.cs from the MineDefine.lex file.
If you need change MineDefine.lex, you'll need to recompile. Folow these steps:

1) Download gplex from http://gplex.codeplex.com/
2) Extract it on your machine, and add /gplex-distr-*/binaries to your path
3) On a command line open MineDefine/Lexer
4) Execute: gplex -out:RawLexer.cs Lexer.lex