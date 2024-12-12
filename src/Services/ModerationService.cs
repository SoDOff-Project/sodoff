using System;
using sodoff.Model;
using sodoff.Schema;

namespace sodoff.Services;

public class ModerationService
{
    private readonly DBContext ctx;

    public ModerationService(DBContext ctx)
    {
        this.ctx = ctx;
    }

    public UserBan AddBanToUser(User user, UserBanType banType, DateTime dateEnd = new DateTime())
    {
        // create a ban in relation to the specified user
        UserBan userBan = new UserBan
        {
            BanType = banType,
            CreatedAt = DateTime.UtcNow
        };

        // if suspension is indefinite, set end date to infinity
        if(banType == UserBanType.IndefiniteSuspension || banType == UserBanType.IndefiniteMPSuspension || banType == UserBanType.IndefiniteTCSuspension) userBan.EndsAt = new DateTime(9999, 01, 01);
        else userBan.EndsAt = dateEnd;

        // add ban to user ban list
        user.Bans.Add(userBan);

        // update database
        ctx.SaveChanges();

        // return ban
        return userBan;
    }

    public bool RemoveBanFromUser(User user, UserBan ban)
    {
        // remove ban from database if it exists on the user
        if(user.Bans.FirstOrDefault(e => e == ban) != null) { user.Bans.Remove(ban); ctx.SaveChanges(); return true; }
        else return false;
    }

    public bool RemoveAllBansFromUser(User user)
    {
        // remove all bans from user if user has any
        if(user.Bans.Count >= 0) { foreach(var ban in user.Bans) user.Bans.Remove(ban); ctx.SaveChanges(); return true; }
        else return false;
    }

    public UserBan GetLatestBanFromUser(User user)
    {
        // retreive the most recently created ban from user
        UserBan? userBan = user.Bans.OrderByDescending(e => e.CreatedAt).FirstOrDefault();

        if(userBan != null) return userBan;
        else return null!; // return null if the user has no bans on record
    }

    public ICollection<UserBan> GetAllBansFromUser(User user, bool descendingOrder = false)
    {
        if(descendingOrder) return user.Bans.OrderByDescending(e => e.CreatedAt).ToList();
        else return user.Bans.OrderBy(e => e.CreatedAt).ToList(); // return sorted list by created date
    }
}
