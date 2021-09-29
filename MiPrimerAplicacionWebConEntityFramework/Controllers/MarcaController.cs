using iTextSharp.text;
using iTextSharp.text.pdf;
using MiPrimerAplicacionWebConEntityFramework.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimerAplicacionWebConEntityFramework.Controllers
{
    public class MarcaController : Controller
    {

        //Para trabajar con excel se trabaja con 2 clases principales; ExcelPackage y  ExcelWorkSheet, y descargando de nuget 
        //la libreria EPPlus que es la libreria principal para trabajar con excel.

        public FileResult generarExcel()
        {
            
            byte[] buffer;

            using (MemoryStream ms = new MemoryStream())
            {
                // Creamos el archivo
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage ep = new ExcelPackage();

                //Le añadimos los 'worksheets' que necesitemos. worksheets significa hojas de trabajo
                ep.Workbook.Worksheets.Add("Reporte excel");

                //Creamos un objecto tipo ExcelWorksheet para
                //manejarlo facilmente.
                //ep.Workbook.Worksheets[1]; con esto le estamos diciendo que estamos haciendo referencia a ep.Workbook.Worksheets.Add("Reporte excel");
                ExcelWorksheet ew = ep.Workbook.Worksheets[0];

                //Definiendo valor de una celda
                ew.Cells[1,1].Value = "ID Marca";
                ew.Cells[1,2].Value = "Nombre";
                ew.Cells[1,3].Value = "Descripcion";
                ew.Column(1).Width = 40;
                ew.Column(2).Width = 80;
                ew.Column(3).Width = 160;

                using (var range = ew.Cells[1, 1, 1, 3])
                {
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //color de letra
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    //color de fondo
                    range.Style.Fill.BackgroundColor.SetColor(Color.Red );

                }
                //Llenando las celdas con los datos
                List<MarcaCLS> listaExcel = (List<MarcaCLS>)Session["lista"];
                    int numeroRegistros = listaExcel.Count();

                    for (int i = 0; i < numeroRegistros; i++)
                    {
                    ew.Cells[i + 2, 1].Value = listaExcel[i].iidmarca;
                    ew.Cells[i + 2, 2].Value = listaExcel[i].nombre;
                    ew.Cells[i + 2, 3].Value = listaExcel[i].descripcion;
                }

                //Guardamos el excelPackage en el MemoryStream
                
                ep.SaveAs(ms);
                    buffer = ms.ToArray();
                
            }

                return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        public FileResult verPDF()
        {
            byte[] buffer;
            Document doc = new Document();
            using (MemoryStream memorys = new MemoryStream())
            {
                PdfWriter.GetInstance(doc, memorys);
                doc.Open();

                Paragraph titulo = new Paragraph("Listas Marcas");
                titulo.Alignment = Element.ALIGN_CENTER;
                Paragraph espacio = new Paragraph(" ");
                doc.Add(titulo);
                doc.Add(espacio);

                //creando la tabla,luego debemos crear la celda
                PdfPTable tabla = new PdfPTable(3);
                float[] anchotabla = new float[3] { 30, 50, 80 };
                tabla.SetWidths(anchotabla);

                //Creando las celdas
                PdfPCell celda1 = new PdfPCell(new Phrase("Id Marca"));
                celda1.BackgroundColor =new  BaseColor(130,130,130);
                celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda1);

                PdfPCell celda2 = new PdfPCell(new Phrase("Nombre"));
                celda2.BackgroundColor = new BaseColor(130, 130, 130);
                celda2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda2);

                PdfPCell celda3 = new PdfPCell(new Phrase("Descripcion"));
                celda3.BackgroundColor = new BaseColor(130, 130, 130);
                celda3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda3);

                //Agregando la informacion a las columnas
                List<MarcaCLS> listaPDF = (List<MarcaCLS>)Session["lista"];
                int numeroRegistros = listaPDF.Count();
                for (int i = 0; i < numeroRegistros; i++)
                {
                    tabla.AddCell(listaPDF[i].iidmarca.ToString());
                    tabla.AddCell(listaPDF[i].nombre);
                    tabla.AddCell(listaPDF[i].descripcion);
                }
                doc.Add(tabla);
                doc.Close();

                buffer = memorys.ToArray();
            }

            return File(buffer,"application/pdf");
        }
        // GET: Marca
        public ActionResult Index(MarcaCLS marcaCLS)
        {
            List<MarcaCLS> listamarca = null;
            string nombre = marcaCLS.nombre;
            using (var bd = new BDPasajeEntities())
            {
                if (marcaCLS.nombre == null)
                {
                    listamarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1
                                  select new MarcaCLS
                                  {
                                      iidmarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                    Session["lista"] = listamarca;
                }
                else
                {
                    listamarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1
                                  && marca.NOMBRE.Contains(nombre)
                                  select new MarcaCLS
                                  {
                                      iidmarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                    Session["lista"] = listamarca;
                }
               
            }
                return View(listamarca);
        }

        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(MarcaCLS marcaCLS)
        {
            string nombreMarca = marcaCLS.nombre;
            int numeroRepite = 0;
         
            using (var bd = new BDPasajeEntities())
            {
                numeroRepite = bd.Marca.Where(p => p.NOMBRE.Equals(nombreMarca)).Count();
            }
                if (!ModelState.IsValid || numeroRepite >=1)
                {
                if (numeroRepite >= 1) marcaCLS.mensajeError = "El nombre que esta ingresando ya existe";
                    return View(marcaCLS);
                }
                else
                {
                    using (var bd = new BDPasajeEntities())
                    {
                        Marca marca = new Marca();
                        marca.NOMBRE = marcaCLS.nombre;
                        marca.DESCRIPCION = marcaCLS.descripcion;
                        marca.BHABILITADO = 1;
                        bd.Marca.Add(marca);
                        bd.SaveChanges();
                    }
                }
            return RedirectToAction("index");
        }

        public ActionResult Editar( int id)
        {
            MarcaCLS marcaCLS = new MarcaCLS();

            using (var bd = new BDPasajeEntities())
            {
                Marca marca = bd.Marca.Where(p => p.IIDMARCA.Equals(id)).First();
                marcaCLS.iidmarca = marca.IIDMARCA;
                marcaCLS.nombre = marca.NOMBRE;
                marcaCLS.descripcion= marca.DESCRIPCION;
            }

                return View(marcaCLS);
        }
        [HttpPost]
        public ActionResult Editar(MarcaCLS marcaCLS)
        {
            Marca marca = new Marca();

            int numeroRegistrosEncontrados =0;
            string nombremarca = marcaCLS.nombre;
            int id = marcaCLS.iidmarca;
            using (var bd = new BDPasajeEntities())
            {
                numeroRegistrosEncontrados = bd.Marca.Where(p => p.NOMBRE.Equals(nombremarca) && !p.IIDMARCA.Equals(id)).Count();
            }

                if (!ModelState.IsValid || numeroRegistrosEncontrados >=1)
                {
                if (numeroRegistrosEncontrados >= 1) marcaCLS.mensajeError = "El nombre ya existe";
                    return View(marcaCLS);
                }
            using (var bd = new BDPasajeEntities())
            {
                marca = bd.Marca.Where(p => p.IIDMARCA.Equals(marcaCLS.iidmarca)).First();
                marca.NOMBRE = marcaCLS.nombre;
                marca.DESCRIPCION = marcaCLS.descripcion;
                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        

        public ActionResult Eliminar(int id)
        {
            using (var bd = new BDPasajeEntities())
            {
                Marca marca = new Marca();
                marca = bd.Marca.Where(p => p.IIDMARCA.Equals(id)).First();
                marca.BHABILITADO = 0;
                bd.SaveChanges();
            }
                return RedirectToAction("Index");
        }
    }
}