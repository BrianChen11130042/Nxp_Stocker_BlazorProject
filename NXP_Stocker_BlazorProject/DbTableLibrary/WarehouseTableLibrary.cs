using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{

    public partial class WarehouseTableLibrary
    {

        readonly IServiceProvider serviceProvider;

        public WarehouseTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            Init_Delete(); //砍掉
        }

        //*************下面砍掉*************//

        List<WarehouseTable> listWarehouseTable { get; set; } = new List<WarehouseTable>();

        void Init_Delete()
        {
            Init_Pier1_Delete();
            Init_Pier2_Delete();
        }

        void Init_Pier1_Delete()
        {
            for(int i = 1 ; i <= 5 ; i++)
            {
                for(int j = 1 ; j <= 18 ; j++)
                {
                    WarehouseTable data = new WarehouseTable()
                    {
                        PierName = "Pier1",
                        Zone = i,
                        Layer = j,
                        BoardSizeSpec = 2,
                        IsOccupy = false,
                        Barcode = string.Empty,
                        BoardSize = 999,
                    };

                    listWarehouseTable.Add(data);
                }
            }

            for(int i = 1 ; i <= 13 ; i++)
            {
                WarehouseTable data = new WarehouseTable()
                {
                    PierName = "Pier1",
                    Zone = 6,
                    Layer = i,
                    BoardSizeSpec = 0,
                    IsOccupy = false,
                    Barcode = string.Empty,
                    BoardSize = 999,
                };

                listWarehouseTable.Add(data);
            }

            for (int i = 1; i <= 5; i++)
            {
                WarehouseTable data = new WarehouseTable()
                {
                    PierName = "Pier1",
                    Zone = 13,
                    Layer = i,
                    BoardSizeSpec = 2,
                    IsOccupy = false,
                    Barcode = string.Empty,
                    BoardSize = 999,
                };

                listWarehouseTable.Add(data);
            }

            for(int i = 1; i <= 1; i++)
            {
                WarehouseTable data = new WarehouseTable()
                {
                    PierName = "Pier1",
                    Zone = 20,
                    Layer = i,
                    BoardSizeSpec = 2,
                    IsOccupy = false,
                    Barcode = string.Empty,
                    BoardSize = 999,
                };

                listWarehouseTable.Add(data);
            }
        }

        void Init_Pier2_Delete()
        {
            for (int i = 7; i <= 10; i++)
            {
                for (int j = 1; j <= 18; j++)
                {
                    WarehouseTable data = new WarehouseTable()
                    {
                        PierName = "Pier2",
                        Zone = i,
                        Layer = j,
                        BoardSizeSpec = 2,
                        IsOccupy = false,
                        Barcode = string.Empty,
                        BoardSize = 999,
                    };

                    listWarehouseTable.Add(data);
                }
            }

            for(int i = 11 ; i <= 12 ; i++)
            {
                for (int j = 1; j <= 13; j++)
                {
                    WarehouseTable data = new WarehouseTable()
                    {
                        PierName = "Pier2",
                        Zone = i,
                        Layer = j,
                        BoardSizeSpec = 0,
                        IsOccupy = false,
                        Barcode = string.Empty,
                        BoardSize = 999,
                    };

                    listWarehouseTable.Add(data);
                }
            }

            for (int i = 1; i <= 5; i++)
            {
                WarehouseTable data = new WarehouseTable()
                {
                    PierName = "Pier2",
                    Zone = 14,
                    Layer = i,
                    BoardSizeSpec = 2,
                    IsOccupy = false,
                    Barcode = string.Empty,
                    BoardSize = 999,
                };

                listWarehouseTable.Add(data);
            }

            for (int i = 1; i <= 1; i++)
            {
                WarehouseTable data = new WarehouseTable()
                {
                    PierName = "Pier2",
                    Zone = 21,
                    Layer = i,
                    BoardSizeSpec = 2,
                    IsOccupy = false,
                    Barcode = string.Empty,
                    BoardSize = 999,
                };

                listWarehouseTable.Add(data);
            }
        }


        //**********************************//
    }

    //**********下面先取代DB 要砍掉*************//

    public class WarehouseTable
    {
        //public int Id { get; set; } 到時候DB要加上這個讓它自動增加

        public string PierName { get; set; }

        public int Zone { get; set; }

        public int Layer { get; set; }

        public int BoardSizeSpec { get; set; }

        public bool IsOccupy { get; set; }

        public string Barcode { get; set; }

        public int BoardSize { get; set; }
    }

    //***************************************//

    public partial class WarehouseTableLibrary
    {
        List<int> pierZone { get; set; } = new List<int>()
        {
            20, 21
        };

        List<int> bufferZone { get; set; } = new List<int>()
        {
            13, 14
        };

        List<int> storageZone { get; set; } = new List<int>()
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
        };
    }

    public partial class WarehouseTableLibrary : IWarehouseTableOperate
    {

        public async Task<(bool status, string msg, List<WarehouseTable> list)> GetAllStorageAndBuffer(string PierName)
        {
            try
            {
                List<WarehouseTable> result = listWarehouseTable.Where(x => x.PierName == PierName
                                                                     && !pierZone.Contains(x.Zone))
                                                            .ToList();

                return (true, "success", result);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, WarehouseTable table)> GetPierTarget(string PierName, int BoardSizeSpec, bool IsOccupy)
        {
            try
            {
                WarehouseTable result = listWarehouseTable.FirstOrDefault(x => x.PierName == PierName
                                                                        && x.IsOccupy == IsOccupy
                                                                        && (x.BoardSizeSpec == BoardSizeSpec || x.BoardSizeSpec == 2)
                                                                        && !bufferZone.Contains(x.Zone)
                                                                        && !storageZone.Contains(x.Zone));

                return (true, string.Empty, result);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, WarehouseTable table)> SetPierTarget(WarehouseTable data)
        {
            try
            {
                int index = listWarehouseTable.FindIndex(x => x.PierName == data.PierName
                                                           && x.Zone == data.Zone
                                                           && x.Layer == data.Layer
                                                           && x.BoardSizeSpec == data.BoardSizeSpec);

                listWarehouseTable[index].IsOccupy = data.IsOccupy;
                listWarehouseTable[index].Barcode = data.Barcode;
                listWarehouseTable[index].BoardSize = data.BoardSize;

                return (true, string.Empty, listWarehouseTable[index]);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, data);
            }
        }

        public async Task<(bool status, string msg, WarehouseTable table)> GetEmptyBuffer(string PierName, int BoardSize)
        {
            try
            {
                WarehouseTable result = listWarehouseTable.FirstOrDefault(x => x.PierName == PierName
                                                                        && x.IsOccupy == false
                                                                        && (x.BoardSizeSpec == BoardSize || x.BoardSizeSpec == 2)
                                                                        && !pierZone.Contains(x.Zone)
                                                                        && !storageZone.Contains(x.Zone));

                return (true, "success", result);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, WarehouseTable table)> GetEmptyStorage(string PierName, int BoardSize)
        {
            try
            {
                WarehouseTable result = listWarehouseTable.FirstOrDefault(x => x.PierName == PierName 
                                                                        && x.IsOccupy == false
                                                                        && (x.BoardSizeSpec == BoardSize || x.BoardSizeSpec == 2)
                                                                        && !pierZone.Contains(x.Zone)
                                                                        && !bufferZone.Contains(x.Zone));

                return (true, "success", result);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, WarehouseTable table)> GetTargetBoard(string PierName, string Barcode)
        {
            try
            {
                WarehouseTable result = listWarehouseTable.FirstOrDefault(x => x.PierName == PierName && x.Barcode == Barcode);

                return (true, "success", result);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }

}
