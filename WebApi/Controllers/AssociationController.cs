using Application.DTO;
using Application.Services;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociationController : ControllerBase
    {   
        private readonly AssociationService _associationService;

        List<string> _errorMessages = new List<string>();

        public AssociationController(AssociationService associationService)
        {
            _associationService = associationService;
        }

        // GET: api/Association
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssociationDTO>>> GetAssociations()
        {
            IEnumerable<AssociationDTO> associationsDTO = await _associationService.GetAll();

            return Ok(associationsDTO);
        }


        // GET: api/Association/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssociationDTO>> GetAssociationById(long id)
        {
            var associationDTO = await _associationService.GetById(id);

            if (associationDTO == null)
            {
                return NotFound();
            }

            return Ok(associationDTO);
        }

        // // PUT: api/Association/a@bc
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{email}")]
        // public async Task<IActionResult> PutAssociation(long id, HolidayDTO HolidayDTO)
        // {
        //     if (id != HolidayDTO.Id)
        //     {
        //         return BadRequest();
        //     }

        //     bool wasUpdated = await _associationService.Update(id, HolidayDTO, _errorMessages);

        //     if (!wasUpdated /* && _errorMessages.Any() */)
        //     {
        //         return BadRequest(_errorMessages);
        //     }

        //     return Ok();
        // }

        //POST: api/Association
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AssociationDTO>> PostAssociation(AssociationDTO associationDTO)
        {
            AssociationDTO associationResultDTO = await _associationService.Add(associationDTO, _errorMessages);

            if(associationResultDTO != null)
                return CreatedAtAction(nameof(GetAssociationById), new { id = associationResultDTO.Id }, associationResultDTO);
            else
                return BadRequest(_errorMessages);
        }
    }
}