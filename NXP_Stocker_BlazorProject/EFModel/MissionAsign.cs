using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NXP_Stocker_BlazorProject.EFModel
{
    public class MissionAsignTable
    {
        [Key]
        public Guid Id { get; set; }

        public int PierNo { get; set; }

        public int ActionCode { get; set; }

        public string Barcode { get; set; } = null!;

        public int BoardSize { get; set; }

        public int PickZone { get; set; }

        public int PickLayer { get; set; }

        public int DropZone { get; set; }

        public int DropLayer { get; set; }

        public DateTime EstablishTime { get; set; }

        [NotMapped]
        public bool IsStart => StartTime is not null;

        public DateTime? StartTime { get; set; }
        [NotMapped]
        public bool IsError => ErrorCode is not 0;

        public int ErrorCode { get; set; }

        [NotMapped]
        public bool IsFinish => FinishTime is not null;

        public DateTime? FinishTime { get; set; }

        public bool IsCancel { get; set; }

        public virtual ICollection<MissionBase> Missions { get; set; } = new List<MissionBase>();
    }
}
