#pragma checksum "C:\Users\Keerthi Raja P\source\repos\EmployeeManage\Project\WebApp\Areas\Security\Views\UserAccount\Register.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1795cfc16b8895a4f6d477a71a83ded6b56b3b64"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Security_Views_UserAccount_Register), @"mvc.1.0.view", @"/Areas/Security/Views/UserAccount/Register.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Security/Views/UserAccount/Register.cshtml", typeof(AspNetCore.Areas_Security_Views_UserAccount_Register))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1795cfc16b8895a4f6d477a71a83ded6b56b3b64", @"/Areas/Security/Views/UserAccount/Register.cshtml")]
    public class Areas_Security_Views_UserAccount_Register : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "C:\Users\Keerthi Raja P\source\repos\EmployeeManage\Project\WebApp\Areas\Security\Views\UserAccount\Register.cshtml"
  
    ViewData["Title"] = "View";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(89, 290, true);
            WriteLiteral(@"
<h2>View</h2>


<form asp-area="" UserAccount"" asp-controller="" UserAccount"" asp-action=""Login"" method=""post"">
    <input type=""text"" name=""username"" id=""username"" />
    <input type=""password"" name=""password"" id=""password"" />
    <input type=""submit"" value=""Login"" />
</form>


");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
