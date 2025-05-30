using Microsoft.AspNetCore.Mvc;

namespace BcHttpFileServer.DTOs
{
    public class Args
    {
        [FromQuery(Name = "path")]
        public string? Path { get; set; }

        [FromQuery(Name = "info")]
        public bool? Info { get; set; }

        [FromQuery(Name = "download")]
        public bool? Download { get; set; }
    }
}
