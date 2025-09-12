using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArtEShop.Domain.DomainModels;
using ArtEShop.Domain.DTO;
using ArtEShop.Repository;
using System.Security.Claims;
using ArtEShop.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using ArtEShop.Service.Implementation;
using Microsoft.IdentityModel.Tokens;

namespace ArtEShop.Web.Controllers
{
    public class ArtPiecesController : Controller
    {
        private readonly IArtPieceService _artPieceService;
        private readonly IFileService _fileService;

        public ArtPiecesController(IArtPieceService artPieceService, IFileService fileService)
        {
            _artPieceService = artPieceService;
            _fileService = fileService;
        }


        // GET: ArtPieces
        public IActionResult Index(string SearchString)
        {
            FilterArtPiecesDTO model = new FilterArtPiecesDTO();
            if (SearchString.IsNullOrEmpty()) model.ArtPieces = _artPieceService.GetAll();
            else model.ArtPieces = _artPieceService.GetAllFilteredByName(SearchString);

            model.SearchString = SearchString.IsNullOrEmpty() ? "" : SearchString;
            return View(model);
        }

        // GET: ArtPieces/Details/5
        public IActionResult Details(Guid id)
        {
            var artPiece = _artPieceService.GetById(id);
            if (artPiece == null)
            {
                return NotFound();
            }

            return View(artPiece);
        }

        public IActionResult Edit(Guid id)
        {
            var artPiece = _artPieceService.GetById(id);
            if (artPiece == null)
            {
                return NotFound();
            }

            return View(artPiece);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArtPiece artPiece)
        {
            _artPieceService.Update(artPiece);
            return RedirectToAction(nameof(Index));
        }

        // GET: ArtPieces/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArtPieces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,Id,ImageFile")] ArtPieceDTO artPieceDTO)
        {
            if (ModelState.IsValid)
            {
                string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
                string createdImageName = await _fileService.SaveFileAsync(artPieceDTO.ImageFile, allowedFileExtentions);

                _artPieceService.Insert(artPieceDTO, createdImageName);
                return RedirectToAction(nameof(Index));
            }

            return View(artPieceDTO);

        }


        public IActionResult ChangeAvailable(Guid id)
        {
            var artPiece = _artPieceService.GetById(id);
            if (artPiece == null)
            {
                return NotFound();
            }
            _artPieceService.ChangeAvailable(artPiece);

            return RedirectToAction(nameof(Index));
        }


        [Authorize]
        public IActionResult AddArtPieceToCart(Guid id)
        {
            AddToCartDTO model = new AddToCartDTO();

            model.ArtPiece = _artPieceService.GetById(id);
            model.Quantity = 1;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddArtPieceToCart(AddToCartDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _artPieceService.AddProductToShoppingCart(model.ArtPiece.Id, Guid.Parse(userId), (int)model.Quantity);

            return RedirectToAction(nameof(Index));
        }

    }
}
