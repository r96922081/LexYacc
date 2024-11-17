%{
// comment
%}

%%
(\-)?[0-9]+ { 
            value = int.Parse(yytext);
            return NUMBER;
}
[ \t\n]+   {}
"+"  {return '+';}
"-"  {return '-';}
"*"  {return '*';}
"/"  {return '/';}
%%