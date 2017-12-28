using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialMusic.Buisness.Abstract;
using SocialMusic.API.Models;
using SocialMusic.Models.EntityModels.AuthModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMusic.API.Controllers
{
    [Route("api/UserProfile")]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService _userProfileService;

        private readonly IMapper _mapper;

        public UserProfileController(IUserProfileService userProfileService, IMapper mapper)
        {
            _userProfileService = userProfileService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return new JsonResult(_mapper.Map<List<UserProfile>, List<UserProfileModel>>(await _userProfileService.GetAll()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return new JsonResult(await _userProfileService.GetById(id));
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            return new JsonResult(await _userProfileService.GetByName(name));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserProfile newUserProfile)
        {
            try
            {
                if (newUserProfile != null)
                {
                    if(ModelState.IsValid)
                    {
                        await _userProfileService.Insert(newUserProfile);
                        return Ok();
                    }
                    
                }
                return BadRequest("user profile is null");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]UserProfileModel newUserProfile)
        {
            try
            {
                await _userProfileService.Update(_mapper.Map<UserProfileModel, UserProfile>(newUserProfile), id);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id != 0)
                {
                    await _userProfileService.Remove(id);
                    return Ok();
                }
                return BadRequest("invalid id");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}