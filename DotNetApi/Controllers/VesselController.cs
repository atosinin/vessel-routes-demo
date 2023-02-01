using DotNetApi.Models;
using DotNetApi.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotNetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VesselController : ControllerBase
    {
        private readonly IVesselService _vesselServices;

        public VesselController(
            IVesselService vesselServices)
        {
            _vesselServices = vesselServices;
        }

        // GET /api/Vessel
        [HttpGet]
        public IActionResult GetAll()
        {
            List<VesselDTO> allVesselDTOs = _vesselServices.GetAllVesselDTOs();
            return Ok(allVesselDTOs);
        }

        // GET /api/Vessel/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            VesselDTO vesselDTO = _vesselServices.GetVesselDTOById(id);
            return Ok(vesselDTO);
        }

        // POST /api/Vessel
        [HttpPost]
        public IActionResult Create([FromBody] VesselDTO vesselCreate)
        {
            _vesselServices.CreateFromVesselDTO(vesselCreate);
            return Ok();
        }

        // PUT /api/Vessel
        [HttpPut]
        public IActionResult Update([FromBody] VesselDTO vesselUpdate)
        {
            _vesselServices.UpdateFromVesselDTO(vesselUpdate);
            return Ok();
        }

        // DELETE /api/Vessel/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _vesselServices.DeleteVesselById(id);
            return Ok();
        }

        // POST /api/Vessel/Import
        [HttpPost("Import")]
        public IActionResult Import([FromBody] string importString)
        {
            VesselRoutesImport? vessels = JsonConvert.DeserializeObject<VesselRoutesImport>(importString);
            if (vessels is not null)
                _vesselServices.Import(vessels);
            return Ok();
        }
    }
}