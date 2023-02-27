namespace Covidien.CGRS.ESS
{
    partial class StatusControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SwStatusLabelLabel = new System.Windows.Forms.Label();
            this.SwStatusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mSwStatusLabelLabel
            // 
            this.SwStatusLabelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SwStatusLabelLabel.Location = new System.Drawing.Point(40, 46);
            this.SwStatusLabelLabel.Name = "mSwStatusLabelLabel";
            this.SwStatusLabelLabel.Size = new System.Drawing.Size(129, 34);
            this.SwStatusLabelLabel.TabIndex = 26;
            this.SwStatusLabelLabel.Text = "Status:";
            // 
            // mSwStatusLabel
            // 
            this.SwStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SwStatusLabel.Location = new System.Drawing.Point(113, 46);
            this.SwStatusLabel.Name = "mSwStatusLabel";
            this.SwStatusLabel.Size = new System.Drawing.Size(448, 34);
            this.SwStatusLabel.TabIndex = 27;
            // 
            // StatusControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SwStatusLabel);
            this.Controls.Add(this.SwStatusLabelLabel);
            this.MinimumSize = new System.Drawing.Size(578, 0);
            this.Name = "StatusControl";
            this.Size = new System.Drawing.Size(578, 93);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label SwStatusLabelLabel;
        private System.Windows.Forms.Label SwStatusLabel;
    }
}
