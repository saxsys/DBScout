using System.Collections.Generic;

namespace DataDictionary.Services.Model
{
    /// <summary>
    /// Definition einer Datenstruktur zur Verwaltung von datenbank-Objekten innerhalb der Abhängigkeitsmatrix.
    /// </summary>
    public class DbObject
    {
        /// <summary>
        /// Standard-Konstruktor. Die beiden Collections UsedBy und DependsOn werden initialisiert.
        /// </summary>
        public DbObject()
        {
            UsedBy = new List<DbObject>();
            DependsOn = new List<DbObject>();
        }

        /// <summary>
        /// Typ-bezeichnung des Datenbank-Objektes
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Owner des Datenbank-Objektes
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Name des Datenbank-Objektes
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Pfadname zur Definitions-Datei
        /// </summary>
        public string FilePath { get; set; }
        
        /// <summary>
        /// Diese Collection enthält Referenzzen auf alle Datenbank-Objekte, die dieses Datenbankobjekt verwenden.
        /// </summary>
        public ICollection<DbObject> UsedBy { get; set; }

        /// <summary>
        /// Diese Collection enthält die Referenzen auf alle Datenbank-Objekte, von denen dieses Datenbankobjekt abhängt.
        /// </summary>
        public ICollection<DbObject> DependsOn { get; set; }

        /// <summary>
        /// Liefert einen Wert zum Rang innerhalb der Definitions-Reihenfolge zurück. Je kleiner der Wert ist,
        /// desto eher wird das Objekt innerhalb der Definitions-Reihenfolge erstellt.
        /// </summary>
        public int DependsOnRank
        {
            get { return DependsOn.Count; }
        }

        /// <summary>
        /// Liefert einen zweiten Wert zum Rang innerhalb der Definitions-Reihenfolge zurück. Je größer der Wert ist,
        /// desto eher wird das Objekt innerhalb der Definitions-Reihenfolge erstellt.
        /// </summary>
        public int UsedByRank
        {
            get { return UsedBy.Count; }
        }
    }
}
