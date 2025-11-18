using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{
    public partial class PierMissionTableLibrary
    {
        readonly IServiceProvider serviceProvider;

        public PierMissionTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        //*************下面砍掉*************//

        List<PierMissionTable> listPierMissionTable { get; set; } = new List<PierMissionTable>();

        //**********************************//
    }

    public partial class PierMissionTableLibrary : IPierMissionTableOperate
    {
        public async Task<(bool status, string msg, PierMissionTable table)> AddPierMission(PierMissionTable data)
        {
            try
            {
                listPierMissionTable.Add(data);

                PierMissionTable table = listPierMissionTable.FirstOrDefault(x => x.PierName == data.PierName
                                                                               && x.AsignId == data.AsignId
                                                                               && x.Barcode == data.Barcode
                                                                               && x.ActionCode == data.ActionCode
                                                                               && x.EstablishTime == data.EstablishTime);

                return (true, string.Empty, table);

            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, PierMissionTable table)> GetNewPierMission(string PierName)
        {
            try
            {
                PierMissionTable table = listPierMissionTable.FirstOrDefault(x => x.PierName == PierName
                                                                               && x.IsStart == false
                                                                               && x.IsFinish == false);

                return (true, string.Empty, table);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, PierMissionTable table)> UpdatePierMission(PierMissionTable data)
        {
            try
            {
                int index = listPierMissionTable.FindIndex(x => x.PierName == data.PierName
                                                             && x.AsignId == data.AsignId
                                                             && x.Barcode == data.Barcode
                                                             && x.ActionCode == data.ActionCode
                                                             && x.EstablishTime == data.EstablishTime);

                listPierMissionTable[index].IsStart = data.IsStart;
                listPierMissionTable[index].StartTime = data.StartTime;
                listPierMissionTable[index].IsError = data.IsError;
                listPierMissionTable[index].ErrorCode = data.ErrorCode;
                listPierMissionTable[index].IsFinish = data.IsFinish;
                listPierMissionTable[index].FinishTime = data.FinishTime;

                return (true, string.Empty, listPierMissionTable[index]);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, PierMissionTable table)> GetTargetPierMission(PierMissionTable data)
        {
            try
            {
                int index = listPierMissionTable.FindIndex(x => x.PierName == data.PierName
                                                             && x.AsignId == data.AsignId
                                                             && x.Barcode == data.Barcode
                                                             && x.ActionCode == data.ActionCode
                                                             && x.EstablishTime == data.EstablishTime);

                return (true, string.Empty, listPierMissionTable[index]);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }
}
