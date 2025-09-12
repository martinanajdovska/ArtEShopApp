using ArtEShop.Domain.DomainModels;
using ArtEShop.Domain.DTO;
using ArtEShop.Domain.Email;
using ArtEShop.Repository.Interface;
using ArtEShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Service.Implementation
{
    public class ArtPieceService : IArtPieceService
    {
        private readonly IRepository<ArtPiece> _artPieceRepository;
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;
        private readonly IShoppingCartService _shoppingCartService;

        public ArtPieceService(IRepository<ArtPiece> artPieceRepository, IRepository<ShoppingCartItem> shoppingCartItemRepository, IShoppingCartService shoppingCartService)
        {
            _artPieceRepository = artPieceRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _shoppingCartService = shoppingCartService;
        }

        public void AddProductToShoppingCart(Guid id, Guid userId, int quantity)
        {
            var artPiece = GetById(id);
            if (artPiece == null)
            {
                throw new Exception("Art Piece not found");
            }

            var shoppingCart = _shoppingCartService.GetByUserId(userId);
            if (shoppingCart == null)
            {
                throw new Exception("Shopping cart not found");
            }

            ShoppingCartItem existingShoppingCartItem = _shoppingCartItemRepository.Get(selector: x => x,
                predicate: x => x.ShoppingCart.Id.Equals(shoppingCart.Id) && x.ArtPiece.Id.Equals(artPiece.Id));

            if (existingShoppingCartItem == null)
            {
                ShoppingCartItem shoppingCartItem = new ShoppingCartItem
                {
                    Id = Guid.NewGuid(),
                    ArtPiece = artPiece,
                    ShoppingCart = shoppingCart,
                    Quantity = quantity,
                    Order = null
                };

                _shoppingCartItemRepository.Insert(shoppingCartItem);
            }
            else
            {
                existingShoppingCartItem.Quantity += quantity;
                _shoppingCartItemRepository.Update(existingShoppingCartItem);
            }
        }

        public ArtPiece DeleteById(Guid id)
        {
            var artPiece = GetById(id);
            if (artPiece == null)
            {
                throw new Exception("Art Piece not found");
            }
            _artPieceRepository.Delete(artPiece);
            return artPiece;
        }

        public List<ArtPiece> GetAll()
        {
            return _artPieceRepository.GetAll(selector: x => x).ToList();
        }

        public ArtPiece? GetById(Guid id)
        {
            return _artPieceRepository.Get(selector: x => x,
                                          predicate: x => x.Id.Equals(id));
        }

        public ArtPiece Insert(ArtPieceDTO artPieceDTO, string fileName)
        {
            ArtPiece artPiece = new ArtPiece();
            artPiece.Id = Guid.NewGuid();
            artPiece.IsAvailable = true;
            artPiece.Price = artPieceDTO.Price;
            artPiece.Image = fileName;
            artPiece.Name = artPieceDTO.Name;
            return _artPieceRepository.Insert(artPiece);
        }


        public ArtPiece Update(ArtPiece artPiece)
        {
            var existing = GetById(artPiece.Id);
            existing.Name = artPiece.Name;
            existing.Price = artPiece.Price;
            return _artPieceRepository.Update(existing);
        }

        public void ChangeAvailable(ArtPiece artPiece)
        {
            artPiece.IsAvailable = !artPiece.IsAvailable;
            _artPieceRepository.Update(artPiece);
        }

        public List<ArtPiece> GetAllFilteredByName(string searchString)
        {
            return _artPieceRepository.GetAll(selector: x => x,
                predicate: x => x.Name.Contains(searchString)).ToList();
        }
    }
}
