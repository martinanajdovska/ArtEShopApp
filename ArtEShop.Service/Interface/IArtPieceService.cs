using ArtEShop.Domain.DomainModels;
using ArtEShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Service.Interface
{
    public interface IArtPieceService
    {
        List<ArtPiece> GetAll();
        ArtPiece? GetById(Guid id);
        ArtPiece Insert(ArtPieceDTO artPieceDTO, string fileName);
        ArtPiece Update(ArtPiece artPiece);
        ArtPiece DeleteById(Guid id);
        void AddProductToShoppingCart(Guid id, Guid userId, int quantity);
        void ChangeAvailable(ArtPiece artPiece);
        List<ArtPiece> GetAllFilteredByName(string searchString);
    }
}
