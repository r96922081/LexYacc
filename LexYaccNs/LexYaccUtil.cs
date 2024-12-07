using System.Text;

namespace LexYaccNs
{
    public class LexYaccUtil
    {
        public static int FindCharNotInLiteral(string s, char c, bool includeSingleQuote)
        {
            return FindCharNotInLiteral(s, new List<char>() { c }, includeSingleQuote);
        }

        public static int FindCharNotInLiteral(string s, List<char> chars, bool includeSingleQuote)
        {
            List<string> stringList = new List<string>();

            foreach (char c in chars)
                stringList.Add(c.ToString());

            return FindStringNotInLiteral(s, stringList, includeSingleQuote);
        }

        public static int FindStringNotInLiteral(string s, string s2, bool includeSingleQuote)
        {
            return FindStringNotInLiteral(s, new List<string>() { s2 }, includeSingleQuote);
        }

        public static int FindStringNotInLiteral(string s, List<string> strings, bool includeSingleQuote)
        {
            bool singleQuote = false;
            bool doubleQuote = false;
            bool comment = false;

            for (int i = 0; i < s.Length; i++)
            {
                if (includeSingleQuote && s[i] == '\'')
                    singleQuote = !singleQuote;
                else if (s[i] == '"')
                    doubleQuote = !doubleQuote;
                else if (s[i] == '\n')
                    comment = false;
                else if (s[i] == '/' && i < s.Length - 1 && s[i + 1] == '/')
                    comment = true;

                if (!singleQuote && !doubleQuote && !comment)
                {
                    foreach (string s2 in strings)
                    {
                        if (i + s2.Length > s.Length)
                            continue;

                        bool found = true;
                        for (int j = 0; j < s2.Length; j++)
                        {
                            if (s[i + j] != s2[j])
                            {
                                found = false;
                                break;
                            }
                        }

                        if (found)
                            return i;
                    }
                }

            }

            return -1;
        }

        public static string GetGenFileFolder()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "../../../GenFile");
        }

        public static string FixGenCodeIndention(string input, string indention)
        {
            StringBuilder sb = new StringBuilder();

            int minIndentCount = int.MaxValue;
            using (StringReader reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int indentCount = 0;
                    for (; indentCount < line.Length; indentCount++)
                        if (line[indentCount] != ' ')
                            break;
                    if (indentCount < minIndentCount)
                        minIndentCount = indentCount;
                }
            }
            using (StringReader reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    sb.AppendLine(indention + line.Substring(minIndentCount));
                }
            }

            return sb.ToString();
        }

        public static string RemoveHeadAndTailEmptyLine(string input)
        {
            string[] lines = input.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            StringBuilder sb = new StringBuilder();

            int start = 0;
            for (; start < lines.Length && lines[start].Trim().Length == 0; start++)
                ;

            int end = lines.Length - 1;
            for (; end >= 0 && lines[end].Trim().Length == 0; end--)
                ;

            for (int i = start; i < lines.Length && i <= end; i++)
                sb.Append(lines[i] + "\n");

            return sb.ToString();
        }
    }



    public class SectionSplitter
    {

    }
}