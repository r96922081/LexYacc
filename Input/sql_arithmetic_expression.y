%{

%}

%token <string> SELECT ID CREATE TABLE NUMBER_TYPE VARCHAR INSERT INTO VALUES DELETE FROM WHERE AND OR NOT SHOW TABLES NOT_EQUAL LESS_OR_EQUAL GREATER_OR_EQUAL STRING UPDATE SET ORDER BY ASC DESC DROP SAVE LOAD DB FILE_PATH TWO_PIPE NULL IS
%token <int> POSITIVE_INT
%token <double> NUMBER_DOUBLE
%type <string> statement column_type save_db load_db create_table_statement insert_statement  delete_statement show_tables_statement drop_table_statement logical_operator select_statement boolean_expression string_number_id update_statement file_path string_number_null
%type <List<string>> comma_sep_id commaSep_id_star commaSep_string_number_null
%type <List<(string, string)>> column_declare
%type <List<object>> order_by_column
%type <List<List<object>>> order_by_condition
%type <List<Tuple<string, string>>> set_expression
%type <List<double>> arithmetic_expression term number_double number_double_id
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
term '*' number_double_id 
{
    $$ = MyDBNs.SqlArithmeticExpressionLexYaccCallback.ArithmeticExpression($1, "*", $3);
}
| term '/' number_double_id 
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
number_double_id
{
    $$ = $1;
}
;

number_double_id:
number_double
{
    $$ = $1;
}
|
ID
{
    $$ = MyDBNs.SqlArithmeticExpressionLexYaccCallback.GetColumnValues($1);
}
;

number_double:
NUMBER_DOUBLE
{
    $$ = MyDBNs.SqlArithmeticExpressionLexYaccCallback.GetColumnValues($1);
}
| 
POSITIVE_INT
{
    $$ = MyDBNs.SqlArithmeticExpressionLexYaccCallback.GetColumnValues($1);
}
;
%%