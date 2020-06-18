using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tbxReportes.Text = @"C:\cadiaz\mavi\migracion\Versiones\6000Capacitacion\test";
            tbxDestino.Text = @"C:\cadiaz\mavi\migracion\Versiones\6000Capacitacion\testCopia";
        }

        private void btnCodigoOriginal_Click(object sender, EventArgs e)
        {
            fbdCodigoOriginal.ShowDialog();
            tbxCodigoOriginal.Text = fbdCodigoOriginal.SelectedPath;
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            fbdReportes.ShowDialog();
            tbxReportes.Text = fbdReportes.SelectedPath;
        }

        private void btnDestino_Click(object sender, EventArgs e)
        {
            fbdDestino.ShowDialog();
            tbxDestino.Text = fbdDestino.SelectedPath;
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            Join.EspecialesHaciaOriginales(tbxReportes.Text, tbxDestino.Text);
            //this.Close();
        }
    }
}
