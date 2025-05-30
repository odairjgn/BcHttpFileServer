using BcHttpFileServer.DTOs;
using MediaInfo.DotNetWrapper;
using Microsoft.AspNetCore.Mvc;

namespace BcHttpFileServer.Controllers
{
    [Route("files")]
    public class FileController : ControllerBase
    {
        const string RootFolder = @"E:\Users\odair\Videos";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Get(Args args)
        {
            if (string.IsNullOrEmpty(args.Path))
            {
                var dir = DirectoryDTO.Create(new DirectoryInfo(RootFolder), RootFolder);
                return Ok(dir);
            }

            var path = Path.Combine(RootFolder, args.Path);
            
            if (Directory.Exists(path))
            {
                var dir = DirectoryDTO.Create(new DirectoryInfo(path), RootFolder);
                return Ok(dir);
            }

            if (System.IO.File.Exists(path))
            {
                if (args.Info == true)
                {
                    var wr = new MediaInfoWrapper(path);
                    return Ok(wr);
                }

                var isDl = args.Download == true;
                var fileInfo = new FileInfo(path);
                var stream = fileInfo.OpenRead();
                var f = FileDTO.Create(fileInfo, RootFolder);
                return isDl ? File(stream, f.ContentType, f.Name) : File(stream, f.ContentType);
            }

            return NotFound();
        }
    }
}
