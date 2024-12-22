%{

%}

%token <string> SELECT ID CREATE TABLE NUMBER_TYPE VARCHAR INSERT INTO VALUES DELETE FROM WHERE AND OR NOT SHOW TABLES NOT_EQUAL LESS_OR_EQUAL GREATER_OR_EQUAL STRING UPDATE SET ORDER BY ASC DESC DROP SAVE LOAD DB FILE_PATH TWO_PIPE
%token <int> POSITIVE_INT
%token <double> NUMBER_DOUBLE
%type <string> statement column_type create_table_statement insert_statement  delete_statement show_tables_statement logical_operator select_statement string_number_id string_number arithmetic_expression_id arithmetic_expression term number_double_id string_expression string_id
%type <List<string>> comma_sep_id comma_sep_id_include_star comma_sep_value
%type <List<(string, string)>> column_declare
%type <HashSet<int>> boolean_expression
%type <double> number_double
%%

boolean_expression:
boolean_expression AND boolean_expression
{
    $1.IntersectWith($3);
    $$ = $1;
}
|
boolean_expression OR boolean_expression
{
    $1.UnionWith($3);
    $$ = $1;
}
| 
'(' boolean_expression ')'
{
    $$ = $2;
}
| 
string_expression '=' string_expression
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionVarcharColumn($1, "=", $3);
}
| 
string_expression '<' string_expression
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionVarcharColumn($1, "<", $3);
}
| 
string_expression '>' string_expression
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionVarcharColumn($1, ">", $3);
}
| 
string_expression NOT_EQUAL string_expression
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionVarcharColumn($1, "!=", $3);
}
| 
string_expression LESS_OR_EQUAL string_expression
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionVarcharColumn($1, "<=", $3);
}
| 
string_expression GREATER_OR_EQUAL string_expression
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionVarcharColumn($1, ">=", $3);
}
| 
arithmetic_expression_id '=' arithmetic_expression_id
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionNumberColumn($1, "=", $3);
}
| 
arithmetic_expression_id '<' arithmetic_expression_id
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionNumberColumn($1, "<", $3);
}
| 
arithmetic_expression_id '>' arithmetic_expression_id
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionNumberColumn($1, ">", $3);
}
| 
arithmetic_expression_id NOT_EQUAL arithmetic_expression_id
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionNumberColumn($1, "!=", $3);
}
| 
arithmetic_expression_id LESS_OR_EQUAL arithmetic_expression_id
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionNumberColumn($1, "<=", $3);
}
| 
arithmetic_expression_id GREATER_OR_EQUAL arithmetic_expression_id
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionNumberColumn($1, ">=", $3);
}
;

arithmetic_expression:
arithmetic_expression '+' term 
{
    $$ = $1 + " + " + $3;
}
| 
arithmetic_expression '-' term 
{
    $$ = $1 + " - " + $3;
}
| 
term 
{
    $$ = $1;
}
;

term:
term '*' number_double_id 
{
    $$ = $1 + " * " + $3;
}
| term '/' number_double_id 
{
    $$ = $1 + " / " + $3;
}
|
term '*' '(' arithmetic_expression ')' 
{
    $$ = $1 + " * ( " + $4 + " )";
}
| term '/' '(' arithmetic_expression ')' 
{
    $$ = $1 + " / ( " + $4 + " )";
}
|
'(' arithmetic_expression ')'
{
    $$ = $2;
}
| 
number_double_id
{
    $$ = $1;
}
;

number_double_id:
number_double
{
    $$ = "" + $1;
}
|
ID
{
    $$ = $1;
}
;

arithmetic_expression_id:
ID
{
    $$ = $1;
}
| 
arithmetic_expression
{
    $$ = $1;
}
;

number_double:
NUMBER_DOUBLE
{
    $$ = $1;
}
| 
POSITIVE_INT
{
    $$ = $1;
}
;

string_expression:
string_expression TWO_PIPE string_id 
{
    $$ = $1 + " || " + $3;
}
string_id 
{
    $$ = $1;
}
;

string_id:
ID
{
    $$ = $1;
}
| 
STRING
{
    $$ = $1;
}
;

%%