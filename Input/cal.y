%{
%}
%token <int> NUMBER
%type <int> cal exp term
%%
cal: exp {$$ = $1; Console.WriteLine("Result = " + $1); };
 
exp:
  exp '+' term {$$ = $1 + $3;}
  | exp '-' term {$$ = $1 - $3;}
  | term {$$ = $1;}
  ;
  
term:
  term '*' NUMBER { $$ = $1 * $3;}
  | term '/' NUMBER { $$ = $1 / $3;}
  | NUMBER {$$ = $1;}
  ;
%%