using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountAuthMicroservice.Entities;

[Table(name:"LoginHistory")]
public class LoginHistory
{
    [Key]
    public string Id { get; set; }
    public string AccountId { get; set; }
    public virtual Account Account { get; set; }
    public DateTime LastLogin { get; set; }
}