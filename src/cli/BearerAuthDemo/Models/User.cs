using System;

namespace BearerAuthDemo;

public class User
{
    public Guid UserIdentifier { get; set; }

    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
