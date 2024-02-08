using AndreasReitberger.ActiveDirectorySearch.Enums;

namespace AndreasReitberger.ActiveDirectorySearch.Models.Documentation
{
    /// <summary>
    /// Code taken from https://github.com/AndreasReitberger/MauiAppBasement
    /// Copyright: Andreas Reitberger
    /// Changed: Yes
    /// License: Apache-2.0 (https://github.com/AndreasReitberger/MauiAppBasement/blob/main/LICENSE)
    /// </summary>
    public class DocumentationInfo
    {
        #region Properties
        public DocumentationIdentifier Identifier { get; set; }
        public string Path { get; set; }

        #endregion

        #region Constructor
        public DocumentationInfo(DocumentationIdentifier identifier, string path)
        {
            Identifier = identifier;
            Path = path;
        }
        #endregion
    }
}
