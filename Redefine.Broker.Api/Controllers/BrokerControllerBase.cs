using Microsoft.AspNetCore.Mvc;

namespace Redefine.Broker.Api.Controllers
{
    public abstract class BrokerControllerBase<T> : ControllerBase
    {
        protected readonly ILogger<T> _logger;
        public BrokerControllerBase(ILogger<T> logger)
        {
            _logger = logger;
        }

        protected async Task<IActionResult> UnpackExceptionAsync(Exception ex)
        {
            var message = $"Exception in {GetType().Name}";
            _logger.LogError(message);
            return await Task.FromResult(BadRequest(message));
        }

        protected bool IsHttpSuccess(System.Net.HttpStatusCode? status) => status != null && (int)status >= 200 && (int)status < 300;
    }
}
