namespace NXP_Stocker_BlazorProject.EFModel
{
    public class LogTable
    {
        public int Id { get; set; }

        public string LogType { get; set; } = null!;

        public string Equipment { get; set; } = null!;

        public string Msg { get; set; } = null!;

        public DateTime RecordTime { get; set; }
    }
}
