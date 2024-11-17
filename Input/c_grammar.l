%{
%}

%%
"void" { return VOID; }
"int" { return INT; }
(\-)?[0-9]+ { 
            value = int.Parse(yytext);
            return CONSTANT;
}
"return" { return RETURN; }
[_a-zA-Z][a-zA-Z0-9]* { value = yytext; return ID; }
[ \t\n\r]+   {}
"{"  { return '{'; }
"}"  { return '}'; }
"("  { return '('; }
")"  { return ')'; }
";"  { return ';'; }
"+"  {return '+';}
"-"  {return '-';}
"*"  {return '*';}
"/"  {return '/';}
"="  { return '='; }
"%"  { return '%'; }
","  { return ','; }
%%