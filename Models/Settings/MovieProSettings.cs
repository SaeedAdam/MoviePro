﻿namespace MoviePro.Models.Settings;

public class MovieProSettings
{
    public string TmDbApiKey { get; set; }
    public string DefaultBackdropSize { get; set; }
    public string DefaultPosterSize { get; set; }
    public string DefaultYoutubeKey { get; set; }
    public string DefaultCastImage { get; set; }
    public DefaultCollection DefaultCollection { get; set; }
    public DefaultCredentials DefaultCredentials { get; set; }
}

public class DefaultCredentials
{
    public string Role { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class DefaultCollection
{
    public string Name { get; set; }
    public string Description { get; set; }
}