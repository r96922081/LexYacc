%{
%}

%token <int>         INT_VALUE
%token <string>      RETURN ID INT_TYPE VOID_TYPE

%type <CCompilerNs.Program>           program 
%type <CCompilerNs.FunDecl>           funDecl
%type <CCompilerNs.Statement>         statement
%type <CCompilerNs.ReturnStatement>   returnStatement 
%type <int>          int_value
%type <string>       id typeSpec funName

%%
program: 
funDecl 
{
    $$= CCompilerNs.CCLexYaccCallback.Program($1);
};

funDecl:
typeSpec funName '(' ')' '{' statement '}'
{  
    $$= CCompilerNs.CCLexYaccCallback.FuncDecl($1, $2, $6);
}
;


funName:
id
{
    $$ = $1;
}
;

typeSpec:
INT_TYPE 
{
    $$ = $1;
}
;

statement: 
returnStatement
{     
    $$ = $1;
}
;

returnStatement:
RETURN ';'
{
    $$= CCompilerNs.CCLexYaccCallback.ReturnStatement(null);
}
|
RETURN int_value ';'
{
    $$= CCompilerNs.CCLexYaccCallback.ReturnStatement($2);
};

id: 
ID
{
    $$=  $1;
}
;

int_value: 
INT_VALUE
{
    $$ = $1;
}
;
%%