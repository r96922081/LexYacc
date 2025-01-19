namespace CCompilerNs
{
    public class Context
    {
        public static Dictionary<string, Variable> gv = new Dictionary<string, Variable>();
        public static FunDecl funDecl;
        public static Stack<ForLoopStatement> forLoopStatementStack = new Stack<ForLoopStatement>();
    }

    public class Gv
    {
        public static int sn = 0;
    }

    public class VariableType
    {
        public VariableTypeEnum type;
        public int size;
    }

    public enum VariableTypeEnum
    {
        void_type,
        int_type,
        char_type
    }

    public class Variable
    {
        public string name;
        public VariableType type;
        public List<int> arraySize = new List<int>();
        public int stackOffset = -1;
    }

}
