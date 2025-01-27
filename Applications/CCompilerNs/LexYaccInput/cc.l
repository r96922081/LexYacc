%{
%}

%% 
"int"                     { value = yytext; return INT_TYPE; }
"void"                    { value = yytext; return VOID_TYPE; }
"char"                    { value = yytext; return CHAR_TYPE; }
"return"                  { value = yytext; return RETURN; }
"if"                      { value = yytext; return IF; }
"else"                    { value = yytext; return ELSE; }
"for"                     { value = yytext; return FOR; }
"while"                   { value = yytext; return WHILE; }
"break"                   { value = yytext; return BREAK; }
"continue"                { value = yytext; return CONTINUE; }
"struct"                  { value = yytext; return STRUCT; }
"=="                      { value = yytext; return EQUAL_SIGN; }
"!="                      { value = yytext; return NOT_EQUAL_SIGN; }
"<="                      { value = yytext; return LESS_OR_EQUAL_SIGN; }
">="                      { value = yytext; return GREATER_OR_EQUAL_SIGN; }
"++"                      { value = yytext; return INCREMENT; }
"--"                      { value = yytext; return DECREMENT; }
"+="                      { value = yytext; return PLUS_ASSIGN; }
"-="                      { value = yytext; return MINUS_ASSIGN; }
"*="                      { value = yytext; return MULTIPLY_ASSIGN; }
"/="                      { value = yytext; return DIVIDE_ASSIGN; }
"&&"                      { value = yytext; return LOGICAL_AND; }
"||"                      { value = yytext; return LOGICAL_OR; }
"->"                      { value = yytext; return ARROW; }
-?[0-9]+                  { value = int.Parse(yytext); return INT_VALUE; }
'[ -~]'                   { value = yytext[1]; return CHAR_VALUE; }
'\\r'                     { value = '\r'; return CHAR_VALUE; }
'\\n'                     { value = '\n'; return CHAR_VALUE; }
'\\t'                     { value = '\t'; return CHAR_VALUE; }
'\\0'                     { value = '\0'; return CHAR_VALUE; }
[_a-zA-Z][_a-zA-Z0-9]*     { value = yytext; return ID; }
\"[^"]*\"                 { yytext = yytext.Substring(1); value = yytext.Substring(0, yytext.Length - 1); return STRING_LITERAL;}
[ \t\n\r]+                {}

"{"                       { return yytext[0]; }
"}"                       { return yytext[0]; }
"["                       { return yytext[0]; }
"]"                       { return yytext[0]; }
"<"                       { return yytext[0]; }
">"                       { return yytext[0]; }
"("                       { return yytext[0]; }
")"                       { return yytext[0]; }
";"                       { return yytext[0]; }
"+"                       { return yytext[0]; }
"-"                       { return yytext[0]; }
"*"                       { return yytext[0]; }
"/"                       { return yytext[0]; }
"="                       { return yytext[0]; }
"%"                       { return yytext[0]; }
","                       { return yytext[0]; }
"."                       { return yytext[0]; }
"&"                       { return yytext[0]; }
%%