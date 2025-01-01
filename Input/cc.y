%{
%}

%token <int>         INT_VALUE
%token <string>      RETURN ID INT_TYPE VOID_TYPE

%type <CCompilerNs.Program>           program 
%type <CCompilerNs.FunDecl>           funDecl
%type <CCompilerNs.Statement>         statement
%type <CCompilerNs.ReturnStatement>   returnStatement 
%type <CCompilerNs.Expression>        addExpression mulExpression
%type <string>                        typeSpec

%%
program: 
funDecl 
{
    $$= CCompilerNs.CCLexYaccCallback.Program($1);
};

funDecl:
typeSpec ID '(' ')' '{' statement '}'
{  
    $$= CCompilerNs.CCLexYaccCallback.FuncDecl($1, $2, $6);
}
;

typeSpec:
INT_TYPE 
{
    $$ = $1;
}
;

statement: 
returnStatement
{     
    $$ = $1;
}
;

returnStatement:
RETURN ';'
{
    $$= CCompilerNs.CCLexYaccCallback.ReturnStatement(null);
}
|
RETURN addExpression ';'
{
    $$= CCompilerNs.CCLexYaccCallback.ReturnStatement($2);
};

addExpression: 
addExpression '+' mulExpression
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, "+", $3);
}
| addExpression '-' mulExpression
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, "-", $3);
}
|
mulExpression
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1);
}
;  

mulExpression: 
mulExpression '*' INT_VALUE
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, "*", $3);
}
| mulExpression '/' INT_VALUE
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, "/", $3);
}
| mulExpression '%' INT_VALUE
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, "%", $3);
}
|
mulExpression '*' '(' addExpression ')' 
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, "*", $4);
}
| mulExpression '/' '(' addExpression ')' 
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, "/", $4);
}
|
'(' addExpression ')'
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($2);
}
|
INT_VALUE
{   
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1);
}
;

%%