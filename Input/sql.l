%{
%}

%%
[sS][eE][lL][eE][cC][tT]      { return SELECT; }
[cC][rR][eE][aA][tT][eE]      { return CREATE; }
[tT][aA][bB][lL][eE]          { return TABLE; }
[iI][nN][sS][eE][rR][tT]      { return INSERT; }
[dD][eE][lL][eE][tT][eE]      { return DELETE; }
[uU][pP][dD][aA][tT][eE]      { return UPDATE; }
[fF][rR][oO][mM]              { return FROM; }
[iI][nN][tT][oO]              { return INTO; }
[wW][hH][eE][rR][eE]          { return WHERE; }
[vV][aA][lL][uU][eE][sS]      { return VALUES; }
[sS][eE][tT]                  { return SET; }
[sS][hH][oO][wW]              { return SHOW; }
[tT][aA][bB][lL][eE][sS]      { return TABLES; }
[aA][nN][dD]                  { return AND; }
[oO][rR]                      { return OR; }
[nN][oO][tT]                  { return NOT; }
[nN][uU][mM][bB][eE][rR]      { value = "NUMBER_TYPE"; return NUMBER_TYPE; }
[vV][aA][rR][cC][hH][aA][rR]  { value = "VARCHAR"; return VARCHAR; }
"!="                     { return NOT_EQUAL; }
"<="                     { return LESS_OR_EQUAL; }
">="                     { return GREATER_OR_EQUAL; }
"{"  { return '{'; }
"}"  { return '}'; }
"("  { return '('; }
")"  { return ')'; }
","  { return ','; }
"="  { return '='; }
"<"  { return '<'; }
">"  { return '>'; }
"*"  { return '*'; }

-?\d+(\.\d+)?           { value = yytext; return NUMBER; }
'([^']|'')*'               { value = yytext; return STRING; }
[a-zA-Z0-9_]*      { value = yytext; return ID; }
[ \t\n]                      {}

%%
