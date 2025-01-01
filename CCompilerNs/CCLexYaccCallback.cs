namespace CCompilerNs
{
    public class CCLexYaccCallback
    {
        public static Program Program(FunDecl funcDecl)
        {
            Program p = new Program();
            p.children.Add(funcDecl);

            return p;
        }

        public static FunDecl FuncDecl(string returnType, string functionName, Statement statement)
        {
            FunDecl f = new FunDecl();
            if (returnType == "int")
            {
                f.returnType = VariableType.int_type;
            }

            f.functionName = functionName;
            f.children.Add(statement);

            return f;
        }

        public static ReturnStatement ReturnStatement(object returnValue)
        {
            ReturnStatement n = new ReturnStatement();

            if (returnValue is int)
            {
                n.returnValue = returnValue;
                n.returnType = VariableType.int_type;
            }

            return n;
        }
    }
}
