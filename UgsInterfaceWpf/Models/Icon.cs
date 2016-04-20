using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.IO;
using System.ComponentModel;
using UGS.Helpers;
using System.Windows.Threading;

namespace UGS.Models
{
    /*
    public static class bitmapextensions
    {
        public static void CreateThumbnail(this WriteableBitmap wbm, string filename)
        {
            BitmapSource image = wbm.Clone();

            if (filename != string.Empty)
            {
                using (FileStream stream = new FileStream(filename, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(stream);
                    stream.Close();
                }
            }
        }
    }
    */

    /// <summary>
    /// Icons defined in the preamp
    /// </summary>
    public enum Ic
    {
        ICONS_Shape = -1,
        ICONS_LargVolCr = 0,// large volume circle (mute & vol+)
        ICONS_VolComm = 1,// volume common (mute, vol +-)
        ICONS_Slash = 2,// slash (for mute)
        ICONS_Aux_Cont = 3,// aux container
        ICONS_Symmout = 4,
        ICONS_Bypass = 5,
        ICONS_Sleep = 6,
        ICONS_Tools = 7,
        ICONS_Standby = 8,
        ICONS_Mute = 9,
        ICONS_VolDown = 10,
        ICONS_VolUp = 11,

        // ICONS_FIRST_IN = 12,

        ICONS_PC = 12,
        ICONS_TV = 13,
        ICONS_Vinyl = 14,
        ICONS_Cd = 15,
        ICONS_Aux1 = 16,
        ICONS_Aux2 = 17,

        // ICONS_LAST_IN = 17,

        ICONS_Input = 18,
        ICONS_Trigger = 19,
        ICONS_Speaker = 20,

        ICONS_Volume = 21,
        ICONS_Balance = 22,
        ICONS_Offset = 23,

        ICONS_1 = 24,
        ICONS_2 = 25,
        ICONS_3 = 26,
        ICONS_4 = 27,
        ICONS_5 = 28,
        ICONS_6 = 29,
        ICONS_7 = 30,
        ICONS_8 = 31,
        ICONS_9 = 32,
        ICONS_0 = 33,

        ICONS_Clear = 34,

        ICONS_Plus = 35,
        ICONS_Minus = 36,


        ICONS_Hourglass = 37,

        ICONS_Presets = 38,
        ICONS_Lighting = 39,

        ICONS_Up = 40,
        ICONS_UDown = 41,

        MAX_ICONS = 42,
        ICONS_END = 128,
    }

    /// <summary>
    /// Commands defined in the preamp
    /// </summary>
    public enum Cm
    {

        IC_CM_FRR = 1,// Fill round rectangle 	(5)
        IC_CM_FR = 2,// Fill rectangle			(4)
        IC_CM_FR2 = 3,// Fill rectangle 2		(6)
        IC_CM_FT = 4,// Fill triangle			(6)
        IC_CM_FC = 5,// Fill cirle				(3)
        IC_CM_FCV = 6,// Fill curve				(5)
        IC_CM_CP = 32, // Copy icon				(1)
        IC_CM_NO = 48, // No icon				(0)
        IC_CM_END = 255, // End command

        // IC_BC = 128,
        // IC_FC = 192

    }

    /// <summary>
    /// Colors defined in the preamp
    /// </summary>
    public enum Cl
    {
        IC_BC = 128,
        IC_FC = 192
    }




    public class Icon : NotificationObject
    {
        /*
        IC_CM_FRR = 1  // Fill round rectangle 	(5)
        IC_CM_FR  = 2  // Fill rectangle		(4)
        IC_CM_FR2 = 3  // Fill rectangle 2		(6)
        IC_CM_FT  = 4  // Fill triangle			(6)
        IC_CM_FC  = 5  // Fill cirle			(3)
        IC_CM_FCV = 6  // Fill curve			(5)
        IC_CM_CP = 32  // Copy icon				(1)
        IC_CM_NO = 48  // No icon				(0)
        */


        public class FilledRectangle : IconShape
        {
            public override void AddData(int d)
            {
                switch (_addDataCounter)
                {
                    case 0: _data[_addDataCounter] = new IconShapeData() { Name = "x", Value = d }; break;
                    case 1: _data[_addDataCounter] = new IconShapeData() { Name = "y", Value = d }; break;
                    case 2: _data[_addDataCounter] = new IconShapeData() { Name = "width", Value = d }; break;
                    case 3: _data[_addDataCounter] = new IconShapeData() { Name = "height", Value = d }; break;
                }
                _addDataCounter++;
            }

            public FilledRectangle()
            {
                _data = new BindingList<IconShapeData>(){
                            new IconShapeData() { Name = "x", Value = 0 },
                            new IconShapeData() { Name = "y", Value = 0 },
                            new IconShapeData() { Name = "width", Value = 0},
                            new IconShapeData() { Name = "height", Value = 0},
                            };
            }
            /*
            public FilledRectangle(int __x, int __y, int __width, int __height)
            {
                x = __x;
                y = __y;
                width = __width;
                height = __height;
            }
            */
            public new Cm command
            {
                get { return _command; }
                set
                {
                    if (value != Cm.IC_CM_FR) throw new Exception();

                    _command = value;
                    RaisePropertyChanged(() => command);

                }
            }
            public int x
            {
                get
                {
                    return _data.Where(x => x.Name == "x").First().Value;
                }
            }
            public int y
            {
                get
                {
                    return _data.Where(x => x.Name == "y").First().Value;
                }
            }
            public int width
            {
                get
                {
                    return _data.Where(x => x.Name == "width").First().Value;
                }
            }
            public int height
            {
                get
                {
                    return _data.Where(x => x.Name == "height").First().Value;
                }
            }
        }
        public class FilledRoundedRectangle : FilledRectangle
        {
            public override void AddData(int d)
            {
                switch (_addDataCounter)
                {
                    case 0: _data[_addDataCounter] = new IconShapeData() { Name = "x", Value = d }; break;
                    case 1: _data[_addDataCounter] = new IconShapeData() { Name = "y", Value = d }; break;
                    case 2: _data[_addDataCounter] = new IconShapeData() { Name = "width", Value = d }; break;
                    case 3: _data[_addDataCounter] = new IconShapeData() { Name = "height", Value = d }; break;
                    case 4: _data[_addDataCounter] = new IconShapeData() { Name = "rndRadius", Value = d }; break;
                }
                _addDataCounter++;
            }

            public FilledRoundedRectangle()
            {
                _data = new BindingList<IconShapeData>(){
                            new IconShapeData() { Name = "x", Value = 0 },
                            new IconShapeData() { Name = "y", Value = 0 },
                            new IconShapeData() { Name = "width", Value = 0},
                            new IconShapeData() { Name = "height", Value = 0},
                            new IconShapeData() { Name = "rndRadius", Value = 0},
                            };
            }
            /*
            public FilledRoundedRectangle(int __x, int __y, int __width, int __height, int __rndRadius)
            {
                x = __x;
                y = __y;
                width = __width;
                height = __height;
                rndRadius = __rndRadius;
            }
            */

            public new Cm command
            {
                get { return _command; }
                set
                {
                    if (value != Cm.IC_CM_FRR) throw new Exception();

                    _command = value;
                    RaisePropertyChanged(() => command);

                }
            }
            public int rndRadius
            {
                get
                {
                    return _data.Where(x => x.Name == "rndRadius").First().Value;
                }
            }
        }
        public class FilledRectangle2 : IconShape
        {
            public override void AddData(int d)
            {
                switch (_addDataCounter)
                {
                    case 0: _data[_addDataCounter] = new IconShapeData() { Name = "x1", Value = d }; break;
                    case 1: _data[_addDataCounter] = new IconShapeData() { Name = "y1", Value = d }; break;
                    case 2: _data[_addDataCounter] = new IconShapeData() { Name = "x2", Value = d }; break;
                    case 3: _data[_addDataCounter] = new IconShapeData() { Name = "y2", Value = d }; break;
                    case 4: _data[_addDataCounter] = new IconShapeData() { Name = "x3", Value = d }; break;
                    case 5: _data[_addDataCounter] = new IconShapeData() { Name = "y3", Value = d }; break;
                }
                _addDataCounter++;
            }

            public FilledRectangle2()
            {
                _data = new BindingList<IconShapeData>(){
                            new IconShapeData() { Name = "x1", Value = 0 },
                            new IconShapeData() { Name = "y1", Value = 0 },
                            new IconShapeData() { Name = "x2", Value = 0 },
                            new IconShapeData() { Name = "y2", Value = 0 },
                            new IconShapeData() { Name = "x3", Value = 0 },
                            new IconShapeData() { Name = "y3", Value = 0 },
                            };
            }
            /*
            public FilledRectangle2(int __x1, int __y1, int __x2, int __y2, int __x3, int __y3)
            {
                x1 = __x1;
                y1 = __y1;
                x2 = __x2;
                y2 = __y2;
                x3 = __x3;
                y3 = __y3;
            }
            */

            public new Cm command
            {
                get { return _command; }
                set
                {
                    if (value != Cm.IC_CM_FR2) throw new Exception();

                    _command = value;
                    RaisePropertyChanged(() => command);

                }
            }
            public int x1
            {
                get
                {
                    return _data.Where(x => x.Name == "x1").First().Value;
                }
            }
            public int y1
            {
                get
                {
                    return _data.Where(x => x.Name == "y1").First().Value;
                }
            }
            public int x2
            {
                get
                {
                    return _data.Where(x => x.Name == "x2").First().Value;
                }
            }
            public int y2
            {
                get
                {
                    return _data.Where(x => x.Name == "y2").First().Value;
                }
            }
            public int x3
            {
                get
                {
                    return _data.Where(x => x.Name == "x3").First().Value;
                }
            }
            public int y3
            {
                get
                {
                    return _data.Where(x => x.Name == "y3").First().Value;
                }
            }
        }
        public class FilledTriangle : FilledRectangle2
        {
            public FilledTriangle()
            {
                _data = new BindingList<IconShapeData>(){
                            new IconShapeData() { Name = "x1", Value = 0 },
                            new IconShapeData() { Name = "y1", Value = 0 },
                            new IconShapeData() { Name = "x2", Value = 0 },
                            new IconShapeData() { Name = "y2", Value = 0 },
                            new IconShapeData() { Name = "x3", Value = 0 },
                            new IconShapeData() { Name = "y3", Value = 0 },
                            };
            }
            /*
            public FilledTriangle(int __x1, int __y1, int __x2, int __y2, int __x3, int __y3)
            {
                x1 = __x1;
                y1 = __y1;
                x2 = __x2;
                y2 = __y2;
                x3 = __x3;
                y3 = __y3;
            }
            */
            public new Cm command
            {
                get { return _command; }
                set
                {
                    if (value != Cm.IC_CM_FT) throw new Exception();

                    _command = value;
                    RaisePropertyChanged(() => command);

                }
            }
        }
        public class FilledCircle : IconShape
        {
            public override void AddData(int d)
            {
                switch (_addDataCounter)
                {
                    case 0: _data[_addDataCounter] = new IconShapeData() { Name = "x", Value = d }; break;
                    case 1: _data[_addDataCounter] = new IconShapeData() { Name = "y", Value = d }; break;
                    case 2: _data[_addDataCounter] = new IconShapeData() { Name = "radius", Value = d }; break;
                }
                _addDataCounter++;
            }

            public FilledCircle()
            {
                _data = new BindingList<IconShapeData>(){
                            new IconShapeData() { Name = "x", Value = 0 },
                            new IconShapeData() { Name = "y", Value = 0 },
                            new IconShapeData() { Name = "radius", Value = 0},
                            };
            }
            /*
            public FilledCircle(int __x, int __y, int __radius)
            {
                _data = new BindingList<IconShapeData>() {
                            new IconShapeData() { Name = "x", Value = __x },
                            new IconShapeData() { Name = "y", Value = __y },
                            new IconShapeData() { Name = "radius", Value = __radius},
                            };

            }
            */
            public new Cm command
            {
                get { return _command; }
                set
                {
                    if (value != Cm.IC_CM_FC) throw new Exception();

                    _command = value;
                    RaisePropertyChanged(() => command);

                }
            }

            public int x
            {
                get
                {
                    return _data.Where(x => x.Name == "x").First().Value;
                }
            }

            public int y
            {
                get
                {
                    return _data.Where(x => x.Name == "y").First().Value;
                }
            }

            public int radius
            {
                get
                {
                    return _data.Where(x => x.Name == "radius").First().Value;
                }
            }
        }
        public class CopyIcon : IconShape
        {
            public override void AddData(int d)
            {
                switch (_addDataCounter)
                {
                    case 0: _data[_addDataCounter] = new IconShapeData() { Name = "iconId", Value = d }; break;
                    case 1: _data[_addDataCounter] = new IconShapeData() { Name = "offsetX", Value = d }; break;
                    case 2: _data[_addDataCounter] = new IconShapeData() { Name = "offsetY", Value = d }; break;
                }
                _addDataCounter++;
            }

            public CopyIcon()
            {
                _data = new BindingList<IconShapeData>(){
                            new IconShapeData() { Name = "iconId", Value = 0 },
                            new IconShapeData() { Name = "offsetX", Value = 0 },
                            new IconShapeData() { Name = "offsetY", Value = 0 },
                            };
            }
            public new Cm command
            {
                get { return _command; }
                set
                {
                    if (value != Cm.IC_CM_CP) throw new Exception();

                    _command = value;
                    RaisePropertyChanged(() => command);

                }
            }
            public int iconId
            {
                get
                {
                    return _data.Where(x => x.Name == "iconId").First().Value;
                }
            }
            public int offsetX
            {
                get
                {
                    return _data.Where(x => x.Name == "offsetX").First().Value;
                }
            }
            public int offsetY
            {
                get
                {
                    return _data.Where(x => x.Name == "offsetY").First().Value;
                }
            }
        }
        public class NoIcon : IconShape
        {
            public NoIcon()
            {
                _data = new BindingList<IconShapeData>() { };
            }


            public new Cm command
            {
                get { return _command; }
                set
                {
                    if (value != Cm.IC_CM_NO) throw new Exception();

                    _command = value;
                    RaisePropertyChanged(() => command);

                }
            }
        }




        public class IconShape : NotificationObject
        {
            public IconShape()
            {

            }
            public IconShape(Cl __Color, Cm __command, BindingList<IconShapeData> __data, bool __IsHighlighted, Icon __parent)
            {
                Color = __Color;
                command = __command;
                data = __data;
                parent = __parent;
                IsHighlighted = __IsHighlighted;
            }

            virtual public void AddData(int data)
            {
            }
            protected int _addDataCounter = 0;

            private Icon _parent;
            public Icon parent
            {
                get { return _parent; }
                set
                {
                    _parent = value;
                    RaisePropertyChanged(() => parent);
                }
            }
            protected Cm _command;
            virtual public Cm command
            {
                get { return _command; }
                set
                {
                    _command = value;
                    RaisePropertyChanged(() => command);
                }
            }
            private Cl _Color;
            public Cl Color
            {
                get { return _Color; }
                set
                {
                    _Color = value;
                    RaisePropertyChanged(() => Color);
                    //  this.parent.RefreshIcon();
                }
            }
            public bool _IsHighlighted;
            public bool IsHighlighted
            {
                get { return _IsHighlighted; }
                set
                {
                    _IsHighlighted = value;
                    RaisePropertyChanged(() => IsHighlighted);
                    if (this.parent != null)
                        this.parent.RefreshIcon();
                }
            }

            protected BindingList<IconShapeData> _data;
            public BindingList<IconShapeData> data
            {
                get
                {
                    return _data;
                }
                set
                {
                    _data = value;
                    RaisePropertyChanged(() => data);
                }
            }
            public void DrawShapeOnBitmap(int x, int y, Color c1, Color c2, ref WriteableBitmap _wrBmp)
            {
                WriteableBitmap _wrBmp2 = _wrBmp;
                Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        var shape = this;
                        foreach (var _wrB in (new List<WriteableBitmap>() { _wrBmp2, _Bitmap }).Where(xb => xb != null).ToList())
                        {
                            switch (shape.command)
                            {
                                case 0: break; // no previus command (first run) or empty definition
                                case Cm.IC_CM_FRR:
                                    var x1 = x + ((FilledRoundedRectangle)shape).x;
                                    var y1 = y + ((FilledRoundedRectangle)shape).y;
                                    var x2 = ((FilledRoundedRectangle)shape).x + ((FilledRoundedRectangle)shape).width;
                                    var y2 = ((FilledRoundedRectangle)shape).y + ((FilledRoundedRectangle)shape).height;
                                    var r = ((FilledRoundedRectangle)shape).rndRadius;

                                    _wrB.FillRectangle(x1, y1, x2, y2, c1);

                                    // cout out the corners
                                    _wrB.FillRectangle(x1, y1, x1 + r, y1 + r, c2);
                                    _wrB.FillRectangle(x2 - r, y1, x2, y1 + r, c2);
                                    _wrB.FillRectangle(x1, y2 - r, x1 + r, y2, c2);
                                    _wrB.FillRectangle(x2 - r, y2 - r, x2, y2, c2);

                                    // draw the rounded corners
                                    _wrB.FillEllipseCentered(x1 + r, y1 + r, r - 1, r - 1, c1);
                                    _wrB.FillEllipseCentered(x2 - r, y1 + r, r - 1, r - 1, c1);
                                    _wrB.FillEllipseCentered(x1 + r, y2 - r, r - 1, r - 1, c1);
                                    _wrB.FillEllipseCentered(x2 - r, y2 - r, r - 1, r - 1, c1);


                                    break;
                                case Cm.IC_CM_FR: _wrB.FillRectangle(x + ((FilledRectangle)shape).x, y + ((FilledRectangle)shape).y, x + ((FilledRectangle)shape).x + ((FilledRectangle)shape).width, y + ((FilledRectangle)shape).y + ((FilledRectangle)shape).height, c1); break;
                                case Cm.IC_CM_FR2:
                                    int x4 = ((FilledRectangle2)shape).x1 - ((FilledRectangle2)shape).x2 + ((FilledRectangle2)shape).x3; // shape.data[0].Value - shape.data[2].Value + shape.data[4].Value;
                                    int y4 = ((FilledRectangle2)shape).y1 - ((FilledRectangle2)shape).y2 + ((FilledRectangle2)shape).y3; // shape.data[1].Value - shape.data[3].Value + shape.data[5].Value;
                                    _wrB.FillQuad(x + ((FilledRectangle2)shape).x1, y + ((FilledRectangle2)shape).y1, x + ((FilledRectangle2)shape).x2, y + ((FilledRectangle2)shape).y2, x + ((FilledRectangle2)shape).x3, y + ((FilledRectangle2)shape).y3, x4, y4, c1); break;
                                case Cm.IC_CM_FT: _wrB.FillTriangle(x + ((FilledTriangle)shape).x1, y + ((FilledTriangle)shape).y1, x + ((FilledTriangle)shape).x2, y + ((FilledTriangle)shape).y2, x + ((FilledTriangle)shape).x3, y + ((FilledTriangle)shape).y3, c1); break;
                                case Cm.IC_CM_FC: _wrB.FillEllipseCentered(x + ((FilledCircle)shape).x, y + ((FilledCircle)shape).y, ((FilledCircle)shape).radius, ((FilledCircle)shape).radius, c1); break;
                                /* case Cm.IC_CM_FCV: tft.fillCurve(x + data[0].Value, y + data[1].Value, data[2].Value, data[3].Value, data[4].Value, color); break;*/
                                case Cm.IC_CM_CP: this.parent.DrawIconOnBitmap((Ic)((CopyIcon)shape).iconId, _wrB, ((CopyIcon)shape).offsetX, ((CopyIcon)shape).offsetY); break;
                                case Cm.IC_CM_NO: break;/*
                   default: Serial.print(F("!! Udf drw cmd "));
                       Serial.println(String(command)); break;*/
                            }
                        }

                        RefreshIcon();
                    }
                    catch (Exception ex)
                    {

                    }
                }));
                _wrBmp = _wrBmp2;
            }

            public void InitBitmap(Color color)
            {
                _Bitmap = new WriteableBitmap(96, 96, 96d, 96d, PixelFormats.Bgra32, null);
                _Bitmap.FillRectangle(0, 0, 96, 96, color);
            }
            public void RefreshIcon()
            {
                try
                {
                    InitBitmap(this.parent.BackColor);
                    _BitmapStream = new MemoryStream();
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(_Bitmap));
                    encoder.Save(_BitmapStream);
                }
                catch (Exception ex)
                {

                }

                RaisePropertyChanged(() => _Bitmap);
                RaisePropertyChanged(() => FrozenBitmap);

            }

            private WriteableBitmap _Bitmap;
            private MemoryStream _BitmapStream;

            public BitmapImage FrozenBitmap
            {
                get
                {
                    try
                    {
                        if (_BitmapStream == null)
                            DrawShapeOnBitmap(0, 0, this.parent.FrontColor, this.parent.BackColor, ref _Bitmap);

                        var _BitmapFromFile = new BitmapImage();

                        /*
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(_Bitmap));
                        encoder.Save(ms);
                        */
                        _BitmapFromFile.BeginInit();
                        _BitmapFromFile.CacheOption = BitmapCacheOption.OnLoad;
                        _BitmapFromFile.StreamSource = _BitmapStream;
                        _BitmapFromFile.EndInit();
                        _BitmapFromFile.Freeze();


                        // int q = _BitmapFromFile.PixelHeight; // Force file loading now !
                        return _BitmapFromFile;

                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }

        }







        public class IconShapeData : NotificationObject
        {
            private string _Name;
            public string Name
            {
                get { return _Name; }
                set
                {
                    _Name = value;
                    RaisePropertyChanged(() => Name);

                }
            }
            private int _Value;
            public int Value
            {
                get { return _Value; }
                set
                {
                    _Value = value;
                    RaisePropertyChanged(() => Value);

                }
            }
        }






        public static Dictionary<Ic, Tuple<Icon, Icon>> Icons;


        private List<IconShape> _Shapes;
        public BindingList<IconShape> Shapes
        {
            get { return new BindingList<IconShape>(_Shapes); }
            set
            {
                _Shapes = value.ToList();
                RaisePropertyChanged(() => Shapes);
            }
        }

        private String _StringValue;
        public String StringValue
        {
            get { return _StringValue; }
            set
            {
                _StringValue = value;
                LoadIconFromString(value, FrontColor, BackColor, this.FrontColor);
                RaisePropertyChanged(() => StringValue);
            }
        }

        public void LoadIconFromString(string val, Color frontColor, Color backColor, Color SelectedColor)
        {

            try
            {
                DrawingCompleted = false;
                _StringValue = "";

                val = val.Replace(" ", "");
                val = val.Replace("\n", "");
                val = val.Replace("\r", "");
                val = val.Replace("\t", "");
                val = val.Replace("-", "");

                int dataLen = 0;
                var allData = new List<int>();

                for (int i = 0; i < val.Length; i += 2)
                {
                    int readValue = byte.Parse(val.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                    allData.Add(readValue);
                }

                Cm command = Cm.IC_CM_NO;

                foreach (int readValue in allData)
                {
                    if (dataLen != 0)
                    {
                        Shapes.Last().AddData(readValue);
                        dataLen--;

                        _StringValue += " " + string.Format("{0:x2}", readValue);
                    }
                    else if (readValue <= (int)Ic.ICONS_END)
                    {
                        Shapes = new BindingList<IconShape>();
                        IconId = (Ic)readValue;

                        _StringValue += " " + string.Format("{0:x2}", readValue);

                    }
                    else if (readValue == 0xFF) // end byte (when used as command byte)
                    {
                        _StringValue += " " + string.Format("{0:x2}", readValue);
                        break;
                    }
                    else if ((readValue & 0x80) == 0x80)
                    {
                        // command byte
                        command = (Cm)(readValue & 0x3F);

                        switch (command)
                        {
                            case Cm.IC_CM_FRR: Shapes.Add(new FilledRoundedRectangle()); break;
                            case Cm.IC_CM_FR: Shapes.Add(new FilledRectangle()); break;
                            case Cm.IC_CM_FR2: Shapes.Add(new FilledRectangle2()); break;
                            case Cm.IC_CM_FT: Shapes.Add(new FilledTriangle()); break;
                            case Cm.IC_CM_FC: Shapes.Add(new FilledCircle()); break;
                            case Cm.IC_CM_FCV: Shapes.Add(new IconShape()); break;
                            case Cm.IC_CM_CP: Shapes.Add(new CopyIcon()); break;
                            case Cm.IC_CM_NO: Shapes.Add(new NoIcon()); break;
                            default: throw new Exception("!! Udf g cmd: " + (readValue & 0x3F));
                        }

                        Shapes.Last().command = (Cm)(readValue & 0x3F);
                        Shapes.Last().Color = (Cl)(readValue & 192);
                        Shapes.Last().parent = this;

                        switch (command)
                        {
                            case Cm.IC_CM_FRR: dataLen = 5; break;
                            case Cm.IC_CM_FR: dataLen = 4; break;
                            case Cm.IC_CM_FR2: dataLen = 6; break;
                            case Cm.IC_CM_FT: dataLen = 6; break;
                            case Cm.IC_CM_FC: dataLen = 3; break;
                            case Cm.IC_CM_FCV: dataLen = 5; break;
                            case Cm.IC_CM_CP: dataLen = 3; break;
                            case Cm.IC_CM_NO: break;
                            default: throw new Exception("!! Udf g cmd: " + (readValue & 0x3F));
                        }

                        _StringValue += " - " + string.Format("{0:x2}", readValue);
                    }
                }
                BackColor = backColor;
                FrontColor = frontColor;

                RaisePropertyChanged(() => StringValue);

                RefreshIcon();

            }
            catch (Exception ex)
            {
            }
            DrawingCompleted = true;
        }

        private static bool DrawingCompleted = false;
        public static Dictionary<Ic, Tuple<Icon, Icon>> LoadAllFromBigString(string val, Color frontColor, Color backColor, Color SelectedColor)
        {
            var Icons = new Dictionary<Ic, Tuple<Icon, Icon>>();
            try
            {
                DrawingCompleted = false;

                Icon currentIcon1 = new Icon();
                Icon currentIcon2 = new Icon();

                val = val.Replace(" ", "");
                val = val.Replace("\n", "");
                val = val.Replace("\r", "");
                val = val.Replace("\t", "");
                val = val.Replace("-", "");

                int dataLen = 0;
                var allData = new List<int>();

                for (int i = 0; i < val.Length; i += 2)
                {
                    int readValue = byte.Parse(val.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                    allData.Add(readValue);
                }

                Cm command = Cm.IC_CM_NO;

                foreach (int readValue in allData)
                {
                    if (dataLen != 0)
                    {
                        currentIcon1.Shapes.Last().AddData(readValue);
                        currentIcon2.Shapes.Last().AddData(readValue);
                        dataLen--;

                        currentIcon1._StringValue += " " + string.Format("{0:x2}", readValue);
                        currentIcon2._StringValue += " " + string.Format("{0:x2}", readValue);
                    }
                    else if (readValue <= (int)Ic.ICONS_END)
                    {
                        currentIcon1 = new Icon()
                        {
                            Shapes = new BindingList<IconShape>(),
                            IconId = (Ic)readValue
                        };

                        currentIcon2 = new Icon()
                        {
                            Shapes = new BindingList<IconShape>(),
                            IconId = (Ic)readValue
                        };

                        currentIcon1._StringValue += " " + string.Format("{0:x2}", readValue);
                        currentIcon2._StringValue += " " + string.Format("{0:x2}", readValue);

                        Icons.Add((Ic)readValue, new Tuple<Icon, Icon>(currentIcon1, currentIcon2));
                    }
                    else if (readValue == 0xFF) // end byte (when used as command byte)
                    {
                        currentIcon1._StringValue += " " + string.Format("{0:x2}", readValue);
                        currentIcon2._StringValue += " " + string.Format("{0:x2}", readValue);
                        break;
                    }
                    else if ((readValue & 0x80) == 0x80)
                    {
                        // command byte
                        command = (Cm)(readValue & 0x3F);

                        switch (command)
                        {
                            case Cm.IC_CM_FRR: currentIcon1.Shapes.Add(new FilledRoundedRectangle()); currentIcon2.Shapes.Add(new FilledRoundedRectangle()); break;
                            case Cm.IC_CM_FR: currentIcon1.Shapes.Add(new FilledRectangle()); currentIcon2.Shapes.Add(new FilledRectangle()); break;
                            case Cm.IC_CM_FR2: currentIcon1.Shapes.Add(new FilledRectangle2()); currentIcon2.Shapes.Add(new FilledRectangle2()); break;
                            case Cm.IC_CM_FT: currentIcon1.Shapes.Add(new FilledTriangle()); currentIcon2.Shapes.Add(new FilledTriangle()); break;
                            case Cm.IC_CM_FC: currentIcon1.Shapes.Add(new FilledCircle()); currentIcon2.Shapes.Add(new FilledCircle()); break;
                            case Cm.IC_CM_FCV: currentIcon1.Shapes.Add(new IconShape()); currentIcon2.Shapes.Add(new IconShape()); break;
                            case Cm.IC_CM_CP: currentIcon1.Shapes.Add(new CopyIcon()); currentIcon2.Shapes.Add(new CopyIcon()); break;
                            case Cm.IC_CM_NO: currentIcon1.Shapes.Add(new NoIcon()); currentIcon2.Shapes.Add(new NoIcon()); break;
                            default: throw new Exception("!! Udf g cmd: " + (readValue & 0x3F));
                        }

                        currentIcon1.Shapes.Last().command = (Cm)(readValue & 0x3F);
                        currentIcon1.Shapes.Last().Color = (Cl)(readValue & 192);
                        currentIcon1.Shapes.Last().parent = currentIcon1;

                        currentIcon2.Shapes.Last().command = (Cm)(readValue & 0x3F);
                        currentIcon2.Shapes.Last().Color = (Cl)(readValue & 192);
                        currentIcon2.Shapes.Last().parent = currentIcon2;

                        switch (command)
                        {
                            case Cm.IC_CM_FRR: dataLen = 5; break;
                            case Cm.IC_CM_FR: dataLen = 4; break;
                            case Cm.IC_CM_FR2: dataLen = 6; break;
                            case Cm.IC_CM_FT: dataLen = 6; break;
                            case Cm.IC_CM_FC: dataLen = 3; break;
                            case Cm.IC_CM_FCV: dataLen = 5; break;
                            case Cm.IC_CM_CP: dataLen = 3; break;
                            case Cm.IC_CM_NO: break;
                            default: throw new Exception("!! Udf g cmd: " + (readValue & 0x3F));
                        }

                        currentIcon1._StringValue += " - " + string.Format("{0:x2}", readValue);
                        currentIcon2._StringValue += " - " + string.Format("{0:x2}", readValue);
                    }
                }
                foreach (var ic in Icons)
                {
                    ic.Value.Item1.BackColor = backColor;
                    ic.Value.Item1.FrontColor = frontColor;
                    ic.Value.Item1.IsSelected = false;

                    ic.Value.Item2.BackColor = backColor;
                    ic.Value.Item2.FrontColor = SelectedColor;
                    ic.Value.Item2.IsSelected = true;

                    ic.Value.Item1.StringValue += " ";
                    ic.Value.Item2.StringValue += " ";
                }

            }
            catch (Exception ex)
            {
            }

            DrawingCompleted = true;
            return new Dictionary<Ic, Tuple<Icon, Icon>>(Icons);
        }

        public static bool ThumbnailsCreated = false;
        public static void RefreshAllIcons()
        {
            ThumbnailsCreated = false;
            foreach (var icon in Icon.Icons.Values)
            {
                icon.Item1.RefreshIcon();
                icon.Item2.RefreshIcon();
            }

            ThumbnailsCreated = true;
        }
        public static void SetIconFrontColor(Color frontColor)
        {
            foreach (var icon in Icon.Icons.Values)
            {
                icon.Item1.FrontColor = frontColor;
                icon.Item1.RefreshIcon();
            }
        }
        public static void SetIconSelectedColor(Color SelectedColor)
        {
            foreach (var icon in Icon.Icons.Values)
            {
                icon.Item2.FrontColor = SelectedColor;
                icon.Item2.RefreshIcon();
            }
        }

        public void RefreshIcon()
        {
            if (DrawingCompleted)
            {
                var _Bitmap = new WriteableBitmap(96, 96, 96d, 96d, PixelFormats.Bgra32, null);
                _Bitmap.FillRectangle(0, 0, 96, 96, BackColor);

                DrawIconOnBitmap(IconId, ref _Bitmap);

                var _BitmapStream = new MemoryStream();
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(_Bitmap));
                encoder.Save(_BitmapStream);

                var _BitmapFromFile = new BitmapImage();

                _BitmapFromFile.BeginInit();
                _BitmapFromFile.CacheOption = BitmapCacheOption.None;
                _BitmapFromFile.StreamSource = _BitmapStream;
                _BitmapFromFile.EndInit();
                _BitmapFromFile.Freeze();

                // int q = _BitmapFromFile.PixelHeight; // Force file loading now !
                _FrozenBitmap = _BitmapFromFile;

                RaisePropertyChanged(() => FrozenBitmap);
            }
        }

        public Icon()
        {
        }


        private BitmapImage _FrozenBitmap;
        public BitmapImage FrozenBitmap
        {
            get
            {
                try
                {
                    return _FrozenBitmap;

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        private Color _FrontColor;
        public Color FrontColor
        {
            get { return _FrontColor; }
            set
            {
                _FrontColor = value;
                RaisePropertyChanged(() => FrontColor);
            }
        }
        private Color _BackColor;
        public Color BackColor
        {
            get { return _BackColor; }
            set
            {
                _BackColor = value;
                RaisePropertyChanged(() => BackColor);
            }
        }
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }
        private Ic _IconId;
        public Ic IconId
        {
            get { return _IconId; }
            set
            {
                _IconId = value;
                RaisePropertyChanged(() => IconId);
                RaisePropertyChanged(() => FrozenBitmap);

            }
        }

        public void DrawIconOnBitmap(Ic ic, WriteableBitmap _wrB, int offsetX = 0, int offsetY = 0)
        {
            DrawIconOnBitmap(ic, ref _wrB, offsetX, offsetY);
        }
        public void DrawIconOnBitmap(Ic ic, ref WriteableBitmap _wrB, int offsetX = 0, int offsetY = 0)
        {
            if (offsetX > 127) offsetX -= 256;
            if (offsetY > 127) offsetY -= 256;

            int x = offsetX;
            int y = offsetY;
            var s = Icons[ic].Item1.Shapes;
            if (IsSelected)
                s = Icons[ic].Item2.Shapes;

            foreach (var shape in s)
            {
                Color c1 = (shape.Color == Cl.IC_BC) ? BackColor : FrontColor;
                Color c2 = (shape.Color == Cl.IC_BC) ? FrontColor : BackColor;

                if (shape.IsHighlighted)
                {
                    c1 = Colors.Blue;
                }

                shape.DrawShapeOnBitmap(x, y, c1, c2, ref _wrB);
            }
        }

    }
}
