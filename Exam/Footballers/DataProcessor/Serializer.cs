namespace Footballers.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;
    using Footballers.Data.Models;
    using Footballers.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            List<Coach> coaches = context.Coaches
                    .ToArray()
                    .Where(c => c.Footballers.Any())
                    .ToList();

            List<ExportCoachDto> dtos = Mapper.Map<List<ExportCoachDto>>(coaches).ToArray()
                .OrderByDescending(c => c.Footballers.Count)
                .ThenBy(c => c.CoachName)
                .ToList();

            return SerializerMethod<List<ExportCoachDto>>(dtos, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            List<ExportTeamWithMostFootballersDto> teams = context.Teams
                          .ToArray()
                          .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                          .Select(t => new ExportTeamWithMostFootballersDto
                          {
                              Name = t.Name,
                              Footballers = t.TeamsFootballers.ToArray().Select(tf => tf.Footballer).Where(f => f.ContractStartDate >= date).Select(f => new ExportFootballerDto
                              {
                                  FoorballerName = f.Name,
                                  ContractStartDate = f.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                                  ContractEndDate = f.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                                  BestSkillType = f.BestSkillType.ToString(),
                                  PositionType = f.PositionType.ToString(),

                              })
                              .OrderByDescending(f => f.ContractEndDate)
                              .ThenBy(f => f.FoorballerName)
                              .ToList()
                          })
                          .OrderByDescending(t => t.Footballers.Count)
                          .ThenBy(t => t.Name)
                          .Take(5)
                          .ToList();

            

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
            
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
