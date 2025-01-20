%{
%}

%token <char>        CHAR_VALUE
%token <int>         INT_VALUE
%token <string>      RETURN ID INT_TYPE VOID_TYPE IF ELSE EQUAL_SIGN NOT_EQUAL_SIGN LESS_OR_EQUAL_SIGN GREATER_OR_EQUAL_SIGN FOR BREAK CONTINUE INCREMENT DECREMENT PLUS_ASSIGN MINUS_ASSIGN MULTIPLY_ASSIGN DIVIDE_ASSIGN CHAR_TYPE SINGLE_LINE_COMMENT STRUCT

%type <CCompilerNs.Program>                    program
%type <CCompilerNs.GlobalDeclare>              globalDeclare
%type <CCompilerNs.FunctionDeclare>            functionDeclare
%type <List<CCompilerNs.Variable>>             functionParams
%type <List<CCompilerNs.Statement>>            statements ifBodyStatements
%type <CCompilerNs.ForLoopStatement>           forLoopStatement
%type <CCompilerNs.Statement>                  statement
%type <CCompilerNs.ReturnStatement>            returnStatement 
%type <CCompilerNs.DeclareStatement>           declareStatement 
%type <CCompilerNs.GlobalVariable>             globalVariable
%type <CCompilerNs.AssignmentStatement>        assignmentStatement assignmentNoSemicolon
%type <CCompilerNs.FunctionCallExpression>     functionCallExpression
%type <CCompilerNs.FunctionCallExpression>     functionCallStatement
%type <CCompilerNs.CompoundIfStatement>        compoundIfStatement
%type <CCompilerNs.IfStatement>                ifStatement elseIfStatement elseStatement
%type <List<CCompilerNs.IfStatement>>          elseIfStatements
%type <CCompilerNs.BreakStatement>             breakStatement
%type <CCompilerNs.ContinueStatement>          continueStatement
%type <CCompilerNs.EmptyStatement>             emptyStatement singleLineComment
%type <List<CCompilerNs.Expression>>           funcCallParams
%type <CCompilerNs.Expression>                 addExpression mulExpression
%type <List<int>>                              arraySize paramArraySize
%type <List<CCompilerNs.Expression>>           arrayIndex
%type <CCompilerNs.StructDef>                  structDef
%type <List<CCompilerNs.StructField>>          structFields
%type <CCompilerNs.StructField>                structField
%type <string>                                 typeSpec relationlOp opAssign incrementDecrement


%%
program:
program globalDeclare
{
    $$= CCompilerNs.LexYaccCallback.Program($1, $2);
}
|
globalDeclare
{
    $$= CCompilerNs.LexYaccCallback.Program(null, $1);
}
;


globalDeclare:
globalVariable
{
    $$ = $1;
}
|
functionDeclare
{
    $$ = $1;
}
|
structDef
{
    $$ = $1;
}
|
SINGLE_LINE_COMMENT
{
    $$ = null;
}
;

functionDeclare:
typeSpec ID '(' ')' '{' statements '}'
{  
    $$= CCompilerNs.LexYaccCallback.FuncDecl($1, $2, null, $6);
}
|
typeSpec ID '(' functionParams ')' '{' statements '}'
{  
    $$= CCompilerNs.LexYaccCallback.FuncDecl($1, $2, $4, $7);
}
typeSpec ID '(' ')' '{' '}'
{  
    $$= CCompilerNs.LexYaccCallback.FuncDecl($1, $2, null, null);
}
|
typeSpec ID '(' functionParams ')' '{' '}'
{  
    $$= CCompilerNs.LexYaccCallback.FuncDecl($1, $2, $4, null);
}
;

typeSpec:
INT_TYPE 
{
    $$ = $1;
}
|
CHAR_TYPE
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
    $$= CCompilerNs.LexYaccCallback.Statements($2, $1);
}
|
statement
{
    $$= CCompilerNs.LexYaccCallback.Statements($1, null);
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
|
breakStatement
{
    $$ = $1;
}
|
continueStatement
{
    $$ = $1;
}
|
emptyStatement
{
    $$ = $1;
}
|
singleLineComment
{
    $$ = $1;
}
;

compoundIfStatement:
ifStatement
{
    $$ = CCompilerNs.LexYaccCallback.CompoundIfStatement($1);
}
|
ifStatement elseIfStatements
{
    $$ = CCompilerNs.LexYaccCallback.CompoundIfStatement($1, $2);
}
|
ifStatement elseIfStatements elseStatement
{
    $$ = CCompilerNs.LexYaccCallback.CompoundIfStatement($1, $2, $3);
}
|
ifStatement elseStatement
{
    $$ = CCompilerNs.LexYaccCallback.CompoundIfStatement($1, $2);
}
;

ifStatement:
IF '(' addExpression relationlOp addExpression ')' ifBodyStatements
{
    $$ = CCompilerNs.LexYaccCallback.IfStatement($3, $4, $5, $7);
}
;

elseIfStatements:
elseIfStatements elseIfStatement 
{
    $$ = CCompilerNs.LexYaccCallback.ElseIfStatements($1, $2);
}
|
elseIfStatement
{
    $$ = CCompilerNs.LexYaccCallback.ElseIfStatements(null, $1);
}
;

elseIfStatement:
ELSE IF '(' addExpression relationlOp addExpression ')' ifBodyStatements
{
    $$= CCompilerNs.LexYaccCallback.IfStatement($4, $5, $6, $8);
}
;

elseStatement:
ELSE ifBodyStatements
{
    $$= CCompilerNs.LexYaccCallback.IfStatement(null, null, null, $2);
}
;

ifBodyStatements:
'{' statements '}'
{
    $$ = $2;
}
|
statement
{
    $$ = new List<CCompilerNs.Statement>();
    $$.Add($1);
}
|
'{' '}'
{
    $$ = new List<CCompilerNs.Statement>();
}
;

returnStatement:
RETURN ';'
{
    $$= CCompilerNs.LexYaccCallback.ReturnStatement(null);
}
|
RETURN addExpression ';'
{
    $$= CCompilerNs.LexYaccCallback.ReturnStatement($2);
}
;

declareStatement:
typeSpec ID '=' addExpression ';'
{
    $$= CCompilerNs.LexYaccCallback.DeclareStatement($1, $2, $4, null);
}
|
typeSpec ID ';'
{
    $$= CCompilerNs.LexYaccCallback.DeclareStatement($1, $2, null, null);
}
|
typeSpec ID arraySize ';'
{
    $$= CCompilerNs.LexYaccCallback.DeclareStatement($1, $2, null, $3);
}
|
STRUCT ID ID ';'
{
    $$= CCompilerNs.LexYaccCallback.DeclareStatement($1 + " " + $2, $3, null, null);
}
;

assignmentStatement:
assignmentNoSemicolon ';'
{
    $$ = $1;
}
;

emptyStatement:
';'
{
    $$= CCompilerNs.LexYaccCallback.EmptyStatement();
}
;

singleLineComment:
SINGLE_LINE_COMMENT
{
    $$= CCompilerNs.LexYaccCallback.EmptyStatement();
}
;

assignmentNoSemicolon: 
ID '=' addExpression
{
    $$= CCompilerNs.LexYaccCallback.AssignmentStatement($1, $3, null);
}
ID arrayIndex '=' addExpression
{
    $$= CCompilerNs.LexYaccCallback.AssignmentStatement($1, $4, $2);
}
|
ID opAssign addExpression
{
    $$= CCompilerNs.LexYaccCallback.OpAssignmentStatement($1, $3, null, $2);
}
|
ID arrayIndex opAssign addExpression
{
    $$= CCompilerNs.LexYaccCallback.OpAssignmentStatement($1, $4, $2, $3);
}
|
ID incrementDecrement
{
    $$= CCompilerNs.LexYaccCallback.IncrementDecrement($1, null, $2);
}
|
ID arrayIndex incrementDecrement
{
    $$= CCompilerNs.LexYaccCallback.IncrementDecrement($1, $2, $3);
}
;

opAssign:
PLUS_ASSIGN
{
    $$ = $1;
}
|
MINUS_ASSIGN
{
    $$ = $1;
}
|
MULTIPLY_ASSIGN
{
    $$ = $1;
}
|
DIVIDE_ASSIGN
{
    $$ = $1;
}
;

incrementDecrement:
INCREMENT
{
    $$ = $1;
}
|
DECREMENT
{
    $$ = $1;
}
;

addExpression: 
addExpression '+' mulExpression
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, "+", $3);
}
| addExpression '-' mulExpression
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, "-", $3);
}
|
mulExpression
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1);
}
;  

mulExpression: 
mulExpression '*' INT_VALUE
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, "*", $3);
}
| mulExpression '/' INT_VALUE
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, "/", $3);
}
| mulExpression '*' CHAR_VALUE
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, "*", $3);
}
| mulExpression '/' CHAR_VALUE
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, "/", $3);
}
| mulExpression '*' ID
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, "*", $3);
}
| mulExpression '/' ID
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, "/", $3);
}
| mulExpression '*' functionCallExpression
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, "*", $3);
}
| mulExpression '/' functionCallExpression
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, "/", $3);
}
|
mulExpression '*' '(' addExpression ')' 
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, "*", $4);
}
| mulExpression '/' '(' addExpression ')' 
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, "/", $4);
}
|
'(' addExpression ')'
{
    $$ = CCompilerNs.LexYaccCallback.Expression($2);
}
|
INT_VALUE
{   
    $$ = CCompilerNs.LexYaccCallback.Expression($1);
}
|
ID
{   
    $$ = CCompilerNs.LexYaccCallback.Expression($1);
}
ID arrayIndex
{   
    $$ = CCompilerNs.LexYaccCallback.Expression($1, $2);
}
|
functionCallExpression
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1);
}
|
CHAR_VALUE
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1);
}
;

functionParams:
functionParams ',' typeSpec ID
{
    $$ = CCompilerNs.LexYaccCallback.FuncParams($3, $4, $1);
}
|
typeSpec ID
{
    $$ = CCompilerNs.LexYaccCallback.FuncParams($1, $2, null);
}
|
functionParams ',' typeSpec paramArraySize ID
{
    $$ = CCompilerNs.LexYaccCallback.FuncParamsArray($3, $5, $4, $1);
}
|
typeSpec paramArraySize ID
{
    $$ = CCompilerNs.LexYaccCallback.FuncParamsArray($1, $3, $2, null);
}
;

funcCallParams:
funcCallParams ',' addExpression
{
    $$ = CCompilerNs.LexYaccCallback.FuncCallParams($3, $1);
}
|
addExpression
{
    $$ = CCompilerNs.LexYaccCallback.FuncCallParams($1, null);
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
    $$ = CCompilerNs.LexYaccCallback.FunctionCallExpression($1, null);
}
|
ID '(' funcCallParams ')'
{
    $$ = CCompilerNs.LexYaccCallback.FunctionCallExpression($1, $3);
}
;

breakStatement:
BREAK ';'
{
    $$ = CCompilerNs.LexYaccCallback.BreakStatement();
}
;

continueStatement:
CONTINUE ';'
{
    $$ = CCompilerNs.LexYaccCallback.ContinueStatement();
}
;

forLoopStatement:
FOR '(' assignmentStatement addExpression relationlOp addExpression ';' assignmentNoSemicolon ')' ifBodyStatements
{
    $$ = CCompilerNs.LexYaccCallback.ForLoopStatement($3, $4, $5, $6, $8, $10);
}
;

arraySize:
arraySize '[' INT_VALUE ']'
{
    $$ = CCompilerNs.LexYaccCallback.ArraySize($3, $1);
}
|
'[' INT_VALUE ']'
{
    $$ = CCompilerNs.LexYaccCallback.ArraySize($2, null);
}
;

paramArraySize:
'[' ']'
{
    $$ = CCompilerNs.LexYaccCallback.ParamArraySize(null);
}
|
'[' ']' arraySize
{
    $$ = CCompilerNs.LexYaccCallback.ParamArraySize($3);
}
;

arrayIndex:
arrayIndex '[' addExpression ']'
{
    $$ = CCompilerNs.LexYaccCallback.ArrayIndex($3, $1);
}
|
'[' addExpression ']'
{
    $$ = CCompilerNs.LexYaccCallback.ArrayIndex($2, null);
}
;

globalVariable:
typeSpec ID ';'
{
    $$= CCompilerNs.LexYaccCallback.GlobalVariable($1, $2, null, null);
}
|
typeSpec ID arraySize ';'
{
    $$= CCompilerNs.LexYaccCallback.GlobalVariable($1, $2, null, $3);
}
|
typeSpec ID '=' INT_VALUE ';'
{
    $$= CCompilerNs.LexYaccCallback.GlobalVariable($1, $2, $4, null);
}
|
typeSpec ID '=' CHAR_VALUE ';'
{
    $$= CCompilerNs.LexYaccCallback.GlobalVariable($1, $2, $4, null);
}
;

structDef:
STRUCT ID '{' structFields '}' ';'
{
    $$= CCompilerNs.LexYaccCallback.StructDef($2, $4);
}
;

structFields:
structFields structField
{
    $$= CCompilerNs.LexYaccCallback.StructFields($1, $2);
}
|
structField
{
    $$= CCompilerNs.LexYaccCallback.StructFields(null, $1);
}
;

structField:
typeSpec ID ';'
{
    $$= CCompilerNs.LexYaccCallback.StructField($1, $2, null);
}
|
typeSpec ID arraySize ';'
{
    $$= CCompilerNs.LexYaccCallback.StructField($1, $2, $3);
}
|
STRUCT ID ID ';'
{
    $$= CCompilerNs.LexYaccCallback.StructField("struct " + $2, $3, null);
}
|
STRUCT ID ID arraySize ';'
{
    $$= CCompilerNs.LexYaccCallback.StructField("struct " + $2, $3, $4);
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
    $$ = $1;
}
|
NOT_EQUAL_SIGN
{
    $$ = $1;
}
|
LESS_OR_EQUAL_SIGN
{
    $$ = $1;
}
|
GREATER_OR_EQUAL_SIGN
{
    $$ = $1;
}

%%