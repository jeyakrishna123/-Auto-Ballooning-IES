
using System.Security.Cryptography;

namespace BubbleDrawing.Cryptography
{
  public abstract class CCryptography
  {
    protected readonly byte[] byIV = new byte[8]
    {
      (byte) 103,
      (byte) 197,
      (byte) 216,
      (byte) 199,
      (byte) 37,
      (byte) 53,
      (byte) 122,
      (byte) 145
    };
    protected byte[] byKey = new byte[24];
    protected TripleDESCryptoServiceProvider objTripleDES;
    protected const string sKey = "etinifnInoitavonnIsseitJ";

    public abstract long Crypt(string sToCrypt, out string sReformedData);
  }
}
