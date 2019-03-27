using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace EmployeeWebAPI.App_Start
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;
            HttpException httpException = exception as HttpException;
            logger.Error(Environment.NewLine + exception.Message, exception);
            logger.Error(Environment.NewLine + "--------------------------------------------------------------------------------" + Environment.NewLine);
                
            HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An unhandled exception was thrown by Customer Web API controller."),
                ReasonPhrase = "An unhandled exception was thrown by Customer Web API controller."
            };
            context.Response = msg;
        }
    }
}