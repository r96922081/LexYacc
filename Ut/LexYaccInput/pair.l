%{
%}

%%
[a-zA-Z_][a-zA-Z0-9_]*      { value = yytext; return KEY; }
[0-9]+                       { value = yytext; return VALUE; }
"="                          { return EQUALS; }
[ \t\n]                      {}
%%