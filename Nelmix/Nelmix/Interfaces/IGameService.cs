using Nelmix.Models;

namespace Nelmix.Interfaces
{
    public interface IGameService
    {

        Task<bool> VerifyEligibilityToPlay(int userId);

        Task<bool> VerifyAvailabilityFiches(int userId, int redChips, int yellowChips, int greenChips, int blackChips);

        Task<bool> VerifyLoseLimit(int userId, int juegoId);

        Task<bool> VerifyGainLimit(int userId);

        Task<bool> ManageUserGame(int userId, int redChips, int yellowChips, int greenChips, int blackChips, bool victory, int gameId);

        Task<bool> VerifyPlay(int usuarioId, int redChips, int yellowChips, int greenChips, int blackChips, int juegoId);

        (bool?, string) PlayCraps();

        (bool, string) PlayTragaperras();

        (bool, string) PlayBlackjack();


    }
}
