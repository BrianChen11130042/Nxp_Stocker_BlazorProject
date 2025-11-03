using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{
    public interface IWarehouseTableOperate
    {

        Task<(bool status, string msg, WarehouseTable table)> GetEmptyStorage(string PierName, int BoardSize);

        Task<(bool status, string msg, WarehouseTable table)> GetEmptyBuffer(string PierName, int BoardSize);

        Task<(bool status, string msg, WarehouseTable table)> GetPierTarget(string PierName, int BoardSizeSpec, bool IsOccupy);

        Task<(bool status, string msg, WarehouseTable table)> SetPierTarget(WarehouseTable data);

        Task<(bool status, string msg, WarehouseTable table)> GetTargetBoard(string PierName, string Barcode);

        Task<(bool status, string msg, List<WarehouseTable> list)> GetAllStorageAndBuffer(string PierName);
    }
}
