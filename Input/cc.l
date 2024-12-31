%{
%}

%% 
"int"                     { return INT; }
"return"                  { return RETURN; }
(\-)?[0-9]+               { value = int.Parse(yytext); return INT_VALUE; }
[_a-zA-Z][a-zA-Z0-9]*     { value = yytext; return ID; }
[ \t\n\r]+                {}

"{"                       { return '{'; }
"}"                       { return '}'; }
"("                       { return '('; }
")"                       { return ')'; }
";"                       { return ';'; }
"+"                       { return '+'; }
"-"                       { return '-'; }
"*"                       { return '*'; }
"/"                       { return '/'; }
"="                       { return '='; }
"%"                       { return '%'; }
","                       { return ','; }
%%