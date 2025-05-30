
using Microsoft.AspNetCore.StaticFiles;

namespace BcHttpFileServer.DTOs
{
    public record FileDTO
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public string Extension { get; set; }

        public long Size { get; set; }

        public string ContentType { get; set; }

        public static FileDTO Create(FileInfo file, string root)
        {
            var f = new FileDTO
            {
                Extension = file.Extension,
                FullName = Path.GetRelativePath(root, file.FullName),
                Name = file.Name,
                Size = file.Length,
                ContentType = TryGetMediaType(file.Name)
            };

            return f;
        }

        private static string TryGetMediaType(string name)
        {
            var provider = new FileExtensionContentTypeProvider();
            return provider.TryGetContentType(name, out var contentType) ? contentType : "application/octet-stream";
        }
    }
}
