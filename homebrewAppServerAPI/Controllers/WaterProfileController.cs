using AutoMapper;
using homebrewAppServerAPI.Domain.Models;
using homebrewAppServerAPI.Domain.Services;
using homebrewAppServerAPI.Helpers;
using homebrewAppServerAPI.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace homebrewAppServerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors(origins: "http://localhost:3000/", headers: "*", methods: "*")]
    public class WaterProfileController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IWaterProfileService _waterProfileService;
        private readonly IMapper _mapper;

        public WaterProfileController(IWaterProfileService waterProfileService, IMapper mapper)
        {
            log.Debug($"Called {Helper.GetCurrentMethod()}");

            _waterProfileService = waterProfileService;
            _mapper = mapper;
        }

        // GET: WaterProfile/Summary
        [HttpGet]
        [Route("Summary")]
        public async Task<IEnumerable<WaterProfileResource>> GetAllWaterProfilesAsync()
        {
            log.Debug($"Called {Helper.GetCurrentMethod()}");

            var waterProfiles = await _waterProfileService.ListAsync();
            var resources = _mapper.Map<IEnumerable<WaterProfile>, IEnumerable<WaterProfileResource>>(waterProfiles);
            return resources;
        }
    }
}
