using System;
using System.Collections.Generic;
using System.IO;

namespace DataDictionary.Services.Tools
{
    public class TemplateStrings
    {
        /// <summary>
        /// Ordnername, in dem sich die SQL-Templates befinden
        /// </summary>
        private const string TemplatesPath = "_templates";

        /// <summary>
        /// Collection zur Speicherung der Datei-Inhalten
        /// </summary>
        private static IDictionary<string, string> _templatesCollection;

        /// <summary>
        /// Liefert die gültige Referenz der Datei-Inhalts-Collection.
        /// </summary>
        private static IDictionary<string, string> Templates
        {
            get { return _templatesCollection ?? (_templatesCollection = new Dictionary<string, string>()); }
        }

        /// <summary>
        /// Liefert den Template String zum angegebenen Template-Filenamen. Wenn dieser in der 
        /// internen Collection noch nicht enthalten ist, wird das File eingelesen und der Wert
        /// zugewiesen.
        /// </summary>
        /// <param name="templateFileName">Template File Name, wird als Key in der Collection verwendet.</param>
        /// <returns>Template String, Inhalt des eingelesenen Template Files</returns>
        public static string GetTemplate(string templateFileName)
        {
            if (Templates.ContainsKey(templateFileName))
            {
                return Templates[templateFileName];
            }

            var fullPath = TemplatesPath + Path.DirectorySeparatorChar + templateFileName;
            if (!File.Exists(fullPath))
            {
                throw new Exception("Kann Template-Datei " + fullPath + " nicht finden!");
            }

            using (var fileReader = new StreamReader(fullPath))
            {
                Templates[templateFileName] = fileReader.ReadToEnd();
            }

            return Templates[templateFileName];
        }
    }
}
