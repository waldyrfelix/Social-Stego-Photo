using System;
using System.Drawing;
using System.Windows.Forms;
using StegoCore;
using StegoCore.Facebook;
using StegoCore.Infra;

namespace StegoUI
{
    public partial class Form1 : Form
    {
        private ISteganographyService _steganographyService;
        private ISerializationService _serializationService;

        public Form1()
        {
            InitializeComponent();

            _steganographyService = IoC.Resolve<ISteganographyService>();
            _serializationService = IoC.Resolve<ISerializationService>();
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
                    txtMessage.Text = _steganographyService.Extract(dialogOpenFile.FileName);

                    var facebookUser = _serializationService.Deserialize<FacebookUser>(txtMessage.Text);
                    fillGrid(facebookUser);

                    MessageBox.Show("Concluido.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void fillGrid(FacebookUser user)
        {
            int i = 0;
            foreach (var property in user.GetType().GetProperties())
            {
                var labelKey = createLabelForKey(property.Name, i);
                var labelValue = createLabelForValue(property.GetValue(user, null).ToString(), i);
                
                pnlContainer.Controls.Add(labelKey);
                pnlContainer.Controls.Add(labelValue);
                
                i++;
            }
        }

        private Label createLabelForKey(string key, int index)
        {
            return new Label
            {
                Location = new Point(10, 25 * index),
                Name = "lbl" + key,
                Text = key + ":",
                Font = new Font("Arial", 10),
                AutoSize = true
            };
        }
        private Label createLabelForValue(string value, int index)
        {
            return new Label
            {
                Location = new Point(110, 25 * index),
                Name = "val" + value,
                Text = value,
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true
            };
        }
    }
}
