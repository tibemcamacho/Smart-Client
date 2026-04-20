using System.Collections.Generic;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class UsersTokenDTO
    {
        public List<UserTokenDTO> UsersToken { get; set; } = new List<UserTokenDTO>();
    }

    public class UserTokenDTO
    {
        public string ConfigFile { get; set; }
        public string UserToken { get; set; }
    }
}
