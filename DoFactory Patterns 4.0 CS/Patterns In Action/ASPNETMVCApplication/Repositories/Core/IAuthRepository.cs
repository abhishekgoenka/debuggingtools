using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASPNETMVCApplication.Repositories.Core
{
    /// <summary>
    /// Authentication Repository interface.
    /// Note: does not derive from IRepository.
    /// </summary>
    public interface IAuthRepository 
    {
        string GetToken();
        bool Login(string username, string password);
        bool Logout();
    }
}
