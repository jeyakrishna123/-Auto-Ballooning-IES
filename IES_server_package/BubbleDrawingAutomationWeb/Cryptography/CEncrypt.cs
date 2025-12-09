

using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BubbleDrawing.Cryptography
{
  internal class CEncrypt : CCryptography
  {
    public CEncrypt()
    {
      this.objTripleDES = new TripleDESCryptoServiceProvider();
      this.byKey = Encoding.Default.GetBytes("etinifnInoitavonnIsseitJ");
      this.objTripleDES.IV = this.byIV;
    }

    public override long Crypt(string sToBeEncrypted, out string sReformedData)
    {
      long num = 0;
      sReformedData = string.Empty;
      try
      {
        byte[] bytes = Encoding.Default.GetBytes(sToBeEncrypted);
        MemoryStream memoryStream = new MemoryStream();
        ICryptoTransform encryptor = this.objTripleDES.CreateEncryptor(this.byKey, this.byIV);
        CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(bytes, 0, bytes.Length);
        cryptoStream.FlushFinalBlock();
        sReformedData = Convert.ToBase64String(memoryStream.ToArray(), 0, (int) memoryStream.Length);
      }
      catch (Exception ex)
      {
        //EventLog.WriteEntry("QIS Cryptography", "Encrypt: " + ex.Message);
        num = 1L;
      }
      return num;
    }
  }
}
