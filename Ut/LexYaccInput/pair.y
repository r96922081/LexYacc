%{
public class Data2 {
    public static Dictionary<string, string> pair = new Dictionary<string, string>();
}
%}

%token <string> KEY VALUE EQUALS
%type <int> config line s

%%

s: config {
        foreach (string key in Data2.pair.Keys)
            Console.WriteLine(key + " = " + Data2.pair[key]);
};
config : config line {} | line {};
line: KEY EQUALS VALUE {  Data2.pair.Add($1, $3); };

%%