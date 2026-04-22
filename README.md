# NewsApp (Devoir 03)

**Auteur**: Fabrice  
**Date**: 22 avril 2026  

## But de l'application

`NewsApp` est une application **.NET MAUI** inspirée du style **Feedly** permettant de visualiser une liste d’articles fictifs avec :

- recherche via `SearchBar`
- filtrage par catégories (Politique, Sport, Cinéma, Technologie)
- navigation (barre inférieure Home / Favoris / Profil)
- ouverture d’un **détail d’article** au clic sur une carte

## Structure

- `NewsApp/` : projet .NET MAUI
- `NewsApp/Models/Article.cs` : modèle `Article`
- `NewsApp/Services/ArticleStore.cs` : données fictives
- `NewsApp/MainPage.xaml` : UI (SearchBar, filtres, CollectionView, NavBar)
- `NewsApp/Pages/` : pages Favoris / Profil / Détails
- `NewsApp/Resources/Images/` : `home.png`, `favoris.png`, `profil.png`, `news*.png`

## Commandes pour lancer

Depuis la racine du dossier (là où se trouve ce `README.md`) :

### Build Android

```bash
dotnet build "NewsApp/NewsApp.csproj" -c Debug -f net10.0-android
```

### Lancer sur émulateur/appareil Android

```bash
dotnet build "NewsApp/NewsApp.csproj" -t:Run -c Debug -f net10.0-android
```
