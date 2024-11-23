%{

%}

%token <string> SELECT STRING CREATE TABLE NUMBER VARCHAR INSERT INTO VALUES
%type <string> statement column_declare column_type create_table_statement insert_statement comma_sep_string

%%

statement: create_table_statement | insert_statement;

create_table_statement: CREATE TABLE STRING '(' column_declare ')' 
{
    SqlTest.CreateTable($3, SqlYaccData.columnNames, SqlYaccData.columnTypes);
};

column_declare: STRING column_type 
{
    SqlYaccData.columnNames.Add($1);

    if ($2 == "NUMBER")
        SqlYaccData.columnTypes.Add(ColumnType.NUMBER);
    else if ($2 == "VARCHAR")
        SqlYaccData.columnTypes.Add(ColumnType.VARCHAR);
} 
| 
STRING column_type ',' column_declare 
{
    SqlYaccData.columnNames.Add($1);

    if ($2 == "NUMBER")
        SqlYaccData.columnTypes.Add(ColumnType.NUMBER);
    else if ($2 == "VARCHAR")
        SqlYaccData.columnTypes.Add(ColumnType.VARCHAR);
};

insert_statement: 
INSERT INTO STRING VALUES '(' comma_sep_string ')'
{
    SqlTest.InsertRow($3, null, $6);
}
|
INSERT INTO STRING '(' comma_sep_string ')' VALUES '(' comma_sep_string ')'
{
    SqlTest.InsertRow($3, $5, $9);
};

comma_sep_string: 
STRING 
{
    $$ = $1;
}
| STRING ',' comma_sep_string
{
    $$ = $1 + "," + $3;
}
;

column_type: VARCHAR {$$ = $1;} | NUMBER {$$ = $1;};
%%