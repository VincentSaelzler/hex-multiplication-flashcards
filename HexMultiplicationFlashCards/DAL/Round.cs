//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HexMultiplicationFlashCards.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Round
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Round()
        {
            this.Question = new HashSet<Question>();
        }
    
        public int Id { get; set; }
        public int Num { get; set; }
        public int QuizId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Question { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}