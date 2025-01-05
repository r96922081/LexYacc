%{
%}

%token <int>         INT_VALUE
%token <string>      RETURN ID INT_TYPE VOID_TYPE IF ELSE EQUAL_SIGN NOT_EQUAL_SIGN LESS_OR_EQUAL_SIGN GREATER_OR_EQUAL_SIGN FOR BREAK CONTINUE

%type <CCompilerNs.Program>                    program 
%type <List<CCompilerNs.FunDecl>>              funDecls
%type <CCompilerNs.FunDecl>                    funDecl
%type <List<CCompilerNs.LocalVariable>>        funcParams
%type <List<CCompilerNs.Statement>>            statements 
%type <CCompilerNs.ForLoopStatement>           forLoopStatement
%type <List<CCompilerNs.Statement>>            forLoopBody
%type <CCompilerNs.Statement>                  statement
%type <CCompilerNs.ReturnStatement>            returnStatement 
%type <CCompilerNs.DeclareStatement>           declareStatement
%type <CCompilerNs.AssignmentStatement>        assignmentStatement
%type <CCompilerNs.FunctionCallExpression>     functionCallExpression
%type <CCompilerNs.FunctionCallExpression>     functionCallStatement
%type <CCompilerNs.CompoundIfStatement>        compoundIfStatement
%type <CCompilerNs.IfStatement>                ifStatement elseIfStatement elseStatement
%type <List<CCompilerNs.IfStatement>>          elseIfStatements
%type <List<CCompilerNs.Expression>>           funcCallParams
%type <CCompilerNs.Expression>                 addExpression mulExpression
%type <string>                                 typeSpec relationlOp

%%
program: 
funDecls 
{
    $$= CCompilerNs.CCLexYaccCallback.Program($1);
}
;

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
    $$= CCompilerNs.CCLexYaccCallback.FuncDecl($1, $2, null, $6);
}
|
typeSpec ID '(' funcParams ')' '{' statements '}'
{  
    $$= CCompilerNs.CCLexYaccCallback.FuncDecl($1, $2, $4, $7);
}
;

typeSpec:
INT_TYPE 
{
    $$ = $1;
}
|
VOID_TYPE
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
|
functionCallStatement
{
    $$ = $1;
}
|
compoundIfStatement
{
    $$ = $1;
}
|
forLoopStatement
{
    $$ = $1;
}
;

compoundIfStatement:
ifStatement
{
    $$ = CCompilerNs.CCLexYaccCallback.CompoundIfStatement($1);
}
|
ifStatement elseIfStatements
{
    $$ = CCompilerNs.CCLexYaccCallback.CompoundIfStatement($1, $2);
}
|
ifStatement elseIfStatements elseStatement
{
    $$ = CCompilerNs.CCLexYaccCallback.CompoundIfStatement($1, $2, $3);
}
|
ifStatement elseStatement
{
    $$ = CCompilerNs.CCLexYaccCallback.CompoundIfStatement($1, $2);
}
;

ifStatement:
IF '(' addExpression relationlOp addExpression ')' '{' statements '}'
{
    $$ = CCompilerNs.CCLexYaccCallback.IfStatement($3, $4, $5, $8);
}
;

elseIfStatements:
elseIfStatements elseIfStatement 
{
    $$ = CCompilerNs.CCLexYaccCallback.ElseIfStatements($1, $2);
}
|
elseIfStatement
{
    $$ = CCompilerNs.CCLexYaccCallback.ElseIfStatements(null, $1);
}
;

elseIfStatement:
ELSE IF '(' addExpression relationlOp addExpression ')' '{' statements '}'
{
    $$= CCompilerNs.CCLexYaccCallback.IfStatement($4, $5, $6, $9);
}
;

elseStatement:
ELSE '{' statements '}'
{
    $$= CCompilerNs.CCLexYaccCallback.IfStatement(null, null, null, $3);
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
| mulExpression '*' functionCallExpression
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, "*", $3);
}
| mulExpression '/' functionCallExpression
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, "/", $3);
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
functionCallExpression
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1);
}
;

funcParams:
funcParams ',' typeSpec ID
{
    $$ = CCompilerNs.CCLexYaccCallback.FuncParams($3, $4, $1);
}
|
typeSpec ID
{
    $$ = CCompilerNs.CCLexYaccCallback.FuncParams($1, $2, null);
}
;

funcCallParams:
funcCallParams ',' addExpression
{
    $$ = CCompilerNs.CCLexYaccCallback.FuncCallParams($3, $1);
}
|
addExpression
{
    $$ = CCompilerNs.CCLexYaccCallback.FuncCallParams($1, null);
}
;

functionCallStatement:
functionCallExpression ';'
{
    $$ = $1;
}
;

functionCallExpression:
ID '(' ')'
{
    $$ = CCompilerNs.CCLexYaccCallback.FunctionCallExpression($1, null);
}
|
ID '(' funcCallParams ')'
{
    $$ = CCompilerNs.CCLexYaccCallback.FunctionCallExpression($1, $3);
}
;

forLoopStatement:
FOR '(' declareStatement addExpression relationlOp addExpression ';' ID '=' addExpression ')' '{' forLoopBody '}'
{
    $$ = CCompilerNs.CCLexYaccCallback.ForLoopStatement($3, $4, $5, $6, $8, $10, $13);
}
;

forLoopBody:
forLoopBody BREAK ';'
{
    $$ = CCompilerNs.CCLexYaccCallback.ForLoopBody("break", $1);
}
|
forLoopBody CONTINUE ';'
{
    $$ = CCompilerNs.CCLexYaccCallback.ForLoopBody("continue", $1);
}
|
forLoopBody statements
{
    $$ = CCompilerNs.CCLexYaccCallback.ForLoopBody($2, $1);
}
|
statements
{
    $$ = CCompilerNs.CCLexYaccCallback.ForLoopBody((string)null, $1);
}
|
BREAK ';'
{
    $$ = CCompilerNs.CCLexYaccCallback.ForLoopBody("break", null);
}
|
CONTINUE ';'
{
    $$ = CCompilerNs.CCLexYaccCallback.ForLoopBody("continue", null);
}
;

relationlOp:
'<'
{
    $$ = "<";
}
|
'>'
{
    $$ = ">";
}
|
EQUAL_SIGN
{
    $$ = "==";
}
|
NOT_EQUAL_SIGN
{
    $$ = "!=";
}
|
LESS_OR_EQUAL_SIGN
{
    $$ = "<=";
}
|
GREATER_OR_EQUAL_SIGN
{
    $$ = ">=";
}

%%