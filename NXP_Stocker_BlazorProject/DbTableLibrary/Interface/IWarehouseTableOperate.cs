using NXP_Stocker_BlazorProject.DbTableLibrary;

namespace NXP_Stocker_BlazorProject.DbTableLibrary.Interface
{
    public interface IWarehouseTableOperate
    {

        Task<(bool status, string msg, WarehouseTable_stub table)> GetEmptyStorage(string pierName, int boardSizeSpec);

        Task<(bool status, string msg, WarehouseTable_stub table)> GetEmptyBuffer(string pierName, int boardSizeSpec);

        Task<(bool status, string msg, WarehouseTable_stub table)> GetEmptyPier(string pierName, int boardSizeSpec);

        Task<(bool status, string msg, WarehouseTable_stub table)> GetPickTarget(string pierName, string barcode);

        Task<(bool status, string msg, WarehouseTable_stub table)> SetWHTarget(WarehouseTable_stub data);

        Task<(bool status, string msg, WarehouseTable_stub table)> GetWHTarget(string pierName, int zone, int layer);

    }
}
