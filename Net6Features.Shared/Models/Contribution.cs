using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Net6Features.Shared.Models
{
    [DataContract]
    public class Contribution : IModelId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DataMember(Order = 1)]
        public int Id { get; set; }
        [DataMember(Order = 2)]
        public string Type { get; set; }
        [DataMember(Order = 3)]
        public string Title { get; set; }
        [DataMember(Order = 4)]
        public string Date { get; set; }
        [DataMember(Order = 5)]
        public string Language { get; set; }
        [DataMember(Order = 6)]
        public bool Billed { get; set; }
        [DataMember(Order = 7)]
        public string Abstract { get; set; }
        [DataMember(Order = 8)]
        public string PreviewSrc { get; set; }
        [DataMember(Order = 11)]
        public string PrimaryTag { get; set; } = String.Empty;
        [DataMember(Order = 12)]
        public bool ExternalSpeaker { get; set; }
        [DataMember(Order = 18)]
        public int Conference { get; set; }
    }
}