using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EFModel.DataDictionary;

namespace DataDictionary.Services.Model
{
    /// <summary>
    /// Abstrakte Basisklasse für SQL Generator, zur Definition allgemeingültiger
    /// Eigenschaften aller SQL Generatoren. 
    /// Darüber hinaus werden statische Eigenschaften definiert, die für alle abgeleiteten 
    /// Klassen gelten.
    /// </summary>
    public abstract class SqlScriptCreator : ISqlScriptCreator
    {
        /// <summary>
        /// Instanz des Datenbank-Kontextes
        /// </summary>
        protected DataDictionaryDbContext DbContext { get; set; }

        /// <summary>
        /// Tabulator-String
        /// </summary>
        public static readonly string TabString = new string(' ', 2);

        /// <summary>
        /// Tabulator-String
        /// </summary>
        public static readonly string HorizontalLine = new string('-', 80);

        /// <summary>
        /// Konstruktor zum Festlegen der spezifischen Eigenschaften zum Zeitpunkt der Erzeugung.
        /// </summary>
        /// <param name="dbContext">Datenbank-Kontext-Instanz</param>
        protected SqlScriptCreator(DataDictionaryDbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// Liefert in abgeleiteten Klassen den jeweiligen SQL-String
        /// </summary>
        /// <returns>SQL-String bzw. SQL-Fragment</returns>
        public abstract string GetSqlString();

        /// <summary>
        /// Liefert zum angegebenen Schema den Default Tablespace. Das Schema muss in DBA_USERS existieren,
        /// ansonsten wird eine Exception ausgelöst.
        /// </summary>
        /// <param name="schemaName">Schema-Name</param>
        /// <returns>Standard Tablespace Name</returns>
        protected string GetDefaultTablespace(string schemaName)
        {
            return DbContext.DBA_USERS.First(u => u.USERNAME == schemaName).DEFAULT_TABLESPACE;
        }

        /// <summary>
        /// Erzeugt eine Zwischenüberschrift mit dem angegebenen Titel zur Verwendung im SQL Output
        /// </summary>
        /// <param name="title">Überschrift-Titel</param>
        /// <returns>
        /// Liefert einen String, der mit einer horizontalen Linie beginnt, danach den Titel als Einzeilen-Kommentar, 
        /// und danach mit einer horizontalen Linie, jedoch ohne Zeilenumbruch, endet.
        /// </returns>
        protected string HeaderString(string title)
        {
            var resultStringBuilder = new StringBuilder();

            resultStringBuilder.AppendLine(HorizontalLine);
            resultStringBuilder.Append("-- ");
            resultStringBuilder.AppendLine(title);
            resultStringBuilder.Append(HorizontalLine);

            return resultStringBuilder.ToString();
        }

    }
}
