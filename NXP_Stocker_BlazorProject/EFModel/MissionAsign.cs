namespace NXP_Stocker_BlazorProject.EFModel
{
    public class MissionAsignTable
    {
        public int PierNo { get; set; }

        public Guid Id { get; set; }

        public int ActionCode { get; set; }

        public string Barcode { get; set; }

        public int BoardSize { get; set; }

        public int PickZone { get; set; }

        public int PickLayer { get; set; }

        public int DropZone { get; set; }

        public int DropLayer { get; set; }

        public DateTime EstablishTime { get; set; }

        public DateTime? StartTime { get; set; }

        public bool IsError { get; set; }

        public int ErrorCode { get; set; }

        public DateTime? FinishTime { get; set; }

        public bool IsCancel { get; set; }

        public virtual ICollection<MissionBase> ListMission { get; set; } = new List<MissionBase>();
    }
}
