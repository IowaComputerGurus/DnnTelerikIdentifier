using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using DotNetNuke.Entities.Users;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using IowaComputerGurus.Dnn.TelerikIdentifier.Models;

namespace IowaComputerGurus.Dnn.TelerikIdentifier.Controllers
{
    public class ContentController : DnnController
    {
        public ActionResult Index()
        {
            var model = new TelerikAnalysisModel();
            var currentUser = UserController.Instance.GetCurrentUserInfo();
            if (!currentUser.IsSuperUser)
            {
                model.AnalysisComplete = false;
                model.ErrorMessage = "You must be a host user to use this module.";
                return View(model);
            }

            model.AnalysisComplete = true;
            //Do the analysis
            var foundAssemblies = new List<string>();
            var files = Directory.GetFiles(Server.MapPath("~/bin"), "*.dll", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                try
                {
                    var references = Assembly.Load(System.IO.File.ReadAllBytes(file)).GetReferencedAssemblies();
                    var foundTelerik =
                        references.Any(r => r.FullName.StartsWith("Telerik", true, CultureInfo.InvariantCulture));
                    if (!foundTelerik)
                        continue;

                    foundAssemblies.Add(Path.GetFileName(file));
                }
                catch (Exception ex)
                {
                    model.AssemblyAnalysisErrors.Add(new AssemblyAnalysisError
                        {AssemblyName = Path.GetFileName(file), ErrorMessage = ex.Message});
                }
            }

            //Filter out the known ones
            if (foundAssemblies.Contains("Telerik.Web.UI.Skins.dll"))
            {
                foundAssemblies.Remove("Telerik.Web.UI.Skins.dll");
                model.ExpectedAssemblies.Add("Telerik.Web.UI.Skins.dll");
            }
            if (foundAssemblies.Contains("DotNetNuke.Website.Deprecated.dll"))
            {
                foundAssemblies.Remove("DotNetNuke.Website.Deprecated.dll");
                model.ExpectedAssemblies.Add("DotNetNuke.Website.Deprecated.dll");
            }
            if (foundAssemblies.Contains("DotNetNuke.Web.Deprecated.dll"))
            {
                foundAssemblies.Remove("DotNetNuke.Web.Deprecated.dll");
                model.ExpectedAssemblies.Add("DotNetNuke.Web.Deprecated.dll");
            }
            if (foundAssemblies.Contains("DotNetNuke.Modules.DigitalAssets.dll"))
            {
                foundAssemblies.Remove("DotNetNuke.Modules.DigitalAssets.dll");
                model.ExpectedAssemblies.Add("DotNetNuke.Modules.DigitalAssets.dll");
            }

            if (foundAssemblies.Count > 0)
                model.UnexpectedAssemblies.AddRange(foundAssemblies);

            return View(model);
        }
    }
}