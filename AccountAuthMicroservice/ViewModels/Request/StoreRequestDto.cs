using System.ComponentModel.DataAnnotations;

namespace AccountAuthMicroservice.ViewModels.Request;

public class StoreRequestDto
{
    public string Id { get; set; }
    [Required(ErrorMessage = "Kolom nama toko tidak boleh kosong")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Kolom alamat tidak boleh kosong")]
    public string Address { get; set; }
}