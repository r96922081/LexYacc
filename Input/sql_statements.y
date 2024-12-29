%{

%}

%token <string> SELECT ID CREATE TABLE NUMBER_TYPE VARCHAR INSERT INTO VALUES DELETE FROM WHERE AND OR NOT SHOW TABLES NOT_EQUAL LESS_OR_EQUAL GREATER_OR_EQUAL STRING UPDATE SET ORDER BY ASC DESC DROP SAVE LOAD DB FILE_PATH TWO_PIPE NULL IS
%token <int> POSITIVE_INT
%token <double> DOUBLE
%type <string> column_type save_db load_db create_table_statement show_tables_statement drop_table_statement logical_operator boolean_expression string_number_id file_path arithmetic_expression string_expression term number_double_id string_id arithmetic_expression_id string_number_null column_name
%type <List<string>> comma_sep_id commaSep_id_star commaSep_string_number_null
%type <List<(string, string)>> column_declare
%type <List<object>> order_by_column
%type <List<List<object>>> order_by_condition
%type <List<MyDBNs.SetExpressionType>> set_expression
%type <MyDBNs.SelectedData> select_statement
%type <int> delete_statement insert_statement update_statement
%type <object> statement
%type <double> number_double
%%

statement: save_db { $$ = $1; } | load_db { $$ = $1; } | create_table_statement { $$ = $1; } | drop_table_statement { $$ = $1; } | insert_statement { $$ = $1; } | delete_statement { $$ = $1; } | show_tables_statement { $$ = $1; } | select_statement { $$ = $1; } | update_statement { $$ = $1; };

save_db: SAVE DB file_path
{
    MyDBNs.SqlLexYaccCallback.SaveDB($3);
};

load_db: LOAD DB file_path
{
    MyDBNs.SqlLexYaccCallback.LoadDB($3);
};


create_table_statement: CREATE TABLE ID '(' column_declare ')' 
{
    MyDBNs.SqlLexYaccCallback.CreateTable($3, $5);
};

drop_table_statement: DROP TABLE ID
{
    MyDBNs.SqlLexYaccCallback.DropTable($3);
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
INSERT INTO ID VALUES '(' commaSep_string_number_null ')'
{
    $$ = MyDBNs.SqlLexYaccCallback.Insert($3, null, $6);
}
|
INSERT INTO ID '(' comma_sep_id ')' VALUES '(' commaSep_string_number_null ')'
{
    $$ = MyDBNs.SqlLexYaccCallback.Insert($3, $5, $9);
};

delete_statement:
DELETE FROM ID
{
    $$ = MyDBNs.SqlLexYaccCallback.Delete($3, null);
}
|
DELETE FROM ID WHERE boolean_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.Delete($3, $5);
}
;

update_statement:
UPDATE ID SET set_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.Update($2, $4, null);
}
|
UPDATE ID SET set_expression WHERE boolean_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.Update($2, $4, $6);
}
;

show_tables_statement:
SHOW TABLES
{
    MyDBNs.SqlLexYaccCallback.ShowTables();
}
;

select_statement:
SELECT commaSep_id_star FROM ID
{
    $$ = MyDBNs.SqlLexYaccCallback.Select($2, $4, null, null);
}
|
SELECT commaSep_id_star FROM ID ORDER BY order_by_condition
{
    $$ = MyDBNs.SqlLexYaccCallback.Select($2, $4, null, $7);
}
|
SELECT commaSep_id_star FROM ID WHERE boolean_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.Select($2, $4, $6, null);
}
|
SELECT commaSep_id_star FROM ID WHERE boolean_expression ORDER BY order_by_condition
{
    $$ = MyDBNs.SqlLexYaccCallback.Select($2, $4, $6, $9);
}
;

boolean_expression:
boolean_expression AND boolean_expression
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "AND", $3);
}
|
boolean_expression OR boolean_expression
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "OR", $3);
}
| 
'(' boolean_expression ')'
{
    $$ = " ( " + $2 + " ) ";
}
| 
string_expression '=' string_expression
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "=", $3);
}
| 
string_expression '<' string_expression
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "<", $3);
}
| 
string_expression '>' string_expression
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, ">", $3);
}
| 
string_expression NOT_EQUAL string_expression
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "!=", $3);
}
| 
string_expression LESS_OR_EQUAL string_expression
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "<=", $3);
}
| 
string_expression GREATER_OR_EQUAL string_expression
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, ">=", $3);
}
| 
arithmetic_expression_id '=' arithmetic_expression_id
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "=", $3);
}
| 
arithmetic_expression_id '<' arithmetic_expression_id
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "<", $3);
}
| 
arithmetic_expression_id '>' arithmetic_expression_id
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, ">", $3);
}
| 
arithmetic_expression_id NOT_EQUAL arithmetic_expression_id
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "!=", $3);
}
| 
arithmetic_expression_id LESS_OR_EQUAL arithmetic_expression_id
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "<=", $3);
}
| 
arithmetic_expression_id GREATER_OR_EQUAL arithmetic_expression_id
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, ">=", $3);
}
|
column_name IS NULL
{
     MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "IS", "NULL");
}
|
column_name IS NOT NULL
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "IS NOT", "NULL");
}
;

set_expression:
ID '=' string_expression ',' set_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpressionVarchar($1, $3, $5);
}
|
ID '=' string_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpressionVarchar($1, $3);
}
|
ID '=' arithmetic_expression ',' set_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpressionNumber($1, $3, $5);
}
|
ID '=' arithmetic_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpressionNumber($1, $3);
}
|
ID '=' NULL ',' set_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpressionNull($1, $5);
}
|
ID '=' NULL
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpressionNull($1);
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

commaSep_string_number_null: 
string_number_null 
{
    MyDBNs.SqlLexYaccCallback.CommaSepID($$, $1);
}
| string_number_null ',' commaSep_string_number_null
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

commaSep_id_star: 
ID 
{
    MyDBNs.SqlLexYaccCallback.CommaSepIDIncludeStar($$, $1);
}
|
ID ',' commaSep_id_star
{
    MyDBNs.SqlLexYaccCallback.CommaSepIDIncludeStar($$, $1, $3);
}
| '*'
{
    MyDBNs.SqlLexYaccCallback.CommaSepIDIncludeStar($$, "*");
}
| '*' ',' commaSep_id_star
{
    MyDBNs.SqlLexYaccCallback.CommaSepIDIncludeStar($$, "*", $3);
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
number_double
{
    $$ = "" + $1;
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

string_number_null:
STRING
{
    $$ = $1;
}
| 
number_double
{
    $$ = "" + $1;
}
|
NULL
{
    $$ = null;
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
POSITIVE_INT
{
    MyDBNs.SqlLexYaccCallback.OrderByColumn(ref $$, $1, true);
}
| 
POSITIVE_INT ASC
{
    MyDBNs.SqlLexYaccCallback.OrderByColumn(ref $$, $1, true);
}
| 
POSITIVE_INT DESC
{
    MyDBNs.SqlLexYaccCallback.OrderByColumn(ref $$, $1, false);
}
;

file_path:
FILE_PATH
{
    $$ = $1;
}
|
POSITIVE_INT
{
    $$ = "" + $1;
}
|
DOUBLE
{
    $$ = "" + $1;
}
|
ID
{
    $$ = $1;
}
;

logical_operator: AND | OR;

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

column_type: VARCHAR '(' POSITIVE_INT ')' {$$ = $1 + "(" + $3 + ")";} | NUMBER_TYPE {$$ = $1;};

column_name:
ID
{
    $$ = $1;
}

%%