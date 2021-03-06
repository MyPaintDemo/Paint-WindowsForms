﻿using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace WindowsFormsApplication1
{
    [Serializable()]
    class FillHinhVuong : HinhVe
    {

        #region Constructor
        public FillHinhVuong()
            : base()
        {
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            Pen pen = new Pen(Color.Black, 1);
            graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new Rectangle(0, 0, 0, 1));
            graphicsPath.Widen(pen);
            khuVuc = new Region(new Rectangle(0, 0, 0, 1));
            khuVuc.Union(graphicsPath);
            loaiHinh = 2;
        }
        public FillHinhVuong(Pen curPen)
            : base(curPen)
        {;
            diemBatDau.X = 0; diemBatDau.Y = 0;
            diemKetThuc.X = 0; diemKetThuc.Y = 1;
            pen = new Pen(curPen.Color, curPen.Width);
            pen.DashStyle = curPen.DashStyle;
            graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new Rectangle(0, 0, 0, 1));
            graphicsPath.Widen(pen);
            khuVuc = new Region(new Rectangle(0, 0, 0, 1));
            khuVuc.Union(graphicsPath);
            loaiHinh = 2;
        }
        public FillHinhVuong(Pen curPen, Point diembatdau, Point diemketthuc, Point diemmousedown,
            GraphicsPath graphicspath, Region khuvuc, int vitrichuotsovoihinhve,
            bool dichuyen,int loaihinh)
            : base(curPen, diembatdau, diemketthuc, diemmousedown,graphicspath, khuvuc, vitrichuotsovoihinhve, dichuyen, loaihinh)
        {
            pen = new Pen(curPen.Color, curPen.Width);
            pen.DashStyle = curPen.DashStyle;
            diemBatDau = diembatdau;
            diemKetThuc = diemketthuc;
            diemMouseDown = diemmousedown;
            graphicsPath = graphicspath;
            khuVuc = khuvuc;
            viTriChuotSoVoiHinhVe = vitrichuotsovoihinhve;
            diChuyen = dichuyen;
            loaihinh = loaiHinh;
        }
        #endregion

        #region Methods

        // Tạo hình chữ nhật từ tọa độ 2 điểm
        protected virtual Rectangle VeHCN(int x1, int y1, int x2, int y2)
        {
            if (x1 > x2)
            {
                int tam = x1;
                x1 = x2;
                x2 = tam;
            }
            if (y1 > y2)
            {
                int tam = y1;
                y1 = y2;
                y2 = tam;
            }
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        protected virtual Rectangle VeHCN(Point A, Point B)
        {
            return VeHCN(A.X, A.Y, B.X, B.Y);
        }

        //Vẽ
        public override void Ve(Graphics g)
        { 
            Brush brush = new SolidBrush(Color.FromArgb(255, pen.Color));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(brush, VeHCN(diemBatDau, new Point(diemKetThuc.X, diemBatDau.Y + diemKetThuc.X - diemBatDau.X)));
            brush.Dispose();
        }
        
        // Sự kiện chuột
        public override void Mouse_Down(MouseEventArgs e)
        {
            viTriChuotSoVoiHinhVe = KiemTraViTri(e.Location);
            if (viTriChuotSoVoiHinhVe == 0)    //đánh dấu băt đầu di chuyển
            {

                diChuyen = true;
                diemMouseDown = e.Location;
            }
            else //vẽ hình mới
            {
                diemBatDau = e.Location;
                diemKetThuc.X = e.X; diemKetThuc.Y = e.Y - 1;

            }
        }

        public override void Mouse_Move(MouseEventArgs e)
        {
            if (diChuyen == true)
            {
                int deltaX = e.X - diemMouseDown.X;
                int deltaY = e.Y - diemMouseDown.Y;
                diemMouseDown = e.Location;
                DiChuyen(deltaX, deltaY);
            }
            else
            {

                diemKetThuc = e.Location;
            }
        }
        #endregion

        #region Bắt đối tượng
        public override void Mouse_Up(Object sender)
        {
            graphicsPath = new GraphicsPath();
            Pen drawpen = new Pen(pen.Color, pen.Width);
            graphicsPath.AddRectangle(VeHCN(diemBatDau, diemKetThuc));
            graphicsPath.Widen(drawpen);
            khuVuc = new Region(VeHCN(diemBatDau, diemKetThuc));
            khuVuc.Union(graphicsPath);
            diChuyen = false;
            viTriChuotSoVoiHinhVe = -1;

        }
        #endregion

        #region Tuần tự hóa 
        public FillHinhVuong(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)

        {
            khuVuc = new Region(VeHCN(diemBatDau, diemKetThuc));
        }
        public new void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
        }
        #endregion
    }
}
