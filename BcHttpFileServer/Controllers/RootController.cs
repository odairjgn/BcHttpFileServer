using BcHttpFileServer.DTOs;
using MediaInfo.DotNetWrapper;
using Microsoft.AspNetCore.Mvc;

namespace BcHttpFileServer.Controllers
{
    [Route("root")]
    public class RootController : ControllerBase
    {
        const string RootFolder = @"E:\Users\odair\Videos";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RootController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var dir = DirectoryDTO.Create(new DirectoryInfo(RootFolder), RootFolder);
            return Ok(dir);
        }

        [HttpGet("{*parametros}")]
        public IActionResult Get(string parametros)
        {
            var frags = parametros.Split('?');
            var path = Path.Combine(RootFolder, parametros.Split('?').First());
            var args = frags.Count() > 1 ? frags.LastOrDefault() : _httpContextAccessor.HttpContext.Request.QueryString.Value;

            args ??= string.Empty;

            if (Directory.Exists(path))
            {
                var dir = DirectoryDTO.Create(new DirectoryInfo(path), RootFolder);
                return Ok(dir);
            }

            if (System.IO.File.Exists(path))
            {
                var isInfo = args.Contains("info=true");

                if (isInfo)
                {
                    var wr = new MediaInfoWrapper(path);
                    return Ok(wr);
                }

                var isDl = args.Contains("download=true");
                var fileInfo = new FileInfo(path);
                var stream = fileInfo.OpenRead();
                var f = FileDTO.Create(fileInfo, RootFolder);
                return isDl ? File(stream, f.ContentType, f.Name) : File(stream, f.ContentType);
            }

            return NotFound();
        }
    }
}
