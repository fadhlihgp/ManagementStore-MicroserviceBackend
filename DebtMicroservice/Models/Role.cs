namespace DebtMicroservice.Models;

public partial class Role
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual IEnumerable<Account>? Accounts { get; set; } = new List<Account>();
}
