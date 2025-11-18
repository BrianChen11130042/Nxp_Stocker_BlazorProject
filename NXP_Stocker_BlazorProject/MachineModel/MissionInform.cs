namespace NXP_Stocker_BlazorProject.MachineModel
{
    public class MissionInform
    {
        public int pierNo { get; set; } // 1:pier1 , 2:pier2 

        public int action { get; set; } // 1:入倉儲 , 2:出倉儲 , 3:儲位至Buffer

        public string barcode { get; set; }

        public int size { get; set; } // 0:大板 , 1:小板

        public int pickZone { get; set; }

        public int pickLayer { get; set; }

        public int dropZone { get; set; }

        public int dropLayer { get; set; }
    }
}
