using Microsoft.AspNetCore.Mvc;
using sodoff.Attributes;
using sodoff.Model;
using sodoff.Schema;
using sodoff.Util;
using GroupMember = sodoff.Model.GroupMember;

namespace sodoff.Controllers.Common;

public class GroupController : Controller {
    // Any permission that is commented out is not implemented.
    private static readonly List<string> PermissionsMember = [
        //"Delete Own Msg",
        //"Post Message"
    ];
    private static readonly List<string> PermissionsElder = [
        //"Invite",
        "Approve Join Request",
        //"Post News",
        //"Delete Own Msg",
        //"Delete Any Msg",
        //"Delete News",
        //"Post Message",
        "Remove Member"
    ];
    private static readonly List<string> PermissionsLeader = [
        //"Invite",
        "Approve Join Request",
        "Assign Leader",
        "Assign Elder",
        "Demote Elder",
        "Edit Group",
        //"Post News",
        //"Delete Own Msg",
        //"Delete Any Msg",
        //"Delete News",
        //"Post Message",
        "Remove Member"
    ];
    
    private static readonly List<RolePermission> RolePermissions = [
        new RolePermission {
            GroupType = GroupType.Public,
            Role = UserRole.Member,
            Permissions = PermissionsMember
        }, new RolePermission {
            GroupType = GroupType.Public,
            Role = UserRole.Elder,
            Permissions = PermissionsElder
        }, new RolePermission {
            GroupType = GroupType.Public,
            Role = UserRole.Leader,
            Permissions = PermissionsLeader
        },
        
        new RolePermission {
            GroupType = GroupType.MembersOnly,
            Role = UserRole.Member,
            Permissions = PermissionsMember
        }, new RolePermission {
            GroupType = GroupType.MembersOnly,
            Role = UserRole.Elder,
            Permissions = PermissionsElder
        }, new RolePermission {
            GroupType = GroupType.MembersOnly,
            Role = UserRole.Leader,
            Permissions = PermissionsLeader
        },
        
        new RolePermission {
            GroupType = GroupType.Private,
            Role = UserRole.Member,
            Permissions = PermissionsMember
        }, new RolePermission {
            GroupType = GroupType.Private,
            Role = UserRole.Elder,
            Permissions = PermissionsElder
        }, new RolePermission {
            GroupType = GroupType.Private,
            Role = UserRole.Leader,
            Permissions = PermissionsLeader
        }
    ];

    private readonly DBContext ctx;

    public GroupController(DBContext ctx) {
        this.ctx = ctx;
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/GroupWebService.asmx/CreateGroup")]
    [VikingSession]
    public IActionResult CreateGroup(Viking viking, [FromForm] string apiKey, [FromForm] string groupCreateRequest) {
        uint gameId = ClientVersion.GetGameID(apiKey);

        if (viking.GroupMembership != null) {
            return Ok(new CreateGroupResult { Success = false, Status = CreateGroupStatus.CreatorIsNotApproved });
        }

        CreateGroupRequest request = XmlUtil.DeserializeXml<CreateGroupRequest>(groupCreateRequest);
        request.Name = request.Name.Trim();

        // Cue the gauntlet of validity checks.
        if (request.Type <= GroupType.System) return Ok(new CreateGroupResult { Success = false, Status = CreateGroupStatus.GroupTypeIsInvalid });
        //if (request.MaxMemberLimit < 4) return Ok(new CreateGroupResult { Success = false, Status = CreateGroupStatus.GroupMaxMemberLimitInvalid }); // Not actually used by the game.
        if (request.Name.Length == 0) return Ok(new CreateGroupResult { Success = false, Status = CreateGroupStatus.GroupNameIsEmpty });
        if (request.Description.Length == 0) return Ok(new CreateGroupResult { Success = false, Status = CreateGroupStatus.GroupDescriptionIsEmpty });
        if (ctx.Groups.Any(g => g.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase))) return Ok(new CreateGroupResult { Success = false, Status = CreateGroupStatus.GroupNameIsDuplicate });

        Model.Group group = new Model.Group {
            Name = request.Name,
            Description = request.Description,
            Logo = request.Logo,
            Color = request.Color,
            Type = request.Type,
            CreateDate = DateTime.Now,
            GameID = gameId,
            MaxMemberLimit = 50,
            GroupID = Guid.NewGuid(),
            Vikings = new List<GroupMember>()
        };

        ctx.Groups.Add(group);
        group.Vikings.Add(new GroupMember {
            Viking = viking,
            Group = group,
            UserRole = UserRole.Leader,
            JoinDate = group.CreateDate
        });
        group.LastActiveTime = group.CreateDate;
        ctx.SaveChanges();

        return Ok(new CreateGroupResult {
            Success = true,
            Status = CreateGroupStatus.Success,
            Group = new Schema.Group {
                Name = group.Name,
                Description = group.Description,
                Logo = group.Logo,
                Color = group.Color,
                Type = group.Type,
                OwnerID = viking.Uid.ToString(),
                Points = 0,//group.Points,
                Active = false,//group.Vikings.Count >= 4,
                MemberLimit = group.MaxMemberLimit,
                GroupID = group.GroupID.ToString()
            }
            // TODO: Delete after 15 days of less than 4 members.
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/GroupWebService.asmx/EditGroup")]
    [VikingSession]
    public IActionResult EditGroup(Viking viking, [FromForm] string apiKey, [FromForm] string groupEditRequest) {
        EditGroupRequest request = XmlUtil.DeserializeXml<EditGroupRequest>(groupEditRequest);
        request.Name = request.Name.Trim();

        GroupMember? vikingRole = viking.GroupMembership;
        if (vikingRole == null || vikingRole.Group.GroupID.ToString() != request.GroupID) {
            return Ok(new EditGroupResult { Success = false, Status = EditGroupStatus.GroupNotFound });
        } else if (vikingRole.UserRole < UserRole.Elder) {
            return Ok(new EditGroupResult { Success = false, Status = EditGroupStatus.PermissionDenied });
        }

        // Cue the gauntlet of validity checks.
        if (request.Type != null && request.Type <= GroupType.System) return Ok(new EditGroupResult { Success = false, Status = EditGroupStatus.GroupTypeIsInvalid });
        //if (request.MaxMemberLimit < 4) return Ok(new EditGroupResult { Success = false, Status = EditGroupStatus.GroupMaxMemberLimitInvalid }); // Not actually used by the game.
        if ((request.Name?.Length ?? 0) == 0) request.Name = vikingRole.Group.Name;
        if ((request.Description?.Length ?? 0) == 0) request.Description = vikingRole.Group.Description;
        if (request.Name != vikingRole.Group.Name && ctx.Groups.Any(g => g.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase))) return Ok(new EditGroupResult { Success = false, Status = EditGroupStatus.GroupNameIsDuplicate });
        
        vikingRole.Group.Name = request.Name;
        vikingRole.Group.Description = request.Description;
        if (request.Type != null) vikingRole.Group.Type = (GroupType)request.Type;
        if ((request.Color?.Length ?? 0) > 0) vikingRole.Group.Color = request.Color;
        if ((request.Logo?.Length ?? 0) > 0) vikingRole.Group.Logo = request.Logo;
        ctx.SaveChanges();

        return Ok(new EditGroupResult {
            Success = true,
            Status = EditGroupStatus.Success,
            NewRolePermissions = RolePermissions
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("GroupWebService.asmx/JoinGroup")]
    [VikingSession]
    public IActionResult JoinGroupV1(Viking viking, [FromForm] string apiKey, [FromForm] string groupID) {
        Guid parsedGroupID = Guid.Parse(groupID);
        uint gameId = ClientVersion.GetGameID(apiKey);
        
        // Check for loyalty.
        if (viking.GroupMembership != null) {
            return Ok(new JoinGroupResult { GroupStatus = GroupMembershipStatus.SELF_BLOCKED });
        }
        Model.Group? group = ctx.Groups.FirstOrDefault(g => g.GroupID == parsedGroupID);
        if (group != null) {
            // This check is only on this side to prevent people from attempting to circumvent the join limit.
            if (group.Type <= GroupType.System || group.Vikings.Count < group.MaxMemberLimit) {
                group.Vikings.Add(new GroupMember {
                    Viking = viking,
                    Group = group,
                    UserRole = UserRole.Member
                });
                ctx.SaveChanges();
                return Ok(new JoinGroupResult { GroupStatus = GroupMembershipStatus.APPROVED });
            }
        }
        return Ok(new JoinGroupResult { GroupStatus = GroupMembershipStatus.REJECTED });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/GroupWebService.asmx/JoinGroup")]
    [VikingSession]
    public IActionResult JoinGroup(Viking viking, [FromForm] string apiKey, [FromForm] string groupJoinRequest) {
        uint gameId = ClientVersion.GetGameID(apiKey);

        JoinGroupRequest request = XmlUtil.DeserializeXml<JoinGroupRequest>(groupJoinRequest);
        Guid parsedGroupID = Guid.Parse(request.GroupID);
        Model.Group? group = ctx.Groups.FirstOrDefault(g => g.GroupID == parsedGroupID);
        if (group == null) return Ok(new GroupJoinResult { Success = false, Status = JoinGroupStatus.Error });
        if (group.Type >= GroupType.Private) {
            return Ok(new GroupJoinResult { Success = false, Status = JoinGroupStatus.GroupTypeIsNotPublic });
        }
        
        GroupMember? existing = viking.GroupMembership;
        if (existing != null) {
            if (existing.Group == group)
                return Ok(new GroupJoinResult { Success = false, Status = JoinGroupStatus.UserAlreadyMemberOfTheGroup });
            
            existing.Group.Vikings.Remove(existing);
            if (!existing.Group.Vikings.Any()) ctx.Groups.Remove(existing.Group);
        }
        
        if (group.Type == GroupType.MembersOnly) {
            if (!group.JoinRequests.Any(r => r.Viking == viking))
                group.JoinRequests.Add(new GroupJoinRequest {
                    Group = group,
                    Viking = viking,
                    //Message = request.Message // For future implemention, once moderation is possible.
                });
            ctx.SaveChanges();
            return Ok(new GroupJoinResult { Success = false, Status = JoinGroupStatus.JoinRequestPending });
        }

        if (group.Vikings.Count >= group.MaxMemberLimit) 
            return Ok(new GroupJoinResult { Success = false, Status = JoinGroupStatus.GroupIsFull });
        
        GroupMember joinee = new GroupMember {
            Viking = viking,
            Group = group,
            UserRole = UserRole.Member,
            JoinDate = DateTime.Now
        };
        group.Vikings.Add(joinee);
        group.LastActiveTime = joinee.JoinDate;
        ctx.SaveChanges();
        return Ok(new GroupJoinResult { Success = true, Status = JoinGroupStatus.Success });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/GroupWebService.asmx/LeaveGroup")]
    [VikingSession]
    public IActionResult LeaveGroup(Viking viking, [FromForm] string groupLeaveRequest) {
        LeaveGroupRequest request = XmlUtil.DeserializeXml<LeaveGroupRequest>(groupLeaveRequest);
        GroupMember? vikingRole = viking.GroupMembership;
        if (vikingRole == null || vikingRole.Group.GroupID.ToString() != request.GroupID)
            return Ok(new LeaveGroupResult { Success = false, Status = LeaveGroupStatus.Error });
        GroupMember? targetRole;
        if (viking.Uid.ToString().Equals(request.UserID, StringComparison.CurrentCultureIgnoreCase)) {
            targetRole = vikingRole.Group.Vikings.FirstOrDefault(gv => gv.Viking == viking);
        } else if (vikingRole.UserRole >= UserRole.Elder) {
            targetRole = vikingRole.Group.Vikings.FirstOrDefault(gv => gv.Viking.Uid.ToString() == request.UserID);
        } else return Ok(new LeaveGroupResult { Success = false, Status = LeaveGroupStatus.Error });

        if (targetRole == null)
            return Ok(new LeaveGroupResult { Success = false, Status = LeaveGroupStatus.UserNotAMemberOfTheGroup });
        vikingRole.Group.Vikings.Remove(targetRole);
        if (!vikingRole.Group.Vikings.Any()) ctx.Groups.Remove(vikingRole.Group);
        ctx.SaveChanges();
        return Ok(new LeaveGroupResult { Success = true, Status = LeaveGroupStatus.Success });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/GroupWebService.asmx/GetGroups")]
    public IActionResult GetGroups([FromForm] string apiKey, [FromForm] string getGroupsRequest) {
        uint gameId = ClientVersion.GetGameID(apiKey);

        GetGroupsRequest request = XmlUtil.DeserializeXml<GetGroupsRequest>(getGroupsRequest);
        IEnumerable<Model.Group> groups = ctx.Groups;
        if (request.ForUserID != null) {
            Viking? target = ctx.Vikings.FirstOrDefault(v => request.ForUserID.ToUpper() == v.Uid.ToString());
            if (target == null) return Ok(new GetGroupsResult { Success = false });
            if (target.GroupMembership?.Group == null) return Ok(new GetGroupsResult { Success = true });
            groups = [target.GroupMembership.Group];
        } else {
            groups = groups.Where(g => g.Type == GroupType.Public || g.Type == GroupType.MembersOnly);
        }
        if (request.Name != null) {
            groups = groups.Where(g => g.Name?.Contains(request.Name, StringComparison.InvariantCultureIgnoreCase) == true);
        }
        int skip = 0;
        if (request.PageSize != null) {
            if ((request.PageNo ?? 0) > 1) skip = (int)((request.PageNo! - 1) * request.PageSize);
            if (skip > 0) groups = groups.Skip(skip);
            groups = groups.Take((int) request.PageSize);
        }
        groups = groups.Where(g => g.GameID == gameId).OrderByDescending(g => g.Points);

        return Ok(new GetGroupsResult {
            Success = true,
            Groups = groups.Select((g, i) => {
                Schema.Group group = new Schema.Group {
                    Name = g.Name,
                    Description = g.Description,
                    GroupID = g.GroupID.ToString(),
                    OwnerID = (g.Vikings.FirstOrDefault(v => v.UserRole == UserRole.Leader)?.Viking?.Uid ?? Guid.Empty).ToString(),
                    Color = g.Color,
                    Logo = g.Logo,
                    Type = g.Type,
                    Rank = i+skip+1,
                    MemberLimit = g.MaxMemberLimit
                };
                if (request.IncludeMemberCount) group.TotalMemberCount = g.Vikings.Count;
                return group;
            }).ToArray(),
            RolePermissions = RolePermissions
        });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/GroupWebService.asmx/RemoveMember")]
    [VikingSession]
    public IActionResult RemoveMember(Viking viking, [FromForm] string removeMemberRequest) {
        RemoveMemberRequest request = XmlUtil.DeserializeXml<RemoveMemberRequest>(removeMemberRequest);
        GroupMember? vikingRole = viking.GroupMembership;
        if (vikingRole == null || vikingRole.GroupID.ToString() != request.GroupID)
            return Ok(new RemoveMemberResult { Success = false, Status = RemoveMemberStatus.Error });
        
        if (vikingRole.UserRole < UserRole.Elder)
            return Ok(new RemoveMemberResult { Success = false, Status = RemoveMemberStatus.UserHasNoPermission });
        
        GroupMember? targetRole = vikingRole.Group.Vikings.FirstOrDefault(gv => gv.Viking.Uid.ToString() == request.RemoveUserID);
        if (targetRole == null)
            return Ok(new RemoveMemberResult { Success = false, Status = RemoveMemberStatus.UserNotAMemberOfTheGroup });
        
        vikingRole.Group.Vikings.Remove(targetRole);
        if (!vikingRole.Group.Vikings.Any()) ctx.Groups.Remove(vikingRole.Group);
        ctx.SaveChanges();
        return Ok(new RemoveMemberResult { Success = true, Status = RemoveMemberStatus.Success });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/GroupWebService.asmx/AuthorizeJoinRequest")]
    [VikingSession]
    public IActionResult AuthorizeJoinRequest(Viking viking, [FromForm] string apiKey, [FromForm] string authorizeJoinRequest) {
        uint gameId = ClientVersion.GetGameID(apiKey);

        AuthorizeJoinRequest request = XmlUtil.DeserializeXml<AuthorizeJoinRequest>(authorizeJoinRequest);
        GroupMember? vikingRole = viking.GroupMembership;
        if (vikingRole == null || vikingRole.GroupID.ToString() != request.GroupID)
            return Ok(new AuthorizeJoinResult { Success = false, Status = AuthorizeJoinStatus.ApproverNotInThisGroup });
        
        if (vikingRole.UserRole < UserRole.Elder)
            return Ok(new AuthorizeJoinResult { Success = false, Status = AuthorizeJoinStatus.ApproverHasNoPermission });
        
        Viking? target = ctx.Vikings.FirstOrDefault(v => v.Uid.ToString() == request.UserID);
        if (target == null) {
            return Ok(new AuthorizeJoinResult { Success = false, Status = AuthorizeJoinStatus.Error });
        }

        GroupMember? existing = target.GroupMembership;
        if (existing != null) {
            return Ok(new AuthorizeJoinResult {
                Success = false,
                Status = existing.Group == vikingRole.Group
                    ? AuthorizeJoinStatus.UserAlreadyMemberOfTheGroup
                    : AuthorizeJoinStatus.UserHasNoJoinRequest
            });
        }

        if (vikingRole.Group.Vikings.Count >= vikingRole.Group.MaxMemberLimit)
            return Ok(new AuthorizeJoinResult { Success = false, Status = AuthorizeJoinStatus.GroupIsFull });
        
        if (request.Approved) {
            GroupMember joinee = new GroupMember {
                Viking = target,
                Group = vikingRole.Group,
                UserRole = UserRole.Member,
                JoinDate = DateTime.Now
            };
            vikingRole.Group.Vikings.Add(joinee);
            vikingRole.Group.LastActiveTime = joinee.JoinDate;
        }

        GroupJoinRequest? joinRequest = ctx.GroupJoinRequests.Find(target.Id, vikingRole.GroupID);
        if (joinRequest != null) ctx.GroupJoinRequests.Remove(joinRequest);
        ctx.SaveChanges();
        return Ok(new AuthorizeJoinResult { Success = true, Status = AuthorizeJoinStatus.Success });

    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/GroupWebService.asmx/AssignRole")]
    [VikingSession]
    public IActionResult AssignRole(Viking viking, [FromForm] string assignRoleRequest) {
        AssignRoleRequest request = XmlUtil.DeserializeXml<AssignRoleRequest>(assignRoleRequest);
        GroupMember? vikingRole = viking.GroupMembership;
        if (vikingRole == null || vikingRole.GroupID.ToString() != request.GroupID) 
            return Ok(new AssignRoleResult { Success = false, Status = AssignRoleStatus.ApproverNotMemberOfTheGroup });
        
        if (vikingRole.UserRole < UserRole.Elder) 
            return Ok(new AssignRoleResult { Success = false, Status = AssignRoleStatus.ApproverHasNoPermission });
        
        GroupMember? targetRole = vikingRole.Group.Vikings.FirstOrDefault(gv => gv.Viking.Uid.ToString() == request.MemberID);
        if (targetRole == null)
            return Ok(new AssignRoleResult { Success = false, Status = AssignRoleStatus.MemberNotPartOfTheGroup });

        if (targetRole.UserRole == request.NewRole)
            return Ok(new AssignRoleResult { Success = false, Status = AssignRoleStatus.MemberAlreadyInTheRole });
        
        if (vikingRole.UserRole == UserRole.Leader) {
            // Disallow leader from simply demoting themself.
            if (viking == targetRole.Viking)
                return Ok(new AssignRoleResult { Success = false, Status = AssignRoleStatus.ApproverHasNoPermission });
        } else if (viking != targetRole.Viking || request.NewRole > vikingRole.UserRole) {
            // Disallow Elders from promoting themselves to leader, or promoting anyone else to elder, but allow them to demote themselves.
            return Ok(new AssignRoleResult { Success = false, Status = AssignRoleStatus.ApproverHasNoPermission });
        }
            
        targetRole.UserRole = request.NewRole;
        if (request.NewRole == UserRole.Leader)
            vikingRole.UserRole = UserRole.Elder; // This is the only way a leader can demote themself.

        ctx.SaveChanges();
        return Ok(new AssignRoleResult { Success = true, Status = AssignRoleStatus.Success });
        
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("V2/GroupWebService.asmx/GetPendingJoinRequest")]
    [VikingSession]
    public IActionResult GetPendingJoinRequests(Viking viking, [FromForm] string getPendingJoinRequest) {
        GetPendingJoinRequest request = XmlUtil.DeserializeXml<GetPendingJoinRequest>(getPendingJoinRequest);
        GroupMember? vikingRole = viking.GroupMembership;
        if (vikingRole?.GroupID.ToString() == request.GroupID && vikingRole?.UserRole >= UserRole.Elder) {
            return Ok(new GetPendingJoinResult {
                Success = true,
                Requests = vikingRole.Group.JoinRequests
                    .Select(r => {
                        PendingJoinRequest req = new PendingJoinRequest {
                            UserID = r.Viking.Uid.ToString(),
                            GroupID = vikingRole.Group.GroupID.ToString(),
                            StatusID = GroupJoinRequestStatus.Pending,
                            Message = r.Message ?? "Hello! Please invite me to your Crew!" // Default from Math Blaster btw
                        };
                        req.FromUserID = req.UserID;
                        return req;
                    }).ToArray()
            });
        }
        return Ok(new GetPendingJoinResult { Success = false });
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("GroupWebService.asmx/GetGroupsByUserID")]
    public Schema.Group[] GetGroupsByUserID([FromForm] string apiKey, [FromForm] string userId) {
        uint gameId = ClientVersion.GetGameID(apiKey);

        Viking? viking = ctx.Vikings.FirstOrDefault(v => v.Uid.ToString() == userId);
        if (viking == null) return [];

        Model.Group? group = viking.GroupMembership.Group;
        return [
            new Schema.Group {
                GroupID = group.GroupID.ToString(),
                Name = group.Name,
                Color = group.Color,
                Logo = group.Logo,
                MemberLimit = group.MaxMemberLimit
            }
        ];
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("GroupWebService.asmx/GetGroupsByGroupType")]
    public Schema.Group[] GetGroupsByGroupType([FromForm] string apiKey, [FromForm] string groupType) {
        uint gameId = ClientVersion.GetGameID(apiKey);
        
        List<Schema.Group> groups = new List<Schema.Group>();
        foreach (Model.Group group in ctx.Groups) {
            if (group.GameID == gameId && group.Type.ToString() == groupType) groups.Add(new Schema.Group {
                GroupID = group.GroupID.ToString(),
                Name = group.Name,
                Color = group.Color,
                Logo = group.Logo,
                Type = group.Type,
                MemberLimit = group.MaxMemberLimit
            });
        }
        return groups.ToArray();
    }

    [HttpPost]
    [Produces("application/xml")]
    [Route("GroupWebService.asmx/GetMembersByGroupID")]
    public Schema.GroupMember[] GetMembersByGroupID([FromForm] string groupID) {
        Guid parsedGroupID = Guid.Parse(groupID);
        Model.Group? group = ctx.Groups.FirstOrDefault(
            g => g.GroupID == parsedGroupID
        );
        if (group == null) return [];

        return group.Vikings.Select(v => new Schema.GroupMember {
            DisplayName = group.GameID == ClientVersion.MB && v.Viking.AvatarSerialized != null
                ? XmlUtil.DeserializeXml<AvatarData>(v.Viking.AvatarSerialized).DisplayName
                : v.Viking.Name,
            UserID = v.Viking.Uid.ToString(),
            JoinDate = v.JoinDate,
            RoleID = (int) v.UserRole,
            Online = false, // There's no way to check this.
            Rank = 0,
            GroupID = group.GroupID.ToString(),
            Points = group.Points
        }).ToArray();
    }
}
