using CoreFramework.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLibrary.CoreFramework.WebConfigInfo
{
    public interface ICustomerPrincipal
    {
        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <value>The user profile.</value>
        CustomerProfile CustomerProfile { get; }
        /// Get the state of current user login.
        /// </summary>
        bool IsCustomerLoginIn { get; }
    }
}
