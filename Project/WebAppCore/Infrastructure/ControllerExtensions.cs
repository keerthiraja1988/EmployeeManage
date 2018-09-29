using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAppCore.Infrastructure
{
    public static class ControllerExtensions
    {

        public async static Task<(int UserId, string UserName, string Email, List<string> UserRoles, DateTime LoggedInTime)>
            GetLoggedInUserDetails(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var userId = Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userName = principal.Claims.Where(w => w.Value == "UserName").FirstOrDefault().ValueType;
            var email = principal.FindFirst(ClaimTypes.Email)?.Value.ToString();
            var loggedInTime = Convert.ToDateTime(
                            principal.Claims.Where(w => w.Value == "LoggedInTime")
                            .FirstOrDefault().ValueType);
            var userRoles = principal.FindAll(ClaimTypes.Role).Select(s => s.Value).ToList();
            return (userId, userName, email, userRoles, loggedInTime);
        }

        public static async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool partial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
