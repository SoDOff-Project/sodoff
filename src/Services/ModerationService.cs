using System;
using sodoff.Model;
using sodoff.Schema;

namespace sodoff.Services;

public class ModerationService
{
    private readonly DBContext ctx;
    private readonly MMOCommunicationService mmoCommService;

    public ModerationService(DBContext ctx, MMOCommunicationService mmoCommService)
    {
        this.ctx = ctx;
        this.mmoCommService = mmoCommService;
    }

    // Banning
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

    // Reporting

    public List<Report> GetAllReportsReceivedFromViking(Viking viking, int typeFilter = 0)
    {
        // get all reports the viking has received by 'CreatedAt' descending
        List<Report> reports = new List<Report>();
        if (typeFilter != 0) reports = viking.ReportsReceived.Where(e => e.ReportType == typeFilter).OrderByDescending(e => e.CreatedAt).ToList();
        else reports = viking.ReportsReceived.OrderByDescending(e => e.CreatedAt).ToList();

        return reports;
    }

    public List<Report> GetAllReportsMadeFromViking(Viking viking, int typeFilter = 0)
    {
        // get all reports the viking has made by 'CreatedAt' descending
        List<Report> reports = new List<Report>();
        if (typeFilter != 0) reports = viking.ReportsMade.Where(e => e.ReportType == typeFilter).OrderByDescending(e => e.CreatedAt).ToList();
        else reports = viking.ReportsMade.OrderByDescending(e => e.CreatedAt).ToList();

        return reports;
    }

    public Report AddReportToViking(string apiToken, Viking viking, Viking vikingToReport, ReportType reportReason)
    {
        // check if the report already exists with the viking creating the report
        Report? existingReport = viking.ReportsMade.FirstOrDefault(e => e.ReportedVikingId == vikingToReport.Id);
        if (existingReport != null && existingReport.ReportType == (int)reportReason)
        {
            return null!;
        }

        // make report on offending user
        Report report = new Report
        {
            ReportType = (int)reportReason,
            CreatedAt = DateTime.UtcNow
        };

        // add report to "ReportsMade" on owner and "ReportsReceived" on offender, EF should do the rest
        viking.ReportsMade.Add(report);
        vikingToReport.ReportsReceived.Add(report);
        ctx.SaveChanges();

        // send a moderation message to the offender (they will receive it if they are online, later on a message will be added to their message board instead)
        mmoCommService.SendPacketToPlayer(apiToken, vikingToReport.Uid.ToString(), "SMM", new string[] { "SMM", "-1", "REPORT_FILED", "Oops! Looks like you may have done something wrong! Repeated offences will result in an account ban." });

        return report;
    }

    public bool RemoveReportFromViking(Viking viking, ReportType reportType)
    {
        // find the report of that type on that viking
        Report? report = viking.ReportsReceived.FirstOrDefault(e => e.ReportType == (int)reportType);
        if (report != null)
        {
            // remove it
            return viking.ReportsReceived.Remove(report); // this should also remove it from the 'ReportsMade' list on the owner
        } else return false;
    }
}
