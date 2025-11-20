using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NXP_Stocker_BlazorProject.EFModel
{
    public class LogTable
    {
        [Key]
        public int Id { get; set; }

        public string LogType { get; set; } = null!;

        public string Equipment { get; set; } = null!;

        public string Msg { get; set; } = null!;

        public DateTime RecordTime { get; set; }
    }
}
