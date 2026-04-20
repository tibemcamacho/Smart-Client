using System.Collections.Generic;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class UsersDTO
    {
        public List<SettingsUsersDTO> SettingsUsers { get; set; } = new List<SettingsUsersDTO>();
    }

    public class SettingsUsersDTO
    {
        public string ConfigFile { get; set; }
        public UserDTO User { get; set; }
    }

}
