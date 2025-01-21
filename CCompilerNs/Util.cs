namespace CCompilerNs
{
    public class Util
    {
        public static VariableTypeInfo GetType(string type)
        {
            if (type == "int")
            {
                VariableTypeInfo t = new VariableTypeInfo();
                t.typeEnum = VariableTypeEnum.int_type;
                t.typeName = type;
                t.size = 8;
                return t;
            }
            else if (type == "void")
            {
                VariableTypeInfo t = new VariableTypeInfo();
                t.typeEnum = VariableTypeEnum.void_type;
                t.typeName = type;
                t.size = 8;
                return t;
            }
            else if (type == "char")
            {
                VariableTypeInfo t = new VariableTypeInfo();
                t.typeEnum = VariableTypeEnum.char_type;
                t.typeName = type;
                t.size = 1;
                return t;
            }
            else if (type.StartsWith("struct "))
            {
                VariableTypeInfo t = new VariableTypeInfo();
                t.typeEnum = VariableTypeEnum.struct_type;
                t.typeName = type.Replace("struct ", "");
                t.size = -1;
                return t;
            }
            else
                throw new Exception("invalid type: " + type);
        }

        public static void Emit(string asm)
        {
            AsmEmitter.Emit(asm);
        }

        public static void SaveArrayIndexAddressToRbx(Variable v, List<Expression> arrayIndex)
        {
            for (int i = arrayIndex.Count - 1; i >= 0; i--)
            {
                int levelCount = 1;
                for (int j = i + 1; j < arrayIndex.Count; j++)
                    levelCount *= v.typeInfo.arraySize[j];

                arrayIndex[i].EmitAsm();
                Emit("pop %rax");
                Emit(string.Format("mov ${0}, %rbx", levelCount));
                Emit("mul %rbx");
                Emit(string.Format("mov ${0}, %rbx", v.typeInfo.size));
                Emit("mul %rbx");
                Emit("push %rax");
            }

            Emit("movq $0, %rax");
            for (int i = 0; i < arrayIndex.Count; i++)
            {
                Emit("pop %rbx");
                Emit("add %rbx, %rax");
            }

            if (v.scope == VariableScopeEnum.global)
            {
                Emit(string.Format("lea {0}(%rip), %rbx", v.name));
            }
            else
            {
                Emit("mov %rbp, %rbx");
                Emit(string.Format("add ${0}, %rbx", v.stackOffset));
                if (v.scope == VariableScopeEnum.param)
                    Emit("mov (%rbx), %rbx");
            }


            Emit("add %rax, %rbx"); // save address at %rbx
        }

        public static Variable GetVariable(string name)
        {
            if (Gv.context.functionDeclare.localMap.ContainsKey(name))
            {
                return Gv.context.functionDeclare.localMap[name];
            }
            else if (Gv.context.functionDeclare.paramMap.ContainsKey(name))
            {
                return Gv.context.functionDeclare.paramMap[name];
            }
            else if (Gv.context.gv.ContainsKey(name))
            {
                return Gv.context.gv[name];
            }
            else
            {
                return null;
            }
        }

    }
}
