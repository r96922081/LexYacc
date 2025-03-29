%{

%}

%token <string> SELECT ID CREATE TABLE NUMBER VARCHAR INSERT INTO VALUES DELETE FROM WHERE AND OR NOT SHOW TABLES NOT_EQUAL LESS_OR_EQUAL GREATER_OR_EQUAL STRING UPDATE SET ORDER BY ASC DESC DROP SAVE LOAD DB FILE_PATH TWO_PIPE NULL IS LIKE TRANSACTION COMMIT ROLLBACK START GROUP MIN MAX SUM COUNT ID_DOT_ID ID_DOT_STAR JOIN ON AS
%token <int> POSITIVE_INT
%token <double> DOUBLE
%type <string> statement column_type save_db load_db create_table_statement insert_statement  delete_statement show_tables_statement drop_table_statement logical_operator select_statement boolean_expression string_number_column update_statement file_path string_number_null column
%type <List<string>> columns column_star_list string_number_null_list
%type <List<(string, string)>> column_declare
%type <List<object>> order_by_column
%type <List<List<object>>> order_by_columns
%type <List<Tuple<string, string>>> set_expression
%type <List<double>> arithmetic_expression term number_double number_column
%%


arithmetic_expression:
arithmetic_expression '+' term 
{
    $$ = MyDBNs.SqlArithmeticExpressionLexYaccCallback.ArithmeticExpression($1, "+", $3);
}
| 
arithmetic_expression '-' term 
{
    $$ = MyDBNs.SqlArithmeticExpressionLexYaccCallback.ArithmeticExpression($1, "-", $3);
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
    $$ = MyDBNs.SqlArithmeticExpressionLexYaccCallback.ArithmeticExpression($1, "*", $3);
}
| term '/' number_column 
{
    $$ = MyDBNs.SqlArithmeticExpressionLexYaccCallback.ArithmeticExpression($1, "/", $3);
}
|
term '*' '(' arithmetic_expression ')' 
{
    $$ = MyDBNs.SqlArithmeticExpressionLexYaccCallback.ArithmeticExpression($1, "*", $4);
}
| term '/' '(' arithmetic_expression ')' 
{
    $$ = MyDBNs.SqlArithmeticExpressionLexYaccCallback.ArithmeticExpression($1, "/", $4);
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
    $$ = $1;
}
|
column
{
    $$ = MyDBNs.SqlArithmeticExpressionLexYaccCallback.GetColumnValues($1);
}
;

number_double:
DOUBLE
{
    $$ = MyDBNs.SqlArithmeticExpressionLexYaccCallback.GetColumnValues($1);
}
| 
POSITIVE_INT
{
    $$ = MyDBNs.SqlArithmeticExpressionLexYaccCallback.GetColumnValues($1);
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
%%