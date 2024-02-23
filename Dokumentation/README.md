-- ChatGPT-Quiz --
WPF-GUI-Projekt von Regin Gabriel Müller

Dies ist ein Git-Repository des ChatGPT-Quiz Projekts.

Der Nutzer eines GUI-Programms soll ein Thema oder einen längeren Text per ChatGPT-Anfrage angeben und die KI soll eine entsprechende Frage und (vier) Antwortmöglichkeiten zurückgeben.
Die Eingabe des Nutzers soll ohne Weiteres in eine einheitliche Chat-Anfrage formatiert werden. 
Die daraus folgende KI-Antwort soll ebenso für den Nutzer einheitlich augegeben werden.
Die Auswahl mehrerer Teilnehmermitglieder der Datenbank ist lokal mit bis zu 4 Teilnehmern möglich.

Daten wie die Quizteilnehmer, deren Punktestände, Themen un deren zugehörigen Fragen und Antwortmöglichkeiten werden in der Datenbank gespeichert 
und durch das Programm verwaltet.

Es gibt 3 verschiedene Schwierigkeitsstufen für die Fragen und zusätzlich die Option, keine Schwierigkeit festzulegen.

-- Hinweise zu Teil 2 des Projekts --

Zu 7.
Das WPF-Programm besitzt eine grafische Oberfläche mit mehreren Bildschirmen, die nacheinander durchlaufen werden und den üblichen Programmablauf darstellen. 
Zum Ende einer Quizrunde gibt es die Wahl zwischen dem Spielen einer neuen Runde mit denselben Teilnehmern oder einer Rückkehr zur Startseite.

Zu 8.
Das Programm wurde größtenteils vor Erstellung der neuen Commits geschrieben, weshalb erst zu Ende der Projektzeit neue Inhalte zum Repository gepusht wurden.
Die WPF-Anwendung selbst wurde mithilfe eines Branches hinzugefügt.

Zu 9.
Die Code-Review und der damit verbundene Commit ist unter den geschlossenen Pull-Requests aufzufinden.

Zu 10.
Die Issues sind offen und mit passendem Tag versehen im Issue-Reiter zu finden. 
Das Issue 3 wurde mit keinem Commit verlinkt, da es sich um ein ungelöstes (bzw. unlösbares) Problem handelt und ich somit eine realistische Situation darstellen wollte.

Zu 12.
-- Dokumentation --
Die Screenshots f1-3 sind im Ordner Dokumentation/Screenshots zu finden

Screenshot f1: Startseite
In der TextBox auf der linken Seite der Startseite kann der Name eines neuen Teilnehmers eingegeben werden. Der darunter liegende Knopf legt diesen in der DB an.
Die Liste in der Mitte der Seite zeigt alle Teilnehmer in der DB an 
und mit den Schaltflächen "Hinzufügen" und "Entfernen" kann ein ausgewähltes Mitglied der Liste zu der Liste der Teilnehmer der bevorstehenden Quizrunde hinzugefügt werden.
Der "Weiter"-Knopf auf der unteren Bildschirmmitte führt auf die nächste Ansicht.

Screenshot f2: Prompterstellungsseite
Durch Auswahl der ToggleButtons auf der oberen Seite der rechten Bildschirmhälfte können Optionen zur Frage getroffen werden, die nachfolgend gestellt werden soll.
Nun kann unter diesen Optionen entweder ein Thema / Text selbstständig in die Textboxen des jeweiligen Tab-Reiters eingegeben werden 
oder die linke Seitenleiste genutzt werden, um bereits in der DB gespeicherte Inhalte auszuwählen und in die rechten Textboxen zu "übernehmen".
Mit dem Knopf "Generieren" wird zur letzten Ansicht gewechselt.

Screenshot f3: Frageseite
Durch Auswahl einer der vier Antwort-Buttons wird die darüber stehende generierte Frage geantwortet. 
Dies passiert so lange bis alle eingetragenen Quiz-Teilnehmer eine Antwort abgegeben haben, wodurch auf dem unteren rechten Seitenteil ein TextBlock mit der richtigen Antwort und zwei Buttons sichtbar werden.
Der linke Knopf führt zurück zur 2. Ansicht, wo die nächste Runde dieses angefangenen Quizspiels gespielt werden kann, 
der rechte Knopf führt zurück zur Startseite, wo die Teilnehmerliste neu zusammengesetzt und ein neues Spiel begonnen werden kann.


-- Generelle Hinweise --
Ich übergebe jegliche Verantwortung für die Korrektheit der Fragen und Antworten an die ChatGPT-KI.
Ebenso gegen die Erstellung derselben Fragen zum gleichen Thema kann ich nichts unternehmen.
Auch für unverhältnismäßig einfache oder schwere Fragen der einzelnen Schwierigkeitsgrade und leite ich die Verantwortung an OpenAI weiter.
Letztlich weise ich jegliche Schuld für Formatierungsprobleme und daraus resultierende Runden- oder Programmabbrüche ab. 

Ich habe viel Zeit darin investiert all dies zu vermeiden, kann aber leider den Quelltext der KI nicht umschreiben :)