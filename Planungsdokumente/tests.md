ID:			T01
Beschreibung:		Eine Runde mit Frage zu einem in der DB gespeicherten Thema spielen und beenden
Vorbedingung:		Das Programm ist gestartet und befindet sich auf der Startseite
Test-Schritte:		1. Ein neuer Teilnehmer wird angelegt und der DB hinzugefügt oder ein vorhandener Nutzer ausgewählt
			2. Der "Weiter"-Knopf auf der Startseite wird betätigt und die Prompterstellungs Seite geöffnet
			3. Der ToggleButton "Thema" wird (bzw. ist bereits) ausgewählt, ein Schwierigkeitsgrad kann optional ausgewählt werden
			4. In der Thema-Schnellauswahlliste auf der Seitenleiste wird eines der angelegten Themen ausgewählt und "Übernehmen" betätigt
			5. Der "Generieren"-Knopf wird betätigt
			6. Einer der 4 Antwort-Buttons wird vom Nutzer ausgewählt und die korrekte Antwort sowie die finalen Navigationsknöpfe werden angezeigt
			7. Der "Zurück zur Startseite"-Knopf wird betätigt und das Quizspiel beendet
Erwartetes Resultat:	Die erspielten Punkte werden der Gesamtpunktzahl des Teilnehmers in der DB gutgeschrieben
			Die neue Frage wird mit ihren Antworten in die DB eingespeichert
			Das GUI des Programms ist wieder zurück auf der Startseite

ID:			T02
Beschreibung:		Eine Spiel mit Frage zu einem neuen, vom Nutzer eingegebenen Text mit 2 Teilnehmern spielen und fortfahren
Vorbedingung:		Das Programm ist gespeichert und befindet sich auf der Startseite
Test-Schritte:		1. Zwei Teilnehmer werden entweder neu angelegt oder vorhandene aus der DB ausgewählt
			2. Der "Weiter"-Knopf auf der Startseite wird betätigt und die Prompterstellungs Seite geöffnet
			3. Der ToggleButton "Text" wird ausgewählt und ein Schwierigkeitsgrad kann optional ausgewählt werden
			4. Das TabItem "TexteBox" wird ausgewählt und der Nutzertext entweder aus externer Quelle eingefügt oder in die Textbox verfasst
			5. Der "Generieren"-Knopf wird betätigt
			6. Einer der 4 Antwort-Buttons wird zuerst vom 1. Nutzer und danach vom 2. Nutzer ausgewählt und die korrekte Antwort sowie die finalen Navigationsknöpfe werden angezeigt
			7. Der "Nächste Runde"-Knopf wird betätigt und die besagte 2. Runde beginnt
Erwartetes Resultat:	Die erspielten Punkte werden der Gesamtpunktzahl der Teilnehmer in der DB gutgeschrieben
			Der neue Text, die neue Frage und ihre Antworten werden in die DB eingespeichert
			Das GUI des Programms ist wieder zurück auf der Prompterstellungsseite auf dessen Seitenleiset nun dieselbe Teilnehmerliste und "Runde: 2" angezeigt werden
			Der neue Text wird im Text Liste der Seitenleiste an unterster Stelle angezeigt