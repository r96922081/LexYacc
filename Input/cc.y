%{
%}

%token <char>        CHAR_VALUE
%token <int>         INT_VALUE
%token <string>      RETURN ID INT_TYPE VOID_TYPE IF ELSE EQUAL_SIGN NOT_EQUAL_SIGN LESS_OR_EQUAL_SIGN GREATER_OR_EQUAL_SIGN FOR BREAK CONTINUE INCREMENT DECREMENT PLUS_ASSIGN MINUS_ASSIGN MULTIPLY_ASSIGN DIVIDE_ASSIGN CHAR_TYPE SINGLE_LINE_COMMENT STRUCT

%type <CCompilerNs.Program>                    program
%type <CCompilerNs.TopLevel>                   topLevel
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
program topLevel
{
    $$= CCompilerNs.CCLexYaccCallback.Program($1, $2);
}
|
topLevel
{
    $$= CCompilerNs.CCLexYaccCallback.Program(null, $1);
}
;


topLevel:
globalVariable
{
    $$= CCompilerNs.CCLexYaccCallback.TopLevel($1);
}
|
funDecl
{
    $$= CCompilerNs.CCLexYaccCallback.TopLevel($1);
}
|
structDef
{
    $$= CCompilerNs.CCLexYaccCallback.TopLevel($1);
}
|
SINGLE_LINE_COMMENT
{
    $$= CCompilerNs.CCLexYaccCallback.TopLevel();
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
typeSpec ID '(' ')' '{' '}'
{  
    $$= CCompilerNs.CCLexYaccCallback.FuncDecl($1, $2, null, null);
}
|
typeSpec ID '(' funcParams ')' '{' '}'
{  
    $$= CCompilerNs.CCLexYaccCallback.FuncDecl($1, $2, $4, null);
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
|
IF '(' addExpression relationlOp addExpression ')' statement
{
    $$ = CCompilerNs.CCLexYaccCallback.IfStatement($3, $4, $5, $7);
}
|
IF '(' addExpression relationlOp addExpression ')' '{' '}'
{
    $$ = CCompilerNs.CCLexYaccCallback.IfStatement($3, $4, $5);
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
|
ELSE IF '(' addExpression relationlOp addExpression ')' statement
{
    $$= CCompilerNs.CCLexYaccCallback.IfStatement($4, $5, $6, $8);
}
|
ELSE IF '(' addExpression relationlOp addExpression ')' '{' '}'
{
    $$= CCompilerNs.CCLexYaccCallback.IfStatement($4, $5, $6);
}
;

elseStatement:
ELSE '{' statements '}'
{
    $$= CCompilerNs.CCLexYaccCallback.IfStatement(null, null, null, $3);
}
|
ELSE statement
{
    $$= CCompilerNs.CCLexYaccCallback.IfStatement(null, null, null, $2);
}
|
ELSE '{' '}'
{
    $$= CCompilerNs.CCLexYaccCallback.IfStatement(null, null, null);
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
    $$= CCompilerNs.CCLexYaccCallback.DeclareStatement($1, $2, $4, null);
}
|
typeSpec ID ';'
{
    $$= CCompilerNs.CCLexYaccCallback.DeclareStatement($1, $2, null, null);
}
|
typeSpec ID arraySize ';'
{
    $$= CCompilerNs.CCLexYaccCallback.DeclareStatement($1, $2, null, $3);
}
|
STRUCT ID ID ';'
{
    $$= CCompilerNs.CCLexYaccCallback.DeclareStatement("struct " + $2, $3, null, null);
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
    $$= CCompilerNs.CCLexYaccCallback.EmptyStatement();
}
;

singleLineComment:
SINGLE_LINE_COMMENT
{
    $$= CCompilerNs.CCLexYaccCallback.EmptyStatement();
}
;

assignmentNoSemicolon: 
ID '=' addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.AssignmentStatement($1, $3, null);
}
ID arrayIndex '=' addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.AssignmentStatement($1, $4, $2);
}
|
ID PLUS_ASSIGN addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $3, null, "+");
}
|
ID arrayIndex PLUS_ASSIGN addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $4, $2, "+");
}
|
ID MINUS_ASSIGN addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $3, null, "-");
}
|
ID arrayIndex MINUS_ASSIGN addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $4, $2, "-");
}
|
ID  MULTIPLY_ASSIGN addExpression 
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $3, null, "*");
}
|
ID arrayIndex MULTIPLY_ASSIGN addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $4, $2, "*");
}
|
ID  DIVIDE_ASSIGN addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $3, null, "/");
}
|
ID arrayIndex DIVIDE_ASSIGN addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $4, $2, "/");
}
|
ID  INCREMENT
{
    $$= CCompilerNs.CCLexYaccCallback.IncrementDecrement($1, null, "+");
}
|
ID arrayIndex INCREMENT
{
    $$= CCompilerNs.CCLexYaccCallback.IncrementDecrement($1, $2, "+");
}
|
ID DECREMENT
{
    $$= CCompilerNs.CCLexYaccCallback.IncrementDecrement($1, null, "-");
}
|
ID arrayIndex DECREMENT
{
    $$= CCompilerNs.CCLexYaccCallback.IncrementDecrement($1, $2, "-");
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
| mulExpression '*' CHAR_VALUE
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, "*", $3);
}
| mulExpression '/' CHAR_VALUE
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
ID arrayIndex
{   
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, $2);
}
|
functionCallExpression
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1);
}
|
CHAR_VALUE
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
|
funcParams ',' typeSpec paramArraySize ID
{
    $$ = CCompilerNs.CCLexYaccCallback.FuncParamsArray($3, $5, $4, $1);
}
|
typeSpec paramArraySize ID
{
    $$ = CCompilerNs.CCLexYaccCallback.FuncParamsArray($1, $3, $2, null);
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

breakStatement:
BREAK ';'
{
    $$ = CCompilerNs.CCLexYaccCallback.BreakStatement();
}
;

continueStatement:
CONTINUE ';'
{
    $$ = CCompilerNs.CCLexYaccCallback.ContinueStatement();
}
;

forLoopStatement:
FOR '(' assignmentStatement addExpression relationlOp addExpression ';' assignmentNoSemicolon ')' '{' statements '}'
{
    $$ = CCompilerNs.CCLexYaccCallback.ForLoopStatement($3, $4, $5, $6, $8, $11);
}
|
FOR '(' assignmentStatement addExpression relationlOp addExpression ';' assignmentNoSemicolon ')' statement
{
    $$ = CCompilerNs.CCLexYaccCallback.ForLoopStatement($3, $4, $5, $6, $8, $10);
}
|
FOR '(' assignmentStatement addExpression relationlOp addExpression ';' assignmentNoSemicolon ')' '{' '}'
{
    $$ = CCompilerNs.CCLexYaccCallback.ForLoopStatement($3, $4, $5, $6, $8);
}
;

arraySize:
arraySize '[' INT_VALUE ']'
{
    $$ = CCompilerNs.CCLexYaccCallback.ArraySize($3, $1);
}
|
'[' INT_VALUE ']'
{
    $$ = CCompilerNs.CCLexYaccCallback.ArraySize($2, null);
}
;

paramArraySize:
'[' ']'
{
    $$ = CCompilerNs.CCLexYaccCallback.ParamArraySize(null);
}
|
'[' ']' arraySize
{
    $$ = CCompilerNs.CCLexYaccCallback.ParamArraySize($3);
}
;

arrayIndex:
arrayIndex '[' addExpression ']'
{
    $$ = CCompilerNs.CCLexYaccCallback.ArrayIndex($3, $1);
}
|
'[' addExpression ']'
{
    $$ = CCompilerNs.CCLexYaccCallback.ArrayIndex($2, null);
}
;

globalVariable:
typeSpec ID ';'
{
    $$= CCompilerNs.CCLexYaccCallback.GlobalVariable($1, $2, null, null);
}
|
typeSpec ID arraySize ';'
{
    $$= CCompilerNs.CCLexYaccCallback.GlobalVariable($1, $2, null, $3);
}
|
typeSpec ID '=' INT_VALUE ';'
{
    $$= CCompilerNs.CCLexYaccCallback.GlobalVariable($1, $2, $4, null);
}
|
typeSpec ID '=' CHAR_VALUE ';'
{
    $$= CCompilerNs.CCLexYaccCallback.GlobalVariable($1, $2, $4, null);
}
;

structDef:
STRUCT ID '{' structFields '}' ';'
{
    $$= CCompilerNs.CCLexYaccCallback.StructDef($2, $4);
}
;

structFields:
structFields structField
{
    $$= CCompilerNs.CCLexYaccCallback.StructFields($1, $2);
}
|
structField
{
    $$= CCompilerNs.CCLexYaccCallback.StructFields(null, $1);
}
;

structField:
typeSpec ID ';'
{
    $$= CCompilerNs.CCLexYaccCallback.StructField($1, $2, null);
}
|
typeSpec ID arraySize ';'
{
    $$= CCompilerNs.CCLexYaccCallback.StructField($1, $2, $3);
}
|
STRUCT ID ID ';'
{
    $$= CCompilerNs.CCLexYaccCallback.StructField("struct " + $2, $3, null);
}
|
STRUCT ID ID arraySize ';'
{
    $$= CCompilerNs.CCLexYaccCallback.StructField("struct " + $2, $3, $4);
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