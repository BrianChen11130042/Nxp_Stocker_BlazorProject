using NXP_Stocker_BlazorProject.DbTableLibrary;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NXP_Stocker_BlazorProject.EFModel
{
    public abstract class MissionBase
    {
        public Guid Id { get; set; }

        public Guid AsignId { get; set; }
        public int PierNo { get; set; }

        public string? Barcode { get; set; }

        public DateTime EstablishTime { get; set; }

        [NotMapped]
        public bool IsStart => StartTime is not null;

        public DateTime? StartTime { get; set; }

        public int Status { get; set; }
        [NotMapped]
        public bool IsError => ErrorCode is not 0;

        public int ErrorCode { get; set; }

        [NotMapped]
        public bool IsFinish => FinishTime is not null;

        public DateTime? FinishTime { get; set; }

        public virtual MissionAssignTable? MissionAsign { get; set; }
    }
}
