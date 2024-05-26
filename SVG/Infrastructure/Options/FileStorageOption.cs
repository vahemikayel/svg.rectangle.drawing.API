namespace SVG.API.Infrastructure.Options
{
    public class FileStorageOption
    {
        public static readonly string SectionName = "FileStorage";

        public string Path { get; set; }

        public string FileName { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
