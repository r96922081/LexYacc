namespace CCompilerNs
{

    public class NodeString
    {
        public string S { get; set; }
        public int XPos { get; set; }
    }

    public class AstNode
    {
        public string s { get; set; }
        public List<AstNode> children { get; private set; }

        public AstNode()
        {
            this.s = "";
            children = new List<AstNode>();
        }

        public AstNode(string s)
        {
            this.s = s;
            children = new List<AstNode>();
        }

        public string GetPrintString()
        {
            var nodeStringByLevel = new List<List<NodeString>>();

            GetTreeString(0, 0, nodeStringByLevel);

            string result = "";

            foreach (var level in nodeStringByLevel)
            {
                int pos = 0;
                foreach (var nodeString in level)
                {
                    while (pos < nodeString.XPos)
                    {
                        result += " ";
                        pos++;
                    }

                    result += nodeString.S;
                    pos += nodeString.S.Length;
                }
                result += "\n";
            }

            return result;
        }

        public void Print()
        {
            Console.Write(GetPrintString());
        }

        public void EmitChildrenAsm()
        {
            foreach (var child in children)
                child.EmitAsm();
        }

        public virtual void EmitCurrentAsm()
        {

        }

        public virtual void EmitAsm()
        {
            EmitCurrentAsm();
            EmitChildrenAsm();
        }

        public void Emit(string asm)
        {
            AsmEmitter.Emit(asm);
        }

        private int GetTreeString(int level, int xPos, List<List<NodeString>> nodeStringByLevel)
        {
            if (level == nodeStringByLevel.Count)
                nodeStringByLevel.Add(new List<NodeString>());

            var s = new NodeString
            {
                S = GetString(),
                XPos = xPos
            };
            nodeStringByLevel[level].Add(s);

            int childMaxXPos = xPos;
            foreach (var child in children)
            {
                childMaxXPos = child.GetTreeString(level + 1, childMaxXPos, nodeStringByLevel);
            }

            return Math.Max(xPos + s.S.Length, childMaxXPos);
        }

        private string GetString()
        {
            return s + "  ";
        }
    }
}