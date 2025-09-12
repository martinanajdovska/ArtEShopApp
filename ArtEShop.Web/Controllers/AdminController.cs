using ArtEShop.Domain.DomainModels;
using ArtEShop.Domain.DTO;
using ArtEShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArtEShop.Web.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        // GET: api/>
        private readonly IRequestService _requestService;

        public AdminController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet("[action]")]
        public List<Request> GetAllRequests()
        {
            return _requestService.GetAll();
        }

        [HttpGet("[action]/{email}")]
        public List<Request> GetAllUserRequests(string email)
        {
            return _requestService.GetAllByUser(email);
        }

    }
}
