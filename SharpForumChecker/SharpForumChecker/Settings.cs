using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpForumChecker
{
    public partial class Settings : Form
    {
        private Form1 parent_form;
        private string soundFileName;

        public Settings(Form1 prnt, int intrvl, int rndm, bool playSound, string pathSound, bool openInBrows)
        {
            InitializeComponent();
            parent_form = prnt;
            
            trackBar1.Value = intrvl;
            trackBar1_Scroll(this, new EventArgs());
            trackBar2.Value = rndm;
            trackBar2_Scroll(this, new EventArgs());
            
            cbPlaySound.Checked = playSound;
            soundFileName = pathSound;
            tbSoundFileName.Text = System.IO.Path.GetFileName(soundFileName);
            selectFileDlg.InitialDirectory = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath)+"\\Audio";

            cbOpenInBrowser.Checked = openInBrows;
        }

    #region Диалоговые кнопки
        private void btnSave_Click(object sender, EventArgs e)
        {
            parent_form.changeSettings(trackBar1.Value, trackBar2.Value, cbPlaySound.Checked, soundFileName, cbOpenInBrowser.Checked);
            this.Close();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    #endregion 

    #region Элементы настройки интервала обновления и рандомизатора 
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            labelReloadRate.Text = Convert.ToString(trackBar1.Value) + " мин.";
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            labelRand.Text = "±" + Convert.ToString(trackBar2.Value / 2.0) + " мин.";
        }
    #endregion 

    #region настройки оповещений
        private void btnSelectAudio_Click(object sender, EventArgs e)
        {
            DialogResult res = selectFileDlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                soundFileName = selectFileDlg.FileName;
                tbSoundFileName.Text = System.IO.Path.GetFileName(soundFileName);
            }
        }
    #endregion
    }
}
