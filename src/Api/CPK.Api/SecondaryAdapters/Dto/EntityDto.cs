using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPK.Api.SecondaryAdapters.Dto
{
    public abstract class EntityDto<T> where T : IEquatable<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public T Id { get; set; }

        [ConcurrencyCheck]
        public string ConcurrencyToken { get; set; }
    }
}
