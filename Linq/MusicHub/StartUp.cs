namespace MusicHub
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            Console.WriteLine(ExportAlbumsInfo(context,9));

        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumsInfo = context.Albums
                                            .Where(a => a.ProducerId == producerId)
                                            .Include(a => a.Producer)
                                            .Include(a => a.Songs)
                                            .ThenInclude(s => s.Writer)
                                            .ToArray()
                                            .Select(
                    a => new
                    {
                        AlbumName = a.Name,
                        ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                        Price = a.Price,
                        ProducerId = a.ProducerId,
                        Producer = a.Producer.Name,
                        Songs = a.Songs.Select(s => new
                        {
                            SongName = s.Name,
                            Price = s.Price,
                            Writer = s.Writer.Name
                        }).OrderByDescending(s => s.SongName).ThenBy(s => s.Writer).ToArray()
                    }).OrderByDescending(a => a.Price).ToArray();

            StringBuilder sb = new StringBuilder();

            int i = 0;
            foreach (var album in albumsInfo)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.Producer}");
                sb.AppendLine($"-Songs:");
               foreach(var song in album.Songs)
               {
                    i++;
                    sb.AppendLine($"---#{i}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.Writer}");
               }
               sb.AppendLine($"-AlbumPrice: {album.Price:f2}");
                i = 0;
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
