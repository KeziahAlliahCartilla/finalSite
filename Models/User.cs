//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Webdev_project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int user_Id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public int role_Id { get; set; }
    
        public virtual Role Role { get; set; }
    }
}
