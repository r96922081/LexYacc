%{
%}

%token <int>         INT_VALUE
%token <string>      RETURN ID INT

%type  <CCompilerNs.AstNode> program funDecl typeSpec funName statement assignmentExpression returnStatement id int_value

%%
program: 
funDecl 
{
    $$ = new CCompilerNs.AstNode("program");
    $$.children.Add($1);   
};

funDecl:
typeSpec funName '(' ')' '{' statement '}'
{
    $$ = new CCompilerNs.AstNode("funDecl");
    $$.children.Add($1);   
    $$.children.Add($2);
    $$.children.Add($6);    
}
;


funName:
id
{
    $$ = new CCompilerNs.AstNode("funName");
    $$.children.Add($1);
}
;

typeSpec:
INT 
{
    $$ = new CCompilerNs.AstNode("typeSpec");
    $$.children.Add(new CCompilerNs.AstNode("int"));  
}
;

statement: 
assignmentExpression
{
    $$ = new CCompilerNs.AstNode("statement");
    $$.children.Add($1);       
}
| 
returnStatement
{
    $$ = new CCompilerNs.AstNode("statement");
    $$.children.Add($1);       
}
;

assignmentExpression:
typeSpec id '=' int_value
{
}
;

returnStatement:
RETURN ';'
{
    $$ = new CCompilerNs.AstNode("returnStmt");
    $$.children.Add(new CCompilerNs.AstNode("return"));
}
|
RETURN int_value ';'
{
    $$ = new CCompilerNs.AstNode("returnStmt");
    $$.children.Add(new CCompilerNs.AstNode("return"));
    $$.children.Add($2);
};

id: 
ID
{
$$ = new CCompilerNs.AstNode("id");
$$.children.Add(new CCompilerNs.AstNode($1));
}
;

int_value: 
INT_VALUE
{
$$ = new CCompilerNs.AstNode("int_value");
$$.children.Add(new CCompilerNs.AstNode("" + $1));
}
;
%%