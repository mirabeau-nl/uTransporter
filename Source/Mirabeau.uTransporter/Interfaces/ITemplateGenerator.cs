namespace Mirabeau.uTransporter.Interfaces
{
    public interface ITemplateGenerator
    {
        int? Generate(string targetPath);

        string CreateTargetPath(string filename, string targetPath);
    }
}