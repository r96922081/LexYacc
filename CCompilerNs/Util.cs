using System.Diagnostics;

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
                t.SetSize(8);
                return t;
            }
            else if (type == "void")
            {
                VariableTypeInfo t = new VariableTypeInfo();
                t.typeEnum = VariableTypeEnum.void_type;
                t.typeName = type;
                t.SetSize(8);
                return t;
            }
            else if (type == "char")
            {
                VariableTypeInfo t = new VariableTypeInfo();
                t.typeEnum = VariableTypeEnum.char_type;
                t.typeName = type;
                t.SetSize(1);
                return t;
            }
            else if (type.StartsWith("struct "))
            {
                VariableTypeInfo t = new VariableTypeInfo();
                t.typeEnum = VariableTypeEnum.struct_type;
                t.typeName = type.Replace("struct ", "");
                t.SetSize(-1);
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

        private static VariablePartInfo GetVariableTypeInfo(Variable variable, VariableId variableId)
        {
            VariablePartInfo p = new VariablePartInfo();
            p.count = variableId.name.Count;
            p.arrayIndexList = variableId.arrayIndexList;

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
                        found = true;
                        break;
                    }
                }

                if (!found)
                    throw new Exception();
            }


            return p;
        }

        private static void PushBaseAddress(string name)
        {
            Variable variable = GetVariableFrom_Local_Param_Global(name);

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
                if (variable.typeInfo.arraySize.Count != 0)
                    Emit("mov (%rbx), %rbx");
                Emit(string.Format("push %rbx", variable.name));
            }
        }



        public static VariableAddressOrValue PushVariableAddress(VariableId variableId)
        {
            Variable variable = GetVariableFrom_Local_Param_Global(variableId.name[0]);
            VariablePartInfo partInfo = GetVariableTypeInfo(variable, variableId);

            PushBaseAddress(variableId.name[0]);

            for (int i = 0; i < partInfo.count; i++)
            {
                if (i > 0)
                {
                    Emit("pop %rbx");
                    Emit(string.Format("add ${0}, %rbx", partInfo.offsets[i]));
                    Emit("push %rbx");
                }

                // [2][3][4] => 2,3,4
                List<Expression> arrayIndexList = partInfo.arrayIndexList[i];
                List<int> arraySizeList = partInfo.type[i].arraySize;

                if (arrayIndexList.Count != 0)
                {
                    for (int j = arrayIndexList.Count - 1; j >= 0; j--)
                    {
                        int levelCount = 1;
                        for (int k = j + 1; k < arraySizeList.Count; k++)
                            levelCount *= arraySizeList[k];

                        arrayIndexList[j].EmitAsm();
                        Emit("pop %rax");
                        Emit(string.Format("mov ${0}, %rcx", levelCount));
                        Emit("mul %rcx");
                        Emit(string.Format("mov ${0}, %rcx", partInfo.type[i].GetSize()));
                        Emit("mul %rcx");
                        Emit("push %rax");
                    }

                    Emit("movq $0, %rax");
                    for (int j = 0; j < partInfo.arrayIndexList[i].Count; j++)
                    {
                        Emit("pop %rcx");
                        Emit("add %rcx, %rax");
                    }

                    Emit("pop %rbx");
                    Emit("add %rax, %rbx");
                    Emit("push %rbx");
                }
            }

            if (partInfo.type[partInfo.count - 1].arraySize.Count != partInfo.arrayIndexList[partInfo.count - 1].Count
                || partInfo.type[0].typeEnum == VariableTypeEnum.struct_type && variableId.name.Count == 1)
                return VariableAddressOrValue.Address;
            else
                return VariableAddressOrValue.Value;

        }

        public static Variable GetVariableFrom_Local_Param_Global(string name)
        {
            if (Gv.program.functionDeclare.localMap.ContainsKey(name))
            {
                return Gv.program.functionDeclare.localMap[name];
            }
            else if (Gv.program.functionDeclare.paramMap.ContainsKey(name))
            {
                return Gv.program.functionDeclare.paramMap[name];
            }
            else if (Gv.program.gv.ContainsKey(name))
            {
                return Gv.program.gv[name];
            }
            else
            {
                return null;
            }
        }

        public static StructDef GetStructDef(string name)
        {
            return Gv.program.structDefs[name];
        }

        public static string GetCallStack()
        {
            var stackTrace = new StackTrace();
            var frames = stackTrace.GetFrames();

            if (frames == null)
                return string.Empty;

            var methodNames = frames
                .Select(frame => frame.GetMethod()?.Name)
                .Where(name => !string.IsNullOrEmpty(name) && name != nameof(GetCallStack))
                .Reverse()
                .Select(name => $"{name}()");

            return string.Join(" -> ", methodNames);
        }

    }
}
