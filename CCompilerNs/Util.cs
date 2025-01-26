using System.Diagnostics;

namespace CCompilerNs
{
    public class Util
    {
        public static VariableTypeInfo GetType(string type)
        {
            VariableTypeInfo t = new VariableTypeInfo();
            t.SetSize(-1);

            int pointerCount = type.Count(f => f == '*');
            type = type.Replace("*", "");
            t.pointerCount = pointerCount;

            if (type == "int")
            {
                t.typeEnum = VariableTypeEnum.int_type;
                t.typeName = type;
                t.SetSize(8);
                return t;
            }
            else if (type == "void")
            {
                t.typeEnum = VariableTypeEnum.void_type;
                t.typeName = type;
                t.SetSize(8);
                return t;
            }
            else if (type == "char")
            {
                t.typeEnum = VariableTypeEnum.char_type;
                t.typeName = type;
                t.SetSize(1);
                return t;
            }
            else if (type.StartsWith("struct "))
            {
                t.typeEnum = VariableTypeEnum.struct_type;
                t.typeName = type.Replace("struct ", "");
                return t;
            }
            else
                throw new Exception("invalid type: " + type);
        }

        public static void Emit(string asm)
        {
            AsmGenerator.EmitToChannel(asm);
        }

        public static VariablePartInfo GetVariableTypeInfo(Variable variable, VariableId variableId)
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

        private static void PushBaseAddress(VariableId variableId)
        {
            string name = variableId.name[0];
            Variable variable = GetVariableFrom_Local_Param_Global(name);

            if (variable.scope == VariableScopeEnum.global)
            {
                Emit(string.Format("lea {0}(%rip), %rbx # global, {1}", variable.name, variableId.ToString()));
                Emit(string.Format("push %rbx", variable.name));
            }
            else if (variable.scope == VariableScopeEnum.local)
            {
                Emit(string.Format("lea {0}(%rbp), %rbx # local, {1}", variable.stackOffset, variableId.ToString()));
                Emit(string.Format("push %rbx", variable.name));
            }
            else if (variable.scope == VariableScopeEnum.param)
            {
                Emit(string.Format("lea {0}(%rbp), %rbx # param, {1}", variable.stackOffset, variableId.ToString()));

                // if pass array as param, then it's address, need to dereference
                if (variable.typeInfo.arraySize.Count != 0)
                    Emit("mov (%rbx), %rbx");
                else if (variable.typeInfo.pointerCount != 0)
                    throw new NotImplementedException();
                // struct is copy to local, and offset is adjusted in CopyParamStruct
                else if (variable.typeInfo.typeEnum == VariableTypeEnum.struct_type)
                    Emit("mov %rbx, %rbx");
                Emit(string.Format("push %rbx", variable.name));
            }
        }



        public static void PushVariableAddress(VariableId variableId)
        {
            Variable variable = GetVariableFrom_Local_Param_Global(variableId.name[0]);
            VariablePartInfo partInfo = GetVariableTypeInfo(variable, variableId);

            PushBaseAddress(variableId);

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


        public static void SetVaraibleIdLhsRhsType(Expression e, VariableIdLhsRhsType type)
        {
            if (e is VariableIdExpression)
                ((VariableIdExpression)e).variableId.lhsRhs = type;
        }
    }
}
