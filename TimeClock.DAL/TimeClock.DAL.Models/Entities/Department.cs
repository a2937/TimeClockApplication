﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TimeClock.DAL.Models.Entities.Base;

namespace TimeClock.DAL.Models.Entities
{
    [Table("Groups", Schema = "User")]
    public class Department : EntityBase
    {
        [DataType(DataType.Text), MaxLength(50)]
        public string Name { get; set; }

        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Manager Manager { get; set; }

        public HashSet<Employee> AssignedEmployees { get; set; }
    }
}