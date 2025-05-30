using BcHttpFileServer.Utils;

namespace BcHttpFileServer.DTOs
{
    public record DirectoryDTO
    {
        public string Name { get; set; }
        public string FullName { get; set; }

        public List<string> Directories { get; set; }

        public List<FileDTO> Files { get; set; }

        public static DirectoryDTO Create(DirectoryInfo dir, string root)
        {
            var directory = new DirectoryDTO
            {
                Name = dir.Name,
                FullName = Path.GetRelativePath(root, dir.FullName),
            };

            var subdir = dir
                .GetDirectories()
                .Select(x => x.SafeCast(y => y.Name))
                .Where(x => x != default)
                .ToList();

            directory.Directories = subdir;

            var files = dir.GetFiles()
                .Select(x => x.SafeCast(y => FileDTO.Create(y, root)))
                .ToList();

            directory.Files = files;

            return directory;
        }
    }
}
