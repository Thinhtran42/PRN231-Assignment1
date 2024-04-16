using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_Group5.Assignment1.Repo.Interfaces;
using PRN231_Group5.Assignment1.Repo.Models;
using PRN231_Group5.Assignment1.Repo.VIewModels.Member;

namespace PRN231_Group5.Assignment1.API.Controllers
{
    public class MembersController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            var members = await _unitOfWork.MemberRepository.GetAsync();
            if (members == null || !members.Any())
            {
                return NotFound();
            }
            return Ok(members);
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _unitOfWork.MemberRepository.GetByIDAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, UpdateMemberViewModel updateMemberViewModel)
        {
            var existedMember = await _unitOfWork.MemberRepository.GetByIDAsync(id);

            if (existedMember is null)
            {
                return NotFound();
            }

            var updateMember = _mapper.Map(updateMemberViewModel, existedMember);

            _unitOfWork.MemberRepository.Update(updateMember);
            _unitOfWork.Save();

            return NoContent();
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Member>> CreateMember(CreateMemberViewModel memberViewModel)
        {
            var existedMember = await _unitOfWork.MemberRepository.GetAsync(m => m.Email == memberViewModel.Email);
            if (existedMember.Any())
            {
                return BadRequest("Emails is already exist");
            }
            var member = _mapper.Map<Member>(memberViewModel);

            Random random = new Random();
            member.MemberId = random.Next();

            _unitOfWork.MemberRepository.Insert(member);
            _unitOfWork.Save();

            memberViewModel.Id = member.MemberId;
            return CreatedAtAction("GetMember", new { id = memberViewModel.Id }, memberViewModel);
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var existedMember = await _unitOfWork.MemberRepository.GetByIDAsync(id);
            if (existedMember is null)
            {
                return NotFound();
            }
            _unitOfWork.MemberRepository.Delete(existedMember);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
