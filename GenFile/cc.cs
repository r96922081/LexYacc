//LexYacc Gen
public class cc
{
    public static object Parse(string input)
    {
        return ccNs.LexYaccNs.LexYacc.Parse(input, ccNs.LexActions.ruleInput, ccNs.YaccActions.ruleInput, ccNs.LexActions.CallAction, ccNs.YaccActions.CallAction);
    }
}
//Yacc Gen 
namespace ccNs
{



public class YaccActions{

    public static Dictionary<string, Func<Dictionary<int, object>, object>> actions = new Dictionary<string, Func<Dictionary<int, object>, object>>();

    public static string ruleInput = @"%{
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
    $$= null;
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
    $$= CCompilerNs.CCLexYaccCallback.DeclareStatement(""struct "" + $2, $3, null, null);
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
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $3, null, ""+"");
}
|
ID arrayIndex PLUS_ASSIGN addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $4, $2, ""+"");
}
|
ID MINUS_ASSIGN addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $3, null, ""-"");
}
|
ID arrayIndex MINUS_ASSIGN addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $4, $2, ""-"");
}
|
ID  MULTIPLY_ASSIGN addExpression 
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $3, null, ""*"");
}
|
ID arrayIndex MULTIPLY_ASSIGN addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $4, $2, ""*"");
}
|
ID  DIVIDE_ASSIGN addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $3, null, ""/"");
}
|
ID arrayIndex DIVIDE_ASSIGN addExpression
{
    $$= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement($1, $4, $2, ""/"");
}
|
ID  INCREMENT
{
    $$= CCompilerNs.CCLexYaccCallback.IncrementDecrement($1, null, ""+"");
}
|
ID arrayIndex INCREMENT
{
    $$= CCompilerNs.CCLexYaccCallback.IncrementDecrement($1, $2, ""+"");
}
|
ID DECREMENT
{
    $$= CCompilerNs.CCLexYaccCallback.IncrementDecrement($1, null, ""-"");
}
|
ID arrayIndex DECREMENT
{
    $$= CCompilerNs.CCLexYaccCallback.IncrementDecrement($1, $2, ""-"");
}
;

addExpression: 
addExpression '+' mulExpression
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, ""+"", $3);
}
| addExpression '-' mulExpression
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, ""-"", $3);
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
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, ""*"", $3);
}
| mulExpression '/' INT_VALUE
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, ""/"", $3);
}
| mulExpression '*' CHAR_VALUE
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, ""*"", $3);
}
| mulExpression '/' CHAR_VALUE
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, ""/"", $3);
}
| mulExpression '*' ID
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, ""*"", $3);
}
| mulExpression '/' ID
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, ""/"", $3);
}
| mulExpression '*' functionCallExpression
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, ""*"", $3);
}
| mulExpression '/' functionCallExpression
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, ""/"", $3);
}
|
mulExpression '*' '(' addExpression ')' 
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, ""*"", $4);
}
| mulExpression '/' '(' addExpression ')' 
{
    $$ = CCompilerNs.CCLexYaccCallback.Expression($1, ""/"", $4);
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
    $$= CCompilerNs.CCLexYaccCallback.StructField(""struct "" + $2, $3, null);
}
|
STRUCT ID ID arraySize ';'
{
    $$= CCompilerNs.CCLexYaccCallback.StructField(""struct "" + $2, $3, $4);
}
;

relationlOp:
'<'
{
    $$ = ""<"";
}
|
'>'
{
    $$ = "">"";
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

%%";


    public static object CallAction(string functionName, Dictionary<int, object> param)
    {
        Init();
        if (!actions.ContainsKey(functionName))
            return null;
        return actions[functionName](param);
    }

    public static void Init()
    {
        if (actions.Count != 0)
            return;

        actions.Add("Rule_start_Producton_0", Rule_start_Producton_0);
        actions.Add("Rule_program_Producton_0", Rule_program_Producton_0);
        actions.Add("Rule_program_LeftRecursionExpand_Producton_0", Rule_program_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_program_LeftRecursionExpand_Producton_1", Rule_program_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_topLevel_Producton_0", Rule_topLevel_Producton_0);
        actions.Add("Rule_topLevel_Producton_1", Rule_topLevel_Producton_1);
        actions.Add("Rule_topLevel_Producton_2", Rule_topLevel_Producton_2);
        actions.Add("Rule_topLevel_Producton_3", Rule_topLevel_Producton_3);
        actions.Add("Rule_funDecl_Producton_0", Rule_funDecl_Producton_0);
        actions.Add("Rule_funDecl_Producton_1", Rule_funDecl_Producton_1);
        actions.Add("Rule_funDecl_Producton_2", Rule_funDecl_Producton_2);
        actions.Add("Rule_funDecl_Producton_3", Rule_funDecl_Producton_3);
        actions.Add("Rule_typeSpec_Producton_0", Rule_typeSpec_Producton_0);
        actions.Add("Rule_typeSpec_Producton_1", Rule_typeSpec_Producton_1);
        actions.Add("Rule_typeSpec_Producton_2", Rule_typeSpec_Producton_2);
        actions.Add("Rule_statements_Producton_0", Rule_statements_Producton_0);
        actions.Add("Rule_statements_LeftRecursionExpand_Producton_0", Rule_statements_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_statements_LeftRecursionExpand_Producton_1", Rule_statements_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_statement_Producton_0", Rule_statement_Producton_0);
        actions.Add("Rule_statement_Producton_1", Rule_statement_Producton_1);
        actions.Add("Rule_statement_Producton_2", Rule_statement_Producton_2);
        actions.Add("Rule_statement_Producton_3", Rule_statement_Producton_3);
        actions.Add("Rule_statement_Producton_4", Rule_statement_Producton_4);
        actions.Add("Rule_statement_Producton_5", Rule_statement_Producton_5);
        actions.Add("Rule_statement_Producton_6", Rule_statement_Producton_6);
        actions.Add("Rule_statement_Producton_7", Rule_statement_Producton_7);
        actions.Add("Rule_statement_Producton_8", Rule_statement_Producton_8);
        actions.Add("Rule_statement_Producton_9", Rule_statement_Producton_9);
        actions.Add("Rule_compoundIfStatement_Producton_0", Rule_compoundIfStatement_Producton_0);
        actions.Add("Rule_compoundIfStatement_Producton_1", Rule_compoundIfStatement_Producton_1);
        actions.Add("Rule_compoundIfStatement_Producton_2", Rule_compoundIfStatement_Producton_2);
        actions.Add("Rule_compoundIfStatement_Producton_3", Rule_compoundIfStatement_Producton_3);
        actions.Add("Rule_ifStatement_Producton_0", Rule_ifStatement_Producton_0);
        actions.Add("Rule_ifStatement_Producton_1", Rule_ifStatement_Producton_1);
        actions.Add("Rule_ifStatement_Producton_2", Rule_ifStatement_Producton_2);
        actions.Add("Rule_elseIfStatements_Producton_0", Rule_elseIfStatements_Producton_0);
        actions.Add("Rule_elseIfStatements_LeftRecursionExpand_Producton_0", Rule_elseIfStatements_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_elseIfStatements_LeftRecursionExpand_Producton_1", Rule_elseIfStatements_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_elseIfStatement_Producton_0", Rule_elseIfStatement_Producton_0);
        actions.Add("Rule_elseIfStatement_Producton_1", Rule_elseIfStatement_Producton_1);
        actions.Add("Rule_elseIfStatement_Producton_2", Rule_elseIfStatement_Producton_2);
        actions.Add("Rule_elseStatement_Producton_0", Rule_elseStatement_Producton_0);
        actions.Add("Rule_elseStatement_Producton_1", Rule_elseStatement_Producton_1);
        actions.Add("Rule_elseStatement_Producton_2", Rule_elseStatement_Producton_2);
        actions.Add("Rule_returnStatement_Producton_0", Rule_returnStatement_Producton_0);
        actions.Add("Rule_returnStatement_Producton_1", Rule_returnStatement_Producton_1);
        actions.Add("Rule_declareStatement_Producton_0", Rule_declareStatement_Producton_0);
        actions.Add("Rule_declareStatement_Producton_1", Rule_declareStatement_Producton_1);
        actions.Add("Rule_declareStatement_Producton_2", Rule_declareStatement_Producton_2);
        actions.Add("Rule_declareStatement_Producton_3", Rule_declareStatement_Producton_3);
        actions.Add("Rule_assignmentStatement_Producton_0", Rule_assignmentStatement_Producton_0);
        actions.Add("Rule_emptyStatement_Producton_0", Rule_emptyStatement_Producton_0);
        actions.Add("Rule_singleLineComment_Producton_0", Rule_singleLineComment_Producton_0);
        actions.Add("Rule_assignmentNoSemicolon_Producton_0", Rule_assignmentNoSemicolon_Producton_0);
        actions.Add("Rule_assignmentNoSemicolon_Producton_1", Rule_assignmentNoSemicolon_Producton_1);
        actions.Add("Rule_assignmentNoSemicolon_Producton_2", Rule_assignmentNoSemicolon_Producton_2);
        actions.Add("Rule_assignmentNoSemicolon_Producton_3", Rule_assignmentNoSemicolon_Producton_3);
        actions.Add("Rule_assignmentNoSemicolon_Producton_4", Rule_assignmentNoSemicolon_Producton_4);
        actions.Add("Rule_assignmentNoSemicolon_Producton_5", Rule_assignmentNoSemicolon_Producton_5);
        actions.Add("Rule_assignmentNoSemicolon_Producton_6", Rule_assignmentNoSemicolon_Producton_6);
        actions.Add("Rule_assignmentNoSemicolon_Producton_7", Rule_assignmentNoSemicolon_Producton_7);
        actions.Add("Rule_assignmentNoSemicolon_Producton_8", Rule_assignmentNoSemicolon_Producton_8);
        actions.Add("Rule_assignmentNoSemicolon_Producton_9", Rule_assignmentNoSemicolon_Producton_9);
        actions.Add("Rule_assignmentNoSemicolon_Producton_10", Rule_assignmentNoSemicolon_Producton_10);
        actions.Add("Rule_assignmentNoSemicolon_Producton_11", Rule_assignmentNoSemicolon_Producton_11);
        actions.Add("Rule_assignmentNoSemicolon_Producton_12", Rule_assignmentNoSemicolon_Producton_12);
        actions.Add("Rule_assignmentNoSemicolon_Producton_13", Rule_assignmentNoSemicolon_Producton_13);
        actions.Add("Rule_addExpression_Producton_0", Rule_addExpression_Producton_0);
        actions.Add("Rule_addExpression_LeftRecursionExpand_Producton_0", Rule_addExpression_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_addExpression_LeftRecursionExpand_Producton_1", Rule_addExpression_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_addExpression_LeftRecursionExpand_Producton_2", Rule_addExpression_LeftRecursionExpand_Producton_2);
        actions.Add("Rule_mulExpression_Producton_0", Rule_mulExpression_Producton_0);
        actions.Add("Rule_mulExpression_Producton_1", Rule_mulExpression_Producton_1);
        actions.Add("Rule_mulExpression_Producton_2", Rule_mulExpression_Producton_2);
        actions.Add("Rule_mulExpression_Producton_3", Rule_mulExpression_Producton_3);
        actions.Add("Rule_mulExpression_Producton_4", Rule_mulExpression_Producton_4);
        actions.Add("Rule_mulExpression_Producton_5", Rule_mulExpression_Producton_5);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_0", Rule_mulExpression_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_1", Rule_mulExpression_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_2", Rule_mulExpression_LeftRecursionExpand_Producton_2);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_3", Rule_mulExpression_LeftRecursionExpand_Producton_3);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_4", Rule_mulExpression_LeftRecursionExpand_Producton_4);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_5", Rule_mulExpression_LeftRecursionExpand_Producton_5);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_6", Rule_mulExpression_LeftRecursionExpand_Producton_6);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_7", Rule_mulExpression_LeftRecursionExpand_Producton_7);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_8", Rule_mulExpression_LeftRecursionExpand_Producton_8);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_9", Rule_mulExpression_LeftRecursionExpand_Producton_9);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_10", Rule_mulExpression_LeftRecursionExpand_Producton_10);
        actions.Add("Rule_funcParams_Producton_0", Rule_funcParams_Producton_0);
        actions.Add("Rule_funcParams_Producton_1", Rule_funcParams_Producton_1);
        actions.Add("Rule_funcParams_LeftRecursionExpand_Producton_0", Rule_funcParams_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_funcParams_LeftRecursionExpand_Producton_1", Rule_funcParams_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_funcParams_LeftRecursionExpand_Producton_2", Rule_funcParams_LeftRecursionExpand_Producton_2);
        actions.Add("Rule_funcCallParams_Producton_0", Rule_funcCallParams_Producton_0);
        actions.Add("Rule_funcCallParams_LeftRecursionExpand_Producton_0", Rule_funcCallParams_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_funcCallParams_LeftRecursionExpand_Producton_1", Rule_funcCallParams_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_functionCallStatement_Producton_0", Rule_functionCallStatement_Producton_0);
        actions.Add("Rule_functionCallExpression_Producton_0", Rule_functionCallExpression_Producton_0);
        actions.Add("Rule_functionCallExpression_Producton_1", Rule_functionCallExpression_Producton_1);
        actions.Add("Rule_breakStatement_Producton_0", Rule_breakStatement_Producton_0);
        actions.Add("Rule_continueStatement_Producton_0", Rule_continueStatement_Producton_0);
        actions.Add("Rule_forLoopStatement_Producton_0", Rule_forLoopStatement_Producton_0);
        actions.Add("Rule_forLoopStatement_Producton_1", Rule_forLoopStatement_Producton_1);
        actions.Add("Rule_forLoopStatement_Producton_2", Rule_forLoopStatement_Producton_2);
        actions.Add("Rule_arraySize_Producton_0", Rule_arraySize_Producton_0);
        actions.Add("Rule_arraySize_LeftRecursionExpand_Producton_0", Rule_arraySize_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_arraySize_LeftRecursionExpand_Producton_1", Rule_arraySize_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_paramArraySize_Producton_0", Rule_paramArraySize_Producton_0);
        actions.Add("Rule_paramArraySize_Producton_1", Rule_paramArraySize_Producton_1);
        actions.Add("Rule_arrayIndex_Producton_0", Rule_arrayIndex_Producton_0);
        actions.Add("Rule_arrayIndex_LeftRecursionExpand_Producton_0", Rule_arrayIndex_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_arrayIndex_LeftRecursionExpand_Producton_1", Rule_arrayIndex_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_globalVariable_Producton_0", Rule_globalVariable_Producton_0);
        actions.Add("Rule_globalVariable_Producton_1", Rule_globalVariable_Producton_1);
        actions.Add("Rule_globalVariable_Producton_2", Rule_globalVariable_Producton_2);
        actions.Add("Rule_globalVariable_Producton_3", Rule_globalVariable_Producton_3);
        actions.Add("Rule_structDef_Producton_0", Rule_structDef_Producton_0);
        actions.Add("Rule_structFields_Producton_0", Rule_structFields_Producton_0);
        actions.Add("Rule_structFields_LeftRecursionExpand_Producton_0", Rule_structFields_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_structFields_LeftRecursionExpand_Producton_1", Rule_structFields_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_structField_Producton_0", Rule_structField_Producton_0);
        actions.Add("Rule_structField_Producton_1", Rule_structField_Producton_1);
        actions.Add("Rule_structField_Producton_2", Rule_structField_Producton_2);
        actions.Add("Rule_structField_Producton_3", Rule_structField_Producton_3);
        actions.Add("Rule_relationlOp_Producton_0", Rule_relationlOp_Producton_0);
        actions.Add("Rule_relationlOp_Producton_1", Rule_relationlOp_Producton_1);
        actions.Add("Rule_relationlOp_Producton_2", Rule_relationlOp_Producton_2);
        actions.Add("Rule_relationlOp_Producton_3", Rule_relationlOp_Producton_3);
        actions.Add("Rule_relationlOp_Producton_4", Rule_relationlOp_Producton_4);
        actions.Add("Rule_relationlOp_Producton_5", Rule_relationlOp_Producton_5);
    }

    public static object Rule_start_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.Program _0 = new CCompilerNs.Program();
        CCompilerNs.Program _1 = (CCompilerNs.Program)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_program_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.Program _0 = new CCompilerNs.Program();
        CCompilerNs.TopLevel _1 = (CCompilerNs.TopLevel)objects[1];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.Program(null, _1);

        return _0;
    }

    public static object Rule_program_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.Program _0 = new CCompilerNs.Program();
        CCompilerNs.Program _1 =(CCompilerNs.Program)objects[1];
        CCompilerNs.TopLevel _2 = (CCompilerNs.TopLevel)objects[2];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.Program(_1, _2);

        return _0;
    }

    public static object Rule_program_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.Program _0 = new CCompilerNs.Program();

        return _0;
    }

    public static object Rule_topLevel_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.TopLevel _0 = new CCompilerNs.TopLevel();
        CCompilerNs.GlobalVariable _1 = (CCompilerNs.GlobalVariable)objects[1];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.TopLevel(_1);

        return _0;
    }

    public static object Rule_topLevel_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.TopLevel _0 = new CCompilerNs.TopLevel();
        CCompilerNs.FunDecl _1 = (CCompilerNs.FunDecl)objects[1];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.TopLevel(_1);

        return _0;
    }

    public static object Rule_topLevel_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.TopLevel _0 = new CCompilerNs.TopLevel();
        CCompilerNs.StructDef _1 = (CCompilerNs.StructDef)objects[1];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.TopLevel(_1);

        return _0;
    }

    public static object Rule_topLevel_Producton_3(Dictionary<int, object> objects) { 
        CCompilerNs.TopLevel _0 = new CCompilerNs.TopLevel();
        string _1 = (string)objects[1];

        // user-defined action
        _0= null;

        return _0;
    }

    public static object Rule_funDecl_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.FunDecl _0 = new CCompilerNs.FunDecl();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        List<CCompilerNs.Statement> _6 = (List<CCompilerNs.Statement>)objects[6];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.FuncDecl(_1, _2, null, _6);

        return _0;
    }

    public static object Rule_funDecl_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.FunDecl _0 = new CCompilerNs.FunDecl();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        List<CCompilerNs.Variable> _4 = (List<CCompilerNs.Variable>)objects[4];
        List<CCompilerNs.Statement> _7 = (List<CCompilerNs.Statement>)objects[7];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.FuncDecl(_1, _2, _4, _7);

        return _0;
    }

    public static object Rule_funDecl_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.FunDecl _0 = new CCompilerNs.FunDecl();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.FuncDecl(_1, _2, null, null);

        return _0;
    }

    public static object Rule_funDecl_Producton_3(Dictionary<int, object> objects) { 
        CCompilerNs.FunDecl _0 = new CCompilerNs.FunDecl();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        List<CCompilerNs.Variable> _4 = (List<CCompilerNs.Variable>)objects[4];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.FuncDecl(_1, _2, _4, null);

        return _0;
    }

    public static object Rule_typeSpec_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_typeSpec_Producton_1(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_typeSpec_Producton_2(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_statements_Producton_0(Dictionary<int, object> objects) { 
        List<CCompilerNs.Statement> _0 = new List<CCompilerNs.Statement>();
        CCompilerNs.Statement _1 = (CCompilerNs.Statement)objects[1];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.Statements(_1, null);

        return _0;
    }

    public static object Rule_statements_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        List<CCompilerNs.Statement> _0 = new List<CCompilerNs.Statement>();
        List<CCompilerNs.Statement> _1 =(List<CCompilerNs.Statement>)objects[1];
        CCompilerNs.Statement _2 = (CCompilerNs.Statement)objects[2];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.Statements(_2, _1);

        return _0;
    }

    public static object Rule_statements_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        List<CCompilerNs.Statement> _0 = new List<CCompilerNs.Statement>();

        return _0;
    }

    public static object Rule_statement_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.Statement _0 = new CCompilerNs.Statement();
        CCompilerNs.ReturnStatement _1 = (CCompilerNs.ReturnStatement)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_statement_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.Statement _0 = new CCompilerNs.Statement();
        CCompilerNs.DeclareStatement _1 = (CCompilerNs.DeclareStatement)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_statement_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.Statement _0 = new CCompilerNs.Statement();
        CCompilerNs.AssignmentStatement _1 = (CCompilerNs.AssignmentStatement)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_statement_Producton_3(Dictionary<int, object> objects) { 
        CCompilerNs.Statement _0 = new CCompilerNs.Statement();
        CCompilerNs.FunctionCallExpression _1 = (CCompilerNs.FunctionCallExpression)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_statement_Producton_4(Dictionary<int, object> objects) { 
        CCompilerNs.Statement _0 = new CCompilerNs.Statement();
        CCompilerNs.CompoundIfStatement _1 = (CCompilerNs.CompoundIfStatement)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_statement_Producton_5(Dictionary<int, object> objects) { 
        CCompilerNs.Statement _0 = new CCompilerNs.Statement();
        CCompilerNs.ForLoopStatement _1 = (CCompilerNs.ForLoopStatement)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_statement_Producton_6(Dictionary<int, object> objects) { 
        CCompilerNs.Statement _0 = new CCompilerNs.Statement();
        CCompilerNs.BreakStatement _1 = (CCompilerNs.BreakStatement)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_statement_Producton_7(Dictionary<int, object> objects) { 
        CCompilerNs.Statement _0 = new CCompilerNs.Statement();
        CCompilerNs.ContinueStatement _1 = (CCompilerNs.ContinueStatement)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_statement_Producton_8(Dictionary<int, object> objects) { 
        CCompilerNs.Statement _0 = new CCompilerNs.Statement();
        CCompilerNs.EmptyStatement _1 = (CCompilerNs.EmptyStatement)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_statement_Producton_9(Dictionary<int, object> objects) { 
        CCompilerNs.Statement _0 = new CCompilerNs.Statement();
        CCompilerNs.EmptyStatement _1 = (CCompilerNs.EmptyStatement)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_compoundIfStatement_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.CompoundIfStatement _0 = new CCompilerNs.CompoundIfStatement();
        CCompilerNs.IfStatement _1 = (CCompilerNs.IfStatement)objects[1];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.CompoundIfStatement(_1);

        return _0;
    }

    public static object Rule_compoundIfStatement_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.CompoundIfStatement _0 = new CCompilerNs.CompoundIfStatement();
        CCompilerNs.IfStatement _1 = (CCompilerNs.IfStatement)objects[1];
        List<CCompilerNs.IfStatement> _2 = (List<CCompilerNs.IfStatement>)objects[2];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.CompoundIfStatement(_1, _2);

        return _0;
    }

    public static object Rule_compoundIfStatement_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.CompoundIfStatement _0 = new CCompilerNs.CompoundIfStatement();
        CCompilerNs.IfStatement _1 = (CCompilerNs.IfStatement)objects[1];
        List<CCompilerNs.IfStatement> _2 = (List<CCompilerNs.IfStatement>)objects[2];
        CCompilerNs.IfStatement _3 = (CCompilerNs.IfStatement)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.CompoundIfStatement(_1, _2, _3);

        return _0;
    }

    public static object Rule_compoundIfStatement_Producton_3(Dictionary<int, object> objects) { 
        CCompilerNs.CompoundIfStatement _0 = new CCompilerNs.CompoundIfStatement();
        CCompilerNs.IfStatement _1 = (CCompilerNs.IfStatement)objects[1];
        CCompilerNs.IfStatement _2 = (CCompilerNs.IfStatement)objects[2];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.CompoundIfStatement(_1, _2);

        return _0;
    }

    public static object Rule_ifStatement_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.IfStatement _0 = new CCompilerNs.IfStatement();
        string _1 = (string)objects[1];
        CCompilerNs.Expression _3 = (CCompilerNs.Expression)objects[3];
        string _4 = (string)objects[4];
        CCompilerNs.Expression _5 = (CCompilerNs.Expression)objects[5];
        List<CCompilerNs.Statement> _8 = (List<CCompilerNs.Statement>)objects[8];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.IfStatement(_3, _4, _5, _8);

        return _0;
    }

    public static object Rule_ifStatement_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.IfStatement _0 = new CCompilerNs.IfStatement();
        string _1 = (string)objects[1];
        CCompilerNs.Expression _3 = (CCompilerNs.Expression)objects[3];
        string _4 = (string)objects[4];
        CCompilerNs.Expression _5 = (CCompilerNs.Expression)objects[5];
        CCompilerNs.Statement _7 = (CCompilerNs.Statement)objects[7];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.IfStatement(_3, _4, _5, _7);

        return _0;
    }

    public static object Rule_ifStatement_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.IfStatement _0 = new CCompilerNs.IfStatement();
        string _1 = (string)objects[1];
        CCompilerNs.Expression _3 = (CCompilerNs.Expression)objects[3];
        string _4 = (string)objects[4];
        CCompilerNs.Expression _5 = (CCompilerNs.Expression)objects[5];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.IfStatement(_3, _4, _5);

        return _0;
    }

    public static object Rule_elseIfStatements_Producton_0(Dictionary<int, object> objects) { 
        List<CCompilerNs.IfStatement> _0 = new List<CCompilerNs.IfStatement>();
        CCompilerNs.IfStatement _1 = (CCompilerNs.IfStatement)objects[1];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.ElseIfStatements(null, _1);

        return _0;
    }

    public static object Rule_elseIfStatements_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        List<CCompilerNs.IfStatement> _0 = new List<CCompilerNs.IfStatement>();
        List<CCompilerNs.IfStatement> _1 =(List<CCompilerNs.IfStatement>)objects[1];
        CCompilerNs.IfStatement _2 = (CCompilerNs.IfStatement)objects[2];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.ElseIfStatements(_1, _2);

        return _0;
    }

    public static object Rule_elseIfStatements_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        List<CCompilerNs.IfStatement> _0 = new List<CCompilerNs.IfStatement>();

        return _0;
    }

    public static object Rule_elseIfStatement_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.IfStatement _0 = new CCompilerNs.IfStatement();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];
        string _5 = (string)objects[5];
        CCompilerNs.Expression _6 = (CCompilerNs.Expression)objects[6];
        List<CCompilerNs.Statement> _9 = (List<CCompilerNs.Statement>)objects[9];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.IfStatement(_4, _5, _6, _9);

        return _0;
    }

    public static object Rule_elseIfStatement_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.IfStatement _0 = new CCompilerNs.IfStatement();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];
        string _5 = (string)objects[5];
        CCompilerNs.Expression _6 = (CCompilerNs.Expression)objects[6];
        CCompilerNs.Statement _8 = (CCompilerNs.Statement)objects[8];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.IfStatement(_4, _5, _6, _8);

        return _0;
    }

    public static object Rule_elseIfStatement_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.IfStatement _0 = new CCompilerNs.IfStatement();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];
        string _5 = (string)objects[5];
        CCompilerNs.Expression _6 = (CCompilerNs.Expression)objects[6];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.IfStatement(_4, _5, _6);

        return _0;
    }

    public static object Rule_elseStatement_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.IfStatement _0 = new CCompilerNs.IfStatement();
        string _1 = (string)objects[1];
        List<CCompilerNs.Statement> _3 = (List<CCompilerNs.Statement>)objects[3];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.IfStatement(null, null, null, _3);

        return _0;
    }

    public static object Rule_elseStatement_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.IfStatement _0 = new CCompilerNs.IfStatement();
        string _1 = (string)objects[1];
        CCompilerNs.Statement _2 = (CCompilerNs.Statement)objects[2];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.IfStatement(null, null, null, _2);

        return _0;
    }

    public static object Rule_elseStatement_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.IfStatement _0 = new CCompilerNs.IfStatement();
        string _1 = (string)objects[1];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.IfStatement(null, null, null);

        return _0;
    }

    public static object Rule_returnStatement_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.ReturnStatement _0 = new CCompilerNs.ReturnStatement();
        string _1 = (string)objects[1];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.ReturnStatement(null);

        return _0;
    }

    public static object Rule_returnStatement_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.ReturnStatement _0 = new CCompilerNs.ReturnStatement();
        string _1 = (string)objects[1];
        CCompilerNs.Expression _2 = (CCompilerNs.Expression)objects[2];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.ReturnStatement(_2);

        return _0;
    }

    public static object Rule_declareStatement_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.DeclareStatement _0 = new CCompilerNs.DeclareStatement();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.DeclareStatement(_1, _2, _4, null);

        return _0;
    }

    public static object Rule_declareStatement_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.DeclareStatement _0 = new CCompilerNs.DeclareStatement();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.DeclareStatement(_1, _2, null, null);

        return _0;
    }

    public static object Rule_declareStatement_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.DeclareStatement _0 = new CCompilerNs.DeclareStatement();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        List<int> _3 = (List<int>)objects[3];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.DeclareStatement(_1, _2, null, _3);

        return _0;
    }

    public static object Rule_declareStatement_Producton_3(Dictionary<int, object> objects) { 
        CCompilerNs.DeclareStatement _0 = new CCompilerNs.DeclareStatement();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        string _3 = (string)objects[3];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.DeclareStatement("struct " + _2, _3, null, null);

        return _0;
    }

    public static object Rule_assignmentStatement_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        CCompilerNs.AssignmentStatement _1 = (CCompilerNs.AssignmentStatement)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_emptyStatement_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.EmptyStatement _0 = new CCompilerNs.EmptyStatement();

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.EmptyStatement();

        return _0;
    }

    public static object Rule_singleLineComment_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.EmptyStatement _0 = new CCompilerNs.EmptyStatement();
        string _1 = (string)objects[1];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.EmptyStatement();

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        CCompilerNs.Expression _3 = (CCompilerNs.Expression)objects[3];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.AssignmentStatement(_1, _3, null);

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        List<CCompilerNs.Expression> _2 = (List<CCompilerNs.Expression>)objects[2];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.AssignmentStatement(_1, _4, _2);

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        CCompilerNs.Expression _3 = (CCompilerNs.Expression)objects[3];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement(_1, _3, null, "+");

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_3(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        List<CCompilerNs.Expression> _2 = (List<CCompilerNs.Expression>)objects[2];
        string _3 = (string)objects[3];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement(_1, _4, _2, "+");

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_4(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        CCompilerNs.Expression _3 = (CCompilerNs.Expression)objects[3];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement(_1, _3, null, "-");

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_5(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        List<CCompilerNs.Expression> _2 = (List<CCompilerNs.Expression>)objects[2];
        string _3 = (string)objects[3];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement(_1, _4, _2, "-");

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_6(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        CCompilerNs.Expression _3 = (CCompilerNs.Expression)objects[3];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement(_1, _3, null, "*");

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_7(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        List<CCompilerNs.Expression> _2 = (List<CCompilerNs.Expression>)objects[2];
        string _3 = (string)objects[3];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement(_1, _4, _2, "*");

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_8(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        CCompilerNs.Expression _3 = (CCompilerNs.Expression)objects[3];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement(_1, _3, null, "/");

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_9(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        List<CCompilerNs.Expression> _2 = (List<CCompilerNs.Expression>)objects[2];
        string _3 = (string)objects[3];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.OpAssignmentStatement(_1, _4, _2, "/");

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_10(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.IncrementDecrement(_1, null, "+");

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_11(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        List<CCompilerNs.Expression> _2 = (List<CCompilerNs.Expression>)objects[2];
        string _3 = (string)objects[3];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.IncrementDecrement(_1, _2, "+");

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_12(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.IncrementDecrement(_1, null, "-");

        return _0;
    }

    public static object Rule_assignmentNoSemicolon_Producton_13(Dictionary<int, object> objects) { 
        CCompilerNs.AssignmentStatement _0 = new CCompilerNs.AssignmentStatement();
        string _1 = (string)objects[1];
        List<CCompilerNs.Expression> _2 = (List<CCompilerNs.Expression>)objects[2];
        string _3 = (string)objects[3];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.IncrementDecrement(_1, _2, "-");

        return _0;
    }

    public static object Rule_addExpression_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _1 = (CCompilerNs.Expression)objects[1];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1);

        return _0;
    }

    public static object Rule_addExpression_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _1 =(CCompilerNs.Expression)objects[1];
        CCompilerNs.Expression _3 = (CCompilerNs.Expression)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1, "+", _3);

        return _0;
    }

    public static object Rule_addExpression_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _1 =(CCompilerNs.Expression)objects[1];
        CCompilerNs.Expression _3 = (CCompilerNs.Expression)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1, "-", _3);

        return _0;
    }

    public static object Rule_addExpression_LeftRecursionExpand_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();

        return _0;
    }

    public static object Rule_mulExpression_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _2 = (CCompilerNs.Expression)objects[2];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_2);

        return _0;
    }

    public static object Rule_mulExpression_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        int _1 = (int)objects[1];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1);

        return _0;
    }

    public static object Rule_mulExpression_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        string _1 = (string)objects[1];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1);

        return _0;
    }

    public static object Rule_mulExpression_Producton_3(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        string _1 = (string)objects[1];
        List<CCompilerNs.Expression> _2 = (List<CCompilerNs.Expression>)objects[2];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1, _2);

        return _0;
    }

    public static object Rule_mulExpression_Producton_4(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.FunctionCallExpression _1 = (CCompilerNs.FunctionCallExpression)objects[1];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1);

        return _0;
    }

    public static object Rule_mulExpression_Producton_5(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        char _1 = (char)objects[1];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1);

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _1 =(CCompilerNs.Expression)objects[1];
        int _3 = (int)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1, "*", _3);

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _1 =(CCompilerNs.Expression)objects[1];
        int _3 = (int)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1, "/", _3);

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _1 =(CCompilerNs.Expression)objects[1];
        char _3 = (char)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1, "*", _3);

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_3(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _1 =(CCompilerNs.Expression)objects[1];
        char _3 = (char)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1, "/", _3);

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_4(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _1 =(CCompilerNs.Expression)objects[1];
        string _3 = (string)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1, "*", _3);

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_5(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _1 =(CCompilerNs.Expression)objects[1];
        string _3 = (string)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1, "/", _3);

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_6(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _1 =(CCompilerNs.Expression)objects[1];
        CCompilerNs.FunctionCallExpression _3 = (CCompilerNs.FunctionCallExpression)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1, "*", _3);

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_7(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _1 =(CCompilerNs.Expression)objects[1];
        CCompilerNs.FunctionCallExpression _3 = (CCompilerNs.FunctionCallExpression)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1, "/", _3);

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_8(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _1 =(CCompilerNs.Expression)objects[1];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1, "*", _4);

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_9(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();
        CCompilerNs.Expression _1 =(CCompilerNs.Expression)objects[1];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.Expression(_1, "/", _4);

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_10(Dictionary<int, object> objects) { 
        CCompilerNs.Expression _0 = new CCompilerNs.Expression();

        return _0;
    }

    public static object Rule_funcParams_Producton_0(Dictionary<int, object> objects) { 
        List<CCompilerNs.Variable> _0 = new List<CCompilerNs.Variable>();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.FuncParams(_1, _2, null);

        return _0;
    }

    public static object Rule_funcParams_Producton_1(Dictionary<int, object> objects) { 
        List<CCompilerNs.Variable> _0 = new List<CCompilerNs.Variable>();
        string _1 = (string)objects[1];
        List<int> _2 = (List<int>)objects[2];
        string _3 = (string)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.FuncParamsArray(_1, _3, _2, null);

        return _0;
    }

    public static object Rule_funcParams_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        List<CCompilerNs.Variable> _0 = new List<CCompilerNs.Variable>();
        List<CCompilerNs.Variable> _1 =(List<CCompilerNs.Variable>)objects[1];
        string _3 = (string)objects[3];
        string _4 = (string)objects[4];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.FuncParams(_3, _4, _1);

        return _0;
    }

    public static object Rule_funcParams_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        List<CCompilerNs.Variable> _0 = new List<CCompilerNs.Variable>();
        List<CCompilerNs.Variable> _1 =(List<CCompilerNs.Variable>)objects[1];
        string _3 = (string)objects[3];
        List<int> _4 = (List<int>)objects[4];
        string _5 = (string)objects[5];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.FuncParamsArray(_3, _5, _4, _1);

        return _0;
    }

    public static object Rule_funcParams_LeftRecursionExpand_Producton_2(Dictionary<int, object> objects) { 
        List<CCompilerNs.Variable> _0 = new List<CCompilerNs.Variable>();

        return _0;
    }

    public static object Rule_funcCallParams_Producton_0(Dictionary<int, object> objects) { 
        List<CCompilerNs.Expression> _0 = new List<CCompilerNs.Expression>();
        CCompilerNs.Expression _1 = (CCompilerNs.Expression)objects[1];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.FuncCallParams(_1, null);

        return _0;
    }

    public static object Rule_funcCallParams_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        List<CCompilerNs.Expression> _0 = new List<CCompilerNs.Expression>();
        List<CCompilerNs.Expression> _1 =(List<CCompilerNs.Expression>)objects[1];
        CCompilerNs.Expression _3 = (CCompilerNs.Expression)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.FuncCallParams(_3, _1);

        return _0;
    }

    public static object Rule_funcCallParams_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        List<CCompilerNs.Expression> _0 = new List<CCompilerNs.Expression>();

        return _0;
    }

    public static object Rule_functionCallStatement_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.FunctionCallExpression _0 = new CCompilerNs.FunctionCallExpression();
        CCompilerNs.FunctionCallExpression _1 = (CCompilerNs.FunctionCallExpression)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_functionCallExpression_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.FunctionCallExpression _0 = new CCompilerNs.FunctionCallExpression();
        string _1 = (string)objects[1];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.FunctionCallExpression(_1, null);

        return _0;
    }

    public static object Rule_functionCallExpression_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.FunctionCallExpression _0 = new CCompilerNs.FunctionCallExpression();
        string _1 = (string)objects[1];
        List<CCompilerNs.Expression> _3 = (List<CCompilerNs.Expression>)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.FunctionCallExpression(_1, _3);

        return _0;
    }

    public static object Rule_breakStatement_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.BreakStatement _0 = new CCompilerNs.BreakStatement();
        string _1 = (string)objects[1];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.BreakStatement();

        return _0;
    }

    public static object Rule_continueStatement_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.ContinueStatement _0 = new CCompilerNs.ContinueStatement();
        string _1 = (string)objects[1];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.ContinueStatement();

        return _0;
    }

    public static object Rule_forLoopStatement_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.ForLoopStatement _0 = new CCompilerNs.ForLoopStatement();
        string _1 = (string)objects[1];
        CCompilerNs.AssignmentStatement _3 = (CCompilerNs.AssignmentStatement)objects[3];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];
        string _5 = (string)objects[5];
        CCompilerNs.Expression _6 = (CCompilerNs.Expression)objects[6];
        CCompilerNs.AssignmentStatement _8 = (CCompilerNs.AssignmentStatement)objects[8];
        List<CCompilerNs.Statement> _11 = (List<CCompilerNs.Statement>)objects[11];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.ForLoopStatement(_3, _4, _5, _6, _8, _11);

        return _0;
    }

    public static object Rule_forLoopStatement_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.ForLoopStatement _0 = new CCompilerNs.ForLoopStatement();
        string _1 = (string)objects[1];
        CCompilerNs.AssignmentStatement _3 = (CCompilerNs.AssignmentStatement)objects[3];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];
        string _5 = (string)objects[5];
        CCompilerNs.Expression _6 = (CCompilerNs.Expression)objects[6];
        CCompilerNs.AssignmentStatement _8 = (CCompilerNs.AssignmentStatement)objects[8];
        CCompilerNs.Statement _10 = (CCompilerNs.Statement)objects[10];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.ForLoopStatement(_3, _4, _5, _6, _8, _10);

        return _0;
    }

    public static object Rule_forLoopStatement_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.ForLoopStatement _0 = new CCompilerNs.ForLoopStatement();
        string _1 = (string)objects[1];
        CCompilerNs.AssignmentStatement _3 = (CCompilerNs.AssignmentStatement)objects[3];
        CCompilerNs.Expression _4 = (CCompilerNs.Expression)objects[4];
        string _5 = (string)objects[5];
        CCompilerNs.Expression _6 = (CCompilerNs.Expression)objects[6];
        CCompilerNs.AssignmentStatement _8 = (CCompilerNs.AssignmentStatement)objects[8];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.ForLoopStatement(_3, _4, _5, _6, _8);

        return _0;
    }

    public static object Rule_arraySize_Producton_0(Dictionary<int, object> objects) { 
        List<int> _0 = new List<int>();
        int _2 = (int)objects[2];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.ArraySize(_2, null);

        return _0;
    }

    public static object Rule_arraySize_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        List<int> _0 = new List<int>();
        List<int> _1 =(List<int>)objects[1];
        int _3 = (int)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.ArraySize(_3, _1);

        return _0;
    }

    public static object Rule_arraySize_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        List<int> _0 = new List<int>();

        return _0;
    }

    public static object Rule_paramArraySize_Producton_0(Dictionary<int, object> objects) { 
        List<int> _0 = new List<int>();

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.ParamArraySize(null);

        return _0;
    }

    public static object Rule_paramArraySize_Producton_1(Dictionary<int, object> objects) { 
        List<int> _0 = new List<int>();
        List<int> _3 = (List<int>)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.ParamArraySize(_3);

        return _0;
    }

    public static object Rule_arrayIndex_Producton_0(Dictionary<int, object> objects) { 
        List<CCompilerNs.Expression> _0 = new List<CCompilerNs.Expression>();
        CCompilerNs.Expression _2 = (CCompilerNs.Expression)objects[2];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.ArrayIndex(_2, null);

        return _0;
    }

    public static object Rule_arrayIndex_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        List<CCompilerNs.Expression> _0 = new List<CCompilerNs.Expression>();
        List<CCompilerNs.Expression> _1 =(List<CCompilerNs.Expression>)objects[1];
        CCompilerNs.Expression _3 = (CCompilerNs.Expression)objects[3];

        // user-defined action
        _0 = CCompilerNs.CCLexYaccCallback.ArrayIndex(_3, _1);

        return _0;
    }

    public static object Rule_arrayIndex_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        List<CCompilerNs.Expression> _0 = new List<CCompilerNs.Expression>();

        return _0;
    }

    public static object Rule_globalVariable_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.GlobalVariable _0 = new CCompilerNs.GlobalVariable();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.GlobalVariable(_1, _2, null, null);

        return _0;
    }

    public static object Rule_globalVariable_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.GlobalVariable _0 = new CCompilerNs.GlobalVariable();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        List<int> _3 = (List<int>)objects[3];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.GlobalVariable(_1, _2, null, _3);

        return _0;
    }

    public static object Rule_globalVariable_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.GlobalVariable _0 = new CCompilerNs.GlobalVariable();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        int _4 = (int)objects[4];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.GlobalVariable(_1, _2, _4, null);

        return _0;
    }

    public static object Rule_globalVariable_Producton_3(Dictionary<int, object> objects) { 
        CCompilerNs.GlobalVariable _0 = new CCompilerNs.GlobalVariable();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        char _4 = (char)objects[4];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.GlobalVariable(_1, _2, _4, null);

        return _0;
    }

    public static object Rule_structDef_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.StructDef _0 = new CCompilerNs.StructDef();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        List<CCompilerNs.StructField> _4 = (List<CCompilerNs.StructField>)objects[4];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.StructDef(_2, _4);

        return _0;
    }

    public static object Rule_structFields_Producton_0(Dictionary<int, object> objects) { 
        List<CCompilerNs.StructField> _0 = new List<CCompilerNs.StructField>();
        CCompilerNs.StructField _1 = (CCompilerNs.StructField)objects[1];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.StructFields(null, _1);

        return _0;
    }

    public static object Rule_structFields_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        List<CCompilerNs.StructField> _0 = new List<CCompilerNs.StructField>();
        List<CCompilerNs.StructField> _1 =(List<CCompilerNs.StructField>)objects[1];
        CCompilerNs.StructField _2 = (CCompilerNs.StructField)objects[2];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.StructFields(_1, _2);

        return _0;
    }

    public static object Rule_structFields_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        List<CCompilerNs.StructField> _0 = new List<CCompilerNs.StructField>();

        return _0;
    }

    public static object Rule_structField_Producton_0(Dictionary<int, object> objects) { 
        CCompilerNs.StructField _0 = new CCompilerNs.StructField();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.StructField(_1, _2, null);

        return _0;
    }

    public static object Rule_structField_Producton_1(Dictionary<int, object> objects) { 
        CCompilerNs.StructField _0 = new CCompilerNs.StructField();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        List<int> _3 = (List<int>)objects[3];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.StructField(_1, _2, _3);

        return _0;
    }

    public static object Rule_structField_Producton_2(Dictionary<int, object> objects) { 
        CCompilerNs.StructField _0 = new CCompilerNs.StructField();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        string _3 = (string)objects[3];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.StructField("struct " + _2, _3, null);

        return _0;
    }

    public static object Rule_structField_Producton_3(Dictionary<int, object> objects) { 
        CCompilerNs.StructField _0 = new CCompilerNs.StructField();
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        string _3 = (string)objects[3];
        List<int> _4 = (List<int>)objects[4];

        // user-defined action
        _0= CCompilerNs.CCLexYaccCallback.StructField("struct " + _2, _3, _4);

        return _0;
    }

    public static object Rule_relationlOp_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");

        // user-defined action
        _0 = "<";

        return _0;
    }

    public static object Rule_relationlOp_Producton_1(Dictionary<int, object> objects) { 
        string _0 = new string("");

        // user-defined action
        _0 = ">";

        return _0;
    }

    public static object Rule_relationlOp_Producton_2(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_relationlOp_Producton_3(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_relationlOp_Producton_4(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_relationlOp_Producton_5(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }
}

}


//Lex Gen 
namespace ccNs
{


    using LexYaccNs;
    public class LexActions
    {
        public static object value = null;

        public static Dictionary<string, Func<string, object>> actions = new Dictionary<string, Func<string, object>>();

        public static Dictionary<int, string> tokenDict = new Dictionary<int, string>
        {
            { 256, "CHAR_VALUE"},
            { 257, "INT_VALUE"},
            { 258, "RETURN"},
            { 259, "ID"},
            { 260, "INT_TYPE"},
            { 261, "VOID_TYPE"},
            { 262, "IF"},
            { 263, "ELSE"},
            { 264, "EQUAL_SIGN"},
            { 265, "NOT_EQUAL_SIGN"},
            { 266, "LESS_OR_EQUAL_SIGN"},
            { 267, "GREATER_OR_EQUAL_SIGN"},
            { 268, "FOR"},
            { 269, "BREAK"},
            { 270, "CONTINUE"},
            { 271, "INCREMENT"},
            { 272, "DECREMENT"},
            { 273, "PLUS_ASSIGN"},
            { 274, "MINUS_ASSIGN"},
            { 275, "MULTIPLY_ASSIGN"},
            { 276, "DIVIDE_ASSIGN"},
            { 277, "CHAR_TYPE"},
            { 278, "SINGLE_LINE_COMMENT"},
            { 279, "STRUCT"},
        };

        public static int CHAR_VALUE = 256;
        public static int INT_VALUE = 257;
        public static int RETURN = 258;
        public static int ID = 259;
        public static int INT_TYPE = 260;
        public static int VOID_TYPE = 261;
        public static int IF = 262;
        public static int ELSE = 263;
        public static int EQUAL_SIGN = 264;
        public static int NOT_EQUAL_SIGN = 265;
        public static int LESS_OR_EQUAL_SIGN = 266;
        public static int GREATER_OR_EQUAL_SIGN = 267;
        public static int FOR = 268;
        public static int BREAK = 269;
        public static int CONTINUE = 270;
        public static int INCREMENT = 271;
        public static int DECREMENT = 272;
        public static int PLUS_ASSIGN = 273;
        public static int MINUS_ASSIGN = 274;
        public static int MULTIPLY_ASSIGN = 275;
        public static int DIVIDE_ASSIGN = 276;
        public static int CHAR_TYPE = 277;
        public static int SINGLE_LINE_COMMENT = 278;
        public static int STRUCT = 279;

        public static void CallAction(List<Terminal> tokens, LexRule rule)
        {
            Init();
            object ret = actions[rule.ruleName](rule.yytext);
            if (ret is int && (int)ret == 0)
            {

            }
            else if (ret is char)
            {
                tokens.Add(Terminal.BuildConstCharTerminal((char)ret));
            }
            else
            {
                tokens.Add(Terminal.BuildToken(tokenDict[(int)ret], LexActions.value));
            }
        }

        public static string ruleInput = @"%{
%}

%% 
""int""                     { value = ""int""; return INT_TYPE; }
""void""                    { value = ""void""; return VOID_TYPE; }
""char""                    { value = ""char""; return CHAR_TYPE; }
""return""                  { value = ""return""; return RETURN; }
""if""                      { value = ""if""; return IF; }
""else""                    { value = ""else""; return ELSE; }
""for""                     { value = ""for""; return FOR; }
""break""                   { value = ""break""; return BREAK; }
""continue""                { value = ""continue""; return CONTINUE; }
""struct""                  { value = ""struct""; return STRUCT; }
""==""                      { value = ""==""; return EQUAL_SIGN; }
""!=""                      { value = ""!=""; return NOT_EQUAL_SIGN; }
""<=""                      { value = ""<=""; return LESS_OR_EQUAL_SIGN; }
"">=""                      { value = "">=""; return GREATER_OR_EQUAL_SIGN; }
""++""                      { value = ""++""; return INCREMENT; }
""--""                      { value = ""--""; return DECREMENT; }
""+=""                      { value = ""+=""; return PLUS_ASSIGN; }
""-=""                      { value = ""-=""; return MINUS_ASSIGN; }
""*=""                      { value = ""+=""; return MULTIPLY_ASSIGN; }
""/=""                      { value = ""+=""; return DIVIDE_ASSIGN; }
[0-9]+                    { value = int.Parse(yytext); return INT_VALUE; }
'[ -~]'                   { value = yytext[1]; return CHAR_VALUE; }
[_a-zA-Z][a-zA-Z0-9]*     { value = yytext; return ID; }
\/\/[^\n]*                { value = """";  return SINGLE_LINE_COMMENT; }  
[ \t\n\r]+                {}

""{""                       { return '{'; }
""}""                       { return '}'; }
""[""                       { return '['; }
""]""                       { return ']'; }
""<""                       { return '<'; }
"">""                       { return '>'; }
""(""                       { return '('; }
"")""                       { return ')'; }
"";""                       { return ';'; }
""+""                       { return '+'; }
""-""                       { return '-'; }
""*""                       { return '*'; }
""/""                       { return '/'; }
""=""                       { return '='; }
""%""                       { return '%'; }
"",""                       { return ','; }
%%";


        public static void Init()
        {
            if (actions.Count != 0)
                return;
            actions.Add("LexRule0", LexAction0);
            actions.Add("LexRule1", LexAction1);
            actions.Add("LexRule2", LexAction2);
            actions.Add("LexRule3", LexAction3);
            actions.Add("LexRule4", LexAction4);
            actions.Add("LexRule5", LexAction5);
            actions.Add("LexRule6", LexAction6);
            actions.Add("LexRule7", LexAction7);
            actions.Add("LexRule8", LexAction8);
            actions.Add("LexRule9", LexAction9);
            actions.Add("LexRule10", LexAction10);
            actions.Add("LexRule11", LexAction11);
            actions.Add("LexRule12", LexAction12);
            actions.Add("LexRule13", LexAction13);
            actions.Add("LexRule14", LexAction14);
            actions.Add("LexRule15", LexAction15);
            actions.Add("LexRule16", LexAction16);
            actions.Add("LexRule17", LexAction17);
            actions.Add("LexRule18", LexAction18);
            actions.Add("LexRule19", LexAction19);
            actions.Add("LexRule20", LexAction20);
            actions.Add("LexRule21", LexAction21);
            actions.Add("LexRule22", LexAction22);
            actions.Add("LexRule23", LexAction23);
            actions.Add("LexRule24", LexAction24);
            actions.Add("LexRule25", LexAction25);
            actions.Add("LexRule26", LexAction26);
            actions.Add("LexRule27", LexAction27);
            actions.Add("LexRule28", LexAction28);
            actions.Add("LexRule29", LexAction29);
            actions.Add("LexRule30", LexAction30);
            actions.Add("LexRule31", LexAction31);
            actions.Add("LexRule32", LexAction32);
            actions.Add("LexRule33", LexAction33);
            actions.Add("LexRule34", LexAction34);
            actions.Add("LexRule35", LexAction35);
            actions.Add("LexRule36", LexAction36);
            actions.Add("LexRule37", LexAction37);
            actions.Add("LexRule38", LexAction38);
            actions.Add("LexRule39", LexAction39);
            actions.Add("LexRule40", LexAction40);
        }
        public static object LexAction0(string yytext)
        {
            value = null;

            // user-defined action
            value = "int"; return INT_TYPE; 

            return 0;
        }
        public static object LexAction1(string yytext)
        {
            value = null;

            // user-defined action
            value = "void"; return VOID_TYPE; 

            return 0;
        }
        public static object LexAction2(string yytext)
        {
            value = null;

            // user-defined action
            value = "char"; return CHAR_TYPE; 

            return 0;
        }
        public static object LexAction3(string yytext)
        {
            value = null;

            // user-defined action
            value = "return"; return RETURN; 

            return 0;
        }
        public static object LexAction4(string yytext)
        {
            value = null;

            // user-defined action
            value = "if"; return IF; 

            return 0;
        }
        public static object LexAction5(string yytext)
        {
            value = null;

            // user-defined action
            value = "else"; return ELSE; 

            return 0;
        }
        public static object LexAction6(string yytext)
        {
            value = null;

            // user-defined action
            value = "for"; return FOR; 

            return 0;
        }
        public static object LexAction7(string yytext)
        {
            value = null;

            // user-defined action
            value = "break"; return BREAK; 

            return 0;
        }
        public static object LexAction8(string yytext)
        {
            value = null;

            // user-defined action
            value = "continue"; return CONTINUE; 

            return 0;
        }
        public static object LexAction9(string yytext)
        {
            value = null;

            // user-defined action
            value = "struct"; return STRUCT; 

            return 0;
        }
        public static object LexAction10(string yytext)
        {
            value = null;

            // user-defined action
            value = "=="; return EQUAL_SIGN; 

            return 0;
        }
        public static object LexAction11(string yytext)
        {
            value = null;

            // user-defined action
            value = "!="; return NOT_EQUAL_SIGN; 

            return 0;
        }
        public static object LexAction12(string yytext)
        {
            value = null;

            // user-defined action
            value = "<="; return LESS_OR_EQUAL_SIGN; 

            return 0;
        }
        public static object LexAction13(string yytext)
        {
            value = null;

            // user-defined action
            value = ">="; return GREATER_OR_EQUAL_SIGN; 

            return 0;
        }
        public static object LexAction14(string yytext)
        {
            value = null;

            // user-defined action
            value = "++"; return INCREMENT; 

            return 0;
        }
        public static object LexAction15(string yytext)
        {
            value = null;

            // user-defined action
            value = "--"; return DECREMENT; 

            return 0;
        }
        public static object LexAction16(string yytext)
        {
            value = null;

            // user-defined action
            value = "+="; return PLUS_ASSIGN; 

            return 0;
        }
        public static object LexAction17(string yytext)
        {
            value = null;

            // user-defined action
            value = "-="; return MINUS_ASSIGN; 

            return 0;
        }
        public static object LexAction18(string yytext)
        {
            value = null;

            // user-defined action
            value = "+="; return MULTIPLY_ASSIGN; 

            return 0;
        }
        public static object LexAction19(string yytext)
        {
            value = null;

            // user-defined action
            value = "+="; return DIVIDE_ASSIGN; 

            return 0;
        }
        public static object LexAction20(string yytext)
        {
            value = null;

            // user-defined action
            value = int.Parse(yytext); return INT_VALUE; 

            return 0;
        }
        public static object LexAction21(string yytext)
        {
            value = null;

            // user-defined action
            value = yytext[1]; return CHAR_VALUE; 

            return 0;
        }
        public static object LexAction22(string yytext)
        {
            value = null;

            // user-defined action
            value = yytext; return ID; 

            return 0;
        }
        public static object LexAction23(string yytext)
        {
            value = null;

            // user-defined action
            value = "";  return SINGLE_LINE_COMMENT; 

            return 0;
        }
        public static object LexAction24(string yytext)
        {
            value = null;

            return 0;
        }
        public static object LexAction25(string yytext)
        {
            value = null;

            // user-defined action
            return '{'; 

            return 0;
        }
        public static object LexAction26(string yytext)
        {
            value = null;

            // user-defined action
            return '}'; 

            return 0;
        }
        public static object LexAction27(string yytext)
        {
            value = null;

            // user-defined action
            return '['; 

            return 0;
        }
        public static object LexAction28(string yytext)
        {
            value = null;

            // user-defined action
            return ']'; 

            return 0;
        }
        public static object LexAction29(string yytext)
        {
            value = null;

            // user-defined action
            return '<'; 

            return 0;
        }
        public static object LexAction30(string yytext)
        {
            value = null;

            // user-defined action
            return '>'; 

            return 0;
        }
        public static object LexAction31(string yytext)
        {
            value = null;

            // user-defined action
            return '('; 

            return 0;
        }
        public static object LexAction32(string yytext)
        {
            value = null;

            // user-defined action
            return ')'; 

            return 0;
        }
        public static object LexAction33(string yytext)
        {
            value = null;

            // user-defined action
            return ';'; 

            return 0;
        }
        public static object LexAction34(string yytext)
        {
            value = null;

            // user-defined action
            return '+'; 

            return 0;
        }
        public static object LexAction35(string yytext)
        {
            value = null;

            // user-defined action
            return '-'; 

            return 0;
        }
        public static object LexAction36(string yytext)
        {
            value = null;

            // user-defined action
            return '*'; 

            return 0;
        }
        public static object LexAction37(string yytext)
        {
            value = null;

            // user-defined action
            return '/'; 

            return 0;
        }
        public static object LexAction38(string yytext)
        {
            value = null;

            // user-defined action
            return '='; 

            return 0;
        }
        public static object LexAction39(string yytext)
        {
            value = null;

            // user-defined action
            return '%'; 

            return 0;
        }
        public static object LexAction40(string yytext)
        {
            value = null;

            // user-defined action
            return ','; 

            return 0;
        }
    }
}


//Src files Gen
namespace ccNs{

namespace LexYaccNs
{
    using RegexNs;
    using System.Collections.Generic;

    public class Lex
    {
        public delegate void CallActionDelegate(List<Terminal> tokens, LexRule rule);

        public static List<Terminal> Parse(string input, string ruleInput, CallActionDelegate actionFunction)
        {
            List<Terminal> tokens = new List<Terminal>();

            List<LexRule> rules;
            Section section;
            LexRuleReader.Parse(ruleInput, out section, out rules);

            int start = 0;

            // find the longest match rule first.
            // If there are multiple rules match the same length, selec the first one
            while (start < input.Length)
            {
                LexRule matchedRule = null;

                for (int i = 0; i < rules.Count; i++)
                {
                    LexRule rule = rules[i];
                    rule.yytext = "";

                    if (rule.plainText != null)
                    {
                        if (input.Substring(start).StartsWith(rule.plainText))
                        {
                            rule.yytext = rule.plainText;
                            if (matchedRule == null)
                                matchedRule = rule;
                            else
                            {
                                if (rule.yytext.Length > matchedRule.yytext.Length)
                                    matchedRule = rule;
                            }
                        }
                    }
                    else
                    {
                        int prevAccept = -1;
                        RecognizeParam param = rule.nfa.CreateRecognizeParam();

                        for (int j = 0; start + j < input.Length; j++)
                        {
                            RecognizeResult result = rule.nfa.StepRecognize(input[start + j], param);
                            if (result == RecognizeResult.AliveAndAccept)
                                prevAccept = j;
                            else if (result == RecognizeResult.EndAndReject)
                                break;
                        }

                        if (prevAccept != -1)
                        {
                            rule.yytext = input.Substring(start, prevAccept + 1);

                            if (matchedRule == null)
                                matchedRule = rule;
                            else
                            {
                                if (rule.yytext.Length > matchedRule.yytext.Length)
                                    matchedRule = rule;
                            }
                        }
                    }
                }

                if (matchedRule == null)
                {
                    throw new Exception("Syntax Error, at pos " + start);
                }
                else
                {
                    actionFunction(tokens, matchedRule);
                    start += matchedRule.yytext.Length;
                }
            }

            return tokens;
        }
    }
}
}

namespace ccNs{

using System.Text;

namespace LexYaccNs
{
    public class LexCodeGen
    {
        public static string classDef = @"
    using LexYaccNs;
    public class LexActions
    {
        public static object value = null;

        public static Dictionary<string, Func<string, object>> actions = new Dictionary<string, Func<string, object>>();
";

        public static string callActionFunctionDef = @"
        public static void CallAction(List<Terminal> tokens, LexRule rule)
        {
            Init();
            object ret = actions[rule.ruleName](rule.yytext);
            if (ret is int && (int)ret == 0)
            {

            }
            else if (ret is char)
            {
                tokens.Add(Terminal.BuildConstCharTerminal((char)ret));
            }
            else
            {
                tokens.Add(Terminal.BuildToken(tokenDict[(int)ret], LexActions.value));
            }
        }
";

        public static string GenCode(string input, string namespaceStr, List<LexTokenDef> lexTokenDef)
        {
            List<LexRule> rules;
            Section section;
            LexRuleReader.Parse(input, out section, out rules);

            StringBuilder sb = new StringBuilder();
            string indent1 = "    ";
            string indent2 = "        ";
            string indent3 = "            ";

            sb.AppendLine("//Lex Gen ");
            sb.AppendLine("namespace " + namespaceStr);
            sb.AppendLine("{");
            sb.AppendLine(section.definitionSection);
            sb.AppendLine(classDef);

            sb.AppendLine(indent2 + "public static Dictionary<int, string> tokenDict = new Dictionary<int, string>");
            sb.AppendLine(indent2 + "{");
            foreach (LexTokenDef lexToken in lexTokenDef)
                sb.AppendLine(indent3 + "{ " + lexToken.index + ", \"" + lexToken.name + "\"},");
            sb.AppendLine(indent2 + "};");
            sb.AppendLine();

            foreach (LexTokenDef lexToken in lexTokenDef)
                sb.AppendLine(indent2 + "public static int " + lexToken.name + " = " + lexToken.index + ";");

            sb.AppendLine(callActionFunctionDef);

            sb.AppendLine(indent2 + "public static string ruleInput = @\"" + input.Replace("\"", "\"\"") + "\";\n");
            sb.AppendLine();
            sb.AppendLine(indent2 + "public static void Init()");
            sb.AppendLine(indent2 + "{");
            sb.AppendLine(indent2 + "    if (actions.Count != 0)");
            sb.AppendLine(indent2 + "        return;");

            for (int i = 0; i < rules.Count; i++)
            {
                LexRule rule = rules[i];
                sb.AppendLine(indent3 + "actions.Add(\"LexRule" + i + "\", LexAction" + i + ");");
            }
            sb.AppendLine(indent2 + "}");


            for (int i = 0; i < rules.Count; i++)
            {
                LexRule rule = rules[i];
                sb.AppendLine(indent2 + "public static object LexAction" + i + "(string yytext)");
                sb.AppendLine(indent2 + "{");
                sb.AppendLine(indent2 + "    value = null;");
                sb.AppendLine();

                if (rule.action.Trim().Length != 0)
                {
                    sb.AppendLine(indent3 + "// user-defined action");
                    sb.AppendLine(LexYaccUtil.FixGenCodeIndention(rule.action, indent3));
                }

                sb.AppendLine(indent2 + "    return 0;");
                sb.AppendLine(indent2 + "}");
            }

            sb.AppendLine(indent1 + "}");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine();

            Console.Write(sb.ToString());

            return sb.ToString();
        }

        public static void GenCode(string ruleSection, string name, List<LexTokenDef> lexTokenDef, string outputFolder, bool append)
        {
            string code = GenCode(ruleSection, name + "Ns", lexTokenDef);
            if (append)
                File.AppendAllText(Path.Combine(outputFolder, name + ".cs"), code);
            else
                File.WriteAllText(Path.Combine(outputFolder, name + ".cs"), code);
        }
    }
}

}

namespace ccNs{

using RegexNs;

namespace LexYaccNs
{
    public class LexRule
    {
        public LexRule(NFA nfa, string ruleName, string action)
        {
            this.nfa = nfa;
            this.ruleName = ruleName;
            this.action = action;
        }

        public LexRule(string plainText, string ruleName, string action)
        {
            this.plainText = plainText;
            this.ruleName = ruleName;
            this.action = action;
        }

        public string ruleName;
        public NFA nfa;
        public string plainText;
        public string action;

        public string yytext;
    }
}

}

namespace ccNs{

using RegexNs;

namespace LexYaccNs
{
    public class LexRuleReader
    {
        public static void Parse(string input, out Section sections, out List<LexRule> rules)
        {
            rules = new List<LexRule>();

            sections = YaccRuleReader.SplitSecction(input, false);
            string ruleSectionString = sections.ruleSection.Trim();

            while (ruleSectionString.Length > 0)
            {
                int leftBracket = LexYaccUtil.FindCharNotInLiteral(ruleSectionString, '{', false);
                string regex = ruleSectionString.Substring(0, leftBracket).Trim();

                // the case } in action:
                // "}"  { return '}'; }
                ruleSectionString = ruleSectionString.Substring(leftBracket + 1);
                int rightBracket = LexYaccUtil.FindCharNotInLiteral(ruleSectionString, '}', true);
                string action = LexYaccUtil.RemoveHeadAndTailEmptyLine(ruleSectionString.Substring(0, rightBracket));

                ruleSectionString = ruleSectionString.Substring(rightBracket + 1).Trim();

                regex = regex.Replace("\\/", "/");

                if (regex.StartsWith("\""))
                    rules.Add(new LexRule(regex.Substring(1, regex.Length - 2), "LexRule" + rules.Count, action));
                else
                    rules.Add(new LexRule(Regex.Compile(regex), "LexRule" + rules.Count, action));
            }
        }
    }
}

}

namespace ccNs{

namespace LexYaccNs
{

    public class LexYacc
    {
        public static object Parse(string input, string lexRule, string yaccRule, Lex.CallActionDelegate lexCallActionDelegate, Yacc.CallActionDelegate yaccActionDelegate)
        {
            List<Terminal> tokens = Lex.Parse(input, lexRule, lexCallActionDelegate);

            Yacc yacc = new Yacc(yaccRule);
            bool result = yacc.Feed(tokens);

            if (!result)
                return "syntax error";

            return yacc.route.startDFA.CallAction(yaccActionDelegate);
        }
    }

}
}

namespace ccNs{

using System.Text;

namespace LexYaccNs
{
    public class LexYaccCodeGen
    {
        public static void GenCode(string lexFile, string yaccFile, string outputFolder, string name)
        {
            string outputFile = Path.Combine(outputFolder, name + ".cs");

            StringBuilder sb = new StringBuilder();

            // LexYacc Gen
            sb.AppendLine("//LexYacc Gen");
            sb.AppendLine("public class " + name);
            sb.AppendLine("{");
            sb.AppendLine("    public static object Parse(string input)");
            sb.AppendLine("    {");
            // pairNs.LexActions.ruleInput, pairNs.YaccActions.ruleInput, 
            sb.AppendLine("        return " + name + "Ns.LexYaccNs.LexYacc.Parse(input, " + name + "Ns.LexActions.ruleInput, " + name + "Ns.YaccActions.ruleInput, " +
                name + "Ns.LexActions.CallAction, " + name + "Ns.YaccActions.CallAction);");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            File.WriteAllText(outputFile, sb.ToString());
            sb.AppendLine("");

            // Yacc Gen
            List<LexTokenDef> lexTokenDef = YaccCodeGen.GenCode(File.ReadAllText(yaccFile), name, outputFolder, true);

            // Lex Gen
            LexCodeGen.GenCode(File.ReadAllText(lexFile), name, lexTokenDef, outputFolder, true);

            // Src files Gen
            File.AppendAllText(outputFile, "//Src files Gen");
            string[] allFiles = Directory.GetFiles("../../../LexYaccNs", "*", SearchOption.AllDirectories);
            foreach (string file in allFiles)
            {
                if (file.EndsWith(".cs"))
                    AppendFile(outputFile, file, name + "Ns");
            }

            allFiles = Directory.GetFiles("../../../RegexNs", "*", SearchOption.AllDirectories);
            foreach (string file in allFiles)
            {
                if (file.EndsWith(".cs"))
                    AppendFile(outputFile, file, name + "Ns");
            }
        }

        private static void AppendFile(string outputFile, string file, string ns)
        {
            File.AppendAllText(outputFile, "\nnamespace " + ns + "{\n\n");
            File.AppendAllText(outputFile, File.ReadAllText(file));
            File.AppendAllText(outputFile, "\n}\n");
        }
    }
}

}

namespace ccNs{

using System.Text;

namespace LexYaccNs
{
    public class LexYaccUtil
    {
        public static int FindCharNotInLiteral(string s, char c, bool includeSingleQuote)
        {
            return FindCharNotInLiteral(s, new List<char>() { c }, includeSingleQuote);
        }

        public static int FindCharNotInLiteral(string s, List<char> chars, bool includeSingleQuote)
        {
            List<string> stringList = new List<string>();

            foreach (char c in chars)
                stringList.Add(c.ToString());

            return FindStringNotInLiteral(s, stringList, includeSingleQuote);
        }

        public static int FindStringNotInLiteral(string s, string s2, bool includeSingleQuote)
        {
            return FindStringNotInLiteral(s, new List<string>() { s2 }, includeSingleQuote);
        }

        public static int FindStringNotInLiteral(string s, List<string> strings, bool includeSingleQuote)
        {
            bool singleQuote = false;
            bool doubleQuote = false;
            bool comment = false;

            for (int i = 0; i < s.Length; i++)
            {
                if (includeSingleQuote && s[i] == '\'')
                    singleQuote = !singleQuote;
                else if (s[i] == '"')
                    doubleQuote = !doubleQuote;
                else if (s[i] == '\n')
                    comment = false;
                else if (s[i] == '/' && i < s.Length - 1 && s[i + 1] == '/')
                    comment = true;

                if (!singleQuote && !doubleQuote && !comment)
                {
                    foreach (string s2 in strings)
                    {
                        if (i + s2.Length > s.Length)
                            continue;

                        bool found = true;
                        for (int j = 0; j < s2.Length; j++)
                        {
                            if (s[i + j] != s2[j])
                            {
                                found = false;
                                break;
                            }
                        }

                        if (found)
                            return i;
                    }
                }

            }

            return -1;
        }

        public static string GetGenFileFolder()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "../../../GenFile");
        }

        public static string FixGenCodeIndention(string input, string indention)
        {
            StringBuilder sb = new StringBuilder();

            int minIndentCount = int.MaxValue;
            using (StringReader reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int indentCount = 0;
                    for (; indentCount < line.Length; indentCount++)
                        if (line[indentCount] != ' ')
                            break;
                    if (indentCount < minIndentCount)
                        minIndentCount = indentCount;
                }
            }
            using (StringReader reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    sb.AppendLine(indention + line.Substring(minIndentCount));
                }
            }

            return sb.ToString();
        }

        public static string RemoveHeadAndTailEmptyLine(string input)
        {
            string[] lines = input.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            StringBuilder sb = new StringBuilder();

            int start = 0;
            for (; start < lines.Length && lines[start].Trim().Length == 0; start++)
                ;

            int end = lines.Length - 1;
            for (; end >= 0 && lines[end].Trim().Length == 0; end--)
                ;

            for (int i = start; i < lines.Length && i <= end; i++)
                sb.Append(lines[i] + "\n");

            return sb.ToString();
        }
    }



    public class SectionSplitter
    {

    }
}
}

namespace ccNs{

/*
Todo:

indirect left recursive, action calling

===

Term:

A: 'B' c | 'D' e

Production rule = A: 'B' c | 'D' e
Production body = 'B' c | 'D' e
Production = 'B' c
Production = 'D' e



===
not support:

1. 
Do not support empty rule like a: | 'A'
empty rule is used only in left recursion

 */
}

namespace ccNs{

namespace LexYaccNs
{
    public class LexTokenDef
    {
        public string name;
        public string type;
        public int index;
    }

    public enum Result
    {
        Alive,
        Accepted,
        Rejected,
    }

    public class Route
    {
        public DFA startDFA = null;
        public int lexTokenIndex = -1;
        public Stack<DFA> dfaStack = new Stack<DFA>();
        public Result result = Result.Alive;
    }

    public class Yacc
    {
        public string input = "";
        public Section sections = null;
        public List<YaccRule> productionRules = null;
        public List<LexTokenDef> lexTokenDef = null;
        public Dictionary<string, string> ruleNonterminalType = null;
        public List<Terminal> lexTokens = new List<Terminal>();

        public List<Route> routes = new List<Route>();
        public Route route = null;

        public delegate object CallActionDelegate(string functionName, Dictionary<int, object> param);

        public Yacc()
        {

        }

        public Yacc(string input)
        {
            this.input = input;
            YaccRuleReader.Parse(input, out sections, out productionRules, out lexTokenDef, out ruleNonterminalType);
            Rebuild();
        }

        public void Rebuild()
        {
            Route route = new Route();
            route.startDFA = new DFA(this, productionRules[0].productions[0]);
            route.dfaStack.Push(route.startDFA);
            route.lexTokenIndex = 0;
            route.result = Result.Alive;

            this.route = route;
            routes.Clear();
            routes.Add(route);
        }

        public Route CloneRoute(int lexTokenIndex)
        {
            Dictionary<DFA, DFA> oldDFAtoNewDFAMapping = new Dictionary<DFA, DFA>();

            DFA newStartDFA = route.startDFA.clone(oldDFAtoNewDFAMapping);
            Route newRoute = new Route();
            newRoute.startDFA = newStartDFA;
            newRoute.lexTokenIndex = lexTokenIndex;
            newRoute.result = Result.Alive;

            // restore stack
            Stack<DFA> reverse = new Stack<DFA>(route.dfaStack);
            while (reverse.Count > 0)
            {
                DFA oldDFA = reverse.Pop();
                newRoute.dfaStack.Push(oldDFAtoNewDFAMapping[oldDFA]);
            }

            return newRoute;
        }

        public void ExpandNonterminal(int lexTokenIndex)
        {
            DFA dfa = route.dfaStack.Peek();
            while (dfa.states[dfa.currentState].symbol is Nonterminal)
            {
                if (dfa.states[dfa.currentState].nonterminalDFA == null)
                {
                    Nonterminal nt = (Nonterminal)dfa.states[dfa.currentState].symbol;
                    List<Production> productions = GetProductions(nt.name);

                    // create new route
                    for (int i = 1; i < productions.Count; i++)
                    {
                        Route newRoute = CloneRoute(lexTokenIndex);

                        DFA newDFA = new DFA(this, productions[i]);
                        newRoute.dfaStack.Peek().states[dfa.currentState].nonterminalDFA = newDFA;
                        newRoute.dfaStack.Push(newDFA);

                        routes.Add(newRoute);
                    }

                    DFA newDFA2 = new DFA(this, productions[0]);
                    dfa.states[dfa.currentState].nonterminalDFA = newDFA2;
                    route.dfaStack.Push(newDFA2);
                }
                dfa = dfa.states[dfa.currentState].nonterminalDFA;
            }
        }

        public void ExpandAndFeedEmpty(int lexTokenIndex)
        {
            bool continueFeed = true;

            while (continueFeed)
            {
                if (route.dfaStack.Count == 0)
                    break;

                ExpandNonterminal(lexTokenIndex);

                DFA dfa = route.dfaStack.Peek();
                continueFeed = false;

                while (true)
                {
                    if (dfa.production.IsEmptyProduction())
                    {
                        route.dfaStack.Peek().Feed(this, lexTokenIndex, true);
                        continueFeed = true;
                        break;
                    }

                    if (dfa.states[dfa.currentState].nonterminalDFA != null)
                        dfa = dfa.states[dfa.currentState].nonterminalDFA;
                    else
                        break;
                }
            }
        }

        public bool FeedInternal()
        {
            ExpandAndFeedEmpty(route.lexTokenIndex);

            for (; route.lexTokenIndex < lexTokens.Count; route.lexTokenIndex++)
            {
                if (route.dfaStack.Count == 0)
                    return false;

                route.dfaStack.Peek().Feed(this, route.lexTokenIndex, false);
                if (route.result == Result.Rejected)
                    return false;

                ExpandAndFeedEmpty(route.lexTokenIndex + 1);
            }

            if (route.result == Result.Accepted)
                return true;
            else
                return false;
        }

        public bool Feed(List<Terminal> lexTokens)
        {
            this.lexTokens = lexTokens;

            while (routes.Count > 0)
            {
                route = routes[routes.Count - 1];
                routes.RemoveAt(routes.Count - 1);
                bool result = FeedInternal();
                if (result == true)
                    return true;
            }

            return false;
        }

        public void AdvanceToNextState()
        {
            route.dfaStack.Pop();
            if (route.dfaStack.Count == 0)
            {
                route.result = Result.Accepted;
                return;
            }

            DFA dfa = route.dfaStack.Peek();
            dfa.currentState++;
            if (dfa.currentState == dfa.acceptedState)
                AdvanceToNextState();
        }

        public List<Production> GetProductions(string name)
        {
            foreach (YaccRule pr in productionRules)
                if (pr.lhs.name == name)
                    return pr.productions;
            return null;
        }

        public YaccRule GetYaccRule(string lhsName)
        {
            foreach (var r in productionRules)
            {
                if (r.lhs.name == lhsName)
                    return r;
            }

            return null;
        }

        public override string ToString()
        {
            string ret = "";

            foreach (YaccRule pr in productionRules)
            {
                foreach (Production p in pr.productions)
                {
                    ret += p.ToString() + ";\n";
                }
            }

            return ret;
        }
    }



}
}

namespace ccNs{

using System.Text;

namespace LexYaccNs
{
    public class YaccCodeGen
    {
        public static List<LexTokenDef> GenCode(string input, string name, string outputFolder, bool append)
        {
            Section sections;
            List<YaccRule> productionRules;
            List<LexTokenDef> lexTokenDef;
            Dictionary<string, string> ruleNonterminalType;

            YaccRuleReader.Parse(input, out sections, out productionRules, out lexTokenDef, out ruleNonterminalType);

            string code = GenCode(input, sections, productionRules, lexTokenDef, ruleNonterminalType, name + "Ns");
            if (append)
                File.AppendAllText(Path.Combine(outputFolder, name + ".cs"), code);
            else
                File.WriteAllText(Path.Combine(outputFolder, name + ".cs"), code);

            return lexTokenDef;
        }

        public static string GenCode(string ruleInput, Section sections, List<YaccRule> productionRules, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType, string namespaceStr)
        {
            string indent1 = "    ";
            string indent2 = "        ";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("//Yacc Gen ");
            sb.AppendLine("namespace " + namespaceStr);
            sb.AppendLine("{");
            sb.AppendLine();

            sb.AppendLine(sections.definitionSection);
            sb.AppendLine();

            sb.AppendLine("public class YaccActions{");
            sb.AppendLine();
            sb.AppendLine(indent1 + "public static Dictionary<string, Func<Dictionary<int, object>, object>> actions = new Dictionary<string, Func<Dictionary<int, object>, object>>();");
            sb.AppendLine();
            sb.AppendLine(indent1 + "public static string ruleInput = @\"" + ruleInput.Replace("\"", "\"\"") + "\";\n");
            sb.AppendLine();
            sb.AppendLine(indent1 + "public static object CallAction(string functionName, Dictionary<int, object> param)");
            sb.AppendLine(indent1 + "{");
            sb.AppendLine(indent1 + "    Init();");
            sb.AppendLine(indent1 + "    if (!actions.ContainsKey(functionName))");
            sb.AppendLine(indent1 + "        return null;");
            sb.AppendLine(indent1 + "    return actions[functionName](param);");
            sb.AppendLine(indent1 + "}");
            sb.AppendLine();
            sb.AppendLine(indent1 + "public static void Init()");
            sb.AppendLine(indent1 + "{");
            sb.AppendLine(indent2 + "if (actions.Count != 0)");
            sb.AppendLine(indent2 + "    return;");
            sb.AppendLine();

            foreach (YaccRule rule in productionRules)
            {
                foreach (Production p in rule.productions)
                {
                    if (p.action == null)
                        continue;
                    string functionName = string.Format("Rule_{0}_Producton_{1}", p.lhs.name, p.index);
                    sb.AppendLine(indent2 + string.Format("actions.Add(\"{0}\", {1});", functionName, functionName));
                }
            }
            sb.AppendLine(indent1 + "}");

            foreach (YaccRule rule in productionRules)
            {
                foreach (Production p in rule.productions)
                {
                    if (p.action != null)
                    {
                        sb.AppendLine();
                        sb.AppendLine(indent1 + string.Format("public static object Rule_{0}_Producton_{1}(Dictionary<int, object> objects) {{ ", p.lhs.name, p.index));

                        string type = ruleNonterminalType[rule.lhs.name];

                        sb.Append(indent2 + string.Format("{0} {1}", type, "_0 = new " + type));
                        if (type == "string" || type == "String")
                            sb.AppendLine("(\"\");");
                        else
                            sb.AppendLine("();");

                        if (p.IsEmptyProduction())
                        {

                        }
                        else
                        {
                            if (p.type == ProductionType.LeftRecursiveSecond)
                                sb.AppendLine(indent2 + string.Format("{0} {1} =({2})objects[1]", type, "_1", type) + ";");

                            for (int i = 0; i < p.symbols.Count - (p.type == ProductionType.Plain ? 0 : 1); i++)
                            {
                                Symbol symbol = p.symbols[i];
                                string typeName = "";


                                if (symbol is Terminal)
                                {
                                    Terminal t = (Terminal)symbol;
                                    if (t.type == TerminalType.CONSTANT_CHAR || t.type == TerminalType.EMPTY)
                                        continue;

                                    typeName = "";
                                    foreach (LexTokenDef l in lexTokenDef)
                                    {
                                        if (l.name == t.tokenName)
                                            typeName = l.type;
                                    }
                                }
                                else
                                {
                                    Nonterminal nt = (Nonterminal)symbol;
                                    typeName = ruleNonterminalType[nt.name];
                                }

                                int shift = p.type == ProductionType.LeftRecursiveSecond ? 1 : 0;
                                sb.AppendLine(indent2 + string.Format("{0} {1} = ({2})objects[{3}]", typeName, "_" + (i + 1 + shift).ToString(), typeName, i + 1 + shift) + ";");

                            }
                        }

                        sb.AppendLine();

                        if (p.action.Trim().Length != 0)
                        {
                            sb.AppendLine(indent2 + "// user-defined action");
                            sb.AppendLine(LexYaccUtil.FixGenCodeIndention(p.action, indent2));
                        }

                        sb.AppendLine(indent2 + "return _0;");
                        sb.AppendLine(indent1 + "}");
                    }
                }
            }

            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }
    }
}

}

namespace ccNs{

namespace LexYaccNs
{

    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class State
    {
        public Symbol symbol;
        public DFA nonterminalDFA = null;

        public State()
        { }

        public State(Symbol s)
        {
            this.symbol = s;
        }
    }

    public class DFA
    {
        public int startState = 0;
        public int acceptedState = -1;
        public int currentState = 0;

        public List<State> states = new List<State>();

        public Dictionary<int, object> tokenObjects = new Dictionary<int, object>();
        Dictionary<int, object> param = new Dictionary<int, object>();

        public Production production = null;
        public Yacc yacc = null;

        public DFA()
        {
        }

        public DFA(Yacc yacc, Production p)
        {
            production = p;
            this.yacc = yacc;

            if (p.symbols.Count == 0)
            {
                // empty production
                startState = states.Count;
                states.Add(new State());

                acceptedState = states.Count;
                states.Add(new State());

                states[startState].symbol = Terminal.BuildEmptyTerminal();

                currentState = startState;
            }
            else
            {
                foreach (Symbol s in p.symbols)
                    states.Add(new State(s));

                acceptedState = states.Count;
                states.Add(new State());
                startState = 0;
                currentState = 0;
            }
        }

        public void Feed(Yacc yacc, int lexTokenIndex, bool empty)
        {
            if (states[currentState].symbol is Nonterminal)
            {
                if (yacc.route.dfaStack.Count == 0 || yacc.route.dfaStack.Peek() != states[currentState].nonterminalDFA)
                    yacc.route.dfaStack.Push(states[currentState].nonterminalDFA);

                yacc.route.dfaStack.Peek().Feed(yacc, lexTokenIndex, empty);
            }
            else
            {
                Terminal lexToken = null;
                if (!empty)
                    lexToken = yacc.lexTokens[lexTokenIndex];
                else
                    lexToken = Terminal.BuildEmptyTerminal();

                if (production.IsEmptyProduction())
                {
                    // empty
                    if (lexToken.type == TerminalType.EMPTY)
                    {
                        yacc.AdvanceToNextState();
                        return;
                    }
                    else
                    {
                        yacc.route.result = Result.Rejected;
                        return;
                    }
                }
                else
                {
                    if (lexToken.type == TerminalType.EMPTY)
                    {

                    }
                    else
                    {
                        Terminal stateTerminal = (Terminal)states[currentState].symbol;

                        if (stateTerminal.type == lexToken.type)
                        {
                            if (stateTerminal.type == TerminalType.CONSTANT_CHAR)
                            {
                                if (stateTerminal.constCharValue == lexToken.constCharValue)
                                {
                                    currentState++;
                                    if (currentState == acceptedState)
                                    {
                                        yacc.AdvanceToNextState();
                                        return;
                                    }
                                }
                                else
                                {
                                    yacc.route.result = Result.Rejected;
                                    return;
                                }
                            }
                            else if (stateTerminal.type == TerminalType.TOKEN)
                            {
                                if (stateTerminal.tokenName == lexToken.tokenName)
                                {
                                    tokenObjects[currentState] = lexToken.tokenObject;

                                    currentState++;
                                    if (currentState == acceptedState)
                                    {
                                        yacc.AdvanceToNextState();
                                        return;
                                    }
                                }
                                else
                                {
                                    yacc.route.result = Result.Rejected;
                                    return;
                                }
                            }
                            else
                            {
                                Trace.Assert(false);
                            }
                        }
                        else
                        {
                            yacc.route.result = Result.Rejected;
                            return;
                        }
                    }
                }
            }
        }

        public object CallPlainAction(Yacc.CallActionDelegate invokeFunction)
        {
            if (production.IsEmptyProduction())
                return param[1];

            for (int i = 0; i < production.symbols.Count; i++)
            {
                Symbol symbol = production.symbols[i];

                if (symbol is Terminal)
                {
                    Terminal t = (Terminal)symbol;
                    if (t.type == TerminalType.CONSTANT_CHAR || t.type == TerminalType.EMPTY)
                        continue;

                    if (production.type == ProductionType.LeftRecursiveSecond)
                        param[i + 2] = tokenObjects[i];
                    else
                        param[i + 1] = tokenObjects[i];
                }
                else
                {
                    if (production.type == ProductionType.LeftRecursiveSecond)
                        param[i + 2] = states[i].nonterminalDFA.CallAction(invokeFunction);
                    else
                        param[i + 1] = states[i].nonterminalDFA.CallAction(invokeFunction);
                }
            }

            return invokeFunction(production.GetFunctionName(), param);
        }

        /*

        a: a 'A' | 'B'  =>

        a: 'B' a2
        a2: 'A' a2 | empty

         */

        public object CallLeftRecursionAction(Yacc.CallActionDelegate invokeFunction)
        {
            if (production.IsEmptyProduction())
                return param[1];

            for (int i = 0; i < production.symbols.Count - 1; i++)
            {
                Symbol symbol = production.symbols[i];

                if (symbol is Terminal)
                {
                    Terminal t = (Terminal)symbol;
                    if (t.type == TerminalType.CONSTANT_CHAR)
                        continue;

                    if (production.type == ProductionType.LeftRecursiveSecond)
                        param[i + 2] = tokenObjects[i];
                    else
                        param[i + 1] = tokenObjects[i];
                }
                else
                {
                    if (production.type == ProductionType.LeftRecursiveSecond)
                        param[i + 2] = states[i].nonterminalDFA.CallAction(invokeFunction);
                    else
                        param[i + 1] = states[i].nonterminalDFA.CallAction(invokeFunction);
                }
            }

            object o = invokeFunction(production.GetFunctionName(), param);
            states[production.symbols.Count - 1].nonterminalDFA.param[1] = o;

            return states[production.symbols.Count - 1].nonterminalDFA.CallAction(invokeFunction);
        }

        public object CallAction(Yacc.CallActionDelegate invokeFunction)
        {
            if (production.type == ProductionType.Plain)
                return CallPlainAction(invokeFunction);
            else
                return CallLeftRecursionAction(invokeFunction);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(this);
        }

        public DFA clone(Dictionary<DFA, DFA> oldDFAtoNewDFAMapping)
        {
            DFA newDFA = new DFA();
            newDFA.startState = this.startState;
            newDFA.acceptedState = this.acceptedState;
            newDFA.currentState = this.currentState;

            foreach (var tokenObject in tokenObjects)
                newDFA.tokenObjects.Add(tokenObject.Key, tokenObject.Value);
            foreach (var p in param)
                newDFA.param.Add(p.Key, p.Value);

            newDFA.production = this.production;
            newDFA.yacc = this.yacc;

            oldDFAtoNewDFAMapping.Add(this, newDFA);

            for (int i = 0; i < this.states.Count; i++)
            {
                newDFA.states.Add(new State(states[i].symbol));

                State s = this.states[i];
                if (s.nonterminalDFA != null)
                {
                    newDFA.states[i].nonterminalDFA = s.nonterminalDFA.clone(oldDFAtoNewDFAMapping);
                }
            }

            return newDFA;
        }
    }

}
}

namespace ccNs{

using System.Runtime.CompilerServices;

namespace LexYaccNs
{

    /*
    Term:

    A: 'B' c | 'D' e

    Production rule = A: 'B' c | 'D' e
    Production body = 'B' c | 'D' e
    Production = 'B' c
    Production = 'D' e
    */

    public class YaccRule
    {
        public YaccRule()
        {

        }

        public YaccRule(Nonterminal lhs, List<Production> productions)
        {
            this.lhs = lhs;
            this.productions = productions;
        }

        public Nonterminal lhs;
        public List<Production> productions = new List<Production>();

        public override string ToString()
        {
            string ret = "";

            foreach (Production p in productions)
                ret += p.ToString() + "\n";

            return ret;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(this);
        }
    }

    public enum ProductionType
    {
        Plain,
        LeftRecursiveFirst,
        LeftRecursiveSecond,
    }

    public class Production
    {
        public List<Symbol> symbols = null;
        public string? action = null;
        public Nonterminal lhs = null;
        public int index = -1;
        public List<LexTokenDef> lexTokenDef;
        public Dictionary<string, string> ruleNonterminalType;
        public ProductionType type = ProductionType.Plain;

        public Production(Nonterminal lhs, List<Symbol> symbols, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType, string action, ProductionType type)
        {
            this.lhs = lhs;
            this.symbols = symbols;
            this.lexTokenDef = lexTokenDef;
            this.ruleNonterminalType = ruleNonterminalType;
            this.action = action;
            this.type = type;
        }

        public Production Clone()
        {
            List<Symbol> cloneSymbols = new List<Symbol>();
            foreach (Symbol s in symbols)
                cloneSymbols.Add(s.Clone());

            Production p = new Production(lhs, cloneSymbols, lexTokenDef, ruleNonterminalType, action, type);
            p.index = index;
            return p;
        }

        public string GetFunctionName()
        {
            return string.Format("Rule_{0}_Producton_{1}", lhs.name, index);
        }

        public bool IsEmptyProduction()
        {
            return symbols.Count == 1 && symbols[0] is Terminal && ((Terminal)symbols[0]).type == TerminalType.EMPTY;
        }

        public override string ToString()
        {
            string s = lhs.name + ":";

            foreach (Symbol symbol in symbols)
            {
                if (symbol is Terminal)
                {
                    Terminal t = (Terminal)symbol;
                    if (t.type == TerminalType.TOKEN)
                        s += " " + t.tokenName;
                    else if (t.type == TerminalType.CONSTANT_CHAR)
                        s += " '" + t.constCharValue + "'";
                }
                else if (symbol is Nonterminal)
                {
                    Nonterminal n = (Nonterminal)symbol;
                    s += " " + n.name;
                }
            }

            if (action != null)
                s += " {" + action + "}";

            return s;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(this);
        }
    }


    public interface Symbol
    {
        public Symbol Clone();
    }

    public class Nonterminal : Symbol
    {
        public string name;

        public Nonterminal(string name)
        {
            this.name = name;
        }

        public Symbol Clone()
        {
            return new Nonterminal(name);
        }
    }

    // %token < strVal > VOID INT
    public enum TerminalType
    {
        TOKEN,
        CONSTANT_CHAR,
        EMPTY, // for empty rule
        None
    }

    public class Terminal : Symbol
    {
        public static Terminal BuildToken(string tokenName)
        {
            Terminal t = new Terminal();
            t.tokenName = tokenName;
            t.type = TerminalType.TOKEN;

            return t;
        }

        public static Terminal BuildToken(string tokenName, object tokenObject)
        {
            Terminal t = new Terminal();
            t.tokenName = tokenName;
            t.type = TerminalType.TOKEN;
            t.tokenObject = tokenObject;

            return t;
        }

        public static Terminal BuildConstCharTerminal(char constCharValue)
        {
            Terminal t = new Terminal();
            t.constCharValue = "" + constCharValue;
            t.type = TerminalType.CONSTANT_CHAR;

            return t;
        }

        public static Terminal BuildEmptyTerminal()
        {
            Terminal t = new Terminal();
            t.type = TerminalType.EMPTY;

            return t;
        }

        public string tokenName;
        public object tokenObject;
        public string constCharValue;
        public TerminalType type;

        public Symbol Clone()
        {
            Terminal t = new Terminal();
            t.type = type;
            t.tokenName = tokenName;
            t.tokenObject = tokenObject;
            t.constCharValue = constCharValue;

            return t;
        }
    }

}
}

namespace ccNs{

using System.Text;

namespace LexYaccNs
{
    public class Section
    {
        public string definitionSection;
        public string typeSection;
        public string ruleSection;
    }

    public class TypeSectionParser
    {
        // %token <intVal> CONSTANT
        // %type <astVal> program declList decl funDecl typeSpec returnStmt funName param paramList id constant
        public static Tuple<List<LexTokenDef>, Dictionary<string, string>> Parse(string input)
        {
            List<LexTokenDef> lexTokenDef = new List<LexTokenDef>();
            int tokenIndex = 256;
            Dictionary<string, string> ruleNonterminalType = new Dictionary<string, string>();



            if (input != null)
            {
                using (StringReader reader = new StringReader(input))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.Length == 0)
                            continue;

                        bool isToken = false;
                        if (line.StartsWith("%token"))
                        {
                            isToken = true;
                        }
                        else if (line.StartsWith("%type"))
                        {
                            isToken = false;
                        }
                        else
                        {
                            throw new Exception("Syntax error");
                        }


                        int start = line.IndexOf('<');
                        int end = line.IndexOf('>');

                        // the case <List<List<string>>>
                        for (; line[end + 1] == '>'; end++)
                            ;

                        string type = line.Substring(start + 1, end - start - 1);
                        line = line.Substring(end + 1);

                        string[] symbols = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string symbol in symbols)
                        {
                            if (isToken)
                            {
                                LexTokenDef lexTokenDef2 = new LexTokenDef();
                                lexTokenDef2.type = type;
                                lexTokenDef2.name = symbol;
                                lexTokenDef2.index = tokenIndex++;
                                lexTokenDef.Add(lexTokenDef2);
                            }
                            else
                                ruleNonterminalType.Add(symbol, type);
                        }
                    }
                }
            }

            return new Tuple<List<LexTokenDef>, Dictionary<string, string>>(lexTokenDef, ruleNonterminalType);
        }
    }

    public class RuleSectionParser
    {
        public static List<YaccRule> Parse(string input)
        {
            return Parse(input, new List<LexTokenDef>(), new Dictionary<string, string>());
        }

        public static List<YaccRule> Parse(string input, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            List<YaccRule> allRules = new List<YaccRule>();

            YaccRule rule = ReadRule(ref input, lexTokenDef, ruleNonterminalType);
            Dictionary<string, YaccRule> nameToYaccRuleMap = new Dictionary<string, YaccRule>();

            while (rule != null)
            {
                allRules.Add(rule);
                nameToYaccRuleMap.Add(rule.lhs.name, rule);
                rule = ReadRule(ref input, lexTokenDef, ruleNonterminalType);
            }

            ConvertIndirectLeftRecursion(allRules, lexTokenDef, ruleNonterminalType, nameToYaccRuleMap);

            allRules = ConvertLeftRecursion(allRules, lexTokenDef, ruleNonterminalType);

            foreach (YaccRule r in allRules)
            {
                for (int i = 0; i < r.productions.Count; i++)
                {
                    Production p = r.productions[i];
                    p.index = i;
                    p.action = ConvertActionVariable(p.action);
                }
            }

            return allRules;
        }

        private static string ConvertActionVariable(string action)
        {
            if (action == null)
                return null;

            action = action.Replace("$$", "_0");

            StringBuilder sb = new StringBuilder();

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < action.Length; i++)
            {
                if (action[i] == '$' && i + 1 < action.Length && char.IsDigit(action[i + 1]))
                {
                    // Replace '$' with '_'
                    sb.Append('_');
                }
                else
                {
                    sb.Append(action[i]);
                }
            }
            return sb.ToString();
        }

        public static Production ReadProduction2(string productionString, Nonterminal lhs, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            List<Symbol> symbols = new List<Symbol>();
            string[] rhsTokens = productionString.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            // empty rule
            if (rhsTokens.Length == 0)
            {
                symbols.Add(Terminal.BuildEmptyTerminal());
                return new Production(lhs, symbols, lexTokenDef, ruleNonterminalType, null, ProductionType.Plain);
            }
            else
            {
                for (int i = 0; i < rhsTokens.Length; i++)
                {
                    string token = rhsTokens[i].Trim();
                    bool isToken = false;
                    foreach (LexTokenDef l in lexTokenDef)
                    {
                        if (l.name == token)
                            isToken = true;
                    }

                    if (isToken)
                    {
                        symbols.Add(Terminal.BuildToken(token));
                    }
                    else if (token.StartsWith("'") && token.EndsWith("'"))
                    {
                        symbols.Add(Terminal.BuildConstCharTerminal(token[1]));
                    }
                    else
                    {
                        symbols.Add(new Nonterminal(token));
                    }
                }

                return new Production(lhs, symbols, lexTokenDef, ruleNonterminalType, null, ProductionType.Plain);
            }
        }

        private static Production ReadProduction(ref string input, Nonterminal lhs, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            input = input.Trim();
            if (input.Length == 0)
                return null;

            if (input[0] == '|')
            {
                input = input.Substring(1).Trim();
            }
            else if (input[0] == ';')
            {
                input = input.Substring(1).Trim();
                return null;
            }

            string productionString = null;
            string action = null;

            int keyPos = LexYaccUtil.FindCharNotInLiteral(input, new List<char>() { '{', '|', ';' }, true);

            if (keyPos == -1)
            {
                // the very last production in the end of input
                productionString = input;
                input = "";
            }
            else if (input[keyPos] == '{')
            {
                productionString = input.Substring(0, keyPos).Trim();
                int rightBrace = LexYaccUtil.FindCharNotInLiteral(input, '}', true);
                if (rightBrace == -1)
                    throw new Exception("Syntax error");
                action = input.Substring(keyPos + 1, rightBrace - keyPos - 1);
                action = LexYaccUtil.RemoveHeadAndTailEmptyLine(action);

                input = input.Substring(rightBrace + 1).Trim();
            }
            else if (input[keyPos] == '|' || input[keyPos] == ';')
            {
                productionString = input.Substring(0, keyPos);
                input = input.Substring(keyPos).Trim();
            }


            Production p = ReadProduction2(productionString, lhs, lexTokenDef, ruleNonterminalType);
            p.action = action;

            return p;
        }

        private static YaccRule ReadRule(ref string input, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            input = input.Trim();
            if (input.Length == 0)
                return null;

            int comma = input.IndexOf(':');
            if (comma == -1)
                throw new Exception("Syntax error");

            string lhs = input.Substring(0, comma).Trim();
            input = input.Substring(comma + 1);
            YaccRule rule = new YaccRule();
            rule.lhs = new Nonterminal(lhs);

            // special handling for empty production in the front case:   a: | 'A' or a : {} | 'A'
            input = input.Trim();
            if (input[0] == '|')
            {
                rule.productions.Add(new Production(rule.lhs, new List<Symbol> { Terminal.BuildEmptyTerminal() }, lexTokenDef, ruleNonterminalType, null, ProductionType.Plain));
            }
            else if (input[0] == '{')
            {
                int rightCurlyPos = LexYaccUtil.FindCharNotInLiteral(input, '}', true);
                string action = input.Substring(1, rightCurlyPos - 1).Trim();
                input = input.Substring(rightCurlyPos + 1);
                rule.productions.Add(new Production(rule.lhs, new List<Symbol> { Terminal.BuildEmptyTerminal() }, lexTokenDef, ruleNonterminalType, action, ProductionType.Plain));

            }

            Production production = ReadProduction(ref input, rule.lhs, lexTokenDef, ruleNonterminalType);
            while (production != null)
            {
                rule.productions.Add(production);
                production = ReadProduction(ref input, rule.lhs, lexTokenDef, ruleNonterminalType);
            }

            return rule;
        }

        private static List<YaccRule> GetIndirectLeftRecursionDfs(YaccRule r, HashSet<string> traversed, HashSet<string> tempTraversed, List<Tuple<string, Production>> tempTraversedList, Dictionary<string, YaccRule> nameToYaccRuleMap)
        {
            foreach (Production p in r.productions)
            {
                traversed.Add(r.lhs.name);

                if (p.IsEmptyProduction())
                    continue;

                if (p.symbols[0] is Terminal)
                    continue;

                Nonterminal nt = (Nonterminal)p.symbols[0];
                if (nt.name == p.lhs.name)
                    continue;

                tempTraversed.Add(r.lhs.name);
                tempTraversedList.Add(Tuple.Create(r.lhs.name, p));

                string name = nt.name;
                if (tempTraversed.Contains(name))
                {
                    List<YaccRule> indirect = new List<YaccRule>();
                    for (int i = tempTraversedList.Count - 1; i >= 0; i--)
                    {
                        Tuple<string, Production> t = tempTraversedList.ElementAt(i);
                        string name2 = t.Item1;

                        indirect.Insert(0, nameToYaccRuleMap[t.Item1]);

                        if (name2 == name)
                            return indirect;
                    }
                }

                List<YaccRule> ret = GetIndirectLeftRecursionDfs(nameToYaccRuleMap[name], traversed, tempTraversed, tempTraversedList, nameToYaccRuleMap);
                if (ret != null)
                    return ret;

                tempTraversed.Remove(r.lhs.name);
                tempTraversedList.RemoveAt(tempTraversedList.Count - 1);
            }

            return null;
        }

        private static List<YaccRule> GetIndirectLeftRecursion(List<YaccRule> rulesParam, Dictionary<string, YaccRule> nameToYaccRuleMap)
        {
            List<YaccRule> rules = new List<YaccRule>(rulesParam);
            HashSet<string> traversed = new HashSet<string>();
            HashSet<string> tempTraversed = new HashSet<string>();
            List<Tuple<string, Production>> tempTraversedList = new List<Tuple<string, Production>>();

            while (rules.Count > 0)
            {
                traversed.Clear();
                tempTraversed.Clear();
                tempTraversedList.Clear();


                List<YaccRule> indirect = GetIndirectLeftRecursionDfs(rules[0], traversed, tempTraversed, tempTraversedList, nameToYaccRuleMap);
                if (indirect != null)
                    return indirect;

                foreach (var name in traversed)
                    rules.Remove(nameToYaccRuleMap[name]);
            }

            return null;
        }

        private static void ReplaceIndirectLeftRecursion(List<YaccRule> indirectLeftRecursionRule, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {

            /*
             a: b ' A' | 'X';
             b: a 'B' | 'B'

             => a: a 'B' 'A' | 'B' 'A' | 'X'

            =======

            a: b 'A';
            b: c 'B';
            c: d 'C';
            d: a 'D';

            =>
            a: c 'B' 'A'
            =>
            a: d 'C' 'B' 'A'
            =>
            a: a 'D' 'C' 'B' 'A'
            =>
             */

            YaccRule r0 = indirectLeftRecursionRule[0];

            for (int i = 1; i < indirectLeftRecursionRule.Count; i++)
            {
                YaccRule nextRule = indirectLeftRecursionRule[i];

                List<Production> oldProductions = new List<Production>(r0.productions);
                r0.productions.Clear();

                foreach (Production oldProduction in oldProductions)
                {
                    if (oldProduction.IsEmptyProduction())
                    {
                        r0.productions.Add(oldProduction);
                        continue;
                    }

                    if (oldProduction.symbols[0] is Terminal)
                    {
                        r0.productions.Add(oldProduction);
                        continue;
                    }

                    Nonterminal nt = (Nonterminal)oldProduction.symbols[0];

                    if (nt.name != nextRule.lhs.name)
                    {
                        r0.productions.Add(oldProduction);
                        continue;
                    }

                    if (nt.name == nextRule.lhs.name)
                    {
                        foreach (Production nextP in nextRule.productions)
                        {
                            List<Symbol> symbols = new List<Symbol>();
                            symbols.AddRange(nextP.symbols);
                            symbols.AddRange(oldProduction.symbols.GetRange(1, oldProduction.symbols.Count - 1));
                            r0.productions.Add(new Production(r0.lhs, symbols, lexTokenDef, ruleNonterminalType, null, ProductionType.Plain));
                        }
                    }
                }
            }
        }

        public static void ConvertIndirectLeftRecursion(List<YaccRule> rules, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType, Dictionary<string, YaccRule> nameToYaccRuleMap)
        {
            List<YaccRule> indirectLeftRecursionRule = GetIndirectLeftRecursion(rules, nameToYaccRuleMap);

            while (indirectLeftRecursionRule != null)
            {
                ReplaceIndirectLeftRecursion(indirectLeftRecursionRule, lexTokenDef, ruleNonterminalType);

                indirectLeftRecursionRule = GetIndirectLeftRecursion(rules, nameToYaccRuleMap);
            }
        }

        public static List<YaccRule> ConvertLeftRecursion(List<YaccRule> rules, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            List<YaccRule> ret = new List<YaccRule>();

            foreach (YaccRule pr in rules)
            {
                bool leftRecursion = false;
                foreach (Production p in pr.productions)
                {
                    if (p.symbols[0] is Nonterminal && ((Nonterminal)p.symbols[0]).name == pr.lhs.name)
                    {
                        leftRecursion = true;
                        break;
                    }
                }

                if (!leftRecursion)
                {
                    ret.Add(pr);
                }
                else
                {
                    YaccRule newPr1 = new YaccRule();
                    newPr1.lhs = new Nonterminal(pr.lhs.name);
                    ret.Add(newPr1);

                    YaccRule newPr2 = new YaccRule();
                    string expandedName = pr.lhs.name + "_LeftRecursionExpand";
                    newPr2.lhs = new Nonterminal(expandedName);
                    ret.Add(newPr2);

                    if (ruleNonterminalType.ContainsKey(pr.lhs.name))
                        ruleNonterminalType.Add(expandedName, ruleNonterminalType[pr.lhs.name]);

                    /*
                        a: a 'A' | a 'B' | 'C' | 'D' =>

                        a: 'C' a2 | 'D' a2
                        a2: 'A' a2 | 'B' a2 | empty
                    */
                    foreach (Production p in pr.productions)
                    {
                        // is left recursive
                        if (p.symbols.Count > 0 && p.symbols[0] is Nonterminal && ((Nonterminal)(p.symbols[0])).name == pr.lhs.name)
                        {
                            Production p2 = p.Clone();
                            p2.type = ProductionType.LeftRecursiveSecond;
                            p2.symbols.RemoveAt(0);
                            p2.symbols.Add(new Nonterminal(expandedName));
                            p2.lhs = newPr2.lhs;
                            newPr2.productions.Add(p2);
                        }
                        else
                        {
                            Production p2 = p.Clone();
                            p2.type = ProductionType.LeftRecursiveFirst;
                            p2.symbols.Add(new Nonterminal(expandedName));
                            newPr1.productions.Add(p2);
                        }
                    }

                    // empty production
                    newPr2.productions.Add(new Production(newPr2.lhs, new List<Symbol>() { Terminal.BuildEmptyTerminal() }, lexTokenDef, ruleNonterminalType, "", ProductionType.LeftRecursiveSecond));
                }
            }

            return ret;
        }
    }

    public class YaccRuleReader
    {
        public static void Parse(string input, out Section sections, out List<YaccRule> productionRules, out List<LexTokenDef> lexTokenDef, out Dictionary<string, string> ruleNonterminalType)
        {
            sections = SplitSecction(input, true);
            productionRules = new List<YaccRule>();

            Tuple<List<LexTokenDef>, Dictionary<string, string>> types = TypeSectionParser.Parse(sections.typeSection);
            lexTokenDef = types.Item1;
            ruleNonterminalType = types.Item2;

            productionRules = RuleSectionParser.Parse(sections.ruleSection, lexTokenDef, ruleNonterminalType);
            InsertStartRule(productionRules, lexTokenDef, ruleNonterminalType);
        }

        private static void InsertStartRule(List<YaccRule> productionRules, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            string userStartNonterminal = productionRules[0].lhs.name;
            Nonterminal start = new Nonterminal("start");
            Production startProduction = RuleSectionParser.ReadProduction2(userStartNonterminal, start, lexTokenDef, ruleNonterminalType);
            startProduction.action = "_0 = _1;";
            if (ruleNonterminalType.ContainsKey(userStartNonterminal))
                ruleNonterminalType["start"] = ruleNonterminalType[userStartNonterminal];
            startProduction.index = 0;
            YaccRule pr = new YaccRule(start, new List<Production> { startProduction });
            productionRules.Insert(0, pr);
        }

        public static Section SplitSecction(string input, bool singleQuoteAsString)
        {
            Section s = new Section();

            int definitionStart = LexYaccUtil.FindStringNotInLiteral(input, "%{", singleQuoteAsString);
            if (definitionStart != -1)
            {
                int definitionEnd = LexYaccUtil.FindStringNotInLiteral(input, "%}", singleQuoteAsString);
                s.definitionSection = input.Substring(definitionStart + 2, definitionEnd - definitionStart - 2).Trim();
                input = input.Substring(definitionEnd + 2);

                int ruleStart = LexYaccUtil.FindStringNotInLiteral(input, "%%", singleQuoteAsString);
                s.typeSection = input.Substring(0, ruleStart).Trim();
                input = input.Substring(ruleStart + 2);

                int ruleEnd = LexYaccUtil.FindStringNotInLiteral(input, "%%", singleQuoteAsString);
                s.ruleSection = input.Substring(0, ruleEnd).Trim();
            }
            else
            {
                // special format for ut that has only rule section
                s.ruleSection = input.Trim();
            }

            return s;
        }
    }
}

}

namespace ccNs{

namespace RegexNs
{

    public class State
    {
        public PatternChar pc = null;
        public int index = -1;
        public List<State> epislonTransition = new List<State>();
        public State matchTransition = null;

        public State(PatternChar pc)
        {
            this.pc = pc;
        }
    }

    public enum RecognizeResult
    {
        AliveAndAccept,
        AliveButNotAccept,
        EndAndReject
    }

    public class RecognizeParam
    {
        public List<State> availableStates;
        public RecognizeParam(List<State> availableStates)
        {
            this.availableStates = availableStates;
        }
    }

    public class NFA
    {
        public State startState = null;
        public State acceptedState = null;
        public bool startsWith = false;
        public bool endsWith = false;
        public List<PatternChar> patternChars = null;

        // One "|" must be used with one ()
        // for example, ((A|B)|C)
        // so in the operatorStack, when encounter a '|', the next pop must be '('
        public static NFA Build(string pattern)
        {
            NFA nfa = new NFA();

            nfa.patternChars = PatternTransformer.Transform(pattern, ref nfa.startsWith, ref nfa.endsWith);

            List<State> states = new List<State>();
            for (int i = 0; i < nfa.patternChars.Count; i++)
            {
                State s = new State(nfa.patternChars[i]);
                s.index = i;
                states.Add(s);
            }
            State acceptState = new State(new PatternChar('\0', PatternCharType.MetaChar));
            states.Add(acceptState);
            nfa.acceptedState = acceptState;

            nfa.startState = states[0];

            Stack<State> operatorStack = new Stack<State>();

            for (int i = 0; i < nfa.patternChars.Count; i++)
            {
                State s = states[i];
                PatternChar pc = s.pc;
                PatternCharType type = pc.type;


                if (type == PatternCharType.Char || type == PatternCharType.MultipleChar || (pc.c == '.' && type == PatternCharType.MetaChar))
                    s.matchTransition = states[i + 1];
                else if (type == PatternCharType.MetaChar && (pc.c == '(' || pc.c == ')' || pc.c == '*'))
                    s.epislonTransition.Add(states[i + 1]);


                if ((pc.c == '(' && type == PatternCharType.MetaChar) || (pc.c == '|' && type == PatternCharType.MetaChar))
                {
                    operatorStack.Push(s);
                }
                else if (pc.c == ')' && type == PatternCharType.MetaChar)
                {
                    State op = operatorStack.Pop();

                    State nextState = states[i + 1];

                    if (op.pc.c == '|' && op.pc.type == PatternCharType.MetaChar)
                    {
                        State op2 = operatorStack.Pop();
                        op2.epislonTransition.Add(states[op.index + 1]);
                        op.epislonTransition.Add(s);

                        if (nextState.pc.c == '*' && nextState.pc.type == PatternCharType.MetaChar)
                        {
                            nextState.epislonTransition.Add(op2);
                            op2.epislonTransition.Add(nextState);
                        }
                    }
                    else if (op.pc.c == '(' && op.pc.type == PatternCharType.MetaChar)
                    {
                        if (nextState.pc.c == '*' && nextState.pc.type == PatternCharType.MetaChar)
                        {
                            nextState.epislonTransition.Add(op);
                            op.epislonTransition.Add(nextState);
                        }
                    }
                }
                else if (s.pc.type != PatternCharType.MetaChar || (s.pc.c != '(' && s.pc.c != ')' && s.pc.c != '|'))
                {
                    State nextState = states[i + 1];
                    if (nextState.pc.c == '*' && nextState.pc.type == PatternCharType.MetaChar)
                    {
                        s.epislonTransition.Add(nextState);
                        nextState.epislonTransition.Add(s);
                    }
                }
            }

            return nfa;
        }


        public string Match(string txt)
        {
            for (int i = 0; i < txt.Length; i++)
            {
                string matchString = MatchInternal(txt.Substring(i));

                // found match
                if (matchString != "")
                {
                    if (startsWith == true && i != 0)
                        continue;
                    if (endsWith == true && i + matchString.Length != txt.Length)
                        continue;
                    return matchString;
                }
            }

            return "";
        }

        public string MatchInternal(string txt)
        {
            Tuple<RecognizeParam, RecognizeResult> ret = InitRecognize();
            RecognizeParam param = ret.Item1;
            RecognizeResult result = ret.Item2;

            int lastMatch = -1;
            for (int i = 0; i < txt.Length; i++)
            {
                result = StepRecognize(txt[i], param);
                if (result == RecognizeResult.AliveAndAccept)
                    lastMatch = i;
                else if (result == RecognizeResult.EndAndReject)
                    break;
            }

            return txt.Substring(0, lastMatch + 1);
        }

        public RecognizeParam CreateRecognizeParam()
        {
            return new RecognizeParam(new List<State>() { startState });
        }

        public RecognizeResult StepRecognize(char c, RecognizeParam param)
        {
            List<State> availableStates = DoEpsilonTransition(param.availableStates);
            List<State> nextAvailableStates = new List<State>();

            foreach (State s in availableStates)
            {
                if (s.pc.type == PatternCharType.Char)
                {
                    if (s.pc.c == c)
                        nextAvailableStates.Add(s.matchTransition);
                }
                else if (s.pc.type == PatternCharType.MultipleChar)
                {
                    bool match = s.pc.multipleChars.Contains(c);
                    if (s.pc.not)
                        match = !match;

                    if (match)
                        nextAvailableStates.Add(s.matchTransition);
                }
                else if (s.pc.c == '.' && s.pc.type == PatternCharType.MetaChar)
                {
                    nextAvailableStates.Add(s.matchTransition);
                }
            }


            param.availableStates.Clear();
            param.availableStates.AddRange(DoEpsilonTransition(nextAvailableStates));

            if (param.availableStates.Count == 0)
                return RecognizeResult.EndAndReject;
            else if (param.availableStates.Contains(acceptedState))
                return RecognizeResult.AliveAndAccept;
            else
                return RecognizeResult.AliveButNotAccept;
        }

        private Tuple<RecognizeParam, RecognizeResult> InitRecognize()
        {
            List<State> availableStates = new List<State>();
            availableStates.Add(startState);

            RecognizeResult result = RecognizeResult.AliveButNotAccept;
            availableStates = DoEpsilonTransition(availableStates);
            if (availableStates.Contains(acceptedState))
                result = RecognizeResult.AliveAndAccept;

            return new Tuple<RecognizeParam, RecognizeResult>(new RecognizeParam(availableStates), result);
        }

        public bool Recognize(string txt)
        {
            Tuple<RecognizeParam, RecognizeResult> ret = InitRecognize();
            RecognizeParam param = ret.Item1;
            RecognizeResult result = ret.Item2;

            foreach (char c in txt)
            {
                result = StepRecognize(c, param);
                if (result == RecognizeResult.EndAndReject)
                    break;
            }

            return result == RecognizeResult.AliveAndAccept;
        }

        private void DFS(State s, HashSet<int> visited, List<State> newStates)
        {
            if (visited.Contains(s.index))
                return;

            visited.Add(s.index);
            newStates.Add(s);
            foreach (State s2 in s.epislonTransition)
                DFS(s2, visited, newStates);
        }

        private List<State> DoEpsilonTransition(List<State> states)
        {
            List<State> newStates = new List<State>();
            HashSet<int> visited = new HashSet<int>();

            foreach (State s in states)
                DFS(s, visited, newStates);

            return newStates;
        }
    }

}
}

namespace ccNs{

namespace RegexNs
{
    public class PatternChar
    {
        public PatternChar()
        {
        }

        public PatternChar(char c)
        {
            this.c = c;
            type = PatternCharType.Char;
        }

        public PatternChar(char c, PatternCharType type)
        {
            this.c = c;
            this.type = type;
        }

        public char c = '\0';
        public List<char> multipleChars = new List<char>();
        public bool not = false;

        public PatternCharType type = PatternCharType.None;

        public PatternChar Clone()
        {
            PatternChar pc = new PatternChar(c, type);
            pc.not = not;
            pc.multipleChars.AddRange(multipleChars);

            return pc;
        }
    }

    public enum PatternCharType
    {
        Char,
        MultipleChar,
        MetaChar,
        None
    }

    public class PatternTransformer
    {
        public static List<PatternChar> ToPatternChar(string pattern)
        {
            List<PatternChar> patternChars = new List<PatternChar>();
            foreach (char c in pattern)
                patternChars.Add(new PatternChar(c));

            return patternChars;
        }

        // \ ^ | . $ ? * + ( ) [ ] { } d D w W s W
        public static List<PatternChar> TransformEscape(List<PatternChar> patternChars)
        {
            List<PatternChar> newPatternChars = new List<PatternChar>();

            List<char> escapedChars = new List<char>() { '\\', '^', '|', '.', '$', '?', '*', '+', '(', ')', '[', ']', '{', '}', };
            List<char> shorthand = new List<char>() { 'd', 'D', 'w', 'W', 's', 'S' };

            for (int i = 0; i < patternChars.Count; i++)
            {
                PatternChar c = patternChars[i];
                if (c.c == '\\' && i < patternChars.Count)
                {
                    if (escapedChars.Contains(patternChars[i + 1].c))
                    {
                        newPatternChars.Add(new PatternChar(patternChars[i + 1].c));
                        i++;
                    }
                    else if (shorthand.Contains(patternChars[i + 1].c))
                    {
                        newPatternChars.Add(new PatternChar(patternChars[i + 1].c, PatternCharType.MetaChar));
                        i++;
                    }
                    else if (patternChars[i + 1].c == 'n')
                    {
                        newPatternChars.Add(new PatternChar('\n'));
                        i++;
                    }
                    else if (patternChars[i + 1].c == 'r')
                    {
                        newPatternChars.Add(new PatternChar('\r'));
                        i++;
                    }
                    else if (patternChars[i + 1].c == 't')
                    {
                        newPatternChars.Add(new PatternChar('\t'));
                        i++;
                    }
                }
                else
                {
                    if (escapedChars.Contains(c.c))
                        newPatternChars.Add(new PatternChar(c.c, PatternCharType.MetaChar));
                    else
                        newPatternChars.Add(c);
                }
            }

            return newPatternChars;
        }

        // "[-\\na-z-][^-\\[]\\tx"
        // in [], escape only ], \, \t, \n, \r
        public static List<PatternChar> TransformSquareBracket(List<PatternChar> patternChars)
        {
            List<PatternChar> newPatternChars = new List<PatternChar>();

            for (int i = 0; i < patternChars.Count;)
            {
                PatternChar c = patternChars[i];

                if (c.c == '[' && c.type == PatternCharType.MetaChar)
                {
                    int squareStart = i;
                    int squareEnd = squareStart + 1;
                    for (; squareEnd < patternChars.Count; squareEnd++)
                    {
                        if (patternChars[squareEnd].c == ']' && patternChars[squareEnd].type == PatternCharType.MetaChar)
                            break;
                    }
                    i = squareEnd + 1;

                    PatternChar multipleChar = new PatternChar();
                    multipleChar.type = PatternCharType.MultipleChar;

                    if (patternChars[squareStart + 1].c == '^')
                    {
                        multipleChar.not = true;
                        squareStart++;
                    }

                    for (int j = squareStart + 1; j < squareEnd;)
                    {
                        // [-a] case
                        if (j == squareStart + 1)
                        {
                            if (patternChars[j].c == '-')
                            {
                                multipleChar.multipleChars.Add('-');
                                j++;
                                continue;
                            }
                        }

                        // [-], [a] case
                        if (j + 1 == squareEnd)
                        {
                            multipleChar.multipleChars.Add(patternChars[j].c);
                            break;
                        }

                        // [ab] case
                        if (j + 2 == squareEnd)
                        {
                            multipleChar.multipleChars.Add(patternChars[j].c);
                            multipleChar.multipleChars.Add(patternChars[j + 1].c);
                            break;
                        }

                        if (patternChars[j + 1].c == '-')
                        {
                            for (char c2 = patternChars[j].c; c2 <= patternChars[j + 2].c; c2++)
                                multipleChar.multipleChars.Add(c2);
                            j += 3;
                            continue;
                        }
                        else
                        {
                            multipleChar.multipleChars.Add(patternChars[j].c);
                            j++;
                            continue;
                        }
                    }


                    newPatternChars.Add(multipleChar);
                    continue;
                }
                else
                {
                    newPatternChars.Add(patternChars[i]);
                    i++;
                }
            }

            return newPatternChars;
        }

        /*
          \d	[0-9]
          \D	[^0-9]
          \w	[a-zA-Z0-9]
          \W	[^a-zA-Z0-9]
          \s	[ \t\n\r]
          \S	[^ \t\n\r]
        */
        public static List<PatternChar> TransformShorthand(List<PatternChar> patternChars)
        {
            List<PatternChar> newPatternChars = new List<PatternChar>();
            for (int i = 0; i < patternChars.Count; i++)
            {
                PatternChar pc = patternChars[i];
                if (pc.type == PatternCharType.MetaChar && new List<char> { 'd', 'D', 'w', 'W', 's', 'S' }.Contains(pc.c) && i < patternChars.Count)
                {
                    PatternChar patternChar = new PatternChar();
                    patternChar.type = PatternCharType.MultipleChar;

                    if (pc.c == 'd' || pc.c == 'D')
                    {
                        for (char c2 = '0'; c2 <= '9'; c2++)
                            patternChar.multipleChars.Add(c2);

                        if (pc.c == 'D')
                            patternChar.not = true;

                        newPatternChars.Add(patternChar);
                    }
                    else if (pc.c == 'w' || pc.c == 'W')
                    {
                        for (char c2 = '0'; c2 <= '9'; c2++)
                            patternChar.multipleChars.Add(c2);
                        for (char c2 = 'a'; c2 <= 'z'; c2++)
                            patternChar.multipleChars.Add(c2);
                        for (char c2 = 'A'; c2 <= 'Z'; c2++)
                            patternChar.multipleChars.Add(c2);

                        if (pc.c == 'W')
                            patternChar.not = true;

                        newPatternChars.Add(patternChar);
                    }
                    else if (pc.c == 's' || pc.c == 'S')
                    {
                        patternChar.multipleChars.Add(' ');
                        patternChar.multipleChars.Add('t');
                        patternChar.multipleChars.Add('n');
                        patternChar.multipleChars.Add('r');

                        if (pc.c == 'S')
                            patternChar.not = true;

                        newPatternChars.Add(patternChar);
                    }
                }
                else
                {
                    newPatternChars.Add(pc);
                }
            }

            return newPatternChars;
        }

        // Add () to each or, ex: a|b|c -> ((a|b)|c)
        // remove redundant () between |, ex: ((ab)|(cd)) -> (ab|cd)
        public static List<PatternChar> ModifyParentsisBetweenOr(List<PatternChar> patternChars)
        {
            List<List<PatternChar>> tokensSplitedByOr = new List<List<PatternChar>>();

            List<PatternChar> token = new List<PatternChar>();
            int level = 0;

            for (int i = 0; i < patternChars.Count; i++)
            {
                PatternChar c = patternChars[i];

                if (c.c == '(' && c.type == PatternCharType.MetaChar)
                    level++;
                else if (c.c == ')' && c.type == PatternCharType.MetaChar)
                    level--;

                if (level > 0)
                {
                    token.Add(c);
                }
                else
                {
                    if (c.c == '|' && c.type == PatternCharType.MetaChar)
                    {
                        tokensSplitedByOr.Add(token);
                        token = new List<PatternChar>();
                    }
                    else
                    {
                        token.Add(c);
                    }
                }
            }

            tokensSplitedByOr.Add(token);

            if (tokensSplitedByOr.Count == 1)
            {
                token = tokensSplitedByOr[0];

                if (token.Count == 0)
                {
                    // the case (|a)
                    return token;
                }

                // if (a)(b), return (a)(b)
                // if abc, return abc
                // if ((a)(b)), return (a)(b)
                if (token[0].c == '(' && token[0].type == PatternCharType.MetaChar)
                {
                    int level2 = 1;
                    for (int j = 1; j < token.Count; j++)
                    {
                        if (token[j].c == '(' && token[j].type == PatternCharType.MetaChar)
                            level2++;
                        else if (token[j].c == ')' && token[j].type == PatternCharType.MetaChar)
                            level2--;


                        if (level2 == 0)
                        {
                            if (j == token.Count - 1)
                            {
                                // the case ((a)(b))
                                token.RemoveAt(0);
                                token.RemoveAt(token.Count - 1);
                                return ModifyParentsisBetweenOr(token);
                            }
                            else
                            {
                                // the case (a)(b)
                                return tokensSplitedByOr[0];
                            }
                        }
                    }

                    return ModifyParentsisBetweenOr(token);
                }
                else
                {
                    return tokensSplitedByOr[0];
                }
            }
            else
            {
                List<PatternChar> ret = ModifyParentsisBetweenOr(tokensSplitedByOr[0]);
                for (int i = 1; i < tokensSplitedByOr.Count; i++)
                {
                    ret.Insert(0, new PatternChar('(', PatternCharType.MetaChar));
                    ret.Add(new PatternChar('|', PatternCharType.MetaChar));
                    ret.AddRange(ModifyParentsisBetweenOr(tokensSplitedByOr[i]));
                    ret.Add(new PatternChar(')', PatternCharType.MetaChar));
                }

                return ret;
            }
        }

        /*
        // A+ = AA*
        // A? = (|A)
        // A{3} = AAA
        // A{2-4} = (AA|AAA|AAAA)
        // A{3-} = AAAA*
        */
        public static List<PatternChar> TransformSuffix(List<PatternChar> patternChars)
        {
            List<PatternChar> newPatternChars = new List<PatternChar>();
            List<PatternChar> nextNewPatternChars = new List<PatternChar>();
            nextNewPatternChars.AddRange(patternChars);

            bool keepGoing = true;

            while (keepGoing)
            {
                newPatternChars.Clear();
                newPatternChars.AddRange(nextNewPatternChars);
                nextNewPatternChars.Clear();

                keepGoing = false;

                for (int i = newPatternChars.Count - 1; i >= 0; i--)
                {
                    PatternChar c = newPatternChars[i];
                    if (c.type == PatternCharType.MetaChar && (c.c == '+' || c.c == '?' || c.c == '}'))
                    {
                        int j = i;

                        int countStart = -1;
                        int countEnd = -1;

                        if (c.c == '+')
                        {
                            keepGoing = true;
                            j--;
                        }
                        else if (c.c == '?')
                        {
                            keepGoing = true;
                            j--;
                        }
                        else if (c.c == '}')
                        {
                            keepGoing = true;

                            // A{3}
                            // A{3-}
                            // A{2-4}
                            int rightCurlyBracket = j;
                            int leftCurlyBracket = j - 1;
                            for (; !(newPatternChars[leftCurlyBracket].c == '{' && newPatternChars[leftCurlyBracket].type == PatternCharType.MetaChar); leftCurlyBracket--)
                                ;

                            int dash = leftCurlyBracket + 1;
                            for (; newPatternChars[dash].c != '-' && dash < rightCurlyBracket; dash++)
                                ;

                            string temp = "";
                            if (dash == rightCurlyBracket)
                            {
                                // A{3}
                                for (int k = leftCurlyBracket + 1; k < rightCurlyBracket; k++)
                                    temp += newPatternChars[k].c;
                                countStart = int.Parse(temp);
                                countEnd = countStart;
                            }
                            else if (dash + 1 != rightCurlyBracket)
                            {
                                // A{2-4}
                                temp = "";
                                for (int k = leftCurlyBracket + 1; k < dash; k++)
                                    temp += newPatternChars[k].c;
                                countStart = int.Parse(temp);

                                temp = "";
                                for (int k = dash + 1; k < rightCurlyBracket; k++)
                                    temp += newPatternChars[k].c;
                                countEnd = int.Parse(temp);
                            }
                            else
                            {
                                // A{3-}
                                temp = "";
                                for (int k = leftCurlyBracket + 1; k < dash; k++)
                                    temp += newPatternChars[k].c;
                                countStart = int.Parse(temp);
                            }

                            j = leftCurlyBracket - 1;
                        }

                        // a+ => subPattern = a
                        // (abc)+ => subPattern = (abc)
                        List<PatternChar> subPattern = new List<PatternChar>();
                        if (newPatternChars[j].c == ')' && newPatternChars[j].type == PatternCharType.MetaChar)
                        {
                            subPattern.Insert(0, newPatternChars[j]);
                            j--;
                            int level = 1;

                            for (; j >= 0; j--)
                            {
                                if (newPatternChars[j].c == ')' && newPatternChars[j].type == PatternCharType.MetaChar)
                                    level++;
                                else if (newPatternChars[j].c == '(' && newPatternChars[j].type == PatternCharType.MetaChar)
                                    level--;

                                subPattern.Insert(0, newPatternChars[j]);

                                if (level == 0)
                                {
                                    j--;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            subPattern.Add(newPatternChars[j]);
                            j--;
                        }

                        if (c.c == '+')
                        {
                            // A+ = AA*
                            // (abc)+ = (abc)(abc)*
                            nextNewPatternChars.Insert(0, new PatternChar('*', PatternCharType.MetaChar));
                            nextNewPatternChars.InsertRange(0, RepeatPatternChar(subPattern, 2));
                        }
                        else if (c.c == '?')
                        {
                            // A? = (|A)
                            // (abc)? = (|(abc))
                            nextNewPatternChars.Insert(0, new PatternChar(')', PatternCharType.MetaChar));
                            nextNewPatternChars.InsertRange(0, RepeatPatternChar(subPattern, 1));
                            nextNewPatternChars.Insert(0, new PatternChar('|', PatternCharType.MetaChar));
                            nextNewPatternChars.Insert(0, new PatternChar('(', PatternCharType.MetaChar));
                        }
                        else if (c.c == '}')
                        {
                            if (countEnd == -1)
                            {
                                // A{2-} = AAA*
                                // (abc){2-} = (abc)(abc)(abc)*
                                nextNewPatternChars.Insert(0, new PatternChar('*', PatternCharType.MetaChar));
                                nextNewPatternChars.InsertRange(0, RepeatPatternChar(subPattern, countStart + 1));
                            }
                            else if (countStart == countEnd)
                            {
                                // A{3} = AAA
                                // (abc){3} = (abc)(abc)(abc)
                                nextNewPatternChars.InsertRange(0, RepeatPatternChar(subPattern, countStart));
                            }
                            else
                            {
                                // A{2-3} = (AA|AAA)
                                // (abc){2-4} = (((abc)(abc)|(abc)(abc)(abc))|(abc)(abc)(abc)(abc))

                                List<PatternChar> temp = new List<PatternChar>();
                                for (int k = countStart; k <= countEnd; k++)
                                {
                                    if (k != countStart)
                                    {
                                        temp.Insert(0, new PatternChar('(', PatternCharType.MetaChar));
                                        temp.Add(new PatternChar('|', PatternCharType.MetaChar));
                                    }

                                    temp.AddRange(RepeatPatternChar(subPattern, k));

                                    if (k != countStart)
                                        temp.Add(new PatternChar(')', PatternCharType.MetaChar));
                                }

                                nextNewPatternChars.InsertRange(0, temp);
                            }
                        }

                        for (; j >= 0; j--)
                        {
                            nextNewPatternChars.Insert(0, newPatternChars[j]);
                        }
                        break;
                    }
                    else
                    {
                        nextNewPatternChars.Insert(0, c);
                    }
                }
            }

            return nextNewPatternChars;
        }

        private static List<PatternChar> RepeatPatternChar(List<PatternChar> patternChars, int repeatCount)
        {
            List<PatternChar> ret = new List<PatternChar>();

            for (int count = 0; count < repeatCount; count++)
            {
                for (int k = 0; k < patternChars.Count; k++)
                {
                    ret.Add(patternChars[k].Clone());
                }
            }

            return ret;
        }

        public static List<PatternChar> TransformStartsWithEndsWith(List<PatternChar> patternChars, ref bool startsWith, ref bool endsWith)
        {
            if (patternChars.Count > 0 && patternChars[0].c == '^' && patternChars[0].type == PatternCharType.MetaChar)
            {
                startsWith = true;
                patternChars.RemoveAt(0);
            }

            if (patternChars.Count > 0 && patternChars[patternChars.Count - 1].c == '$' && patternChars[patternChars.Count - 1].type == PatternCharType.MetaChar)
            {
                endsWith = true;
                patternChars.RemoveAt(patternChars.Count - 1);
            }


            return patternChars;
        }


        public static List<PatternChar> Transform(string pattern, ref bool startsWith, ref bool endsWith)
        {
            List<PatternChar> patternChars = ToPatternChar(pattern);

            patternChars = TransformEscape(patternChars);
            patternChars = TransformStartsWithEndsWith(patternChars, ref startsWith, ref endsWith);
            patternChars = TransformShorthand(patternChars);
            patternChars = TransformSquareBracket(patternChars);
            patternChars = ModifyParentsisBetweenOr(patternChars);
            patternChars = TransformSuffix(patternChars);

            return patternChars;
        }
    }
}

}

namespace ccNs{

namespace RegexNs
{

    public class Regex
    {
        public static bool Recognize(string regex, string input)
        {
            NFA nfa = NFA.Build(regex);
            return nfa.Recognize(input);
        }

        public static string MatchFirst(string regex, string input)
        {
            NFA nfa = NFA.Build(regex);
            return nfa.Match(input);
        }

        public static List<string> MatchAll(string regex, string input)
        {
            List<string> all = new List<string>();

            NFA nfa = NFA.Build(regex);
            string match = nfa.Match(input);
            while (match != "")
            {
                all.Add(match);
                input = input.Substring(input.IndexOf(match) + match.Length);
                match = nfa.Match(input);
            }

            return all;
        }

        public static NFA Compile(string regex)
        {
            return NFA.Build(regex);
        }
    }

}
}
