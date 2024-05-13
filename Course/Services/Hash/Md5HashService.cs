namespace Course.Services.Hash
{
    public class Md5HashService : IHashService
    {
        public string Digest(string input) => Convert.ToHexString(
            System.Security.Cryptography.MD5.HashData(
                System.Text.Encoding.UTF8.GetBytes(input)));

    }
}
