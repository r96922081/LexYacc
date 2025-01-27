//Yacc Gen 
namespace UtYacc4Ns
{



public class YaccActions{

    public static Dictionary<string, Func<Dictionary<int, object>, object>> actions = new Dictionary<string, Func<Dictionary<int, object>, object>>();

    public static string ruleInput = @"
%{        
%}
%type <string> a
%%
a: a 'A' {Console.WriteLine(""a with A"");} 
|  a 'B' {Console.WriteLine(""a with B"");} 
|  'C' {Console.WriteLine(""single C"");} 
|  'D' {Console.WriteLine(""single D"");} 
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
        actions.Add("Rule_a_Producton_0", Rule_a_Producton_0);
        actions.Add("Rule_a_Producton_1", Rule_a_Producton_1);
        actions.Add("Rule_a_LeftRecursionExpand_Producton_0", Rule_a_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_a_LeftRecursionExpand_Producton_1", Rule_a_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_a_LeftRecursionExpand_Producton_2", Rule_a_LeftRecursionExpand_Producton_2);
    }

    public static object Rule_start_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 = (string)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_a_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");

        // user-defined action
        Console.WriteLine("single C");

        return _0;
    }

    public static object Rule_a_Producton_1(Dictionary<int, object> objects) { 
        string _0 = new string("");

        // user-defined action
        Console.WriteLine("single D");

        return _0;
    }

    public static object Rule_a_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 =(string)objects[1];

        // user-defined action
        Console.WriteLine("a with A");

        return _0;
    }

    public static object Rule_a_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        string _0 = new string("");
        string _1 =(string)objects[1];

        // user-defined action
        Console.WriteLine("a with B");

        return _0;
    }

    public static object Rule_a_LeftRecursionExpand_Producton_2(Dictionary<int, object> objects) { 
        string _0 = new string("");

        return _0;
    }
}

}


