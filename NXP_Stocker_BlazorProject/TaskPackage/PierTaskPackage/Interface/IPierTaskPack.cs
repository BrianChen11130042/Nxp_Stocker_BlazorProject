namespace NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage.Interface
{
    public interface IPierTaskPack
    {
        Task<bool> GetPierName();

        Task<bool> CheckNewMission();

        bool IsInputLargeBoard();

        bool IsInputSmallBoard();

        bool IsOutputLargeBoard();

        bool IsOutputSmallBoard();
    }
}
