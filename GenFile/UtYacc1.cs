//Yacc Gen 
namespace UtYacc1Ns
{

class TypeA 
{ 

}

class TypeB
{ 

}

class TypeC
{ 

}

class MyStr{
    public MyStr(string s){this.s = s;}
    public string s;
}

public class YaccActions{

    public static Dictionary<string, Func<Dictionary<int, object>, object>> actions = new Dictionary<string, Func<Dictionary<int, object>, object>>();

    public static string ruleInput = @"
%{        
class TypeA 
{ 

}

class TypeB
{ 

}

class TypeC
{ 

}

class MyStr{
    public MyStr(string s){this.s = s;}
    public string s;
}
%}

%type <TypeA>  a
%type <TypeB>  b
%type <TypeC>  c
%token <MyStr> VOID

%%
a: 'A' c 'X' b VOID
{ 
    Console.WriteLine(""rule A lhs = "" + $$.ToString()); 
    Console.WriteLine($2.ToString()); 
    Console.WriteLine($4.ToString()); 
    Console.WriteLine($5.s);};
b: 'B' { Console.WriteLine(""it is b"");};
c: 'C' { Console.WriteLine(""it is c"");}
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
        actions.Add("Rule_b_Producton_0", Rule_b_Producton_0);
        actions.Add("Rule_c_Producton_0", Rule_c_Producton_0);
    }

    public static object Rule_start_Producton_0(Dictionary<int, object> objects) { 
        TypeA _0 = new TypeA();
        TypeA _1 = (TypeA)objects[1];

        // user-defined action
        _0 = _1;

        return _0;
    }

    public static object Rule_a_Producton_0(Dictionary<int, object> objects) { 
        TypeA _0 = new TypeA();
        TypeC _2 = (TypeC)objects[2];
        TypeB _4 = (TypeB)objects[4];
        MyStr _5 = (MyStr)objects[5];

        // user-defined action
        Console.WriteLine("rule A lhs = " + _0.ToString()); 
        Console.WriteLine(_2.ToString()); 
        Console.WriteLine(_4.ToString()); 
        Console.WriteLine(_5.s);

        return _0;
    }

    public static object Rule_b_Producton_0(Dictionary<int, object> objects) { 
        TypeB _0 = new TypeB();

        // user-defined action
        Console.WriteLine("it is b");

        return _0;
    }

    public static object Rule_c_Producton_0(Dictionary<int, object> objects) { 
        TypeC _0 = new TypeC();

        // user-defined action
        Console.WriteLine("it is c");

        return _0;
    }
}

}


