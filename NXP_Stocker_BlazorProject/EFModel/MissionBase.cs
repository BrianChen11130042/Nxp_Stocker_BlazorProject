using NXP_Stocker_BlazorProject.DbTableLibrary;
using System;

namespace NXP_Stocker_BlazorProject.EFModel
{
    public abstract class MissionBase
    {
        public int PierNo { get; set; } 

        public Guid Id { get; set; }

        public Guid AsignId { get; set; }

        public string Barcode { get; set; }

        public DateTime? EstablishTime { get; set; }

        public bool IsStart { get; set; }

        public DateTime? StartTime { get; set; }

        public int Status { get; set; }

        public bool IsError { get; set; }

        public int ErrorCode { get; set; }

        public bool IsFinish { get; set; }

        public DateTime? FinishTime { get; set; }

        public virtual MissionAsignTable? MissionAsign { get; set; }
    }
}
