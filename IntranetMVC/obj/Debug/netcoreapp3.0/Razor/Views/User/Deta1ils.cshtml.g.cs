#pragma checksum "C:\Users\sistemas10.ATP\source\repos\IntranetMVC\IntranetMVC\Views\User\Deta1ils.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d6466c900b0ca991dbed38ec28c83b035c566914"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_User_Deta1ils), @"mvc.1.0.view", @"/Views/User/Deta1ils.cshtml")]
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
#nullable restore
#line 1 "C:\Users\sistemas10.ATP\source\repos\IntranetMVC\IntranetMVC\Views\_ViewImports.cshtml"
using IntranetMVC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\sistemas10.ATP\source\repos\IntranetMVC\IntranetMVC\Views\_ViewImports.cshtml"
using IntranetMVC.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d6466c900b0ca991dbed38ec28c83b035c566914", @"/Views/User/Deta1ils.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6abd35b21bd3a269d140767b639faec55410d89c", @"/Views/_ViewImports.cshtml")]
    public class Views_User_Deta1ils : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<PermisosViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#nullable restore
#line 4 "C:\Users\sistemas10.ATP\source\repos\IntranetMVC\IntranetMVC\Views\User\Deta1ils.cshtml"
  
    ViewData["Title"] = "Details";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

<h1>Permisos del usuario : </h1>
<!--caaard-->
<!-- Card -->
<div class=""card testimonial-card"">

    <!-- Background color -->

    <div class=""card-header"" style=""background-color:darkblue"">
        <h3></h3>
    </div>
    <div class=""card-body"">

        <div class=""row"">
            <div class=""col-md-5"">
                <div class="" tiles white col-md-12 no-padding""");
            BeginWriteAttribute("style", " style=\"", 465, "\"", 473, 0);
            EndWriteAttribute();
            WriteLiteral(@">
                    <div class=""tiles white"">
                        <div class=""row"">
                            <div class=""col-md-3 col-sm-3"">
                                <div class=""user-profile-pic"">
                                   
                                </div>
                                <br><br>
                            </div>
                            <div class=""col-md-8 user-description-box  col-sm-5"">
");
#nullable restore
#line 33 "C:\Users\sistemas10.ATP\source\repos\IntranetMVC\IntranetMVC\Views\User\Deta1ils.cshtml"
                                 foreach (var a in Model.Informacion)
                                 {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <h4 class=\"semi-bold no-margin\" id=\"NomUsuario\"> ");
#nullable restore
#line 35 "C:\Users\sistemas10.ATP\source\repos\IntranetMVC\IntranetMVC\Views\User\Deta1ils.cshtml"
                                                                                Write(a.Nombre);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h4>
                                    <h6 class=""no-margin"" id=""PuestoUsu""> Puesto</h6>
                                    <br>
                                    <p id=""DepUsuario""><i class=""fa fa-briefcase""></i> Departamento</p>
                                    <p id=""CorreoUsu""><i class=""fa fa-envelope""></i> Correo</p>
                                    <p id=""ExtUsuario""><i class=""fa fa-phone""></i> Extension</p>
                                    <div class=""form-check"">
                                        <input class=""form-check-input"" type=""checkbox""");
            BeginWriteAttribute("value", " value=\"", 1716, "\"", 1724, 0);
            EndWriteAttribute();
            WriteLiteral(@" id=""defaultCheck1"">
                                        <label class=""form-check-label"" for=""defaultCheck1"">
                                            ¿Es Gerente?
                                        </label>
                                    </div>
");
            WriteLiteral("                                    <i class=\"fa fa-users\" aria-hidden=\"true\"> </i> \r\n");
#nullable restore
#line 49 "C:\Users\sistemas10.ATP\source\repos\IntranetMVC\IntranetMVC\Views\User\Deta1ils.cshtml"
                                    
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n\r\n\r\n\r\n            </div>\r\n\r\n        </div>\r\n    </div>\r\n        <!-- Card -->\r\n </div>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<PermisosViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
