using Application.DTO;
using Application.Services;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociationController : ControllerBase
    {   
        private readonly AssociatonService _associacaoService;

        List<string> _errorMessages = new List<string>();

        public AssociationController(AssociatonService associacaoService)
        {
            _associacaoService = associacaoService;
        }

        // GET: api/Association
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssociationDTO>>> GetAssociacoes()
        {
            IEnumerable<AssociationDTO> associacoesDTO = await _associacaoService.GetAll();

            return Ok(associacoesDTO);
        }


        // GET: api/Association/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssociationDTO>> GetAssociacaoById(long id)
        {
            var associacaoDTO = await _associacaoService.GetById(id);

            if (associacaoDTO == null)
            {
                return NotFound();
            }

            return Ok(associacaoDTO);
        }

        // // PUT: api/Colaborator/a@bc
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{email}")]
        // public async Task<IActionResult> PutColaborator(long id, HolidayDTO HolidayDTO)
        // {
        //     if (id != HolidayDTO.Id)
        //     {
        //         return BadRequest();
        //     }

        //     bool wasUpdated = await _associacaoService.Update(id, HolidayDTO, _errorMessages);

        //     if (!wasUpdated /* && _errorMessages.Any() */)
        //     {
        //         return BadRequest(_errorMessages);
        //     }

        //     return Ok();
        // }

        //POST: api/Holiday
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AssociationDTO>> PostAssociacao(AssociationDTO associacaoDTO)
        {
            AssociationDTO asssociacaoResultDTO = await _associacaoService.Add(associacaoDTO);

            if(asssociacaoResultDTO != null)
                return CreatedAtAction(nameof(GetAssociacaoById), new { id = asssociacaoResultDTO.Id }, asssociacaoResultDTO);
            else
                return BadRequest(_errorMessages);
        }
    }
}