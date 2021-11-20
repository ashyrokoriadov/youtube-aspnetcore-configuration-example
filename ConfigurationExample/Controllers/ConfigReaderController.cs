using ConfigurationExample.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigReaderController : ControllerBase
    {
        private readonly ILogger<ConfigReaderController> _logger;
        private readonly IConfiguration _configuration;
        private readonly PollyOptions _pollyOptions;

        public ConfigReaderController(
            ILogger<ConfigReaderController> logger,
            IConfiguration configuration,
            IOptions<PollyOptions> pollyOptionsProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _pollyOptions = pollyOptionsProvider.Value;
        }

        [HttpGet]
        public IActionResult GetConfiguration()
        {
            var deliveryApi = _configuration["DeliveryServiceApi"];
            var retriesNumber = _configuration["Polly:RetriesNumber"];
            var interval = _configuration["Polly:IntervalBetweenRetries"];

            var configuration = $"deliveryApi: {deliveryApi} \n" +
                           $"retriesNumber: {retriesNumber} \n" +
                           $"interval: {interval}";

            return Ok(configuration);
        }

        [HttpGet("BindOptions")]
        public IActionResult GetConfigurationWithBindOptions()
        {
            var pollyOptions = new PollyOptions();
            _configuration.GetSection(PollyOptions.SectionName).Bind(pollyOptions);

            var configuration = $"retriesNumber: {pollyOptions.RetriesNumber} \n" +
                           $"interval: {pollyOptions.IntervalBetweenRetries}";

            return Ok(configuration);
        }

        [HttpGet("GetOptions")]
        public IActionResult GetConfigurationWithGetOptions()
        {
            var pollyOptions = _configuration.GetSection(PollyOptions.SectionName).Get<PollyOptions>();

            var configuration = $"retriesNumber: {pollyOptions.RetriesNumber} \n" +
                           $"interval: {pollyOptions.IntervalBetweenRetries}";

            return Ok(configuration);
        }

        [HttpGet("OptionsFromRegistartion")]
        public IActionResult GetOptionsFromRegistartion()
        {
            var configuration = $"retriesNumber: {_pollyOptions.RetriesNumber} \n" +
                           $"interval: {_pollyOptions.IntervalBetweenRetries}";

            return Ok(configuration);
        }

        [HttpGet("AllOptions")]
        public IActionResult GetAllOptions()
        {
            var sb = new StringBuilder();

            foreach (var c in _configuration.AsEnumerable())
            {
                sb.AppendLine(c.Key + " = " + c.Value);
            }

            return Ok(sb.ToString());
        }
    }
}
