
## Create simple C language parser to generate syntax tree

**c_grammar.l:**  

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

**c_grammar.y:** 
   

    %{
    
    public class c_grammar_gv{
        public static AstNode root;
    }
    
    %}
    
    %token <int> CONSTANT
    %token <string>  VOID INT  ID RETURN
    %type <string>   program
    %type <AstNode>  declList decl funDecl typeSpec returnStmt funName param paramList id constant 
    %type <AstNode>  primaryExpression addExpression mulExpression
    %type <AstNode>  compoundStatement statementList statement expressionStatement assignmentExpression
    
    %%
    program: declList
      {
        c_grammar_gv.root = new AstNode("program");
        c_grammar_gv.root.children.Add($1);
        c_grammar_gv.root.Print();
        return c_grammar_gv.root.GetPrintString();
      };
    declList:
      declList decl
      {
         $1.children.Add($2); 
         $$ = new AstNode("declList");
         $$.children.AddRange($1.children);
      }
      |
      decl
      {
        $$ = new AstNode("declList");
        $$.children.Add($1);  
      };
    decl:
      funDecl
      {
        $$ = new AstNode("decl");
        $$.children.Add($1);
      };
    funDecl:
      typeSpec funName '(' ')' compoundStatement
      {
        $$ = new AstNode("funDecl");
        $$.children.Add($1);   
        $$.children.Add($2);
        $$.children.Add($5);
      }
      |
      typeSpec funName '(' paramList ')' compoundStatement
      {
        $$ = new AstNode("funDecl");
        $$.children.Add($1);   
        $$.children.Add($2);
        $$.children.Add($4);
        $$.children.Add($6);
      }
      |
      typeSpec funName '(' ')' compoundStatement
      {
        $$ = new AstNode("funDecl");
        $$.children.Add($1);   
        $$.children.Add($2);
        $$.children.Add($5);
      };
    paramList: 
      paramList ',' typeSpec id 
      {
         $1.children.Add($3); 
         $1.children.Add($4);
         $$ = new AstNode("paramList");
         $$.children.AddRange($1.children); 
      }
      |
      typeSpec id 
      {
        $$ = new AstNode("param");
        $$.children.Add($1); 
        $$.children.Add($2);
      };
    funName:
        id
        {
        $$ = new AstNode("funName");
        $$.children.Add($1);
        }
        ;
    returnStmt:
      RETURN ';'
      {
        $$ = new AstNode("returnStmt");
        $$.children.Add(new AstNode("return"));
      }
      |
      RETURN addExpression ';'
      {
        $$ = new AstNode("returnStmt");
        $$.children.Add(new AstNode("return"));
        $$.children.Add($2);
      };
    typeSpec:
      INT 
      {
        $$ = new AstNode("typeSpec");
        $$.children.Add(new AstNode("int"));  
      }
      | 
      VOID
      {
        $$ = new AstNode("typeSpec");
        $$.children.Add(new AstNode("void"));    
      };
    id: 
      ID
      {
        $$ = new AstNode("id");
        $$.children.Add(new AstNode($1));
      };
    constant:
      CONSTANT
      {
        $$ = new AstNode("constant");
        $$.children.Add(new AstNode("" + $1));
      };
    primaryExpression: 
      id
      {
        $$ = new AstNode("primaryExpression");
        $$.children.Add($1);    
      }
      | 
      constant
      {
        $$ = new AstNode("primaryExpression");
        $$.children.Add($1);      
      };
    
    mulExpression: 
        primaryExpression
        {
            $$ = new AstNode("mulExpression");
            $$.children.Add($1);       
        }
    	| mulExpression '*' primaryExpression
        {
            $$ = new AstNode("mulExpression");
            $$.children.Add($1); 
            $$.children.Add(new AstNode("*")); 
            $$.children.Add($3); 
        }
    	| mulExpression '/' primaryExpression
        {
            $$ = new AstNode("mulExpression");
            $$.children.Add($1); 
            $$.children.Add(new AstNode("/")); 
            $$.children.Add($3); 
        }
    	| mulExpression '%' primaryExpression
        {
            $$ = new AstNode("mulExpression");
            $$.children.Add($1); 
            $$.children.Add(new AstNode("%")); 
            $$.children.Add($3); 
        }
    	;
    
    addExpression: 
        mulExpression
        {
            $$ = new AstNode("addExpression");
            $$.children.Add($1);     
        }
    	| addExpression '+' mulExpression
        {
            $$ = new AstNode("addExpression");
            $$.children.Add($1); 
            $$.children.Add(new AstNode("+")); 
            $$.children.Add($3); 
        }
    	| addExpression '-' mulExpression
        {
            $$ = new AstNode("addExpression");
            $$.children.Add($1); 
            $$.children.Add(new AstNode("-")); 
            $$.children.Add($3); 
        }
    	;  
    
    compoundStatement: 
        '{' '}'
        {
            $$ = new AstNode("compoundStatement");
        }
    	| 
        '{' statementList '}'
        {
             $$ = new AstNode("compoundStatement");
             $$.children.Add($2);      
        }
    	;
    statementList: 
        statement
        {
            $$ = new AstNode("statementList");
            $$.children.Add($1);        
        }
    	| 
        statementList statement
        {
             $1.children.Add($2); 
             $$ = new AstNode("statementList");
             $$.children.AddRange($1.children);    
        }
    	;
    
    statement: 
        compoundStatement
        {
            $$ = new AstNode("statement");
            $$.children.Add($1);        
        }
    	| 
        assignmentExpression
        {
            $$ = new AstNode("statement");
            $$.children.Add($1);       
        }
    	| 
        returnStmt
        {
            $$ = new AstNode("statement");
            $$.children.Add($1);       
        }
        |
        expressionStatement
        {
            $$ = new AstNode("statement");
            $$.children.Add($1);       
        }
    	;
    
    assignmentExpression:
        typeSpec id '=' expressionStatement
        {
            $$ = new AstNode("assignmentExpression");
            $$.children.Add($1);    
            $$.children.Add($2); 
            $$.children.Add(new AstNode("=")); 
            $$.children.Add($4); 
        }
        ;
    expressionStatement: 
        ';'
        {
            $$ = new AstNode("expressionStatement");
        }
    	| 
        addExpression ';'
        {
            $$ = new AstNode("expressionStatement");
            $$.children.Add($1);       
        }
    	;
    
    %%


**Generate code: it will create c_grammar.cs**

    LexYaccNs.LexYaccCodeGen.GenCode("c_grammar.l", "c_grammar.y", "D:/", "c_grammar");

**Use c_grammar.cs + hand-written AstNode.cs**

            string cCode = @"
    int main(int a) 
    {
        int b = 3;
        return a * b - 4;
    } 
    ";
            c_grammar.Parse(cCode);

**Output**
[![](https://r96922081.github.io/images/LexYacc/syntax_tree.png)]


## Create a calculator

**cal.l:**

   
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

**cal.y:**
   

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

**Generate code: it will create cal.cs**

    LexYaccNs.LexYaccCodeGen.GenCode("cal.l", "cal.y", "D:/", "cal");

**Use cal.cs**

    cal.Parse("2 * 3 + 6 / 3");

**Output**
    
    Result = 8


