%{

%}

%token <string>                                      SELECT ID CREATE TABLE NUMBER VARCHAR INSERT INTO VALUES DELETE FROM WHERE AND OR NOT SHOW TABLES NOT_EQUAL LESS_OR_EQUAL GREATER_OR_EQUAL STRING UPDATE SET ORDER BY ASC DESC DROP SAVE LOAD DB FILE_PATH TWO_PIPE NULL IS LIKE TRANSACTION COMMIT ROLLBACK START GROUP MIN MAX SUM COUNT ID_DOT_ID ID_DOT_STAR JOIN ON AS
%token <int>                                         POSITIVE_INT
%token <double>                                      DOUBLE

%type <string>                                       column_type save_db load_db create_table_statement show_tables_statement drop_table_statement logical_operator boolean_expression string_number_column file_path arithmetic_expression string_expression term number_column string_column arithmeticExpression_column string_number_null table column transaction_start relational_operator 
%type <List<string>>                                 columns string_number_null_list
%type <MyDBNs.TableNameAlias>                        table_id
%type <MyDBNs.JoinTable>                             join_table
%type <List<MyDBNs.JoinTable>>                       join_tables
%type <MyDBNs.Tables>                                table_or_joins
%type <MyDBNs.ColumnDeclare>                         column_declare
%type <List<MyDBNs.ColumnDeclare>>                   column_declares
%type <MyDBNs.OrderByColumn>                         order_by_column
%type <List<MyDBNs.OrderByColumn>>                   order_by_columns
%type <MyDBNs.AggregationColumn>                     aggregation_column aggregation_column_as
%type <List<MyDBNs.AggregationColumn>>               aggregation_columns
%type <List<MyDBNs.SetExpressionType>>               set_expression
%type <MyDBNs.SelectedData>                          select_statement
%type <int>                                          delete_statement insert_statement update_statement commit rollback
%type <object>                                       statement
%type <double>                                       number_double
%%

statement: save_db { $$ = $1; } | load_db { $$ = $1; } | transaction_start { $$ = $1; } | commit { $$ = $1; } | rollback { $$ = $1; }| create_table_statement { $$ = $1; } | drop_table_statement { $$ = $1; } | insert_statement { $$ = $1; } | delete_statement { $$ = $1; } | show_tables_statement { $$ = $1; } | select_statement { $$ = $1; } | update_statement { $$ = $1; };

save_db: SAVE DB file_path
{
    MyDBNs.SqlStatementsLexYaccCallback.SaveDB($3);
};

load_db: LOAD DB file_path
{
    MyDBNs.SqlStatementsLexYaccCallback.LoadDB($3);
};

transaction_start: TRANSACTION START
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.TransactionStart();
}
;

commit: COMMIT
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Commit();
}
;

rollback: ROLLBACK
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Rollback();
}
;

create_table_statement: CREATE TABLE table '(' column_declares ')' 
{
    MyDBNs.SqlStatementsLexYaccCallback.CreateTable($3, $5);
};

drop_table_statement: DROP TABLE table
{
    MyDBNs.SqlStatementsLexYaccCallback.DropTable($3);
};

column_declares: column_declares ',' column_declare
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.ColumnDeclares($3, $1);
} 
| 
column_declare
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.ColumnDeclares($1, null);
};

column_declare: column column_type 
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.ColumnDeclare($1, $2);
}
;

insert_statement: 
INSERT INTO table VALUES '(' string_number_null_list ')'
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Insert($3, null, $6);
}
|
INSERT INTO table '(' columns ')' VALUES '(' string_number_null_list ')'
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Insert($3, $5, $9);
};

delete_statement:
DELETE FROM table
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Delete($3, null);
}
|
DELETE FROM table WHERE boolean_expression
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Delete($3, $5);
}
;

update_statement:
UPDATE table SET set_expression
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Update($2, $4, null);
}
|
UPDATE table SET set_expression WHERE boolean_expression
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Update($2, $4, $6);
}
;

show_tables_statement:
SHOW TABLES
{
    MyDBNs.SqlStatementsLexYaccCallback.ShowTables();
}
;

select_statement:
SELECT aggregation_columns FROM table_or_joins
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Select($2, $4, null, null, null);
}
|
SELECT aggregation_columns FROM table_or_joins GROUP BY columns
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Select($2, $4, null, $7, null);
}
|
SELECT aggregation_columns FROM table_or_joins ORDER BY order_by_columns
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Select($2, $4, null, null, $7);
}
|
SELECT aggregation_columns FROM table_or_joins GROUP BY columns ORDER BY order_by_columns
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Select($2, $4, null, $7, $10);
}
|
SELECT aggregation_columns FROM table_or_joins WHERE boolean_expression
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Select($2, $4, $6, null, null);
}
|
SELECT aggregation_columns FROM table_or_joins WHERE boolean_expression GROUP BY columns
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Select($2, $4, $6, $9, null);
}
|
SELECT aggregation_columns FROM table_or_joins WHERE boolean_expression ORDER BY order_by_columns
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Select($2, $4, $6, null, $9);
}
|
SELECT aggregation_columns FROM table_or_joins WHERE boolean_expression GROUP BY columns ORDER BY order_by_columns
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Select($2, $4, $6, $9, $12);
}
;

table_or_joins:
table_id
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Tables($1, null);
}
|
table_id join_tables
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.Tables($1, $2);
}
;

join_tables:
join_tables join_table
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.JoinTables($1, $2);
}
|
join_table
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.JoinTables(null, $1);
}
;

join_table:
JOIN table_id 
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.JoinTable($2, null);
}
|
JOIN table_id ON boolean_expression
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.JoinTable($2, $4);
}
;

boolean_expression:
boolean_expression logical_operator boolean_expression
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.BooleanExpression($1, $2, $3);
}
| 
'(' boolean_expression ')'
{
    $$ = " ( " + $2 + " ) ";
}
| 
string_expression relational_operator string_expression
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.BooleanExpression($1, $2, $3);
}
| 
arithmeticExpression_column relational_operator arithmeticExpression_column
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.BooleanExpression($1, $2, $3);
}
|
column IS NULL
{
     $$ = MyDBNs.SqlStatementsLexYaccCallback.BooleanExpression($1, "IS", "NULL");
}
|
column IS NOT NULL
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.BooleanExpression($1, "IS NOT", "NULL");
}
|
column LIKE STRING
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.BooleanExpression($1, "LIKE", $3);
}
|
column NOT LIKE STRING
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.BooleanExpression($1, "NOT LIKE", $4);
}
;

set_expression:
column '=' string_expression ',' set_expression
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.SetExpressionVarchar($1, $3, $5);
}
|
column '=' string_expression
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.SetExpressionVarchar($1, $3);
}
|
column '=' arithmetic_expression ',' set_expression
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.SetExpressionNumber($1, $3, $5);
}
|
column '=' arithmetic_expression
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.SetExpressionNumber($1, $3);
}
|
column '=' NULL ',' set_expression
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.SetExpressionNull($1, $5);
}
|
column '=' NULL
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.SetExpressionNull($1);
}
;

columns: 
column 
{
    MyDBNs.SqlStatementsLexYaccCallback.CommaSepColumn($$, $1);
}
| column ',' columns
{
    MyDBNs.SqlStatementsLexYaccCallback.CommaSepColumn($$, $1, $3);
}
;

string_number_null_list: 
string_number_null 
{
    MyDBNs.SqlStatementsLexYaccCallback.CommaSepColumn($$, $1);
}
| string_number_null ',' string_number_null_list
{
    MyDBNs.SqlStatementsLexYaccCallback.CommaSepColumn($$, $1, $3);
}
;

order_by_columns:
order_by_column
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.OrderByColumns($1, null);
}
|
order_by_columns ',' order_by_column
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.OrderByColumns($3, $1);
};

aggregation_columns:
aggregation_columns ',' aggregation_column_as
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.AggregrationColumns($1, $3);
}
|
aggregation_column_as
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.AggregrationColumns(null, $1);
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
    $$ = MyDBNs.SqlStatementsLexYaccCallback.OrderByColumn($1, true);
}
| 
column ASC
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.OrderByColumn($1, true);
}
| 
column DESC
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.OrderByColumn($1, false);
}
| 
POSITIVE_INT
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.OrderByColumn($1, true);
}
| 
POSITIVE_INT ASC
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.OrderByColumn($1, true);
}
| 
POSITIVE_INT DESC
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.OrderByColumn($1, false);
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
|
ID_DOT_ID
{
    $$ = $1;
}
;

aggregation_column_as:
aggregation_column
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.AggregationColumnAs($1, null);
}
|
aggregation_column ID
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.AggregationColumnAs($1, $2);
}
|
aggregation_column AS ID
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.AggregationColumnAs($1, $3);
}
;

aggregation_column:
MAX '(' column ')'
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.AggregationColumn(MyDBNs.AggerationOperation.MAX, $3);
}
|
MIN '(' column ')'
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.AggregationColumn(MyDBNs.AggerationOperation.MIN, $3);
}
|
COUNT '(' column ')'
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.AggregationColumn(MyDBNs.AggerationOperation.COUNT, $3);
}
|
SUM '(' column ')'
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.AggregationColumn(MyDBNs.AggerationOperation.SUM, $3);
}
|
column
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.AggregationColumn(MyDBNs.AggerationOperation.NONE, $1);
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
{
    $$ = $1;
}
| 
OR
{
    $$ = $1;
};

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

table_id:
ID
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.TableNameAlias($1, null);
}
|
ID ID
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.TableNameAlias($1, $2);
}
|
ID AS ID
{
    $$ = MyDBNs.SqlStatementsLexYaccCallback.TableNameAlias($1, $3);
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
|
'*'
{
    $$ = "*";
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