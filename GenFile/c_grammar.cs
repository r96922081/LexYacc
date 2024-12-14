//LexYacc Gen
public class c_grammar
{
    public static object Parse(string input)
    {
        return c_grammarNs.LexYaccNs.LexYacc.Parse(input, c_grammarNs.LexActions.ruleInput, c_grammarNs.YaccActions.ruleInput, c_grammarNs.LexActions.CallAction, c_grammarNs.YaccActions.CallAction);
    }
}
//Yacc Gen 
namespace c_grammarNs
{

public class c_grammar_gv{
    public static AstNode root;
}

public class YaccActions{

    public static Dictionary<string, Func<Dictionary<int, object>, object>> actions = new Dictionary<string, Func<Dictionary<int, object>, object>>();

    public static string ruleInput = @"%{

public class c_grammar_gv{
    public static AstNode root;
}

%}

%token <int> CONSTANT
%token <string>  VOID INT  ID RETURN
%type <string>   program
%type <AstNode>  declList decl funDecl typeSpec returnStmt funName param paramList id constant 
%type <AstNode>  primaryExpression addExpression mulExpression
%type <AstNode>  compoundStatement statementList statement expressionStatement assignmentExpression

%%
program: declList
  {
    c_grammar_gv.root = new AstNode(""program"");
    c_grammar_gv.root.children.Add($1);
    c_grammar_gv.root.Print();
    return c_grammar_gv.root.GetPrintString();
  };
declList:
  declList decl
  {
     $1.children.Add($2); 
     $$ = new AstNode(""declList"");
     $$.children.AddRange($1.children);
  }
  |
  decl
  {
    $$ = new AstNode(""declList"");
    $$.children.Add($1);  
  };
decl:
  funDecl
  {
    $$ = new AstNode(""decl"");
    $$.children.Add($1);
  };
funDecl:
  typeSpec funName '(' ')' compoundStatement
  {
    $$ = new AstNode(""funDecl"");
    $$.children.Add($1);   
    $$.children.Add($2);
    $$.children.Add($5);
  }
  |
  typeSpec funName '(' paramList ')' compoundStatement
  {
    $$ = new AstNode(""funDecl"");
    $$.children.Add($1);   
    $$.children.Add($2);
    $$.children.Add($4);
    $$.children.Add($6);
  }
  |
  typeSpec funName '(' ')' compoundStatement
  {
    $$ = new AstNode(""funDecl"");
    $$.children.Add($1);   
    $$.children.Add($2);
    $$.children.Add($5);
  };
paramList: 
  paramList ',' typeSpec id 
  {
     $1.children.Add($3); 
     $1.children.Add($4);
     $$ = new AstNode(""paramList"");
     $$.children.AddRange($1.children); 
  }
  |
  typeSpec id 
  {
    $$ = new AstNode(""param"");
    $$.children.Add($1); 
    $$.children.Add($2);
  };
funName:
    id
    {
    $$ = new AstNode(""funName"");
    $$.children.Add($1);
    }
    ;
returnStmt:
  RETURN ';'
  {
    $$ = new AstNode(""returnStmt"");
    $$.children.Add(new AstNode(""return""));
  }
  |
  RETURN addExpression ';'
  {
    $$ = new AstNode(""returnStmt"");
    $$.children.Add(new AstNode(""return""));
    $$.children.Add($2);
  };
typeSpec:
  INT 
  {
    $$ = new AstNode(""typeSpec"");
    $$.children.Add(new AstNode(""int""));  
  }
  | 
  VOID
  {
    $$ = new AstNode(""typeSpec"");
    $$.children.Add(new AstNode(""void""));    
  };
id: 
  ID
  {
    $$ = new AstNode(""id"");
    $$.children.Add(new AstNode($1));
  };
constant:
  CONSTANT
  {
    $$ = new AstNode(""constant"");
    $$.children.Add(new AstNode("""" + $1));
  };
primaryExpression: 
  id
  {
    $$ = new AstNode(""primaryExpression"");
    $$.children.Add($1);    
  }
  | 
  constant
  {
    $$ = new AstNode(""primaryExpression"");
    $$.children.Add($1);      
  };

mulExpression: 
    primaryExpression
    {
        $$ = new AstNode(""mulExpression"");
        $$.children.Add($1);       
    }
	| mulExpression '*' primaryExpression
    {
        $$ = new AstNode(""mulExpression"");
        $$.children.Add($1); 
        $$.children.Add(new AstNode(""*"")); 
        $$.children.Add($3); 
    }
	| mulExpression '/' primaryExpression
    {
        $$ = new AstNode(""mulExpression"");
        $$.children.Add($1); 
        $$.children.Add(new AstNode(""/"")); 
        $$.children.Add($3); 
    }
	| mulExpression '%' primaryExpression
    {
        $$ = new AstNode(""mulExpression"");
        $$.children.Add($1); 
        $$.children.Add(new AstNode(""%"")); 
        $$.children.Add($3); 
    }
	;

addExpression: 
    mulExpression
    {
        $$ = new AstNode(""addExpression"");
        $$.children.Add($1);     
    }
	| addExpression '+' mulExpression
    {
        $$ = new AstNode(""addExpression"");
        $$.children.Add($1); 
        $$.children.Add(new AstNode(""+"")); 
        $$.children.Add($3); 
    }
	| addExpression '-' mulExpression
    {
        $$ = new AstNode(""addExpression"");
        $$.children.Add($1); 
        $$.children.Add(new AstNode(""-"")); 
        $$.children.Add($3); 
    }
	;  

compoundStatement: 
    '{' '}'
    {
        $$ = new AstNode(""compoundStatement"");
    }
	| 
    '{' statementList '}'
    {
         $$ = new AstNode(""compoundStatement"");
         $$.children.Add($2);      
    }
	;
statementList: 
    statement
    {
        $$ = new AstNode(""statementList"");
        $$.children.Add($1);        
    }
	| 
    statementList statement
    {
         $1.children.Add($2); 
         $$ = new AstNode(""statementList"");
         $$.children.AddRange($1.children);    
    }
	;

statement: 
    compoundStatement
    {
        $$ = new AstNode(""statement"");
        $$.children.Add($1);        
    }
	| 
    assignmentExpression
    {
        $$ = new AstNode(""statement"");
        $$.children.Add($1);       
    }
	| 
    returnStmt
    {
        $$ = new AstNode(""statement"");
        $$.children.Add($1);       
    }
    |
    expressionStatement
    {
        $$ = new AstNode(""statement"");
        $$.children.Add($1);       
    }
	;

assignmentExpression:
    typeSpec id '=' expressionStatement
    {
        $$ = new AstNode(""assignmentExpression"");
        $$.children.Add($1);    
        $$.children.Add($2); 
        $$.children.Add(new AstNode(""="")); 
        $$.children.Add($4); 
    }
    ;
expressionStatement: 
    ';'
    {
        $$ = new AstNode(""expressionStatement"");
    }
	| 
    addExpression ';'
    {
        $$ = new AstNode(""expressionStatement"");
        $$.children.Add($1);       
    }
	;

%%

";


    public static object CallAction(string functionName, Dictionary<int, object> param)
    {
        Init();
        if (!actions.ContainsKey(functionName))
            return null;
        return actions[functionName](param);
    }

    public static void Init()
    {
        if (actions.Count != 0)
            return;

        actions.Add("Rule_start_Producton_0", Rule_start_Producton_0);
        actions.Add("Rule_program_Producton_0", Rule_program_Producton_0);
        actions.Add("Rule_declList_Producton_0", Rule_declList_Producton_0);
        actions.Add("Rule_declList_LeftRecursionExpand_Producton_0", Rule_declList_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_declList_LeftRecursionExpand_Producton_1", Rule_declList_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_decl_Producton_0", Rule_decl_Producton_0);
        actions.Add("Rule_funDecl_Producton_0", Rule_funDecl_Producton_0);
        actions.Add("Rule_funDecl_Producton_1", Rule_funDecl_Producton_1);
        actions.Add("Rule_funDecl_Producton_2", Rule_funDecl_Producton_2);
        actions.Add("Rule_paramList_Producton_0", Rule_paramList_Producton_0);
        actions.Add("Rule_paramList_LeftRecursionExpand_Producton_0", Rule_paramList_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_paramList_LeftRecursionExpand_Producton_1", Rule_paramList_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_funName_Producton_0", Rule_funName_Producton_0);
        actions.Add("Rule_returnStmt_Producton_0", Rule_returnStmt_Producton_0);
        actions.Add("Rule_returnStmt_Producton_1", Rule_returnStmt_Producton_1);
        actions.Add("Rule_typeSpec_Producton_0", Rule_typeSpec_Producton_0);
        actions.Add("Rule_typeSpec_Producton_1", Rule_typeSpec_Producton_1);
        actions.Add("Rule_id_Producton_0", Rule_id_Producton_0);
        actions.Add("Rule_constant_Producton_0", Rule_constant_Producton_0);
        actions.Add("Rule_primaryExpression_Producton_0", Rule_primaryExpression_Producton_0);
        actions.Add("Rule_primaryExpression_Producton_1", Rule_primaryExpression_Producton_1);
        actions.Add("Rule_mulExpression_Producton_0", Rule_mulExpression_Producton_0);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_0", Rule_mulExpression_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_1", Rule_mulExpression_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_2", Rule_mulExpression_LeftRecursionExpand_Producton_2);
        actions.Add("Rule_mulExpression_LeftRecursionExpand_Producton_3", Rule_mulExpression_LeftRecursionExpand_Producton_3);
        actions.Add("Rule_addExpression_Producton_0", Rule_addExpression_Producton_0);
        actions.Add("Rule_addExpression_LeftRecursionExpand_Producton_0", Rule_addExpression_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_addExpression_LeftRecursionExpand_Producton_1", Rule_addExpression_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_addExpression_LeftRecursionExpand_Producton_2", Rule_addExpression_LeftRecursionExpand_Producton_2);
        actions.Add("Rule_compoundStatement_Producton_0", Rule_compoundStatement_Producton_0);
        actions.Add("Rule_compoundStatement_Producton_1", Rule_compoundStatement_Producton_1);
        actions.Add("Rule_statementList_Producton_0", Rule_statementList_Producton_0);
        actions.Add("Rule_statementList_LeftRecursionExpand_Producton_0", Rule_statementList_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_statementList_LeftRecursionExpand_Producton_1", Rule_statementList_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_statement_Producton_0", Rule_statement_Producton_0);
        actions.Add("Rule_statement_Producton_1", Rule_statement_Producton_1);
        actions.Add("Rule_statement_Producton_2", Rule_statement_Producton_2);
        actions.Add("Rule_statement_Producton_3", Rule_statement_Producton_3);
        actions.Add("Rule_assignmentExpression_Producton_0", Rule_assignmentExpression_Producton_0);
        actions.Add("Rule_expressionStatement_Producton_0", Rule_expressionStatement_Producton_0);
        actions.Add("Rule_expressionStatement_Producton_1", Rule_expressionStatement_Producton_1);
    }

    public static object Rule_start_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_program_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        c_grammar_gv.root = new AstNode("program");
        c_grammar_gv.root.children.Add(_1);
        c_grammar_gv.root.Print();
        return c_grammar_gv.root.GetPrintString();

        return _0;
    }

    public static object Rule_declList_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        _0 = new AstNode("declList");
        _0.children.Add(_1);  

        return _0;
    }

    public static object Rule_declList_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 =(AstNode)objects[1];
        AstNode _2 = (AstNode)objects[2];

        // user-defined action
        _1.children.Add(_2); 
        _0 = new AstNode("declList");
        _0.children.AddRange(_1.children);

        return _0;
    }

    public static object Rule_declList_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();

        return _0;
    }

    public static object Rule_decl_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        _0 = new AstNode("decl");
        _0.children.Add(_1);

        return _0;
    }

    public static object Rule_funDecl_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];
        AstNode _2 = (AstNode)objects[2];
        AstNode _5 = (AstNode)objects[5];

        // user-defined action
        _0 = new AstNode("funDecl");
        _0.children.Add(_1);   
        _0.children.Add(_2);
        _0.children.Add(_5);

        return _0;
    }

    public static object Rule_funDecl_Producton_1(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];
        AstNode _2 = (AstNode)objects[2];
        AstNode _4 = (AstNode)objects[4];
        AstNode _6 = (AstNode)objects[6];

        // user-defined action
        _0 = new AstNode("funDecl");
        _0.children.Add(_1);   
        _0.children.Add(_2);
        _0.children.Add(_4);
        _0.children.Add(_6);

        return _0;
    }

    public static object Rule_funDecl_Producton_2(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];
        AstNode _2 = (AstNode)objects[2];
        AstNode _5 = (AstNode)objects[5];

        // user-defined action
        _0 = new AstNode("funDecl");
        _0.children.Add(_1);   
        _0.children.Add(_2);
        _0.children.Add(_5);

        return _0;
    }

    public static object Rule_paramList_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];
        AstNode _2 = (AstNode)objects[2];

        // user-defined action
        _0 = new AstNode("param");
        _0.children.Add(_1); 
        _0.children.Add(_2);

        return _0;
    }

    public static object Rule_paramList_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 =(AstNode)objects[1];
        AstNode _3 = (AstNode)objects[3];
        AstNode _4 = (AstNode)objects[4];

        // user-defined action
        _1.children.Add(_3); 
        _1.children.Add(_4);
        _0 = new AstNode("paramList");
        _0.children.AddRange(_1.children); 

        return _0;
    }

    public static object Rule_paramList_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();

        return _0;
    }

    public static object Rule_funName_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        _0 = new AstNode("funName");
        _0.children.Add(_1);

        return _0;
    }

    public static object Rule_returnStmt_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        string _1 = (string)objects[1];

        // user-defined action
        _0 = new AstNode("returnStmt");
        _0.children.Add(new AstNode("return"));

        return _0;
    }

    public static object Rule_returnStmt_Producton_1(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        string _1 = (string)objects[1];
        AstNode _2 = (AstNode)objects[2];

        // user-defined action
        _0 = new AstNode("returnStmt");
        _0.children.Add(new AstNode("return"));
        _0.children.Add(_2);

        return _0;
    }

    public static object Rule_typeSpec_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        string _1 = (string)objects[1];

        // user-defined action
        _0 = new AstNode("typeSpec");
        _0.children.Add(new AstNode("int"));  

        return _0;
    }

    public static object Rule_typeSpec_Producton_1(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        string _1 = (string)objects[1];

        // user-defined action
        _0 = new AstNode("typeSpec");
        _0.children.Add(new AstNode("void"));    

        return _0;
    }

    public static object Rule_id_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        string _1 = (string)objects[1];

        // user-defined action
        _0 = new AstNode("id");
        _0.children.Add(new AstNode(_1));

        return _0;
    }

    public static object Rule_constant_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        int _1 = (int)objects[1];

        // user-defined action
        _0 = new AstNode("constant");
        _0.children.Add(new AstNode("" + _1));

        return _0;
    }

    public static object Rule_primaryExpression_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        _0 = new AstNode("primaryExpression");
        _0.children.Add(_1);    

        return _0;
    }

    public static object Rule_primaryExpression_Producton_1(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        _0 = new AstNode("primaryExpression");
        _0.children.Add(_1);      

        return _0;
    }

    public static object Rule_mulExpression_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        _0 = new AstNode("mulExpression");
        _0.children.Add(_1);       

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 =(AstNode)objects[1];
        AstNode _3 = (AstNode)objects[3];

        // user-defined action
        _0 = new AstNode("mulExpression");
        _0.children.Add(_1); 
        _0.children.Add(new AstNode("*")); 
        _0.children.Add(_3); 

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 =(AstNode)objects[1];
        AstNode _3 = (AstNode)objects[3];

        // user-defined action
        _0 = new AstNode("mulExpression");
        _0.children.Add(_1); 
        _0.children.Add(new AstNode("/")); 
        _0.children.Add(_3); 

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_2(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 =(AstNode)objects[1];
        AstNode _3 = (AstNode)objects[3];

        // user-defined action
        _0 = new AstNode("mulExpression");
        _0.children.Add(_1); 
        _0.children.Add(new AstNode("%")); 
        _0.children.Add(_3); 

        return _0;
    }

    public static object Rule_mulExpression_LeftRecursionExpand_Producton_3(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();

        return _0;
    }

    public static object Rule_addExpression_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        _0 = new AstNode("addExpression");
        _0.children.Add(_1);     

        return _0;
    }

    public static object Rule_addExpression_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 =(AstNode)objects[1];
        AstNode _3 = (AstNode)objects[3];

        // user-defined action
        _0 = new AstNode("addExpression");
        _0.children.Add(_1); 
        _0.children.Add(new AstNode("+")); 
        _0.children.Add(_3); 

        return _0;
    }

    public static object Rule_addExpression_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 =(AstNode)objects[1];
        AstNode _3 = (AstNode)objects[3];

        // user-defined action
        _0 = new AstNode("addExpression");
        _0.children.Add(_1); 
        _0.children.Add(new AstNode("-")); 
        _0.children.Add(_3); 

        return _0;
    }

    public static object Rule_addExpression_LeftRecursionExpand_Producton_2(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();

        return _0;
    }

    public static object Rule_compoundStatement_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();

        // user-defined action
        _0 = new AstNode("compoundStatement");

        return _0;
    }

    public static object Rule_compoundStatement_Producton_1(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _2 = (AstNode)objects[2];

        // user-defined action
        _0 = new AstNode("compoundStatement");
        _0.children.Add(_2);      

        return _0;
    }

    public static object Rule_statementList_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        _0 = new AstNode("statementList");
        _0.children.Add(_1);        

        return _0;
    }

    public static object Rule_statementList_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 =(AstNode)objects[1];
        AstNode _2 = (AstNode)objects[2];

        // user-defined action
        _1.children.Add(_2); 
        _0 = new AstNode("statementList");
        _0.children.AddRange(_1.children);    

        return _0;
    }

    public static object Rule_statementList_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();

        return _0;
    }

    public static object Rule_statement_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        _0 = new AstNode("statement");
        _0.children.Add(_1);        

        return _0;
    }

    public static object Rule_statement_Producton_1(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        _0 = new AstNode("statement");
        _0.children.Add(_1);       

        return _0;
    }

    public static object Rule_statement_Producton_2(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        _0 = new AstNode("statement");
        _0.children.Add(_1);       

        return _0;
    }

    public static object Rule_statement_Producton_3(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        _0 = new AstNode("statement");
        _0.children.Add(_1);       

        return _0;
    }

    public static object Rule_assignmentExpression_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];
        AstNode _2 = (AstNode)objects[2];
        AstNode _4 = (AstNode)objects[4];

        // user-defined action
        _0 = new AstNode("assignmentExpression");
        _0.children.Add(_1);    
        _0.children.Add(_2); 
        _0.children.Add(new AstNode("=")); 
        _0.children.Add(_4); 

        return _0;
    }

    public static object Rule_expressionStatement_Producton_0(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();

        // user-defined action
        _0 = new AstNode("expressionStatement");

        return _0;
    }

    public static object Rule_expressionStatement_Producton_1(Dictionary<int, object> objects) { 
        AstNode _0 = new AstNode();
        AstNode _1 = (AstNode)objects[1];

        // user-defined action
        _0 = new AstNode("expressionStatement");
        _0.children.Add(_1);       

        return _0;
    }
}

}


//Lex Gen 
namespace c_grammarNs
{


    using LexYaccNs;
    public class LexActions
    {
        public static object value = null;

        public static Dictionary<string, Func<string, object>> actions = new Dictionary<string, Func<string, object>>();

        public static Dictionary<int, string> tokenDict = new Dictionary<int, string>
        {
            { 256, "CONSTANT"},
            { 257, "VOID"},
            { 258, "INT"},
            { 259, "ID"},
            { 260, "RETURN"},
        };

        public static int CONSTANT = 256;
        public static int VOID = 257;
        public static int INT = 258;
        public static int ID = 259;
        public static int RETURN = 260;

        public static void CallAction(List<Terminal> tokens, LexRule rule)
        {
            Init();
            object ret = actions[rule.ruleName](rule.yytext);
            if (ret is int && (int)ret == 0)
            {

            }
            else if (ret is char)
            {
                tokens.Add(Terminal.BuildConstCharTerminal((char)ret));
            }
            else
            {
                tokens.Add(Terminal.BuildToken(tokenDict[(int)ret], LexActions.value));
            }
        }

        public static string ruleInput = @"%{
%}

%%
""void"" { return VOID; }
""int"" { return INT; }
(\-)?[0-9]+ { 
            value = int.Parse(yytext);
            return CONSTANT;
}
""return"" { return RETURN; }
[_a-zA-Z][a-zA-Z0-9]* { value = yytext; return ID; }
[ \t\n\r]+   {}
""{""  { return '{'; }
""}""  { return '}'; }
""(""  { return '('; }
"")""  { return ')'; }
"";""  { return ';'; }
""+""  {return '+';}
""-""  {return '-';}
""*""  {return '*';}
""/""  {return '/';}
""=""  { return '='; }
""%""  { return '%'; }
"",""  { return ','; }
%%";


        public static void Init()
        {
            if (actions.Count != 0)
                return;
            actions.Add("LexRule0", LexAction0);
            actions.Add("LexRule1", LexAction1);
            actions.Add("LexRule2", LexAction2);
            actions.Add("LexRule3", LexAction3);
            actions.Add("LexRule4", LexAction4);
            actions.Add("LexRule5", LexAction5);
            actions.Add("LexRule6", LexAction6);
            actions.Add("LexRule7", LexAction7);
            actions.Add("LexRule8", LexAction8);
            actions.Add("LexRule9", LexAction9);
            actions.Add("LexRule10", LexAction10);
            actions.Add("LexRule11", LexAction11);
            actions.Add("LexRule12", LexAction12);
            actions.Add("LexRule13", LexAction13);
            actions.Add("LexRule14", LexAction14);
            actions.Add("LexRule15", LexAction15);
            actions.Add("LexRule16", LexAction16);
            actions.Add("LexRule17", LexAction17);
        }
        public static object LexAction0(string yytext)
        {
            value = null;

            // user-defined action
            return VOID; 

            return 0;
        }
        public static object LexAction1(string yytext)
        {
            value = null;

            // user-defined action
            return INT; 

            return 0;
        }
        public static object LexAction2(string yytext)
        {
            value = null;

            // user-defined action
            value = int.Parse(yytext);
            return CONSTANT;

            return 0;
        }
        public static object LexAction3(string yytext)
        {
            value = null;

            // user-defined action
            return RETURN; 

            return 0;
        }
        public static object LexAction4(string yytext)
        {
            value = null;

            // user-defined action
            value = yytext; return ID; 

            return 0;
        }
        public static object LexAction5(string yytext)
        {
            value = null;

            return 0;
        }
        public static object LexAction6(string yytext)
        {
            value = null;

            // user-defined action
            return '{'; 

            return 0;
        }
        public static object LexAction7(string yytext)
        {
            value = null;

            // user-defined action
            return '}'; 

            return 0;
        }
        public static object LexAction8(string yytext)
        {
            value = null;

            // user-defined action
            return '('; 

            return 0;
        }
        public static object LexAction9(string yytext)
        {
            value = null;

            // user-defined action
            return ')'; 

            return 0;
        }
        public static object LexAction10(string yytext)
        {
            value = null;

            // user-defined action
            return ';'; 

            return 0;
        }
        public static object LexAction11(string yytext)
        {
            value = null;

            // user-defined action
            return '+';

            return 0;
        }
        public static object LexAction12(string yytext)
        {
            value = null;

            // user-defined action
            return '-';

            return 0;
        }
        public static object LexAction13(string yytext)
        {
            value = null;

            // user-defined action
            return '*';

            return 0;
        }
        public static object LexAction14(string yytext)
        {
            value = null;

            // user-defined action
            return '/';

            return 0;
        }
        public static object LexAction15(string yytext)
        {
            value = null;

            // user-defined action
            return '='; 

            return 0;
        }
        public static object LexAction16(string yytext)
        {
            value = null;

            // user-defined action
            return '%'; 

            return 0;
        }
        public static object LexAction17(string yytext)
        {
            value = null;

            // user-defined action
            return ','; 

            return 0;
        }
    }
}


//Src files Gen
namespace c_grammarNs{

namespace LexYaccNs
{
    using RegexNs;
    using System.Collections.Generic;

    public class Lex
    {
        public delegate void CallActionDelegate(List<Terminal> tokens, LexRule rule);

        public static List<Terminal> Parse(string input, string ruleInput, CallActionDelegate actionFunction)
        {
            List<Terminal> tokens = new List<Terminal>();

            List<LexRule> rules;
            Section section;
            LexRuleReader.Parse(ruleInput, out section, out rules);

            int start = 0;

            // find the longest match rule first.
            // If there are multiple rules match the same length, selec the first one
            while (start < input.Length)
            {
                LexRule matchedRule = null;

                for (int i = 0; i < rules.Count; i++)
                {
                    LexRule rule = rules[i];
                    rule.yytext = "";

                    if (rule.plainText != null)
                    {
                        if (input.Substring(start).StartsWith(rule.plainText))
                        {
                            rule.yytext = rule.plainText;
                            if (matchedRule == null)
                                matchedRule = rule;
                            else
                            {
                                if (rule.yytext.Length > matchedRule.yytext.Length)
                                    matchedRule = rule;
                            }
                        }
                    }
                    else
                    {
                        int prevAccept = -1;
                        RecognizeParam param = rule.nfa.CreateRecognizeParam();

                        for (int j = 0; start + j < input.Length; j++)
                        {
                            RecognizeResult result = rule.nfa.StepRecognize(input[start + j], param);
                            if (result == RecognizeResult.AliveAndAccept)
                                prevAccept = j;
                            else if (result == RecognizeResult.EndAndReject)
                                break;
                        }

                        if (prevAccept != -1)
                        {
                            rule.yytext = input.Substring(start, prevAccept + 1);

                            if (matchedRule == null)
                                matchedRule = rule;
                            else
                            {
                                if (rule.yytext.Length > matchedRule.yytext.Length)
                                    matchedRule = rule;
                            }
                        }
                    }
                }

                if (matchedRule == null)
                {
                    throw new Exception("Syntax Error, at pos " + start);
                }
                else
                {
                    actionFunction(tokens, matchedRule);
                    start += matchedRule.yytext.Length;
                }
            }

            return tokens;
        }
    }
}
}

namespace c_grammarNs{

using System.Text;

namespace LexYaccNs
{
    public class LexCodeGen
    {
        public static string classDef = @"
    using LexYaccNs;
    public class LexActions
    {
        public static object value = null;

        public static Dictionary<string, Func<string, object>> actions = new Dictionary<string, Func<string, object>>();
";

        public static string callActionFunctionDef = @"
        public static void CallAction(List<Terminal> tokens, LexRule rule)
        {
            Init();
            object ret = actions[rule.ruleName](rule.yytext);
            if (ret is int && (int)ret == 0)
            {

            }
            else if (ret is char)
            {
                tokens.Add(Terminal.BuildConstCharTerminal((char)ret));
            }
            else
            {
                tokens.Add(Terminal.BuildToken(tokenDict[(int)ret], LexActions.value));
            }
        }
";

        public static string GenCode(string input, string namespaceStr, List<LexTokenDef> lexTokenDef)
        {
            List<LexRule> rules;
            Section section;
            LexRuleReader.Parse(input, out section, out rules);

            StringBuilder sb = new StringBuilder();
            string indent1 = "    ";
            string indent2 = "        ";
            string indent3 = "            ";

            sb.AppendLine("//Lex Gen ");
            sb.AppendLine("namespace " + namespaceStr);
            sb.AppendLine("{");
            sb.AppendLine(section.definitionSection);
            sb.AppendLine(classDef);

            sb.AppendLine(indent2 + "public static Dictionary<int, string> tokenDict = new Dictionary<int, string>");
            sb.AppendLine(indent2 + "{");
            foreach (LexTokenDef lexToken in lexTokenDef)
                sb.AppendLine(indent3 + "{ " + lexToken.index + ", \"" + lexToken.name + "\"},");
            sb.AppendLine(indent2 + "};");
            sb.AppendLine();

            foreach (LexTokenDef lexToken in lexTokenDef)
                sb.AppendLine(indent2 + "public static int " + lexToken.name + " = " + lexToken.index + ";");

            sb.AppendLine(callActionFunctionDef);

            sb.AppendLine(indent2 + "public static string ruleInput = @\"" + input.Replace("\"", "\"\"") + "\";\n");
            sb.AppendLine();
            sb.AppendLine(indent2 + "public static void Init()");
            sb.AppendLine(indent2 + "{");
            sb.AppendLine(indent2 + "    if (actions.Count != 0)");
            sb.AppendLine(indent2 + "        return;");

            for (int i = 0; i < rules.Count; i++)
            {
                LexRule rule = rules[i];
                sb.AppendLine(indent3 + "actions.Add(\"LexRule" + i + "\", LexAction" + i + ");");
            }
            sb.AppendLine(indent2 + "}");


            for (int i = 0; i < rules.Count; i++)
            {
                LexRule rule = rules[i];
                sb.AppendLine(indent2 + "public static object LexAction" + i + "(string yytext)");
                sb.AppendLine(indent2 + "{");
                sb.AppendLine(indent2 + "    value = null;");
                sb.AppendLine();

                if (rule.action.Trim().Length != 0)
                {
                    sb.AppendLine(indent3 + "// user-defined action");
                    sb.AppendLine(LexYaccUtil.FixGenCodeIndention(rule.action, indent3));
                }

                sb.AppendLine(indent2 + "    return 0;");
                sb.AppendLine(indent2 + "}");
            }

            sb.AppendLine(indent1 + "}");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine();

            Console.Write(sb.ToString());

            return sb.ToString();
        }

        public static void GenCode(string ruleSection, string name, List<LexTokenDef> lexTokenDef, string outputFolder, bool append)
        {
            string code = GenCode(ruleSection, name + "Ns", lexTokenDef);
            if (append)
                File.AppendAllText(Path.Combine(outputFolder, name + ".cs"), code);
            else
                File.WriteAllText(Path.Combine(outputFolder, name + ".cs"), code);
        }
    }
}

}

namespace c_grammarNs{

using RegexNs;

namespace LexYaccNs
{
    public class LexRule
    {
        public LexRule(NFA nfa, string ruleName, string action)
        {
            this.nfa = nfa;
            this.ruleName = ruleName;
            this.action = action;
        }

        public LexRule(string plainText, string ruleName, string action)
        {
            this.plainText = plainText;
            this.ruleName = ruleName;
            this.action = action;
        }

        public string ruleName;
        public NFA nfa;
        public string plainText;
        public string action;

        public string yytext;
    }
}

}

namespace c_grammarNs{

using RegexNs;

namespace LexYaccNs
{
    public class LexRuleReader
    {
        public static void Parse(string input, out Section sections, out List<LexRule> rules)
        {
            rules = new List<LexRule>();

            sections = YaccRuleReader.SplitSecction(input, false);
            string ruleSectionString = sections.ruleSection.Trim();

            while (ruleSectionString.Length > 0)
            {
                int leftBracket = LexYaccUtil.FindCharNotInLiteral(ruleSectionString, '{', false);
                string regex = ruleSectionString.Substring(0, leftBracket).Trim();

                // the case } in action:
                // "}"  { return '}'; }
                ruleSectionString = ruleSectionString.Substring(leftBracket + 1);
                int rightBracket = LexYaccUtil.FindCharNotInLiteral(ruleSectionString, '}', true);
                string action = LexYaccUtil.RemoveHeadAndTailEmptyLine(ruleSectionString.Substring(0, rightBracket));

                ruleSectionString = ruleSectionString.Substring(rightBracket + 1).Trim();

                if (regex.StartsWith("\""))
                    rules.Add(new LexRule(regex.Substring(1, regex.Length - 2), "LexRule" + rules.Count, action));
                else
                    rules.Add(new LexRule(Regex.Compile(regex), "LexRule" + rules.Count, action));
            }
        }
    }
}

}

namespace c_grammarNs{

namespace LexYaccNs
{

    public class LexYacc
    {
        public static object Parse(string input, string lexRule, string yaccRule, Lex.CallActionDelegate lexCallActionDelegate, Yacc.CallActionDelegate yaccActionDelegate)
        {
            List<Terminal> tokens = Lex.Parse(input, lexRule, lexCallActionDelegate);

            Yacc yacc = new Yacc(yaccRule);
            bool result = yacc.Feed(tokens);

            if (!result)
                return "syntax error";

            return yacc.route.startDFA.CallAction(yaccActionDelegate);
        }
    }

}
}

namespace c_grammarNs{

using System.Text;

namespace LexYaccNs
{
    public class LexYaccCodeGen
    {
        public static void GenCode(string lexFile, string yaccFile, string outputFolder, string name)
        {
            string outputFile = Path.Combine(outputFolder, name + ".cs");

            StringBuilder sb = new StringBuilder();

            // LexYacc Gen
            sb.AppendLine("//LexYacc Gen");
            sb.AppendLine("public class " + name);
            sb.AppendLine("{");
            sb.AppendLine("    public static object Parse(string input)");
            sb.AppendLine("    {");
            // pairNs.LexActions.ruleInput, pairNs.YaccActions.ruleInput, 
            sb.AppendLine("        return " + name + "Ns.LexYaccNs.LexYacc.Parse(input, " + name + "Ns.LexActions.ruleInput, " + name + "Ns.YaccActions.ruleInput, " +
                name + "Ns.LexActions.CallAction, " + name + "Ns.YaccActions.CallAction);");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            File.WriteAllText(outputFile, sb.ToString());
            sb.AppendLine("");

            // Yacc Gen
            List<LexTokenDef> lexTokenDef = YaccCodeGen.GenCode(File.ReadAllText(yaccFile), name, outputFolder, true);

            // Lex Gen
            LexCodeGen.GenCode(File.ReadAllText(lexFile), name, lexTokenDef, outputFolder, true);

            // Src files Gen
            File.AppendAllText(outputFile, "//Src files Gen");
            string[] allFiles = Directory.GetFiles("../../../LexYaccNs", "*", SearchOption.AllDirectories);
            foreach (string file in allFiles)
            {
                if (file.EndsWith(".cs"))
                    AppendFile(outputFile, file, name + "Ns");
            }

            allFiles = Directory.GetFiles("../../../RegexNs", "*", SearchOption.AllDirectories);
            foreach (string file in allFiles)
            {
                if (file.EndsWith(".cs"))
                    AppendFile(outputFile, file, name + "Ns");
            }
        }

        private static void AppendFile(string outputFile, string file, string ns)
        {
            File.AppendAllText(outputFile, "\nnamespace " + ns + "{\n\n");
            File.AppendAllText(outputFile, File.ReadAllText(file));
            File.AppendAllText(outputFile, "\n}\n");
        }
    }
}

}

namespace c_grammarNs{

using System.Text;

namespace LexYaccNs
{
    public class LexYaccUtil
    {
        public static int FindCharNotInLiteral(string s, char c, bool includeSingleQuote)
        {
            return FindCharNotInLiteral(s, new List<char>() { c }, includeSingleQuote);
        }

        public static int FindCharNotInLiteral(string s, List<char> chars, bool includeSingleQuote)
        {
            List<string> stringList = new List<string>();

            foreach (char c in chars)
                stringList.Add(c.ToString());

            return FindStringNotInLiteral(s, stringList, includeSingleQuote);
        }

        public static int FindStringNotInLiteral(string s, string s2, bool includeSingleQuote)
        {
            return FindStringNotInLiteral(s, new List<string>() { s2 }, includeSingleQuote);
        }

        public static int FindStringNotInLiteral(string s, List<string> strings, bool includeSingleQuote)
        {
            bool singleQuote = false;
            bool doubleQuote = false;
            bool comment = false;

            for (int i = 0; i < s.Length; i++)
            {
                if (includeSingleQuote && s[i] == '\'')
                    singleQuote = !singleQuote;
                else if (s[i] == '"')
                    doubleQuote = !doubleQuote;
                else if (s[i] == '\n')
                    comment = false;
                else if (s[i] == '/' && i < s.Length - 1 && s[i + 1] == '/')
                    comment = true;

                if (!singleQuote && !doubleQuote && !comment)
                {
                    foreach (string s2 in strings)
                    {
                        if (i + s2.Length > s.Length)
                            continue;

                        bool found = true;
                        for (int j = 0; j < s2.Length; j++)
                        {
                            if (s[i + j] != s2[j])
                            {
                                found = false;
                                break;
                            }
                        }

                        if (found)
                            return i;
                    }
                }

            }

            return -1;
        }

        public static string GetGenFileFolder()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "../../../GenFile");
        }

        public static string FixGenCodeIndention(string input, string indention)
        {
            StringBuilder sb = new StringBuilder();

            int minIndentCount = int.MaxValue;
            using (StringReader reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int indentCount = 0;
                    for (; indentCount < line.Length; indentCount++)
                        if (line[indentCount] != ' ')
                            break;
                    if (indentCount < minIndentCount)
                        minIndentCount = indentCount;
                }
            }
            using (StringReader reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    sb.AppendLine(indention + line.Substring(minIndentCount));
                }
            }

            return sb.ToString();
        }

        public static string RemoveHeadAndTailEmptyLine(string input)
        {
            string[] lines = input.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            StringBuilder sb = new StringBuilder();

            int start = 0;
            for (; start < lines.Length && lines[start].Trim().Length == 0; start++)
                ;

            int end = lines.Length - 1;
            for (; end >= 0 && lines[end].Trim().Length == 0; end--)
                ;

            for (int i = start; i < lines.Length && i <= end; i++)
                sb.Append(lines[i] + "\n");

            return sb.ToString();
        }
    }



    public class SectionSplitter
    {

    }
}
}

namespace c_grammarNs{

namespace LexYaccNs
{
    public class LexTokenDef
    {
        public string name;
        public string type;
        public int index;
    }

    public enum Result
    {
        Alive,
        Accepted,
        Rejected,
    }

    public class Route
    {
        public DFA startDFA = null;
        public int lexTokenIndex = -1;
        public Stack<DFA> dfaStack = new Stack<DFA>();
        public Result result = Result.Alive;
    }

    public class Yacc
    {
        public string input = "";
        public Section sections = null;
        public List<YaccRule> productionRules = null;
        public List<LexTokenDef> lexTokenDef = null;
        public Dictionary<string, string> ruleNonterminalType = null;
        public List<Terminal> lexTokens = new List<Terminal>();

        public List<Route> routes = new List<Route>();
        public Route route = null;

        public delegate object CallActionDelegate(string functionName, Dictionary<int, object> param);

        public Yacc()
        {

        }

        public Yacc(string input)
        {
            this.input = input;
            YaccRuleReader.Parse(input, out sections, out productionRules, out lexTokenDef, out ruleNonterminalType);
            Rebuild();
        }

        public void Rebuild()
        {
            Route route = new Route();
            route.startDFA = new DFA(this, productionRules[0].productions[0]);
            route.dfaStack.Push(route.startDFA);
            route.lexTokenIndex = 0;
            route.result = Result.Alive;

            this.route = route;
            routes.Clear();
            routes.Add(route);
        }

        public Route CloneRoute(int lexTokenIndex)
        {
            Dictionary<DFA, DFA> oldDFAtoNewDFAMapping = new Dictionary<DFA, DFA>();

            DFA newStartDFA = route.startDFA.clone(oldDFAtoNewDFAMapping);
            Route newRoute = new Route();
            newRoute.startDFA = newStartDFA;
            newRoute.lexTokenIndex = lexTokenIndex;
            newRoute.result = Result.Alive;

            // restore stack
            Stack<DFA> reverse = new Stack<DFA>(route.dfaStack);
            while (reverse.Count > 0)
            {
                DFA oldDFA = reverse.Pop();
                newRoute.dfaStack.Push(oldDFAtoNewDFAMapping[oldDFA]);
            }

            return newRoute;
        }

        public void ExpandNonterminal(int lexTokenIndex)
        {
            DFA dfa = route.dfaStack.Peek();
            while (dfa.states[dfa.currentState].symbol is Nonterminal)
            {
                if (dfa.states[dfa.currentState].nonterminalDFA == null)
                {
                    Nonterminal nt = (Nonterminal)dfa.states[dfa.currentState].symbol;
                    List<Production> productions = GetProductions(nt.name);

                    // create new route
                    for (int i = 1; i < productions.Count; i++)
                    {
                        Route newRoute = CloneRoute(lexTokenIndex);

                        DFA newDFA = new DFA(this, productions[i]);
                        newRoute.dfaStack.Peek().states[dfa.currentState].nonterminalDFA = newDFA;
                        newRoute.dfaStack.Push(newDFA);

                        routes.Add(newRoute);
                    }

                    DFA newDFA2 = new DFA(this, productions[0]);
                    dfa.states[dfa.currentState].nonterminalDFA = newDFA2;
                    route.dfaStack.Push(newDFA2);
                }
                dfa = dfa.states[dfa.currentState].nonterminalDFA;
            }
        }

        public void ExpandAndFeedEmpty(int lexTokenIndex)
        {
            bool continueFeed = true;

            while (continueFeed)
            {
                if (route.dfaStack.Count == 0)
                    break;

                ExpandNonterminal(lexTokenIndex);

                DFA dfa = route.dfaStack.Peek();
                continueFeed = false;

                while (true)
                {
                    if (dfa.production.IsEmptyProduction())
                    {
                        route.dfaStack.Peek().Feed(this, lexTokenIndex, true);
                        continueFeed = true;
                        break;
                    }

                    if (dfa.states[dfa.currentState].nonterminalDFA != null)
                        dfa = dfa.states[dfa.currentState].nonterminalDFA;
                    else
                        break;
                }
            }
        }

        public bool FeedInternal()
        {
            ExpandAndFeedEmpty(route.lexTokenIndex);

            for (; route.lexTokenIndex < lexTokens.Count; route.lexTokenIndex++)
            {
                if (route.dfaStack.Count == 0)
                    return false;

                route.dfaStack.Peek().Feed(this, route.lexTokenIndex, false);
                if (route.result == Result.Rejected)
                    return false;

                ExpandAndFeedEmpty(route.lexTokenIndex + 1);
            }

            if (route.result == Result.Accepted)
                return true;
            else
                return false;
        }

        public bool Feed(List<Terminal> lexTokens)
        {
            this.lexTokens = lexTokens;

            while (routes.Count > 0)
            {
                route = routes[routes.Count - 1];
                routes.RemoveAt(routes.Count - 1);
                bool result = FeedInternal();
                if (result == true)
                    return true;
            }

            return false;
        }

        public void AdvanceToNextState()
        {
            route.dfaStack.Pop();
            if (route.dfaStack.Count == 0)
            {
                route.result = Result.Accepted;
                return;
            }

            DFA dfa = route.dfaStack.Peek();
            dfa.currentState++;
            if (dfa.currentState == dfa.acceptedState)
                AdvanceToNextState();
        }

        public List<Production> GetProductions(string name)
        {
            foreach (YaccRule pr in productionRules)
                if (pr.lhs.name == name)
                    return pr.productions;
            return null;
        }

        public override string ToString()
        {
            string ret = "";

            foreach (YaccRule pr in productionRules)
            {
                foreach (Production p in pr.productions)
                {
                    ret += p.ToString() + ";\n";
                }
            }

            return ret;
        }
    }



}
}

namespace c_grammarNs{

using System.Text;

namespace LexYaccNs
{
    public class YaccCodeGen
    {
        public static List<LexTokenDef> GenCode(string input, string name, string outputFolder, bool append)
        {
            Section sections;
            List<YaccRule> productionRules;
            List<LexTokenDef> lexTokenDef;
            Dictionary<string, string> ruleNonterminalType;

            YaccRuleReader.Parse(input, out sections, out productionRules, out lexTokenDef, out ruleNonterminalType);

            string code = GenCode(input, sections, productionRules, lexTokenDef, ruleNonterminalType, name + "Ns");
            if (append)
                File.AppendAllText(Path.Combine(outputFolder, name + ".cs"), code);
            else
                File.WriteAllText(Path.Combine(outputFolder, name + ".cs"), code);

            return lexTokenDef;
        }

        public static string GenCode(string ruleInput, Section sections, List<YaccRule> productionRules, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType, string namespaceStr)
        {
            string indent1 = "    ";
            string indent2 = "        ";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("//Yacc Gen ");
            sb.AppendLine("namespace " + namespaceStr);
            sb.AppendLine("{");
            sb.AppendLine();

            sb.AppendLine(sections.definitionSection);
            sb.AppendLine();

            sb.AppendLine("public class YaccActions{");
            sb.AppendLine();
            sb.AppendLine(indent1 + "public static Dictionary<string, Func<Dictionary<int, object>, object>> actions = new Dictionary<string, Func<Dictionary<int, object>, object>>();");
            sb.AppendLine();
            sb.AppendLine(indent1 + "public static string ruleInput = @\"" + ruleInput.Replace("\"", "\"\"") + "\";\n");
            sb.AppendLine();
            sb.AppendLine(indent1 + "public static object CallAction(string functionName, Dictionary<int, object> param)");
            sb.AppendLine(indent1 + "{");
            sb.AppendLine(indent1 + "    Init();");
            sb.AppendLine(indent1 + "    if (!actions.ContainsKey(functionName))");
            sb.AppendLine(indent1 + "        return null;");
            sb.AppendLine(indent1 + "    return actions[functionName](param);");
            sb.AppendLine(indent1 + "}");
            sb.AppendLine();
            sb.AppendLine(indent1 + "public static void Init()");
            sb.AppendLine(indent1 + "{");
            sb.AppendLine(indent2 + "if (actions.Count != 0)");
            sb.AppendLine(indent2 + "    return;");
            sb.AppendLine();

            foreach (YaccRule rule in productionRules)
            {
                foreach (Production p in rule.productions)
                {
                    if (p.action == null)
                        continue;
                    string functionName = string.Format("Rule_{0}_Producton_{1}", p.lhs.name, p.index);
                    sb.AppendLine(indent2 + string.Format("actions.Add(\"{0}\", {1});", functionName, functionName));
                }
            }
            sb.AppendLine(indent1 + "}");

            foreach (YaccRule rule in productionRules)
            {
                foreach (Production p in rule.productions)
                {
                    if (p.action != null)
                    {
                        sb.AppendLine();
                        sb.AppendLine(indent1 + string.Format("public static object Rule_{0}_Producton_{1}(Dictionary<int, object> objects) {{ ", p.lhs.name, p.index));

                        string type = ruleNonterminalType[rule.lhs.name];

                        sb.Append(indent2 + string.Format("{0} {1}", type, "_0 = new " + type));
                        if (type == "string" || type == "String")
                            sb.AppendLine("(\"\");");
                        else
                            sb.AppendLine("();");

                        if (p.IsEmptyProduction())
                        {

                        }
                        else
                        {
                            if (p.type == ProductionType.LeftRecursiveSecond)
                                sb.AppendLine(indent2 + string.Format("{0} {1} =({2})objects[1]", type, "_1", type) + ";");

                            for (int i = 0; i < p.symbols.Count - (p.type == ProductionType.Plain ? 0 : 1); i++)
                            {
                                Symbol symbol = p.symbols[i];
                                string typeName = "";


                                if (symbol is Terminal)
                                {
                                    Terminal t = (Terminal)symbol;
                                    if (t.type == TerminalType.CONSTANT_CHAR || t.type == TerminalType.EMPTY)
                                        continue;

                                    typeName = "";
                                    foreach (LexTokenDef l in lexTokenDef)
                                    {
                                        if (l.name == t.tokenName)
                                            typeName = l.type;
                                    }
                                }
                                else
                                {
                                    Nonterminal nt = (Nonterminal)symbol;
                                    typeName = ruleNonterminalType[nt.name];
                                }

                                int shift = p.type == ProductionType.LeftRecursiveSecond ? 1 : 0;
                                sb.AppendLine(indent2 + string.Format("{0} {1} = ({2})objects[{3}]", typeName, "_" + (i + 1 + shift).ToString(), typeName, i + 1 + shift) + ";");

                            }
                        }

                        sb.AppendLine();

                        if (p.action.Trim().Length != 0)
                        {
                            sb.AppendLine(indent2 + "// user-defined action");
                            sb.AppendLine(LexYaccUtil.FixGenCodeIndention(p.action, indent2));
                        }

                        sb.AppendLine(indent2 + "return _0;");
                        sb.AppendLine(indent1 + "}");
                    }
                }
            }

            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }
    }
}

}

namespace c_grammarNs{

namespace LexYaccNs
{

    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class State
    {
        public DFA nonterminalDFA = null;
        public Symbol symbol;

        public State()
        { }

        public State(Symbol s)
        {
            this.symbol = s;
        }
    }

    public class DFA
    {
        public int startState = 0;
        public int acceptedState = -1;
        public int currentState = 0;

        public List<State> states = new List<State>();

        public Dictionary<int, object> tokenObjects = new Dictionary<int, object>();
        Dictionary<int, object> param = new Dictionary<int, object>();

        public Production production = null;
        public Yacc yacc = null;

        public DFA()
        {
        }

        public DFA(Yacc yacc, Production p)
        {
            production = p;
            this.yacc = yacc;

            if (p.symbols.Count == 0)
            {
                // empty production
                startState = states.Count;
                states.Add(new State());

                acceptedState = states.Count;
                states.Add(new State());

                states[startState].symbol = Terminal.BuildEmptyTerminal();

                currentState = startState;
            }
            else
            {
                foreach (Symbol s in p.symbols)
                    states.Add(new State(s));

                acceptedState = states.Count;
                states.Add(new State());
                startState = 0;
                currentState = 0;
            }
        }

        public void Feed(Yacc yacc, int lexTokenIndex, bool empty)
        {
            if (states[currentState].symbol is Nonterminal)
            {
                if (yacc.route.dfaStack.Count == 0 || yacc.route.dfaStack.Peek() != states[currentState].nonterminalDFA)
                    yacc.route.dfaStack.Push(states[currentState].nonterminalDFA);

                yacc.route.dfaStack.Peek().Feed(yacc, lexTokenIndex, empty);
            }
            else
            {
                Terminal lexToken = null;
                if (!empty)
                    lexToken = yacc.lexTokens[lexTokenIndex];
                else
                    lexToken = Terminal.BuildEmptyTerminal();

                if (production.IsEmptyProduction())
                {
                    // empty
                    if (lexToken.type == TerminalType.EMPTY)
                    {
                        yacc.AdvanceToNextState();
                        return;
                    }
                    else
                    {
                        yacc.route.result = Result.Rejected;
                        return;
                    }
                }
                else
                {
                    if (lexToken.type == TerminalType.EMPTY)
                    {

                    }
                    else
                    {
                        Terminal stateTerminal = (Terminal)states[currentState].symbol;

                        if (stateTerminal.type == lexToken.type)
                        {
                            if (stateTerminal.type == TerminalType.CONSTANT_CHAR)
                            {
                                if (stateTerminal.constCharValue == lexToken.constCharValue)
                                {
                                    currentState++;
                                    if (currentState == acceptedState)
                                    {
                                        yacc.AdvanceToNextState();
                                        return;
                                    }
                                }
                                else
                                {
                                    yacc.route.result = Result.Rejected;
                                    return;
                                }
                            }
                            else if (stateTerminal.type == TerminalType.TOKEN)
                            {
                                if (stateTerminal.tokenName == lexToken.tokenName)
                                {
                                    tokenObjects[currentState] = lexToken.tokenObject;

                                    currentState++;
                                    if (currentState == acceptedState)
                                    {
                                        yacc.AdvanceToNextState();
                                        return;
                                    }
                                }
                                else
                                {
                                    yacc.route.result = Result.Rejected;
                                    return;
                                }
                            }
                            else
                            {
                                Trace.Assert(false);
                            }
                        }
                        else
                        {
                            yacc.route.result = Result.Rejected;
                            return;
                        }
                    }
                }
            }
        }

        public object CallPlainAction(Yacc.CallActionDelegate invokeFunction)
        {
            if (production.IsEmptyProduction())
                return param[1];

            for (int i = 0; i < production.symbols.Count; i++)
            {
                Symbol symbol = production.symbols[i];

                if (symbol is Terminal)
                {
                    Terminal t = (Terminal)symbol;
                    if (t.type == TerminalType.CONSTANT_CHAR || t.type == TerminalType.EMPTY)
                        continue;

                    if (production.type == ProductionType.LeftRecursiveSecond)
                        param[i + 2] = tokenObjects[i];
                    else
                        param[i + 1] = tokenObjects[i];
                }
                else
                {
                    if (production.type == ProductionType.LeftRecursiveSecond)
                        param[i + 2] = states[i].nonterminalDFA.CallAction(invokeFunction);
                    else
                        param[i + 1] = states[i].nonterminalDFA.CallAction(invokeFunction);
                }
            }

            return invokeFunction(production.GetFunctionName(), param);
        }

        public object CallLeftRecursionAction(Yacc.CallActionDelegate invokeFunction)
        {
            if (production.IsEmptyProduction())
                return param[1];

            for (int i = 0; i < production.symbols.Count - 1; i++)
            {
                Symbol symbol = production.symbols[i];

                if (symbol is Terminal)
                {
                    Terminal t = (Terminal)symbol;
                    if (t.type == TerminalType.CONSTANT_CHAR)
                        continue;

                    if (production.type == ProductionType.LeftRecursiveSecond)
                        param[i + 2] = tokenObjects[i];
                    else
                        param[i + 1] = tokenObjects[i];
                }
                else
                {
                    if (production.type == ProductionType.LeftRecursiveSecond)
                        param[i + 2] = states[i].nonterminalDFA.CallAction(invokeFunction);
                    else
                        param[i + 1] = states[i].nonterminalDFA.CallAction(invokeFunction);
                }
            }

            object o = invokeFunction(production.GetFunctionName(), param);
            states[production.symbols.Count - 1].nonterminalDFA.param[1] = o;

            return states[production.symbols.Count - 1].nonterminalDFA.CallAction(invokeFunction);
        }

        public object CallAction(Yacc.CallActionDelegate invokeFunction)
        {
            if (production.type == ProductionType.Plain)
                return CallPlainAction(invokeFunction);
            else
                return CallLeftRecursionAction(invokeFunction);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(this);
        }

        public DFA clone(Dictionary<DFA, DFA> oldDFAtoNewDFAMapping)
        {
            DFA newDFA = new DFA();
            newDFA.startState = this.startState;
            newDFA.acceptedState = this.acceptedState;
            newDFA.currentState = this.currentState;

            foreach (var tokenObject in tokenObjects)
                newDFA.tokenObjects.Add(tokenObject.Key, tokenObject.Value);
            foreach (var p in param)
                newDFA.param.Add(p.Key, p.Value);

            newDFA.production = this.production;
            newDFA.yacc = this.yacc;

            oldDFAtoNewDFAMapping.Add(this, newDFA);

            for (int i = 0; i < this.states.Count; i++)
            {
                newDFA.states.Add(new State(states[i].symbol));

                State s = this.states[i];
                if (s.nonterminalDFA != null)
                {
                    newDFA.states[i].nonterminalDFA = s.nonterminalDFA.clone(oldDFAtoNewDFAMapping);
                }
            }

            return newDFA;
        }
    }

}
}

namespace c_grammarNs{

namespace LexYaccNs
{

    /*
    Term:

    A: 'B' c | 'D' e

    Production rule = A: 'B' c | 'D' e
    Production body = 'B' c | 'D' e
    Production = 'B' c
    Production = 'D' e
    */

    public class YaccRule
    {
        public YaccRule()
        {

        }

        public YaccRule(Nonterminal lhs, List<Production> productions)
        {
            this.lhs = lhs;
            this.productions = productions;
        }

        public Nonterminal lhs;
        public List<Production> productions = new List<Production>();

        public override string ToString()
        {
            string ret = "";

            foreach (Production p in productions)
                ret += p.ToString() + "\n";

            return ret;
        }
    }

    public enum ProductionType
    {
        Plain,
        LeftRecursiveFirst,
        LeftRecursiveSecond,
    }

    public class Production
    {
        public List<Symbol> symbols = null;
        public string? action = null;
        public Nonterminal lhs = null;
        public int index = -1;
        public List<LexTokenDef> lexTokenDef;
        public Dictionary<string, string> ruleNonterminalType;
        public ProductionType type = ProductionType.Plain;

        public Production(Nonterminal lhs, List<Symbol> symbols, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType, string action, ProductionType type)
        {
            this.lhs = lhs;
            this.symbols = symbols;
            this.lexTokenDef = lexTokenDef;
            this.ruleNonterminalType = ruleNonterminalType;
            this.action = action;
            this.type = type;
        }

        public Production Clone()
        {
            List<Symbol> cloneSymbols = new List<Symbol>();
            foreach (Symbol s in symbols)
                cloneSymbols.Add(s.Clone());

            Production p = new Production(lhs, cloneSymbols, lexTokenDef, ruleNonterminalType, action, type);
            p.index = index;
            return p;
        }

        public string GetFunctionName()
        {
            return string.Format("Rule_{0}_Producton_{1}", lhs.name, index);
        }

        public bool IsEmptyProduction()
        {
            return symbols.Count == 1 && symbols[0] is Terminal && ((Terminal)symbols[0]).type == TerminalType.EMPTY;
        }

        public override string ToString()
        {
            string s = lhs.name + ":";

            foreach (Symbol symbol in symbols)
            {
                if (symbol is Terminal)
                {
                    Terminal t = (Terminal)symbol;
                    if (t.type == TerminalType.TOKEN)
                        s += " " + t.tokenName;
                    else if (t.type == TerminalType.CONSTANT_CHAR)
                        s += " '" + t.constCharValue + "'";
                }
                else if (symbol is Nonterminal)
                {
                    Nonterminal n = (Nonterminal)symbol;
                    s += " " + n.name;
                }
            }

            if (action != null)
                s += " {" + action + "}";

            return s;
        }
    }


    public interface Symbol
    {
        public Symbol Clone();
    }

    public class Nonterminal : Symbol
    {
        public string name;

        public Nonterminal(string name)
        {
            this.name = name;
        }

        public Symbol Clone()
        {
            return new Nonterminal(name);
        }
    }

    // %token < strVal > VOID INT
    public enum TerminalType
    {
        TOKEN,
        CONSTANT_CHAR,
        EMPTY, // for empty rule
        None
    }

    public class Terminal : Symbol
    {
        public static Terminal BuildToken(string tokenName)
        {
            Terminal t = new Terminal();
            t.tokenName = tokenName;
            t.type = TerminalType.TOKEN;

            return t;
        }

        public static Terminal BuildToken(string tokenName, object tokenObject)
        {
            Terminal t = new Terminal();
            t.tokenName = tokenName;
            t.type = TerminalType.TOKEN;
            t.tokenObject = tokenObject;

            return t;
        }

        public static Terminal BuildConstCharTerminal(char constCharValue)
        {
            Terminal t = new Terminal();
            t.constCharValue = "" + constCharValue;
            t.type = TerminalType.CONSTANT_CHAR;

            return t;
        }

        public static Terminal BuildEmptyTerminal()
        {
            Terminal t = new Terminal();
            t.type = TerminalType.EMPTY;

            return t;
        }

        public string tokenName;
        public object tokenObject;
        public string constCharValue;
        public TerminalType type;

        public Symbol Clone()
        {
            Terminal t = new Terminal();
            t.type = type;
            t.tokenName = tokenName;
            t.tokenObject = tokenObject;
            t.constCharValue = constCharValue;

            return t;
        }
    }

}
}

namespace c_grammarNs{

using System.Text;

namespace LexYaccNs
{
    public class Section
    {
        public string definitionSection;
        public string typeSection;
        public string ruleSection;
    }

    public class TypeSectionParser
    {
        // %token <intVal> CONSTANT
        // %type <astVal> program declList decl funDecl typeSpec returnStmt funName param paramList id constant
        public static Tuple<List<LexTokenDef>, Dictionary<string, string>> Parse(string input)
        {
            List<LexTokenDef> lexTokenDef = new List<LexTokenDef>();
            int tokenIndex = 256;
            Dictionary<string, string> ruleNonterminalType = new Dictionary<string, string>();



            if (input != null)
            {
                using (StringReader reader = new StringReader(input))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.Length == 0)
                            continue;

                        bool isToken = false;
                        if (line.StartsWith("%token"))
                        {
                            isToken = true;
                        }
                        else if (line.StartsWith("%type"))
                        {
                            isToken = false;
                        }
                        else
                        {
                            throw new Exception("Syntax error");
                        }


                        int start = line.IndexOf('<');
                        int end = line.IndexOf('>');

                        // the case <List<List<string>>>
                        for (; line[end + 1] == '>'; end++)
                            ;

                        string type = line.Substring(start + 1, end - start - 1);
                        line = line.Substring(end + 1);

                        string[] symbols = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string symbol in symbols)
                        {
                            if (isToken)
                            {
                                LexTokenDef lexTokenDef2 = new LexTokenDef();
                                lexTokenDef2.type = type;
                                lexTokenDef2.name = symbol;
                                lexTokenDef2.index = tokenIndex++;
                                lexTokenDef.Add(lexTokenDef2);
                            }
                            else
                                ruleNonterminalType.Add(symbol, type);
                        }
                    }
                }
            }

            return new Tuple<List<LexTokenDef>, Dictionary<string, string>>(lexTokenDef, ruleNonterminalType);
        }
    }

    public class RuleSectionParser
    {
        public static List<YaccRule> Parse(string input)
        {
            return Parse(input, new List<LexTokenDef>(), new Dictionary<string, string>());
        }

        public static List<YaccRule> Parse(string input, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            List<YaccRule> ret = new List<YaccRule>();

            YaccRule rule = ReadRule(ref input, lexTokenDef, ruleNonterminalType);
            while (rule != null)
            {
                ret.Add(rule);
                rule = ReadRule(ref input, lexTokenDef, ruleNonterminalType);
            }

            ret = ConvertLeftRecursion(ret, lexTokenDef, ruleNonterminalType);

            foreach (YaccRule r in ret)
            {
                for (int i = 0; i < r.productions.Count; i++)
                {
                    Production p = r.productions[i];
                    p.index = i;
                    p.action = ConvertActionVariable(p.action);
                }
            }

            return ret;
        }

        private static string ConvertActionVariable(string action)
        {
            if (action == null)
                return null;

            action = action.Replace("$$", "_0");

            StringBuilder sb = new StringBuilder();

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < action.Length; i++)
            {
                if (action[i] == '$' && i + 1 < action.Length && char.IsDigit(action[i + 1]))
                {
                    // Replace '$' with '_'
                    sb.Append('_');
                }
                else
                {
                    sb.Append(action[i]);
                }
            }
            return sb.ToString();
        }

        public static Production ReadProduction2(string productionString, Nonterminal lhs, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            List<Symbol> symbols = new List<Symbol>();
            string[] rhsTokens = productionString.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            // empty rule
            if (rhsTokens.Length == 0)
            {
                symbols.Add(Terminal.BuildEmptyTerminal());
                return new Production(lhs, symbols, lexTokenDef, ruleNonterminalType, null, ProductionType.Plain);
            }
            else
            {
                for (int i = 0; i < rhsTokens.Length; i++)
                {
                    string token = rhsTokens[i].Trim();
                    bool isToken = false;
                    foreach (LexTokenDef l in lexTokenDef)
                    {
                        if (l.name == token)
                            isToken = true;
                    }

                    if (isToken)
                    {
                        symbols.Add(Terminal.BuildToken(token));
                    }
                    else if (token.StartsWith("'") && token.EndsWith("'"))
                    {
                        symbols.Add(Terminal.BuildConstCharTerminal(token[1]));
                    }
                    else
                    {
                        symbols.Add(new Nonterminal(token));
                    }
                }

                return new Production(lhs, symbols, lexTokenDef, ruleNonterminalType, null, ProductionType.Plain);
            }
        }

        private static Production ReadProduction(ref string input, Nonterminal lhs, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            input = input.Trim();
            if (input.Length == 0)
                return null;

            if (input[0] == '|')
            {
                input = input.Substring(1).Trim();
            }
            else if (input[0] == ';')
            {
                input = input.Substring(1).Trim();
                return null;
            }

            string productionString = null;
            string action = null;

            int keyPos = LexYaccUtil.FindCharNotInLiteral(input, new List<char>() { '{', '|', ';' }, true);

            if (keyPos == -1)
            {
                // the very last production in the end of input
                productionString = input;
                input = "";
            }
            else if (input[keyPos] == '{')
            {
                productionString = input.Substring(0, keyPos).Trim();
                int rightBrace = LexYaccUtil.FindCharNotInLiteral(input, '}', true);
                if (rightBrace == -1)
                    throw new Exception("Syntax error");
                action = input.Substring(keyPos + 1, rightBrace - keyPos - 1);
                action = LexYaccUtil.RemoveHeadAndTailEmptyLine(action);

                input = input.Substring(rightBrace + 1).Trim();
            }
            else if (input[keyPos] == '|' || input[keyPos] == ';')
            {
                productionString = input.Substring(0, keyPos);
                input = input.Substring(keyPos).Trim();
            }


            Production p = ReadProduction2(productionString, lhs, lexTokenDef, ruleNonterminalType);
            p.action = action;

            return p;
        }

        private static YaccRule ReadRule(ref string input, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            input = input.Trim();
            if (input.Length == 0)
                return null;

            int comma = input.IndexOf(':');
            if (comma == -1)
                throw new Exception("Syntax error");

            string lhs = input.Substring(0, comma).Trim();
            input = input.Substring(comma + 1);
            YaccRule rule = new YaccRule();
            rule.lhs = new Nonterminal(lhs);

            // special handling for empty production in the front case:   a: | 'A' or a : {} | 'A'
            input = input.Trim();
            if (input[0] == '|')
            {
                rule.productions.Add(new Production(rule.lhs, new List<Symbol> { Terminal.BuildEmptyTerminal() }, lexTokenDef, ruleNonterminalType, null, ProductionType.Plain));
            }
            else if (input[0] == '{')
            {
                int rightCurlyPos = LexYaccUtil.FindCharNotInLiteral(input, '}', true);
                string action = input.Substring(1, rightCurlyPos - 1).Trim();
                input = input.Substring(rightCurlyPos + 1);
                rule.productions.Add(new Production(rule.lhs, new List<Symbol> { Terminal.BuildEmptyTerminal() }, lexTokenDef, ruleNonterminalType, action, ProductionType.Plain));

            }

            Production production = ReadProduction(ref input, rule.lhs, lexTokenDef, ruleNonterminalType);
            while (production != null)
            {
                rule.productions.Add(production);
                production = ReadProduction(ref input, rule.lhs, lexTokenDef, ruleNonterminalType);
            }

            return rule;
        }

        public static List<YaccRule> ConvertLeftRecursion(List<YaccRule> rules, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            List<YaccRule> ret = new List<YaccRule>();

            foreach (YaccRule pr in rules)
            {
                bool leftRecursion = false;
                foreach (Production p in pr.productions)
                {
                    if (p.symbols[0] is Nonterminal && ((Nonterminal)p.symbols[0]).name == pr.lhs.name)
                    {
                        leftRecursion = true;
                        break;
                    }
                }

                if (!leftRecursion)
                {
                    ret.Add(pr);
                }
                else
                {
                    YaccRule newPr1 = new YaccRule();
                    newPr1.lhs = new Nonterminal(pr.lhs.name);
                    ret.Add(newPr1);

                    YaccRule newPr2 = new YaccRule();
                    string expandedName = pr.lhs.name + "_LeftRecursionExpand";
                    newPr2.lhs = new Nonterminal(expandedName);
                    ret.Add(newPr2);

                    if (ruleNonterminalType.ContainsKey(pr.lhs.name))
                        ruleNonterminalType.Add(expandedName, ruleNonterminalType[pr.lhs.name]);

                    /*
                        a: a 'A' | a 'B' | 'C' | 'D' =>

                        a: 'C' a2 | 'D' a2
                        a2: 'A' a2 | 'B' a2 | empty
                    */
                    foreach (Production p in pr.productions)
                    {
                        // is left recursive
                        if (p.symbols.Count > 0 && p.symbols[0] is Nonterminal && ((Nonterminal)(p.symbols[0])).name == pr.lhs.name)
                        {
                            Production p2 = p.Clone();
                            p2.type = ProductionType.LeftRecursiveSecond;
                            p2.symbols.RemoveAt(0);
                            p2.symbols.Add(new Nonterminal(expandedName));
                            p2.lhs = newPr2.lhs;
                            newPr2.productions.Add(p2);
                        }
                        else
                        {
                            Production p2 = p.Clone();
                            p2.type = ProductionType.LeftRecursiveFirst;
                            p2.symbols.Add(new Nonterminal(expandedName));
                            newPr1.productions.Add(p2);
                        }
                    }

                    // empty production
                    newPr2.productions.Add(new Production(newPr2.lhs, new List<Symbol>() { Terminal.BuildEmptyTerminal() }, lexTokenDef, ruleNonterminalType, "", ProductionType.LeftRecursiveSecond));
                }
            }

            return ret;
        }
    }

    public class YaccRuleReader
    {
        public static void Parse(string input, out Section sections, out List<YaccRule> productionRules, out List<LexTokenDef> lexTokenDef, out Dictionary<string, string> ruleNonterminalType)
        {
            sections = SplitSecction(input, true);
            productionRules = new List<YaccRule>();

            Tuple<List<LexTokenDef>, Dictionary<string, string>> types = TypeSectionParser.Parse(sections.typeSection);
            lexTokenDef = types.Item1;
            ruleNonterminalType = types.Item2;

            productionRules = RuleSectionParser.Parse(sections.ruleSection, lexTokenDef, ruleNonterminalType);
            InsertStartRule(productionRules, lexTokenDef, ruleNonterminalType);
        }

        private static void InsertStartRule(List<YaccRule> productionRules, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            string userStartNonterminal = productionRules[0].lhs.name;
            Nonterminal start = new Nonterminal("start");
            Production startProduction = RuleSectionParser.ReadProduction2(userStartNonterminal, start, lexTokenDef, ruleNonterminalType);
            startProduction.action = "_0 = _1;";
            if (ruleNonterminalType.ContainsKey(userStartNonterminal))
                ruleNonterminalType["start"] = ruleNonterminalType[userStartNonterminal];
            startProduction.index = 0;
            YaccRule pr = new YaccRule(start, new List<Production> { startProduction });
            productionRules.Insert(0, pr);
        }

        public static Section SplitSecction(string input, bool singleQuoteAsString)
        {
            Section s = new Section();

            int definitionStart = LexYaccUtil.FindStringNotInLiteral(input, "%{", singleQuoteAsString);
            if (definitionStart != -1)
            {
                int definitionEnd = LexYaccUtil.FindStringNotInLiteral(input, "%}", singleQuoteAsString);
                s.definitionSection = input.Substring(definitionStart + 2, definitionEnd - definitionStart - 2).Trim();
                input = input.Substring(definitionEnd + 2);

                int ruleStart = LexYaccUtil.FindStringNotInLiteral(input, "%%", singleQuoteAsString);
                s.typeSection = input.Substring(0, ruleStart).Trim();
                input = input.Substring(ruleStart + 2);

                int ruleEnd = LexYaccUtil.FindStringNotInLiteral(input, "%%", singleQuoteAsString);
                s.ruleSection = input.Substring(0, ruleEnd).Trim();
            }
            else
            {
                // special format for ut that has only rule section
                s.ruleSection = input.Trim();
            }

            return s;
        }
    }
}

}

namespace c_grammarNs{

namespace RegexNs
{

    public class State
    {
        public PatternChar pc = null;
        public int index = -1;
        public List<State> epislonTransition = new List<State>();
        public State matchTransition = null;

        public State(PatternChar pc)
        {
            this.pc = pc;
        }
    }

    public enum RecognizeResult
    {
        AliveAndAccept,
        AliveButNotAccept,
        EndAndReject
    }

    public class RecognizeParam
    {
        public List<State> availableStates;
        public RecognizeParam(List<State> availableStates)
        {
            this.availableStates = availableStates;
        }
    }

    public class NFA
    {
        public State startState = null;
        public State acceptedState = null;
        public bool startsWith = false;
        public bool endsWith = false;
        public List<PatternChar> patternChars = null;

        // One "|" must be used with one ()
        // for example, ((A|B)|C)
        // so in the operatorStack, when encounter a '|', the next pop must be '('
        public static NFA Build(string pattern)
        {
            NFA nfa = new NFA();

            nfa.patternChars = PatternTransformer.Transform(pattern, ref nfa.startsWith, ref nfa.endsWith);

            List<State> states = new List<State>();
            for (int i = 0; i < nfa.patternChars.Count; i++)
            {
                State s = new State(nfa.patternChars[i]);
                s.index = i;
                states.Add(s);
            }
            State acceptState = new State(new PatternChar('\0', PatternCharType.MetaChar));
            states.Add(acceptState);
            nfa.acceptedState = acceptState;

            nfa.startState = states[0];

            Stack<State> operatorStack = new Stack<State>();

            for (int i = 0; i < nfa.patternChars.Count; i++)
            {
                State s = states[i];
                PatternChar pc = s.pc;
                PatternCharType type = pc.type;


                if (type == PatternCharType.Char || type == PatternCharType.MultipleChar || (pc.c == '.' && type == PatternCharType.MetaChar))
                    s.matchTransition = states[i + 1];
                else if (type == PatternCharType.MetaChar && (pc.c == '(' || pc.c == ')' || pc.c == '*'))
                    s.epislonTransition.Add(states[i + 1]);


                if ((pc.c == '(' && type == PatternCharType.MetaChar) || (pc.c == '|' && type == PatternCharType.MetaChar))
                {
                    operatorStack.Push(s);
                }
                else if (pc.c == ')' && type == PatternCharType.MetaChar)
                {
                    State op = operatorStack.Pop();

                    State nextState = states[i + 1];

                    if (op.pc.c == '|' && op.pc.type == PatternCharType.MetaChar)
                    {
                        State op2 = operatorStack.Pop();
                        op2.epislonTransition.Add(states[op.index + 1]);
                        op.epislonTransition.Add(s);

                        if (nextState.pc.c == '*' && nextState.pc.type == PatternCharType.MetaChar)
                        {
                            nextState.epislonTransition.Add(op2);
                            op2.epislonTransition.Add(nextState);
                        }
                    }
                    else if (op.pc.c == '(' && op.pc.type == PatternCharType.MetaChar)
                    {
                        if (nextState.pc.c == '*' && nextState.pc.type == PatternCharType.MetaChar)
                        {
                            nextState.epislonTransition.Add(op);
                            op.epislonTransition.Add(nextState);
                        }
                    }
                }
                else if (s.pc.type != PatternCharType.MetaChar || (s.pc.c != '(' && s.pc.c != ')' && s.pc.c != '|'))
                {
                    State nextState = states[i + 1];
                    if (nextState.pc.c == '*' && nextState.pc.type == PatternCharType.MetaChar)
                    {
                        s.epislonTransition.Add(nextState);
                        nextState.epislonTransition.Add(s);
                    }
                }
            }

            return nfa;
        }


        public string Match(string txt)
        {
            for (int i = 0; i < txt.Length; i++)
            {
                string matchString = MatchInternal(txt.Substring(i));

                // found match
                if (matchString != "")
                {
                    if (startsWith == true && i != 0)
                        continue;
                    if (endsWith == true && i + matchString.Length != txt.Length)
                        continue;
                    return matchString;
                }
            }

            return "";
        }

        public string MatchInternal(string txt)
        {
            Tuple<RecognizeParam, RecognizeResult> ret = InitRecognize();
            RecognizeParam param = ret.Item1;
            RecognizeResult result = ret.Item2;

            int lastMatch = -1;
            for (int i = 0; i < txt.Length; i++)
            {
                result = StepRecognize(txt[i], param);
                if (result == RecognizeResult.AliveAndAccept)
                    lastMatch = i;
                else if (result == RecognizeResult.EndAndReject)
                    break;
            }

            return txt.Substring(0, lastMatch + 1);
        }

        public RecognizeParam CreateRecognizeParam()
        {
            return new RecognizeParam(new List<State>() { startState });
        }

        public RecognizeResult StepRecognize(char c, RecognizeParam param)
        {
            List<State> availableStates = DoEpsilonTransition(param.availableStates);
            List<State> nextAvailableStates = new List<State>();

            foreach (State s in availableStates)
            {
                if (s.pc.type == PatternCharType.Char)
                {
                    if (s.pc.c == c)
                        nextAvailableStates.Add(s.matchTransition);
                }
                else if (s.pc.type == PatternCharType.MultipleChar)
                {
                    bool match = s.pc.multipleChars.Contains(c);
                    if (s.pc.not)
                        match = !match;

                    if (match)
                        nextAvailableStates.Add(s.matchTransition);
                }
                else if (s.pc.c == '.' && s.pc.type == PatternCharType.MetaChar)
                {
                    nextAvailableStates.Add(s.matchTransition);
                }
            }


            param.availableStates.Clear();
            param.availableStates.AddRange(DoEpsilonTransition(nextAvailableStates));

            if (param.availableStates.Count == 0)
                return RecognizeResult.EndAndReject;
            else if (param.availableStates.Contains(acceptedState))
                return RecognizeResult.AliveAndAccept;
            else
                return RecognizeResult.AliveButNotAccept;
        }

        private Tuple<RecognizeParam, RecognizeResult> InitRecognize()
        {
            List<State> availableStates = new List<State>();
            availableStates.Add(startState);

            RecognizeResult result = RecognizeResult.AliveButNotAccept;
            availableStates = DoEpsilonTransition(availableStates);
            if (availableStates.Contains(acceptedState))
                result = RecognizeResult.AliveAndAccept;

            return new Tuple<RecognizeParam, RecognizeResult>(new RecognizeParam(availableStates), result);
        }

        public bool Recognize(string txt)
        {
            Tuple<RecognizeParam, RecognizeResult> ret = InitRecognize();
            RecognizeParam param = ret.Item1;
            RecognizeResult result = ret.Item2;

            foreach (char c in txt)
            {
                result = StepRecognize(c, param);
                if (result == RecognizeResult.EndAndReject)
                    break;
            }

            return result == RecognizeResult.AliveAndAccept;
        }

        private void DFS(State s, HashSet<int> visited, List<State> newStates)
        {
            if (visited.Contains(s.index))
                return;

            visited.Add(s.index);
            newStates.Add(s);
            foreach (State s2 in s.epislonTransition)
                DFS(s2, visited, newStates);
        }

        private List<State> DoEpsilonTransition(List<State> states)
        {
            List<State> newStates = new List<State>();
            HashSet<int> visited = new HashSet<int>();

            foreach (State s in states)
                DFS(s, visited, newStates);

            return newStates;
        }
    }

}
}

namespace c_grammarNs{

namespace RegexNs
{
    public class PatternChar
    {
        public PatternChar()
        {
        }

        public PatternChar(char c)
        {
            this.c = c;
            type = PatternCharType.Char;
        }

        public PatternChar(char c, PatternCharType type)
        {
            this.c = c;
            this.type = type;
        }

        public char c = '\0';
        public List<char> multipleChars = new List<char>();
        public bool not = false;

        public PatternCharType type = PatternCharType.None;

        public PatternChar Clone()
        {
            PatternChar pc = new PatternChar(c, type);
            pc.not = not;
            pc.multipleChars.AddRange(multipleChars);

            return pc;
        }
    }

    public enum PatternCharType
    {
        Char,
        MultipleChar,
        MetaChar,
        None
    }

    public class PatternTransformer
    {
        public static List<PatternChar> ToPatternChar(string pattern)
        {
            List<PatternChar> patternChars = new List<PatternChar>();
            foreach (char c in pattern)
                patternChars.Add(new PatternChar(c));

            return patternChars;
        }

        // \ ^ | . $ ? * + ( ) [ ] { } d D w W s W
        public static List<PatternChar> TransformEscape(List<PatternChar> patternChars)
        {
            List<PatternChar> newPatternChars = new List<PatternChar>();

            List<char> escapedChars = new List<char>() { '\\', '^', '|', '.', '$', '?', '*', '+', '(', ')', '[', ']', '{', '}', };
            List<char> shorthand = new List<char>() { 'd', 'D', 'w', 'W', 's', 'S' };

            for (int i = 0; i < patternChars.Count; i++)
            {
                PatternChar c = patternChars[i];
                if (c.c == '\\' && i < patternChars.Count)
                {
                    if (escapedChars.Contains(patternChars[i + 1].c))
                    {
                        newPatternChars.Add(new PatternChar(patternChars[i + 1].c));
                        i++;
                    }
                    else if (shorthand.Contains(patternChars[i + 1].c))
                    {
                        newPatternChars.Add(new PatternChar(patternChars[i + 1].c, PatternCharType.MetaChar));
                        i++;
                    }
                    else if (patternChars[i + 1].c == 'n')
                    {
                        newPatternChars.Add(new PatternChar('\n'));
                        i++;
                    }
                    else if (patternChars[i + 1].c == 'r')
                    {
                        newPatternChars.Add(new PatternChar('\r'));
                        i++;
                    }
                    else if (patternChars[i + 1].c == 't')
                    {
                        newPatternChars.Add(new PatternChar('\t'));
                        i++;
                    }
                }
                else
                {
                    if (escapedChars.Contains(c.c))
                        newPatternChars.Add(new PatternChar(c.c, PatternCharType.MetaChar));
                    else
                        newPatternChars.Add(c);
                }
            }

            return newPatternChars;
        }

        // "[-\\na-z-][^-\\[]\\tx"
        // in [], escape only ], \, \t, \n, \r
        public static List<PatternChar> TransformSquareBracket(List<PatternChar> patternChars)
        {
            List<PatternChar> newPatternChars = new List<PatternChar>();

            for (int i = 0; i < patternChars.Count;)
            {
                PatternChar c = patternChars[i];

                if (c.c == '[' && c.type == PatternCharType.MetaChar)
                {
                    int squareStart = i;
                    int squareEnd = squareStart + 1;
                    for (; squareEnd < patternChars.Count; squareEnd++)
                    {
                        if (patternChars[squareEnd].c == ']' && patternChars[squareEnd].type == PatternCharType.MetaChar)
                            break;
                    }
                    i = squareEnd + 1;

                    PatternChar multipleChar = new PatternChar();
                    multipleChar.type = PatternCharType.MultipleChar;

                    if (patternChars[squareStart + 1].c == '^')
                    {
                        multipleChar.not = true;
                        squareStart++;
                    }

                    for (int j = squareStart + 1; j < squareEnd;)
                    {
                        // [-a] case
                        if (j == squareStart + 1)
                        {
                            if (patternChars[j].c == '-')
                            {
                                multipleChar.multipleChars.Add('-');
                                j++;
                                continue;
                            }
                        }

                        // [-], [a] case
                        if (j + 1 == squareEnd)
                        {
                            multipleChar.multipleChars.Add(patternChars[j].c);
                            break;
                        }

                        // [ab] case
                        if (j + 2 == squareEnd)
                        {
                            multipleChar.multipleChars.Add(patternChars[j].c);
                            multipleChar.multipleChars.Add(patternChars[j + 1].c);
                            break;
                        }

                        if (patternChars[j + 1].c == '-')
                        {
                            for (char c2 = patternChars[j].c; c2 <= patternChars[j + 2].c; c2++)
                                multipleChar.multipleChars.Add(c2);
                            j += 3;
                            continue;
                        }
                        else
                        {
                            multipleChar.multipleChars.Add(patternChars[j].c);
                            j++;
                            continue;
                        }
                    }


                    newPatternChars.Add(multipleChar);
                    continue;
                }
                else
                {
                    newPatternChars.Add(patternChars[i]);
                    i++;
                }
            }

            return newPatternChars;
        }

        /*
          \d	[0-9]
          \D	[^0-9]
          \w	[a-zA-Z0-9]
          \W	[^a-zA-Z0-9]
          \s	[ \t\n\r]
          \S	[^ \t\n\r]
        */
        public static List<PatternChar> TransformShorthand(List<PatternChar> patternChars)
        {
            List<PatternChar> newPatternChars = new List<PatternChar>();
            for (int i = 0; i < patternChars.Count; i++)
            {
                PatternChar pc = patternChars[i];
                if (pc.type == PatternCharType.MetaChar && new List<char> { 'd', 'D', 'w', 'W', 's', 'S' }.Contains(pc.c) && i < patternChars.Count)
                {
                    PatternChar patternChar = new PatternChar();
                    patternChar.type = PatternCharType.MultipleChar;

                    if (pc.c == 'd' || pc.c == 'D')
                    {
                        for (char c2 = '0'; c2 <= '9'; c2++)
                            patternChar.multipleChars.Add(c2);

                        if (pc.c == 'D')
                            patternChar.not = true;

                        newPatternChars.Add(patternChar);
                    }
                    else if (pc.c == 'w' || pc.c == 'W')
                    {
                        for (char c2 = '0'; c2 <= '9'; c2++)
                            patternChar.multipleChars.Add(c2);
                        for (char c2 = 'a'; c2 <= 'z'; c2++)
                            patternChar.multipleChars.Add(c2);
                        for (char c2 = 'A'; c2 <= 'Z'; c2++)
                            patternChar.multipleChars.Add(c2);

                        if (pc.c == 'W')
                            patternChar.not = true;

                        newPatternChars.Add(patternChar);
                    }
                    else if (pc.c == 's' || pc.c == 'S')
                    {
                        patternChar.multipleChars.Add(' ');
                        patternChar.multipleChars.Add('t');
                        patternChar.multipleChars.Add('n');
                        patternChar.multipleChars.Add('r');

                        if (pc.c == 'S')
                            patternChar.not = true;

                        newPatternChars.Add(patternChar);
                    }
                }
                else
                {
                    newPatternChars.Add(pc);
                }
            }

            return newPatternChars;
        }

        // Add () to each or, ex: a|b|c -> ((a|b)|c)
        // remove redundant () between |, ex: ((ab)|(cd)) -> (ab|cd)
        public static List<PatternChar> ModifyParentsisBetweenOr(List<PatternChar> patternChars)
        {
            List<List<PatternChar>> tokensSplitedByOr = new List<List<PatternChar>>();

            List<PatternChar> token = new List<PatternChar>();
            int level = 0;

            for (int i = 0; i < patternChars.Count; i++)
            {
                PatternChar c = patternChars[i];

                if (c.c == '(' && c.type == PatternCharType.MetaChar)
                    level++;
                else if (c.c == ')' && c.type == PatternCharType.MetaChar)
                    level--;

                if (level > 0)
                {
                    token.Add(c);
                }
                else
                {
                    if (c.c == '|' && c.type == PatternCharType.MetaChar)
                    {
                        tokensSplitedByOr.Add(token);
                        token = new List<PatternChar>();
                    }
                    else
                    {
                        token.Add(c);
                    }
                }
            }

            tokensSplitedByOr.Add(token);

            if (tokensSplitedByOr.Count == 1)
            {
                token = tokensSplitedByOr[0];

                if (token.Count == 0)
                {
                    // the case (|a)
                    return token;
                }

                // if (a)(b), return (a)(b)
                // if abc, return abc
                // if ((a)(b)), return (a)(b)
                if (token[0].c == '(' && token[0].type == PatternCharType.MetaChar)
                {
                    int level2 = 1;
                    for (int j = 1; j < token.Count; j++)
                    {
                        if (token[j].c == '(' && token[j].type == PatternCharType.MetaChar)
                            level2++;
                        else if (token[j].c == ')' && token[j].type == PatternCharType.MetaChar)
                            level2--;


                        if (level2 == 0)
                        {
                            if (j == token.Count - 1)
                            {
                                // the case ((a)(b))
                                token.RemoveAt(0);
                                token.RemoveAt(token.Count - 1);
                                return ModifyParentsisBetweenOr(token);
                            }
                            else
                            {
                                // the case (a)(b)
                                return tokensSplitedByOr[0];
                            }
                        }
                    }

                    return ModifyParentsisBetweenOr(token);
                }
                else
                {
                    return tokensSplitedByOr[0];
                }
            }
            else
            {
                List<PatternChar> ret = ModifyParentsisBetweenOr(tokensSplitedByOr[0]);
                for (int i = 1; i < tokensSplitedByOr.Count; i++)
                {
                    ret.Insert(0, new PatternChar('(', PatternCharType.MetaChar));
                    ret.Add(new PatternChar('|', PatternCharType.MetaChar));
                    ret.AddRange(ModifyParentsisBetweenOr(tokensSplitedByOr[i]));
                    ret.Add(new PatternChar(')', PatternCharType.MetaChar));
                }

                return ret;
            }
        }

        /*
        // A+ = AA*
        // A? = (|A)
        // A{3} = AAA
        // A{2-4} = (AA|AAA|AAAA)
        // A{3-} = AAAA*
        */
        public static List<PatternChar> TransformSuffix(List<PatternChar> patternChars)
        {
            List<PatternChar> newPatternChars = new List<PatternChar>();
            List<PatternChar> nextNewPatternChars = new List<PatternChar>();
            nextNewPatternChars.AddRange(patternChars);

            bool keepGoing = true;

            while (keepGoing)
            {
                newPatternChars.Clear();
                newPatternChars.AddRange(nextNewPatternChars);
                nextNewPatternChars.Clear();

                keepGoing = false;

                for (int i = newPatternChars.Count - 1; i >= 0; i--)
                {
                    PatternChar c = newPatternChars[i];
                    if (c.type == PatternCharType.MetaChar && (c.c == '+' || c.c == '?' || c.c == '}'))
                    {
                        int j = i;

                        int countStart = -1;
                        int countEnd = -1;

                        if (c.c == '+')
                        {
                            keepGoing = true;
                            j--;
                        }
                        else if (c.c == '?')
                        {
                            keepGoing = true;
                            j--;
                        }
                        else if (c.c == '}')
                        {
                            keepGoing = true;

                            // A{3}
                            // A{3-}
                            // A{2-4}
                            int rightCurlyBracket = j;
                            int leftCurlyBracket = j - 1;
                            for (; !(newPatternChars[leftCurlyBracket].c == '{' && newPatternChars[leftCurlyBracket].type == PatternCharType.MetaChar); leftCurlyBracket--)
                                ;

                            int dash = leftCurlyBracket + 1;
                            for (; newPatternChars[dash].c != '-' && dash < rightCurlyBracket; dash++)
                                ;

                            string temp = "";
                            if (dash == rightCurlyBracket)
                            {
                                // A{3}
                                for (int k = leftCurlyBracket + 1; k < rightCurlyBracket; k++)
                                    temp += newPatternChars[k].c;
                                countStart = int.Parse(temp);
                                countEnd = countStart;
                            }
                            else if (dash + 1 != rightCurlyBracket)
                            {
                                // A{2-4}
                                temp = "";
                                for (int k = leftCurlyBracket + 1; k < dash; k++)
                                    temp += newPatternChars[k].c;
                                countStart = int.Parse(temp);

                                temp = "";
                                for (int k = dash + 1; k < rightCurlyBracket; k++)
                                    temp += newPatternChars[k].c;
                                countEnd = int.Parse(temp);
                            }
                            else
                            {
                                // A{3-}
                                temp = "";
                                for (int k = leftCurlyBracket + 1; k < dash; k++)
                                    temp += newPatternChars[k].c;
                                countStart = int.Parse(temp);
                            }

                            j = leftCurlyBracket - 1;
                        }

                        // a+ => subPattern = a
                        // (abc)+ => subPattern = (abc)
                        List<PatternChar> subPattern = new List<PatternChar>();
                        if (newPatternChars[j].c == ')' && newPatternChars[j].type == PatternCharType.MetaChar)
                        {
                            subPattern.Insert(0, newPatternChars[j]);
                            j--;
                            int level = 1;

                            for (; j >= 0; j--)
                            {
                                if (newPatternChars[j].c == ')' && newPatternChars[j].type == PatternCharType.MetaChar)
                                    level++;
                                else if (newPatternChars[j].c == '(' && newPatternChars[j].type == PatternCharType.MetaChar)
                                    level--;

                                subPattern.Insert(0, newPatternChars[j]);

                                if (level == 0)
                                {
                                    j--;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            subPattern.Add(newPatternChars[j]);
                            j--;
                        }

                        if (c.c == '+')
                        {
                            // A+ = AA*
                            // (abc)+ = (abc)(abc)*
                            nextNewPatternChars.Insert(0, new PatternChar('*', PatternCharType.MetaChar));
                            nextNewPatternChars.InsertRange(0, RepeatPatternChar(subPattern, 2));
                        }
                        else if (c.c == '?')
                        {
                            // A? = (|A)
                            // (abc)? = (|(abc))
                            nextNewPatternChars.Insert(0, new PatternChar(')', PatternCharType.MetaChar));
                            nextNewPatternChars.InsertRange(0, RepeatPatternChar(subPattern, 1));
                            nextNewPatternChars.Insert(0, new PatternChar('|', PatternCharType.MetaChar));
                            nextNewPatternChars.Insert(0, new PatternChar('(', PatternCharType.MetaChar));
                        }
                        else if (c.c == '}')
                        {
                            if (countEnd == -1)
                            {
                                // A{2-} = AAA*
                                // (abc){2-} = (abc)(abc)(abc)*
                                nextNewPatternChars.Insert(0, new PatternChar('*', PatternCharType.MetaChar));
                                nextNewPatternChars.InsertRange(0, RepeatPatternChar(subPattern, countStart + 1));
                            }
                            else if (countStart == countEnd)
                            {
                                // A{3} = AAA
                                // (abc){3} = (abc)(abc)(abc)
                                nextNewPatternChars.InsertRange(0, RepeatPatternChar(subPattern, countStart));
                            }
                            else
                            {
                                // A{2-3} = (AA|AAA)
                                // (abc){2-4} = (((abc)(abc)|(abc)(abc)(abc))|(abc)(abc)(abc)(abc))

                                List<PatternChar> temp = new List<PatternChar>();
                                for (int k = countStart; k <= countEnd; k++)
                                {
                                    if (k != countStart)
                                    {
                                        temp.Insert(0, new PatternChar('(', PatternCharType.MetaChar));
                                        temp.Add(new PatternChar('|', PatternCharType.MetaChar));
                                    }

                                    temp.AddRange(RepeatPatternChar(subPattern, k));

                                    if (k != countStart)
                                        temp.Add(new PatternChar(')', PatternCharType.MetaChar));
                                }

                                nextNewPatternChars.InsertRange(0, temp);
                            }
                        }

                        for (; j >= 0; j--)
                        {
                            nextNewPatternChars.Insert(0, newPatternChars[j]);
                        }
                        break;
                    }
                    else
                    {
                        nextNewPatternChars.Insert(0, c);
                    }
                }
            }

            return nextNewPatternChars;
        }

        private static List<PatternChar> RepeatPatternChar(List<PatternChar> patternChars, int repeatCount)
        {
            List<PatternChar> ret = new List<PatternChar>();

            for (int count = 0; count < repeatCount; count++)
            {
                for (int k = 0; k < patternChars.Count; k++)
                {
                    ret.Add(patternChars[k].Clone());
                }
            }

            return ret;
        }

        public static List<PatternChar> TransformStartsWithEndsWith(List<PatternChar> patternChars, ref bool startsWith, ref bool endsWith)
        {
            if (patternChars.Count > 0 && patternChars[0].c == '^' && patternChars[0].type == PatternCharType.MetaChar)
            {
                startsWith = true;
                patternChars.RemoveAt(0);
            }

            if (patternChars.Count > 0 && patternChars[patternChars.Count - 1].c == '$' && patternChars[patternChars.Count - 1].type == PatternCharType.MetaChar)
            {
                endsWith = true;
                patternChars.RemoveAt(patternChars.Count - 1);
            }


            return patternChars;
        }


        public static List<PatternChar> Transform(string pattern, ref bool startsWith, ref bool endsWith)
        {
            List<PatternChar> patternChars = ToPatternChar(pattern);

            patternChars = TransformEscape(patternChars);
            patternChars = TransformStartsWithEndsWith(patternChars, ref startsWith, ref endsWith);
            patternChars = TransformShorthand(patternChars);
            patternChars = TransformSquareBracket(patternChars);
            patternChars = ModifyParentsisBetweenOr(patternChars);
            patternChars = TransformSuffix(patternChars);

            return patternChars;
        }
    }
}

}

namespace c_grammarNs{

namespace RegexNs
{

    public class Regex
    {
        public static bool Recognize(string regex, string input)
        {
            NFA nfa = NFA.Build(regex);
            return nfa.Recognize(input);
        }

        public static string MatchFirst(string regex, string input)
        {
            NFA nfa = NFA.Build(regex);
            return nfa.Match(input);
        }

        public static List<string> MatchAll(string regex, string input)
        {
            List<string> all = new List<string>();

            NFA nfa = NFA.Build(regex);
            string match = nfa.Match(input);
            while (match != "")
            {
                all.Add(match);
                input = input.Substring(input.IndexOf(match) + match.Length);
                match = nfa.Match(input);
            }

            return all;
        }

        public static NFA Compile(string regex)
        {
            return NFA.Build(regex);
        }
    }

}
}
