using System;
using System.Collections.Generic;
using WeighbridgeAdmin.Data;
using WeighbridgeAdmin.Model;

namespace WeighbridgeAdmin.Security
{
    /// <summary>
    /// The signed on session.  One operator per running copy of the program,
    /// so this is static - the sign on dialog fills it in before the main form
    /// is shown and nothing changes it afterwards.  The granted privileges are
    /// read once at sign on; altering a profile in the database does not take
    /// effect until the operator signs on again.
    /// </summary>
    public static class SecurityContext
    {
        private static UserAccount _currentUser;
        private static HashSet<string> _granted = new HashSet<string>();

        /// <summary>
        /// Loads the privileges that go with the user's profile and makes them
        /// the current session.
        /// </summary>
        public static void SignIn(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            HashSet<string> granted = new HashSet<string>();
            List<string> names = Repository.Current.GetProfilePrivileges(user.ProfileId);
            for (int i = 0; i < names.Count; i++)
            {
                granted.Add(names[i]);
            }

            _currentUser = user;
            _granted = granted;
        }

        /// <summary>The signed on operator, or null before sign on.</summary>
        public static UserAccount CurrentUser
        {
            get { return _currentUser; }
        }

        public static string FullName
        {
            get
            {
                if (_currentUser == null)
                {
                    return "";
                }
                return _currentUser.FullName;
            }
        }

        public static string ProfileName
        {
            get
            {
                if (_currentUser == null)
                {
                    return "";
                }
                return _currentUser.ProfileName;
            }
        }

        /// <summary>True when the signed on profile grants the privilege.</summary>
        public static bool HasPrivilege(string privilege)
        {
            return _granted.Contains(privilege);
        }

        /// <summary>
        /// Called at the top of every gated handler.  The controls are disabled
        /// up front as well, so this only fires for the ways into a command
        /// that the screen did not think to switch off - a shortcut key, a
        /// double click, a handler called from another handler.
        /// </summary>
        public static void Demand(string privilege)
        {
            if (!HasPrivilege(privilege))
            {
                throw new UnauthorizedAccessException(privilege);
            }
        }
    }
}
