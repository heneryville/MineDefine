%namespace MineDefine.Lexer

%option minimize, verbose, persistbuffer, unicode, compressNext, embedbuffers, caseInsensitive, noParser

%{
public void yyerror(string format, params object[] args) // remember to add override back
{
    string msg = string.Format("Error: line {0} - " + format, yyline);
    System.Console.Error.WriteLine(msg);
    throw new UnexpectedSequenceException(format, yyline, yycol);
}


public int line { get { return yyline; } }
public int col { get { return yycol; } }
%}

// Syntax
LineEnd ";"
OpenBrace "{"
CloseBrace "}"
Colon ":"

//Absolute directions
Up up
Down down
North north
South south
East east
West west


//Relative directions
Top top
Bottom bottom
Left left
Right right
Front front
Back back

// Terminals
Dimension ([0-9]+x)+[0-9]+
Location (-?[0-9]+,)+-?[0-9]+
Identifier		@[a-zA-Z]([a-zA-Z0-9_\-])*
Operation		[a-zA-Z]([a-zA-Z0-9_\-])*
Numeral -?[0-9]+

// Non characters
WhiteSpace		[ \t\n\r]
Unknown [^\s]

%%
//
// Start of Rules
//

// Remove whitespaces.
{WhiteSpace}+	{ ; }

// Emit tokens
// Syntax elements
{LineEnd}	{ return (int) TokenKind.EndOfLine; }
{OpenBrace}	{ return (int) TokenKind.OpenBrace; }
{CloseBrace}	{ return (int) TokenKind.CloseBrace; }
{Colon}	{ return (int) TokenKind.Colon; }

//Absolute Directions
{Up}	{ return (int) TokenKind.Up; }
{Down}	{ return (int) TokenKind.Down; }
{North}	{ return (int) TokenKind.North; }
{South}	{ return (int) TokenKind.South; }
{East}	{ return (int) TokenKind.East; }
{West}	{ return (int) TokenKind.West; }

//Relative Directions
{Top}	{ return (int) TokenKind.Top; }
{Bottom}	{ return (int) TokenKind.Bottom; }
{Left}	{ return (int) TokenKind.Left; }
{Right}	{ return (int) TokenKind.Right; }
{Front}	{ return (int) TokenKind.Front; }
{Back}	{ return (int) TokenKind.Back; }

//Terminals
{Numeral}	{ return (int) TokenKind.Numeral; }
{Dimension}	{ return (int) TokenKind.Dimension; }
{Location}				{ return (int) TokenKind.Location; }
{Identifier}				{ return (int) TokenKind.Identifier; }
{Operation}				{ return (int) TokenKind.Operation; }
{Unknown}		{ yyerror("Unknown character: " + yytext);}