namespace Sundarban.Modules.Customers.Domain;

public class Customer
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } =  string.Empty;
    public DateTime CreatedAt { get; private set; }
    
    private Customer () {}

    public static Customer Create(string firstName, string lastName, string email)
    {
        if (string.IsNullOrEmpty(firstName)) throw new ArgumentException("First Name is required");
        if (string.IsNullOrEmpty(email)) throw new ArgumentException("Email is required");

        return new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            CreatedAt = DateTime.UtcNow
        };
    }
}