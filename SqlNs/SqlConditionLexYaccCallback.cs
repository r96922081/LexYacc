namespace SqlNs
{
    public class SqlConditionLexYaccCallback
    {
        public enum OpType
        {
            Column,
            String,
            Number
        }

        public static string tableName = "";

        private static OpType GetOpType(string s)
        {
            if (s.StartsWith("'") && s.EndsWith("'"))
                return OpType.String;

            double n = 0;
            bool ret = double.TryParse(s, out n);

            if (ret)
                return OpType.Number;

            return OpType.Column;
        }

        public static void VerifyBooleanExpression(string lhs, string op, string rhs)
        {
            List<Table> tables = SqlNs.DB.tables;
            Table table = DB.tables.FirstOrDefault(t => t.tableName == tableName);
            if (table == null)
                throw new Exception("Table does not exist: " + tableName);

            OpType lhsType = GetOpType(lhs);
            OpType rhsType = GetOpType(rhs);

            OpType lhsType2 = lhsType;
            if (lhsType2 == OpType.Column)
            {
                ColumnType t = table.columnTypesMap[lhs];
                if (t == ColumnType.NUMBER)
                    lhsType2 = OpType.Number;
                else
                    lhsType2 = OpType.String;
            }

            OpType rhsType2 = rhsType;
            if (rhsType2 == OpType.Column)
            {
                ColumnType t = table.columnTypesMap[rhs];
                if (t == ColumnType.NUMBER)
                    rhsType2 = OpType.Number;
                else
                    rhsType2 = OpType.String;
            }

            if (lhsType2 != rhsType2)
                throw new Exception(lhs + " & " + rhs + " have different type");
        }

        public static HashSet<int> BooleanExpression(string lhs, string op, string rhs)
        {
            VerifyBooleanExpression(lhs, op, rhs);

            HashSet<int> rows = new HashSet<int>();

            List<Table> tables = SqlNs.DB.tables;
            Table table = DB.tables.FirstOrDefault(t => t.tableName == tableName);

            OpType lhsType = GetOpType(lhs);
            OpType rhsType = GetOpType(rhs);
            int lhsColumnIndex = -1;
            int rhsColumnIndex = -1;

            OpType lhsType2 = lhsType;
            if (lhsType2 == OpType.Column)
            {
                ColumnType t = table.columnTypesMap[lhs];
                if (t == ColumnType.NUMBER)
                    lhsType2 = OpType.Number;
                else
                    lhsType2 = OpType.String;

                lhsColumnIndex = table.columnIndexMap[lhs];
            }

            OpType rhsType2 = rhsType;
            if (rhsType2 == OpType.Column)
            {
                ColumnType t = table.columnTypesMap[rhs];
                if (t == ColumnType.NUMBER)
                    rhsType2 = OpType.Number;
                else
                    rhsType2 = OpType.String;

                rhsColumnIndex = table.columnIndexMap[rhs];
            }


            for (int i = 0; i < table.rows.Count; i++)
            {
                object[] row = table.rows[i];

                object lhsValue = null;
                object rhsValue = null;

                if (lhsType == OpType.Column)
                {
                    lhsValue = row[lhsColumnIndex];
                }
                else if (lhsType2 == OpType.Number)
                {
                    if (lhs != null)
                        lhsValue = double.Parse(lhs);
                }
                else if (lhsType2 == OpType.String)
                {
                    if (lhs != null)
                        lhsValue = lhs.Substring(1, lhs.Length - 2);
                }

                if (rhsType == OpType.Column)
                {
                    rhsValue = row[rhsColumnIndex];
                }
                else if (rhsType2 == OpType.Number)
                {
                    if (rhs != null)
                        rhsValue = double.Parse(rhs);
                }
                else if (rhsType2 == OpType.String)
                {
                    if (rhs != null)
                        rhsValue = rhs.Substring(1, rhs.Length - 2);
                }

                switch (op)
                {
                    case "=":
                        if (lhsValue == null || rhsValue == null)
                        {
                            if (lhsValue == rhsValue)
                                rows.Add(i);
                        }
                        else
                        {
                            if (lhsType2 == OpType.String)
                            {
                                if (((string)lhsValue).CompareTo(rhsValue) == 0)
                                    rows.Add(i);
                            }
                            else if (lhsType2 == OpType.Number)
                            {
                                if (((double)lhsValue).CompareTo(rhsValue) == 0)
                                    rows.Add(i);
                            }
                        }
                        break;
                    case "!=":
                        if (lhsValue == null || rhsValue == null)
                        {
                            if (lhsValue != rhsValue)
                                rows.Add(i);
                        }
                        else
                        {
                            if (lhsType2 == OpType.String)
                            {
                                if (((string)lhsValue).CompareTo(rhsValue) != 0)
                                    rows.Add(i);
                            }
                            else if (lhsType2 == OpType.Number)
                            {
                                if (((double)lhsValue).CompareTo(rhsValue) != 0)
                                    rows.Add(i);
                            }
                        }
                        break;
                    case "<":
                        if (lhsValue != null && rhsValue != null)
                        {
                            if (lhsType2 == OpType.String)
                            {
                                if (((string)lhsValue).CompareTo(rhsValue) < 0)
                                    rows.Add(i);
                            }
                            else if (lhsType2 == OpType.Number)
                            {
                                if (((double)lhsValue).CompareTo(rhsValue) < 0)
                                    rows.Add(i);
                            }
                        }
                        break;
                    case "<=":
                        if (lhsValue != null && rhsValue != null)
                        {
                            if (lhsType2 == OpType.String)
                            {
                                if (((string)lhsValue).CompareTo(rhsValue) <= 0)
                                    rows.Add(i);
                            }
                            else if (lhsType2 == OpType.Number)
                            {
                                if (((double)lhsValue).CompareTo(rhsValue) <= 0)
                                    rows.Add(i);
                            }
                        }
                        break;
                    case ">":
                        if (lhsValue != null && rhsValue != null)
                        {
                            if (lhsType2 == OpType.String)
                            {
                                if (((string)lhsValue).CompareTo(rhsValue) > 0)
                                    rows.Add(i);
                            }
                            else if (lhsType2 == OpType.Number)
                            {
                                if (((double)lhsValue).CompareTo(rhsValue) > 0)
                                    rows.Add(i);
                            }
                        }
                        break;
                    case ">=":
                        if (lhsValue != null && rhsValue != null)
                        {
                            if (lhsType2 == OpType.String)
                            {
                                if (((string)lhsValue).CompareTo(rhsValue) >= 0)
                                    rows.Add(i);
                            }
                            else if (lhsType2 == OpType.Number)
                            {
                                if (((double)lhsValue).CompareTo(rhsValue) >= 0)
                                    rows.Add(i);
                            }
                        }
                        break;
                }
            }

            return rows;
        }
    }
}
