using System.ComponentModel.DataAnnotations;

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

        public DateTime? EstablishTime { get; set; }

        public bool IsStart { get; set; }

        public DateTime? StartTime { get; set; }

        public bool IsError { get; set; }

        public int ErrorCode { get; set; }

        public bool IsFinish { get; set; }

        public DateTime? FinishTime { get; set; }

        public bool IsCancel { get; set; }

        public virtual ICollection<MissionBase> Missions { get; set; } = new List<MissionBase>();
    }
}
