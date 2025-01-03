%{
%}

%%
[sS][aA][vV][eE]              { return SAVE; }
[lL][oO][aA][dD]              { return LOAD; }
[dD][bB]                      { return DB; }
[sS][eE][lL][eE][cC][tT]      { return SELECT; }
[cC][rR][eE][aA][tT][eE]      { return CREATE; }
[dD][rR][oO][pP]              { return DROP; }
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
[oO][rR][dD][eE][rR]          { return ORDER; }
[bB][yY]                      { return BY; }
[mM][iI][nN]                  { return MIN; }
[mM][aA][xX]                  { return MAX; }
[sS][uU][mM]                  { return SUM; }
[cC][oO][uU][nN][tT]          { return COUNT; }
[aA][sS][cC]                  { return ASC; }
[dD][eE][sS][cC]              { return DESC; }
[nN][uU][lL][lL]              { return NULL; }
[lL][iI][kK][eE]              { return LIKE; }
[gG][rR][oO][uU][pP]          { return GROUP; }
[iI][sS]                      { return IS; }
[nN][uU][mM][bB][eE][rR]      { value = "NUMBER"; return NUMBER; }
[vV][aA][rR][cC][hH][aA][rR]  { value = "VARCHAR"; return VARCHAR; }
[sS][tT][aA][rR][tT]          { return START; }
[cC][oO][mM][mM][iI][tT]      { return COMMIT; }
[rR][oO][lL][lL][bB][aA][cC][kK]                 { return ROLLBACK; }
[tT][rR][aA][nN][sS][aA][cC][tT][iI][oO][nN]     { return TRANSACTION; }

"||"                          { return TWO_PIPE; }
"!="                          { return NOT_EQUAL; }
"<="                          { return LESS_OR_EQUAL; }
">="                          { return GREATER_OR_EQUAL; }
"{"                           { return '{'; }
"}"                           { return '}'; }
"("                           { return '('; }
")"                           { return ')'; }
","                           { return ','; }
"="                           { return '='; }
"<"                           { return '<'; }
">"                           { return '>'; }
"*"                           { return '*'; }
"+"                           { return '+'; }
"-"                           { return '-'; }
"/"                           { return '/'; }

\d+                           { value = int.Parse(yytext); return POSITIVE_INT; }
-?\d+(\.\d+)?                 { value = double.Parse(yytext); return DOUBLE; }
'([^']|'')*'                  { value = yytext; return STRING; }
[a-zA-Z0-9_]*                 { value = yytext; return ID; }
[a-zA-Z0-9_:\.\\]+            { value = yytext; return FILE_PATH; }
[ \t\n]                       {}

%%
