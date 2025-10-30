using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{
    public interface IStorageTableOperate
    {

        Task<(bool status, string msg, StorageTable teble)> GetEmptyStorage(string PierName, int BoardSize);

        Task<(bool status, string msg, StorageTable teble)> GetEmptyBuffer(string PierName, int BoardSize);

        Task<(bool status, string msg, StorageTable teble)> GetEmptyPier(string PierName, int BoardSize);

        Task<(bool status, string msg, StorageTable teble)> GetTargetBoard(string PierName, string Barcode);

        Task<(bool status, string msg, List<StorageTable> list)> GetAllStorageAndBuffer(string PierName);
    }
}
