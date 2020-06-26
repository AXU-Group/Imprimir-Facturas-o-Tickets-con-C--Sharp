using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

//Declaramos variables
Font printFont = null;
SolidBrush myBrush = new SolidBrush(Color.Black);
string fontName = "Lucida Console";
int fontSize = 8;
Graphics gfx = null;


// Permito que el usuario seleccione una impresora
// Abro el cuadro de dialogo
PrintDialog pd = new PrintDialog();

// Creo la instacia de la configuarion de impresion
pd.PrinterSettings = new PrinterSettings();

// Creo el tipo de letra que se va a usar 
printFont = new Font(fontName, fontSize, FontStyle.Regular);

 //creo el documento con el que vamos a trabjar
PrintDocument doc = new PrintDocument();

//Determina la impresora que vamos a usar es la que seleccionamos en la configuracion
doc.PrinterSettings.PrinterName = pd.PrinterSettings.PrinterName;

//Nombre en del documento 
doc.DocumentName = "Impresion de Prueba";

//Organiza la pagina para posteriomente imprimirla
doc.PrintPage += new PrintPageEventHandler(pr_PrintPage);

//Imprime el documento
doc.Print();

//funcion que se encarga de imprimir
private void pr_PrintPage(Object sender , PrintPageEventArgs e)
{
e.Graphics.PageUnit = GraphicsUnit.Millimeter; //unidades de la impresion
gfx = e.Graphics;

//Agregamos tantas lineas como querramos y posiciones variadas.
gfx.DrawString("STRING A IMPRIMIR", printFont, myBrush, PosX, PosY, new StringFormat());
}