using System.Collections.Generic;

namespace TextmagicRest.Model
{
    /// <summary>
    ///     TextMagic contact class
    /// </summary>
    public class Contact : BaseModel
    {
        /// <summary>
        ///     Contact ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Contact first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Contact last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Contact company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        ///     Contact phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///     Contact country
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        ///     Contact email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Contact custom fields
        /// </summary>
        public List<CustomField> CustomFields { get; set; }
    }
}