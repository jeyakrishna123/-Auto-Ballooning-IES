

namespace BubbleDrawing.Cryptography
{
  public class CCryptFactory
  {
    public CCryptography GetDecryptor()
    {
      return (CCryptography) new CDecrypt();
    }

    public CCryptography GetEncryptor()
    {
      return (CCryptography) new CEncrypt();
    }
  }
}
