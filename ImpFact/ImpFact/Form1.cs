using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacturaControl;

namespace ImpFact
{
    public partial class Form1 : Form
    {
        PrintDocument doc = new PrintDocument(); //creas e instancias el objeto, imaginatelo como si fuera una hoja a imprimir.
        public Form1()
        {
            InitializeComponent();
            doc.PrintPage += new PrintPageEventHandler(doc_PrintPage);
            //Donde doc_PrintPage es el metodo al que va a llamar cuando mandes imprimir el documento.

        }
        private void doc_PrintPage(Object sender, PrintPageEventArgs e)
        {
            //Aqui va a ir todo lo que quieras imprimir en la hoja o ticket en tu caso.
            //Sintaxis:     e.Graphics.DrawString(Cadena_a_imprimir, Font_A_Utilizar, Brush_A_Utilizar, posX, PosY);

            //Declaramos un tipo de letra      
            Font letra = new Font("Lucida Console", 9);

            e.Graphics.DrawString("Texto De Prueba2", letra, Brushes.Black, 0, 0);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            PrintDialog dlg = new PrintDialog();
            dlg.Document = doc;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                doc.Print(); //Este codigo lo que hace es ponerte el cuadro de dialogo de imprimir ahi puedes seleccionar la impresora y demas settings y manda a imprimir solo si le das en OK.
            }  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Factura factura = new Factura();
            factura.AddItem("100", "Articulo Prueba1", "0.15", "10.50", "", "15.00");
            factura.AddItem("2", "Articulo Prueba2", "15", "10.50", "", "30.00");
            factura.AddItem("100", "Articulo 3", "0.15", "10.50", "", "15.00");
            factura.AddItem("100", "Articulo 4", "0.15", "10.50", "", "15.00");
            factura.AddItem("100", "Articulo 5", "0.15", "10.50", "", "15.00");
            factura.AddItem("100", "Articulo 6", "0.15", "10.50", "", "15.00");
            factura.AddItem("100", "Articulo 7", "0.15", "10.50", "", "15.00");
            factura.AddItem("100", "Articulo 8", "0.15", "10.50", "", "15.00");
            factura.AddItem("100", "326 - DISCO RIGIDO PORTATIL WESTER DIGITAL 250 GB", "0.15", "10.50", "", "15.00");
            //El metodo AddTotal requiere 2 parametros, la descripcion del total, y el precio
            factura.AddTotal("SUBTOTAL", "29.75" );
            factura.AddTotal("IVA", "5.25" );
            factura.AddTotal("TOTAL", "35.00" );
            factura.AddTotal("", "" ); //Ponemos un total en blanco que sirve de espacio
            factura.AddTotal("RECIBIDO", "50.00" );
            factura.AddTotal("CAMBIO", "15.00" );
            factura.AddTotal("", "" );//Ponemos un total en blanco que sirve de espacio
            factura.AddTotal("USTED AHORRO", "0.00" );
            factura.PrintFactura("HP LaserJet P1005"); 
        }
    }
}
