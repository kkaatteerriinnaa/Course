using Course.Services.Hash;

namespace Course.Services.Kdf
{
    public class Pbkdf1Service : IKdfService
    {
        private readonly IHashService _hashService;

        public Pbkdf1Service(IHashService hashService)
        {
            _hashService = hashService;
        }
        public string DerivedKey(string salt, string password) 
        {
            String t1 = _hashService.Digest(password + salt);
            String t2 = _hashService.Digest(t1);
            String t3 = _hashService.Digest(t2);
            return t3;
        }

        
    }
}
