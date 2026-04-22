using NewsApp.Models;

namespace NewsApp.Services;

public static class ArticleStore
{
    private static readonly List<Article> _all = new()
    {
        new Article
        {
            Titre = "Politique : Actualité à la chambre des communes",
            Description = "Le Parlement adopte une nouvelle loi.",
            Image = "news1.png",
            Contenu = "Texte complet de l'article... (Politique)\n\nLorem ipsum dolor sit amet, consectetur adipiscing elit.",
            Source = "Radio Canada",
            Date = "2h"
        },
        new Article
        {
            Titre = "Sport : Victoire spectaculaire en prolongation",
            Description = "Un match intense se conclut sur un but décisif.",
            Image = "news2.png",
            Contenu = "Texte complet de l'article... (Sport)\n\nSuspendisse potenti. Integer non mauris ut libero.",
            Source = "RDS",
            Date = "4h"
        },
        new Article
        {
            Titre = "Cinéma : Un film québécois primé à l’international",
            Description = "La production locale reçoit un prix prestigieux.",
            Image = "news3.png",
            Contenu = "Texte complet de l'article... (Cinéma)\n\nCurabitur at metus a risus finibus malesuada.",
            Source = "Le Devoir",
            Date = "6h"
        },
        new Article
        {
            Titre = "Technologie : Une nouvelle IA révolutionne la recherche",
            Description = "Un modèle améliore la pertinence des résultats.",
            Image = "news4.png",
            Contenu = "Texte complet de l'article... (Technologie)\n\nPraesent quis tincidunt erat. Etiam vitae fermentum.",
            Source = "The Verge",
            Date = "8h"
        },
        new Article
        {
            Titre = "Politique : Débat sur le budget et mesures sociales",
            Description = "Les partis discutent des priorités pour l'année.",
            Image = "news5.png",
            Contenu = "Texte complet de l'article... (Politique)\n\nAliquam erat volutpat. Mauris at sapien.",
            Source = "CBC",
            Date = "12h"
        },
        new Article
        {
            Titre = "Sport : Record battu lors du marathon annuel",
            Description = "Une performance historique sous la pluie.",
            Image = "news6.png",
            Contenu = "Texte complet de l'article... (Sport)\n\nDonec blandit, magna vitae tincidunt.",
            Source = "ESPN",
            Date = "1j"
        },
        new Article
        {
            Titre = "Cinéma : Bande-annonce attendue pour un blockbuster",
            Description = "Les fans analysent déjà les premières images.",
            Image = "news7.png",
            Contenu = "Texte complet de l'article... (Cinéma)\n\nSed ut perspiciatis unde omnis iste natus error sit.",
            Source = "IMDb",
            Date = "2j"
        }
    };

    public static IReadOnlyList<Article> All => _all;

    public static Article? GetById(string id) =>
        _all.FirstOrDefault(a => string.Equals(a.Id, id, StringComparison.OrdinalIgnoreCase));
}

