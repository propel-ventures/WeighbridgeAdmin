using System;

namespace WeighbridgeAdmin.Model
{
    /// <summary>
    /// Job role.  The privileges that go with the role live in
    /// dbo.SecurityProfilePrivileges - a user gets whatever their profile grants.
    /// </summary>
    public class SecurityProfile
    {
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string Description { get; set; }

        public SecurityProfile()
        {
            this.ProfileName = "";
            this.Description = "";
        }

        public override string ToString()
        {
            return this.ProfileName;
        }
    }
}
