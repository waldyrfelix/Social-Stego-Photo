using System;
using System.Windows.Forms;
using StegoCore;

namespace StegoUI
{
    public partial class Form1 : Form
    {
        //private ISteganographyService _steganographyService;

        public Form1()
        {
            InitializeComponent();
          //  _steganographyService = IoC.Resolve<ISteganographyService>();
        }

        private void picImage_Click(object sender, EventArgs e)
        {
            if (dialogOpenFile.ShowDialog() == DialogResult.OK)
            {
                picImage.ImageLocation = dialogOpenFile.FileName;
            }
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            if (dialogOpenFile.CheckFileExists)
            {
                try
                {
                    //txtMessage.Text = _steganographyService.Extract(dialogOpenFile.FileName);
                    MessageBox.Show("Concluido.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
