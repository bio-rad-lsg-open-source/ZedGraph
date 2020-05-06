//============================================================================
//ZedGraph Class Library - A Flexible Line Graph/Bar Graph Library in C#
//Copyright © 2004  John Champion
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Diagnostics;
using System.Collections.Generic;

namespace ZedGraph
{
	/// <summary>
	/// Encapsulates a bar type that displays vertical or horizontal bars
	/// </summary>
	/// <remarks>
	/// The orientation of the bars depends on the state of
	/// <see cref="BarSettings.Base"/>, and the bars can be stacked or
	/// clustered, depending on the state of <see cref="BarSettings.Type"/>
	/// </remarks>
	/// <author> John Champion
	/// Revised by BioRad</author>
	/// <version> $Revision: 8 $ $Date: 5/14/10 11:20a $ </version>
	[Serializable]
	public class BarItem : CurveItem, ICloneable, ISerializable
	{
		/// <summary>
		/// class to contain info returned from methods which find closest bar to a certain point.
		/// </summary>
		public class ClosestBarInfo
		{
			/// <summary>
			/// zero based index of the bar which is closest.  -1 for none.
			/// </summary>
			public int BarIndex;
			/// <summary>
			/// distance from point of the closest bar.
			/// </summary>
			public float Distance;
		}
		#region Fields
		/// <summary>
		/// Private field that stores a reference to the <see cref="ZedGraph.Bar"/>
		/// class defined for this <see cref="BarItem"/>.  Use the public
		/// property <see cref="Bar"/> to access this value.
		/// </summary>
		protected Bar _bar;

		// added by BioRad 2007.4.9
		/// <summary>
		/// error bar (absolute) values
		/// </summary>
		protected double[] _errorBarValues;
		/// <summary>
		/// Whether to show error bars
		/// </summary>
		protected bool _showErrorBars;
		// end BioRad addition
		#endregion

		#region Properties
		/// <summary>
		/// Gets a reference to the <see cref="ZedGraph.Bar"/> class defined
		/// for this <see cref="BarItem"/>.
		/// </summary>
		public Bar Bar
		{
			get { return _bar; }
		}
		// Added by BioRad 2007.4.10
		/// <summary>
		/// error bar values coinciding with the bar graphs.
		/// </summary>
		public double[] ErrorBarValues
		{
			get { return _errorBarValues; }
		}
		/// <summary>
		/// Whether to show the error bars.
		/// </summary>
		public bool ShowErrorBars
		{
			get { return _showErrorBars; }
			set { _showErrorBars = value; }
		}
		// End BioRad addition.

		/// <summary>
		/// Gets a flag indicating if the Z data range should be included in the axis scaling calculations.
		/// </summary>
		/// <param name="pane">The parent <see cref="GraphPane" /> of this <see cref="CurveItem" />.
		/// </param>
		/// <value>true if the Z data are included, false otherwise</value>
		override internal bool IsZIncluded(GraphPane pane)
		{
			return pane._barSettings.Type == BarType.ClusterHiLow;
		}

		/// <summary>
		/// Gets a flag indicating if the X axis is the independent axis for this <see cref="CurveItem" />
		/// </summary>
		/// <param name="pane">The parent <see cref="GraphPane" /> of this <see cref="CurveItem" />.
		/// </param>
		/// <value>true if the X axis is independent, false otherwise</value>
		override internal bool IsXIndependent(GraphPane pane)
		{
			return pane._barSettings.Base == BarBase.X;
		}

		#endregion

		#region Constructors
		/// <summary>
		/// Create a new <see cref="BarItem"/>, specifying only the legend label for the bar.
		/// </summary>
		/// <param name="label">The label that will appear in the legend.</param>
		public BarItem(string label)
			: base(label)
		{
			_bar = new Bar();

			// Added by BioRad 2007.4.10
			_errorBarValues = null;
		}
		/// <summary>
		/// Create a new <see cref="BarItem"/> using the specified properties.
		/// </summary>
		/// <param name="label">The label that will appear in the legend.</param>
		/// <param name="x">An array of double precision values that define
		/// the independent (X axis) values for this curve</param>
		/// <param name="y">An array of double precision values that define
		/// the dependent (Y axis) values for this curve</param>
		/// <param name="color">A <see cref="Color"/> value that will be applied to
		/// the <see cref="ZedGraph.Bar.Fill"/> and <see cref="ZedGraph.Bar.Border"/> properties.
		/// </param>
		public BarItem(string label, double[] x, double[] y, Color color)
			: this(label, new PointPairList(x, y), color)
		{
		}

		/// <summary>
		/// Create a new <see cref="BarItem"/> using the specified properties.
		/// </summary>
		/// <param name="label">The label that will appear in the legend.</param>
		/// <param name="points">A <see cref="IPointList"/> of double precision value pairs that define
		/// the X and Y values for this curve</param>
		/// <param name="color">A <see cref="Color"/> value that will be applied to
		/// the <see cref="ZedGraph.Bar.Fill"/> and <see cref="ZedGraph.Bar.Border"/> properties.
		/// </param>
		public BarItem(string label, IPointList points, Color color)
			: base(label, points)
		{
			_bar = new Bar(color);
			// Added by BioRad 2007.4.10
			_errorBarValues = new double[points.Count];
			for (int i = 0; i < points.Count; i += 1)
				_errorBarValues[i] = (0);
			_showErrorBars = false;
			// End BioRad addition
		}

		// Added by BioRad 2007.4.10
		/// <summary>
		/// Create a new <see cref="BarItem"/> using the specified properties.
		/// </summary>
		/// <param name="label">The label that will appear in the legend.</param>
		/// <param name="points">A <see cref="IPointList"/> of double precision value pairs that define
		/// the X and Y values for this curve</param>
		/// <param name="errorValues">error bar values, which will be displayed at the "top" (as in opposite end
		/// from the base) of the bar graph.</param>
		/// <param name="color">A <see cref="Color"/> value that will be applied to
		/// the <see cref="ZedGraph.Bar.Fill"/> and <see cref="ZedGraph.Bar.Border"/> properties.
		/// </param>
		public BarItem(string label, IPointList points, double[] errorValues, Color color)
			: base(label, points)
		{
			_bar = new Bar(color);
			Debug.Assert(errorValues.Length == points.Count);
			_errorBarValues = new double[errorValues.Length];
			errorValues.CopyTo(_errorBarValues, 0);
			_showErrorBars = true;
		}
		// End BioRad addition

		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The <see cref="BarItem"/> object from which to copy</param>
		public BarItem(BarItem rhs)
			: base(rhs)
		{
			//bar = new Bar( rhs.Bar );
			_bar = rhs._bar.Clone();
			_errorBarValues = new double[rhs._errorBarValues.Length];
			rhs._errorBarValues.CopyTo(_errorBarValues, 0);
			ShowErrorBars = rhs.ShowErrorBars;
		}

		/// <summary>
		/// Implement the <see cref="ICloneable" /> interface in a typesafe manner by just
		/// calling the typed version of <see cref="Clone" />
		/// </summary>
		/// <returns>A deep copy of this object</returns>
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>
		/// Typesafe, deep-copy clone method.
		/// </summary>
		/// <returns>A new, independent copy of this class</returns>
		public BarItem Clone()
		{
			return new BarItem(this);
		}

		#endregion

		#region Serialization
		/// <summary>
		/// Current schema value that defines the version of the serialized file
		/// </summary>
		public const int schema2 = 10;

		/// <summary>
		/// Constructor for deserializing objects
		/// </summary>
		/// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
		/// </param>
		/// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
		/// </param>
		protected BarItem(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			// The schema value is just a file version parameter.  You can use it to make future versions
			// backwards compatible as new member variables are added to classes
			int sch = info.GetInt32("schema2");

			_bar = (Bar)info.GetValue("bar", typeof(Bar));
		}
		/// <summary>
		/// Populates a <see cref="SerializationInfo"/> instance with the data needed to serialize the target object
		/// </summary>
		/// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data</param>
		/// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data</param>
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("schema2", schema2);
			info.AddValue("bar", _bar);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Do all rendering associated with this <see cref="BarItem"/> to the specified
		/// <see cref="Graphics"/> device.  This method is normally only
		/// called by the Draw method of the parent <see cref="ZedGraph.CurveList"/>
		/// collection object.
		/// </summary>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="pane">
		/// A reference to the <see cref="ZedGraph.GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="pos">The ordinal position of the current <see cref="Bar"/>
		/// curve.</param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see cref="ZedGraph.GraphPane"/> object using the
		/// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		override public void Draw(Graphics g, GraphPane pane, int pos,
									float scaleFactor)
		{
			// Pass the drawing onto the bar class
			if (_isVisible)
				_bar.DrawBars(g, pane, this, BaseAxis(pane), ValueAxis(pane),
								this.GetBarWidth(pane), pos, scaleFactor);
		}

		/// <summary>
		/// Draw a legend key entry for this <see cref="BarItem"/> at the specified location
		/// </summary>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="pane">
		/// A reference to the <see cref="ZedGraph.GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="rect">The <see cref="RectangleF"/> struct that specifies the
		/// location for the legend key</param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see cref="ZedGraph.GraphPane"/> object using the
		/// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		override public void DrawLegendKey(Graphics g, GraphPane pane, RectangleF rect, float scaleFactor)
		{
			_bar.Draw(g, pane, rect, scaleFactor, true, null, -1);
		}

		/// <summary>
		/// Create a <see cref="TextObj" /> for each bar in the <see cref="GraphPane" />.
		/// </summary>
		/// <remarks>
		/// This method will go through the bars, create a label that corresponds to the bar value,
		/// and place it on the graph depending on user preferences.  This works for horizontal or
		/// vertical bars in clusters or stacks, but only for <see cref="BarItem" /> types.  This method
		/// does not apply to <see cref="ErrorBarItem" /> or <see cref="HiLowBarItem" /> objects.
		/// Call this method only after calling <see cref="GraphPane.AxisChange" />.
		/// </remarks>
		/// <param name="pane">The GraphPane in which to place the text labels.</param>
		/// <param name="isBarCenter">true to center the labels inside the bars, false to
		/// place the labels just above the top of the bar.</param>
		/// <param name="valueFormat">The double.ToString string format to use for creating
		/// the labels.
		/// </param>
		public static void CreateBarLabels(GraphPane pane, bool isBarCenter, string valueFormat)
		{
			bool isVertical = pane.BarSettings.Base == BarBase.X;

			// Make the gap between the bars and the labels = 2% of the axis range
			float labelOffset;
			if (isVertical)
				labelOffset = (float)(pane.YAxis._scale._max - pane.YAxis._scale._min) * 0.015f;
			else
				labelOffset = (float)(pane.XAxis._scale._max - pane.XAxis._scale._min) * 0.015f;

			// keep a count of the number of BarItems
			int curveIndex = 0;

			// Get a valuehandler to do some calculations for us
			ValueHandler valueHandler = new ValueHandler(pane, true);

			// Loop through each curve in the list
			foreach (CurveItem curve in pane.CurveList)
			{
				// work with BarItems only
				BarItem bar = curve as BarItem;
				if (bar != null)
				{
					IPointList points = curve.Points;

					// Loop through each point in the BarItem
					for (int i = 0; i < points.Count; i++)
					{
						// Get the high, low and base values for the current bar
						// note that this method will automatically calculate the "effective"
						// values if the bar is stacked
						double baseVal, lowVal, hiVal;
						valueHandler.GetValues(curve, i, out baseVal, out lowVal, out hiVal);

						// Get the value that corresponds to the center of the bar base
						// This method figures out how the bars are positioned within a cluster
						float centerVal = (float)valueHandler.BarCenterValue(bar,
							bar.GetBarWidth(pane), i, baseVal, curveIndex);

						// Create a text label -- note that we have to go back to the original point
						// data for this, since hiVal and lowVal could be "effective" values from a bar stack
						string barLabelText = (isVertical ? points[i].Y : points[i].X).ToString(valueFormat);

						// Calculate the position of the label -- this is either the X or the Y coordinate
						// depending on whether they are horizontal or vertical bars, respectively
						float position;
						if (isBarCenter)
							position = (float)(hiVal + lowVal) / 2.0f;
						else if (hiVal >= 0)
							position = (float)hiVal + labelOffset;
						else
							position = (float)hiVal - labelOffset;

						// Create the new TextObj
						TextObj label;
						if (isVertical)
							label = new TextObj(barLabelText, centerVal, position);
						else
							label = new TextObj(barLabelText, position, centerVal);

						// Configure the TextObj
						label.Location.CoordinateFrame = CoordType.AxisXYScale;
						label.FontSpec.Size = 12;
						label.FontSpec.FontColor = Color.Black;
						label.FontSpec.Angle = isVertical ? 90 : 0;
						label.Location.AlignH = isBarCenter ? AlignH.Center :
									(hiVal >= 0 ? AlignH.Left : AlignH.Right);
						label.Location.AlignV = AlignV.Center;
						label.FontSpec.Border.IsVisible = false;
						label.FontSpec.Fill.IsVisible = false;

						// Add the TextObj to the GraphPane
						pane.GraphObjList.Add(label);
					}
				}
				curveIndex++;
			}
		}

		// Added by BioRad 2007.4.10 
		/// <summary>
		/// Go through the list of <see cref="PointPair"/> data values for this <see cref="CurveItem"/>
		/// and determine the minimum and maximum values in the data.
		/// </summary>
		/// <param name="xMin">The minimum X value in the range of data</param>
		/// <param name="xMax">The maximum X value in the range of data</param>
		/// <param name="yMin">The minimum Y value in the range of data</param>
		/// <param name="yMax">The maximum Y value in the range of data</param>
		/// <param name="ignoreInitial">ignoreInitial is a boolean value that
		/// affects the data range that is considered for the automatic scale
		/// ranging (see <see cref="GraphPane.IsIgnoreInitial"/>).  If true, then initial
		/// data points where the Y value is zero are not included when
		/// automatically determining the scale <see cref="Scale.Min"/>,
		/// <see cref="Scale.Max"/>, and <see cref="Scale.MajorStep"/> size.  All data after
		/// the first non-zero Y value are included.
		/// </param>
		/// <param name="isBoundedRanges">
		/// Determines if the auto-scaled axis ranges will subset the
		/// data points based on any manually set scale range values.
		/// </param>
		/// <param name="pane">
		/// A reference to the <see cref="GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <seealso cref="GraphPane.IsBoundedRanges"/>
		override public void GetRange(out double xMin, out double xMax,
										out double yMin, out double yMax,
										bool ignoreInitial,
										bool isBoundedRanges,
										GraphPane pane)
		{
			// The lower and upper bounds of allowable data for the X values.  These
			// values allow you to subset the data values.  If the X range is bounded, then
			// the resulting range for Y will reflect the Y values for the points within the X
			// bounds.
			double xLBound = double.MinValue;
			double xUBound = double.MaxValue;
			double yLBound = double.MinValue;
			double yUBound = double.MaxValue;

			// initialize the values to outrageous ones to start
			xMin = yMin = Double.MaxValue;
			xMax = yMax = Double.MinValue;

			Axis yAxis = this.GetYAxis(pane);
			if (yAxis == null)
				return;

			if (isBoundedRanges)
			{
				xLBound = pane.XAxis._scale._lBound;
				xUBound = pane.XAxis._scale._uBound;
				yLBound = yAxis._scale._lBound;
				yUBound = yAxis._scale._uBound;
			}

			bool isZIncluded = this.IsZIncluded(pane);
			bool isXIndependent = this.IsXIndependent(pane);
			bool isXLog = pane.XAxis.Scale.IsLog;
			bool isYLog = yAxis.Scale.IsLog;

			// Loop over each point in the arrays
			//foreach ( PointPair point in this.Points )
			for (int i = 0; i < this.Points.Count; i++)
			{
				IPointPair point = this.Points[i];

				double curX = point.X;
				double curY = point.Y;
				double curZ = point.Z;

				double errorRadiusHighX = 0;
				double errorRadiusLowX = 0;
				double errorRadiusHighY = 0;
				double errorRadiusLowY = 0;
				if (ShowErrorBars)
				{
					double errorRadius = ErrorBarValues[i];
					if (pane._barSettings.Base == BarBase.X)
					{
						GetErrorRadii(ValueAxis(pane).Scale, curY, errorRadius, out errorRadiusHighY, out errorRadiusLowY);
					}
					else if (pane._barSettings.Base == BarBase.Y || pane._barSettings.Base == BarBase.Y2)
					{
						GetErrorRadii(ValueAxis(pane).Scale, curX, errorRadius, out errorRadiusHighX, out errorRadiusLowX);
					}
				}

				bool outOfBounds =
					curX - errorRadiusLowX < xLBound ||
					curX + errorRadiusHighX > xUBound ||
					curY - errorRadiusLowY < yLBound ||
					curY + errorRadiusHighY > yUBound ||
					(isZIncluded && isXIndependent && (curZ < yLBound || curZ > yUBound)) ||
					(isZIncluded && !isXIndependent && (curZ < xLBound || curZ > xUBound))
					// BioRad change: removed this. (and re-added)
					|| (curX - errorRadiusLowX <= 0 && isXLog) || (curY - errorRadiusLowY <= 0 && isYLog)
					// End BioRad Change.
				;
				// ignoreInitial becomes false at the first non-zero
				// Y value
				if (ignoreInitial && curY != 0 &&
						curY != PointPair.Missing)
					ignoreInitial = false;

				if (!ignoreInitial &&
						!outOfBounds &&
						curX != PointPair.Missing &&
						curY != PointPair.Missing)
				{
					if (curX - errorRadiusLowX < xMin)
						xMin = curX - errorRadiusLowX;
					if (curX + errorRadiusHighX > xMax)
						xMax = curX + errorRadiusHighX;
					if (curY - errorRadiusLowY < yMin)
						yMin = curY - errorRadiusLowY;
					if (curY + errorRadiusHighY > yMax)
						yMax = curY + errorRadiusHighY;

					if (isZIncluded && isXIndependent && curZ != PointPair.Missing)
					{
						if (curZ < yMin)
							yMin = curZ;
						if (curZ > yMax)
							yMax = curZ;
					}
					else if (isZIncluded && curZ != PointPair.Missing)
					{
						if (curZ < xMin)
							xMin = curZ;
						if (curZ > xMax)
							xMax = curZ;
					}
				}
			}
		}
		// End BioRad addition.

		// Added by BioRad 7.26.2007
		/// <summary>
		/// compute two visually symmetrical error radii (low and high) given a desired linearly symmetrical 
		/// error radius.  For log scales, the calculated visually symmetrical error radii will be unsymmetrical
		/// linearly.
		/// </summary>
		/// <param name="scale">scale this baritem belongs to</param>
		/// <param name="barValue">value (height) of this bar</param>
		/// <param name="desiredRadius">desired linear error radius</param>
		/// <param name="errorRadiusHighX">calculated visually symmetric error radius in the + direction</param>
		/// <param name="errorRadiusLowX">calculated visually symmetric error radius in the - direction</param>
		internal void GetErrorRadii(
				Scale scale,
				double barValue,
				double desiredRadius, 
				out double errorRadiusHighX, 
				out double errorRadiusLowX)
		{
			if (scale.IsLog == false)
			{
				errorRadiusHighX = desiredRadius;
				errorRadiusLowX = desiredRadius;
			}
			else
			{
				double centerLog, lowLog, highLog;
				double logBase = 10;
				if (scale.Type == AxisType.Log)
					logBase = 10;
				else if (scale.Type == AxisType.Log2)
					logBase = 2;
				else
					Debug.Assert(false, "unknown log type");

				centerLog = scale.SafeLog(barValue, logBase);
				highLog = scale.SafeLog(barValue + desiredRadius, logBase);
				lowLog = scale.SafeLog(barValue - desiredRadius, logBase);
				if (lowLog == 0)
					lowLog = double.MinValue;
				double highDifference = Math.Abs(highLog - centerLog);
				double lowDifference = Math.Abs(centerLog - lowLog);
				if (highDifference < lowDifference)
					lowLog = centerLog - highDifference;
				else
					highLog = centerLog + lowDifference;

				double lowLinear = Math.Pow(logBase, lowLog);
				double highLinear = Math.Pow(logBase, highLog);

				errorRadiusHighX = Math.Abs(highLinear - barValue);
				errorRadiusLowX = Math.Abs(barValue - lowLinear);
			}
		}
		// End biorad addition

		/// <summary>
		/// Determine the coords for the rectangle associated with a specified point for 
		/// this <see cref="CurveItem" />
		/// </summary>
		/// <param name="pane">The <see cref="GraphPane" /> to which this curve belongs</param>
		/// <param name="i">The index of the point of interest</param>
		/// <param name="coords">A list of coordinates that represents the "rect" for
		/// this point (used in an html AREA tag)</param>
		/// <returns>true if it's a valid point, false otherwise</returns>
		override public bool GetCoords(GraphPane pane, int i, out string coords)
		{
			coords = string.Empty;

			if (i < 0 || i >= _points.Count)
				return false;

			Axis valueAxis = ValueAxis(pane);
			Axis baseAxis = BaseAxis(pane);

			// pixBase = pixel value for the bar center on the base axis
			// pixHiVal = pixel value for the bar top on the value axis
			// pixLowVal = pixel value for the bar bottom on the value axis
			float pixBase, pixHiVal, pixLowVal;

			float clusterWidth = pane.BarSettings.GetClusterWidth();
			float barWidth = GetBarWidth(pane);
			float clusterGap = pane._barSettings.MinClusterGap * barWidth;
			float barGap = barWidth * pane._barSettings.MinBarGap;

			// curBase = the scale value on the base axis of the current bar
			// curHiVal = the scale value on the value axis of the current bar
			// curLowVal = the scale value of the bottom of the bar
			double curBase, curLowVal, curHiVal;
			ValueHandler valueHandler = new ValueHandler(pane, false);
			valueHandler.GetValues(this, i, out curBase, out curLowVal, out curHiVal);

			// Any value set to double max is invalid and should be skipped
			// This is used for calculated values that are out of range, divide
			//   by zero, etc.
			// Also, any value <= zero on a log scale is invalid

			if (!_points[i].IsInvalid3D)
			{
				// calculate a pixel value for the top of the bar on value axis
				pixLowVal = valueAxis.Scale.Transform(_isOverrideOrdinal, i, curLowVal);
				pixHiVal = valueAxis.Scale.Transform(_isOverrideOrdinal, i, curHiVal);
				// calculate a pixel value for the center of the bar on the base axis
				pixBase = baseAxis.Scale.Transform(_isOverrideOrdinal, i, curBase);

				// Calculate the pixel location for the side of the bar (on the base axis)
				float pixSide = pixBase - clusterWidth / 2.0F + clusterGap / 2.0F +
								pane.CurveList.GetBarItemPos(pane, this) * (barWidth + barGap);

				// Draw the bar
				if (pane._barSettings.Base == BarBase.X)
					coords = String.Format("{0:f0},{1:f0},{2:f0},{3:f0}",
								pixSide, pixLowVal,
								pixSide + barWidth, pixHiVal);
				else
					coords = String.Format("{0:f0},{1:f0},{2:f0},{3:f0}",
								pixLowVal, pixSide,
								pixHiVal, pixSide + barWidth);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Find the closest bar which is part of this BarItem, from a certain pixel point in client window.
		/// </summary>
		/// <param name="fromPixel">point from which to find the closest bar</param>
		/// <param name="maxDistance">max distance to consider.</param>
		/// <returns>object containing info about the closest bar (if any), or null
		/// if there is none within the given max distance.</returns>
		public ClosestBarInfo FindClosestBar(Point fromPixel, float maxDistance)
		{
			return _bar.FindClosestBar(fromPixel, maxDistance);
		}
		// Added by BioRad 7.11.2007
		/// <summary>
		/// set or reset highlight on a certain bar in this series.
		/// </summary>
		/// <param name="barIndex">the bar index who's highlight to turn off or on.</param>
		/// <param name="highlighted">Whether the bar index will be highlighted.</param>
		public void SetBarIndexHighlighted(int barIndex, bool highlighted)
		{
			if (highlighted)
			{
				if (_bar.HighlightedBarIndexes.Contains(barIndex) == false)
				{
					_bar.HighlightedBarIndexes.Add(barIndex);
					return;
				}
			}
			else
			{
				if (_bar.HighlightedBarIndexes.Contains(barIndex))
				{
					_bar.HighlightedBarIndexes.Remove(barIndex);
					Debug.Assert(_bar.HighlightedBarIndexes.Contains(barIndex) == false,
							"still contains this bar index even after removal.");
				}
			}
		}
		/// <summary>
		/// Removes highlights from all bars in this series.
		/// </summary>
		public void ClearHighlights()
		{
			_bar.HighlightedBarIndexes.Clear();
		}
		// End BioRad addition.
		#endregion
	}
}
