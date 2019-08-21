using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel
{
    [Table("AspNetUsers")]
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public User()
        {
            Document = new HashSet<Document>();
        }
        public virtual ICollection<Document> Document { get; set; }
    }
}
