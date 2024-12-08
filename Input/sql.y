%{

%}

%token <string> SELECT ID CREATE TABLE NUMBER_TYPE VARCHAR INSERT INTO VALUES DELETE FROM WHERE AND OR NOT SHOW TABLES NOT_EQUAL LESS_OR_EQUAL GREATER_OR_EQUAL STRING NUMBER
%type <string> statement column_type create_table_statement insert_statement  delete_statement show_tables_statement logical_operator select_statement boolean_expression string_number_id string_number
%type <List<string>> comma_sep_id comma_sep_id_include_star comma_sep_value
%type <List<(string, string)>> column_declare
%%

statement: create_table_statement | insert_statement | delete_statement | show_tables_statement | select_statement;

create_table_statement: CREATE TABLE ID '(' column_declare ')' 
{
    MyDBNs.SqlLexYaccCallback.CreateTable($3, $5);
};

column_declare: ID column_type 
{
    MyDBNs.SqlLexYaccCallback.ColumnDeclare($$, $1, $2);
} 
| 
ID column_type ',' column_declare 
{
    MyDBNs.SqlLexYaccCallback.ColumnDeclare($$, $1, $2, $4);
};

insert_statement: 
INSERT INTO ID VALUES '(' comma_sep_value ')'
{
    MyDBNs.SqlLexYaccCallback.Insert($3, null, $6);
}
|
INSERT INTO ID '(' comma_sep_id ')' VALUES '(' comma_sep_value ')'
{
    MyDBNs.SqlLexYaccCallback.Insert($3, $5, $9);
};

delete_statement:
DELETE FROM ID WHERE boolean_expression
{
    MyDBNs.SqlLexYaccCallback.Delete($3, $5);
}
;

show_tables_statement:
SHOW TABLES
{
    MyDBNs.SqlLexYaccCallback.ShowTables();
}
;

select_statement:
SELECT comma_sep_id_include_star FROM ID WHERE boolean_expression
{
    MyDBNs.SqlLexYaccCallback.Select($2, $4, $6);
}
;

boolean_expression:
boolean_expression AND boolean_expression
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression2(ref $$, $1, "AND", $3);
}
|
boolean_expression OR boolean_expression
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression2(ref $$, $1, "OR", $3);
}
| 
'(' boolean_expression ')'
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression1(ref $$, $2);
}
| 
string_number_id '=' string_number_id
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression2(ref $$, $1, "=", $3);
}
| 
string_number_id '<' string_number_id
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression2(ref $$, $1, "<", $3);
}
| 
string_number_id '>' string_number_id
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression2(ref $$, $1, ">", $3);
}
| 
string_number_id NOT_EQUAL string_number_id
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression2(ref $$, $1, "!=", $3);
}
| 
string_number_id LESS_OR_EQUAL string_number_id
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression2(ref $$, $1, "<=", $3);
}
| 
string_number_id GREATER_OR_EQUAL string_number_id
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression2(ref $$, $1, ">=", $3);
}
;

comma_sep_id: 
ID 
{
    MyDBNs.SqlLexYaccCallback.CommaSepID($$, $1);
}
| ID ',' comma_sep_id
{
    MyDBNs.SqlLexYaccCallback.CommaSepID($$, $1, $3);
}
;

comma_sep_value: 
string_number 
{
    MyDBNs.SqlLexYaccCallback.CommaSepID($$, $1);
}
| string_number ',' comma_sep_value
{
    MyDBNs.SqlLexYaccCallback.CommaSepID($$, $1, $3);
}
;

comma_sep_id_include_star: 
ID 
{
    MyDBNs.SqlLexYaccCallback.CommaSepIDIncludeStar($$, $1);
}
|
ID ',' comma_sep_id_include_star
{
    MyDBNs.SqlLexYaccCallback.CommaSepIDIncludeStar($$, $1, $3);
}
| '*'
{
    MyDBNs.SqlLexYaccCallback.CommaSepIDIncludeStar($$, "*");
}
| '*' ',' comma_sep_id_include_star
{
    MyDBNs.SqlLexYaccCallback.CommaSepIDIncludeStar($$, "*", $3);
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

string_number:
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

logical_operator: AND | OR;

column_type: VARCHAR '(' NUMBER ')' {$$ = $1 + "(" + $3 + ")";} | NUMBER_TYPE {$$ = $1;};
%%