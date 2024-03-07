namespace Nelmix.DTOs
{
    public class GameDTO
    {

        public class ManageUserGameRequestDto
        {
            public int UserId { get; set; }
            public int RedChips { get; set; }
            public int YellowChips { get; set; }
            public int GreenChips { get; set; }
            public int BlackChips { get; set; }
            public int GameId { get; set; }
            public bool Victory { get; set; }

        }

    }
}
