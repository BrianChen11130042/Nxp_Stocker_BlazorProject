using CommonLibraryB_NXP.Library.PLC.Adapter;
using CommonLibraryB_NXP.Library.PLC;
using NXP_Stocker_BlazorProject.CommonService.Data;
using NXP_Stocker_BlazorProject.CommonService.Observer;
using NXP_Stocker_BlazorProject.TaskPackage.PlcRegularTaskPackage.Interface;
using NXP_Stocker_BlazorProject.CommonService.Data.Interface;
using CommonLibraryB_NXP.Tools.LogWritter;
using NXP_Stocker_BlazorProject.CommonService.Observer.Interface;

namespace NXP_Stocker_BlazorProject.TaskPackage.PlcRegularTaskPackage
{
    public partial class PlcRegularTaskPack<EPLC>
    {
        readonly EPLC Warehouse;
        readonly EPLC Heartbeat;

        readonly IPlcOperate<EPLC> IPlcOp;
        readonly PlcLibrary<EPLC> plcLib;

        readonly IPlcRegularDataService IDataService;

        readonly INLogWritterObservable INLogObser;
        readonly IPlcRegularUIObserverable IPlcRegularObser;

        public PlcRegularTaskPack(EPLC Warehouse, EPLC Heartbeat, PlcLibrary<EPLC> plcLib,
                                  PlcRegularDataService dataService, ObserverService observerService)
        {
            this.Warehouse = Warehouse;
            this.Heartbeat = Heartbeat;

            this.IPlcOp = plcLib;
            this.plcLib = plcLib;

            this.IDataService = dataService;

            this.INLogObser = observerService;
            this.IPlcRegularObser = observerService;
        }

        async Task writeNLogError(string log)
        {
            await INLogObser.NotifyNLog(EStatus.Error, log);
        }

        async Task writeNLogInform(string log)
        {
            await INLogObser.NotifyNLog(EStatus.Info, log);
        }
    }

    public partial class PlcRegularTaskPack<EPLC> : IPlcRegularTaskPack
    {
        public async Task<bool> SetPlcHeartBeat()
        {
            plcLib.Packages[Heartbeat].property.setHeartbeat.heartBeat = _getHeartBeat();

            if (await IPlcOp.SetHeartBeat(Heartbeat))
            {
                return true;
            }
            else
            {
                string nlog = plcLib.Packages[Heartbeat].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task<bool> GetPlcWarehouse()
        {
            if (await IPlcOp.GetWarehouse(Warehouse))
            {
                if (plcLib.Packages[Warehouse].property.getWarehouse.dcWarehouse.Count == 458)
                {
                    IDataService.DcWarehouse = _upWarehouse(plcLib.Packages[Warehouse].property.getWarehouse.dcWarehouse);
                }

                return true;
            }
            else
            {
                string nlog = plcLib.Packages[Warehouse].errorLog;
                await writeNLogError(nlog);
                return false;
            }
        }

        public async Task UpdateUIWarehouse()
        {
            await IPlcRegularObser.NotifyWarehouseInform(IDataService.DcWarehouse);
        }
    }

    public partial class PlcRegularTaskPack<EPLC>
    {
        int heartBeat { get; set; } = 0;

        ushort _getHeartBeat()
        {
            if (heartBeat == 0)
            {
                heartBeat = 1;
            }
            else
            {
                heartBeat = 0;
            }

            return (ushort)heartBeat;
        }


        Dictionary<int, EWhStatus> _upWarehouse(Dictionary<int, bool> dcWh)
        {
            Dictionary<int, EWhStatus> dcResult = new Dictionary<int, EWhStatus>();

            foreach (var item in _getPier1Wh(dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(101, 107, 36, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(110, 117, 38, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(120, 122, 40, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(123, 127, 40, false, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(141, 147, 44, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(150, 157, 46, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(160, 162, 48, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(201, 207, 72, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(210, 217, 74, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(220, 222, 76, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(241, 247, 80, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(250, 257, 82, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(260, 262, 84, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(301, 307, 108, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(310, 317, 110, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(320, 322, 112, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(341, 347, 116, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(350, 355, 118, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getPier2Wh(dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(501, 507, 180, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(510, 517, 182, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(520, 522, 184, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(523, 527, 184, false, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(541, 547, 188, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(550, 557, 190, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(560, 562, 192, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(601, 607, 216, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(610, 617, 218, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(620, 622, 220, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(641, 647, 224, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(650, 657, 226, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(660, 662, 228, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(701, 707, 252, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(710, 715, 254, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(741, 747, 260, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            foreach (var item in _getWh(750, 755, 262, true, dcWh))
            {
                dcResult.Add(item.Key, item.Value);
            }

            return dcResult;
        }

        Dictionary<int, EWhStatus> _getPier1Wh(Dictionary<int, bool> dcWh)
        {
            Dictionary<int, EWhStatus> dcPier1 = new Dictionary<int, EWhStatus>();

            bool Large = !(dcWh[36]);
            bool Small = !(dcWh[37]);

            if (Large == true && Small == false)
            {
                dcPier1.Add(44, EWhStatus.Large);
            }
            else if (Large == false && Small == true)
            {
                dcPier1.Add(44, EWhStatus.Small);
            }
            else
            {
                dcPier1.Add(44, EWhStatus.Empty);
            }

            return dcPier1;
        }

        Dictionary<int, EWhStatus> _getPier2Wh(Dictionary<int, bool> dcWh)
        {
            Dictionary<int, EWhStatus> dcPier2 = new Dictionary<int, EWhStatus>();

            bool Large = !(dcWh[276]);
            bool Small = !(dcWh[277]);

            if (Large == true && Small == false)
            {
                dcPier2.Add(424, EWhStatus.Large);
            }
            else if (Large == false && Small == true)
            {
                dcPier2.Add(424, EWhStatus.Small);
            }
            else
            {
                dcPier2.Add(424, EWhStatus.Empty);
            }

            return dcPier2;
        }

        Dictionary<int, EWhStatus> _getWh(int start, int finish, int offset, bool reverse, Dictionary<int, bool> dcWh)
        {
            Dictionary<int, EWhStatus> dcData = new Dictionary<int, EWhStatus>();

            for (int i = start; i <= finish; i++)
            {
                bool occupy;

                if (reverse)
                {
                    occupy = !(dcWh[i - offset]);
                }
                else
                {
                    occupy = dcWh[i - offset];
                }

                if (occupy)
                {
                    dcData.Add(i, EWhStatus.Small);
                }
                else
                {
                    dcData.Add(i, EWhStatus.Empty);
                }
            }

            return dcData;
        }
    }
}
