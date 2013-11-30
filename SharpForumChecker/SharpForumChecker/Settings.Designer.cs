namespace SharpForumChecker
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelRand = new System.Windows.Forms.Label();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.labelReloadRate = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbMailAddr = new System.Windows.Forms.TextBox();
            this.cbSendMail = new System.Windows.Forms.CheckBox();
            this.cbOpenInBrowser = new System.Windows.Forms.CheckBox();
            this.btnSelectAudio = new System.Windows.Forms.Button();
            this.tbSoundFileName = new System.Windows.Forms.TextBox();
            this.cbPlaySound = new System.Windows.Forms.CheckBox();
            this.selectFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.cbMinimizeOnLaunch = new System.Windows.Forms.CheckBox();
            this.cbAutostart = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(295, 324);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(376, 324);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelRand);
            this.groupBox1.Controls.Add(this.trackBar2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.labelReloadRate);
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 111);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(439, 96);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // labelRand
            // 
            this.labelRand.Location = new System.Drawing.Point(364, 55);
            this.labelRand.Name = "labelRand";
            this.labelRand.Size = new System.Drawing.Size(69, 27);
            this.labelRand.TabIndex = 5;
            this.labelRand.Text = "±0 мин";
            this.labelRand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar2
            // 
            this.trackBar2.AutoSize = false;
            this.trackBar2.Location = new System.Drawing.Point(131, 52);
            this.trackBar2.Maximum = 30;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(227, 30);
            this.trackBar2.TabIndex = 4;
            this.trackBar2.Value = 1;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "Рандомизация";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelReloadRate
            // 
            this.labelReloadRate.Location = new System.Drawing.Point(364, 16);
            this.labelReloadRate.Name = "labelReloadRate";
            this.labelReloadRate.Size = new System.Drawing.Size(69, 19);
            this.labelReloadRate.TabIndex = 2;
            this.labelReloadRate.Text = "0 мин";
            this.labelReloadRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.Location = new System.Drawing.Point(131, 16);
            this.trackBar1.Maximum = 30;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(227, 30);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.Value = 1;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Интервал обновления";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbMailAddr);
            this.groupBox2.Controls.Add(this.cbSendMail);
            this.groupBox2.Controls.Add(this.cbOpenInBrowser);
            this.groupBox2.Controls.Add(this.btnSelectAudio);
            this.groupBox2.Controls.Add(this.tbSoundFileName);
            this.groupBox2.Controls.Add(this.cbPlaySound);
            this.groupBox2.Location = new System.Drawing.Point(12, 213);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(439, 101);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Оповещения";
            // 
            // tbMailAddr
            // 
            this.tbMailAddr.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbMailAddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbMailAddr.Location = new System.Drawing.Point(219, 68);
            this.tbMailAddr.Name = "tbMailAddr";
            this.tbMailAddr.Size = new System.Drawing.Size(214, 20);
            this.tbMailAddr.TabIndex = 5;
            this.tbMailAddr.Text = "aso.asa7elo@gmail.com";
            this.tbMailAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbMailAddr.TextChanged += new System.EventHandler(this.tbMailAddr_TextChanged);
            // 
            // cbSendMail
            // 
            this.cbSendMail.AutoSize = true;
            this.cbSendMail.Location = new System.Drawing.Point(6, 70);
            this.cbSendMail.Name = "cbSendMail";
            this.cbSendMail.Size = new System.Drawing.Size(210, 17);
            this.cbSendMail.TabIndex = 4;
            this.cbSendMail.Text = " Отправлять уведомление на адрес:";
            this.cbSendMail.UseVisualStyleBackColor = true;
            // 
            // cbOpenInBrowser
            // 
            this.cbOpenInBrowser.AutoSize = true;
            this.cbOpenInBrowser.Location = new System.Drawing.Point(6, 47);
            this.cbOpenInBrowser.Name = "cbOpenInBrowser";
            this.cbOpenInBrowser.Size = new System.Drawing.Size(183, 17);
            this.cbOpenInBrowser.TabIndex = 3;
            this.cbOpenInBrowser.Text = " Открыть найденое в браузере";
            this.cbOpenInBrowser.UseVisualStyleBackColor = true;
            // 
            // btnSelectAudio
            // 
            this.btnSelectAudio.Location = new System.Drawing.Point(358, 19);
            this.btnSelectAudio.Name = "btnSelectAudio";
            this.btnSelectAudio.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAudio.TabIndex = 2;
            this.btnSelectAudio.Text = "Выбрать";
            this.btnSelectAudio.UseVisualStyleBackColor = true;
            this.btnSelectAudio.Click += new System.EventHandler(this.btnSelectAudio_Click);
            // 
            // tbSoundFileName
            // 
            this.tbSoundFileName.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tbSoundFileName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSoundFileName.Location = new System.Drawing.Point(131, 24);
            this.tbSoundFileName.Name = "tbSoundFileName";
            this.tbSoundFileName.ReadOnly = true;
            this.tbSoundFileName.Size = new System.Drawing.Size(221, 13);
            this.tbSoundFileName.TabIndex = 1;
            this.tbSoundFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbPlaySound
            // 
            this.cbPlaySound.Location = new System.Drawing.Point(6, 21);
            this.cbPlaySound.Name = "cbPlaySound";
            this.cbPlaySound.Size = new System.Drawing.Size(129, 20);
            this.cbPlaySound.TabIndex = 0;
            this.cbPlaySound.Text = "Проигрывать звук";
            this.cbPlaySound.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbPlaySound.UseVisualStyleBackColor = true;
            // 
            // selectFileDlg
            // 
            this.selectFileDlg.Filter = "аудиофайлы wav|*.wav";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbMinimizeToTray);
            this.groupBox3.Controls.Add(this.cbMinimizeOnLaunch);
            this.groupBox3.Controls.Add(this.cbAutostart);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(439, 93);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            // 
            // cbMinimizeToTray
            // 
            this.cbMinimizeToTray.AutoSize = true;
            this.cbMinimizeToTray.Location = new System.Drawing.Point(6, 65);
            this.cbMinimizeToTray.Name = "cbMinimizeToTray";
            this.cbMinimizeToTray.Size = new System.Drawing.Size(189, 17);
            this.cbMinimizeToTray.TabIndex = 2;
            this.cbMinimizeToTray.Text = " Сворачивать в системный трей";
            this.cbMinimizeToTray.UseVisualStyleBackColor = true;
            // 
            // cbMinimizeOnLaunch
            // 
            this.cbMinimizeOnLaunch.AutoSize = true;
            this.cbMinimizeOnLaunch.Location = new System.Drawing.Point(6, 42);
            this.cbMinimizeOnLaunch.Name = "cbMinimizeOnLaunch";
            this.cbMinimizeOnLaunch.Size = new System.Drawing.Size(248, 17);
            this.cbMinimizeOnLaunch.TabIndex = 1;
            this.cbMinimizeOnLaunch.Text = " Сворачивать окно при запуске программы";
            this.cbMinimizeOnLaunch.UseVisualStyleBackColor = true;
            // 
            // cbAutostart
            // 
            this.cbAutostart.AutoSize = true;
            this.cbAutostart.Location = new System.Drawing.Point(6, 19);
            this.cbAutostart.Name = "cbAutostart";
            this.cbAutostart.Size = new System.Drawing.Size(185, 17);
            this.cbAutostart.TabIndex = 0;
            this.cbAutostart.Text = "Запускать при старте системы";
            this.cbAutostart.UseVisualStyleBackColor = true;
            this.cbAutostart.CheckedChanged += new System.EventHandler(this.cbAutostart_CheckedChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 359);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Монитор объявлений - Настройки";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelReloadRate;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelRand;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbPlaySound;
        private System.Windows.Forms.Button btnSelectAudio;
        private System.Windows.Forms.TextBox tbSoundFileName;
        private System.Windows.Forms.OpenFileDialog selectFileDlg;
        private System.Windows.Forms.CheckBox cbOpenInBrowser;
        private System.Windows.Forms.CheckBox cbSendMail;
        private System.Windows.Forms.TextBox tbMailAddr;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbMinimizeToTray;
        private System.Windows.Forms.CheckBox cbMinimizeOnLaunch;
        private System.Windows.Forms.CheckBox cbAutostart;
    }
}