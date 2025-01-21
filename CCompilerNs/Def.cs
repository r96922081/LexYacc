namespace CCompilerNs
{
    public class Gv
    {
        public static int sn = 0;
        public static Program program = null;
        public static Context context = new Context();
    }

    public class Context
    {
        public Dictionary<string, Variable> gv = new Dictionary<string, Variable>();
        public Dictionary<string, StructDef> structDefs = new Dictionary<string, StructDef>();
        public FunctionDeclare functionDeclare;
        public Stack<ForLoopStatement> forLoopStatementStack = new Stack<ForLoopStatement>();

        public void Clear()
        {
            gv = new Dictionary<string, Variable>();
            structDefs = new Dictionary<string, StructDef>();
            functionDeclare = null;
            forLoopStatementStack = new Stack<ForLoopStatement>();
        }
    }

    public class VariableTypeInfo
    {
        public string typeName;
        public VariableTypeEnum typeEnum;
        public int size;
        public StructDef structDef;
        public List<int> arraySize = new List<int>();

        public void UpdateStructInfo()
        {
            if (typeEnum == VariableTypeEnum.struct_type)
            {
                structDef = Gv.context.structDefs[typeName];
                size = structDef.size;
            }

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
        public VariableTypeInfo typeInfo;
        public int stackOffset = -1;
    }
}
