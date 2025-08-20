using System.ComponentModel.DataAnnotations;

namespace AdminBFF.Areas.Catalogos.Payloads
{
    public class CatalogoPayloadChange
    {
        [Required(ErrorMessage = $"O campo {nameof(name)} é obrigatório")]
        public string name { get; set; }

        [Required(ErrorMessage = $"O campo {nameof(description)} é obrigatório")]
        public string description { get; set; }
    }
}
