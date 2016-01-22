DataDictionary Tool
===================

Das DataDictionary Tool liest das Datenmodell des spezifizierten Oracle-Datenbankschemas aus bund erzeugt in das Ausgabeverzeichnis einzelne Dateien für die Erstellung der Datenbankobjekte. 
Zusätzlich wird in das Schema-Verzeichnis im Ausgabeverzeichnis eine Datei erzeugt, die die generierten Erstellungsscripts für die DB-Objekte in der korrekten Reihenfolge aufruft, in der diese Objekte unter Berücksichtigung von Abhängigkeiten erzeugt werden müssen.

Das Tool ist ein Kommandozeilen-Tool, das eine Applikations-Konfigurationsdatei enthält, dem ebenso über die Kommandozeile Parameter übergeben werden können.
Um Passwörter zu schützen, empfiehlt es sich, mindestens das Passwort in der Kommandozeile anzugeben, und nicht dauerhaft in die app.config einzutragen.

App.config - Parameter:

database.sid - SID der Datenbank (z.B. vera.05)
database.username - Username des Benutzers, der Admin-Berechtigung in der o.g. datenbank hat (Lesen der DBA_* Views)
database.password - Passwort des unter database.username angegebenen Benutzers (möglichst nicht in app.config ablegen)
export.schema - Schema Name für den Export
export.root_path - Basis-Pfad für die Ausgabe der erzeugten Dateien. 

Diese App.config-Parameter können ebenfalls durch Kommandozeilen-Parameter im Format "key=value" übergeben werden. Wichtig ist, dass innerhalb der Key-Value-Parameter kein leerzeichen enthalten ist, oder dass dann der gesamte Parameter in doppelte Anführungszeichen gesetzt wird.
Folgende Kommandozeilen-Parameter entsprechen den app.config-Parametern:

sid - database.sid
username - database.username
password - database.password
schema - export.schema
rootpath - export.root_path

Beispiel-Aufruf:

DataDictionaryTool sid=vera05.world username=EEG_ADMIN password=**** schema=EEG_PROD_2 rootpath=_output
