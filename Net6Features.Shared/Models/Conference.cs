using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Net6Features.Shared.Models
{
    [DataContract]
    public class Conference : IModelId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DataMember(Order = 1)]
        public int Id { get; set; }
        [DataMember(Order = 2)]
        public string Title { get; set; }
        [DataMember(Order = 3)]
        public DateTime DateFrom { get; set; }
        [DataMember(Order = 4)]
        public DateTime DateTo { get; set; }
        [DataMember(Order = 5)]
        public string Country { get; set; }
        [DataMember(Order = 6)]
        public string City { get; set; }
        [DataMember(Order = 7)]
        public string Url { get; set; }
        [DataMember(Order = 8)]
        public int ContributionsCount { get; set; }
        [DataMember(Order = 9)]
        public int SpeakerCount { get; set; }
    }
}