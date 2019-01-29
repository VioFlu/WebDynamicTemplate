using System.ComponentModel.DataAnnotations;

namespace BlogVF.ViewModels
{
    public class TemplateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int SeqNo { get; set; }
        public bool Hidden { get; set; }
        public string PageName { get; set; }
    }
}
