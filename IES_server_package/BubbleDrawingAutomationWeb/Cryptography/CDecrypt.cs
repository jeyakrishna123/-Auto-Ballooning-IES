

using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BubbleDrawing.Cryptography
{
  internal class CDecrypt : CCryptography
  {
    public CDecrypt()
    {
      this.objTripleDES = new TripleDESCryptoServiceProvider();
      this.byKey = Encoding.Default.GetBytes("etinifnInoitavonnIsseitJ");
      this.objTripleDES.IV = this.byIV;
    }

    public override long Crypt(string sToBeDecrypted, out string sReformedData)
    {
      long num = 0;
      sReformedData = string.Empty;
      try
      {
        byte[] buffer = Convert.FromBase64String(sToBeDecrypted);
        CryptoStream cryptoStream = new CryptoStream((Stream) new MemoryStream(buffer, 0, buffer.Length), this.objTripleDES.CreateDecryptor(this.byKey, this.byIV), CryptoStreamMode.Read);
        sReformedData = new StreamReader((Stream) cryptoStream).ReadToEnd();
      }
      catch (Exception ex)
      {
        //EventLog.WriteEntry("QIS Cryptography", "Decrypt: " + ex.Message);
        num = 1L;
      }
      return num;
    }
  }
}
