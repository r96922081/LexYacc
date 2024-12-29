﻿using System.Diagnostics;

namespace MyDBNs
{
    public class BaseUt
    {
        public void Check(bool b)
        {
            if (!b)
                Trace.Assert(false);
        }

        public void CheckOk(object result)
        {
            Check(result == null || (string)result == "");
        }

        public List<object[]> RunSelectStatementAndConvertResult(string s)
        {
            return Util.GetSelectRows((SelectedData)sql_statements.Parse(s));
        }

        public void CheckException(Func<object> func)
        {
            bool hasException = false;
            try
            {
                func();
            }
            catch
            {
                hasException = true;
            }

            Check(hasException);
        }

        public void CheckSyntaxErrorOrException(Func<object> func)
        {
            bool hasException = false;
            object result = null;
            try
            {
                result = func();
            }
            catch
            {
                hasException = true;
            }

            Check(hasException || (result is string && ((string)result).ToLower() == "syntax error"));
        }

    }
}