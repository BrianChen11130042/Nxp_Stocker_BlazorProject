using CommonLibraryB.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;
using NXP_Stocker_BlazorProject.DbTableLibrary;

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

        public async Task NotifyPierMission(string pier, PierMissionTable table)
        {
            if(osPier != null)
            {
                foreach(var o in osPier)
                {
                    await o.UpdatePierMission(pier, table);
                }
            }
        }

        public async Task NotifyPierLog(string pier, List<LogTable> list)
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
}
