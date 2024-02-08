using System.DirectoryServices;

namespace AndreasReitberger.ActiveDirectorySearch.Models.ActiveDirectory
{
    public static class ActiveDirectoryManager
    {
        #region Methods

        public static List<ActiveDirectoryUser> GetUsersFromActiveDirectory(string userNameWildcard)
        {
            List<ActiveDirectoryUser> result = [];
            string filterText = "(&(objectClass=user)(|(SAMAccountName=" + userNameWildcard.Trim() + "*)(GivenName=" + userNameWildcard.Trim() + "*)(SN=" + userNameWildcard.Trim() + "*)))";
            string[] patterns = userNameWildcard.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (patterns?.Length > 1)
            {
                filterText = "(&" +
                    "(objectClass=user)";
                foreach (var filterPart in patterns)
                {
                    filterText +=
                        "(|(SAMAccountName=" + filterPart + "*)(GivenName=" + filterPart + "*)(SN=" + filterPart + "*))";
                }
                // Closing
                filterText += ")";
            }
            DirectorySearcher ds = new()
            {
                Asynchronous = true,
                Filter = filterText,
            };
            SearchResultCollection users = ds.FindAll();
            if (users is not null)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    try
                    {
                        SearchResult entry = users[i];
#if DEBUG
                        string department = entry.Properties["department"].Count > 0 ? entry.Properties["department"][0].ToString() : "";
                        string city = entry.Properties["l"].Count > 0 ? entry.Properties["l"][0].ToString() : "";
                        string country = entry.Properties["l"].Count > 0 ? entry.Properties["l"][0].ToString() : "";
#endif
                        ActiveDirectoryUser temp = new()
                        {
                            UserName = entry.Properties["samaccountname"].Count > 0 ? entry.Properties["samaccountname"][0].ToString() : "",
                            Unit = entry.Properties["physicaldeliveryofficename"].Count > 0 ? entry.Properties["physicaldeliveryofficename"][0].ToString() : "",
                            Plant = entry.Properties["l"].Count > 0 ? entry.Properties["l"][0].ToString() : "",
                            Firstname = entry.Properties["givenname"].Count > 0 ? entry.Properties["givenname"][0].ToString() : "",
                            Lastname = entry.Properties["sn"].Count > 0 ? entry.Properties["sn"][0].ToString() : "",
                            Email = entry.Properties["mail"].Count > 0 ? entry.Properties["mail"][0].ToString() : "",
                            Phone = entry.Properties["telephonenumber"].Count > 0 ? entry.Properties["telephonenumber"][0].ToString() : "",
                        };
                        result.Add(temp);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            return result;
        }

        public static ActiveDirectoryUser GetUserFromActiveDirectory(string userNameWildcard)
        {
            List<ActiveDirectoryUser> users = GetUsersFromActiveDirectory(userNameWildcard);
            return users?.FirstOrDefault();
        }

        #endregion
    }
}
