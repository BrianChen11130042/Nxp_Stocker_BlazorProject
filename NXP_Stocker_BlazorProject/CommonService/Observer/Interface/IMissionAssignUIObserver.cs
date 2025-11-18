using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;
using NXP_Stocker_BlazorProject.MachineModel;

namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IMissionAssignUIObserverable
    {
        void AddMissionAssignUIObserver(IMissionAssignUIObserver o);

        void RemoveMissionAssignUIObserver(IMissionAssignUIObserver o);

        Task NotifyMissionAsign(int pier, MissionAsignTable missionAsign);

        Task NotifyMissionAsignLog(int pier, List<LogTable> list);

        Task NotifyWarehouseInform(int pier, WarehouseInform warehouse);
    }


    public interface IMissionAssignUIObserver
    {
        Task UpdateMissionAsign(int pier, MissionAsignTable missionAsign);

        Task UpdateMissionAsignLog(int pier, List<LogTable> list);

        Task UpdateWarehouseInform(int pier, WarehouseInform warehouse);
    }
}
