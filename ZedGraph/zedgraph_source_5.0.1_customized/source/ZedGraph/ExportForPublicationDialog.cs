using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace ZedGraph
{
	/// <summary></summary>
	public partial class ExportForPublicationDialog : Form
	{
		private MasterPane m_Mp;

		/// <summary></summary>
		/// <param name="mp"></param>
		/// <param name="parent"></param>
		public ExportForPublicationDialog(Form parent, MasterPane mp)
		{
			InitializeComponent();

			Owner = parent;
			m_Mp = mp;
			m_DpiUpDown.Value = 96;
			m_WidthUpDown.Value = (decimal)(GetWidth() / 96.0f);
			m_HeightUpDown.Value = (decimal)(GetHeight() / 96.0f);
		}

		private float GetHeight()
		{
			return m_Mp.Rect.Height;
		}

		private double GetWidth()
		{
			return m_Mp.Rect.Width;
		}

		private void OnSaveButtonClick(object sender, EventArgs e)
		{
			List<Duple<ILineItem, float>> oldThicknesses = new List<Duple<ILineItem, float>>();
			bool exception = false;
			bool oldPenWidthScaled = m_Mp.IsPenWidthScaled;
			try
			{
				float dpi = (float)m_DpiUpDown.Value;
				m_Mp.m_Dpi = dpi;
				if(dpi > 96)
					m_Mp.IsPenWidthScaled = true;
				float widthInUnknownUnits = (float)m_WidthUpDown.Value;
				float heightInUnknownUnits = (float)m_HeightUpDown.Value;
				bool isInInches = m_InchesRadio.Checked;
				float scaleFactor;
				if (isInInches)
					scaleFactor = 1;
				else
					scaleFactor = 0.393700787f;
				float widthInInches = widthInUnknownUnits * scaleFactor;
				float heightInInches = heightInUnknownUnits * scaleFactor;

				Size pixelDimensions = new Size((int)(widthInInches * dpi + 0.5), (int)(heightInInches * dpi + 0.5));
				//int lineWidth = (int)m_LineWidthUpDown.Value;

				//foreach (GraphPane gp in m_Mp.PaneList)
				//{
				//   foreach (CurveItem ci in gp.CurveList)
				//   {
				//      if (ci is ILineItem)
				//      {
				//         float oldThickness = ((ILineItem)ci).Thickness;
				//         if (oldThickness < lineWidth)
				//         {
				//            oldThicknesses.Add(new Duple<ILineItem, float>((ILineItem)ci, oldThickness));
				//            ((ILineItem)ci).Thickness = lineWidth;
				//         }
				//      }
				//   }
				//}

				SaveFileDialog saveDlg = new SaveFileDialog();
				saveDlg.Filter = "PNG Format (*.png)|*.png|" +
					 "Gif Format (*.gif)|*.gif|" +
					 "Jpeg Format (*.jpg)|*.jpg|" +
					 "Tiff Format (*.tif)|*.tif|" +
					 "Bmp Format (*.bmp)|*.bmp";

				if (saveDlg.ShowDialog() != DialogResult.OK)
					return;

				ImageFormat format = ImageFormat.Png;
				if (saveDlg.FilterIndex == 2)
					format = ImageFormat.Gif;
				else if (saveDlg.FilterIndex == 3)
					format = ImageFormat.Jpeg;
				else if (saveDlg.FilterIndex == 4)
					format = ImageFormat.Tiff;
				else if (saveDlg.FilterIndex == 5)
					format = ImageFormat.Bmp;

				Stream myStream = saveDlg.OpenFile();
				if (myStream != null)
				{
					using (Image i = m_Mp.GetImage(pixelDimensions.Width, pixelDimensions.Height, dpi))
					{
						i.Save(myStream, format);
					}
					myStream.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				exception = true;
			}
			finally
			{
				m_Mp.m_Dpi = 72;
				m_Mp.IsPenWidthScaled = oldPenWidthScaled;
				foreach (Duple<ILineItem, float> curveAndThickness in oldThicknesses)
					curveAndThickness.Item1.Thickness = curveAndThickness.Item2;
				if (!exception)
					Close();
			}
		}
	}

	internal class Duple<T1, T2>
	{
		/// <summary>first item in the Duple.</summary>
		private T1 m_Item1;
		/// <summary>second item in the Duple.</summary>
		private T2 m_Item2;

		#region Accessors
		/// <summary>first item in the Duple.</summary>
		public T1 Item1
		{
			get { return m_Item1; }
			set { m_Item1 = value; }
		}
		/// <summary>second item in the Duple.</summary>
		public T2 Item2
		{
			get { return m_Item2; }
			set { m_Item2 = value; }
		}
		#endregion

		#region Constructors
		/// <summary>Constructs a new Duple with the given initial items.</summary>
		/// <param name="item1">initial Item1</param>
		/// <param name="item2">initial Item2</param>
		public Duple(T1 item1, T2 item2)
		{
			m_Item1 = item1;
			m_Item2 = item2;
		}
		#endregion

		#region Methods
		/// <summary>value equals comparer.</summary>
		/// <param name="other">object to compare to</param>
		/// <returns>true if value equals, else false.</returns>
		public override bool Equals(object other)
		{
			Duple<T1, T2> otherDuple = other as Duple<T1, T2>;
			if (otherDuple != null)
				return Item1.Equals(otherDuple.Item1) && Item2.Equals(otherDuple.Item2);
			else
				return base.Equals(other);
		}
		/// <summary>Gets a hash code for this object</summary>
		/// <returns>See summary.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return Item1.GetHashCode() + Item2.GetHashCode();
			}
		}
		#endregion
	}
}