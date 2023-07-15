using TicimaxTask.Entities.Entities.Enums;

namespace TicimaxTask.MovementAPI.DTOs
{
    public class GetCheckInOutByIdDTO
    {
        public string UserName { get; set; }
        public DateTime CheckTime { get; set; }
        public CheckStatus CheckType { get; set; }
        public string CheckTypeDisplay 
        {
            get { return CheckType == CheckStatus.CheckIn ? "In" : "Out"; }
            
        }
        
    }
}
