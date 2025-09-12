using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArtEShop.Domain.DomainModels;
using ArtEShop.Repository;
using ArtEShop.Service.Interface;
using ArtEShop.Domain.DTO;
using System.Security.Claims;
using ArtEShop.Service.Implementation;
using Microsoft.IdentityModel.Tokens;

namespace ArtEShop.Web.Controllers
{
    public class RequestsController : Controller
    {
        private readonly IRequestService _requestService;
        private readonly IFileService _fileService;

        public RequestsController(IRequestService requestService, IFileService fileService)
        {
            _requestService = requestService;
            _fileService = fileService;
        }


        // GET: Requests
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Create));
            }
            HttpClient httpClient = new HttpClient();
            string URL = "";

            if (User.IsInRole("Admin"))
            {
                //URL = "https://localhost:44363/api/getallrequests";
                URL = "https://arteshopweb20250907181936-b6gaf4b3c6h7engd.canadacentral-01.azurewebsites.net/api/getallrequests";

            }
            else
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                //URL = $"https://localhost:44363/api/getalluserrequests/{userEmail}";
                URL = $"https://arteshopweb20250907181936-b6gaf4b3c6h7engd.canadacentral-01.azurewebsites.net/api/getalluserrequests/{userEmail}";

            }

            HttpResponseMessage message = await httpClient.GetAsync(URL);
            var data = await message.Content.ReadFromJsonAsync<List<Request>>();

            List<RequestDTO> model = new List<RequestDTO>();
            foreach (var item in data)
            {
                model.Add(_requestService.EntityToDTO(item));
            }
            ;

            return View(model);
        }

        // GET: Requests/Details/5
        public IActionResult Details(Guid id)
        {
            var request = _requestService.GetById(id);
            if (request == null)
            {
                return NotFound();
            }

            RequestDTO model = _requestService.EntityToDTO(request);

            return View(model);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
                string createdImageName = await _fileService.SaveFileAsync(model.ReferenceImageFile, allowedFileExtentions);
                if (!User.Identity.IsAuthenticated)
                {
                    await _requestService.RequestPiece(model, createdImageName);

                    return RedirectToAction("Index", "ArtPieces");

                }
                model.Email = User.FindFirst(ClaimTypes.Email)?.Value;
                await _requestService.RequestPiece(model, createdImageName);
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Requests/Edit/5
        public IActionResult Edit(Guid id)
        {
            var request = _requestService.GetById(id);
            if (request == null)
            {
                return NotFound();
            }

            RequestDTO model = _requestService.EntityToDTO(request);

            return View(model);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RequestDTO request)
        {
            if (User.IsInRole("Admin")) _requestService.UpdateFromDTO(request, true);
            else _requestService.UpdateFromDTO(request, false);
            return RedirectToAction(nameof(Index));
        }

    }
}
