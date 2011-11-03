﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StegoJpeg;

namespace StegoUI
{
    public partial class Form1 : Form
    {
        private StegoJpegFacade _stegoJpegFacade;

        public Form1()
        {
            InitializeComponent();

            _stegoJpegFacade = new StegoJpegFacade();
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
                txtMessage.Text = _stegoJpegFacade.ExtractData(dialogOpenFile.FileName);
                MessageBox.Show("Concluido.");
            }
        }

        private void btnEmbed_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtMessage.Text))
            {
                _stegoJpegFacade.EmbedData(dialogOpenFile.FileName, txtMessage.Text);
                MessageBox.Show("A stego image foi salva!");
            }
        }
    }
}
