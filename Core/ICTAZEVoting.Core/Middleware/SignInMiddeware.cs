using ICTAZEvoting.Shared.Requests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using System.Collections.Concurrent;

namespace ICTAZEVoting.Core.Middleware
{
    public class SignInMiddleware<TUser> where TUser : class
    {
        readonly RequestDelegate next;
        readonly ILogger<SignInMiddleware<TUser>> logger;
        static IDictionary<Guid, TokenRequest> Logins { get; set; }
               = new ConcurrentDictionary<Guid, TokenRequest>();
        public static Guid AnnounceLogin(TokenRequest request)
        {
            request.LoginStarted = DateTime.Now;
            var key = Guid.NewGuid();
            Logins.TryAdd(key, request);
            return key;
        }
        public static TokenRequest GetLoginInProgress(Guid key)
        {
            if (Logins.ContainsKey(key))
            {
                return Logins[key];
            }
            return new TokenRequest();
        }
        public static TokenRequest GetLoginInProgress(string key)
        {
            return GetLoginInProgress(Guid.Parse(key));
        }
        public SignInMiddleware(RequestDelegate requestDelegate, ILogger<SignInMiddleware<TUser>> logger)
        {
            next = requestDelegate;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context, SignInManager<TUser> signInManager)
        {
            logger.LogInformation("Started Listening..");
            if (context.Request.Path == "/login/" && context.Request.Query.ContainsKey("key"))
            {
                var key = Guid.Parse(context.Request.Query["key"]);
                var tokenRequest = Logins[key];
                var result = await signInManager.PasswordSignInAsync(tokenRequest.UserName, tokenRequest.Password, tokenRequest.RememberMe, false);
                if (result.Succeeded)
                {
                    Logins.Remove(key);
                    logger.LogInformation("User logged in successifully..");
                    context.Response.Redirect(tokenRequest.ReturnUrl);
                    return;
                }
                else if (result.RequiresTwoFactor)
                {
                    context.Response.Redirect("/loginWith2fa/" + key);
                    return;
                }
                else if (result.IsLockedOut)
                {
                    return;
                }
                else
                {

                    await next.Invoke(context);
                    return;
                }
            }
            else if (context.Request.Path.StartsWithSegments("/loginWith2fa"))
            {
                var key = Guid.Parse(context.Request.Path.Value.Split('/').Last());
                var tokenRequest = Logins[key];
                if (string.IsNullOrEmpty(tokenRequest.TwoFactorCode))
                {
                    //user login 2fa for the first time
                    var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
                  
                }
                else
                {
                    var result = await signInManager.TwoFactorAuthenticatorSignInAsync(tokenRequest.TwoFactorCode, tokenRequest.RememberMe, tokenRequest.RemberMachine);
                    if (result.Succeeded)
                    {
                        Logins.Remove(key);
                        context.Response.Redirect(tokenRequest.ReturnUrl);
                        return;
                    }
                    else if (result.IsLockedOut)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if (context.Request.Path.StartsWithSegments("/logout"))
            {
                await signInManager.SignOutAsync();
                context.Response.Redirect("/");
                return;
            }
            //We get here? then something went wrong
            //continue the http middleware chain

            await next.Invoke(context);
        }
    }
}
