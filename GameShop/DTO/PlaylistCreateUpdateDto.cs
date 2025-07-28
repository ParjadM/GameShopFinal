using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GameShop.DTO
{
    public class PlaylistCreateUpdateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
