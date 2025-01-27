%{
// comment
%}

%%
(\-)?[0-9]+ { 
            value = int.Parse(yytext);
            return DOUBLE;
}
[ \t\n]+   {}
"+"  {return '+';}
"-"  {return '-';}
"*"  {return '*';}
"/"  {return '/';}
%%