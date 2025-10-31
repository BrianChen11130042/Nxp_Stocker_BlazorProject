namespace NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage.Interface
{
    public interface IPierTaskPack
    {
        Task<bool> GetPlcPierName();

        Task<bool> GetTableNewMission();

        Task<bool> SetTableMissionStart();

        Task UpdateUIPierMission();

        Task UpdateUIPierLog();

        //入大板
        bool IsInputLargeBoard();

        Task<bool> SetPlcInputLargeBoard();

        Task<bool> SetLogMissionStart();



        bool IsInputSmallBoard();

        bool IsOutputLargeBoard();

        bool IsOutputSmallBoard();
    }
}
