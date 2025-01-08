%{
%}

%% 
"int"                     { value = "int"; return INT_TYPE; }
"void"                    { value = "void"; return VOID_TYPE; }
"return"                  { value = "return"; return RETURN; }
"if"                      { value = "if"; return IF; }
"else"                    { value = "else"; return ELSE; }
"for"                     { value = "for"; return FOR; }
"break"                   { value = "break"; return BREAK; }
"continue"                { value = "continue"; return CONTINUE; }
"=="                      { value = "=="; return EQUAL_SIGN; }
"!="                      { value = "!="; return NOT_EQUAL_SIGN; }
"<="                      { value = "<="; return LESS_OR_EQUAL_SIGN; }
">="                      { value = ">="; return GREATER_OR_EQUAL_SIGN; }
[0-9]+                    { value = int.Parse(yytext); return INT_VALUE; }
[_a-zA-Z][a-zA-Z0-9]*     { value = yytext; return ID; }
[ \t\n\r]+                {}

"{"                       { return '{'; }
"}"                       { return '}'; }
"["                       { return '['; }
"]"                       { return ']'; }
"<"                       { return '<'; }
">"                       { return '>'; }
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