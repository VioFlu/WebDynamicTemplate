using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogData.Entities
{
    [Table("NavBarEntityItem")]
    public class NavBarEntityItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid GUID { get; set; }
        [ForeignKey("FK_tNavBarEntityItem_tNavBarEntity")]
        public Guid NavBarEntityGUID { get; set; }
        public NavBarEntity NavBarEntity { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "smallint")]
        public byte SeqNo { get; set; }
        [Column(TypeName = "bit")]
        public bool Hidden { get; set; }
    }

}