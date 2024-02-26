using System.Security.Cryptography;
using System.Text;

public static class Security
{
    public static string CalculateMD5Hash(string input)
    {

        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = MD5.HashData(inputBytes);


        StringBuilder sb = new();
        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
}