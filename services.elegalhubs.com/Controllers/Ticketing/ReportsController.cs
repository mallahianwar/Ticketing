using library.elegalhubs.com.lms.Admin.Reports;
using library.elegalhubs.com.Ticketing;
using Microsoft.AspNetCore.Mvc;
using services.elegalhubs.com.Model;
using services.elegalhubs.com.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace services.elegalhubs.com.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        private readonly IndexReport _reportR;
        private readonly ReportsRepository _report;

        public ReportsController(IndexReport context,
            ReportsRepository report)
        {
            _reportR = context;
            _report = report;
          
        }

        [HttpGet("IndexReport")]
        public async Task<IndexReport> Get()
        {
            return await _report.IndexReport();
        }

   
    }
}
