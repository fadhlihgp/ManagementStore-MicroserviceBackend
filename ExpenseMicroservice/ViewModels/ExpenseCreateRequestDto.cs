using System.Security.Cryptography;

namespace ExpenseMicroservice.ViewModels;

public class ExpenseCreateRequestDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public IEnumerable<ExpenseDetailRequestDto>? ExpenseDetails{ get; set; }
}