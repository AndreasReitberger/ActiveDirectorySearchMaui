using AndreasReitberger.ActiveDirectorySearch.Enums;

namespace AndreasReitberger.ActiveDirectorySearch.Models.Documentation
{
    /// <summary>
    /// Code taken from https://github.com/AndreasReitberger/MauiAppBasement
    /// Copyright: Andreas Reitberger
    /// Changed: Yes
    /// License: Apache-2.0 (https://github.com/AndreasReitberger/MauiAppBasement/blob/main/LICENSE)
    /// </summary>
    public static class DocumentationManager
    {
        public const string DocumentationBaseUrl = @"https://zip.intern.zollner.de//";
        public const string ChangelogBaseUrl = @"https://zip.intern.zollner.de//";
        public const string ProjectUrl = @"https://zznt14v.intern.zollner.de/svn/e1_testengineering/Tools/VisualStudio/01_Tools/URoboControlProgram";

        public const string ProjectSubmitBugUrl = @$"{ProjectUrl}";
        public const string ProjectSubmitFeatureUrl = @$"{ProjectUrl}";

        public static List<DocumentationInfo> List => new()
        {
            new DocumentationInfo(DocumentationIdentifier.SetupConnection, @"einrichten-loslegen/verbindung-mit-repetier-server-einrichten/"),
            new DocumentationInfo(DocumentationIdentifier.Usage, @"einrichten-loslegen/verbindung-mit-repetier-server-einrichten/"),
        };

        public static string CreateUrl(DocumentationIdentifier documentationIdentifier)
        {
            DocumentationInfo info = List.FirstOrDefault(x => x.Identifier == documentationIdentifier);
            string url = DocumentationBaseUrl;
            if (info != null)
            {
                url += info.Path;
            }
            return url;
        }
    }
}
