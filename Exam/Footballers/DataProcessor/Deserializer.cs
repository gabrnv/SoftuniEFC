namespace Footballers.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            List<ImportCoachDto> dtos = DeserializerMethod<List<ImportCoachDto>>("Coaches",xmlString);

            List<Coach> coaches = new List<Coach>();

            foreach (var dto in dtos)
            {
                if(!IsValid(dto) || string.IsNullOrEmpty(dto.Nationality))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Coach coach = new Coach()
                {
                    Name = dto.Name,
                    Nationality = dto.Nationality
                };

                List<Footballer> footballers = new List<Footballer>();

                foreach (var fDto in dto.Footballers)
                {
                    if(!IsValid(fDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isContractStartDateValid = DateTime.TryParseExact(fDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ContractStartDate);

                    bool isContractEndDateValid = DateTime.TryParseExact(fDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ContractEndDate);

                    if(!isContractStartDateValid || !isContractEndDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(ContractStartDate > ContractEndDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(fDto.PositionType < 0 || fDto.PositionType > 3 || fDto.BestSkillType < 0 || fDto.BestSkillType > 4)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer footballer = new Footballer()
                    {
                        Name = fDto.Name,
                        ContractStartDate = ContractStartDate,
                        ContractEndDate = ContractEndDate,
                        PositionType = (PositionType)fDto.PositionType,
                        BestSkillType = (BestSkillType)fDto.BestSkillType
                    };

                    footballers.Add(footballer);
                }

                coach.Footballers = footballers;

                coaches.Add(coach);

                sb.AppendLine(String.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }

            context.Coaches.AddRange(coaches);
            context.SaveChanges();

            return sb.ToString().Trim();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            List<ImportTeamDto> dtos = JsonConvert.DeserializeObject<List<ImportTeamDto>>(jsonString);

            List<Team> teams = new List<Team>();

            foreach (ImportTeamDto dto in dtos)
            {
                if(!IsValid(dto) || dto.Trophies <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team team = new Team()
                {
                    Name = dto.Name,
                    Nationality = dto.Nationality,
                    Trophies = dto.Trophies
                };

                List<TeamFootballer> footballers = new List<TeamFootballer>();

                foreach (var footballer in dto.Footballers.Distinct())
                {
                    if(!context.Footballers.Any(f => f.Id == footballer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TeamFootballer mFootballer = new TeamFootballer()
                    {
                        FootballerId = footballer
                    };

                    footballers.Add(mFootballer);
                }

                team.TeamsFootballers = footballers;

                teams.Add(team);

                sb.AppendLine(String.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
            }

            context.Teams.AddRange(teams);

            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static T DeserializerMethod<T>(string rootTag, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootTag));

            T dtos = (T)serializer.Deserialize(new StringReader(inputXml));


            return dtos;
        }


        private static bool IsValid(object dto)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
