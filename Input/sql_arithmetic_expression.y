%{

%}

%token <string> SELECT ID CREATE TABLE NUMBER_TYPE VARCHAR INSERT INTO VALUES DELETE FROM WHERE AND OR NOT SHOW TABLES NOT_EQUAL LESS_OR_EQUAL GREATER_OR_EQUAL STRING UPDATE SET ORDER BY ASC DESC DROP SAVE LOAD DB FILE_PATH
%token <int> POSITIVE_INT
%token <double> NUMBER_DOUBLE
%type <string> statement column_type save_db load_db create_table_statement insert_statement  delete_statement show_tables_statement drop_table_statement logical_operator select_statement boolean_expression string_number_id string_number update_statement file_path
%type <List<string>> comma_sep_id comma_sep_id_include_star comma_sep_value
%type <List<(string, string)>> column_declare
%type <List<object>> order_by_column
%type <List<List<object>>> order_by_condition
%type <List<Tuple<string, string>>> set_expression
%type <double> arithmetic_expression term number_double number_double_id
%%


arithmetic_expression:
arithmetic_expression '+' term 
{
    $$ = $1 + $3;
}
| 
arithmetic_expression '-' term 
{
    $$ = $1 - $3;
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
    $$ = $1 * $3;
}
| term '/' number_double_id 
{
    $$ = $1 / $3;
}
|
term '*' '(' arithmetic_expression ')' 
{
    $$ = $1 * $4;
}
| term '/' '(' arithmetic_expression ')' 
{
    $$ = $1 / $4;
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
    //mojo
    $$ = 1;
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
%%