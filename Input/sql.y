%{

%}

%token <string> SELECT STRING CREATE TABLE NUMBER VARCHAR INSERT INTO VALUES DELETE FROM WHERE AND OR NOT SHOW TABLES NOT_EQUAL LESS_OR_EQUAL GREATER_OR_EQUAL
%type <string> statement column_type create_table_statement insert_statement  delete_statement show_tables_statement show_table_statement logical_operator
%type <List<string>> comma_sep_string
%type <List<(string, string)>> column_declare
%type <bool> boolean_expression
%%

statement: create_table_statement | insert_statement | delete_statement | show_tables_statement | show_table_statement;

create_table_statement: CREATE TABLE STRING '(' column_declare ')' 
{
    SqlLexYaccCallback.CreateTable($3, $5);
};

column_declare: STRING column_type 
{
    SqlLexYaccCallback.ColumnDeclare($$, $1, $2);
} 
| 
STRING column_type ',' column_declare 
{
    SqlLexYaccCallback.ColumnDeclare($$, $1, $2, $4);
};

insert_statement: 
INSERT INTO STRING VALUES '(' comma_sep_string ')'
{
    SqlLexYaccCallback.InsertRow($3, null, $6);
}
|
INSERT INTO STRING '(' comma_sep_string ')' VALUES '(' comma_sep_string ')'
{
    SqlLexYaccCallback.InsertRow($3, $5, $9);
};

delete_statement:
DELETE FROM STRING WHERE boolean_expression
;

show_tables_statement:
SHOW TABLES
{
    SqlLexYaccCallback.ShowTables();
}
;

show_table_statement:
SHOW TABLE STRING
{
    SqlLexYaccCallback.ShowTable($3);
}
;

boolean_expression:
boolean_expression AND boolean_expression
|
boolean_expression OR boolean_expression
| 
'(' boolean_expression ')'
| 
STRING '=' STRING
| 
STRING '<' STRING
| 
STRING '>' STRING
| 
STRING NOT_EQUAL STRING
| 
STRING LESS_OR_EQUAL STRING
| 
STRING GREATER_OR_EQUAL STRING
;

comma_sep_string: 
STRING 
{
    SqlLexYaccCallback.CommaSepString($$, $1);
}
| STRING ',' comma_sep_string
{
    SqlLexYaccCallback.CommaSepString($$, $1, $3);
}
;

logical_operator: AND | OR;

column_type: VARCHAR '(' STRING ')' {$$ = $1 + "(" + $3 + ")";} | NUMBER {$$ = $1;};
%%