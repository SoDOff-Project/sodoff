﻿using Microsoft.Extensions.Options;

using sodoff.Schema;
using sodoff.Util;
using sodoff.Configuration;

namespace sodoff.Services;

public class MMOConfigService {
    MMOConfig mmoconfig;
    private readonly IOptions<ApiServerConfig> config;

    public MMOConfigService(IOptions<ApiServerConfig> config) {
        this.config = config;
        this.mmoconfig = XmlUtil.DeserializeXml<MMOConfig>(XmlUtil.ReadResourceXmlString("mmo"));

        char[] delimiterChars = { ' ', ',', '\t', '\n' };
        foreach (var v in mmoconfig.Versions) {
            if (!String.IsNullOrEmpty(v.ZoneList)) {
                List<MMOServerData> MMOServerDataList = new();
                foreach (var ZoneName in v.ZoneList.Split(delimiterChars)) {
                    if (!String.IsNullOrWhiteSpace(ZoneName)) {
                        MMOServerDataList.Add(new MMOServerData {
                            IPAddress = config.Value.MMOAdress,
                            Port = config.Value.MMOPort,
                            Version = "S2X",
                            isDefault = true,
                            ZoneName = ZoneName.Trim(),
                            RootZone = "JumpStart"
                        } );
                    }
                }
                v.MMOServerDataArray = MMOServerDataList.ToArray();
            } else {
                foreach (var s in v.MMOServerDataArray) {
                    if (String.IsNullOrEmpty(s.IPAddress)) {
                        s.IPAddress = config.Value.MMOAdress;
                    }
                    if (s.Port == 0) {
                        s.Port = config.Value.MMOPort;
                    }
                }
            }
        }
    }

    public MMOServerInformation GetMMOServerInformation(uint version) {
        if (version >= config.Value.MMOSupportMinVersion) {
            return new MMOServerInformation {
                MMOServerDataArray = mmoconfig.Versions.FirstOrDefault(c => version >= c.VersionFirst && version <= c.VersionLast)?.MMOServerDataArray
            };
        } else {
            return new MMOServerInformation();
        }
    }
}
