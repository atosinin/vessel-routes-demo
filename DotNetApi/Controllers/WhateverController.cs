using DotNetApi.Models;
using DotNetApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhateverController : ControllerBase
    {
        private readonly IWhateverService _whateverServices;

        public WhateverController(
            IWhateverService whateverServices)
        {
            _whateverServices = whateverServices;
        }

        // GET /api/Whatever
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            List<WhateverDTO> allWhateverDTOs = _whateverServices.GetAllWhateverDTOs();
            return Ok(allWhateverDTOs);
        }

        // GET /api/Whatever/UserAccount/{userId}
        [HttpGet("UserAccount/{userId}")]
        public IActionResult GetAllByUserAccountId(string userId)
        {
            List<WhateverDTO> allWhateverDTOs = _whateverServices.GetAllWhateverDTOsByUserAccountId(userId);
            return Ok(allWhateverDTOs);
        }

        // GET /api/Whatever/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            WhateverDTO whateverDTO = _whateverServices.GetWhateverDTOById(id);
            return Ok(whateverDTO);
        }

        // POST /api/Whatever
        [HttpPost]
        public IActionResult Create([FromBody] WhateverDTO whateverCreate)
        {
            _whateverServices.CreateFromWhateverDTO(whateverCreate);
            return Ok();
        }

        // PUT /api/Whatever
        [HttpPut]
        public IActionResult Update([FromBody] WhateverDTO whateverUpdate)
        {
            _whateverServices.UpdateFromWhateverDTO(whateverUpdate);
            return Ok();
        }

        // DELETE /api/Whatever/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _whateverServices.DeleteWhateverById(id);
            return Ok();
        }
    }
}