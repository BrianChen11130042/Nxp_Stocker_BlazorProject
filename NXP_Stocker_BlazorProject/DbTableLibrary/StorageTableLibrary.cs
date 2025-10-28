using NXP_Stocker_BlazorProject.DbTableLibrary.Interface;

namespace NXP_Stocker_BlazorProject.DbTableLibrary
{

    public partial class StorageTableLibrary
    {

        readonly IServiceProvider serviceProvider;

        public StorageTableLibrary(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            Init_Delete(); //砍掉
        }

        //*************下面砍掉*************//

        List<StorageTable> listStorageTable { get; set; } = new List<StorageTable>();

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
                    StorageTable data = new StorageTable()
                    {
                        PierName = "Pier1",
                        Zone = i,
                        Layer = j,
                        BoardSizeSpec = 2,
                        IsOccupy = false,
                        Barcode = string.Empty,
                        BoardSize = 999,
                    };

                    listStorageTable.Add(data);
                }
            }

            for(int i = 1 ; i <= 13 ; i++)
            {
                StorageTable data = new StorageTable()
                {
                    PierName = "Pier1",
                    Zone = 6,
                    Layer = i,
                    BoardSizeSpec = 0,
                    IsOccupy = false,
                    Barcode = string.Empty,
                    BoardSize = 999,
                };

                listStorageTable.Add(data);
            }

            for (int i = 1; i <= 5; i++)
            {
                StorageTable data = new StorageTable()
                {
                    PierName = "Pier1",
                    Zone = 13,
                    Layer = i,
                    BoardSizeSpec = 2,
                    IsOccupy = false,
                    Barcode = string.Empty,
                    BoardSize = 999,
                };

                listStorageTable.Add(data);
            }

            for(int i = 1; i <= 1; i++)
            {
                StorageTable data = new StorageTable()
                {
                    PierName = "Pier1",
                    Zone = 20,
                    Layer = i,
                    BoardSizeSpec = 2,
                    IsOccupy = false,
                    Barcode = string.Empty,
                    BoardSize = 999,
                };

                listStorageTable.Add(data);
            }
        }

        void Init_Pier2_Delete()
        {
            for (int i = 7; i <= 10; i++)
            {
                for (int j = 1; j <= 18; j++)
                {
                    StorageTable data = new StorageTable()
                    {
                        PierName = "Pier2",
                        Zone = i,
                        Layer = j,
                        BoardSizeSpec = 2,
                        IsOccupy = false,
                        Barcode = string.Empty,
                        BoardSize = 999,
                    };

                    listStorageTable.Add(data);
                }
            }

            for(int i = 11 ; i <= 12 ; i++)
            {
                for (int j = 1; j <= 13; j++)
                {
                    StorageTable data = new StorageTable()
                    {
                        PierName = "Pier2",
                        Zone = i,
                        Layer = j,
                        BoardSizeSpec = 0,
                        IsOccupy = false,
                        Barcode = string.Empty,
                        BoardSize = 999,
                    };

                    listStorageTable.Add(data);
                }
            }

            for (int i = 1; i <= 5; i++)
            {
                StorageTable data = new StorageTable()
                {
                    PierName = "Pier2",
                    Zone = 14,
                    Layer = i,
                    BoardSizeSpec = 2,
                    IsOccupy = false,
                    Barcode = string.Empty,
                    BoardSize = 999,
                };

                listStorageTable.Add(data);
            }

            for (int i = 1; i <= 1; i++)
            {
                StorageTable data = new StorageTable()
                {
                    PierName = "Pier2",
                    Zone = 21,
                    Layer = i,
                    BoardSizeSpec = 2,
                    IsOccupy = false,
                    Barcode = string.Empty,
                    BoardSize = 999,
                };

                listStorageTable.Add(data);
            }
        }


        //**********************************//
    }

    //**********下面先取代DB 要砍掉*************//

    public class StorageTable
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

    public partial class StorageTableLibrary : IStorageTableOperate
    {
        public async Task<(bool status, string msg, List<StorageTable> list)> GetAllStoragePerPier(string PierName)
        {
            try
            {
                List<StorageTable> result = listStorageTable.Where(x => x.PierName == PierName).ToList();

                return (true, "success", result);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, StorageTable teble)> GetEmptyStorage(string PierName, int BoardSize)
        {
            try
            {
                StorageTable result = listStorageTable.FirstOrDefault(x => x.PierName == PierName && x.BoardSizeSpec == BoardSize);

                return (true, "success", result);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool status, string msg, StorageTable teble)> GetTargetStroage(string PierName, string Barcode)
        {
            try
            {
                StorageTable result = listStorageTable.FirstOrDefault(x => x.PierName == PierName && x.Barcode == Barcode);

                return (true, "success", result);
            }
            catch(Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }

}
