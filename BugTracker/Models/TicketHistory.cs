//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BugTracker.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Changed { get; set; }
        public string UserId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}