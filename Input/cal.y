%{
%}
%token <int> NUMBER_DOUBLE
%type <int> cal exp term
%%
cal: exp {$$ = $1; Console.WriteLine("Result = " + $1); };
 
exp:
  exp '+' term {$$ = $1 + $3;}
  | exp '-' term {$$ = $1 - $3;}
  | term {$$ = $1;}
  ;
  
term:
  term '*' NUMBER_DOUBLE { $$ = $1 * $3;}
  | term '/' NUMBER_DOUBLE { $$ = $1 / $3;}
  | NUMBER_DOUBLE {$$ = $1;}
  ;
%%