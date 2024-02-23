USE master;
GO

IF DB_ID('GPTQuiz_DB') IS NULL
CREATE DATABASE GPTQuiz_DB;
GO

USE GPTQuiz_DB;
GO

IF OBJECT_ID('Antwort') IS NOT NULL
DROP TABLE Antwort;

IF OBJECT_ID('Frage') IS NOT NULL
DROP TABLE Frage;

IF OBJECT_ID('Thema_Text') IS NOT NULL
DROP TABLE Thema_Text;

IF OBJECT_ID('Teilnehmer') IS NOT NULL
DROP TABLE Teilnehmer;

CREATE TABLE Teilnehmer(
	TeilnehmerID INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	Name nvarchar(15),
	PunkteGesamt INT DEFAULT 0,
	PunkteRunde INT DEFAULT 0
)

--Die Datenbank behandelt Themen und Texte, die vom Nutzer eingegeben werden in Form von Eintr臠en in der "Thema_Text" Tabelle gleich
CREATE TABLE Thema_Text(
	ThemaID INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	Bezeichnung nvarchar(4000),
	IstThema BIT
)

CREATE TABLE Frage(
	FrageID INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	ThemaID INT FOREIGN KEY REFERENCES Thema_Text(ThemaID),
	Text nvarchar(200),
	--Schwierigkeit von 1-3, Leicht-Schwer (so eingrenzen, dass nur diese Werte mlich sind)
	Schwierigkeit INT,
)

CREATE TABLE Antwort(
	FrageID INT FOREIGN KEY REFERENCES Frage(FrageID),
	Buchstabe char(1),
	CONSTRAINT PK_FrageBuchstabe PRIMARY KEY (FrageID, Buchstabe),
	Text nvarchar(200),
	Korrektheit BIT
)


--INSERT INTO Teilnehmer(Name) VALUES
--('Regin'), ('Schmegin'), ('Isabella'), ('Walter');

INSERT INTO Thema_Text(Bezeichnung, IstThema) VALUES
('Fussball',1),
('Griechische Mythologie',1),
('OpenAI, Inc. ist ein US-amerikanisches Unternehmen der Rechtsform einer Public Charity (501(c), Nonprofit), das sich mit der Erforschung von künstlicher Intelligenz (KI, englisch Artificial Intelligence, AI) beschäftigt. Die gewinnorientierte Tochtergesellschaft OpenAI Global, LLC wird dabei durch das Non-Profit-Mutterunternehmen OpenAI, Inc. kontrolliert.

Open AI, Inc. (Nonprofit) wurde ursprünglich ausschließlich von Spenden finanziert.[4] Anfänglich war das Ziel von OpenAI, künstliche Intelligenz auf Open-Source-Basis auf eine Art und Weise zu entwickeln und zu vermarkten, dass sie der Gesellschaft Vorteile bringt und nicht schadet. Die Organisation wollte eine „freie Zusammenarbeit“ mit anderen Institutionen[5] und Forschern, indem sie ihre Patente und Forschungsergebnisse für die Öffentlichkeit zugänglich mache.[6]

Nachdem die bis 2019 durch Spenden erworbenen finanziellen Mittel nur etwa 130 Millionen US-Dollar erreichten und nicht mehr für die Weiterentwicklung der Organisation ausreichten, wurde die gewinnorientierte Tochterfirma OpenAI Global, LLC gegründet. Dabei handelt es sich um ein Unternehmen, für dessen Investoren die Erträge begrenzt sind (englisch Capped profit company). Allerdings ist die Begrenzung der zukünftig ausbezahlten Erträge sehr hoch angesetzt, indem sie bis zum Hundertfachen der ursprünglichen Investition ansteigen dürfen.[7] Open AI, Inc. übt weiterhin die Kontrolle über diese Tochterfirma aus und erhält deren allfällig überschüssige Erträge.[1]

Wichtiger Geldgeber von OpenAI Global, LLC ist das Unternehmen Microsoft. Seit Mitte 2023 ist Microsoft der bedeutendste Geldgeber mit Investitionen von 13 Milliarden US-Dollar und Hauptaktionär.[8]

OpenAI beschäftigt sich nach eigenen Angaben mit der Frage der „existenziellen Bedrohung durch künstliche Intelligenz“ – also dem möglichen Übertreffen und Ersetzen der menschlichen durch künstliche Intelligenz (KI). Die Organisation dient bis jetzt nur der Erforschung künstlicher Intelligenz. Im Februar 2018 verließ Elon Musk die Leitung von OpenAI, um angeblich Interessenkonflikte mit Teslas Entwicklungen im Bereich künstliche Intelligenz zu vermeiden. Er bleibt weiterhin Spender und Berater der Organisation.

Anfang 2023 beschäftigte Open AI Global, LLC 375 Mitarbeiter.',0);

INSERT INTO Frage VALUES
(1, 'Welche Farbe haben die Karten, die Schiedsrichter im Fuﾟball zeigen, um einen Spieler zu verwarnen?', 1),
(3, 'Wer ist der bedeutendste Geldgeber von OpenAI Global, LLC, seit Mitte 2023?', 3),
(2, 'Welche griechische Göttin ist die Göttin der Weisheit, des Krieges und der Strategie?', 2);

INSERT INTO Antwort VALUES
(1, 'A', 'Blau', 0),
(1, 'B', 'Gelb', 1),
(1, 'C', 'Grün', 0),
(1, 'D', 'Rot', 0),

(2, 'A', 'Google', 0),
(2, 'B', 'Microsoft', 1),
(2, 'C', 'Tesla', 0),
(2, 'D', 'Facebook', 0),

(3, 'A', 'Hera', 0),
(3, 'B', 'Aphrodite', 0),
(3, 'C', 'Athena', 1),
(3, 'D', 'Artemis', 0);

SELECT * FROM Teilnehmer;
SELECT * FROM Thema_Text;
SELECT * FROM Frage;
SELECT * FROM Antwort;

SELECT * FROM Antwort WHERE FrageID = 2;
