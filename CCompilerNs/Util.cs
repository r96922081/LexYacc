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
            AsmGenerator.EmitToChannel(asm);
        }

        public static void SaveVariableAddressToRbx(VariableId variableId)
        {
            Variable variable = GetVariableFrom_Local_Param_Global(variableId.name[0]);

            string prevTypeName = null;
            for (int i = 0; i < variableId.name.Count; i++)
            {
                string name = variableId.name[i];
                List<Expression> arrayIndex = variableId.arrayIndexList[i];

                VariableTypeInfo typeInfo = null;
                if (i == 0)
                {
                    typeInfo = variable.typeInfo;
                    prevTypeName = typeInfo.typeName;

                    if (arrayIndex.Count == 0)
                    {
                        // If ID is array, then value is address of array
                        if (typeInfo.arraySize.Count != 0)
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
                                Emit(string.Format("lea {0}(%rip), %rbx", name));
                            }
                        }
                        else if (variable.scope == VariableScopeEnum.global)
                            Emit(string.Format("lea {0}(%rip), %rbx", name));
                        else
                            Emit(string.Format("lea {0}(%rbp), %rbx", variable.stackOffset));
                    }
                    else
                    {
                        for (int j = arrayIndex.Count - 1; j >= 0; j--)
                        {
                            int levelCount = 1;
                            for (int k = j + 1; k < arrayIndex.Count; k++)
                                levelCount *= typeInfo.arraySize[k];

                            arrayIndex[j].EmitAsm();
                            Emit("pop %rax");
                            Emit(string.Format("mov ${0}, %rbx", levelCount));
                            Emit("mul %rbx");
                            Emit(string.Format("mov ${0}, %rbx", typeInfo.size));
                            Emit("mul %rbx");
                            Emit("push %rax");
                        }

                        Emit("movq $0, %rax");
                        for (int j = 0; j < arrayIndex.Count; j++)
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
                else
                {
                    StructDef structDef = GetStructDef(prevTypeName);

                    int offset = 0;
                    for (int j = 0; j < structDef.fields.Count; j++)
                    {
                        if (structDef.fields[j].name == name)
                        {
                            offset = structDef.fields[j].offset;
                            prevTypeName = structDef.fields[j].typeInfo.typeName;
                            break;
                        }
                    }

                    Emit("pop %rbx");
                    Emit(string.Format("add ${0}, %rbx", offset));
                }

                Emit("push %rbx");
            }

            Emit("pop %rbx");
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

        public static StructDef GetStructDef(string name)
        {
            return Gv.context.structDefs[name];
        }

    }
}
