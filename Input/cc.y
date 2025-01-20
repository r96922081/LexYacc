%{
%}

%token <char>        CHAR_VALUE
%token <int>         INT_VALUE
%token <string>      RETURN ID INT_TYPE VOID_TYPE IF ELSE EQUAL_SIGN NOT_EQUAL_SIGN LESS_OR_EQUAL_SIGN GREATER_OR_EQUAL_SIGN FOR BREAK CONTINUE INCREMENT DECREMENT PLUS_ASSIGN MINUS_ASSIGN MULTIPLY_ASSIGN DIVIDE_ASSIGN CHAR_TYPE SINGLE_LINE_COMMENT STRUCT

%type <CCompilerNs.Program>                    program
%type <CCompilerNs.GlobalDeclare>              globalDeclare
%type <CCompilerNs.FunDecl>                    funDecl
%type <List<CCompilerNs.Variable>>             funcParams
%type <List<CCompilerNs.Statement>>            statements 
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
%type <string>                                 typeSpec relationlOp


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
funDecl
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

funDecl:
typeSpec ID '(' ')' '{' statements '}'
{  
    $$= CCompilerNs.LexYaccCallback.FuncDecl($1, $2, null, $6);
}
|
typeSpec ID '(' funcParams ')' '{' statements '}'
{  
    $$= CCompilerNs.LexYaccCallback.FuncDecl($1, $2, $4, $7);
}
typeSpec ID '(' ')' '{' '}'
{  
    $$= CCompilerNs.LexYaccCallback.FuncDecl($1, $2, null, null);
}
|
typeSpec ID '(' funcParams ')' '{' '}'
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
IF '(' addExpression relationlOp addExpression ')' '{' statements '}'
{
    $$ = CCompilerNs.LexYaccCallback.IfStatement($3, $4, $5, $8);
}
|
IF '(' addExpression relationlOp addExpression ')' statement
{
    $$ = CCompilerNs.LexYaccCallback.IfStatement($3, $4, $5, $7);
}
|
IF '(' addExpression relationlOp addExpression ')' '{' '}'
{
    $$ = CCompilerNs.LexYaccCallback.IfStatement($3, $4, $5);
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
ELSE IF '(' addExpression relationlOp addExpression ')' '{' statements '}'
{
    $$= CCompilerNs.LexYaccCallback.IfStatement($4, $5, $6, $9);
}
|
ELSE IF '(' addExpression relationlOp addExpression ')' statement
{
    $$= CCompilerNs.LexYaccCallback.IfStatement($4, $5, $6, $8);
}
|
ELSE IF '(' addExpression relationlOp addExpression ')' '{' '}'
{
    $$= CCompilerNs.LexYaccCallback.IfStatement($4, $5, $6);
}
;

elseStatement:
ELSE '{' statements '}'
{
    $$= CCompilerNs.LexYaccCallback.IfStatement(null, null, null, $3);
}
|
ELSE statement
{
    $$= CCompilerNs.LexYaccCallback.IfStatement(null, null, null, $2);
}
|
ELSE '{' '}'
{
    $$= CCompilerNs.LexYaccCallback.IfStatement(null, null, null);
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
};

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
    $$= CCompilerNs.LexYaccCallback.DeclareStatement("struct " + $2, $3, null, null);
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
ID PLUS_ASSIGN addExpression
{
    $$= CCompilerNs.LexYaccCallback.OpAssignmentStatement($1, $3, null, "+");
}
|
ID arrayIndex PLUS_ASSIGN addExpression
{
    $$= CCompilerNs.LexYaccCallback.OpAssignmentStatement($1, $4, $2, "+");
}
|
ID MINUS_ASSIGN addExpression
{
    $$= CCompilerNs.LexYaccCallback.OpAssignmentStatement($1, $3, null, "-");
}
|
ID arrayIndex MINUS_ASSIGN addExpression
{
    $$= CCompilerNs.LexYaccCallback.OpAssignmentStatement($1, $4, $2, "-");
}
|
ID  MULTIPLY_ASSIGN addExpression 
{
    $$= CCompilerNs.LexYaccCallback.OpAssignmentStatement($1, $3, null, "*");
}
|
ID arrayIndex MULTIPLY_ASSIGN addExpression
{
    $$= CCompilerNs.LexYaccCallback.OpAssignmentStatement($1, $4, $2, "*");
}
|
ID  DIVIDE_ASSIGN addExpression
{
    $$= CCompilerNs.LexYaccCallback.OpAssignmentStatement($1, $3, null, "/");
}
|
ID arrayIndex DIVIDE_ASSIGN addExpression
{
    $$= CCompilerNs.LexYaccCallback.OpAssignmentStatement($1, $4, $2, "/");
}
|
ID  INCREMENT
{
    $$= CCompilerNs.LexYaccCallback.IncrementDecrement($1, null, "+");
}
|
ID arrayIndex INCREMENT
{
    $$= CCompilerNs.LexYaccCallback.IncrementDecrement($1, $2, "+");
}
|
ID DECREMENT
{
    $$= CCompilerNs.LexYaccCallback.IncrementDecrement($1, null, "-");
}
|
ID arrayIndex DECREMENT
{
    $$= CCompilerNs.LexYaccCallback.IncrementDecrement($1, $2, "-");
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

funcParams:
funcParams ',' typeSpec ID
{
    $$ = CCompilerNs.LexYaccCallback.FuncParams($3, $4, $1);
}
|
typeSpec ID
{
    $$ = CCompilerNs.LexYaccCallback.FuncParams($1, $2, null);
}
|
funcParams ',' typeSpec paramArraySize ID
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
FOR '(' assignmentStatement addExpression relationlOp addExpression ';' assignmentNoSemicolon ')' '{' statements '}'
{
    $$ = CCompilerNs.LexYaccCallback.ForLoopStatement($3, $4, $5, $6, $8, $11);
}
|
FOR '(' assignmentStatement addExpression relationlOp addExpression ';' assignmentNoSemicolon ')' statement
{
    $$ = CCompilerNs.LexYaccCallback.ForLoopStatement($3, $4, $5, $6, $8, $10);
}
|
FOR '(' assignmentStatement addExpression relationlOp addExpression ';' assignmentNoSemicolon ')' '{' '}'
{
    $$ = CCompilerNs.LexYaccCallback.ForLoopStatement($3, $4, $5, $6, $8);
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