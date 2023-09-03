namespace DebtMicroservice.Models;

public partial class Store
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual IEnumerable<Customer>? Customers { get; set; } = new List<Customer>();

    public virtual IEnumerable<Debt>? Debts { get; set; } = new List<Debt>();

    public virtual IEnumerable<Delivery>? Deliveries { get; set; } = new List<Delivery>();

    public virtual IEnumerable<Expense>? Expenses { get; set; } = new List<Expense>();

    public virtual IEnumerable<Member>? Members { get; set; } = new List<Member>();

    public virtual IEnumerable<Product>? Products { get; set; } = new List<Product>();

    public virtual IEnumerable<Purchase>? Purchases { get; set; } = new List<Purchase>();
}
