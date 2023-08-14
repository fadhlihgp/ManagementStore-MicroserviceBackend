using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountAuthMicroservice.Entities;

[Table(name:"Member")]
public class Member
{
    [Key] public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string StoreId { get; set; } = string.Empty;
    public virtual Store Store { get; set; }
}