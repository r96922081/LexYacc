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

        public static void SaveVariableAddressToRbx(Variable variable, VariableId variableId)
        {
            if (variableId.arrayIndex.Count == 0)
            {
                // If ID is array, then value is address of array
                if (variable.typeInfo.arraySize.Count != 0)
                {
                    if (variable.scope == VariableScopeEnum.local)
                    {
                        Emit(string.Format("mov %rbp, %rbx"));
                        Emit(string.Format("add ${0}, %rbx", variable.stackOffset));
                    }
                    else if (variable.scope == VariableScopeEnum.param)
                    {
                        Emit(string.Format("mov %rbp, %rbx"));
                        Emit(string.Format("add ${0}, %rbx", variable.stackOffset));
                        Emit(string.Format("mov (%rbx), %rbx"));
                    }
                    else if (variable.scope == VariableScopeEnum.global)
                    {
                        Emit(string.Format("lea {0}(%rip), %rbx", variableId.name));
                    }
                }
                else if (variable.scope == VariableScopeEnum.global)
                    Emit(string.Format("lea {0}(%rip), %rbx", variableId.name));
                else
                    Emit(string.Format("lea {0}(%rbp), %rbx", variable.stackOffset));
            }
            else
            {
                for (int i = variableId.arrayIndex.Count - 1; i >= 0; i--)
                {
                    int levelCount = 1;
                    for (int j = i + 1; j < variableId.arrayIndex.Count; j++)
                        levelCount *= variable.typeInfo.arraySize[j];

                    variableId.arrayIndex[i].EmitAsm();
                    Emit("pop %rax");
                    Emit(string.Format("mov ${0}, %rbx", levelCount));
                    Emit("mul %rbx");
                    Emit(string.Format("mov ${0}, %rbx", variable.typeInfo.size));
                    Emit("mul %rbx");
                    Emit("push %rax");
                }

                Emit("movq $0, %rax");
                for (int i = 0; i < variableId.arrayIndex.Count; i++)
                {
                    Emit("pop %rbx");
                    Emit("add %rbx, %rax");
                }

                if (variable.scope == VariableScopeEnum.global)
                {
                    Emit(string.Format("lea {0}(%rip), %rbx", variable.name));
                }
                else
                {
                    Emit("mov %rbp, %rbx");
                    Emit(string.Format("add ${0}, %rbx", variable.stackOffset));
                    if (variable.scope == VariableScopeEnum.param)
                        Emit("mov (%rbx), %rbx");
                }

                Emit("add %rax, %rbx");
            }
        }

        public static Variable GetVariableFrom_Local_Param_Global(string name)
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
