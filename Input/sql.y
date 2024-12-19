%{

%}

%token <string> SELECT ID CREATE TABLE NUMBER_TYPE VARCHAR INSERT INTO VALUES DELETE FROM WHERE AND OR NOT SHOW TABLES NOT_EQUAL LESS_OR_EQUAL GREATER_OR_EQUAL STRING NUMBER UPDATE SET ORDER BY ASC DESC
%token <int> POSITIVE
%type <string> statement column_type create_table_statement insert_statement  delete_statement show_tables_statement logical_operator select_statement boolean_expression string_number_id string_number update_statement number
%type <List<string>> comma_sep_id comma_sep_id_include_star comma_sep_value
%type <List<(string, string)>> column_declare
%type <List<object>> order_by_column
%type <List<List<object>>> order_by_condition
%type <List<Tuple<string, string>>> set_expression
%type <object> expression term 
%%

statement: create_table_statement | insert_statement | delete_statement | show_tables_statement | select_statement | update_statement | expression;

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
DELETE FROM ID
{
    MyDBNs.SqlLexYaccCallback.Delete($3, null);
}
|
DELETE FROM ID WHERE boolean_expression
{
    MyDBNs.SqlLexYaccCallback.Delete($3, $5);
}
;

update_statement:
UPDATE ID SET set_expression
{
    MyDBNs.SqlLexYaccCallback.Update($2, $4, null);
}
|
UPDATE ID SET set_expression WHERE boolean_expression
{
    MyDBNs.SqlLexYaccCallback.Update($2, $4, $6);
}
;

show_tables_statement:
SHOW TABLES
{
    MyDBNs.SqlLexYaccCallback.ShowTables();
}
;

select_statement:
SELECT comma_sep_id_include_star FROM ID
{
    MyDBNs.SqlLexYaccCallback.Select($2, $4, null, null);
}
|
SELECT comma_sep_id_include_star FROM ID ORDER BY order_by_condition
{
    MyDBNs.SqlLexYaccCallback.Select($2, $4, null, $7);
}
|
SELECT comma_sep_id_include_star FROM ID WHERE boolean_expression
{
    MyDBNs.SqlLexYaccCallback.Select($2, $4, $6, null);
}
|
SELECT comma_sep_id_include_star FROM ID WHERE boolean_expression ORDER BY order_by_condition
{
    MyDBNs.SqlLexYaccCallback.Select($2, $4, $6, $9);
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

set_expression:
ID '=' string_number_id ',' set_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpression($1, $3, $5);
}
|
ID '=' string_number_id
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpression($1, $3);
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

order_by_condition:
order_by_column
{
    MyDBNs.SqlLexYaccCallback.OrderByCondition($$, $1, null);
}
|
order_by_column ',' order_by_condition
{
    MyDBNs.SqlLexYaccCallback.OrderByCondition($$, $1, $3);
};

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

expression:
expression '+' expression 
{

}
| expression '-' expression 
{

}
|
'(' expression ')'
{

}
| 
term 
{

}
;

term:
term '*' string_number_id 
{

}
| term '/' string_number_id 
{

}
| string_number_id 
{

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
number
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
number
{
    $$ = $1;
}
;

order_by_column:
ID
{
    MyDBNs.SqlLexYaccCallback.OrderByColumn(ref $$, $1, true);
}
| 
ID ASC
{
    MyDBNs.SqlLexYaccCallback.OrderByColumn(ref $$, $1, true);
}
| 
ID DESC
{
    MyDBNs.SqlLexYaccCallback.OrderByColumn(ref $$, $1, false);
}
| 
POSITIVE
{
    MyDBNs.SqlLexYaccCallback.OrderByColumn(ref $$, $1, true);
}
| 
POSITIVE ASC
{
    MyDBNs.SqlLexYaccCallback.OrderByColumn(ref $$, $1, true);
}
| 
POSITIVE DESC
{
    MyDBNs.SqlLexYaccCallback.OrderByColumn(ref $$, $1, false);
}
;

logical_operator: AND | OR;

number:
NUMBER
{
    $$ = $1;
}
| 
POSITIVE
{
    $$ = "" + $1;
}
;

column_type: VARCHAR '(' POSITIVE ')' {$$ = $1 + "(" + $3 + ")";} | NUMBER_TYPE {$$ = $1;};
%%