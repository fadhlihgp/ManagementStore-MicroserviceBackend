using System;
using System.Collections.Generic;

namespace AccountAuthMicroservice.Models;

public partial class Account
{
    public string Id { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string NoHp { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public string MemberId { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<LoginHistory> LoginHistories { get; set; } = new List<LoginHistory>();

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<Product> ProductCreatedAccounts { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductEditedAccounts { get; set; } = new List<Product>();

    public virtual Role Role { get; set; } = null!;
}
