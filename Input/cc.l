%{
%}

%% 
"int"                     { value = "int"; return INT_TYPE; }
"void"                    { value = "void"; return VOID_TYPE; }
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