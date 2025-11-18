using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.EFModel
{
    public class RobotMissionTable_stub : MissionBase_stub
    {
        public int BoardSize { get; set; }

        public int PickZone { get; set; }

        public int PickLayer { get; set; }

        public int DropZone { get; set; }

        public int DropLayer { get; set; }
    }
}
