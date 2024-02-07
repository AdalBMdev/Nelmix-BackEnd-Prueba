using Nelmix.Models;

namespace Nelmix.Interfaces
{
    public interface IFinanceService
    {
        Task<EstadoFinanciero> GetFinancialStatusUser(int userId);

        Task<List<GananciasYPérdidasPorJuego>> GetProfitAndLossFromGaming(int userId);
    }
  
 
}
