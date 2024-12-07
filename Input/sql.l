%{
%}

%%
"SELECT"                     { return SELECT; }
"CREATE"                     { return CREATE; }
"TABLE"                     { return TABLE; }
"INSERT"                     { return INSERT; }
"DELETE"                     { return DELETE; }
"FROM"                     { return FROM; }
"INTO"                     { return INTO; }
"WHERE"                     { return WHERE; }
"VALUES"                     { return VALUES; }
"SHOW"                     { return SHOW; }
"TABLES"                     { return TABLES; }
"TABLE"                     { return TABLE; }
"AND"                     { return AND; }
"OR"                     { return OR; }
"NOT"                     { return NOT; }
"NUMBER"                     { value = "NUMBER_TYPE"; return NUMBER_TYPE; }
"VARCHAR"                     { value = "VARCHAR"; return VARCHAR; }
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
