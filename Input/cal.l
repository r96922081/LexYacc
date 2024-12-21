%{
// comment
%}

%%
(\-)?[0-9]+ { 
            value = int.Parse(yytext);
            return NUMBER_DOUBLE;
}
[ \t\n]+   {}
"+"  {return '+';}
"-"  {return '-';}
"*"  {return '*';}
"/"  {return '/';}
%%