GOTHIC MOD DEVELOPMENT-KIT
---------------------------
Enthält Programme, Dokumente und Daten, die zur Erstellung von MODs benötigt werden.

Viele GOTHIC-Source-Daten sind schon in der Verkaufsversion von GOTHIC enthalten.
Mit dem Tool GothicVDFS.exe kann man diese aus den VDFS-Archiven extrahieren um sie zu bearbeiten.
Außerdem braucht ihr dieses Tool, um eigene VDFS-Archive (*.mod) mit euren modifizierten Daten zu erstellen.

Im Verzeichnis \GOTHIC findet ihr Source-Daten, die für die Erstellung von MODs benötigt werden.
Unter anderem sind hier die kompletten Game-Scripte von GOTHIC enthalten (Stand des 2. Patches, 1.08h).
Um diese Daten benutzen zu können, müssen sie komplett mit der Verzeichnisstruktur ins GOTHIC-Release-Verzeichnis kopiert werden!

--------------------------------------------------------------------------

\DOCUMENTS               MOD-KIT Doku im HTML-Format
    gothic.htm           Inhaltsverzeichnis der HTML-Doku

\GOTHIC                  Alle Unterverzeichnisse und Dateien in diesem Verzeichnis müssen komplett ins GOTHIC Release-Verzeichnis kopiert werden!
    vdfs.cfg             aktualisierte Konfigurationsdatei für VDFS, ersetzt die aus der Verkaufsversion.

    \_WORK               GOTHIC-Source-Daten
        \DATA
            \ANIMS       Verzeichnis für 3DS-MAX Animationen, die mit dem zenexp.dle exportiert wurden (*.ASC)
            \MESHES      3DS-Files von Items, Levels, Objekten
            \SCRIPTS     GOTHIC-Scripte (Stand der Version 1.08h)
            \WORLDS      zen-Files von Testlevels und den Einzelteilen der Oberwelt
        \TOOLS           Daten für den SPACER

    \SYSTEM
        \HELP             Help-Files für den SPACER
        spacer.exe        Welt-Editor für GOTHIC (bitte über GothicStarter.exe aufrufen!)
        zspy.exe          Debugging-Tool
        defaultmod.ini    Vorlage für MOD-INI-Dateien
	ready.wav	  Ton-Signal für den Spacer

\MISC                     
    \DAEDALUS SYNTAX HIGHLIGHTING
        \CODEEDIT	  Syntax Highlighting für CodeEdit
        \CRIMSONEDITOR	  " für den Crimson Editor
        \NOTEPAD++        " für Notepad++
        \SOURCEEDIT       " für SourceEdit
        \ULTRAEDIT        " für UltraEdit
        \VSCODE           " für VSCode     
    \GOTHIC VERSIONS      Dokumentation der Gothic Versionen
    \ZTEX FILE REFERENCE  Dokumentation des ZenGin Texture Formats (ZTEX)
 
\TOOLS                    
    \3DS MAX STUFF
        \DATA                     Beispiele für 3DS-MAX-Animationen (Meatbug)
        \PLUGINS  
	    \KERRAX_ASC_MAX5      Asc Importer/Exporter für 3DS MAX 5 von Kerrax
            \KERRAX_ASC_MAX6-8    Asc Importer/Exporter für 3DS Max 6-8 von Kerrax     
            3dsexp.dle            modifizierter 3ds-exporter für 3DS MAX (zum Export von Level- und 
                                  Objekt-Meshes. Unterstützt lange Dateinamen für Texturen)
            3dsimp.dli            modifizierter 3ds-importer für 3DS MAX (zum Import von Level- und 
                                  Objekt-Meshes. Unterstützt lange Dateinamen für Texturen)
	    zenexp.dle            modifizierter ASC-Exporter für 3DS MAX (zum Export von Animationen und Figuren-Models)
    \BLENDER STUFF
        \PLUGINS
            Gothic_MaT_Blender    Synchronisiert UV-Texturen und Materialien und beseitigt Fehler (Duplikate etc.)
            KerraxImpExp          Importer/Exporter für Blender von Kerrax
            ZenVis                Tool zum Exportieren der Visuals einer ZEN, um sie in Blender zu laden
    \FARBEIMER                    Ein kleines Tool, um im Forum Code mit Synthax Highlighting anzuzeigen
    \FNTEDIT                      zum Bearbeiten und Erstellen von FNT Dateien (Fonts)
    \FONT2TARGA                   wandelt Windows Fonts in Gothic-taugliche Fonts um      
    \GOMAN                        Zum Betrachten und modifizieren von Texturen in einer .vdf oder .mod Datei
    \GOTHICSOURCER                Ein Tool, um kompilierte Dateien von Gothic zu dekompilieren
    \GothicVDFS                   VDFS Viewer, Extractor und MOD-Builder
    \GothicVdfsOpt                VDFS Optimizer, zum Verhindern von Duplikaten
    \GothicZTEX                   ZTEX Decompressor, bringt Texturen von .tex zurück in .tga 
    \GVE                          Programm zum Manipulieren der Savegames
    \MILKSHAPE 3D STUFF           3DS Importer/Exporter für MilkShape 3D
    \NPC-VIEWER                   Visualisierung der Erstellung von NPCs (Gender, Skin, Headmesh, Bodytex, Headtex)
    \OUTPUT-COMMANDER             Liest Output-Dateien (.csl und .bin) aus und listet alle Outputs auf
    \REDEFIX                      Aktualisiert die Untertitel (in einem Bruchteil der Zeit, die der Spacer dafür benötigt)
    \SPACERHOTKEYS                Kleines Tool, um diversen Spacer Funktionen feste Hotkeys zuzuweisen
    \SPRACHAUSGABENHELFER         Liest Dialogskripte und/oder die SVM.d ein und prüft, ob Sounddateien dazu vorhanden sind
    \STAMPFER                     Umfangreicher Texteditor für Gothic
    \VDFMANAGER		          VDF Manager zum Aktivieren/Deaktivieren von VDF-Volumes
    \ZEN CONVERT                  Programm zum Konvertieren von ZEN-Dateien von Gothic 2 zu Gothic 1 und vice versa
    \ZSLANG                       Skriptsprache zur Lösung von mechanischen Aufgaben im Zusammenhang mit ZEN Dateien
    \ZTEXTOOLS                    Tools, die eine Konvertierung ohne die Engine ermöglichen
    Releases.txt                  Auflistung aller Tools und ihrer Veröffentlichungstermine  
    WorldofGothicLinks.txt        Linkliste zu den Tools auf World of Gothic  

    
-------------------------------------------------------------------------- 


Nutzungsbedingungen
---------------------------------
- Die Nutzung des MOD PLAYER-KIT und des MOD DEVELOPMENT-KIT ist ausschliesslich für nichtkommerzielle Zwecke gestattet.
- Alle im PLAYER-KIT und DEVELOPMENT-KIT enthaltenen Programme und Daten, sowie alle damit erstellten,
  bzw. modifizierten Daten dürfen nicht kommerziell genutzt werden.


Support
---------------------------------
Fragen zum MOD-KIT können in den extra dafür eingerichteten Foren der GOTHIC-Fanpages gestellt werden.
Natürlich wird auch das GOTHIC-Team ab und zu dort reinschauen und versuchen euch bei Problemen zu helfen.
Wendet euch bitte nicht mit Fragen zum MOD-KIT an den Piranha Bytes Support.
Es gibt dafür keinen offiziellen Support von Piranha Bytes!
