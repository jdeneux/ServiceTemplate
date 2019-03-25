using jwtApi.Core.Application.Interfaces.Mapping;
using jwtApi.Core.Domain.Entities;

namespace jwtApi.Core.Application.Users.Models
{
    public class UserViewModel : IMapFrom<User>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
    }
}
