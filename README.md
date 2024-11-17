
## LexYacc

**Create a Calculator:**

   cal.l :
   
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

   cal.y :
   

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

Generate code: it will create cal.cs 

    LexYaccNs.LexYaccCodeGen.GenCode("cal.l", "cal.y", "D:/", "cal");

Use cal.cs

    cal.Parse("2 * 3 + 6 / 3");

Output
    
    Result = 8
