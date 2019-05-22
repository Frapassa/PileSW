using Shell32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PileSwLib.UtilTest
{
    public class Altro:UtilTestLib
    {
        public static int GetCestino()
        {

            int itemsCount=0;
            Shell shell = new Shell();
            Folder recycleBin = shell.NameSpace(10);
            try
            {

                itemsCount = recycleBin.Items().Count;
                // int hresult = SHQueryRecycleBin(String.Empty, ref sqrbi);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return itemsCount;
          
        }
    }
}
