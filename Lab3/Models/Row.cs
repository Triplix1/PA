using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab3.Models
{
    public class Row
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RowId { get; set; }
        [StringLength(30, ErrorMessage = "Value has to be less than 30 characters!")]
        public string Value { get; set; }
        public override bool Equals(object obj)
        {
            var other = obj as Row;
            return this.RowId == other.RowId && this.Value == other.Value;
        }
        public override int GetHashCode()
        {
            return this.RowId + this.RowId * this.Value.Length;
        }
    }
}
