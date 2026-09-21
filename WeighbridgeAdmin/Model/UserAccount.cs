using System;

namespace WeighbridgeAdmin.Model
{
    /// <summary>
    /// Operator login.  Plain data holder - the profile name is joined on by
    /// the repository so the sign on screen and the status bar do not have to
    /// go back for it.  There is no password: the weighbridge office is a shop
    /// floor terminal and the operator just picks their own name.
    /// </summary>
    public class UserAccount
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public bool IsActive { get; set; }

        public UserAccount()
        {
            this.UserName = "";
            this.FullName = "";
            this.ProfileName = "";
            this.IsActive = true;
        }

        // Used by the operator combo box on the sign on screen.
        public override string ToString()
        {
            return this.FullName;
        }
    }
}
