using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;
using NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage.Interface;

namespace NXP_Stocker_BlazorProject.TaskPackage.MissionAssignTaskPackage
{
    public partial class MissionAsignTaskPack
    {
        readonly IMissionAssignDataService IDataService;
        readonly IMissionAssignUIObserverable IMissionAsignObser;

        public MissionAsignTaskPack(MissionAssignDataService dataService, ObserverService observerService)
        {
            this.IDataService = dataService;
            this.IMissionAsignObser = observerService;
        }
    }

    public partial class MissionAsignTaskPack : IMissionAsignTaskPack
    {

    }
}
