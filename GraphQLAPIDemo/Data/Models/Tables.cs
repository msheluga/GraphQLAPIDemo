﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GraphQLAPIDemo.Data.Models
{
    public partial class Tables
    {
        public Tables()
        {
            Fields = new HashSet<Fields>();
            UserTableAccess = new HashSet<UserTableAccess>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public string TableName { get; set; }
        [Required]
        public string ControllerName { get; set; }

        [InverseProperty("Table")]
        public virtual ICollection<Fields> Fields { get; set; }
        [InverseProperty("Table")]
        public virtual ICollection<UserTableAccess> UserTableAccess { get; set; }
    }
}