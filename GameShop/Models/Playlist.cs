
﻿using System.Collections.Generic;

namespace GameShop.Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        // Initialize the collection to avoid null references
        public ICollection<PlaylistGame> PlaylistGames { get; set; } = new List<PlaylistGame>();
    }
}
