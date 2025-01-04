namespace CCompilerNs
{
    public class Util
    {
        public static VariableType GetType(string type)
        {
            if (type == "int")
            {
                VariableType t = new VariableType();
                t.type = VariableTypeEnum.int_type;
                t.size = 8;
                return t;
            }
            else if (type == "void")
            {
                VariableType t = new VariableType();
                t.type = VariableTypeEnum.void_type;
                t.size = 8;
                return t;
            }
            else
                throw new Exception("invalid type: " + type);
        }
    }
}
