namespace Theatre.DataProcessor
{
    using AutoMapper;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var dtos = DeserializerMethod<ImportPlayDto[]>("Plays", xmlString);

            List<Play> plays = new List<Play>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isGenreValid = Enum.TryParse(typeof(Genre), dto.Genre, out var genre);

                if(!isGenreValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var duration = TimeSpan.Parse(dto.Duration, CultureInfo.InvariantCulture);

                if(duration.Hours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play()
                {
                    Title = dto.Title,
                    Duration = duration,
                    Rating = dto.Rating,
                    Genre = (Genre)genre,
                    Description = dto.Description,
                    Screenwriter = dto.Screenwriter
                };

                plays.Add(play);

                sb.AppendLine($"Successfully imported {play.Title} with genre {play.Genre} and a rating of {play.Rating}!");
            }

            context.Plays.AddRange(plays);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            var dtos = DeserializerMethod<ImportCastDto[]>("Casts", xmlString);

            List<Cast> casts = new List<Cast>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast()
                {
                    FullName = dto.FullName,
                    IsMainCharacter = dto.IsMainCharacter,
                    PhoneNumber = dto.PhoneNumber,
                    PlayId = dto.PlayId
                };

                casts.Add(cast);

                sb.AppendLine($"Successfully imported actor {cast.FullName} as a {(cast.IsMainCharacter ? "main" : "lesser")} character!");
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var dtos = JsonConvert.DeserializeObject<ImportTheatreAndTicketsDto[]>(jsonString);

            List<Theatre> Theatres = new List<Theatre>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre Theatre = new Theatre()
                {
                    Name = dto.Name,
                    NumberOfHalls = dto.NumberOfHalls,
                    Director = dto.Director
                };

                List<Ticket> tickets = new List<Ticket>();

                foreach (var t in dto.Tickets)
                {
                    if(!IsValid(t))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket ticket = new Ticket()
                    {
                        Price = t.Price,
                        RowNumber = t.RowNumber,
                        PlayId = t.PlayId
                    };

                    tickets.Add(ticket);
                }

                Theatre.Tickets = tickets;

                Theatres.Add(Theatre);

                sb.AppendLine($"Successfully imported theatre {Theatre.Name} with #{Theatre.Tickets.Count} tickets!");
            }

            context.Theatres.AddRange(Theatres);
            context.SaveChanges();

            return sb.ToString();
        }

        private static T DeserializerMethod<T>(string rootTag, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootTag));

            T dtos = (T)serializer.Deserialize(new StringReader(inputXml));


            return dtos;
        }

        private static bool IsValid(object obj)
        {
            var validator = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
