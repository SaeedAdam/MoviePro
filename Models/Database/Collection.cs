﻿namespace MoviePro.Models.Database;

public class Collection
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<MovieCollection> MovieCollections { get; set; } = new HashSet<MovieCollection>();
}
