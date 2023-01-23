using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dto.Import
{
    [XmlType("Car")]
    public class ImportCarsDto
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]

        public string Model { get; set; }

        [XmlElement("TraveledDistance")]

        public long TraveledDistance { get; set; }

        [XmlArray("parts")]
        public List<ImportCarPartsDto> Parts { get; set; }
    }
}
