namespace Theatre.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Data.Models;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using AutoMapper.QueryableExtensions;
    using AutoMapper;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            List<Theatre> theatres = context.Theatres
                    .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count() >= 20)
                    .ToList();

            List<ExportTheatreDto> dtos = Mapper.Map<List<ExportTheatreDto>>(theatres).OrderByDescending(t => t.Halls).ThenBy(t => t.Name).ToList();

            return JsonConvert.SerializeObject(dtos, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            List<Play> plays = context.Plays
                                      .Where(p => p.Rating <= rating)
                                      .ToList();

            List<ExportPlayDto> dtos = Mapper.Map<List<ExportPlayDto>>(plays).OrderBy(p => p.Title).ThenByDescending(p => p.Genre).ToList();

            return SerializerMethod<List<ExportPlayDto>>(dtos, "Plays");
        }

        private static string XmlSerializer<T>(object playDtos, string v)
        {
            throw new NotImplementedException();
        }

        private static string SerializerMethod<T>(T dto, string rootTag)
        {
            var sb = new StringBuilder();

            var root = new XmlRootAttribute(rootTag);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var serializer = new XmlSerializer(typeof(T), root);

            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, dto, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
