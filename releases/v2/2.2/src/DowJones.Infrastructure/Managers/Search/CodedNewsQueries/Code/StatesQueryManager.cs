using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;

namespace DowJones.Managers.Search.CodedNewsQueries.Code
{
    /// <summary>
    /// Summary description for StatesQueryManager.
    /// </summary>
    public class StatesQueryManager
    {
        private static readonly string _US_STATES_FILE = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + @"\USStatesAbbreviations.xml";
        private static readonly object syncLock = new object();
        private static SortedList abbreviations = new SortedList();
        private static StatesQueryManager instance;

        private StatesQueryManager()
        {
            abbreviations = ReadStateXml(_US_STATES_FILE); //Load state xml
        }

        public static StatesQueryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncLock)
                    {
                        if (instance == null)
                        {
                            instance = new StatesQueryManager();
                        }
                    }
                }
                return instance;
            }
        }

        public static string GetStateQuery(string state)
        {
            if (abbreviations.ContainsKey(state.ToUpper()))
                return (string) abbreviations[state.ToUpper()];
            return state;
        }


        private static SortedList ReadStateXml(string file)
        {
            string code;
            string text;
            var table = new SortedList(50);
            var reader = new XmlTextReader(file);
            while (reader.Read())
            {
                if (reader.IsStartElement("States"))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            code = reader.GetAttribute("code");
                            reader.Read();
                            text = reader.Value;
                            if (code != null && text != null)
                                table.Add(code, text);
                        }
                    }
                }
            }
            reader.Close();
            return table;
        }
    }
}