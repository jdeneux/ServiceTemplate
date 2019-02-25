using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwtApi.Dto
{
    public class UserAuthenticationDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserAuthenticatedDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }

    public class UserNewRegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
