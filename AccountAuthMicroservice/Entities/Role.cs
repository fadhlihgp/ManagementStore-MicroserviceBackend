using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountAuthMicroservice.Entities;

[Table(name:"Role")]
public class Role
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Account>? Accounts { get; set; }
}