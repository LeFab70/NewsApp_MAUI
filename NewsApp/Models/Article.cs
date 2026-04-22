namespace NewsApp.Models;

public class Article
{
    public string Titre { get; set; } = "";
    public string Description { get; set; } = "";
    public string Contenu { get; set; } = "";
    public string Image { get; set; } = "";
    public string Source { get; set; } = "";
    public string Date { get; set; } = "";
    public string SourceEtDate => $"{Source} / {Date}";

    // Utilisé pour la navigation vers la page de détails
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
}

