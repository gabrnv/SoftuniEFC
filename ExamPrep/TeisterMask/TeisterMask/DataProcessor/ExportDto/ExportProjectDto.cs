using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ExportProjectDto
    {
        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        [XmlElement("ProjectName")]
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string ProjectName { get; set; }
        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }
        [XmlArray("Tasks")]
        public ExportTaskDto[] Tasks { get; set; }
    }
}
