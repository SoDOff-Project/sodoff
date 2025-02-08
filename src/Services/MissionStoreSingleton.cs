using sodoff.Schema;
using sodoff.Util;
using System.Runtime.Serialization.Formatters.Binary;

namespace sodoff.Services;
public class MissionStoreSingleton {

    private Dictionary<int, Mission> missions = new();
    private int[] activeMissions;
    private int[] upcomingMissions;
    private int[] activeMissionsV1;
    private int[] upcomingMissionsV1;
    private int[] activeMissionsMaM;
    private int[] upcomingMissionsMaM;
    private int[] activeMissionsWoJS;
    private int[] upcomingMissionsWoJS;

    public MissionStoreSingleton() {
        ServerMissionArray missionArray = XmlUtil.DeserializeXml<ServerMissionArray>(XmlUtil.ReadResourceXmlString("missions.missions_sod"));
        DefaultMissions defaultMissions = XmlUtil.DeserializeXml<DefaultMissions>(XmlUtil.ReadResourceXmlString("missions.defaultmissionlist"));
        foreach (var mission in missionArray.MissionDataArray) {
            SetUpRecursive(mission);
        }
        activeMissions = defaultMissions.Active;
        upcomingMissions = defaultMissions.Upcoming;

        defaultMissions = XmlUtil.DeserializeXml<DefaultMissions>(XmlUtil.ReadResourceXmlString("missions.defaultmissionlist_sod_v1"));
        activeMissionsV1 = defaultMissions.Active;
        upcomingMissionsV1 = defaultMissions.Upcoming;

        missionArray = XmlUtil.DeserializeXml<ServerMissionArray>(XmlUtil.ReadResourceXmlString("missions.missions_mam"));
        defaultMissions = XmlUtil.DeserializeXml<DefaultMissions>(XmlUtil.ReadResourceXmlString("missions.defaultmissionlist_mam"));
        foreach (var mission in missionArray.MissionDataArray) {
            SetUpRecursive(mission);
        }
        activeMissionsMaM = defaultMissions.Active;
        upcomingMissionsMaM = defaultMissions.Upcoming;

        missionArray = XmlUtil.DeserializeXml<ServerMissionArray>(XmlUtil.ReadResourceXmlString("missions.missions_wojs"));
        defaultMissions = XmlUtil.DeserializeXml<DefaultMissions>(XmlUtil.ReadResourceXmlString("missions.defaultmissionlist_wojs"));
        foreach (var mission in missionArray.MissionDataArray) {
            SetUpRecursive(mission);
        }
        activeMissionsWoJS = defaultMissions.Active;
        upcomingMissionsWoJS = defaultMissions.Upcoming;
    }

    public Mission GetMission(int missionID) {
        return DeepCopy(missions[missionID]);
    }

    public int[] GetActiveMissions(uint gameVersion) {
        if (gameVersion >= 0xa2a00a0a) {
            return activeMissions;
        }
        if (gameVersion >= ClientVersion.Min_SoD) {
            return activeMissionsV1;
        }
        if (gameVersion == ClientVersion.MaM) {
            return activeMissionsMaM;
        }
        if ((gameVersion & ClientVersion.WoJS) != 0) {
            return activeMissionsWoJS;
        }
        return new int[0];
    }

    public int[] GetUpcomingMissions(uint gameVersion) {
        if (gameVersion >= 0xa2a00a0a) {
            return upcomingMissions;
        }
        if (gameVersion >= ClientVersion.Min_SoD) {
            return upcomingMissionsV1;
        }
        if (gameVersion == ClientVersion.MaM) {
            return upcomingMissionsMaM;
        }
        if ((gameVersion & ClientVersion.WoJS) != 0) {
            return upcomingMissionsWoJS;
        }
        return new int[0];
    }

    private void SetUpRecursive(Mission mission) {
        missions.Add(mission.MissionID, mission);
        foreach (var innerMission in mission.Missions) {
            SetUpRecursive(innerMission);
        }
    }

    // FIXME: Don't use BinaryFormatter for deep copying
    // FIXME: Remove <EnableUnsafeBinaryFormatterSerialization> flag from the project file once we have a different way of deep copying
    public static Mission DeepCopy(Mission original) {
        using (MemoryStream memoryStream = new MemoryStream()) {
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(memoryStream, original);

            memoryStream.Position = 0;

            Mission clone = (Mission)formatter.Deserialize(memoryStream);

            return clone;
        }
    }

}
