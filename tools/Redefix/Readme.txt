  ______     ______     _____     ______     ______   __     __  __
 /\  == \   /\  ___\   /\  __-.  /\  ___\   /\  ___\ /\ \   /\_\_\_\
 \ \  __<   \ \  __\   \ \ \/\ \ \ \  __\   \ \  __\ \ \ \  \/_/\_\/_
  \ \_\ \_\  \ \_____\  \ \____-  \ \_____\  \ \_\    \ \_\   /\_\/\_\
   \/_/ /_/   \/_____/   \/____/   \/_____/   \/_/     \/_/   \/_/\/_/
   
   (c) Sumpfkrautjunkie 2010

   
   
 +------+
 |Inhalt|
 +------+
 1. Der Sinn des Programms
 2. Was das Programm kann
 3. Einrichten des Programms
 4. Die einzelnen Funktionen im Überblick
 5. FAQ
 6. Changelog
 
 +-------------------------+
 |1. Der Sinn des Programms|
 +-------------------------+
 
 Redefix dient dazu, die Untertitel beim Entwickeln von Gothic-Modifikationen 
 (für G1 und G2-DNdR) schnell und unkompliziert zu aktualisieren.
 Zwar bietet der Spacer bereits die Möglichkeit Untertitel zu aktualisieren, 
 allerdings kann man bei einer Aktualisierung durch den Spacer gerne mal eine
 Kaffeepause einplanen, da dieser unglaublich (und vor allem unnötig) lange für 
 diesen Vorgang braucht. Des Weiteren ist die Handhabung der spacerseitigen 
 Aktualisierung unnötig kompliziert. Aus diesem Grund wurde Redefix entwickelt.
 Redefix kann mit nur einem Mausklick die Untertitel in nur einem kleinen Bruchteil
 der Zeit aktualisieren, die der Spacer dafür benötigt. Des Weiteren bietet er
 einige zusätzliche Komfortfunktionen. Redefix ist übrigens nur eine stark
 optimierte und mit einer Benutzeroberfläche ausgestattete Weiterentwicklung von
 ADOUSADS, einer etwas älteren Software von mir :-)
	

 +------------------------+
 |2. Was das Programm kann|
 +------------------------+
 
 Redefix kann die OutputUnits aktualisieren, indem es im Cutscene-Ordner die alte
 OU.BIN löscht und eine neue OU.CSL erstellt (oder direkt selbst eine OU.BIN erstellt), aus welcher Gothic beim nächsten
 Start automatisch eine neue OU.BIN generiert, welche für die Untertitel benötigt
 wird. Findet Redefix Untertitel, welche zwar die gleiche Bezeichnung haben aber
 unterschiedliche Untertiteltexte, was beim Erstellen von Dialogen per Copy & Paste
 schnell mal passieren kann, so werden diese aufgelistet.
 Davon wäre z.B: betroffen:
 
 AI_Output (other, self, "DIA_Addon_Fortuno_Hi_15_02");//Alles klar bei dir?
 AI_Output (other, self, "DIA_Addon_Fortuno_Hi_15_02");//Hallo Fremder!
 
 
 Daneben gibt es die Möglichkeit, schnell die Scripte auf Syntaxfehler zu überprüfen,
 also, ob z.B. irgendwo eine Klammer oder ein Semikolon vergessen worden ist.
 
 Sofern man keine Sprchausgabe hat oder erstellen möchte und sich an der imho recht bescheidenen internen
 Berechnung für die Anzeigedauer unvertonter Dialoge stört, kann sich von Redefix eine angepasste *.CSL/*.BIN
 erstellen lassen, welche stumme .WAV Dateien variabler Länge in unvertonte Dialoge einsetzt. Damit kann man 
 die Anzeigedauer der Untertitel selber beeinflussen ohne irgendwelche störenden Nebeneffekte.

 Mehr kann Redefix nicht.

 +---------------------------+
 |3. Einrichten des Programms|
 +---------------------------+
 
 Zum Start des Programms ist erstmal das .Net Framework 2.0 nötig. Wird ein älteres
 Windows als Vista verwendet, ist es unter Umständen nicht installiert (beim Start
 von Redefix erscheint eine Fehlermeldung). Das .Net Framework kann von der
 Microsoft-Seite heruntergeladen werden:
 http://www.microsoft.com/downloads/details.aspx?displaylang=de&FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5
 
 Damit das Programm arbeiten kann, benötigt es einige Dateipfade. Daneben können
 einige Einstellungen des Programms an die eigenen Wünsche angepasst werden.
 
 Beim ersten Start des Programms erscheint eine Fehlermeldung, dass die Datei für
 die Einstellungen nicht gefunden wurde. Dies ist nicht verwunderlich, denn diese
 Datei existiert ja noch gar nicht. Die Fehlermeldung kann also getrost ignoriert
 werden. Nur wenn sie später mal scheint ist sie ein Indiz dafür, dass die Datei
 für die Einstellungen (die Settings.xml) versehentlich gelöscht wurde. Nach der
 Meldung wird  Automatisch das Einstellungsfenster geöffnet, um dort die benötigten
 Einstellungen angeben zu können.
 
 Die erste wichtige Einstellung ist "Pfad zum Gothic Ordner". Hier muss man den
 Pfad  zum Ordner angeben, in welchem sich Gothic befindet. Dies könnte z.B. der
 Pfad:
 C:\Program Files (x86)\JoWooD\Gothic II
 sein. Natürlich ist  es abhängig von den eigenen Ordnernamen und ist daher von
 Rechner zu Rechner unterschiedlich.
 
 Die zweite wichtige Einstellung ist "Zu verwendende *.Src". Damit legt man fest,
 welche  Src-Datei Redefix konsultieren soll, um die Scripte zu finden in denen
 es nach  Untertiteln suchen soll. Src-Dateien sind im Prinzip Dateilisten, welche
 die Dateien definieren, welche Gothic als Scripte betrachten soll. Sollte man
 da keine besonderen Wünsche haben, sollte man die Standard-Datei angeben, die
 "Gothic.src". Diese findet man in vom Gothic-Ordner aus gesehen in 
 _work\Data\Scripts\Content
 
 Die Einstellung "Name der zu erstellenden *.Csl/*.Bin" sollte bei ihrem Standardwert
 OU belassen werden, sofern man dahingehend keine vom Standard abweichenden
 Wünsche hat. Die Einstellung legt den Dateinamen der Csl/Bin-Datei fest, welche aus
 den gefundenen Untertiteln erstellt wird. In der Modifaktions-Ini kann nämlich 
 der Name der Untertiteldatei angegeben werden, welche für die Modifikation
 verwendet werden soll. Diese Einstellung  ist standardmäßig "OU".
 
 Die Option "Erstellen" legt fest, ob eine *.Csl, eine kompilierte *.Bin oder direkt
 beide Dateien erstellt werden sollen. Wird die .Bin erstellt, muss diese nicht mehr
 von Gothic selbst erstellt werden, was prinzipiell Zeit sparen sollte.
 
 
 "Kommentare filtern" legt fest, ob Redefix beim Aktualisieren der Untertitel
 überprüfen soll, ob die Untertitel ggf. auskommentiert sind und daher gar nicht
 im Spiel vorkommen können. Dies kostet zwar etwas Leistung, dafür werden aber
 keine unnötigen Untertitel erfasst und es kommt nicht zu Problemen mit doppelt
 vorhandenen Untertiteln (die aufgrund einer Auskommentierung eigentlich einmalig
 sind). Die regulären Kommentare hinter AI_Output in welchen die Untertiteltexte
 stehen, sind davon NICHT betroffen.
 
 Die Option "Hotkey verwenden" legt fest, ob die Aktualisierung der Untertitel
 außerhalb des Programms durch ein Tastenkürzel erfolgen kann. Dies kann
 insofern praktisch sein, dass man das Redefix-Fenster nicht in den Fokus nehmen
 muss, um die Untertitel zu aktualisieren. Im nebenstehenden (nur bei gesetztem
 Häkchen sichtbaren) Feld kann die Tastenkombination festgelegt werden. Dazu muss
 das Feld erst mit der Maus angeklickt und dann die gewünschte Tastenkombination
 gedrückt werden. Ab dann kann die Aktualisierungs-Funktion jederzeit  (Redefix
 muss selbstverständlich gestartet sein, aber es ist egal, ob das Redefix-Fenster
 fokussiert ist oder nicht ) über diese Kombination aufgerufen werden. 
 

 "Variable Untertitellänge" öffnet ein Untermenü, in welchem man einstellen kann,
 ob und wie den unvertonten Dialogen stumme .WAVs zugewiesen werden, um die Anzeigedauer
 der Dialoge selber anzupassen.
 "Variable Untertitellänge aktivieren" aktiviert die Erstellung und Zuordnung der stummen .WAV
 Dateien. ,
 "Buchsabe/Sekunde" legt die angenommene Lesegeschwindigkeit fest.
 "Minestlänge" legt die Zeit in Sekunden fest für die die Untertitel mindestens angezeigt werden.
 "Höhstlänge" legt die Zeit in Sekunden fest, für die die Untertitel höchstens angezeigt werden.
 "Längenaufschlag" addiert zur errechneten Anzeigelänge immer einen festen Wert dazu.
 Die Formel für die Längenberechnung ergibt sich aus:

	Länge=Textlänge/("Buchstaben/Sekunde")+Längenaufschlag.
	FALLS Länge<Mindestlänge DANN Länge = Mindestlänge
	FALLS Länge>Höchstlänge DANN Länge = Höchstlänge
 "Pfad zum Spech-Ordner" gibt den Ordner an, in welchem die Sprachausgabe für Gothic zu finden ist.
 Dort werden die stummen .Wav-Dateien hin kopiert und nach bereits vorhandenen Vertonungsdateien gesucht.
 Standardmäßig ist dieser Ordner: $Gothicordner$\_work\data\Sound\Speech

 Mit "Übernehmen" werden die Einstellungen übernommen und gespeichert.
 
 +----------------------------------------+
 |4. Die einzelnen Funktionen im Überblick|
 +----------------------------------------+
 
 Redefix bietet zwei Hauptfunktionen zum Aktualisieren der Untertitel.
 Zum einen "Aktualisieren" und "Neu erstellen", welche beide über Buttons, bzw.
 die jeweiligen Kontextmenüeinträge des Tray-Icons erreichbar sind.
 "Neu erstellen" durchsucht alle in der *.Src aufgelisteten Scripte nach
 Untertiteln und  erstellt daraus die OU.CSL, welche Gothic für die Untertitel
 benötigt. "Aktualisieren" arbeitet ähnlich, allerdings überprüft es bei jeder
 Datei, ob diese seit der letzten Aktualisierung verändert wurde. Nur wenn dies
 der Fall ist, wird diese auf Untertitel untersucht, andernfalls wird sie ignoriert.
 Dies hat den Vorteil einer höheren Geschwindigkeit, allerdings werden dabei kein
 Untertitel berücksichtigt, welche zwischenzeitlich gelöscht wurden. Das heißt,
 dass die Untertiteldatei unter Umständen zu viele Einträge haben kann, was aber
 für den Praxisbetrieb nicht wirklich relevant ist. Zum Erstellen der für den
 Release/Betatest finalen Version der Untertiteldatei sollte allerdings der
 kleineren Größe wegen lieber die "Neu erstellen"-Methode verwendet werden.
 Änderungen an Untertiteltexten werden in der "Aktualisieren"-Methode berücksichtigt
 und eingebunden, allerdings funktioniert hierbei aufgrund der speziellen 
 Arbeitsweise die Erkennung von doppelt vorhandenen Untertiteln nicht.
 Dies sollte aber für den Alltagsgebrauch kein allzu großes Problem darstellen.
 
 Die Aktualisierung kann darüber hinaus über eine Tastenkombination, welche
 man in den Einstellungen definieren kann und über einen Klick mit der mittleren
 Maustaste auf das Tray-Icon (das kleine Redefix-Icon im Bereich neben der Uhr)
 erfolgen.
 
 "Syntaxcheck"  überprüft die Scripte auf Syntaxfehler und zeigt gefundene Fehler
 unten bei den Zusatzinfos an.
 
 "Zusatzinfos anzeigen" zeigt bzw. blendet den Bereich an/aus, in welchem
 doppelt vorhandene Untertitel oder gefundene Syntaxfehler angezeigt werden. 
 Ein einfacher Klick kopiert ihren Namen direkt in die Zwischenablage, sodass 
 man in der "Suchen in Dateien"  Funktion des Editors der eigenen Wahl schnell 
 danach suchen kann. Ein Doppelklick  hingegen öffnet alle Scriptdateien, in 
 welchen der Name des Untertitels vorkommt. Bei Fehlern öffnet sich direkt das
 als fehlerhaft erkannte Script und es wird die Zeilennummer mit dem Fehler in
 die Zwischenablage kopiert, damit man schnell zu der fehlerhaften Zeile navigieren
 kann (in den meisten Editoren ist es die Tastenkombination STRG+G).
 
 Beim Minimieren des Fensters verschwindet dieses aus der Taskleiste. Hervorholen
 kann man es,  indem man doppelt auf das Redefix-Tray-Icon (links neben der Uhr)
 klickt oder mit einem Rechtsklick im dortigen Kontextmenü die Option "Anzeigen"
 auswählt.
 

 +------+
 |5. FAQ|
 +------+

 F: Ich habe die Untertitel mit Redefix aktualisiert, aber sie erscheinen nicht
 im Spiel.
 A: Dies kann unterschiedliche Gründe haben.
 
 Zuerst sollte man überprüfen, ob im Ordner _work\data\Scripts\Content\Cutscene
 eine neue OU.CSL erstellt wurde (auf Änderungsdatum der Datei schauen) und es 
 im Ordner keine OU.BIN mehr gibt. Ist keine neue OU.CSL erstellt worden oder
 die OU.BIN bereits vorhanden, sollte überprüft werden, ob in den Redefix-
 Einstellungen auch die richtigen Pfade angegeben wurden und ob man auf diese 
 Schreibrechte hat. Dies kann bei den seltsamen Wirrungen von Windows manchmal
 sogar der Fall sein, wenn man als Administrator angemeldet ist. Die Schreibrechte
 eines Ordners kann man in den  Ordnereigenschaften (Rechtsklick auf den Ordner 
 -> Eigenschaften) unter dem Reiter "Sicherheit" einstellen.  Allgemein empfiehlt
 es sich, Gothic unter Vista nicht in den Ordner "C:\Programme" zu installieren,
 da dort andere Sicherheitsbestimmungen gelten, als in anderen Ordnern. Geeigneter
 wäre ein  Installationsort z.B. in einem Ordner "C:\Spiele".
 
 Sollte eine neue OU.CSL erstellt worden sein, kann das Ausbleiben der Untertitel
 an der Modifikations-Ini-Datei liegen, über die man die Mod im GothicStarter
 startet. Diese darf keinen VDF= Eintrag haben, da sonst nicht die aktuell erstellten
 Untertitel verwendet werden. So hat man in der GothicGame.ini z.B. standardmäßig
 einen Eintrag bei VDF=, welcher  unter anderem zu den Originaluntertiteln führt.
 Eine mögliche Lösung wäre es, die GothicGame.ini im Gothic-System-Ordner zu öffnen
 und dort bei der Zeile 
 VDF=GothicGame.mod
 den Rest der Zeile hinter dem = zu löschen:
 VDF=
 
 Eleganter wäre es eine eigene .ini Datei zu erstellen, denn darum kommt man,
 wenn man irgendwann die Mod veröffentlichen möchte nicht herum. Dazu kopiert
 man einfach die GothicGame.ini im Systemverzeichnis und benennt die Datei um,
 z.B. in: DarkIsland.ini (am besten ist es, wenn der Name etwas mit dem Titel
 der Mod zu tun hat :-) )
 Dann öffnet man diese Datei und passt sie den eigenen Wünschen entsprechend an.
 Zuallererst sollte, wie oben beschrieben erstmal der Eintrag hinter VDF= gelöscht
 werden. Eine fertig  eigene Ini könnte so aussehen:

	[INFO]
	Title=Die Dunkle Insel
	Version=1.0
	Authors=Ich und Ich
	Webpage=http://www.meinewebseite123344.de
	Description=!<symlink>DarkIsland.rtf
	Icon=GothicStarter.exe

	[FILES]
	VDF=
	Game=Content\Gothic
	FightAI=Content\Fight
	Menu=System\Menu
	Camera=System\Camera
	Music=System\Music
	SoundEffects=System\SFX
	ParticleEffects=System\ParticleFX
	VisualEffects=System\VisualFX
	OutputUnits=OU

	[SETTINGS]
	Player=PC_HERO
	World=NewWorld\NewWorld.zen

	[OPTIONS]
	show_Info=0
	show_InfoX=800
	show_InfoY=7200
	show_Version=1
	show_VersionX=6500
	show_VersionY=7200
	show_Focus=1
	show_FocusItm=1
	show_FocusMob=1
	show_FocusNpc=1
	show_FocusBar=1
	force_Subtitles=0
	force_Parameters=

	[OVERRIDES]
	INTERNAL.extendedMenu=1
 
 
 Description verweist auf eine Datei in der die Modbeschreibung zu finden ist
 (der Text wird später beim Spieler angezeigt). Diese kann erstellt werden, 
 indem die GothicGame.rtf kopiert, umbenannt und inhaltlich z.B. mit dem
 Windowsprogramm "Wordpad" verändert wird. Bei Icon kann man ein eigenes Symbol
 angeben, mit welchem die Mod im Gothicstarter abgebildet wird. Die Einstellung
 World legt die Startwelt der Mod fest. Dies ist eine Angabe vom Ordner
 _work\data\Worlds aus gesehen. Sollte die Mod in der gleichen Welt, wie Gothic 2
 spielen, ist hier keine Änderung nötig. Die restlichen Angaben außerhalb der
 INFO-Sektion sollten (für den Anfang) nicht verändert werden.
 
 
 F: Redefix ist mir zu langsam, geht's schneller?
 A: Wer Lust hat, kann eine native C/C++ Version schreiben :P
 Ansonsten kann man einige Optimierungen vornehmen, wie auf das Filtern von 
 Kommentaren verzichten und eine optimierte *.Src verwenden. Diese sollte nur
 Dateien beinhalten, in welchen auch wirklich Untertitel zu finden sind. So 
 kann man im Content-Ordner eine eigene *.Src nur für Redefix anlegen,
 welche z.B. den Inhalt hat:
 
 STORY\svm.d
 STORY\DIALOGE\DIA*.d
 STORY\B_AssignAmbientInfos\*.d

 Dadurch muss Redefix deutlich weniger Dateien abarbeiten, was eine
 Aktualisierung deutlich beschleunigt.
 
 F: Die Tastenkombination einer anderen Anwendung, die ich verwende, funktioniert
 nicht, wenn Redefix läuft. 
 A: Die Hotkeyfunktion von Redefix kann in den Einstellungen deaktiviert oder
 umbelegt werden.
 
 F: Redefix startet nicht, es kommt eine Fehlermeldung
 A: Wenn ein älteres Windows verwendet wird, als Vista, kann es durchaus sein,
 dass das zum Start des Programms benötigte .Net Framework 2.0 nicht installiert
 ist (bei Vista ist es schon vorinstalliert). Das .Net Framwork kann  von der
 Microsoft-Seite heruntergeladen werden:
 http://www.microsoft.com/downloads/details.aspx?displaylang=de&FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5
 
 F: Warum muss ich das ".Net Framework" installieren?
 A: Aus dem selben Grund, warum man z.B. Flash, Python, Java oder Visual C++ 2005
 Redistributable Packages installiert. Manche Software benötigt einfach bestimmte
 Dateien um zu funktionieren. Bei Redefix ist es die .Net  Laufzeitumgebung und
 die .Net Bibliotheken. Analog übertragen: Ohne Backöfen in den Haushalten wären
 Tiefkühlpizzen ziemlich sinnlos :-)
 
 +------------+
 |6. Changelog|
 +------------+
 
 # 1.4
 Bugfixes:
 - UNIX - Formatierte Scriptdateien sollten jetzt korrekt gelesen werden können
 - Fehler, dass die letzte OU im Script manchmal übersehen wurde, behoben
 
 Neues:
 - Interfaceänderungen
 - Optionale Syntaxüberprüfung der Scripte
 
 # 1.9
 Bugfixes:
 - Unix-Formatierte Svm.d sollte nun erkannt werden
 - Fehler, dass die letzte OU im Script manchmal übersehen wurde, nun hoffentlich endlich behoben
 - Fehler bei Kommentarerkennung behoben
 
Neues:
 - Möglichkeit, direkt die .Bin Datei zu generieren


# 2.0
Neues:
 - Neues Icon
 - Möglichkeit, die Anzeigedauer von unvertonten Dialogen einzustellen (ähnlich Sektenspinners B.lang.loS )

# 2.1
Bugfixes:
 -.src Angaben in .src Dateien werden nun erkannt