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
using ArtEShop.Service.Implementation;
using System.Security.Claims;
using ArtEShop.Domain.DTO;
using Microsoft.AspNetCore.Authorization;

namespace ArtEShop.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;

        public ShoppingCartsController(IShoppingCartService shoppingCartService, IOrderService orderService)
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
        }

        // GET: ShoppingCarts
        [Authorize]
        public IActionResult Index()
        {
            if (User.IsInRole("Admin")) return RedirectToAction("Index", "ArtPieces");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userShoppingCart = _shoppingCartService.GetByUserIdWithIncludedProducts(Guid.Parse(userId));
            return View(userShoppingCart);
        }

        public IActionResult DeleteShoppingCartItem(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userShoppingCart = _shoppingCartService.GetByUserId(Guid.Parse(userId));
            _shoppingCartService.DeleteItemFromShoppingCart(id, userShoppingCart.Id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Payment(Guid? id, int price)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            PaymentDTO model = new PaymentDTO();
            model.Email = userEmail;
            model.ArtPieceId = id;
            model.TotalPrice = price;

            return View(model);
        }

        [HttpPost]
        public IActionResult Payment(PaymentDTO paymentDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Order order = _orderService.CreateOrder(paymentDTO.ArtPieceId, Guid.Parse(userId));

            _shoppingCartService.PayOrder(paymentDTO, order);

            return RedirectToAction(nameof(Index));
        }

    }
}
