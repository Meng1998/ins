using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace InstallTools.Ser
{
    class FolderDialog : FolderNameEditor
    {
        FolderNameEditor.FolderBrowser fDialog = new
             FolderNameEditor.FolderBrowser();
        //构造函数
        public FolderDialog()
        {

        }

        public DialogResult DisplayDialog()
        {
            return DisplayDialog("请选择一个文件夹");
        }

        public DialogResult DisplayDialog(string description)
        {
            fDialog.Description = description;
            return fDialog.ShowDialog();
        }

        public string Path
        {
            get
            {
                return fDialog.DirectoryPath;
            }
        }

        //析构函数
        ~FolderDialog()
        {
            fDialog.Dispose();
        }

    }

}
