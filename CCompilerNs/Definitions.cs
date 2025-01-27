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
        public int pointerCount = 0;

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

    public class VariablePartInfo
    {
        public int count;
        public List<VariableTypeInfo> type = new List<VariableTypeInfo>();
        public List<int> offsets = new List<int>();
        public List<List<Expression>> arrayIndexList = new List<List<Expression>>();
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

    public enum VariableIdLhsRhsType
    {
        Lhs,
        Value,
        None
    }

    // a[1].b.c[2][3].d
    public class VariableId
    {
        public List<string> name = new List<string>();
        public List<List<Expression>> arrayIndexList = new List<List<Expression>>();
        public int pointerCount = 0; // **a.b.c
        public bool addressOf = false; // &a.b.c
        public List<string> op = new List<string>(); // . ->
        public VariableIdLhsRhsType lhsRhs = VariableIdLhsRhsType.None;

        public override string ToString()
        {
            string s = name[0];
            for (int i = 1; i < name.Count; i++)
                s += op[i-1] + name[i];

            if (addressOf)
                s = "&" + s;

            for (int i = 0; i < arrayIndexList.Count; i++)
                if (arrayIndexList[i].Count != 0)
                    s += "[]";

            return s;
        }
    }

    public enum VariableIdType
    {
        Dereference,
        Pointer,
        ArrayAddress,
        AddressOf,
        Struct,
        PureValue
    }

    public class BooleanExpression
    {
        public Expression lhs;
        public string op;
        public Expression rhs;
    }
}
