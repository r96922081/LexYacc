namespace CCompilerNs
{
    public class VariableType
    {
        public VariableTypeEnum type;
        public int size;
    }

    public enum VariableTypeEnum
    {
        void_type,
        int_type
    }

    public class LocalVariable
    {
        public string name;
        public VariableType type;
        public int stackOffset = 0;
    }

}
