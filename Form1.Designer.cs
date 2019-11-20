namespace SpectrumAnalyzer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.rec_btn = new System.Windows.Forms.Button();
            this.stop_btn = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.plotEntityBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.lbl_timer = new System.Windows.Forms.Label();
            this.btn_open = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.lbl_name = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.plot1 = new ScottPlot.FormsPlot();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_play = new System.Windows.Forms.Button();
            this.buildDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.plotEntityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.form1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.progressBar_saving = new System.Windows.Forms.ProgressBar();
            this.lbl_frequency = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.plotEntityBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buildDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plotEntityBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.form1BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // rec_btn
            // 
            this.rec_btn.Enabled = false;
            this.rec_btn.Location = new System.Drawing.Point(139, 24);
            this.rec_btn.Name = "rec_btn";
            this.rec_btn.Size = new System.Drawing.Size(65, 23);
            this.rec_btn.TabIndex = 0;
            this.rec_btn.Text = "Record";
            this.rec_btn.UseVisualStyleBackColor = true;
            this.rec_btn.Click += new System.EventHandler(this.Rec_btn_Click);
            // 
            // stop_btn
            // 
            this.stop_btn.Enabled = false;
            this.stop_btn.Location = new System.Drawing.Point(218, 24);
            this.stop_btn.Name = "stop_btn";
            this.stop_btn.Size = new System.Drawing.Size(65, 23);
            this.stop_btn.TabIndex = 1;
            this.stop_btn.Text = "Stop";
            this.stop_btn.UseVisualStyleBackColor = true;
            this.stop_btn.Click += new System.EventHandler(this.Stop_btn_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 74);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(136, 420);
            this.listBox1.TabIndex = 2;
            // 
            // lbl_timer
            // 
            this.lbl_timer.AutoSize = true;
            this.lbl_timer.Location = new System.Drawing.Point(289, 29);
            this.lbl_timer.Name = "lbl_timer";
            this.lbl_timer.Size = new System.Drawing.Size(34, 13);
            this.lbl_timer.TabIndex = 3;
            this.lbl_timer.Text = "00:00";
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(854, 24);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(65, 23);
            this.btn_open.TabIndex = 4;
            this.btn_open.Text = "Open";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.Btn_open_Click);
            // 
            // btn_save
            // 
            this.btn_save.Enabled = false;
            this.btn_save.Location = new System.Drawing.Point(925, 24);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(65, 23);
            this.btn_save.TabIndex = 5;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.Btn_save_ClickAsync);
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_name.Location = new System.Drawing.Point(377, 24);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(72, 24);
            this.lbl_name.TabIndex = 6;
            this.lbl_name.Text = "Current";
            // 
            // btn_close
            // 
            this.btn_close.Enabled = false;
            this.btn_close.Location = new System.Drawing.Point(1008, 24);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(65, 23);
            this.btn_close.TabIndex = 8;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.Btn_close_Click);
            // 
            // plot1
            // 
            this.plot1.Location = new System.Drawing.Point(209, 74);
            this.plot1.Name = "plot1";
            this.plot1.Size = new System.Drawing.Size(639, 383);
            this.plot1.TabIndex = 9;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 24);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 10;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(252, 474);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Peak frequency: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Opened plots:";
            // 
            // btn_play
            // 
            this.btn_play.Enabled = false;
            this.btn_play.Location = new System.Drawing.Point(783, 24);
            this.btn_play.Name = "btn_play";
            this.btn_play.Size = new System.Drawing.Size(65, 23);
            this.btn_play.TabIndex = 13;
            this.btn_play.Text = "Play";
            this.btn_play.UseVisualStyleBackColor = true;
            this.btn_play.Click += new System.EventHandler(this.Btn_play_Click);
            // 
            // progressBar_saving
            // 
            this.progressBar_saving.Location = new System.Drawing.Point(12, 514);
            this.progressBar_saving.Name = "progressBar_saving";
            this.progressBar_saving.Size = new System.Drawing.Size(1061, 23);
            this.progressBar_saving.TabIndex = 14;
            // 
            // lbl_frequency
            // 
            this.lbl_frequency.AutoSize = true;
            this.lbl_frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_frequency.Location = new System.Drawing.Point(385, 474);
            this.lbl_frequency.Name = "lbl_frequency";
            this.lbl_frequency.Size = new System.Drawing.Size(18, 20);
            this.lbl_frequency.TabIndex = 15;
            this.lbl_frequency.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1085, 549);
            this.Controls.Add(this.lbl_frequency);
            this.Controls.Add(this.progressBar_saving);
            this.Controls.Add(this.btn_play);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.plot1);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.lbl_name);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_open);
            this.Controls.Add(this.lbl_timer);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.stop_btn);
            this.Controls.Add(this.rec_btn);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.plotEntityBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buildDataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plotEntityBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.form1BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button rec_btn;
        private System.Windows.Forms.Button stop_btn;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label lbl_timer;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.Button btn_close;
        private ScottPlot.FormsPlot plot1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_play;
        private System.Windows.Forms.BindingSource form1BindingSource;
        private System.Windows.Forms.BindingSource plotEntityBindingSource;
        private System.Windows.Forms.BindingSource buildDataBindingSource;
        private System.Windows.Forms.BindingSource plotEntityBindingSource1;
        private System.Windows.Forms.ProgressBar progressBar_saving;
        private System.Windows.Forms.Label lbl_frequency;
    }
}

