//Yacc Gen 
namespace UtFeed28Ns
{



public class YaccActions{

    public static Dictionary<string, Func<Dictionary<int, object>, object>> actions = new Dictionary<string, Func<Dictionary<int, object>, object>>();

    public static string ruleInput = @"
%{
%}

%type <string> compoundIfStatement ifStatement elseIfStatements elseStatement elseIfStatement

%%
compoundIfStatement:
ifStatement
{
}
|
ifStatement elseIfStatements
{
}
|
ifStatement elseIfStatements elseStatement
{
}
|
ifStatement elseStatement
{
}
;

ifStatement:
'i'
{
    Console.WriteLine(""If"");
}
;

elseIfStatements:
elseIfStatements elseIfStatement
{
}
|
elseIfStatement
{
}
;

elseIfStatement:
'e' 'i'
{
    Console.WriteLine(""else If"");
}
;

elseStatement:
'e'
{
    Console.WriteLine(""else"");
}
%%



;
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
        actions.Add("Rule_compoundIfStatement_Producton_0", Rule_compoundIfStatement_Producton_0);
        actions.Add("Rule_compoundIfStatement_Producton_1", Rule_compoundIfStatement_Producton_1);
        actions.Add("Rule_compoundIfStatement_Producton_2", Rule_compoundIfStatement_Producton_2);
        actions.Add("Rule_compoundIfStatement_Producton_3", Rule_compoundIfStatement_Producton_3);
        actions.Add("Rule_ifStatement_Producton_0", Rule_ifStatement_Producton_0);
        actions.Add("Rule_elseIfStatements_Producton_0", Rule_elseIfStatements_Producton_0);
        actions.Add("Rule_elseIfStatements_LeftRecursionExpand_Producton_0", Rule_elseIfStatements_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_elseIfStatements_LeftRecursionExpand_Producton_1", Rule_elseIfStatements_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_elseIfStatement_Producton_0", Rule_elseIfStatement_Producton_0);
        actions.Add("Rule_elseStatement_Producton_0", Rule_elseStatement_Producton_0);
    }

    public static object Rule_start_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_compoundIfStatement_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];

        return _0;
    }

    public static object Rule_compoundIfStatement_Producton_1(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];

        return _0;
    }

    public static object Rule_compoundIfStatement_Producton_2(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];
        string _3 = (string)objects[3];

        return _0;
    }

    public static object Rule_compoundIfStatement_Producton_3(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];
        string _2 = (string)objects[2];

        return _0;
    }

    public static object Rule_ifStatement_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");

        // user-defined action
        Console.WriteLine("If");

        return _0;
    }

    public static object Rule_elseIfStatements_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];

        return _0;
    }

    public static object Rule_elseIfStatements_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 =(string)objects[1];
        string _2 = (string)objects[2];

        return _0;
    }

    public static object Rule_elseIfStatements_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        string _0 = new string("");

        return _0;
    }

    public static object Rule_elseIfStatement_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");

        // user-defined action
        Console.WriteLine("else If");

        return _0;
    }

    public static object Rule_elseStatement_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");

        // user-defined action
        Console.WriteLine("else");

        return _0;
    }
}

}


