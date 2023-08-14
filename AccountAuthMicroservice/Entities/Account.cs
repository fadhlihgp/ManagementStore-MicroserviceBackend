using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountAuthMicroservice.Entities;

[Table(name:"Account")]
public class Account
{
    [Key]
    public string Id { get; set; }
    
    public string UserName { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string NoHp { get; set; }
    
    public string RoleId { get; set; }
    
    public virtual Role Role { get; set; }
    
    public string MemberId { get; set;}
    
    public virtual Member Member { get; set; }
    
    public bool IsActive { get; set; }
}