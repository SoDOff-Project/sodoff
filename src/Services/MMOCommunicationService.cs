using System;
using System.Data.Common;
using System.Text.Json;
using Microsoft.Extensions.Options;
using sodoff.Configuration;
using sodoff.Model;

namespace sodoff.Services;

public class MMOCommunicationService
{
    public readonly DBContext dBContext;
    public readonly HttpClient httpClient;
    public readonly IOptions<ApiServerConfig> config;

    public MMOCommunicationService(DBContext dBContext, IOptions<ApiServerConfig> options)
    {
        this.dBContext = dBContext;
        config = options;
        httpClient = new HttpClient();
    }

    public bool SendPacketToRoom(string apiToken, string roomName, string cmd, string[] args)
    {
        var serializedArgs = JsonSerializer.Serialize(args);

        FormUrlEncodedContent form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "apiToken", apiToken },
            { "roomName", roomName },
            { "cmd", cmd },
            { "serializedArgs", serializedArgs }
        });
        try
        {
            var result = httpClient.PostAsync($"http://{config.Value.MMOAdress}:{config.Value.MMOHttpApiPort}/mmo/update/SendPacketToRoom", form)?.Result;
            if (result != null && result.StatusCode == System.Net.HttpStatusCode.OK) return true;
            else return false;
        } catch (AggregateException ex)
        {
            Console.WriteLine("MMO Communication Failiure. Please Investigate - " + ex.Message);
            return false;
        }
    }

    public bool SendPacketToPlayer(string apiToken, string userId, string cmd, string[] args)
    {
        var argsSerialized = JsonSerializer.Serialize(args);
        FormUrlEncodedContent form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "apiToken", apiToken },
            { "userId", userId },
            { "cmd", cmd },
            { "serializedArgs", argsSerialized }
        });

        try
        {
            var result = httpClient.PostAsync($"http://{config.Value.MMOAdress}:{config.Value.MMOHttpApiPort}/mmo/update/SendPacketToPlayer", form)?.Result;
            if (result != null && result.StatusCode == System.Net.HttpStatusCode.OK) return true;
            else return false;
        }
        catch (AggregateException ex)
        {
            Console.WriteLine("MMO Communication Failiure. Please Investigate - " + ex.Message);
            return false;
        }
    }

    public bool UpdateRoomVarsInRoom<T>(string apiToken, string roomName, Dictionary<string, T> vars)
    {
        var varsSerialized = JsonSerializer.Serialize(vars);
        FormUrlEncodedContent form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "apiToken", apiToken },
            { "roomName", roomName },
            { "serializedVars", varsSerialized }
        });

        try
        {
            var result = httpClient.PostAsync($"http://{config.Value.MMOAdress}:{config.Value.MMOHttpApiPort}/mmo/update/UpdateRoomVarsInRoom", form)?.Result;
            if (result != null && result.StatusCode == System.Net.HttpStatusCode.OK) return true;
            else return false;
        }
        catch (AggregateException ex)
        {
            Console.WriteLine("MMO Communication Failiure. Please Investigate - " + ex.Message);
            return false;
        }
    }
}
