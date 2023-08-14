using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountAuthMicroservice.Entities;

[Table("Store")]
public class Store
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public virtual ICollection<Member>? Members { get; set; }
}