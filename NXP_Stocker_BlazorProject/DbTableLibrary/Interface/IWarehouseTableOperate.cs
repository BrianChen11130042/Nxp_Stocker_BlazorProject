using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{
    public interface IWarehouseTableOperate
    {

        Task<(bool status, string msg, WarehouseTable table)> GetEmptyStorage(string pierName, int boardSizeSpec);

        Task<(bool status, string msg, WarehouseTable table)> GetEmptyBuffer(string pierName, int boardSizeSpec);

        Task<(bool status, string msg, WarehouseTable table)> GetEmptyPier(string pierName, int boardSizeSpec);

        Task<(bool status, string msg, WarehouseTable table)> GetPickTarget(string pierName, string barcode);

        Task<(bool status, string msg, WarehouseTable table)> SetWHTarget(WarehouseTable data);

        Task<(bool status, string msg, WarehouseTable table)> GetWHTarget(string pierName, int zone, int layer);

    }
}
