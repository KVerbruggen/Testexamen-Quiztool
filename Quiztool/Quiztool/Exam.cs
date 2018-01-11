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
    
    public partial class Exam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Exam()
        {
            this.ExamTopics = new HashSet<ExamTopic>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<short> Timelimit { get; set; }
        public Nullable<short> Minimumscore { get; set; }
        public byte GenerateMethod { get; set; }
        public short GenerateInput { get; set; }
        public Nullable<byte> MinimumTopicsPassed { get; set; }
        public bool IsHidden { get; set; }
    
        public virtual Subject Subject { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExamTopic> ExamTopics { get; set; }
    }
}
