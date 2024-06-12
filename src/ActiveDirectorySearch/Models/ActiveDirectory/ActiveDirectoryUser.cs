using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.ActiveDirectorySearch.Models.ActiveDirectory
{
    public partial class ActiveDirectoryUser : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        string userName = string.Empty;

        [ObservableProperty]
        string unit = string.Empty;

        [ObservableProperty]
        string email = string.Empty;

        [ObservableProperty]
        string firstname = string.Empty;

        [ObservableProperty]
        string lastname = string.Empty;

        [ObservableProperty]
        string status = string.Empty;

        [ObservableProperty]
        string phone = string.Empty;

        [ObservableProperty]
        string plant = string.Empty;
        #endregion

        #region Constructor
        public ActiveDirectoryUser() { }
        #endregion

        #region Overrides
        public override string ToString() => UserName + ": {" + Lastname + ", " + Firstname + "}";


        public override int GetHashCode() => base.GetHashCode();
        
        public override bool Equals(object? obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ActiveDirectoryUser user = (ActiveDirectoryUser)obj;
                return (UserName == user.UserName) && (Firstname == user.Firstname)
                    && (Lastname == user.Lastname) && (Email == user.Email)
                    && (Unit == user.Unit) && (Status == user.Status)
                    ;
            }
        }
        #endregion
    }
}
