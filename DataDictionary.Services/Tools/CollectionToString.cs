using System;
using System.Collections.Generic;
using System.Text;

namespace DataDictionary.Services.Tools
{
    public class CollectionToString
    {
        /// <summary>
        /// Collection, die als String konvertiert werden soll
        /// </summary>
        public IEnumerable<string> Collection { get; set; }

        /// <summary>
        /// Trenn-String zwischen den einzelnen Collection-Einträgen
        /// </summary>
        public string SeparatorString { get; set; }

        /// <summary>
        /// Wenn true, wird nach jedem Trennzeichen ein NewLine eingefügt
        /// </summary>
        public bool InsertNewLine { get; set; }

        /// <summary>
        /// Größe eines Tabs als Anzahl von Leerzeichen
        /// </summary>
        public int TabSize { get; set; }

        /// <summary>
        /// Anzahl der Tabs am Beginn der neuen Zeile, wenn ein NewLine eingefügt worden ist
        /// </summary>
        public int TabCountForNewLine { get; set; }

        /// <summary>
        /// Wenn true, wird am beginn des resultierenden Strings ein newLine eingefügt
        /// </summary>
        public bool StartWithNewLine { get; set; }

        /// <summary>
        /// Liefert einen String, der bis zur in TabSize festgelegten Anzahl mit Leerzeichen aufgefüllt ist.
        /// </summary>
        public string TabString
        {
            get { return new string(' ', TabSize); }
        }

        /// <summary>
        /// Standard-Konstructor der Klasse mit Vorbelegung der Properties.
        /// </summary>
        /// <param name="collection">Collection, die als String konvertiert werden soll</param>
        /// <param name="separatorString">Trenn-String zwischen den einzelnen Collection-Einträgen</param>
        /// <param name="insertCrLf">Wenn true, wird nach jedem Trennzeichen ein NewLine eingefügt</param>
        /// <param name="tabCountWhenNewLine">Anzahl der Tabs am Beginn der neuen Zeile, wenn ein NewLine eingefügt worden ist</param>
        /// <param name="startWithNewLine">Wenn true, wird am beginn des resultierenden Strings ein newLine eingefügt</param>
        /// <param name="tabSize">Größe eines Tabs als Anzahl von Leerzeichen</param>
        public CollectionToString(
            IEnumerable<string> collection = null,
            string separatorString = ",",
            bool insertCrLf = false,
            int tabCountWhenNewLine = 0,
            bool startWithNewLine = false,
            int tabSize = 2)
        {
            Collection = collection;
            SeparatorString = separatorString;
            InsertNewLine = insertCrLf;
            TabCountForNewLine = tabCountWhenNewLine;
            StartWithNewLine = startWithNewLine;
            TabSize = tabSize;
        }

        /// <summary>
        /// Erzeugt aus der eingestellten Collection einen entsprechend der Properties formatierten String. 
        /// </summary>
        /// <returns>String-Repräsentation der Collection</returns>
        public string GetAsString()
        {
            if (Collection == null)
            {
                throw new InvalidOperationException("Collection instance is invalid!");
            }

            var resultStringBuilder = new StringBuilder();

            var firstLoop = true;
            foreach (var item in Collection)
            {
                resultStringBuilder.Append(firstLoop ? string.Empty : SeparatorString);
                if (firstLoop && StartWithNewLine || 
                    !firstLoop && InsertNewLine) 
                {
                    resultStringBuilder.AppendLine();
                    resultStringBuilder.Append(GetTabString(TabCountForNewLine));
                }
                resultStringBuilder.Append(item);
                firstLoop = false;
            }

            return resultStringBuilder.ToString();
        }

        /// <summary>
        /// Erzeugt aus der angegebenen String-Collection einen entsprechend der Properties formatierten String. 
        /// </summary>
        /// <param name="collection">String collection</param>
        /// <returns>String-Repräsentation der Collection</returns>
        public string GetAsString(IEnumerable<string> collection)
        {
            Collection = collection;
            return GetAsString();
        }

        /// <summary>
        /// Erzeugt einen String, der der angegebenen Tab-Größe entspricht
        /// </summary>
        /// <param name="tabCount">Anzahl der einzufügenden Leerzeichen in Tab-Größe</param>
        /// <returns>String mit bis zur Anzahl der Tab Größe aufgefüllten Leerzeichen</returns>
        protected string GetTabString(int tabCount)
        {
            return new string(' ',tabCount * TabSize);
        }
    }
}
