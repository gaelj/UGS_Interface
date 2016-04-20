using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UGS
{
    /// <summary>
    /// Logique d'interaction pour IconEditor.xaml
    /// </summary>
    public partial class IconEditor : Window
    {
        public IconEditor()
        {
            InitializeComponent();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var UGS = ((ViewModels.UGSViewModel)DataContext);
            UGS.ugs.ReloadAllIcons();
        }

        private void HighlightCurrentShape()
        {
            try
            {/*
                if (DatagridShapes.CurrentCell.Item.GetType().BaseType == typeof(Models.Icon.IconShape))
                {
                    var UGS = ((ViewModels.UGSViewModel)DataContext);
                    var curShape = (Models.Icon.IconShape DatagridShapes.CurrentCell.Item;

                    foreach (var s in curShape.parent.Shapes)
                        s.IsHighlighted = false;

                    if (curShape != null)
                        curShape.IsHighlighted = true;

                    curShape.parent.RefreshIcon();
                    UGS.ugs.Icons = UGS.ugs.Icons; // trigger the icon refresh event
                }*/
            }
            catch (Exception ex)
            {

            }
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            HighlightCurrentShape();
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            HighlightCurrentShape();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HighlightCurrentShape();
        }

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            HighlightCurrentShape();
        }


        private void DataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            /*
            var icvs = ((CollectionViewSource)this.FindResource("uGSIconsViewSource"));
            var dv = icvs.View;
            var curi = (Icon)dv.CurrentItem;
            curi.Shapes.Add(new Source.Icon.IconShape() { parent = curi });
            */
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var UGS = ((ViewModels.UGSViewModel)DataContext);
            UGS.ugs.ReloadAllIcons();
        }
    }
}
