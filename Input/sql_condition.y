%{

%}

%token <string> SELECT ID CREATE TABLE NUMBER_TYPE VARCHAR INSERT INTO VALUES DELETE FROM WHERE AND OR NOT SHOW TABLES NOT_EQUAL LESS_OR_EQUAL GREATER_OR_EQUAL STRING NUMBER UPDATE SET
%type <string> statement column_type create_table_statement insert_statement  delete_statement show_tables_statement logical_operator select_statement string_number_id string_number
%type <List<string>> comma_sep_id comma_sep_id_include_star comma_sep_value
%type <List<(string, string)>> column_declare
%type <HashSet<int>> boolean_expression
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
string_number_id '=' string_number_id
{
    $$ = MyDBNs.SqlConditionLexYaccCallback.BooleanExpression($1, "=", $3);
}
| 
string_number_id '<' string_number_id
{
    $$ = MyDBNs.SqlConditionLexYaccCallback.BooleanExpression($1, "<", $3);
}
| 
string_number_id '>' string_number_id
{
    $$ = MyDBNs.SqlConditionLexYaccCallback.BooleanExpression($1, ">", $3);
}
| 
string_number_id NOT_EQUAL string_number_id
{
    $$ = MyDBNs.SqlConditionLexYaccCallback.BooleanExpression($1, "!=", $3);
}
| 
string_number_id LESS_OR_EQUAL string_number_id
{
    $$ = MyDBNs.SqlConditionLexYaccCallback.BooleanExpression($1, "<=", $3);
}
| 
string_number_id GREATER_OR_EQUAL string_number_id
{
    $$ = MyDBNs.SqlConditionLexYaccCallback.BooleanExpression($1, ">=", $3);
}
;

string_number_id:
ID
{
    $$ = $1;
}
| 
STRING
{
    $$ = $1;
}
| 
NUMBER
{
    $$ = $1;
}
;
%%