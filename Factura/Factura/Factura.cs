using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace FacturaControl
{
    class Factura
    {
        String[,] datos = new String[100, 3];
        String[,] items = new String[1000, 6];
        int contador_datos = 0;
        int contador_items = 0;
        int contador = 1;
        int posItemsY = 0;

        Font printFont = null;
        SolidBrush myBrush = new SolidBrush(Color.Black);
        string fontName = "Lucida Console";
        int fontSize = 8;
        Graphics gfx = null;

        public Factura()
        {

        }

        public bool PrinterExists(string impresora)
        {
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                if (impresora == strPrinter)
                    return true;
            }
            return false;
        }

        public void PrintFactura(string impresora)
        {
            printFont = new Font(fontName, fontSize, FontStyle.Regular);
            PrintDocument pr = new PrintDocument();
            pr.PrinterSettings.PrinterName = impresora;
            pr.DocumentName = "Impresion de Factura";
            pr.PrintPage += new PrintPageEventHandler(pr_PrintPage);
            pr.Print();
        }

        private void pr_PrintPage(Object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            gfx = e.Graphics;
            DrawDatos();
            DrawItems();
        }

        private void DrawDatos()
        {
            for (int i = 0; i < contador_datos; i++)
            {
                int PosX = int.Parse(datos[i, 1].ToString());
                int PosY = int.Parse(datos[i, 2].ToString());
                gfx.DrawString(datos[i, 0], printFont, myBrush, PosX, PosY, new StringFormat());
            }
        }

        private void DrawItems()
        {
            for (int i = 0; i < contador_items; i++)
            {
                int PosX = int.Parse(items[i, 1]);
                int PosY = int.Parse(items[i, 2]);
                gfx.DrawString(items[i, 0], printFont, myBrush, PosX, PosY + posItemsY, new StringFormat());
                if (contador == 6)
                {
                    posItemsY += 4; //incremento en 4 milimitros para la proxima linea
                    contador = 0;
                }
                contador++;
            }
        }

        public void AddDatos(string datoTexto, string PosX, string PosY)
        {
            datos[contador_datos, 0] = datoTexto;
            datos[contador_datos, 1] = PosX;
            datos[contador_datos, 2] = PosY;
            contador_datos++;
        }

        public void AddItems(string AdditemTexto)
        {
            string[] itemsDatos = AdditemTexto.Split(',');
            items[contador_items, 0] = AlignRightText(itemsDatos[0].Length, int.Parse(ConfigurationSettings.AppSettings["textBox_ImpCanMC"])) + itemsDatos[0];
            items[contador_items, 1] = ConfigurationSettings.AppSettings["textBox_ImpCanX"];
            items[contador_items, 2] = ConfigurationSettings.AppSettings["textBox_ImpCanY"];
            contador_items++;
            items[contador_items, 0] = AlignRightText(itemsDatos[2].Length, int.Parse(ConfigurationSettings.AppSettings["textBox_ImpPreMC"])) + itemsDatos[2];
            items[contador_items, 1] = ConfigurationSettings.AppSettings["textBox_ImpPreX"];
            items[contador_items, 2] = ConfigurationSettings.AppSettings["textBox_ImpPreY"];
            contador_items++;
            items[contador_items, 0] = AlignRightText(itemsDatos[3].Length, int.Parse(ConfigurationSettings.AppSettings["textBox_ImpAliMC"])) + itemsDatos[3];
            items[contador_items, 1] = ConfigurationSettings.AppSettings["textBox_ImpAliX"];
            items[contador_items, 2] = ConfigurationSettings.AppSettings["textBox_ImpAliY"];
            contador_items++;
            items[contador_items, 0] = AlignRightText(itemsDatos[4].Length, int.Parse(ConfigurationSettings.AppSettings["textBox_ImpBivMC"])) + itemsDatos[4];
            items[contador_items, 1] = ConfigurationSettings.AppSettings["textBox_ImpBivX"];
            items[contador_items, 2] = ConfigurationSettings.AppSettings["textBox_ImpBivY"];
            contador_items++;
            items[contador_items, 0] = AlignRightText(itemsDatos[5].Length, int.Parse(ConfigurationSettings.AppSettings["textBox_ImpImpMC"])) + itemsDatos[5];
            items[contador_items, 1] = ConfigurationSettings.AppSettings["textBox_ImpImpX"];
            items[contador_items, 2] = ConfigurationSettings.AppSettings["textBox_ImpImpY"];
            // Al detalle lo mando a comprobar y dividir en dos o mas lineas si es necesario
            DivideItemDet(itemsDatos[1].Length, int.Parse(ConfigurationSettings.AppSettings["textBox_ImpDetMC"]), itemsDatos[1]);
            // Continuo almacenando
            contador_items++;
        }

        private string AlignRightText(int lenght, int maxChar)
        {
            string espacios = "";
            int spaces = maxChar - lenght;
            for (int x = 0; x < spaces; x++)
                espacios += " ";
            return espacios;
        }

        private void DivideItemDet(int lenghtDet, int maxCharDet, string detalleText)
        {
            if (lenghtDet > maxCharDet)
            {
                string[] splitDetalle = detalleText.Split(' ');
                string linea = "";
                int i = 0;
                int contador_lineas = 0;

                while (i < splitDetalle.Length)
                {
                    while ((linea.Length < maxCharDet) && (i < splitDetalle.Length))
                    {
                        linea += splitDetalle[i] + " ";
                        i++;
                    }

                    if (contador_lineas < 1)
                    {
                        contador_items++;
                        items[contador_items, 0] = linea;
                        items[contador_items, 1] = ConfigurationSettings.AppSettings["textBox_ImpDetX"];
                        items[contador_items, 2] = ConfigurationSettings.AppSettings["textBox_ImpDetY"];
                        contador_lineas++;
                        linea = "";
                    }
                    else
                    {
                        contador_items++;
                        items[contador_items, 0] = "";
                        items[contador_items, 1] = ConfigurationSettings.AppSettings["textBox_ImpCanX"];
                        items[contador_items, 2] = ConfigurationSettings.AppSettings["textBox_ImpCanY"];
                        contador_items++;
                        items[contador_items, 0] = linea;
                        items[contador_items, 1] = ConfigurationSettings.AppSettings["textBox_ImpDetX"];
                        items[contador_items, 2] = ConfigurationSettings.AppSettings["textBox_ImpDetY"];
                        contador_items++;
                        items[contador_items, 0] = "";
                        items[contador_items, 1] = ConfigurationSettings.AppSettings["textBox_ImpPreX"];
                        items[contador_items, 2] = ConfigurationSettings.AppSettings["textBox_ImpPreY"];
                        contador_items++;
                        items[contador_items, 0] = "";
                        items[contador_items, 1] = ConfigurationSettings.AppSettings["textBox_ImpAliX"];
                        items[contador_items, 2] = ConfigurationSettings.AppSettings["textBox_ImpAliY"];
                        contador_items++;
                        items[contador_items, 0] = "";
                        items[contador_items, 1] = ConfigurationSettings.AppSettings["textBox_ImpBivX"];
                        items[contador_items, 2] = ConfigurationSettings.AppSettings["textBox_ImpBivY"];
                        contador_items++;
                        items[contador_items, 0] = "";
                        items[contador_items, 1] = ConfigurationSettings.AppSettings["textBox_ImpImpX"];
                        items[contador_items, 2] = ConfigurationSettings.AppSettings["textBox_ImpImpY"];
                        linea = "";
                    }
                }
            }
            else
            {
                contador_items++;
                items[contador_items, 0] = detalleText;
                items[contador_items, 1] = ConfigurationSettings.AppSettings["textBox_ImpDetX"];
                items[contador_items, 2] = ConfigurationSettings.AppSettings["textBox_ImpDetY"];
            }
        }
    }

}