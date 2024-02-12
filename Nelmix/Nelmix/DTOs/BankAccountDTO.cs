namespace Nelmix.DTO
{
    public class BankAccountDTO
    {

        public class CreateBankAccountRequestDto
        {
            public int UserId { get; set; }
            public int MonedaId { get; set; }

        }

        public class AddBankAccountBalanceRequestDto
        {
            public int UserId { get; set; }
            public int CurrencyId { get; set; }
            public decimal Saldo { get; set; }
        }

    }
}
