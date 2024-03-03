namespace sodoff.Util;
public class ClientVersion {
    public const uint Min_SoD = 0xa0000000;
    public const uint MaM     = 0x00800000;
    public const uint MB      = 0x00700000;
    public const uint WoJS    = 0x00600000;

    public static uint GetVersion(string apiKey) {
        if (
            apiKey == "b99f695c-7c6e-4e9b-b0f7-22034d799013" || // PC / Windows
            apiKey == "515af675-bec7-4c42-ba64-7bfaf198d8ea" || // Android
            apiKey == "1e7ccc3a-4adb-4736-b843-7b3da5140a43"    // iOS
        ) {
            return 0xa3a31a0a;
        } else if (
            apiKey.EndsWith("7c6e-4e9b-b0f7-22034d799013")
        ) {
            return Convert.ToUInt32(apiKey.Substring(0, 8), 16);
        } else if (
            apiKey == "e20150cc-ff70-435c-90fd-341dc9161cc3"
        ) {
            return MaM;
        } else if (
            apiKey == "6738196d-2a2c-4ef8-9b6e-1252c6ec7325"
        ) {
            return MB;
        } else if (
            apiKey == "1552008f-4a95-46f5-80e2-58574da65875"
        ) {
            return WoJS;
        } else if (
            apiKey == "b4e0f71a-1cda-462a-97b3-0b355e87e0c8"
        ) {
            return WoJS+10; // WoJS--Adventureland
        }
        return 0;
    }

    public static bool IsMultiplayerSupported(string apiKey) {
        return GetVersion(apiKey) >= 0xa3a19a0a;
    }
}
