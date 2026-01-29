namespace NXP_Stocker_BlazorProject.MachineModel
{
    public enum EMachineUnit
    {
        Pier1,
        Pier2,
        Robot,
        UPS
    }

    public class MachineUnitStatus
    {
        ERobotStatus robotStatus { get; set; } = ERobotStatus.None;

        EPierStatus pier1Status { get; set; } = EPierStatus.None;

        EPierStatus pier2Status { get; set; } = EPierStatus.None;

        EUpsStatus upsStatus { get; set; } = EUpsStatus.None;
    }

    public enum ERobotStatus
    {
        None = 0,
        Init = 900,
        TryConnecting = 901,
        DisConnect = 902,
        FetchingData = 903,
        Idle = 904,
        Running = 905,
        Pause = 906,
        Stop = 907,
        Error = 908,

        PickMotorMove = 1,
        PickArmStandby = 2,
        PickPier1LargeLB = 3,
        PickPier1SmallLB = 4,
        PickPier2LargeLB = 5,
        PickPier2SmallLB = 6,
        PickLargeLB = 7,
        PickSmallLB = 8,

        ReadBarcode = 9,
        ReadBarcodeFail = 10,

        DropMotorMove = 11,
        DropArmStandby = 12,
        DropPier1LargeLB = 13,
        DropPier1SmallLB = 14,
        DropPier2LargeLB = 15,
        DropPier2SmallLB = 16,
        DropLargeLB = 17,
        DropSmallLB = 18,

        MissionComplete = 20,
    }

    public enum EPierStatus
    {
        None = 0,
        Init = 900,
        TryConnecting = 901,
        DisConnect = 902,
        FetchingData = 903,
        Idle = 904,
        Running = 905,
        Pause = 906,
        Stop = 907,
        Error = 908,

        InputLargeLB_WaitingButtonPress_1 = 1,
        OutputLargeLB_WaitingButtonPress_1 = 11,
        InputSmallLB_WaitingButtonPress_1 = 21,
        OutputSmallLB_WaitingButtonPress_1 = 31,

        ShuttleStickOutUnsatisfied = 100,

        InputLargeLB_ShuttleStickOut = 2,
        OutputLargeLB_ShuttleStickOut = 12,
        InputSmallLB_ShuttleStickOut = 22,
        OutputSmallLB_ShuttleStickOut = 32,

        InputLargeLB_WaitingButtonPress_2 = 3,
        OutputLargeLB_WaitingButtonPress_2 = 13,
        InputSmallLB_WaitingButtonPress_2 = 23,
        OutputSmallLB_WaitingButtonPress_2 = 33,

        ShuttleRecedeUnsatisfied = 110,

        InputLargeLB_ShuttleRecede = 4,
        OutputLargeLB_ShuttleRecede = 14,
        InputSmallLB_ShuttleRecede = 24,
        OutputSmallLB_ShuttleRecede = 34,

        InputLargeLB_MissionComplete = 5,
        OutputLargeLB_MissionComplete = 15,
        InputSmallLB_MissionComplete = 25,
        OutputSmallLB_MissionComplete = 35,
    }

    public enum EUpsStatus
    {
        None = 0,
        Init = 900,
        TryConnecting = 901,
        DisConnect = 902,
        FetchingData = 903,
        Idle = 904,
        Running = 905,
        Pause = 906,
        Stop = 907,
        Error = 908,
    }
}
