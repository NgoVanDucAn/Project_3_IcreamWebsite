using C2108G1_Project_3.Models;
using C2108G1_Project_3.Repository;
using Microsoft.AspNetCore.Mvc;

namespace C2108G1_Project_3.Controllers
{
	
		[ApiController]
		public class RegisterUsersController : APIBaseController<RegisterUsers>
		{

			private readonly IRegisterUsersRepository _registerUsersRepository;



			public RegisterUsersController(IBaseRepository<RegisterUsers> repository, IRegisterUsersRepository registerUsersRepository) : base(repository)
			{
				_registerUsersRepository = registerUsersRepository;

		}

			[HttpGet("[controller]/[action]")]
			public async Task<List<RegisterUsers>> GetRegisterUserssByKeyword(string keyword)
			{
				var result = await _registerUsersRepository.GetRegisterUserssByKeyword(keyword);
				return result;
			}

		}
	
}
