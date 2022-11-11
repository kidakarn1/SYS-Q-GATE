using Newtonsoft.Json.Linq;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FA_SYSTEM1
{

    class TAGPRINT
    {
        private PrintDocument PNT;
        public JArray TAGDATA;
        private PrintPreviewDialog PRE;
        public string ITEMCD { set; get; }
        public string ITEMNM { set; get; }
        public string REMAIN { set; get; }
        public string VENDCD { set; get; }
        public string VENDNM { set; get; }
        public string PONUMB { set; get; }
        public string RCDATE { set; get; }
        public string ISDATE { set; get; }
        public string ACPQTY { set; get; }
        public string TAGSEQ { set; get; }
        public string TAGSNP { set; get; }
        public string MODELP { set; get; }
        public string USERID { set; get; }
        public string USERNM { set; get; }
        public string USERDP { set; get; }
        public string QRCODE { set; get; }

        public TAGPRINT(JArray d)
        {
            TAGDATA = d;
            PNT = new PrintDocument();
            PNT.PrintPage += new PrintPageEventHandler(this.PRINTDOCUMENT_TAGFORMNG);
            PNT.PrinterSettings.PrinterName = CONTROLFORMS.CONFPRO.PRINTERNAME;
            if (!CONTROLFORMS.FMCONP.LBD_REMAIN.Text.Equals("0")) PNT.Print();
            //PRE = new PrintPreviewDialog();
            //PRE.Document = PNT;
            //PRE.WindowState = FormWindowState.Normal;
            //PRE.StartPosition = FormStartPosition.CenterParent;
            //PRE.ClientSize = new Size(800, 600); 
            //PRE.PrintPreviewControl.Zoom = 1; 
            //PRE.ShowDialog();
            //PNT.Print();
        }
        private void PRINTDOCUMENT_TAGFORMNG(object sender, System.Drawing.Printing.PrintPageEventArgs ev)
        {

            ITEMCD = TAGDATA[0]["ITEMCD"].ToString();
            ITEMNM = TAGDATA[0]["ITEMNM"].ToString();
            VENDCD = TAGDATA[0]["VENDCD"].ToString();
            VENDNM = TAGDATA[0]["VENDNM"].ToString();
            PONUMB = TAGDATA[0]["PONUMB"].ToString();
            RCDATE = TAGDATA[0]["RCDATE"].ToString();
            TAGSEQ = TAGDATA[0]["MAXSEQ"].ToString();
            TAGSNP = TAGDATA[0]["TAGSNP"].ToString();
            MODELP = TAGDATA[0]["MODELP"].ToString();
            USERID = CONTROLFORMS.MEMBER.UID_USER; // PadLeft(3, '0')
            USERDP = CONTROLFORMS.MEMBER.DEP_USER;
            ACPQTY = float.Parse(TAGDATA[0]["ACPQTY"].ToString()).ToString("###");
            REMAIN = float.Parse(CONTROLFORMS.FMCONP.LBD_REMAIN.Text, System.Globalization.NumberStyles.AllowThousands).ToString("###");
            QRCODE = string.Format("GD{0}{1}{2}{3}{4}{5}", PONUMB, ITEMCD.PadRight(25, ' '), VENDCD, ACPQTY.PadLeft(8, '0'), REMAIN.PadLeft(8, '0'), TAGDATA[0]["TAGSEQ"].ToString());


            QRCodeGenerator qr = new QRCodeGenerator();
            QRCodeData str_qr = qr.CreateQrCode(QRCODE, QRCodeGenerator.ECCLevel.M);
            QRCode code = new QRCode(str_qr);
            Image img_qr = code.GetGraphic(5);

            str_qr = qr.CreateQrCode(USERID, QRCodeGenerator.ECCLevel.M);
            code = new QRCode(str_qr);
            Image uid_qr = code.GetGraphic(5);

            PNT.PrinterSettings.DefaultPageSettings.PaperSize = new PaperSize("Custom", 800, 310);

            Console.Write(CONTROLFORMS.MEMBER.UID_USER + " " + CONTROLFORMS.MEMBER.ENAME_USER + " Test!.");
            Graphics g = ev.Graphics;
            Pen pen = new Pen(Color.Black, 2);
            Pen pen_in = new Pen(Color.Black, 2);
            StringFormat format = new StringFormat();
            format = new StringFormat(StringFormatFlags.DirectionRightToLeft);
            string fnm = CONTROLFORMS.MEMBER.ENAME_USER.Substring(0, CONTROLFORMS.MEMBER.ENAME_USER.IndexOf(' '));
            string lnm = CONTROLFORMS.MEMBER.ENAME_USER.Substring(CONTROLFORMS.MEMBER.ENAME_USER.IndexOf(' '));
            Console.WriteLine(CONTROLFORMS.MEMBER.ENAME_USER);
            USERNM = (fnm + "  " + lnm.Trim().Substring(0, 1) + ".").ToUpper();
            Console.WriteLine(USERNM);
            ISDATE = DateTime.Now.ToString("yyyyMMdd");

            g.DrawRectangle(pen, 10, 10, 700, 285);
            g.DrawRectangle(pen_in, 10, 10, 700, 25);
            //g.DrawRectangle(pen_in, 10, 15, 645, 25);
            g.DrawRectangle(pen_in, 10, 85, 150, 50);
            g.FillRectangle(new SolidBrush(Color.Black), 10, 85, 150, 50);
            g.DrawRectangle(pen_in, 10, 135, 150, 60);

            g.DrawRectangle(pen_in, 310, 10, 200, 25);
            g.DrawRectangle(pen_in, 510, 10, 200, 25);
            g.FillRectangle(new SolidBrush(Color.Black), 310, 10, 200, 25);

            g.DrawRectangle(pen_in, 510, 85, 100, 50);
            g.DrawRectangle(pen_in, 310, 135, 300, 60);
            g.DrawRectangle(pen_in, 310, 245, 150, 50);
            g.DrawRectangle(pen_in, 460, 195, 150, 50);
            //g.DrawRectangle(pen_in, 360,  135, 150, 160);

            g.DrawRectangle(pen_in, 10, 35, 300, 50);
            g.DrawRectangle(pen_in, 310, 35, 300, 50);
            g.DrawRectangle(pen_in, 160, 85, 450, 50);
            g.DrawRectangle(pen_in, 160, 135, 550, 60);
            g.DrawRectangle(pen_in, 10, 195, 600, 50);
            g.DrawRectangle(pen_in, 10, 245, 150, 50);
            g.DrawRectangle(pen_in, 160, 245, 450, 50);
            g.FillRectangle(new SolidBrush(Color.Black), 510, 85, 100, 50);

            //SET HEADER
            g.DrawRectangle(pen_in, 610, 35, 100, 260);
            g.DrawString("TBKK Thailand Co.,Ltd.", new Font("Calibri Light", 12, FontStyle.Bold), new SolidBrush(Color.Black), 12, 13);
            g.DrawString("QC Inspection", new Font("Calibri Light", 12, FontStyle.Bold), new SolidBrush(Color.White), 320, 13);
            g.DrawString("035 IDENTIFICATION_TAG", new Font("Calibri Light", 10, FontStyle.Bold), new SolidBrush(Color.Black), 520, 15);

            g.DrawString("PARTS NO", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 12, 37);

            g.DrawString("QTY", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.White), 12, 87);
            g.DrawString("SUPPLIER", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 12, 137);
            g.DrawString("SUPPLIER NAME", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 12, 197);
            g.DrawString("ORDER NO", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 12, 247);

            g.DrawString("PARTS NAME", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 162, 87);
            g.DrawString("TOTAL", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 162, 137);
            g.DrawString("RECEIVE DATE", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 162, 247);

            g.DrawString("MODEL", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 312, 37);
            g.DrawString("INSPC. ISSU BY", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 312, 137);

            g.DrawString("SNP.", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 312, 247);

            g.DrawString("PAGE", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 462, 197);
            g.DrawString("ISSU DATE", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 462, 247);

            g.DrawString("ID.", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 612, 137);
            g.DrawString("DEPT.", new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 612, 177);
            g.DrawString("TAG REMAIN", new Font("Arial", 10, FontStyle.Bold), new SolidBrush(Color.White), 515, 102);

            //SET DETAIL TAG 

            g.DrawString(ITEMCD, new Font("Calibri Light", 18, FontStyle.Bold), new SolidBrush(Color.Black), 12, 50);

            g.DrawString(ITEMNM, new Font("Calibri Light", 18, FontStyle.Bold), new SolidBrush(Color.Black), 162, 102);

            g.DrawString(VENDCD, new Font("Calibri Light", 20, FontStyle.Bold), new SolidBrush(Color.Black), 12, 160);
            g.DrawString(MODELP, new Font("Calibri Light", 20, FontStyle.Bold), new SolidBrush(Color.Black), 312, 50);
            g.DrawString(USERNM, new Font("Calibri Light", 20, FontStyle.Bold), new SolidBrush(Color.Black), 312, 160);
            g.DrawString(USERID, new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 644, 137);
            g.DrawString(USERDP, new Font("Calibri Light", 8, FontStyle.Bold), new SolidBrush(Color.Black), 644, 177);

            g.DrawString(VENDNM, new Font("Calibri Light", 12, FontStyle.Bold), new SolidBrush(Color.Black), 12, 220);

            g.DrawString(PONUMB, new Font("Calibri Light", 18, FontStyle.Bold), new SolidBrush(Color.Black), 12, 262);

            g.DrawString(RCDATE, new Font("Calibri Light", 18, FontStyle.Bold), new SolidBrush(Color.Black), 183, 262);
            g.DrawString(ISDATE, new Font("Calibri Light", 18, FontStyle.Bold), new SolidBrush(Color.Black), 483, 262);

            g.DrawString(TAGSEQ, new Font("Calibri Light", 20, FontStyle.Bold), new SolidBrush(Color.Black), 495, 210);

            g.DrawString(REMAIN, new Font("Calibri Light", 20, FontStyle.Bold), new SolidBrush(Color.White), new RectangleF(12, 100, 145, 35), format);
            g.DrawString(ACPQTY, new Font("Calibri Light", 20, FontStyle.Bold), new SolidBrush(Color.Black), new RectangleF(162, 160, 145, 35), format);

            g.DrawString(TAGSNP, new Font("Calibri L