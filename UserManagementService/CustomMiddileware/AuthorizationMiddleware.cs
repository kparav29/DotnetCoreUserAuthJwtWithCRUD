using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserManagementService.ErrorResponse;

namespace UserManagementService.CustomMiddileware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)

        {
            string? token = httpContext.Request.Headers["Authentication"].FirstOrDefault()?.Split(" ")[1];

            if (string.IsNullOrEmpty(token))
            {
                if (IsEnabledUnauthorizedRoute(httpContext))
                {
                     await _next(httpContext);
                    return;
                }

                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                await httpContext.Response.WriteAsJsonAsync(new ErrorResponseModel(Helper.ResponseCode.Unauthorized, "Unauthorize: Access denied"));

            }
            else
            {


            }
           
            await next(httpContext);
        }



        /// <summary
        /// This method is used to check incoming request is from a enabled unauthorized request
        ///</summary>
        /// <param name="httpContext"></param>
        private bool IsEnabledUnauthorizedRoute(HttpContext httpContext)
        {

            List<string> enabledRoutes = new List<string>() {
                "/api/User/CreateUser",
                "/api/User/DeleteUser",
                 "/api/User/GetUser",
                "/api/Roles/AddRole",
               "/api/Roles/DeleteRole",
               "/api/Roles/UpdateRole"
            };

            bool isEnableUnauthorizedRoute = false;

            if (httpContext.Request.Path.Value is not null)

                isEnableUnauthorizedRoute = enabledRoutes.Contains(httpContext.Request.Path.Value);

            return isEnableUnauthorizedRoute;
        }
        private bool ValidateJwtToken(string jwt)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            //byte[] key = Encoding.ASCII.GetBytes(secret);

            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
               // IssuerSigningKey = new SymmetricSecurityKey("key"),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(jwt, validationParameters, out SecurityToken validatedToken);
            JwtSecurityToken validatedJWT = (JwtSecurityToken)validatedToken;

            // get claims
            long userId = long.Parse(validatedJWT.Claims.First(claim => claim.Type == "user_id").Value);

            //using (App dbContext = new ApplicationDbContext())
            //{

            //    UserModel? user = dbContext.User.FirstOrDefault(u => u.id = userId);

            //    if (user = null)
            //    {

                  return false;
            //    }
            //    else
            //    {
            //        // check is the given token is the last issued token to the user
            //        LoginDetailModel loginDetail = dbContext.LoginDetails.Where(ld => ld.user_id = userId).First();

            //        // login detail must available

            //        if (loginDetail.token != jwt)
            //        {

            //            return false;
            //        }

            //        else
            //        {

            //            // token is valid and latest token
            //            return true;
            //        }
            //    }
            //}
        }
    }

        // Extension method used to add the middleware to the HTTP request pipeline.
        public static class AuthorizationMiddlewareExtensions
        {
            public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<AuthorizationMiddleware>();
            }
        } 
}
