namespace NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage.Interface
{
    public interface IPierTaskPack
    {
        Task<bool> GetPlcPierName();

        Task<bool> GetTableNewMission();

        Task<bool> GetTablePierTarget();

        Task<bool> SetTablePierTarge();

        Task<bool> SetTableMissionStart();

        Task<bool> SetLogMissionStart();

        Task<bool> SetTableMissionFinsih();

        Task<bool> SetLogMissionFinish();

        Task UpdateUIPierMission();

        Task UpdateUIPierLog();

        Task UpdateUIPierTable();

        //入大板
        bool IsInputLargeBoard();

        Task<bool> SetPlcStartInputLargeBoard();

        Task<bool> GetPlcInputLargeBoardStatus();

        bool IsInputLargeBoardFinish();

        Task<bool> SetPlcFinshInputLargeBoard();

        //入小板
        bool IsInputSmallBoard();

        Task<bool> SetPlcStartInputSmallBoard();

        Task<bool> GetPlcInputSmallBoardStatus();

        bool IsInputSmallBoardFinish();

        Task<bool> SetPlcFinishInputSmallBoard();

        //出大板
        bool IsOutputLargeBoard();

        Task<bool> SetPlcStartOutputLargeBoard();

        Task<bool> GetPlcOutputLargeBoardStatus();

        bool IsOutputLargeBoardFinish();

        Task<bool> SetPlcFinishOutputLargeBoard();

        //出小板
        bool IsOutputSmallBoard();

        Task<bool> SetPlcStartOutputSmallBoard();

        Task<bool> GetPlcOutputSmallBoardStatus();

        bool IsOutputSmallBoardFinish();

        Task<bool> SetPlcFinishOutputSmallBoard();
    }
}
