namespace MyDBNs
{
    public class SqlConditionLexYaccCallback
    {
        public static string tableName = "";

        public static void VerifyBooleanExpression(string lhs, string op, string rhs)
        {
            List<Table> tables = MyDBNs.DB.tables;
            Table table = MyDBNs.DB.GetTable(tableName);
            if (table == null)
                throw new Exception("Table does not exist: " + tableName);

            StringType lhsType = DBUtil.GetStringType(lhs);
            StringType rhsType = DBUtil.GetStringType(rhs);

            StringType lhsType2 = lhsType;
            if (lhsType2 == StringType.Column)
            {
                ColumnType t = table.columnNameToTypesMap[lhs.ToUpper()];
                if (t == ColumnType.NUMBER)
                    lhsType2 = StringType.Number;
                else
                    lhsType2 = StringType.String;
            }

            StringType rhsType2 = rhsType;
            if (rhsType2 == StringType.Column)
            {
                ColumnType t = table.columnNameToTypesMap[rhs.ToUpper()];
                if (t == ColumnType.NUMBER)
                    rhsType2 = StringType.Number;
                else
                    rhsType2 = StringType.String;
            }

            if (lhsType2 != rhsType2)
                throw new Exception(lhs + " & " + rhs + " have different type");
        }

        public static HashSet<int> BooleanExpression(string lhs, string op, string rhs)
        {
            VerifyBooleanExpression(lhs, op, rhs);

            HashSet<int> rows = new HashSet<int>();

            List<Table> tables = MyDBNs.DB.tables;
            Table table = MyDBNs.DB.GetTable(tableName);

            StringType lhsType = DBUtil.GetStringType(lhs);
            StringType rhsType = DBUtil.GetStringType(rhs);
            int lhsColumnIndex = -1;
            int rhsColumnIndex = -1;

            StringType lhsType2 = lhsType;
            if (lhsType2 == StringType.Column)
            {
                ColumnType t = table.columnNameToTypesMap[lhs.ToUpper()];
                if (t == ColumnType.NUMBER)
                    lhsType2 = StringType.Number;
                else
                    lhsType2 = StringType.String;

                lhsColumnIndex = table.columnNameToIndexMap[lhs.ToUpper()];
            }

            StringType rhsType2 = rhsType;
            if (rhsType2 == StringType.Column)
            {
                ColumnType t = table.columnNameToTypesMap[rhs.ToUpper()];
                if (t == ColumnType.NUMBER)
                    rhsType2 = StringType.Number;
                else
                    rhsType2 = StringType.String;

                rhsColumnIndex = table.columnNameToIndexMap[rhs.ToUpper()];
            }


            for (int i = 0; i < table.rows.Count; i++)
            {
                object[] row = table.rows[i];

                object lhsValue = null;
                object rhsValue = null;

                if (lhsType == StringType.Column)
                {
                    lhsValue = row[lhsColumnIndex];
                }
                else if (lhsType2 == StringType.Number)
                {
                    if (lhs != null)
                        lhsValue = double.Parse(lhs);
                }
                else if (lhsType2 == StringType.String)
                {
                    if (lhs != null)
                        lhsValue = lhs.Substring(1, lhs.Length - 2);
                }

                if (rhsType == StringType.Column)
                {
                    rhsValue = row[rhsColumnIndex];
                }
                else if (rhsType2 == StringType.Number)
                {
                    if (rhs != null)
                        rhsValue = double.Parse(rhs);
                }
                else if (rhsType2 == StringType.String)
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
                            if (lhsType2 == StringType.String)
                            {
                                if (((string)lhsValue).CompareTo(rhsValue) == 0)
                                    rows.Add(i);
                            }
                            else if (lhsType2 == StringType.Number)
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
                            if (lhsType2 == StringType.String)
                            {
                                if (((string)lhsValue).CompareTo(rhsValue) != 0)
                                    rows.Add(i);
                            }
                            else if (lhsType2 == StringType.Number)
                            {
                                if (((double)lhsValue).CompareTo(rhsValue) != 0)
                                    rows.Add(i);
                            }
                        }
                        break;
                    case "<":
                        if (lhsValue != null && rhsValue != null)
                        {
                            if (lhsType2 == StringType.String)
                            {
                                if (((string)lhsValue).CompareTo(rhsValue) < 0)
                                    rows.Add(i);
                            }
                            else if (lhsType2 == StringType.Number)
                            {
                                if (((double)lhsValue).CompareTo(rhsValue) < 0)
                                    rows.Add(i);
                            }
                        }
                        break;
                    case "<=":
                        if (lhsValue != null && rhsValue != null)
                        {
                            if (lhsType2 == StringType.String)
                            {
                                if (((string)lhsValue).CompareTo(rhsValue) <= 0)
                                    rows.Add(i);
                            }
                            else if (lhsType2 == StringType.Number)
                            {
                                if (((double)lhsValue).CompareTo(rhsValue) <= 0)
                                    rows.Add(i);
                            }
                        }
                        break;
                    case ">":
                        if (lhsValue != null && rhsValue != null)
                        {
                            if (lhsType2 == StringType.String)
                            {
                                if (((string)lhsValue).CompareTo(rhsValue) > 0)
                                    rows.Add(i);
                            }
                            else if (lhsType2 == StringType.Number)
                            {
                                if (((double)lhsValue).CompareTo(rhsValue) > 0)
                                    rows.Add(i);
                            }
                        }
                        break;
                    case ">=":
                        if (lhsValue != null && rhsValue != null)
                        {
                            if (lhsType2 == StringType.String)
                            {
                                if (((string)lhsValue).CompareTo(rhsValue) >= 0)
                                    rows.Add(i);
                            }
                            else if (lhsType2 == StringType.Number)
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
