%{
%}

%token <int>         INT_VALUE
%token <string>      RETURN ID INT_TYPE VOID_TYPE

%type <CCompilerNs.Program>           program 
%type <CCompilerNs.FunDecl>           funDecl
%type <CCompilerNs.Statement>         statement
%type <CCompilerNs.ReturnStatement>   returnStatement 
%type <CCompilerNs.AddExpression>     addExpression
%type <string>                        typeSpec

%%
program: 
funDecl 
{
    $$= CCompilerNs.CCLexYaccCallback.Program($1);
};

funDecl:
typeSpec ID '(' ')' '{' statement '}'
{  
    $$= CCompilerNs.CCLexYaccCallback.FuncDecl($1, $2, $6);
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
RETURN addExpression ';'
{
    $$= CCompilerNs.CCLexYaccCallback.ReturnStatement($2);
};

addExpression: 
addExpression '+' INT_VALUE
{
    $$ = CCompilerNs.CCLexYaccCallback.AddExpression($1, "+", $3);
}
| addExpression '-' INT_VALUE
{
    $$ = CCompilerNs.CCLexYaccCallback.AddExpression($1, "-", $3);
}
|
INT_VALUE
{
    $$ = CCompilerNs.CCLexYaccCallback.AddExpression($1);
}
;  

%%