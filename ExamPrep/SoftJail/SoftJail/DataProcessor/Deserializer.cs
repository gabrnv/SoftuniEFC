namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            List<ImportDepartmentDto> dtos = JsonConvert.DeserializeObject<List<ImportDepartmentDto>>(jsonString);

            List<Department> departments = new List<Department>();
            foreach (var dto in dtos)
            {
                bool isDepartmentValid = true;
                if(!IsValid(dto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if(dto.Cells.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                foreach (var cell in dto.Cells)
                {
                    if(IsValid(cell))
                    {
                        sb.AppendLine("Invalid Data");
                        isDepartmentValid = false;
                        continue;
                    }
                }

                if(!isDepartmentValid)
                {

                    continue;
                }

                Department department = Mapper.Map<Department>(dto);
                departments.Add(department);

                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportPrisonersDto[] dtos = JsonConvert.DeserializeObject<ImportPrisonersDto[]>(jsonString);

            List<Prisoner> prisoners = new List<Prisoner>();

            foreach(var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if(dto.Mails.Any(m => !IsValid(m)))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                bool isIncarsDateValid = DateTime.TryParseExact(dto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,DateTimeStyles.None, out DateTime incarserationDate);

                if (!isIncarsDateValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime? releaseDate = null;

                if (!String.IsNullOrEmpty(dto.ReleaseDate))
                {
                    bool isReleaseDateValid = DateTime.TryParseExact(dto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDateValue);

                    if (!isReleaseDateValid)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    releaseDate = releaseDateValue;
                    
                }



                Prisoner prisoner = new Prisoner()
                {
                    FullName = dto.FullName,
                    Nickname = dto.Nickname,
                    Age = dto.Age,
                    IncarcerationDate = incarserationDate,
                    ReleaseDate = releaseDate,
                    Bail = dto.Bail,
                    CellId = dto.CellId
                };

                foreach (var item in dto.Mails)
                {
                    Mail mail = Mapper.Map<Mail>(item);
                    prisoner.Mails.Add(mail);
                }

                prisoners.Add(prisoner);

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ImportOfficerDto[] dtos = DeserializerMethod<ImportOfficerDto[]>("Officers", xmlString);

            List<Officer> officers = new List<Officer>();   

            foreach (var dto in dtos)
            {
                if(!IsValid(dto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                bool isPositionValid = Enum.TryParse(typeof(Position), dto.Position, true, out var position);
                bool isWeaponValid = Enum.TryParse(typeof(Weapon), dto.Weapon, true, out var weapon);

                if(!isPositionValid || !isWeaponValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName = dto.FullName,
                    Salary = dto.Salary,
                    Position = (Position)position,
                    Weapon = (Weapon)weapon,
                    DepartmentId = dto.DepartmentId
                };

                foreach (var prisoner in dto.Prisoners)
                {
                    OfficerPrisoner op = new OfficerPrisoner()
                    {
                        Officer = officer,
                        PrisonerId = prisoner.Id
                    };

                    officer.OfficerPrisoners.Add(op);
                }
                officers.Add(officer);
                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static T DeserializerMethod<T>(string rootTag, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootTag));

            T dtos = (T)serializer.Deserialize(new StringReader(inputXml));


            return dtos;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}