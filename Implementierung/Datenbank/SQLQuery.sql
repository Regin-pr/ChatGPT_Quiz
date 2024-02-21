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

--Die Datenbank behandelt Themen und Texte, die vom Nutzer eingegeben werden in Form von Eintr‰gen in der "Thema_Text" Tabelle gleich
CREATE TABLE Thema_Text(
	ThemaID INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	Bezeichnung nvarchar(4000)
)

CREATE TABLE Frage(
	FrageID INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	ThemaID INT FOREIGN KEY REFERENCES Thema_Text(ThemaID),
	Text nvarchar(200),
	--Schwierigkeit von 1-3 Leicht-Schwer (und vielleicht mehr) (so eingrenzen, dass nur diese Werte mˆglich sind)
	Schwierigkeit INT,
	Punktewert INT
)

CREATE TABLE Antwort(
	FrageID INT FOREIGN KEY REFERENCES Frage(FrageID),
	Buchstabe char(1),
	CONSTRAINT PK_FrageBuchstabe PRIMARY KEY (FrageID, Buchstabe),

	Text nvarchar(200),
	Korrektheit BIT
)


INSERT INTO Teilnehmer(Name, PunkteGesamt) VALUES
('Regin',0), ('Schmegin',0), ('Isabella',0), ('Walter',800), ('Martin',200);

INSERT INTO Thema_Text VALUES
('Fussball'),
('Griechische Mythologie'),
('Tiefseefische sind Fische, die an das Leben in Meerestiefen unter ca. 500 Meter angepasst sind.
Die Tiefsee ist gekennzeichnet durch eine Wassertemperatur von knapp 4 ∞C, Abwesenheit von Pflanzenwuchs und nahezu vollst‰ndige Dunkelheit. 
Im Laufe der Evolution haben die Tiefseefische Anpassungen an diese extreme Umwelt entwickelt. 
Bemerkenswert ist dabei, dass die besonderen Merkmale der Tiefseefische h‰ufig unabh‰ngig voneinander in nicht n‰her verwandten Gattungen in sehr ‰hnlicher Weise entstanden (Konvergenz).
Die grˆﬂte Tiefe, in der jemals ein Fisch beobachtet wurde, betr‰gt 8336 m (Stand April 2023). 
In dieser Tiefe wurde im September 2022 im Boningraben (auch Izu-Ogasawara-Graben) sÅElich von Japan ein einzelner Fisch aus der Familie der Scheibenb‰uche (Liparidae) beobachtet. 
Dies ist 148 Meter tiefer als die bisherige letzte Sichtung eines Exemplars dieser Gattung im Marianengraben im August 2017. 
Damit ÅEerschreitet das beobachtete Exemplar die bisher fÅE Fische angenommene maximal erreichbare ‹berlebenstiefe von 8200 m um 136 Meter');

INSERT INTO Frage VALUES
(1, 'Welche Farbe haben die Karten, die Schiedsrichter im Fuﬂball zeigen, um einen Spieler zu verwarnen?', 1, 100),
(3, 'Welche ist die bisher grˆﬂte beobachtete Tiefe, in der ein Fisch entdeckt wurde, und wo wurde diese Entdeckung gemacht?', 3, 500),
(2, 'Welche griechische Gˆttin ist die Gˆttin der Weisheit, des Krieges und der Strategie?', 2, 300);

INSERT INTO Antwort VALUES
(1, 'A', 'Blau', 0),
(1, 'B', 'Gelb', 1),
(1, 'C', 'GrÅE', 0),
(1, 'D', 'Rot', 0),

(2, 'A', '7200 Meter im Marianengraben', 0),
(2, 'B', '8100 Meter im Boningraben', 0),
(2, 'C', '8336 Meter im Boningraben', 1),
(2, 'D', '9000 Meter im Sunda-Graben', 0),

(3, 'A', 'Hera', 0),
(3, 'B', 'Aphrodite', 0),
(3, 'C', 'Athena', 1),
(3, 'D', 'Artemis', 0);

SELECT * FROM Teilnehmer;
SELECT * FROM Thema_Text;
SELECT * FROM Frage;
SELECT * FROM Antwort WHERE FrageID = 2;
