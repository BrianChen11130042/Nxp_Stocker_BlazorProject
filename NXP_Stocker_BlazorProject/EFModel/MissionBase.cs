using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.EFModel
{
    public class MissionBase_stub
    {
        public string PierName { get; set; } //int

        public Guid Id { get; set; }

        public Guid AsignId { get; set; }

        public string Barcode { get; set; }

        public DateTime EstablishTime { get; set; }

        public bool IsStart { get; set; } //砍

        public DateTime StartTime { get; set; }

        public string Status { get; set; } //int

        public bool IsError { get; set; }

        public int ErrorCode { get; set; }

        public bool IsFinish { get; set; } //砍

        public DateTime FinishTime { get; set; }

        public virtual MissionAsignTable_stub Asign { get; set; }
    }
}
