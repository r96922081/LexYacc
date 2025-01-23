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

        private class VariablePartInfo
        {
            public int count;
            public List<VariableTypeInfo> type = new List<VariableTypeInfo>();
            public List<int> offsets = new List<int>();
            public List<List<Expression>> arrayIndexList = new List<List<Expression>>();
        }

        private static VariablePartInfo GetVariableTypeInfo(VariableId variableId)
        {
            VariablePartInfo p = new VariablePartInfo();
            p.count = variableId.name.Count;
            p.arrayIndexList = variableId.arrayIndexList;

            Variable variable = GetVariableFrom_Local_Param_Global(variableId.name[0]);
            if (variable.typeInfo.typeEnum != VariableTypeEnum.struct_type)
                return p;

            p.type.Add(variable.typeInfo);
            p.offsets.Add(0);

            for (int i = 1; i < p.count; i++)
            {
                StructDef structDef = GetStructDef(p.type[i - 1].typeName);
                bool found = false;
                for (int j = 0; j < structDef.fields.Count; j++)
                {
                    if (variableId.name[i] == structDef.fields[j].name)
                    {
                        p.offsets.Add(structDef.fields[j].offset);
                        p.type.Add(structDef.fields[j].typeInfo);

                        if (i != p.count - 1) // a.b.c, last name c is not struct
                            structDef = GetStructDef(structDef.fields[j].typeInfo.typeName);
                        found = true;
                        break;
                    }
                }

                if (!found)
                    throw new Exception();
            }


            return p;
        }

        // the case ID is array, then value is address of array
        // ex: 
        // int a[1][2];
        // f1(a);  // a is the address of array
        private static bool IsArrayAddressVariable(VariablePartInfo partInfo, Variable variable)
        {
            return partInfo.count == 1 && variable.typeInfo.arraySize.Count != 0 && partInfo.arrayIndexList[0].Count == 0;
        }

        private static void PushBaseAddress(Variable variable, VariablePartInfo partInfo)
        {
            if (variable.scope == VariableScopeEnum.global)
            {
                Emit(string.Format("lea {0}(%rip), %rbx", variable.name));
                Emit(string.Format("push %rbx", variable.name));
            }
            else if (variable.scope == VariableScopeEnum.local)
            {
                Emit(string.Format("lea {0}(%rbp), %rbx", variable.stackOffset));
                Emit(string.Format("push %rbx", variable.name));
            }
            else if (variable.scope == VariableScopeEnum.param)
            {
                Emit(string.Format("lea {0}(%rbp), %rbx", variable.stackOffset));

                // if pass array as param, then it's address, need to dereference
                if (partInfo.arrayIndexList[0].Count != 0)
                    Emit("mov (%rbx), %rbx");
                Emit(string.Format("push %rbx", variable.name));
            }
        }

        public static void PushVariableAddress(VariableId variableId)
        {
            VariablePartInfo partInfo = GetVariableTypeInfo(variableId);
            Variable variable = GetVariableFrom_Local_Param_Global(variableId.name[0]);

            PushBaseAddress(variable, partInfo);

            if (IsArrayAddressVariable(partInfo, variable))
                return;


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
                        Emit(string.Format("pop %rbx"));
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
                            Emit(string.Format("mov ${0}, %rcx", levelCount));
                            Emit("mul %rcx");
                            Emit(string.Format("mov ${0}, %rcx", typeInfo.size));
                            Emit("mul %rcx");
                            Emit("push %rax");
                        }

                        Emit("movq $0, %rax");
                        for (int j = 0; j < arrayIndex.Count; j++)
                        {
                            Emit("pop %rcx");
                            Emit("add %rcx, %rax");
                        }

                        Emit("pop %rbx");
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
