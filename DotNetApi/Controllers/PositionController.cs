using DotNetApi.Models;
using DotNetApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionServices;

        public PositionController(
            IPositionService positionServices)
        {
            _positionServices = positionServices;
        }

        // GET /api/Position
        [HttpGet]
        public IActionResult GetAll()
        {
            List<PositionDTO> allPositionDTOs = _positionServices.GetAllPositionDTOs();
            return Ok(allPositionDTOs);
        }

        // GET /api/Position/Vessel/{vesselId}
        [HttpGet("Vessel/{vesselId}")]
        public IActionResult GetAllByVesselId(int vesselId)
        {
            List<PositionDTO> allPositionDTOs = _positionServices.GetAllPositionDTOsByVesselId(vesselId);
            return Ok(allPositionDTOs);
        }

        // GET /api/Position/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            PositionDTO positionDTO = _positionServices.GetPositionDTOById(id);
            return Ok(positionDTO);
        }

        // POST /api/Position
        [HttpPost]
        public IActionResult Create([FromBody] PositionDTO positionCreate)
        {
            _positionServices.CreateFromPositionDTO(positionCreate);
            return Ok();
        }

        // PUT /api/Position
        [HttpPut]
        public IActionResult Update([FromBody] PositionDTO positionUpdate)
        {
            _positionServices.UpdateFromPositionDTO(positionUpdate);
            return Ok();
        }

        // DELETE /api/Position/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _positionServices.DeletePositionById(id);
            return Ok();
        }
    }
}