//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Quiztool
{
    using System;
    using System.Collections.Generic;
    
    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            this.AnswerMultipleChoices = new HashSet<AnswerMultipleChoice>();
        }
    
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string Codeblock { get; set; }
        public byte QuestionType { get; set; }
        public byte Weight { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnswerMultipleChoice> AnswerMultipleChoices { get; set; }
        public virtual AnswerOpen AnswerOpen { get; set; }
        public virtual Topic Topic { get; set; }
    }
}