namespace WindowsFormsApp5
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.fbdCodigoOriginal = new System.Windows.Forms.FolderBrowserDialog();
            this.btnCodigoOriginal = new System.Windows.Forms.Button();
            this.tbxCodigoOriginal = new System.Windows.Forms.TextBox();
            this.lblCodigoOriginal = new System.Windows.Forms.Label();
            this.btnReportes = new System.Windows.Forms.Button();
            this.tbxReportes = new System.Windows.Forms.TextBox();
            this.lblReportes = new System.Windows.Forms.Label();
            this.btnDestino = new System.Windows.Forms.Button();
            this.tbxDestino = new System.Windows.Forms.TextBox();
            this.lblDestino = new System.Windows.Forms.Label();
            this.btnEjecutar = new System.Windows.Forms.Button();
            this.fbdReportes = new System.Windows.Forms.FolderBrowserDialog();
            this.fbdDestino = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnCodigoOriginal
            // 
            this.btnCodigoOriginal.Location = new System.Drawing.Point(436, 37);
            this.btnCodigoOriginal.Name = "btnCodigoOriginal";
            this.btnCodigoOriginal.Size = new System.Drawing.Size(75, 23);
            this.btnCodigoOriginal.TabIndex = 0;
            this.btnCodigoOriginal.Text = "Cambiar";
            this.btnCodigoOriginal.UseVisualStyleBackColor = true;
            this.btnCodigoOriginal.Click += new System.EventHandler(this.btnCodigoOriginal_Click);
            // 
            // tbxCodigoOriginal
            // 
            this.tbxCodigoOriginal.Location = new System.Drawing.Point(28, 40);
            this.tbxCodigoOriginal.Name = "tbxCodigoOriginal";
            this.tbxCodigoOriginal.Size = new System.Drawing.Size(392, 20);
            this.tbxCodigoOriginal.TabIndex = 1;
            // 
            // lblCodigoOriginal
            // 
            this.lblCodigoOriginal.AutoSize = true;
            this.lblCodigoOriginal.Location = new System.Drawing.Point(25, 24);
            this.lblCodigoOriginal.Name = "lblCodigoOriginal";
            this.lblCodigoOriginal.Size = new System.Drawing.Size(78, 13);
            this.lblCodigoOriginal.TabIndex = 2;
            this.lblCodigoOriginal.Text = "Codigo Original";
            // 
            // btnReportes
            // 
            this.btnReportes.Location = new System.Drawing.Point(436, 84);
            this.btnReportes.Name = "btnReportes";
            this.btnReportes.Size = new System.Drawing.Size(75, 23);
            this.btnReportes.TabIndex = 0;
            this.btnReportes.Text = "Cambiar";
            this.btnReportes.UseVisualStyleBackColor = true;
            this.btnReportes.Click += new System.EventHandler(this.btnReportes_Click);
            // 
            // tbxReportes
            // 
            this.tbxReportes.Location = new System.Drawing.Point(28, 87);
            this.tbxReportes.Name = "tbxReportes";
            this.tbxReportes.Size = new System.Drawing.Size(392, 20);
            this.tbxReportes.TabIndex = 1;
            // 
            // lblReportes
            // 
            this.lblReportes.AutoSize = true;
            this.lblReportes.Location = new System.Drawing.Point(25, 71);
            this.lblReportes.Name = "lblReportes";
            this.lblReportes.Size = new System.Drawing.Size(50, 13);
            this.lblReportes.TabIndex = 2;
            this.lblReportes.Text = "Reportes";
            // 
            // btnDestino
            // 
            this.btnDestino.Location = new System.Drawing.Point(436, 132);
            this.btnDestino.Name = "btnDestino";
            this.btnDestino.Size = new System.Drawing.Size(75, 23);
            this.btnDestino.TabIndex = 0;
            this.btnDestino.Text = "Cambiar";
            this.btnDestino.UseVisualStyleBackColor = true;
            this.btnDestino.Click += new System.EventHandler(this.btnDestino_Click);
            // 
            // tbxDestino
            // 
            this.tbxDestino.Location = new System.Drawing.Point(28, 135);
            this.tbxDestino.Name = "tbxDestino";
            this.tbxDestino.Size = new System.Drawing.Size(392, 20);
            this.tbxDestino.TabIndex = 1;
            // 
            // lblDestino
            // 
            this.lblDestino.AutoSize = true;
            this.lblDestino.Location = new System.Drawing.Point(25, 119);
            this.lblDestino.Name = "lblDestino";
            this.lblDestino.Size = new System.Drawing.Size(43, 13);
            this.lblDestino.TabIndex = 2;
            this.lblDestino.Text = "Destino";
            // 
            // btnEjecutar
            // 
            this.btnEjecutar.Location = new System.Drawing.Point(345, 172);
            this.btnEjecutar.Name = "btnEjecutar";
            this.btnEjecutar.Size = new System.Drawing.Size(75, 23);
            this.btnEjecutar.TabIndex = 0;
            this.btnEjecutar.Text = "Ejecutar";
            this.btnEjecutar.UseVisualStyleBackColor = true;
            this.btnEjecutar.Click += new System.EventHandler(this.btnEjecutar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 221);
            this.Controls.Add(this.lblDestino);
            this.Controls.Add(this.lblReportes);
            this.Controls.Add(this.lblCodigoOriginal);
            this.Controls.Add(this.tbxDestino);
            this.Controls.Add(this.btnEjecutar);
            this.Controls.Add(this.btnDestino);
            this.Controls.Add(this.tbxReportes);
            this.Controls.Add(this.btnReportes);
            this.Controls.Add(this.tbxCodigoOriginal);
            this.Controls.Add(this.btnCodigoOriginal);
            this.Name = "Form1";
            this.Text = "Juntar Especiales";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog fbdCodigoOriginal;
        private System.Windows.Forms.Button btnCodigoOriginal;
        private System.Windows.Forms.TextBox tbxCodigoOriginal;
        private System.Windows.Forms.Label lblCodigoOriginal;
        private System.Windows.Forms.Button btnReportes;
        private System.Windows.Forms.TextBox tbxReportes;
        private System.Windows.Forms.Label lblReportes;
        private System.Windows.Forms.Button btnDestino;
        private System.Windows.Forms.TextBox tbxDestino;
        private System.Windows.Forms.Label lblDestino;
        private System.Windows.Forms.Button btnEjecutar;
        private System.Windows.Forms.FolderBrowserDialog fbdReportes;
        private System.Windows.Forms.FolderBrowserDialog fbdDestino;
    }
}

