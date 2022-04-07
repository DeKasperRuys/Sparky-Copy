# Developer Manual

## Introductie

In deze manual wordt beknopt uitgelegd hoe de app werkt en hoe de app is opgebouwd, van de technologie, tot de structuur en hoe je moet debuggen.

## Technologieën
***

### Globale Architectuur

[![architectuur](https://i.gyazo.com/d1327911f7dfaaf0609ad8acaf366f1a.png)](https://gyazo.com/d1327911f7dfaaf0609ad8acaf366f1a)
### Angular

Angular is een open source webapplicatieframework dat wordt onderhouden door Google en een collectie van individuele ontwikkelaars en bedrijven die bezig zijn om de mogelijkheden voor het ontwikkelen van Single Page Applications te verbeteren. Het doel is het vereenvoudigen van zowel de ontwikkeling als het testen van dergelijke applicaties door het aanbieden van een framework voor client-side model-view-controller (MVC)-architectuur, samen met componenten die gewoonlijk worden gebruikt in rich internet applications.

Het framework werkt door eerst de HTML-pagina te lezen, waarin aanvullende specifieke HTML-attributen zijn opgenomen. Die attributen worden geïnterpreteerd als directieven die ervoor zorgen dat Angular invoer- of uitvoercomponenten van de pagina koppelt aan een model dat wordt weergegeven door middel van standaard JavaScript-variabelen. De waarden van die JavaScript-variabelen kunnen worden ingesteld binnen de code, of worden opgehaald uit statische of dynamische JSON-dataobjecten.

### ASP.NET Core 2.2 API

.NET Core is een gratis open-source web framework dat ontwikkeld is door Microsoft.De eerste versie van .NET Core is uitgebracht in 2016 in een update van Visual Studio 2015 en is sneller dan ASP.NET.
Een jaar later kwam .NET Core 2.0 uit, dit was nog sneller dan het origineel. Bij de nieuwe versie werden er meer dan 20.000 API's ondersteund wat ook niet in het origineel zat.

Het voordeel van .NET Core is dat het razend snel is op jouw hosting.Microsoft heeft vorig jaar Bing overgezet naar .NET Core waarna Bing 34 procent sneller is geworden.
Omdat .NET Core nog een vrij nieuw framework is wordt nog niet elke .NET library ondersteund.Als dit het geval is raden we je aan om gewoon gebruik te maken van het .NET Framework.

### MSSQL

Microsoft SQL Server is een relationeel databasebeheersysteem ontwikkeld door Microsoft. Het ondersteunt T-SQL, een dialect van SQL, de meest gebruikte databasetaal. Het wordt algemeen gebruikt door organisaties voor kleine tot middelgrote databases.

### Python

Python is een programmeertaal die begin jaren 90 ontworpen en ontwikkeld werd door Guido van Rossum, destijds verbonden aan het Centrum voor Wiskunde en Informatica (daarvoor Mathematisch Centrum) in Amsterdam. De taal is mede gebaseerd op inzichten van professor Lambert Meertens die een taal genaamd ABC had ontworpen, bedoeld als alternatief voor BASIC, maar dan met geavanceerde datastructuren. Inmiddels wordt de taal doorontwikkeld door een enthousiaste groep, tot juli 2018 geleid door Van Rossum. Deze groep wordt ondersteund door vrijwilligers op het internet. De ontwikkeling van Python wordt geleid door de Python Software Foundation. Python is vrije software.

### Azure

Microsoft Azure is een set cloudservices die constant blijft groeien en uw organisatie helpt te voorzien in allerlei zakelijke uitdagingen. Azure geeft u de vrijheid om met uw favoriete hulpprogramma's en frameworks, toepassingen te ontwikkelen, beheren en implementeren op een omvangrijk, wereldwijd netwerk.

### Swagger

Swagger is een open-source softwareframework ondersteund door een hoop development tools waarmee ontwikkelaars RESTful-webservices kunnen ontwerpen, bouwen, documenteren en consumeren.

### Firebase

Firebase is een complete suite met app-ondersteunende tools. Zo omvat Firebase onder meer Analytics, developer tools en AdMob-integratie.

Firebase biedt eenvoudige tools voor analytics, realtime messaging, monitoring, data-opslag, data-hosting, remote beheer, een testomgeving, gebruiker-authenticatie en integratiemogelijkeden voor monetizing platformen.

Sparky gebruikt vooral Firebase authenticatie en security.

## App runnen vanuit angular
***

Om de app te runnen vanaf uw computer binnen angular dient u de volgende zaken te ondernemen.

### Install Node

Eerst en vooral dient Node geïnstalleerd te worden. Dit kan via de volgende link: https://nodejs.org/en/download/

### Install Typescript

Van zodra node geïnstalleerd is kunnen we node packages installeren met behulp van de Node Package manager. Run het commando "npm install -g typescript" om typescript te installeren.

### Install angular

Voer het volgende commando uit in de command prompt om angular te installeren:
"npm install -g @angular/cli”

### Visual Studio code

Installeer visual studio code via de volgende link: https://code.visualstudio.com

### Run App

Open de ClientApp folder van Sparky met Visual Studio Code. Klik vervolgens op "Terminal>New Terminal" en voor het commando "ng serve" uit.

De app front-end zal lokaal op uw pc runnen.
