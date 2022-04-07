# CI / CD Pipeline

## Azure portal

Zowel de client als de API solution draaien in de Azure cloud omgeving van Microsoft. Hierbij zijn 2 App Services aangemaakt waar de 2 applicaties op draaien, 1 voor de API (Sparky1920API) en 1 voor de client (Sparky1920). Omdat de Sparky app gebruik maakt van een database zal ook een server draaien waarop de sparky database gehost is.

[![Image from Gyazo](https://i.gyazo.com/40e7d9543421f52df61d032ff719ead9.png)](https://gyazo.com/40e7d9543421f52df61d032ff719ead9)

## Git

Tijdens ontwikkeling is gewerkt volgens het git workflow principe waarbij elke feature los van de master en development branch een eigen branch zal krijgen. Wanneer de feature ontwikkeld is zal deze eerst in development worden gemerged waarna via een pull request de toegevoegde code door mede developers kan gereviewed worden. 

## CI pipeline

Dit project bestaat zoals eerder vermeld uit een development en een master branch, op beide branches is een CI pipeline opgesteld m.b.v.
Azure DevOps. Omdat geen echte production omgeving aanwezig is wordt enkel de CI pipeline van de master branch toegelicht. 
De github repository van het project is gelinkt aan de AzureDevops, dit laat vervolgens toe de master branch te koppelen aan de build pipeline m.a.w. wanneer naar de master branch gepushed wordt zal de build pipeline getriggered worden en zal een build starten.

### Build tasks

De volledige pipeline bestaat uit een aantal geconfigureerde task die ervoor zorgen dat alle packages worden hersteld en de client en api applicatie gebuild kunnen worden. onderstaande screenshot toont de tasks geconfigureerd voor dit project. Wanneer deze tasks zijn uitgevoerd zal van de client en de api solution Artifacts worden gemaakt. Deze artifacts zijn gestagede versies van de solutions klaar voor release. 

[![Image from Gyazo](https://i.gyazo.com/b3a7279388e68867e542c8c97aeeea75.png)](https://gyazo.com/b3a7279388e68867e542c8c97aeeea75)

[![Image from Gyazo](https://i.gyazo.com/73427e5ad252fad85ae7821e407ce5ec.png)](https://gyazo.com/73427e5ad252fad85ae7821e407ce5ec)

## CD pipeline

Nadat een build geslaagd is zonder foutmeldingen zal een release getriggered worden. Hierbij zal Azure DevOps de artifacts van de client en api solution nemen en deze gebruiken om te deployed naar de bijhorende azure resources. 

[![Image from Gyazo](https://i.gyazo.com/a79ce095534524a34676e4ae2346bdb4.png)](https://gyazo.com/a79ce095534524a34676e4ae2346bdb4)

[![Image from Gyazo](https://i.gyazo.com/947fa7704652a2802486c5f1666a6d4a.png)](https://gyazo.com/947fa7704652a2802486c5f1666a6d4a)


