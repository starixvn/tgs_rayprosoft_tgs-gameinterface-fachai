namespace Tgs.GameInterface.FaChai.Core;

public interface ICryptographyUtensil
{
    string AesDecrypt(string source, string key);

    string AesEncrypt(string source, string key);

    string Md5Encrypt(string source);
}
