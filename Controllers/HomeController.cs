using ISAM4332.Assignment05.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ISAM4332.Assignment05.Controllers
{
    public class HomeController : Controller
    {
        // Used to track response messages between redirects
        protected List<ResponseMessage> Messages { get; set; } = new List<ResponseMessage>();

        public IActionResult Index()
        {
            IEnumerable<MortgageApplication> applications = Repository.Get();

            return View(applications);
        }

        public IActionResult New()
        {
            MortgageApplication application = new MortgageApplication();

            return View(application);
        }

        [HttpPost]
        public IActionResult Submit(MortgageApplication application)
        {
            if (application.ApplicationId == Guid.Empty)
            {
                application.ApplicationId = Guid.NewGuid();
            }
            bool success = Repository.Add(application);
            if (success)
            {
                Messages.Add(new ResponseMessage(ResponseMessageType.Success, $"Successfully submitted mortage application. Your application id is {application.ApplicationId}"));
            }
            else
            {
                Messages.Add(new ResponseMessage(ResponseMessageType.Error, $"Failed to submit mortage application."));
                return View("New", application);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(Guid id)
        {
            MortgageApplication application = Repository.Get(id);

            if (application == null)
            {
                return RedirectToAction("Index");
            }

            return View(application);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /*
		 * This method detects any messages that have been passed through the TempData scope
		 * and re-adds it to the controller before the next action executes.
		 */
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            IEnumerable<ResponseMessage> _messages = TempData.Get<IEnumerable<ResponseMessage>>("Messages");
            if (_messages != null)
            {
                // Attempt to cleanup any duplicate messages after redirects
                this.Messages = _messages.Select(k => new ResponseMessage()
                {
                    MessageType = k.MessageType,
                    Header = k.Header,
                    Message = k.Message
                }).Distinct().ToList();
                TempData["Messages"] = null;
            }
            else
            {
                if (this.Messages == null)
                {
                    this.Messages = new List<ResponseMessage>();
                }
            }
        }

        /*
		 * This method detects a redirect and automatically adds the messages to the TempData scope.
		 */
        public override void OnActionExecuted(ActionExecutedContext ctx)
        {
            try
            {
                base.OnActionExecuted(ctx);
                ViewBag.Messages = this.Messages ?? new List<ResponseMessage>();
                var isRedirect = ctx.Result.GetType().Name.Contains("Redirect");
                if (isRedirect)
                {
                    TempData.Set("Messages", Messages ?? new List<ResponseMessage>());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}