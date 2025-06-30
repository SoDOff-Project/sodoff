using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sodoff.Model;

namespace sodoff.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class VikingSession : Attribute, IAsyncActionFilter {
    public enum Modes { VIKING, USER, VIKING_OR_USER };

    public string ApiToken { get; set; } = "apiToken";
    public Modes Mode { get; set; } = Modes.VIKING;
    public bool  UseLock = false;

    private static Dictionary<string, SemaphoreSlim> semaphores = new();
    private static SemaphoreSlim dictSemaphore = new(1, 1);

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
        DBContext ctx = context.HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;

        // get session from apiToken

        if (!context.HttpContext.Request.Form.ContainsKey(ApiToken)) {
            context.Result = new UnauthorizedObjectResult("Unauthorized") { StatusCode = 403 };
            return;
        }

        Session? session = ctx.Sessions.FirstOrDefault(x => x.ApiToken == Guid.Parse(context.HttpContext.Request.Form[ApiToken].ToString()));

        // get viking / user id from session

        string? userVikingId = null;
        if (Mode == Modes.VIKING || (Mode == Modes.VIKING_OR_USER && session?.UserId is null) ) {
            userVikingId = session?.VikingId?.ToString();
        } else {
            userVikingId = session?.UserId?.ToString();
        }

        if (userVikingId is null) {
            context.Result = new UnauthorizedObjectResult("Unauthorized") { StatusCode = 403 };
            return;
        }

        // call next (with lock if requested)

        if (UseLock) {
            // NOTE: we can't refer to session.User / session.Viking here,
            //       because it may cause to ignore modifications from the threads we are waiting for
            //       we can use its only after vikingMutex.WaitOne()
            string semKey = "SoDOffViking:" + userVikingId;
            SemaphoreSlim semaphore = await GetSemaphore(semKey);
            try {
                await semaphore.WaitAsync();
                context.ActionArguments["user"] = session.User;
                context.ActionArguments["viking"] = session.Viking;
                await next();
            } finally {
                semaphore.Release();
                await RemoveSemaphore(semKey, semaphore);
            }
        } else {
            context.ActionArguments["user"] = session.User;
            context.ActionArguments["viking"] = session.Viking;
            await next();
        }
    }

    private static async Task<SemaphoreSlim> GetSemaphore(string key) {
        await dictSemaphore.WaitAsync();
        if (!semaphores.TryGetValue(key, out SemaphoreSlim semaphore)) {
            semaphore = new SemaphoreSlim(1, 1);
            semaphores.Add(key, semaphore);
        }
        dictSemaphore.Release();
        return semaphore;
    }

    private static async Task RemoveSemaphore(string key, SemaphoreSlim sem) {
        await dictSemaphore.WaitAsync();
        if (sem.CurrentCount == 1) {
            semaphores.Remove(key);
        }
        dictSemaphore.Release();
    }
}
