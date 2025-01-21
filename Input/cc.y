%{
%}

%token <char>        CHAR_VALUE
%token <int>         INT_VALUE
%token <string>      RETURN ID INT_TYPE VOID_TYPE IF ELSE EQUAL_SIGN NOT_EQUAL_SIGN LESS_OR_EQUAL_SIGN GREATER_OR_EQUAL_SIGN FOR BREAK CONTINUE INCREMENT DECREMENT PLUS_ASSIGN MINUS_ASSIGN MULTIPLY_ASSIGN DIVIDE_ASSIGN CHAR_TYPE SINGLE_LINE_COMMENT STRUCT

%type <CCompilerNs.Program>                    program
%type <CCompilerNs.GlobalDeclare>              globalDeclare
%type <CCompilerNs.FunctionDeclare>            functionDeclare
%type <List<CCompilerNs.Variable>>             functionParams
%type <List<CCompilerNs.Statement>>            statements ifBodyStatements functionBodyStatements
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
%type <List<CCompilerNs.Expression>>           functionCallParams
%type <CCompilerNs.Expression>                 addExpression mulExpression
%type <List<int>>                              arraySize
%type <List<CCompilerNs.Expression>>           arrayIndex
%type <CCompilerNs.StructDef>                  structDef
%type <List<CCompilerNs.StructField>>          structFields
%type <CCompilerNs.StructField>                structField
%type <CCompilerNs.VariableId>                 variableId
%type <CCompilerNs.Variable>                   declare
%type <string>                                 typeSpec relationlOp opAssign incrementDecrement addMinusOp multiplyDivideOp


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
CHAR_TYPE
{
    $$ = $1;
}
|
VOID_TYPE
{
    $$ = $1;
}
|
STRUCT ID
{
    $$ = $1 + " " + $2;
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
variableId '=' addExpression
{
    $$= CCompilerNs.LexYaccCallback.AssignmentStatement($1, $3);
}
|
variableId opAssign addExpression
{
    $$= CCompilerNs.LexYaccCallback.OpAssignmentStatement($1, $3, $2);
}
|
variableId incrementDecrement
{
    $$= CCompilerNs.LexYaccCallback.IncrementDecrement($1, $2);
}
;

variableId:
variableId '.' ID
{
    $$ = CCompilerNs.LexYaccCallback.VariableId($1, $3, null);
}
|
variableId '.' ID arrayIndex
{
    $$ = CCompilerNs.LexYaccCallback.VariableId($1, $3, $4);
}
|
ID
{
    $$ = CCompilerNs.LexYaccCallback.VariableId(null, $1, null);
}
|
ID arrayIndex
{
    $$ = CCompilerNs.LexYaccCallback.VariableId(null, $1, $2);
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
functionParams ',' typeSpec arraySize ID
{
    $$ = CCompilerNs.LexYaccCallback.FuncParamsArray($3, $5, $4, $1);
}
|
typeSpec arraySize ID
{
    $$ = CCompilerNs.LexYaccCallback.FuncParamsArray($1, $3, $2, null);
}
;

functionCallParams:
functionCallParams ',' addExpression
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