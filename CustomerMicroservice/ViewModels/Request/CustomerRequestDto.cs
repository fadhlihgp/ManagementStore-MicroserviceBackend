using System.ComponentModel.DataAnnotations;

namespace CustomerMicroservice.ViewModels.Request;

public class CustomerRequestDto
{
    [Required(ErrorMessage = "Nama tidak boleh kosong")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Nomor ponsel tidak boleh kosong")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Alamat tidak boleh kosong")]
    public string Address { get; set; }
}