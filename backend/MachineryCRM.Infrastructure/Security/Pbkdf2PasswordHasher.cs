namespace MachineryCRM.Infrastructure.Security;

using System;
using System.Security.Cryptography;
using MachineryCRM.Domain.Interfaces;

public class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; // 128 bits
    private const int KeySize = 32;  // 256 bits
    private const int Iterations = 100000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;
    private const char Delimiter = ';';

    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

        return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public bool Verify(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            return false;

        string[] parts = passwordHash.Split(Delimiter);
        if (parts.Length != 2)
        {
            return false;
        }

        byte[] salt;
        byte[] hash;
        try
        {
            salt = Convert.FromBase64String(parts[0]);
            hash = Convert.FromBase64String(parts[1]);
        }
        catch (FormatException)
        {
            return false;
        }

        if (salt.Length != SaltSize || hash.Length != KeySize)
            return false;

        byte[] hashInput = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }
}