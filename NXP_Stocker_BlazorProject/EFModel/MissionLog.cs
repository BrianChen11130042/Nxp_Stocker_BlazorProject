namespace NXP_Stocker_BlazorProject.EFModel
{
    public class LogTable
    {
        public int Id { get; set; } //自動增加

        public string LogType { get; set; }

        public string Equipment { get; set; }

        public string Msg { get; set; }

        public DateTime RecordTime { get; set; }
    }
}
