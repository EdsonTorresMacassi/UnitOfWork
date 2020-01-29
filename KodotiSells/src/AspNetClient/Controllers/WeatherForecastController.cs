using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace AspNetClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public WeatherForecastController(IInvoiceService invoiceService)
        {
            this._invoiceService = invoiceService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(
                _invoiceService.GetAll()
            );
        }
    }
}
