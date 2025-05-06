using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VillaApi.Models.Dto.Villa;
using VillaApi.Models;
using VillaApi.Repository.IRepository;
using VillaApi.Models.Dto.VillaNumber;
using VillaApi.Models.Dto;

namespace VillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly ILogger<VillaNumberAPIController> _logger;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaNumberAPIController(ILogger<VillaNumberAPIController> logger,
                                  IMapper mapper, IVillaNumberRepository dbVillaNumber)
        {
            _logger = logger;
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
            _response = new APIResponse();
        }

        #region GetApi
        /// <summary>
        /// To get the list of Villa
        /// </summary>
        /// <returns>List Of Vilaa</returns>
        [HttpGet] //swagger get schemas from here
        public async Task<ActionResult<APIResponse>> Get()
        {
            try
            {
                _logger.LogInformation("VillaNumbers MilGaya");
                var listOfVillaNumber = await _dbVillaNumber.GetAllAsync();
                var villaNumberDtoList = _mapper.Map<VillaNumberDto>(listOfVillaNumber); //yeh mapping ho rhi hai dto ko villa me

                _response.Result = villaNumberDtoList;
                _response.StatusCode = System.Net.HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.Message);
                _response.IsSuccess = false;
                return _response;
            }
        }


        [HttpGet("{villaNo:int}", Name = "Get")] //get schemas from here
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(APIResponse))]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {

            try
            {
                if (villaNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("");
                    _logger.LogError("error hai id me" + villaNo);
                    return BadRequest(_response);
                }
                var villaNumberFromDb = await _dbVillaNumber.GetAllAsync(v => v.VillaNo == villaNo);
                if (villaNumberFromDb == null)
                {
                    _logger.LogError("null hai id" + villaNo);
                    return NotFound();
                }
                VillaNumberDto villaNumberDto = _mapper.Map<VillaNumberDto>(villaNumberFromDb);

                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("");

                _response.Result = villaNumberDto;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.ErrorMessages.Add(ex.Message);
                //_response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
        }
        #endregion

        #region PostApi
        [HttpPost]
        public async Task<ActionResult<APIResponse>> Create([FromBody] VillaNumberCreateDto createDto)
        {
            // Step 1: Null check — agar client ne data hi nahi bheja toh galat request
            if (createDto == null)
            {
                return BadRequest();
            }
            // Step 2: Duplicate check — agar same name ka villa already exist karta hai toh error bhejna
            VillaNumber matchingVillaNumber = await _dbVillaNumber.GetAsync(v => v.VillaNo == createDto.VillaNo);
            if (matchingVillaNumber is not null)
            {
                // Error message ModelState me add karke bad request bhejna
                ModelState.AddModelError("message", "VillaNumber Already Existed!");
                return BadRequest(ModelState);
            }
            // Step 3: Mapping — DTO ko Entity me convert karna, kyunki database me DTO save nahi hota
            VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDto);

            // Step 4: Database me naya villa save karna
            await _dbVillaNumber.CreateAsync(villaNumber);

            // Step 5: Custom response prepare karna (jo humne APIResponse class banayi hai usme)
            _response.Result = villaNumber;                        // Result me naya villa object
            _response.StatusCode = HttpStatusCode.Created;  // Status code 201 Created set karna
            // Step 6: Client ko response bhejna + usse batana ki yeh naya villa kis route pe milega
            return CreatedAtRoute("GetVillaNumber", new { villaNo = villaNumber.VillaNo }, _response);
        }
        #endregion


        #region PutAPi
        [HttpPut("{villaNo:int}")]
        public async Task<ActionResult<APIResponse>> Update(int villaNo, [FromBody] VillaNumberUpdateDto updateDto)
        {
            try
            {
                if (villaNo == 0 || villaNo != updateDto.VillaNo)
                {
                    return BadRequest();
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(updateDto);

                await _dbVillaNumber.UpdateAsync(villaNumber);

                _response.Result = _mapper.Map<VillaDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                return _response;
            }
            catch (Exception ex)
            {

                _response.ErrorMessages.Add(ex.Message);
                _response.IsSuccess = false;
            }
            return _response;
        }
        #endregion

        #region DeleteApi
        [HttpDelete("{villaNo:int}")]
        public async Task<ActionResult<APIResponse>> Delete(int villaNo, [FromBody] VillaDto villaDto)
        {
            try
            {
                if (villaNo == 0)
                {
                    return BadRequest();
                }
                var villaNumberFromDb = await _dbVillaNumber.GetAsync(v => v.VillaNo == villaNo);

                if (villaNumberFromDb == null)
                {
                    return BadRequest();
                }
                await _dbVillaNumber.RemoveAsync(villaNumberFromDb);
                _response.Result = villaNumberFromDb;
                _response.StatusCode = HttpStatusCode.NoContent;

                return _response;
            }
            catch (Exception ex)
            {
                _response.ErrorMessages.Add(ex.Message);
                _response.IsSuccess = false;

            }
            return _response;
        }
        #endregion

    }
}
