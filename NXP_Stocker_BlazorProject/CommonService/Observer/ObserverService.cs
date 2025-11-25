using CommonLibraryB_NXP.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;
using NXP_Stocker_BlazorProject.DbTableLibrary;
using NXP_Stocker_BlazorProject.EFModel;
using NXP_Stocker_BlazorProject.MachineModel;

namespace NXP_Stocker_BlazorProject.CommonService.Observer
{
    public partial class ObserverService : INLogWritterObservable
    {
        List<INLogWritterObserver> os { get; set; }

        public void AddNLogWritterObserver(INLogWritterObserver o)
        {
            if(os == null)
                os = new List<INLogWritterObserver>();

            if(!os.Contains(o))
            {
                os.Add(o);
            }
        }

        public void RemoveNLogWritterObserver(INLogWritterObserver o)
        {
            if(os != null && os.Contains(o))
            {
                os.Remove(o);
            }
        }

        public async Task NotifyNLog(EStatus status, string msg)
        {
            if(os != null)
            {
                foreach(var o in os)
                {
                    await o.WriteNLog(status, msg);
                }
            }
        }

    }

    public partial class ObserverService : IPierUIObserverable
    {
        List<IPierUIObserver> osPier { get; set; }

        public void AddPierUIObserver(IPierUIObserver o)
        {
            if (osPier == null)
                osPier = new List<IPierUIObserver>();

            if(!osPier.Contains(o))
            {
                osPier.Add(o);
            }
        }

        public void RemovePierUIObserver(IPierUIObserver o)
        {
            if(osPier != null && osPier.Contains(o))
            {
                osPier.Remove(o);
            }
        }

        public async Task NotifyPierMission(int pier, PierMissionTable table)
        {
            if(osPier != null)
            {
                foreach(var o in osPier)
                {
                    await o.UpdatePierMission(pier, table);
                }
            }
        }

        public async Task NotifyPierLog(int pier, List<LogTable> list)
        {
            if (osPier != null)
            {
                foreach (var o in osPier)
                {
                    await o.UpdatePierLog(pier, list);
                }
            }
        }
    }

    public partial class ObserverService : IRobotUIObserverable
    {

        List<IRobotUIObserver> osRobot { get; set; }

        public void AddRobotUIObserver(IRobotUIObserver o)
        {
            if(osRobot == null)
                osRobot = new List<IRobotUIObserver>();

            if(!osRobot.Contains(o))
            {
                osRobot.Add(o);
            }
        }

        public void RemoveRobotUIObserver(IRobotUIObserver o)
        {
            if(osRobot != null && osRobot.Contains(o))
            {
                osRobot.Remove(o);
            }
        }

        public async Task NotifyRobotLog(int pier, List<LogTable> list)
        {
            if(osRobot != null)
            {
                foreach(var o in osRobot)
                {
                    await o.UpdateRobotLog(pier, list);
                }
            }
        }

        public async Task NotifyRobotMission(int pier, RobotMissionTable table)
        {
            if(osRobot != null)
            {
                foreach(var o in osRobot)
                {
                    await o.UpdateRobotMission(pier, table);
                }
            }
        }
    }

    public partial class ObserverService : IMissionAssignUIObserverable
    {

        List<IMissionAssignUIObserver> osMissionAssign { get; set; }

        public void AddMissionAssignUIObserver(IMissionAssignUIObserver o)
        {
            if (osMissionAssign == null)
                osMissionAssign = new List<IMissionAssignUIObserver>();

            if(!osMissionAssign.Contains(o))
            {
                osMissionAssign.Add(o);
            }
        }

        public void RemoveMissionAssignUIObserver(IMissionAssignUIObserver o)
        {
            if(osMissionAssign != null && osMissionAssign.Contains(o))
            {
                osMissionAssign.Remove(o);
            }
        }

        public async Task NotifyMissionAsign(int pier, MissionAssignTable missionAsign)
        {
            if(osMissionAssign != null)
            {
                foreach(var o in osMissionAssign)
                {
                    await o.UpdateMissionAsign(pier, missionAsign);
                }
            }
        }

        public async Task NotifyMissionAsignLog(int pier, List<LogTable> list)
        {
            if(osMissionAssign != null)
            {
                foreach(var o in osMissionAssign)
                {
                    await o.UpdateMissionAsignLog(pier, list);
                }
            }
        }

        public async Task NotifyWarehouseInform(int pier, WarehouseInform warehouse)
        {
            if(osMissionAssign != null)
            {
                foreach(var o in osMissionAssign)
                {
                    await o.UpdateWarehouseInform(pier, warehouse);
                }
            }
        }
    }

    public partial class ObserverService : IMainUIObserverable
    {
        List<IMainUIObserver> osMain { get; set; }

        public void AddMainUIObserver(IMainUIObserver o)
        {
            if (osMain == null)
                osMain = new List<IMainUIObserver>();

            if(!osMain.Contains(o))
            {
                osMain.Add(o);
            }
        }

        public void RemoveMainUIObserver(IMainUIObserver o)
        {
            if(osMain != null && osMain.Contains(o))
            {
                osMain.Remove(o);
            }
        }

        public async Task NotifyPopUpMessage(bool popUp, string msg)
        {
            if(osMain != null)
            {
                foreach(var o in osMain)
                {
                    await o.UpdatePopUpMessage(popUp, msg);
                }
            }
        }

        public async Task NotifyMainLog(List<LogTable> list)
        {
            if(osMain != null)
            {
                foreach(var o in osMain)
                {
                    await o.UpdateMainLog(list);
                }
            }
        }
    }
}
