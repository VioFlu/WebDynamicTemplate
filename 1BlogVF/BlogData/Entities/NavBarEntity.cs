using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BlogData.Entities
{
    [Table("NavBarEntity")]
    public class NavBarEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid GUID { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "smallint")]
        public byte SeqNo { get; set; }
        [Column(TypeName = "bit")]
        public bool Hidden { get; set; }
        public string PageName { get; set; }
        public ICollection<NavBarEntityItem> NavBarEntityItems { get; set; }
    }
}
