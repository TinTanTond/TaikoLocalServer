namespace Domain.Entities
{
    public partial class Credential
    {
        public uint Baid { get; set; }
        public string Password { get; set; } = null!;
        
        // TODO: Remove salt since it is embedded in password hash
        public string Salt { get; set; } = null!;

        public virtual UserDatum? Ba { get; set; }
    }
}