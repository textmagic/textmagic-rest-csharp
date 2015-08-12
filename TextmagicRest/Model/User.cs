using System;

namespace TextmagicRest.Model
{
    /// <summary>
    /// User account representation class
    /// </summary>
    public class User: BaseModel
    {
        /// <summary>
        /// User ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Account username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Account first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Account last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Account full name
        /// </summary>
        public virtual string Name { get { return FirstName + " " + LastName; } }

        /// <summary>
        /// Account balance, in account currency
        /// </summary>
        public double Balance { get; set; }

        /// <summary>
        /// Account currency
        /// </summary>
        public Currency Currency { get; set; }
        
        /// <summary>
        /// Account timezone
        /// </summary>
        public Timezone Timezone { get; set; }
    }
}
