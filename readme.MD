# Ziekenhuis casus Security & BCM - data generator

Binnen de Ziekenhuis casus van de module Security & BCM (opleiding HBO-ICT 2e jaar keuzemodule periode 3)
is het handig als er ook representatieve gegevens gebruikt worden. Hiertoe is een applicatie ontwikkeld in C#.

## Creatie van de database
Eerst moet de database aangemaakt worden. Hiervoor is een `.SQL`-file beschikbaar in de repository map `database`.

Het aanmaken kan bijvoorbeeld door een `mysql`-prompt te starten in de virtuele machine:

```bash
  $ sudo mysql 
```

of indien al wachtwoorden zijn toegekend aan bijvoorbeeld de username 'gebruiker':

```bash
  $ sudo mysql -u gebruiker -p 
```

Eenmaal in MySQL kan dan de inhoud van een bestand aangeroepen worden met het `source` commando:

```bash
  mysql [(none)]> source /home/student/CreateDatabase.sql  
```

Uiteraard is het ook mogelijk om via je IDE (Rider / VScode) verbinding te maken via de `root`-user en 
zo het bestand uit te voeren.

De eerste 4 statements (zie ook hier onder) hoeven waarschijnlijk maar één keer uitgevoerd te worden. Eventueel
kan het (nogal onveilige) wachtwoord aangepast worden.

``` SQL
CREATE DATABASE IF NOT EXISTS ziekenhuisdata collate utf8_general_ci;
USE ziekenhuisdata;

CREATE OR REPLACE USER 'ziekenhuis'@'localhost' IDENTIFIED WITH mysql_native_password AS PASSWORD('ziekenhuis');
grant all on ziekenhuisdata.* to 'ziekenhuis'@'localhost';

```

Daarna kunnen de tabellen aangemaakt worden met de `CREATE TABLE`-statements.

Daarna zijn er een aantal standaard waarden die ingevoerd worden in de tabellen, zoals 
  * verzekeraars 
  * behandelaars
  * opleidingsniveau
  * leefstijl

Als laatste wordt een `view` gemaakt waarin de tabellen aan elkaar gekoppeld worden.

### Bronnen
* [MySQL login](https://dev.mysql.com/doc/refman/8.0/en/connecting.html)
* [MySQL statements](https://dev.mysql.com/doc/refman/8.0/en/mysql-commands.html)

## Starten van het programma

Het programma heeft de volgende afhankelijkheden:
  * Er kan verbinding gemaakt worden met de beoogde database
  * Het `dotnet`-framework versie 6 is geinstalleerd

Om de verbinding met de database te configureren moet er een aanpassing in het bestand `MySQLDBContext.cs` gemaakt
worden. Deze is te vinden in de projectmap `database`. 

In het voorbeeld bestand wordt verwacht dat het bestand op de eigen machine draait (ipv bijvoorbeeld op jouw VM) 
en toegang mogelijk is via gebruikersnaam (`Uid`) 'ziekenhuis' en wachtwoord (`Pwd`) 'ziekenhuis'. 

```c#
connection = new MySqlConnection();
connection.ConnectionString = "Server=localhost;Database=ziekenhuisdata;Uid=ziekenhuis;Pwd=ziekenhuis";
```

Het programma is eenvoudig te starten vanuit een IDE of vanaf de commando-prompt via `dotnet`.

## Aanpassing in het aantal te genereren gegevens
Eventueel kan het programma meer of minder gegevens aanmaken. Er zijn twee parameters die instelbaar zijn:
  * aantal patienten
  * aantal meetwaarden per patient

Deze aanpassingen moeten gedaan in het bestand `Program.cs`. 

Onderstaande regel maakt 50 patienten aan. 

```c#
   List<Patient> patients = repo.CreateRandomPatienten(50);
```

Onderstaande regel kent twee parameters. Het mininum en maximum van het random aantal meetwarden dat per patient
aangemaakt moet worden. In onderstaande voorbeeld worden minimaal 50 en maximaal 200 meetwaarden gegenereerd. 
```c#
    patient.Meetwaarden = repo.GenerateMeetwaardenForPatient(patient, 50, 200);
```

Martin Molema
februari 2023
