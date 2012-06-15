using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JsXmlDocParser
{
    public class JsParser
    {
        private static readonly Regex FunctionExpression = new Regex(@"function(\s+\w+\s*|\s*)\(");
        private static readonly Regex CommentExpression = new Regex("$[ \t]*//");

        /// <summary>
        /// Parse a JS block.
        /// </summary>
        /// <param name="reader">Text Stream containing valid JavaScript</param>
        /// <param name="writer"> </param>
        /// <returns>Parse Results as XML string</returns>
        public void Parse(TextReader reader, MemberInfoWriter writer)
        {
            var functions = ReadFunctionBlocks(reader);
            var functionInfos = functions.Select(function => new FunctionInfo(function));

            foreach (var functionInfo in functionInfos)
            {
                writer.Write(functionInfo);
            }
        }

        internal static List<string> ReadFunctionBlocks(TextReader tr, string startLine = null)
        {
            var functionBuilder = new StringBuilder();
            string line;
            var functionStarted = false;
            var openBraces = 0;
            var functionBlocks = new List<string>();

            if (!string.IsNullOrEmpty(startLine))
            {
                functionStarted = true;
                openBraces = CountBraces(startLine);
                functionBuilder.AppendLine(startLine);
            }

            while ((line = tr.ReadLine()) != null)
            {
                line = line.Trim();

                if (FunctionExpression.IsMatch(line))
                {
                    if (functionStarted)
                    {
                        // function within function, read inner function first
                        functionBlocks.AddRange(ReadFunctionBlocks(tr, line));
                        continue;
                    }

                    functionStarted = line.Contains("{");
                }
                else if (!functionStarted)
                    continue;	// skip all comments, variable declaration at the beginning of file


                if (!string.IsNullOrEmpty(line))
                    functionBuilder.AppendLine(line);

                if (!CommentExpression.IsMatch(line))
                {
                    openBraces += CountBraces(line);

                    if (functionStarted && openBraces == 0)
                    {
                        functionBlocks.Add(functionBuilder.ToString());
                        break;
                    }
                }
            }

            return functionBlocks;
        }

        private static int CountBraces(string input)
        {
            var chars = input.ToCharArray();
            return chars.Count(x => x == '{') - chars.Count(x => x == '}');
        }
    }
}