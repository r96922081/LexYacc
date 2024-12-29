%{

%}

%token <string> SELECT ID CREATE TABLE NUMBER VARCHAR INSERT INTO VALUES DELETE FROM WHERE AND OR NOT SHOW TABLES NOT_EQUAL LESS_OR_EQUAL GREATER_OR_EQUAL STRING UPDATE SET ORDER BY ASC DESC DROP SAVE LOAD DB FILE_PATH TWO_PIPE NULL IS LIKE TRANSACTION COMMIT ROLLBACK START GROUP MIN MAX SUM COUNT
%token <int> POSITIVE_INT
%token <double> DOUBLE

%type <string> column_type save_db load_db create_table_statement show_tables_statement drop_table_statement logical_operator boolean_expression string_number_column file_path arithmetic_expression string_expression term number_column string_column arithmeticExpression_column string_number_null table column transaction_start groupBy_column
%type <List<string>> commaSep_column commaSep_column_star commaSep_string_number_null
%type <List<(string, string)>> column_declare
%type <List<object>> order_by_column
%type <List<List<object>>> order_by_condition
%type <List<MyDBNs.SetExpressionType>> set_expression
%type <MyDBNs.SelectedData> select_statement
%type <int> delete_statement insert_statement update_statement commit rollback
%type <object> statement
%type <double> number_double
%%

statement: save_db { $$ = $1; } | load_db { $$ = $1; } | transaction_start { $$ = $1; } | commit { $$ = $1; } | rollback { $$ = $1; }| create_table_statement { $$ = $1; } | drop_table_statement { $$ = $1; } | insert_statement { $$ = $1; } | delete_statement { $$ = $1; } | show_tables_statement { $$ = $1; } | select_statement { $$ = $1; } | update_statement { $$ = $1; };

save_db: SAVE DB file_path
{
    MyDBNs.SqlLexYaccCallback.SaveDB($3);
};

load_db: LOAD DB file_path
{
    MyDBNs.SqlLexYaccCallback.LoadDB($3);
};

transaction_start: TRANSACTION START
{
    $$ = MyDBNs.SqlLexYaccCallback.TransactionStart();
}
;

commit: COMMIT
{
    $$ = MyDBNs.SqlLexYaccCallback.Commit();
}
;

rollback: ROLLBACK
{
    $$ = MyDBNs.SqlLexYaccCallback.Rollback();
}
;

create_table_statement: CREATE TABLE table '(' column_declare ')' 
{
    MyDBNs.SqlLexYaccCallback.CreateTable($3, $5);
};

drop_table_statement: DROP TABLE table
{
    MyDBNs.SqlLexYaccCallback.DropTable($3);
};

column_declare: column column_type 
{
    MyDBNs.SqlLexYaccCallback.ColumnDeclare($$, $1, $2);
} 
| 
column column_type ',' column_declare 
{
    MyDBNs.SqlLexYaccCallback.ColumnDeclare($$, $1, $2, $4);
};

insert_statement: 
INSERT INTO table VALUES '(' commaSep_string_number_null ')'
{
    $$ = MyDBNs.SqlLexYaccCallback.Insert($3, null, $6);
}
|
INSERT INTO table '(' commaSep_column ')' VALUES '(' commaSep_string_number_null ')'
{
    $$ = MyDBNs.SqlLexYaccCallback.Insert($3, $5, $9);
};

delete_statement:
DELETE FROM table
{
    $$ = MyDBNs.SqlLexYaccCallback.Delete($3, null);
}
|
DELETE FROM table WHERE boolean_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.Delete($3, $5);
}
;

update_statement:
UPDATE table SET set_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.Update($2, $4, null);
}
|
UPDATE table SET set_expression WHERE boolean_expression
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
SELECT commaSep_column_star FROM table
{
    $$ = MyDBNs.SqlLexYaccCallback.Select($2, $4, null, null);
}
|
SELECT commaSep_column_star FROM table ORDER BY order_by_condition
{
    $$ = MyDBNs.SqlLexYaccCallback.Select($2, $4, null, $7);
}
|
SELECT commaSep_column_star FROM table WHERE boolean_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.Select($2, $4, $6, null);
}
|
SELECT commaSep_column_star FROM table WHERE boolean_expression ORDER BY order_by_condition
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
arithmeticExpression_column '=' arithmeticExpression_column
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "=", $3);
}
| 
arithmeticExpression_column '<' arithmeticExpression_column
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "<", $3);
}
| 
arithmeticExpression_column '>' arithmeticExpression_column
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, ">", $3);
}
| 
arithmeticExpression_column NOT_EQUAL arithmeticExpression_column
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "!=", $3);
}
| 
arithmeticExpression_column LESS_OR_EQUAL arithmeticExpression_column
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "<=", $3);
}
| 
arithmeticExpression_column GREATER_OR_EQUAL arithmeticExpression_column
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, ">=", $3);
}
|
column IS NULL
{
     MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "IS", "NULL");
}
|
column IS NOT NULL
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "IS NOT", "NULL");
}
|
column LIKE STRING
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "LIKE", $3);
}
|
column NOT LIKE STRING
{
    MyDBNs.SqlLexYaccCallback.BooleanExpression(ref $$, $1, "NOT LIKE", $4);
}
;

set_expression:
column '=' string_expression ',' set_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpressionVarchar($1, $3, $5);
}
|
column '=' string_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpressionVarchar($1, $3);
}
|
column '=' arithmetic_expression ',' set_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpressionNumber($1, $3, $5);
}
|
column '=' arithmetic_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpressionNumber($1, $3);
}
|
column '=' NULL ',' set_expression
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpressionNull($1, $5);
}
|
column '=' NULL
{
    $$ = MyDBNs.SqlLexYaccCallback.SetExpressionNull($1);
}
;

commaSep_column: 
column 
{
    MyDBNs.SqlLexYaccCallback.CommaSepColumn($$, $1);
}
| column ',' commaSep_column
{
    MyDBNs.SqlLexYaccCallback.CommaSepColumn($$, $1, $3);
}
;

commaSep_string_number_null: 
string_number_null 
{
    MyDBNs.SqlLexYaccCallback.CommaSepColumn($$, $1);
}
| string_number_null ',' commaSep_string_number_null
{
    MyDBNs.SqlLexYaccCallback.CommaSepColumn($$, $1, $3);
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

commaSep_column_star: 
column 
{
    MyDBNs.SqlLexYaccCallback.CommaSep_Column_Star($$, $1);
}
|
column ',' commaSep_column_star
{
    MyDBNs.SqlLexYaccCallback.CommaSep_Column_Star($$, $1, $3);
}
| '*'
{
    MyDBNs.SqlLexYaccCallback.CommaSep_Column_Star($$, "*");
}
| '*' ',' commaSep_column_star
{
    MyDBNs.SqlLexYaccCallback.CommaSep_Column_Star($$, "*", $3);
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

arithmeticExpression_column:
column
{
    $$ = $1;
}
| 
arithmetic_expression
{
    $$ = $1;
}
;

order_by_column:
column
{
    MyDBNs.SqlLexYaccCallback.OrderByColumn(ref $$, $1, true);
}
| 
column ASC
{
    MyDBNs.SqlLexYaccCallback.OrderByColumn(ref $$, $1, true);
}
| 
column DESC
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

groupBy_column:
MAX '(' column ')'
{

}
|
MIN '(' column ')'
{

}
|
COUNT '(' column ')'
{

}
|
SUM '(' column ')'
{

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

string_number_column:
column
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

logical_operator: 
AND 
| 
OR;

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

column_type: 
VARCHAR '(' POSITIVE_INT ')' 
{
    $$ = $1 + "(" + $3 + ")";
} 
| 
NUMBER 
{
    $$ = $1;
}
;

table:
ID
{
    $$ = $1;
}
;

column:
ID
{
    $$ = $1;
}
;

%%