using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace Make_a_Drop.Application.Helpers
{
    public static class CreateZipFile
    {

        public static byte[] Zip(List<FileStreamResult> files)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var entry = archive.CreateEntry(file.FileDownloadName, CompressionLevel.Fastest);

                        using (var entryStream = entry.Open())
                        {
                            file.FileStream.CopyTo(entryStream);
                        }
                    }
                }

                return memoryStream.ToArray();
            }
        }
    }
}
