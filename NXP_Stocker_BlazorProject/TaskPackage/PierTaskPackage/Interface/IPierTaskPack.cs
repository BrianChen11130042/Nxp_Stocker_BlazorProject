namespace NXP_Stocker_BlazorProject.TaskPackage.PierTaskPackage.Interface
{
    public interface IPierTaskPack
    {
        Task<bool> GetPlcPierName();

        Task<bool> GetTableNewMission();

        //入大板
        bool IsInputLargeBoard();

        Task<bool> SetPlcInputLargeBoard();



        bool IsInputSmallBoard();

        bool IsOutputLargeBoard();

        bool IsOutputSmallBoard();
    }
}
