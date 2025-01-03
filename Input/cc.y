%{
%}

%token <int>         INT_VALUE
%token <string>      RETURN ID INT_TYPE VOID_TYPE

%type <CCompilerNs.Program>                    program 
%type <List<CCompilerNs.FunDecl>>              funDecls
%type <CCompilerNs.FunDecl>                    funDecl
%type <List<CCompilerNs.Statement>>            statements
%type <CCompilerNs.Statement>                  statement
%type <CCompilerNs.ReturnStatement>            returnStatement 
%type <CCompilerNs.DeclareStatement>           declareStatement
%type <CCompilerNs.AssignmentStatement>        assignmentStatement
%type <CCompilerNs.Expression>                 addExpression mulExpression
%type <string>                                 typeSpec functionCall

%%
program: 
funDecls 
{
    $$= CCompilerNs.CCLexYaccCallback.Program($1);
};

funDecls:
funDecls funDecl
{
    $$= CCompilerNs.CCLexYaccCallback.FunDecls($2, $1);
}
|
funDecl
{
    $$= CCompilerNs.CCLexYaccCallback.FunDecls($1, null);
}
;

funDecl:
typeSpec ID '(' ')' '{' statements '}'
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

statements:
statements statement
{
    $$= CCompilerNs.CCLexYaccCallback.Statements($2, $1);
}
|
statement
{
    $$= CCompilerNs.CCLexYaccCallback.Statements($1, null);
}
;

statement: 
returnStatement
{     
    $$ = $1;
}
|
declareStatement
{
    $$ = $1;
}
|
assignmentStatement
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

declareStatement:
typeSpec ID '=' addExpression ';'
{
    $$= CCompilerNs.CCLexYaccCallback.DeclareStatement($1, $2, $4);
}
|
typeSpec ID ';'
{
    $$= CCompilerNs.CCLexYaccCallback.DeclareStatement($1, $2, null);
}
;

assignmentStatement:
ID '=' addExpression ';'
{
    $$= CCompilerNs.CCLexYaccCallback.AssignmentStatement($1, $3);
}
;

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
| mulExpression '*' ID
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, "*", $3);
}
| mulExpression '/' ID
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, "/", $3);
}
| mulExpression '*' functionCall
{
    $$ = CCompilerNs.CCLexYaccCallback.ExpressionCallFunc($1, "*", $3);
}
| mulExpression '/' functionCall
{
    $$ = CCompilerNs.CCLexYaccCallback.ExpressionCallFunc($1, "/", $3);
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
|
ID
{   
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1);
}
|
functionCall
{
    $$ = CCompilerNs.CCLexYaccCallback.ExpressionCallFunc($1);
}
;

functionCall:
ID '(' ')'
{
    $$ = $1;
}
;

%%