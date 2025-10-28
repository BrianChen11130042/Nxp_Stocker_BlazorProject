using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{
    public interface IStorageTableOperate
    {
        Task<(bool status, string msg, StorageTable teble)> GetEmptyStorage(string PierName, int BoardSize);

        Task<(bool status, string msg, StorageTable teble)> GetTargetStroage(string PierName, string Barcode);

        Task<(bool status, string msg, List<StorageTable> list)> GetAllStoragePerPier(string PierName);
    }
}
