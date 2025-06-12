using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;
using System.Text.RegularExpressions;

namespace sodoff.Services;
public class GameDataService {
    private readonly DBContext ctx;

    public GameDataService(DBContext ctx) {
        this.ctx = ctx;
    }

    public bool SaveGameData(Viking viking, int gameId, bool isMultiplayer, int difficulty, int gameLevel, string xmlDocumentData, bool win, bool loss) {
        //TODO: save only unique scores; scores that the player hasn't hit yet, probably keep old date for existing scores
        //or don't if you want ultra-unique scores (overwriting even the old score)
        Model.GameData? gameData = viking.GameData.FirstOrDefault(x => x.GameId == gameId && x.IsMultiplayer == isMultiplayer && x.Difficulty == difficulty && x.GameLevel == gameLevel && x.Win == win && x.Loss == loss);
        
        if (gameData == null) { //comment this check to turn off ultra-unique scores (for now)
            gameData = new Model.GameData {
                GameId = gameId,
                IsMultiplayer = isMultiplayer,
                Difficulty = difficulty,
                GameLevel = gameLevel,
                Win = win,
                Loss = loss,
                GameDataPairs = new List<GameDataPair>()
            };
            viking.GameData.Add(gameData);

        }
        gameData.DatePlayed = DateTime.UtcNow;
        
        DailyHighscore? daily = viking.DailyHighscores.FirstOrDefault(x => x.GameId == gameId && x.IsMultiplayer == isMultiplayer && x.Difficulty == difficulty && x.GameLevel == gameLevel);
        if (daily == null) {
            daily = new DailyHighscore {
                GameId = gameId,
                Difficulty = difficulty,
                GameLevel = gameLevel,
                IsMultiplayer = isMultiplayer,
                ScorePairs = new List<DailyHighscorePair>()
            };
            viking.DailyHighscores.Add(daily);
        }
        SavePairs(gameData, daily, xmlDocumentData);
        ctx.SaveChanges();
        
        return true;
    }
    
    List<GameDataResponse> GameDataResponseToList(IQueryable<Model.GameData> originalQuery, string key, int count, bool AscendingOrder, string apiKey) {
        var query = originalQuery.SelectMany(e => e.GameDataPairs)
            .Where(x => x.Name == key);
        
        if (AscendingOrder)
            query = query.OrderBy(e => e.Value);
        else
            query = query.OrderByDescending(e => e.Value);

        uint gameVersion = ClientVersion.GetVersion(apiKey);
        if (gameVersion <= ClientVersion.Max_OldJS)
            // use DisplayName instead of Name
            return query.Select(e => new GameDataResponse(
                XmlUtil.DeserializeXml<AvatarData>(e.GameData.Viking.AvatarSerialized).DisplayName, e.GameData.Viking.Uid, e.GameData.DatePlayed, e.GameData.Win, e.GameData.Loss, e.Value)
            ).Take(count).ToList();
        else
            return query.Select(e => new GameDataResponse(
                e.GameData.Viking.Name, e.GameData.Viking.Uid, e.GameData.DatePlayed, e.GameData.Win, e.GameData.Loss, e.Value)
            ).Take(count).ToList();
    }

    public GameDataSummary GetGameData(Viking viking, int gameId, bool isMultiplayer, int difficulty, int gameLevel, string key, int count, bool AscendingOrder, bool buddyFilter, string apiKey, DateTime? startDate = null, DateTime? endDate = null) {
        IQueryable<Model.GameData> query = ctx.GameData.Where(x => x.GameId == gameId && x.IsMultiplayer == false && x.Difficulty == difficulty && x.GameLevel == gameLevel);
        
        // TODO: Buddy filter

        if (startDate != null && endDate != null)
            query = query.Where(x => x.DatePlayed >= startDate.Value.ToUniversalTime() && x.DatePlayed <= endDate.Value.AddMinutes(2).ToUniversalTime());

        List<GameDataResponse> selectedData = GameDataResponseToList(query, key, count, AscendingOrder, apiKey);

        return GetSummaryFromResponse(viking, isMultiplayer, difficulty, gameLevel, key, selectedData);
    }

    public GameDataSummary GetDailyGameData(Viking viking, int gameId, bool isMultiplayer, int difficulty, int gameLevel, string key, int count, bool AscendingOrder, bool buddyFilter, string apiKey) {
        IQueryable<DailyHighscorePair> query = ctx.DailyHighscores
            .Where(x => x.GameId == gameId && x.IsMultiplayer == false && x.Difficulty == difficulty && x.GameLevel == gameLevel)
            .SelectMany(e => e.ScorePairs).Where(x => x.DatePlayed == DateTime.Today && x.Name == key);

        // TODO: Buddy filter

        if (AscendingOrder) query = query.OrderBy(e => e.Value);
        else query = query.OrderByDescending(e => e.Value);

        List<GameDataResponse> selectedData;
        if (ClientVersion.GetVersion(apiKey) <= ClientVersion.Max_OldJS)
            // use DisplayName instead of Name
            selectedData = query.Select(e => new GameDataResponse(
                XmlUtil.DeserializeXml<AvatarData>(e.DailyScore.Viking.AvatarSerialized).DisplayName, e.DailyScore.Viking.Uid, e.DatePlayed, false, false, e.Value)
            ).Take(count).ToList();
        else
            selectedData = query.Select(e => new GameDataResponse(
                e.DailyScore.Viking.Name, e.DailyScore.Viking.Uid, e.DatePlayed, false, false, e.Value)
            ).Take(count).ToList();

        return GetSummaryFromResponse(viking, isMultiplayer, difficulty, gameLevel, key, selectedData);
    }
    
    // ByUser for JumpStart's My Scores
    public GameDataSummary GetGameDataByUser(Viking viking, int gameId, bool isMultiplayer, int difficulty, int gameLevel, string key, int count, bool AscendingOrder, string apiKey) {
        IQueryable<Model.GameData> query = ctx.GameData.Where(x => x.GameId == gameId && x.IsMultiplayer == false && x.Difficulty == difficulty && x.GameLevel == gameLevel && x.VikingId == viking.Id);

        List<GameDataResponse> selectedData = GameDataResponseToList(query, key, count, AscendingOrder, apiKey);

        return GetSummaryFromResponse(viking, isMultiplayer, difficulty, gameLevel, key, selectedData);
    }

    public GetGameDataResponse GetGameDataForPlayer(Viking viking, GetGameDataRequest request) {
        GetGameDataResponse response = new();
        if (request.GameID is null)
            return response;

        var dbData = viking.GameData.Where(x => x.GameId == request.GameID)
            .SelectMany(e => e.GameDataPairs)
            .Select(x => new { x.Name, x.Value, x.GameData.DatePlayed, x.GameData.Win, x.GameData.Loss, x.GameData.IsMultiplayer, x.GameData.Difficulty, x.GameData.GameLevel });
        foreach (var data in dbData) {
            response.GameDataSummaryList.Add(new GameDataSummary {
                GameID = (int)request.GameID,
                IsMultiplayer = data.IsMultiplayer,
                Difficulty = data.Difficulty,
                GameLevel = data.GameLevel,
                Key = data.Name,
                GameDataList = new Schema.GameData[] {
                    new Schema.GameData {
                        IsMember = true,
                        Value = data.Value,
                        DatePlayed = data.DatePlayed,
                        Win = data.Win ? 1 : 0,
                        Loss = data.Loss ? 1 : 0,
                        UserID = viking.Uid
                    }
                }
            });
        }

        return response;
    }

    private GameDataSummary GetSummaryFromResponse(Viking viking, bool isMultiplayer, int difficulty, int gameLevel, string key, List<GameDataResponse> selectedData) {
        GameDataSummary gameData = new();
        gameData.IsMultiplayer = isMultiplayer;
        gameData.Difficulty = difficulty;
        gameData.GameLevel = gameLevel;
        gameData.Key = key;
        gameData.UserPosition = -1;
        gameData.GameDataList = new Schema.GameData[selectedData.Count];
        for (int i = 0; i < selectedData.Count; i++) {
            Schema.GameData data = new();
            data.RankID = i + 1;
            data.IsMember = true;
            data.UserName = selectedData[i].Name;
            data.Value = selectedData[i].Value;
            data.DatePlayed = selectedData[i].DatePlayed;
            data.Win = selectedData[i].Win ? 1 : 0;
            data.Loss = selectedData[i].Loss ? 1 : 0;
            data.UserID = selectedData[i].Uid;
            gameData.GameDataList[i] = data;
            if (data.UserName == viking.Name && gameData.UserPosition == -1)
                gameData.UserPosition = i;
        }
        if (gameData.UserPosition == -1)
            gameData.UserPosition = selectedData.Count;
        return gameData;
    }

    private void SavePairs(Model.GameData gameData, DailyHighscore daily, string xmlDocumentData) {
        foreach (var pair in GetGameDataPairs(xmlDocumentData)) {
            GameDataPair? dbPair = gameData.GameDataPairs.FirstOrDefault(x => x.Name == pair.Name);
            if (dbPair == null)
                gameData.GameDataPairs.Add(pair);
            else if (pair.Name == "time" && dbPair.Value > pair.Value)
                dbPair.Value = pair.Value;
            else if (pair.Name != "time" && dbPair.Value <= pair.Value)
                dbPair.Value = pair.Value;

            DailyHighscorePair? dailyPair = daily.ScorePairs.FirstOrDefault(x => x.Name == pair.Name);
            if (dailyPair == null) {
                daily.Difficulty = gameData.Difficulty;
                daily.GameLevel = gameData.GameLevel;
                dailyPair = new DailyHighscorePair {
                    Name = pair.Name,
                    Value = pair.Value,
                    DatePlayed = DateTime.Today
                };
                daily.ScorePairs.Add(dailyPair);
            } else if (
                (pair.Name == "time" && dailyPair.Value > pair.Value) ||  // Better Time
                (pair.Name != "time" && dailyPair.Value <= pair.Value) || // Better Score
                dailyPair.DatePlayed != DateTime.Today                    // Another Day
            ) {
                daily.Difficulty = gameData.Difficulty;
                daily.GameLevel = gameData.GameLevel;
                dailyPair.DatePlayed = DateTime.Today;
                dailyPair.Value = pair.Value;
            }
        }
    }

    private ICollection<GameDataPair> GetGameDataPairs(string xmlDocumentData) {
        List<GameDataPair> pairs = new();
        foreach (Match match in Regex.Matches(xmlDocumentData, @"<(\w+)>(.*?)<\/\1>")) {
            pairs.Add(new GameDataPair {
                Name = match.Groups[1].Value,
                Value = int.Parse(match.Groups[2].Value)
            });
        }
        return pairs;
    }

    struct GameDataResponse {
        public GameDataResponse(string Name, Guid Uid, DateTime DatePlayed, bool Win, bool Loss, int Value) {
            this.Name = Name;
            this.Uid = Uid;
            this.DatePlayed = DatePlayed;
            this.Win = Win;
            this.Loss = Loss;
            this.Value = Value;
        }
        public string Name;
        public Guid Uid;
        public DateTime DatePlayed;
        public bool Win;
        public bool Loss;
        public int Value;
    }
}
