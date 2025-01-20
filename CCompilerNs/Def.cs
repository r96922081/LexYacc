namespace CCompilerNs
{
    public class Context
    {
        public static Dictionary<string, Variable> gv = new Dictionary<string, Variable>();
        public static Dictionary<string, StructDef> structDefs = new Dictionary<string, StructDef>();
        public static FunDecl funDecl;
        public static Stack<ForLoopStatement> forLoopStatementStack = new Stack<ForLoopStatement>();
    }

    public class Gv
    {
        public static int sn = 0;
    }

    public class VariableType
    {
        public string typeName;
        public VariableTypeEnum type;
        public int size;

        public void GetStructSize()
        {
            if (type == VariableTypeEnum.struct_type)
                size = Context.structDefs[typeName].size;

            if (size == -1)
                throw new Exception("size not set");
        }
    }

    public enum VariableTypeEnum
    {
        void_type,
        int_type,
        char_type,
        struct_type,
    }

    public enum VariableScopeEnum
    {
        global,
        param,
        local,
    }

    public class Variable
    {
        public string name;
        public VariableScopeEnum scope;
        public VariableType type;
        public List<int> arraySize = new List<int>();
        public int stackOffset = -1;
    }

}
