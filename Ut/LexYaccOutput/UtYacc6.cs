//Yacc Gen 
namespace UtYacc6Ns
{



public class YaccActions{

    public static Dictionary<string, Func<Dictionary<int, object>, object>> actions = new Dictionary<string, Func<Dictionary<int, object>, object>>();

    public static string ruleInput = @"
%{        
%}
%token <int> DOUBLE
%type <int> cal exp term
%%
cal: exp {Console.WriteLine(""Result = "" + $1); };
 
exp: exp '+' term {$$ = $1 + $3;}
  | term {$$ = $1;}
  ;
  
term: DOUBLE {$$ = $1;}
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
        actions.Add("Rule_cal_Producton_0", Rule_cal_Producton_0);
        actions.Add("Rule_exp_Producton_0", Rule_exp_Producton_0);
        actions.Add("Rule_exp_LeftRecursionExpand_Producton_0", Rule_exp_LeftRecursionExpand_Producton_0);
        actions.Add("Rule_exp_LeftRecursionExpand_Producton_1", Rule_exp_LeftRecursionExpand_Producton_1);
        actions.Add("Rule_term_Producton_0", Rule_term_Producton_0);
    }

    public static object Rule_start_Producton_0(Dictionary<int, object> objects) { 
        int _0 = new int();
        int _1 = (int)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_cal_Producton_0(Dictionary<int, object> objects) { 
        int _0 = new int();
        int _1 = (int)objects[1];

        // user-defined action
        Console.WriteLine("Result = " + _1); 

        return _0;
    }

    public static object Rule_exp_Producton_0(Dictionary<int, object> objects) { 
        int _0 = new int();
        int _1 = (int)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_exp_LeftRecursionExpand_Producton_0(Dictionary<int, object> objects) { 
        int _0 = new int();
        int _1 =(int)objects[1];
        int _3 = (int)objects[3];

        // user-defined action
        _0 = _1 + _3;

        return _0;
    }

    public static object Rule_exp_LeftRecursionExpand_Producton_1(Dictionary<int, object> objects) { 
        int _0 = new int();

        return _0;
    }

    public static object Rule_term_Producton_0(Dictionary<int, object> objects) { 
        int _0 = new int();
        int _1 = (int)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }
}

}


