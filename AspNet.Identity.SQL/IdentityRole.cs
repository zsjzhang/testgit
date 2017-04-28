using Microsoft.AspNet.Identity;
using System;

namespace AspNet.Identity.SQL
{
    /// <summary>
    /// Class that implements the ASP.NET Identity
    /// IRole interface 
    /// </summary>
    public class IdentityRole : IRole
    {
        /// <summary>
        /// Default constructor for Role 
        /// </summary>
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// Constructor that takes names as argument 
        /// </summary>
        /// <param name="name"></param>
        public IdentityRole(string name) : this()
        {
            Name = name;
        }

        public IdentityRole(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public IdentityRole(string name, string id,string describe)
        {
            Name = name;
            Id = id;
            Describe = describe;
        }
        /// <summary>
        /// Role ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is deleate
        /// </summary>
        public int IsDelete { get; set; }

        /// <summary>
        /// Describe
        /// </summary>
        public string _description;

        public string Describe
        {
            get
            {
                if (_description == null)
                {
                    return string.Empty;
                }
                return _description;
            }
            set { _description = value; }
        }
    }
}
