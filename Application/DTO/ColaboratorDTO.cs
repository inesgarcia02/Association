namespace Application.DTO
{
    public class ColaboratorDTO
    {
        public string Identifier { get; set; }
        public long Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

        public ColaboratorDTO()
        {
        }

        public ColaboratorDTO(long id, string strName, string strEmail, string strStreet, string strPostalCode)
        {
            Identifier = "Colaborator";
            Id = id;
            Name = strName;
            Email = strEmail;
            Street = strStreet;
            PostalCode = strPostalCode;
        }
    }
}