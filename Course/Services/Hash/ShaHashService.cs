﻿namespace Course.Services.Hash
{
    public class ShaHashService : IHashService
    {
        public String Digest(String input) => Convert.ToHexString(
            System.Security.Cryptography.SHA1.HashData(
                System.Text.Encoding.UTF8.GetBytes(input)));

    }
}
