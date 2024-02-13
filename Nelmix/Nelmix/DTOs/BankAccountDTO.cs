namespace Nelmix.DTOs
{
    public class BankAccountDTO
    {

        public class CreateBankAccountRequestDto
        {
            public int UserId { get; set; }
            public int CurrencyId { get; set; }

        }

        public class AddBankAccountBalanceRequestDto
        {
            public int UserId { get; set; }
            public int CurrencyId { get; set; }
            public decimal Saldo { get; set; }
        }

        public class DeleteBankAccountRequestDto
        {
            public int UserId { get; set; }
            public int BankAccountId { get; set; }

        }
    }
}
