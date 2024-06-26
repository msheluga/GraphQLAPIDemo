﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GraphQLAPIDemo.Data.Models
{
    [Table("UserFieldAccess")]
    public partial class UserFieldAccess
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid FieldId { get; set; }
        public int FieldLevelAccess { get; set; }

        [ForeignKey(nameof(FieldId))]
        [InverseProperty("UserFieldAccesses")]
        public virtual Field Field { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserFieldAccesses")]
        public virtual User User { get; set; }
    }
}