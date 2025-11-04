namespace NXP_Stocker_BlazorProject.CommonService.Observer.Interface
{
    public interface IMissionAssignUIObserverable
    {
        void AddMissionAssignUIObserver(IMissionAssignUIObserver o);

        void RemoveMissionAssignUIObserver(IMissionAssignUIObserver o);
    }


    public interface IMissionAssignUIObserver
    {

    }
}
