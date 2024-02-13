using Nelmix.Models;
using static Nelmix.DTOs.GameDTO;

namespace Nelmix.Interfaces
{
    public interface IGameService
    {
        Task ManageUserGame(ManageUserGameRequestDto request);

        (bool?, string) PlayCraps();

        (bool, string) PlayTragaperras();

        (bool, string) PlayBlackjack();

    }
}
