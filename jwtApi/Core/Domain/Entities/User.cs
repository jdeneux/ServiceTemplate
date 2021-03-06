﻿#nullable enable

namespace jwtApi.Core.Domain.Entities
{
    public class User
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? Token { get; set; }
        public Role? Role { get; set; }
    }

    public enum Role
    {
        Admin,
        User,
        Limited
    }
}
