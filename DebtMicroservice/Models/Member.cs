namespace DebtMicroservice.Models;

public partial class Member
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string StoreId { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual IEnumerable<Account>? Accounts { get; set; } = new List<Account>();

    public virtual Store Store { get; set; } = null!;
}
