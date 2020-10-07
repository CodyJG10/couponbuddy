using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CouponBuddy.Entities;
using CouponBuddy.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UptimeSharp;

namespace CouponBuddy.WebCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UptimeController : ControllerBase
    {
        private ApplicationDbContext _context;

        public UptimeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPut("UpdateUptime")]
        public async Task<IActionResult> UpdateUptime(UptimeReport report)
        {
            if (_context.UptimeReports.AsNoTracking().Any(x => x.DeviceId == report.DeviceId 
                                             && x.LocationName == report.LocationName))
            {
                int id = _context.UptimeReports.AsNoTracking().Single(x => x.DeviceId == report.DeviceId
                                             && x.LocationName == report.LocationName).Id;
                report.Id = id;
                _context.Update(report);
            }
            else
            {
                //Add moniter
                string apiKey = "u1046033-1b07e2adadcde58618f59cab";
                UptimeClient _client = new UptimeClient(apiKey);
                string friendlyName = report.LocationName + " " + report.DeviceId;
                string target = "https://couponbuddy.azurewebsites.net/api/uptime/" +
                    "status?deviceId=" + report.DeviceId + "&locationName=" + report.LocationName.Replace(" ", "%20");
                await _client.AddMonitor(friendlyName, target, UptimeSharp.Models.Type.HTTP);
                _context.UptimeReports.Add(report);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("Status")]
        public IActionResult GetStatus(int deviceId, string locationName)
        {
            if (_context.UptimeReports.Any(x => x.DeviceId == deviceId
                                             && x.LocationName == locationName))
            {
                var report = _context.UptimeReports.Single(x => x.DeviceId == deviceId
                                                             && x.LocationName == locationName);
                if (report.Active)
                    return Ok();
                else
                    return BadRequest();
            }
            else
                return BadRequest();
        }
    }
}