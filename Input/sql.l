%{
%}

%%
"SELECT"                     { return SELECT; }
"CREATE"                     { return CREATE; }
"TABLE"                     { return TABLE; }
"INSERT"                     { return INSERT; }
"INTO"                     { return INTO; }
"VALUES"                     { return VALUES; }
"NUMBER"                     { value = "NUMBER"; return NUMBER; }
"VARCHAR"                     { value = "VARCHAR"; return VARCHAR; }
"{"  { return '{'; }
"}"  { return '}'; }
"("  { return '('; }
")"  { return ')'; }
","  { return ','; }

[a-zA-Z0-9_]*      { value = yytext; return STRING; }
[ \t\n]                      {}
%%