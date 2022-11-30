using System.Net;
using System.Text.Json;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ClearBank.DeveloperTest.API
{
    public class PaymentApi
    {
        private readonly ILogger _logger;
        private readonly IPaymentService _paymentService;

        public PaymentApi(
            IPaymentService paymentService,
            ILoggerFactory loggerFactory
            )
        {
            _logger = loggerFactory.CreateLogger<PaymentApi>();
            _paymentService = paymentService; ;
        }

        [Function("http-trigger-create-payment")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "payments")]
            HttpRequestData req
            )
        {
            try
            {
                var json = await req.ReadAsStringAsync();

                if (string.IsNullOrEmpty(json))
                {
                   return req.CreateResponse(HttpStatusCode.BadRequest);
                }

                _logger
                    .LogTrace(
                    "Make Payment Request",
                    new Dictionary<string, string> { { "Make Payment Request", json } }
                    );

                var paymentRequest = JsonSerializer.Deserialize<MakePaymentRequest>(json);

                var result = _paymentService.MakePayment(paymentRequest);

                return result.Success ?
                    req.CreateResponse(HttpStatusCode.OK) :
                    req.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    message: ex?.Message,
                    args: ex
                    );
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
