using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NXP_Stocker_BlazorProject.EFModel
{
    public class MissionAssignTable
    {
        [Key]
        public Guid Id { get; set; }

        [Range(1,2)]
        public int PierNo { get; set; }
        [Range(1, 3)]
        public int ActionCode { get; set; }
        [Required]
        public string Barcode { get; set; } = null!;
        [Range(0, 1)]
        public int BoardSize { get; set; }

        public int PickZone { get; set; }

        public int PickLayer { get; set; }

        public int DropZone { get; set; }

        public int DropLayer { get; set; }

        public DateTime EstablishTime { get; set; }
        [Range(0, 1)]
        public int Emergency { get; set; }

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

        [NotMapped]
        public TimeSpan? PierMissionTimeSpan
        {
            get
            {
                var mission = Missions.OfType<PierMissionTable>()
                                      .FirstOrDefault(x => x.StartTime.HasValue && x.FinishTime.HasValue);

                return mission != null ? mission.FinishTime - mission.StartTime : null;
            }
        }

        [NotMapped]
        public TimeSpan? RobotMissionTimeSpan
        {
            get
            {
                var mission = Missions.OfType<RobotMissionTable>()
                                      .FirstOrDefault(x => x.StartTime.HasValue && x.FinishTime.HasValue);

                return mission != null ? mission.FinishTime - mission.StartTime : null;
            }
        }

        public virtual ICollection<MissionBase> Missions { get; set; } = new List<MissionBase>();
    }
}
