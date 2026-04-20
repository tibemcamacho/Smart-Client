
namespace Elipgo.SmartClient.Common.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public long UserId { get; set; }
        public long EntityId { get; set; }
        public long RoleId { get; set; }
        public string Token { get; set; }
        public bool IsWorkingHours { get; set; }
        public string MatrixHours { get; set; }
        public string UserIdGuid { get; set; }
        public string UserRoles { get; set; }
        public bool LogOutOnInactivity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PreviusLastLogin { get; set; }


    }
}
