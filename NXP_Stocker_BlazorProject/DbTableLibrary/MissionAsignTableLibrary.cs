using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;
using NXP_Stocker_BlazorProject.EFModel;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{

    public partial class MissionAsignTableLibrary
    {

        readonly IServiceProvider serviceProvider;

        public MissionAsignTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        //*************下面砍掉*************//

        List<MissionAsignTable_stub> listMissionAsignTable { get; set; } = new List<MissionAsignTable_stub>();

        //**********************************//
    }

    public partial class MissionAsignTableLibrary : IMissionAsignTableOperate
    {
        public async Task<(bool status, string msg, MissionAsignTable_stub table)> AddMissionAsign(MissionAsignTable_stub data)
        {
            try
            {
                listMissionAsignTable.Add(data);

                MissionAsignTable_stub table = listMissionAsignTable.FirstOrDefault(x => x.PierName == data.PierName
                                                                               && x.Id == data.Id
                                                                               && x.Barcode == data.Barcode);

                return (true, string.Empty, table);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, MissionAsignTable_stub table)> GetNewMissionAsign(string PierName)
        {
            try
            {
                MissionAsignTable_stub table = listMissionAsignTable.FirstOrDefault(x => x.PierName == PierName
                                                                               && x.IsStart == false
                                                                               && x.IsFinish == false);

                return (true, string.Empty, table);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, MissionAsignTable_stub table)> UpdateMissionAsign(MissionAsignTable_stub data)
        {
            try
            {
                int index = listMissionAsignTable.FindIndex(x => x.PierName == data.PierName
                                                              && x.Id == data.Id
                                                              && x.ActionCode == data.ActionCode
                                                              && x.Barcode == data.Barcode
                                                              && x.BoardSize == data.BoardSize
                                                              && x.PickZone == data.PickZone
                                                              && x.PickLayer == data.PickLayer
                                                              && x.DropZone == data.DropZone
                                                              && x.DropLayer == data.DropLayer
                                                              && x.EstablishTime == data.EstablishTime);

                listMissionAsignTable[index].IsStart = data.IsStart;
                listMissionAsignTable[index].StartTime = data.StartTime;
                listMissionAsignTable[index].IsError = data.IsError;
                listMissionAsignTable[index].ErrorCode = data.ErrorCode;
                listMissionAsignTable[index].IsFinish = data.IsFinish;
                listMissionAsignTable[index].FinishTime = data.FinishTime;

                return (true, string.Empty, listMissionAsignTable[index]);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }
}
