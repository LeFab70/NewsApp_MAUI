# NewsAppMVVM_Fab — NewsApp+ (Projet 01)

**Auteur**: Fabrice  
**Cours**: PROG1342  

## Description (but du projet)

Ce projet est une évolution de **NewsApp (Devoir 03)** vers **NewsApp+**, une application **.NET MAUI** qui :

- récupère de vraies nouvelles via **NewsAPI.org** (`top-headlines`, Canada)
- applique une architecture **MVVM** (sans CommunityToolkit, `INotifyPropertyChanged` manuel)
- gère un **fallback hors-ligne** (articles locaux) + bandeau *"Hors ligne"*
- supporte la **recherche en temps réel** + compteur de résultats
- propose une **page détail** (image, titre, source/date, contenu) + boutons **Lire** / **Partager**

## Structure

- `NewsApp/` : projet MAUI
  - `Models/` : `Article`, `NewsApiReponse`
  - `Services/` : `NewsApiService` (API + fallback), `ArticleStore` (fallback local)
  - `ViewModels/` : `NewsViewModel`
  - `Views/` : `DetailPage`, `FavoritesPage`, `ProfilePage`

## Configurer la clé API (obligatoire)

La clé NewsAPI est lue depuis :

- `NewsApp/Constants.Local.cs` → `ConstantsLocal.NewsApiKey`

Pour tester avec ta propre clé, modifie cette ligne :

```csharp
public const string NewsApiKey = "TA_CLE_ICI";
```

## Lancer l’application (Android)

Depuis la racine du workspace :

```bash
cd "NewsAppMVVM_Fab/NewsApp"
dotnet build "NewsApp.csproj" -c Debug -f net10.0-android
dotnet build "NewsApp.csproj" -t:Run -c Debug -f net10.0-android
```

