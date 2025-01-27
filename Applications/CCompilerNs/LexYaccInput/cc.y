%{
%}

%token <char>        CHAR_VALUE
%token <int>         INT_VALUE
%token <string>      RETURN ID INT_TYPE VOID_TYPE IF ELSE EQUAL_SIGN NOT_EQUAL_SIGN LESS_OR_EQUAL_SIGN GREATER_OR_EQUAL_SIGN FOR WHILE BREAK CONTINUE INCREMENT DECREMENT PLUS_ASSIGN MINUS_ASSIGN MULTIPLY_ASSIGN DIVIDE_ASSIGN CHAR_TYPE STRUCT STRING_LITERAL LOGICAL_AND LOGICAL_OR ARROW

%type <CCompilerNs.Program>                    program
%type <CCompilerNs.GlobalDeclare>              globalDeclare
%type <CCompilerNs.FunctionDeclare>            functionDeclare
%type <CCompilerNs.StructDef>                  structDef

%type <List<CCompilerNs.Variable>>             functionParams
%type <List<CCompilerNs.Expression>>           functionCallParams

%type <List<CCompilerNs.Statement>>            statements ifBodyStatements functionBodyStatements
%type <CCompilerNs.Statement>                  statement
%type <CCompilerNs.ForLoopStatement>           forLoopStatement
%type <CCompilerNs.WhileLoopStatement>         whileLoopStatement
%type <CCompilerNs.ReturnStatement>            returnStatement 
%type <CCompilerNs.DeclareStatement>           declareStatement 
%type <CCompilerNs.AssignmentStatement>        assignmentStatement assignmentNoSemicolon
%type <CCompilerNs.FunctionCallExpression>     functionCallStatement
%type <CCompilerNs.CompoundIfStatement>        compoundIfStatement
%type <CCompilerNs.IfStatement>                ifStatement elseIfStatement elseStatement
%type <List<CCompilerNs.IfStatement>>          elseIfStatements
%type <CCompilerNs.BreakStatement>             breakStatement
%type <CCompilerNs.ContinueStatement>          continueStatement
%type <CCompilerNs.EmptyStatement>             emptyStatement

%type <CCompilerNs.GlobalVariable>             globalVariable
%type <List<CCompilerNs.StructField>>          structFields
%type <CCompilerNs.StructField>                structField
%type <CCompilerNs.VariableId>                 variableId AssignmentLhsVariableId
%type <CCompilerNs.Declare>                    declare functionParamDeclare

%type <CCompilerNs.FunctionCallExpression>     functionCallExpression
%type <CCompilerNs.Expression>                 addExpression mulExpression
%type <CCompilerNs.BooleanExpressions>         booleanExpressions
%type <CCompilerNs.BooleanExpression>          booleanExpression

%type <List<int>>                              arraySize
%type <List<CCompilerNs.Expression>>           arrayIndex

%type <string>                                 typeSpec relationalOp opAssign incrementDecrement addMinusOp multiplyDivideOp logicalOperation pointers


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
;

functionDeclare:
typeSpec ID '(' ')' functionBodyStatements
{  
    $$= CCompilerNs.LexYaccCallback.FuncDecl($1, $2, null, $5);
}
|
typeSpec ID '(' functionParams ')' functionBodyStatements
{  
    $$= CCompilerNs.LexYaccCallback.FuncDecl($1, $2, $4, $6);
}
;

functionBodyStatements:
'{' statements '}'
{
    $$ = $2;
}
|
'{' '}'
{
    $$ = new List<CCompilerNs.Statement>();
}
;

typeSpec:
INT_TYPE 
{
    $$ = $1;
}
|
INT_TYPE pointers
{
    $$ = $1 + $2;
}
|
CHAR_TYPE
{
    $$ = $1;
}
|
CHAR_TYPE pointers
{
    $$ = $1 + $2;
}
|
VOID_TYPE
{
    $$ = $1;
}
|
VOID_TYPE pointers
{
    $$ = $1 + $2;
}
|
STRUCT ID 
{
    $$ = $1 + " " + $2;
}
|
STRUCT ID pointers
{
    $$ = $1 + " " + $2 + $3;
}
;

pointers:
pointers '*'
{
    $$ = $1 + "*";
}
|
'*'
{
    $$ = "*";
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
whileLoopStatement
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
IF '(' booleanExpressions ')' ifBodyStatements
{
    $$ = CCompilerNs.LexYaccCallback.IfStatement($3, $5);
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
ELSE IF '(' booleanExpressions ')' ifBodyStatements
{
    $$= CCompilerNs.LexYaccCallback.IfStatement($4, $6);
}
;

elseStatement:
ELSE ifBodyStatements
{
    $$= CCompilerNs.LexYaccCallback.IfStatement(null, $2);
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
declare '=' addExpression ';'
{
    $$= CCompilerNs.LexYaccCallback.DeclareStatement($1, $3);
}
|
declare ';'
{
    $$= CCompilerNs.LexYaccCallback.DeclareStatement($1, null);
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

assignmentNoSemicolon: 
AssignmentLhsVariableId '=' addExpression
{
    $$= CCompilerNs.LexYaccCallback.AssignmentStatement($1, $3);
}
|
AssignmentLhsVariableId opAssign addExpression
{
    $$= CCompilerNs.LexYaccCallback.OpAssignmentStatement($1, $3, $2);
}
|
AssignmentLhsVariableId incrementDecrement
{
    $$= CCompilerNs.LexYaccCallback.IncrementDecrement($1, $2);
}
;

variableId:
variableId '.' ID
{
    $$ = CCompilerNs.LexYaccCallback.VariableId($1, $3, null, ".");
}
|
variableId '.' ID arrayIndex
{
    $$ = CCompilerNs.LexYaccCallback.VariableId($1, $3, $4, ".");
}
|
variableId ARROW ID
{
    $$ = CCompilerNs.LexYaccCallback.VariableId($1, $3, null, $2);
}
|
variableId ARROW ID arrayIndex
{
    $$ = CCompilerNs.LexYaccCallback.VariableId($1, $3, $4, $2);
}
|
ID
{
    $$ = CCompilerNs.LexYaccCallback.VariableId(null, $1, null, null);
}
|
ID arrayIndex
{
    $$ = CCompilerNs.LexYaccCallback.VariableId(null, $1, $2, null);
}
;

AssignmentLhsVariableId:
variableId
{
    $$ = CCompilerNs.LexYaccCallback.VariableId($1, null);
}
|
pointers variableId
{
    $$ = CCompilerNs.LexYaccCallback.VariableId($2, $1);
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
addExpression addMinusOp mulExpression
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, $2, $3);
}
|
mulExpression
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1);
}
;  

addMinusOp:
'+'
{
    $$ = "+";
}
|
'-'
{
    $$ = "-";
}
;

mulExpression: 
mulExpression multiplyDivideOp INT_VALUE
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, $2, $3);
}
| mulExpression multiplyDivideOp CHAR_VALUE
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, $2, $3);
}
| mulExpression multiplyDivideOp variableId
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, $2, $3);
}
| mulExpression multiplyDivideOp functionCallExpression
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, $2, $3);
}
|
mulExpression multiplyDivideOp '(' addExpression ')' 
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1, $2, $4);
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
variableId
{   
    $$ = CCompilerNs.LexYaccCallback.Expression($1);
}
|
'&' variableId
{   
    $$ = CCompilerNs.LexYaccCallback.Expression($2, true);
}
|
pointers variableId
{   
    $$ = CCompilerNs.LexYaccCallback.Expression($2, $1);
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
|
STRING_LITERAL
{
    $$ = CCompilerNs.LexYaccCallback.Expression($1);
}
;

multiplyDivideOp:
'*'
{
    $$ = "*";
}
|
'/'
{
    $$ = "/";
}
;

functionParams:
functionParams ',' functionParamDeclare
{
    $$ = CCompilerNs.LexYaccCallback.FunctionParams($3, $1);
}
|
functionParamDeclare
{
    $$ = CCompilerNs.LexYaccCallback.FunctionParams($1, null);
}
;

functionParamDeclare:
typeSpec ID
{
    $$ = CCompilerNs.LexYaccCallback.FunctionParamDeclare($1, $2, false, null);
}
|
typeSpec '[' ']' ID
{
    $$ = CCompilerNs.LexYaccCallback.FunctionParamDeclare($1, $4, true, null);
}
|
typeSpec ID '[' ']'
{
    $$ = CCompilerNs.LexYaccCallback.FunctionParamDeclare($1, $2, true, null);
}
|
typeSpec '[' ']' arraySize ID
{
    $$ = CCompilerNs.LexYaccCallback.FunctionParamDeclare($1, $5, true, $4);
}
|
typeSpec ID '[' ']' arraySize 
{
    $$ = CCompilerNs.LexYaccCallback.FunctionParamDeclare($1, $2, true, $5);
}
;

functionCallParams:
functionCallParams ',' addExpression
{
    $$ = CCompilerNs.LexYaccCallback.FunctionCallParams($3, $1);
}
|
addExpression
{
    $$ = CCompilerNs.LexYaccCallback.FunctionCallParams($1, null);
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
ID '(' functionCallParams ')'
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
FOR '(' assignmentNoSemicolon ';' booleanExpressions ';' assignmentNoSemicolon ')' ifBodyStatements
{
    $$ = CCompilerNs.LexYaccCallback.ForLoopStatement($3, $5, $7, $9);
}
;

whileLoopStatement:
WHILE '(' booleanExpressions ')' ifBodyStatements
{
    $$ = CCompilerNs.LexYaccCallback.WhileLoopStatement($3, $5);
}
;

booleanExpressions:
booleanExpressions logicalOperation booleanExpression
{
    $$ = CCompilerNs.LexYaccCallback.BooleanExpressions($3, $2, $1);
}
|
booleanExpression
{
    $$ = CCompilerNs.LexYaccCallback.BooleanExpressions($1, null, null);
}
;


booleanExpression:
addExpression relationalOp addExpression
{
    $$ = CCompilerNs.LexYaccCallback.BooleanExpression($1, $2, $3);
}
|
addExpression
{
    $$ = CCompilerNs.LexYaccCallback.BooleanExpression($1, null, null);
}
;

logicalOperation:
LOGICAL_AND 
{
    $$ = $1;
}
|
LOGICAL_OR
{
    $$ = $1;
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
declare ';'
{
    $$= CCompilerNs.LexYaccCallback.GlobalVariable($1, null);
}
|
declare '=' INT_VALUE ';'
{
    $$= CCompilerNs.LexYaccCallback.GlobalVariable($1, $3);
}
|
declare '=' CHAR_VALUE ';'
{
    $$= CCompilerNs.LexYaccCallback.GlobalVariable($1, $3);
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
declare ';'
{
    $$= CCompilerNs.LexYaccCallback.StructField($1);
}
;

declare:
typeSpec ID
{
    $$ = CCompilerNs.LexYaccCallback.Declare($1, $2, null);
}
|
typeSpec arraySize ID
{
    $$ = CCompilerNs.LexYaccCallback.Declare($1, $3, $2);
}
|
typeSpec ID arraySize 
{
    $$ = CCompilerNs.LexYaccCallback.Declare($1, $2, $3);
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

relationalOp:
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