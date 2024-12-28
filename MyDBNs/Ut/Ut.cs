using System.Diagnostics;

namespace MyDBNs
{
    public class Ut
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

        public void CheckError(object result)
        {
            Check(result != null);
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

        public void CheckErrorOrException(Func<object> func)
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

            Check(result != null || hasException);
        }

    }
}
