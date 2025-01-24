namespace CCompilerNs
{
    public class Gv
    {
        public static int sn = 0;
        public static Dictionary<string, string> stringLiteral = new Dictionary<string, string>();
        public static Program program = null;
    }

    public class VariableTypeInfo
    {
        public string typeName;
        public VariableTypeEnum typeEnum;
        private int size;
        public List<int> arraySize = new List<int>();

        private void UpdateStructInfo()
        {
            if (typeEnum == VariableTypeEnum.struct_type)
                size = Util.GetStructDef(typeName).size;

            if (size == -1)
                throw new Exception("size not set");
        }

        public int GetSize()
        {
            if (size == -1)
                UpdateStructInfo();

            return size;
        }

        public void SetSize(int size)
        {
            this.size = size;
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

    public enum VariableAddressOrValue
    {
        Value,
        Address
    }

    public class Variable
    {
        public string name;
        public VariableScopeEnum scope;
        public VariableTypeInfo typeInfo;
        public int stackOffset = -1;
    }

    public class Declare
    {
        public string name;
        public string type;
        public List<int> arraySize = new List<int>();
    }

    public class StructDef : GlobalDeclare
    {
        public string name;
        public List<StructField> fields = new List<StructField>();
        public int size;
    };

    public class StructField
    {
        public VariableTypeInfo typeInfo;
        public string name;
        public int offset;
    };

    // a[1].b.c[2][3].d
    public class VariableId
    {
        public List<string> name = new List<string>();
        public List<List<Expression>> arrayIndexList = new List<List<Expression>>();
    }

    public class BooleanExpression
    {
        public Expression lhs;
        public string op;
        public Expression rhs;
    }
}
