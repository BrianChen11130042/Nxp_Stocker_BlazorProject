namespace NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage.Interface
{
    public interface IPierTaskPack
    {
        Task<bool> GetPlcPierName();

        Task<bool> GetTableNewMission();

        Task<bool> SetTableMissionStart();

        Task<bool> SetLogMissionStart();

        Task<bool> SetTableMissionFinsih();

        Task<bool> SetLogMissionFinish();

        Task UpdateUIPierMission();

        Task UpdateUIPierLog();

        //入大板
        bool IsInputLargeBoard();

        Task<bool> SetPlcStartInputLargeBoard();

        Task<bool> GetPlcInputLargeBoardStatus();

        bool IsInputLargeBoardFinish();

        Task<bool> SetPlcFinshInputLargeBoard();



        bool IsInputSmallBoard();

        bool IsOutputLargeBoard();

        bool IsOutputSmallBoard();
    }
}
