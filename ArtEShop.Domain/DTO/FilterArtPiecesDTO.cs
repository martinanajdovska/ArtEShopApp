using ArtEShop.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtEShop.Domain.DTO
{
    public class FilterArtPiecesDTO
    {
        public List<ArtPiece>? ArtPieces { get; set; }
        public string? SearchString { get; set; }
    }
}
