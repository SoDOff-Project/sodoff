using sodoff.Model;

namespace sodoff.Util;
public class SavedData {
    public static string? Get(Viking? viking, uint saveId) {
        return viking?.SavedData.FirstOrDefault(s => s.SaveId == saveId)?.SerializedData;
    }

    public static void Set(Viking viking, uint saveId, string? contentXml) {
        Console.WriteLine($"\n\n{saveId} {contentXml}\n");
        Model.SavedData? savedData = viking.SavedData.FirstOrDefault(s => s.SaveId == saveId);
        if (savedData is null) {
            savedData = new() {
                SaveId = saveId,
                SerializedData = contentXml
            };
            viking.SavedData.Add(savedData);
        } else {
            savedData.SerializedData = contentXml;
        }
    }
}
