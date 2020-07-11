using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VrLifeServer.Database.DbModels
{
    public class AppData
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong RecordId { get; set; }
        [Required]
        public ulong AppId { get; set; }
        [Required]
        public string FieldName { get; set; }
        [Required]
        public ulong FieldId { get; set; }
        public string StringValue { get; set; }
        public long NumericValue { get; set; }
        public double DecimalValue { get; set; }

    }
}
