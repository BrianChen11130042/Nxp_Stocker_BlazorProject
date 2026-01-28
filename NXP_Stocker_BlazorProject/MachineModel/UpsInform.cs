namespace NXP_Stocker_BlazorProject.MachineModel
{

    public class UpsInform
    {
        public float InputVoltage { get; set; } = 0; // 單位:V

        public float InputFaultVoltage { get; set; } = 0; // 單位:V

        public float OutputVoltage { get; set; } = 0; // 單位:V

        public float OutputLoad { get; set; } = 0; // 單位:%

        public float InputFrequency { get; set; } = 0; // 單位:Hz

        public float BatteryVoltage { get; set; } = 0; // 單位:V

        public float Temperature { get; set; } = 0; // 單位: degrees of centigrade



        public int UtilityFail { get; set; } = 0; // 1:停電或牆壁電源電壓不穩  0:正常

        public int BatteryLow { get; set; } = 0; // 1:電池電量低  0:正常

        public int BypassBoostActive { get; set; } = 0; // 1:UPS正在使用旁路模式或升壓模式  0:正常

        public int UpsFault { get; set; } = 0; // 1:UPS故障  0:正常

        public int UpsType { get; set; } = 0;// 1:Standby(離線式)  0:On-line (在線式)

        public int TestInProgress { get; set; } = 0; // 1:正在進行自我檢查測試  0:不在測試狀態

        public int ShutdownActive { get; set; } = 0; // 1:UPS正在關機  0:沒有關機
    }
}
