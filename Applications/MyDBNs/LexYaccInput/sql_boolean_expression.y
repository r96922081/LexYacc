%{

%}

%token <string> SELECT ID CREATE TABLE NUMBER VARCHAR INSERT INTO VALUES DELETE FROM WHERE AND OR NOT SHOW TABLES NOT_EQUAL LESS_OR_EQUAL GREATER_OR_EQUAL STRING UPDATE SET ORDER BY ASC DESC DROP SAVE LOAD DB FILE_PATH TWO_PIPE NULL IS LIKE TRANSACTION COMMIT ROLLBACK START GROUP MIN MAX SUM COUNT ID_DOT_ID ID_DOT_STAR JOIN ON AS
%token <int> POSITIVE_INT
%token <double> DOUBLE

%type <string> statement column_type create_table_statement insert_statement  delete_statement show_tables_statement logical_operator select_statement string_number_column arithmeticExpression_column arithmetic_expression term number_column string_expression string_column string_number_null column relational_operator
%type <List<string>> columns column_star_list string_number_null_list
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
string_expression relational_operator string_expression
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionVarcharColumn($1, $2, $3);
}
| 
arithmeticExpression_column relational_operator arithmeticExpression_column
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionNumberColumn($1, $2, $3);
}
|
column IS NULL
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionNullity($1, "IS");   
}
|
column IS NOT NULL
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionNullity($1, "IS NOT"); 
}
|
column LIKE STRING
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionLike($1, "LIKE", $3);
}
|
column NOT LIKE STRING
{
    $$ = MyDBNs.SqlBooleanExpressionLexYaccCallback.BooleanExpressionLike($1, "NOT LIKE", $4);
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
term '*' number_column 
{
    $$ = $1 + " * " + $3;
}
| term '/' number_column 
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
number_column
{
    $$ = $1;
}
;

number_column:
number_double
{
    $$ = "" + $1;
}
|
column
{
    $$ = $1;
}
;

arithmeticExpression_column:
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
DOUBLE
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
string_expression TWO_PIPE string_column 
{
    $$ = $1 + " || " + $3;
}
string_column 
{
    $$ = $1;
}
;

string_column:
column
{
    $$ = $1;
}
| 
STRING
{
    $$ = $1;
}
;

column:
ID
{
    $$ = $1;
}
|
ID_DOT_ID
{
    $$ = $1;
}
|
ID_DOT_STAR
{
    $$ = $1;
}
;

relational_operator:
'='
{
	$$ = "=";
}
|
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
NOT_EQUAL
{
	$$ = $1;
}
|
LESS_OR_EQUAL
{
	$$ = $1;
}
|
GREATER_OR_EQUAL
{
	$$ = $1;
}
;

%%