%{
%}

%%
[sS][aA][vV][eE]                                 { value = yytext;  return SAVE; }
[lL][oO][aA][dD]                                 { value = yytext;  return LOAD; }
[dD][bB]                                         { value = yytext;  return DB; }
[sS][eE][lL][eE][cC][tT]                         { value = yytext;  return SELECT; }
[cC][rR][eE][aA][tT][eE]                         { value = yytext;  return CREATE; }
[dD][rR][oO][pP]                                 { value = yytext;  return DROP; }
[tT][aA][bB][lL][eE]                             { value = yytext;  return TABLE; }
[iI][nN][sS][eE][rR][tT]                         { value = yytext;  return INSERT; }
[dD][eE][lL][eE][tT][eE]                         { value = yytext;  return DELETE; }
[uU][pP][dD][aA][tT][eE]                         { value = yytext;  return UPDATE; }
[fF][rR][oO][mM]                                 { value = yytext;  return FROM; }
[iI][nN][tT][oO]                                 { value = yytext;  return INTO; }
[wW][hH][eE][rR][eE]                             { value = yytext;  return WHERE; }
[vV][aA][lL][uU][eE][sS]                         { value = yytext;  return VALUES; }
[sS][eE][tT]                                     { value = yytext;  return SET; }
[sS][hH][oO][wW]                                 { value = yytext;  return SHOW; }
[tT][aA][bB][lL][eE][sS]                         { value = yytext;  return TABLES; }
[aA][nN][dD]                                     { value = yytext;  return AND; }
[oO][rR]                                         { value = yytext;  return OR; }
[nN][oO][tT]                                     { value = yytext;  return NOT; }
[oO][rR][dD][eE][rR]                             { value = yytext;  return ORDER; }
[bB][yY]                                         { value = yytext;  return BY; }
[mM][iI][nN]                                     { value = yytext;  return MIN; }
[mM][aA][xX]                                     { value = yytext;  return MAX; }
[sS][uU][mM]                                     { value = yytext;  return SUM; }
[cC][oO][uU][nN][tT]                             { value = yytext;  return COUNT; }
[aA][sS][cC]                                     { value = yytext;  return ASC; }
[dD][eE][sS][cC]                                 { value = yytext;  return DESC; }
[nN][uU][lL][lL]                                 { value = yytext;  return NULL; }
[lL][iI][kK][eE]                                 { value = yytext;  return LIKE; }
[gG][rR][oO][uU][pP]                             { value = yytext;  return GROUP; }
[iI][sS]                                         { value = yytext;  return IS; }
[jJ][oO][iI][nN]                                 { value = yytext;  return JOIN; }
[oO][nN]                                         { value = yytext;  return ON; }
[aA][sS]                                         { value = yytext;  return AS; }
[nN][uU][mM][bB][eE][rR]                         { value = yytext.ToUpper(); return NUMBER; }
[vV][aA][rR][cC][hH][aA][rR]                     { value = yytext.ToUpper(); return VARCHAR; }
[sS][tT][aA][rR][tT]                             { value = yytext;  return START; }
[cC][oO][mM][mM][iI][tT]                         { value = yytext;  return COMMIT; }
[rR][oO][lL][lL][bB][aA][cC][kK]                 { value = yytext;  return ROLLBACK; }
[tT][rR][aA][nN][sS][aA][cC][tT][iI][oO][nN]     { value = yytext;  return TRANSACTION; }

"||"                                             { value = yytext;  return TWO_PIPE; }
"!="                                             { value = yytext;  return NOT_EQUAL; }
"<="                                             { value = yytext;  return LESS_OR_EQUAL; }
">="                                             { value = yytext;  return GREATER_OR_EQUAL; }
"{"                                              { value = yytext;  return '{'; }
"}"                                              { value = yytext;  return '}'; }
"("                                              { value = yytext;  return '('; }
")"                                              { value = yytext;  return ')'; }
","                                              { value = yytext;  return ','; }
"="                                              { value = yytext;  return '='; }
"<"                                              { value = yytext;  return '<'; }
">"                                              { value = yytext;  return '>'; }
"*"                                              { value = yytext;  return '*'; }
"+"                                              { value = yytext;  return '+'; }
"-"                                              { value = yytext;  return '-'; }
"/"                                              { value = yytext;  return '/'; }

\d+                                              { value = int.Parse(yytext); return POSITIVE_INT; }
-?\d+(\.\d+)?                                    { value = double.Parse(yytext); return DOUBLE; }
'([^']|'')*'                                     { value = yytext; return STRING; }
[a-zA-Z0-9_]*                                    { value = yytext; return ID; }
[a-zA-Z0-9_]+\.[a-zA-Z0-9_]+                     { value = yytext; return ID_DOT_ID; }
[a-zA-Z0-9_]+\.\*                                { value = yytext; return ID_DOT_STAR; }
[a-zA-Z0-9_:\.\\]+                               { value = yytext; return FILE_PATH; }
[ \t\n]                                          {}

%%
