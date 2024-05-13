namespace Course.Services.Kdf
{
    public interface IKdfService
    {
        String DerivedKey(String salt, String password);
    }
}
/* Key Drivation service by RFC 2898
 * https://datatracker.ietf.org/doc/html/rfc2898#section-5.1
 */
