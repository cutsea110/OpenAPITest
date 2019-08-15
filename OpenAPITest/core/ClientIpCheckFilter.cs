﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OpenAPITest.CustomFilter
{
    internal class ClientIpCheckFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;
        private AccessControl _acl;

        public ClientIpCheckFilter
            (ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger(nameof(ClientIpCheckFilter));
            // Startupのあとで呼ばれるので??以降は不要ではある.
            _acl = AppConfiguration.AccessControl ?? new AccessControl(configuration);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation(
                $"Remote IpAddress: {context.HttpContext.Connection.RemoteIpAddress}");

            var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
            _logger.LogDebug($"Request from Remote IP address: {remoteIp}");

            if (_acl.Allow(remoteIp))
            {
                _logger.LogInformation($"Allow Request from Remote IP address: {remoteIp}");
                base.OnActionExecuting(context);
            }
            else
            {
                _logger.LogInformation($"Forbidden Request from Remote IP address: {remoteIp}");
                context.Result = new StatusCodeResult(401);
                return;
            }
        }
    }
}
