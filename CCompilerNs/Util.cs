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
            else if (type == "char")
            {
                VariableType t = new VariableType();
                t.type = VariableTypeEnum.char_type;
                t.size = 1;
                return t;
            }
            else
                throw new Exception("invalid type: " + type);
        }

        public static void Emit(string asm)
        {
            AsmEmitter.Emit(asm);
        }

        public static void SaveArrayIndexAddressToRbx(LocalVariable l, LocalVariable p, List<Expression> arrayIndex)
        {
            LocalVariable v = l != null ? l : p;

            for (int i = arrayIndex.Count - 1; i >= 0; i--)
            {
                int levelCount = 1;
                for (int j = i + 1; j < arrayIndex.Count; j++)
                    levelCount *= v.arraySize[j];

                arrayIndex[i].EmitAsm();
                Emit("pop %rax");
                Emit(string.Format("mov ${0}, %rbx", levelCount));
                Emit("mul %rbx");
                Emit(string.Format("mov ${0}, %rbx", v.type.size));
                Emit("mul %rbx");
                Emit("push %rax");
            }

            Emit("movq $0, %rax");
            for (int i = 0; i < arrayIndex.Count; i++)
            {
                Emit("pop %rbx");
                Emit("add %rbx, %rax");
            }


            Emit("mov %rbp, %rbx");
            Emit(string.Format("add ${0}, %rbx", v.stackOffset));
            if (p != null)
                Emit("mov (%rbx), %rbx");

            Emit("add %rax, %rbx"); // save address at %rbx
        }
    }
}
