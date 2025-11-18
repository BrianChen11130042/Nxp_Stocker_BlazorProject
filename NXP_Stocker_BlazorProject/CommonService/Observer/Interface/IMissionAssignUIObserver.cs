using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IMissionAssignUIObserverable
    {
        void AddMissionAssignUIObserver(IMissionAssignUIObserver o);

        void RemoveMissionAssignUIObserver(IMissionAssignUIObserver o);

        Task NotifyMissionAsign(string pier, MissionAsignTable missionAsign);

        Task NotifyMissionAsignLog(string pier, List<LogTable_stub> list);
    }


    public interface IMissionAssignUIObserver
    {
        Task UpdateMissionAsign(string pier, MissionAsignTable missionAsign);

        Task UpdateMissionAsignLog(string pier, List<LogTable_stub> list);
    }
}
