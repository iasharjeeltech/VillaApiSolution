using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VillaApi.Models;
using VillaApi.Models.Dto.Villa;
using VillaApi.Repository.IRepository;


namespace VillaApi.Controllers
{
    [ApiController]
    //[Produces("application?json")]
    [Route("Api/VillaApi")]
    public class VillaAPIController : ControllerBase
    {
        private readonly IVillaRepository _dbVilla;
        private readonly ILogger<VillaAPIController> _logger;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaAPIController(ILogger<VillaAPIController> logger,
                                  IMapper mapper, IVillaRepository dbVilla)
        {
            _logger = logger;
            _dbVilla = dbVilla;
            _mapper = mapper;
            _response = new APIResponse();
        }

        #region GetApi
        /// <summary>
        /// To get the list of Villa
        /// </summary>
        /// <returns>List Of Vilaa</returns>
        [HttpGet] //swagger get schemas from here
        [ProducesResponseType(StatusCodes.Status200OK,Type =typeof(APIResponse))]
        public async Task<APIResponse> GetVillas()
        {
            try
            {
                _logger.LogInformation("Villas MilGaya");
                var listOfVilla = await _dbVilla.GetAllAsync();
                var villaDtoList = _mapper.Map<VillaDto>(listOfVilla); //yeh mapping ho rhi hai dto ko villa me

                _response.Result = villaDtoList;
                _response.StatusCode = System.Net.HttpStatusCode.OK;

                return _response;
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }


        [HttpGet("{id:int}", Name = "GetVilla")] //get schemas from here
        [ProducesResponseType(StatusCodes.Status404NotFound,Type =typeof(APIResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type =typeof(APIResponse))]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {

            try
            {
                if (id == 0)
                {
                    _logger.LogError("error hai id me" + id);
                    return BadRequest();
                }
                var villaFromDb = await _dbVilla.GetAllAsync(v => v.Id == id);
                if (villaFromDb == null)
                {
                    _logger.LogError("null hai id" + id);
                    return NotFound();
                }
                VillaDto villaDto = _mapper.Map<VillaDto>(villaFromDb);

                _response.Result = villaDto;
                _response.StatusCode = HttpStatusCode.OK;
                return _response;
            }
            catch (Exception ex) {

                _response.ErrorMessages.Add(ex.Message);
                //_response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

            }
            return _response;
        }
        #endregion

        #region PostApi
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VillaDto))]
        public async Task<IActionResult> Create([FromBody] VillaCreateDto createDto)
        {
            // Step 1: Null check — agar client ne data hi nahi bheja toh galat request
            if (createDto == null)
            {
                return BadRequest();
            }
            // Step 2: Duplicate check — agar same name ka villa already exist karta hai toh error bhejna
            Villa matchingVilla = await _dbVilla.GetAsync(v => v.Name.ToLower() == createDto.Name.ToLower());
            if (matchingVilla is not null)
            {
                // Error message ModelState me add karke bad request bhejna
                ModelState.AddModelError("message", "Villa Already Existed!");
                return BadRequest(ModelState);
            }
            // Step 3: Mapping — DTO ko Entity me convert karna, kyunki database me DTO save nahi hota
            Villa villa = _mapper.Map<Villa>(createDto);

            // Step 4: Database me naya villa save karna
            await _dbVilla.CreateAsync(villa);

            // Step 5: Custom response prepare karna (jo humne APIResponse class banayi hai usme)
            _response.Result = villa;                        // Result me naya villa object
            _response.StatusCode = HttpStatusCode.Created;  // Status code 201 Created set karna
            // Step 6: Client ko response bhejna + usse batana ki yeh naya villa kis route pe milega
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
        }
        #endregion


        #region PutAPi
        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            try
            {
                if (id == 0 || id != updateDto.Id)
                {
                    return BadRequest();
                }

                Villa villa = _mapper.Map<Villa>(updateDto);

                await _dbVilla.UpdateAsync(villa);

                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.NoContent;
                return _response;
            }
            catch (Exception ex) {

                _response.ErrorMessages.Add(ex.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }
        #endregion

        #region DeleteApi
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id, [FromBody] VillaDto villaDto)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villaFromDb = await _dbVilla.GetAsync(v => v.Id == id);

                if (villaFromDb == null)
                {
                    return BadRequest();
                }
                await _dbVilla.RemoveAsync(villaFromDb);
                _response.Result = villaFromDb;
                _response.StatusCode = HttpStatusCode.NoContent;

                return _response;
            }
            catch(Exception ex){
                _response.ErrorMessages.Add(ex.Message);
                _response.IsSuccess = false;

            }
            return _response;
        }
        #endregion

        #region PatchApi
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<APIResponse >> PartialUpdate(int id,[FromBody] JsonPatchDocument<VillaUpdateDto> patchDocument)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                Villa villaFromDb = await _dbVilla.GetAsync(v => v.Id == id, false);
                if (villaFromDb == null)
                {
                    return NotFound();
                }

                VillaUpdateDto updateDto = _mapper.Map<VillaUpdateDto>(villaFromDb);

                //VillaUpdateDto updateDto = new VillaUpdateDto() //yeh manual mapping hai 
                //{
                //    Amenity = villaFromDb.Amenity,
                //    Details = villaFromDb.Details,
                //    Id = villaFromDb.Id,
                //    ImageUrl = villaFromDb.ImageUrl,
                //    Occupancy = villaFromDb.Occupancy,
                //    Rate = villaFromDb.Rate,
                //    SqFt= villaFromDb.SqFt
                //};

                patchDocument.ApplyTo(updateDto, ModelState);

                Villa villa = _mapper.Map<Villa>(updateDto);

                await _dbVilla.UpdateAsync(villa);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.Result = _mapper.Map<VillaDto>(villa);

                return NoContent();
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.Message);
                _response.IsSuccess=false;
            }
            return _response;
            
        }
        #endregion

    }
}
 