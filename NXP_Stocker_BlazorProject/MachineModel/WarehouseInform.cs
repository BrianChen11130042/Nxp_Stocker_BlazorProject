namespace NXP_Stocker_BlazorProject.MachineModel
{
    public class WarehouseInform
    {
        public EPier pier { get; set; }

        public int zone { get; set; }

        public int layer { get; set; }

        public bool isOccupy { get; set; }

        public string barcode { get; set; }

        public int size { get; set; } // 0:大板 , 1:小板
    }
}
