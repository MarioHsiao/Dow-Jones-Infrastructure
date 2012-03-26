/* 
 * Author: Infosys
 * Date: 8/4/09
 * Purpose: Generate Itxml for Bar Chart
 * 
 * 
 * Mod Log
 * -----------------------------------------------------------------------------
 * Modified by                          Date                    Purpose
 * Infosys                              5/1/2010              Modified to implement
 *                                                            Industry Snapshot chart.
 * -----------------------------------------------------------------------------
 */

using System.Drawing;
using System.Text;
using System.Collections;
using DowJones.Charting.Core;
using DowJones.Charting.Core.Data;
using DowJones.Charting.Core.Palettes;
using DowJones.Charting.Core.Response;
using DowJones.Charting.Manager;
using DowJones.Managers.Core;

namespace DowJones.Charting.Fcp
{
    public class BarChartGenerator : AbstractChartGenerator
    {
        #region Constants

        private const int LARGE_HEIGHT = 400;

        private const string DEFAULT_COMBO_APPEARENCE_FILE = "/fcp/fcp_industry_snap_combo_bar.itxml";
        private const string LARGE_SIZED_APPEARENCE_FILE = "/fcp/fcp_segments_bar_small.itxml";
        private const string DEFAULT_LEGEND_APPEARENCE_FILE = "/fcp/fcp_legend.itxml";
        private const int LARGE_WIDTH = 600;
        private const int MAX_ROTATION = 90;
        private const int MIN_ROTATION = 0;
        private const int DEFAULT_SPACE_VFAST = 1;
        private const int DEFAULT_WIDTH_VFAST = 99;
        private const string DEFAULT_SMALL_SIZED_APPEARENCE_FILE = "/fcp/fcp_segments_bar_small.itxml";
        private const string DEFAULT_VFAST_APPEARENCE_FILE = "/fast/vBar_fast.itxml";
        private const string DEFAULT_VFAST_PALETTE_APPEARENCE_FILE = "/fast/vBar_fast_palette.itxml";
        private const string DEFAULT_VFAST_EXPORT_APPEARENCE_FILE = "/fast/vBar_fast_export.itxml";
        //Infosys - 5/1/2010 - INDUSTRY_SNAPSHOT_SIZE_GROUPING Chart - Begin
        private const string DEFAULT_INDUSTRY_SNAPSHOT_SIZE_GROUPING_APPEARENCE_FILE = "/fcp/fcp_ind_snap_size_group.itxml";
        //Infosys - 5/1/2010 - INDUSTRY_SNAPSHOT_SIZE_GROUPING Chart - End
        
        #endregion

        #region Private Variables
        private string appearence_file = string.Empty;
        private Color m_BackgroundColor = Color.White;
        private int m_CategoryScaleLabelRotation = MAX_ROTATION;
        private ChartSize m_ChartSize = ChartSize.Small;
        private AbstractColorPalette m_ColorPalette = new BarChartColorPalette();
        private BarChartDataSet m_DataSet;
        private OutputChartType m_OutputChartType = OutputChartType.FLASH;
        private Color m_GridColor = ColorTranslator.FromHtml("#CCCCCC");
        private Color major_GridColor = ColorTranslator.FromHtml("#000000");
        private Color m_ScaleColor = ColorTranslator.FromHtml("#999999");
        private readonly Legend m_Legend = new Legend("legend");
        private bool m_UseCache = true;
        private string small_sized_appearence_file = string.Empty;
        private string itxmlcombined_combo = string.Empty;
        private int small_width = 0;
        private int small_height = 0;
        private bool iscombo = false;
        private string combograph_name = string.Empty;
        private bool isvfast_barchart = false;
        private static bool isviworks_barchart = false;
        private bool process_legend = true;
        private int percentspace_vfast = 0;
        private int percentwidth_vfast = 0;
        private string descriptor_Tag = string.Empty;
        private string value_Key = string.Empty;
        private string vstart_date = string.Empty;
        private string vend_date = string.Empty;
        private string vdistribution = string.Empty;
        private ArrayList ar_exprtchrt = null;
        private bool isVFastDjce = false;
        private readonly TextBox m_Title = new TextBox("textbox");

        #endregion

        #region Properties

        public BarChartDataSet DataSet
        {
            get
            {
                if (m_DataSet == null)
                {
                    m_DataSet = new BarChartDataSet();
                }
                return m_DataSet;
            }
            set { m_DataSet = value; }
        }

        public static string AppearenceFileCombo
        {
            get
            {
                return DEFAULT_COMBO_APPEARENCE_FILE;
            }

        }

        public static string AppearenceFileVfast
        {
            get
            {
                return DEFAULT_VFAST_APPEARENCE_FILE;
            }

        }


        public static string AppearenceFileExportVfast
        {
            get
            {
                return DEFAULT_VFAST_EXPORT_APPEARENCE_FILE;
            }

        }

        public static bool IsVfastIworks
        {
            get
            {
                return isviworks_barchart;
            }
            set
            {
                isviworks_barchart = value;
            }
        }

        public bool IsVfastDjce
        {
            get
            {
                return isVFastDjce;
            }
            set
            {
                isVFastDjce = value;
            }
        }

        public bool IsVfastBarGraph
        {
            get
            {
                return isvfast_barchart;

            }

            set
            {
                isvfast_barchart = value;

            }
        }

        public int SmallWidth
        {
            get { return small_width; }
            set { small_width = value; }

        }


        public int SmallHeight
        {
            get { return small_height; }
            set { small_height = value; }

        }

        public bool IsProcessLegend
        {
            get { return process_legend; }
            set { process_legend = value; }
        }

        
        
        public string BarChartAppearanceFile
        {
            get
            {
                if (appearence_file == "")
                {
                    appearence_file = DEFAULT_SMALL_SIZED_APPEARENCE_FILE;
                }
                return appearence_file;
            }
            set { appearence_file = value; }
        }

        public string LegendAppearanceFile
        {
            get
            {
                if (appearence_file == "")
                {
                    appearence_file = DEFAULT_LEGEND_APPEARENCE_FILE;
                }
                return appearence_file;
            }
            set
            {
                appearence_file = value;
            }

        }


        public string ComboAppearencefile
        {
            get
            {
                if (appearence_file == "")
                {
                    appearence_file = DEFAULT_COMBO_APPEARENCE_FILE;

                }
                return appearence_file;
            }

            set
            {
                appearence_file = value;

            } 
        }


        public string DescriptorTag
        {
            get
            {
                return descriptor_Tag;
            }
            set
            {
                descriptor_Tag = value;
            }

        }

        public string ValueKey
        {
            get
            {
                return value_Key;
            }
            set
            {
                value_Key = value;
            }

        }


        public static string AppearenceLegendFile
        {

            get { return DEFAULT_LEGEND_APPEARENCE_FILE; }
        }

        public static string AppearenceComboFile
        {
            get { return DEFAULT_COMBO_APPEARENCE_FILE; }
        }


        public static string AppearenceBarChartFile
        {
            get { return DEFAULT_SMALL_SIZED_APPEARENCE_FILE; }


        }

        //Infosys - 5/1/2010 - INDUSTRY_SNAPSHOT_SIZE_GROUPING chart - Begin
        public string IndustrySnapshotSizeGroupingChartFile
        {
            get { return DEFAULT_INDUSTRY_SNAPSHOT_SIZE_GROUPING_APPEARENCE_FILE; }
        }
        //Infosys - 5/1/2010 - INDUSTRY_SNAPSHOT_SIZE_GROUPING chart - End



        public string VstartDate
        {
            get{ return vstart_date; }

            set
            {
                vstart_date = value;
            }

        }


        public string VendDate
        {
            get { return vend_date; }

            set
            {
                vend_date = value;
            }
        }




        public string Vdistribution
        {
            get { return vdistribution; }

            set
            {

                vdistribution = value;
            }

        }


        public int PercentWidthVfast
        {
            get { return percentwidth_vfast; }
            set { percentwidth_vfast = value; }
        }

        public int PercentSpaceVfast
        {
            get { return percentspace_vfast; }
            set { percentspace_vfast = value; }
        }

        public ArrayList ArExprtChrt
        {
            get { return ar_exprtchrt; }
            set { ar_exprtchrt = value; }
        }
          
        public Legend Legend
        {
            get { return m_Legend; }
        }

        protected bool UseCache
        {
            get { return m_UseCache; }
            set { m_UseCache = value; }
        }

        public ChartSize ChartSize
        {
            get { return m_ChartSize; }
            set { m_ChartSize = value; }
        }

        public Color BackgroundColor
        {
            get { return m_BackgroundColor; }
            set { m_BackgroundColor = value; }
        }

        public Color GridColor
        {
            get { return m_GridColor; }
            set { m_GridColor = value; }
        }
        public Color MajorGridColor
        {
            get { return major_GridColor; }
            set { major_GridColor = value; }
        }

        public Color ScaleColor
        {
            get { return m_ScaleColor; }
            set { m_ScaleColor = value; }
        }

        public OutputChartType OutputChartType
        {
            get { return m_OutputChartType; }
            set { m_OutputChartType = value; }
        }

        public int CategoryScaleLabelRotation
        {
            get { return m_CategoryScaleLabelRotation; }
            set
            {
                if (value >= MIN_ROTATION && value <= MAX_ROTATION)
                {
                    m_CategoryScaleLabelRotation = value;
                }
            }
        }

        public TextBox Title
        {
            get { return m_Title; }
        }

        public AbstractColorPalette ColorPalette
        {
            get { return m_ColorPalette; }

            set
            {

                m_ColorPalette = value;

            }
        }

        #endregion

        #region Overrides of AbstractChartGenerator
        
        public override IBytesResponse GetBytes()
        {
           string itxml = (iscombo)?ItXmlCombinedCombo:ToITXML();
            ChartTemplate template = GetChartTemplate();
            byte[] bytes = ChartingManager.GetChartBytes(itxml, string.Empty, GetChartTemplate(), m_UseCache);

            ChartBytesResponse response = new ChartBytesResponse(template.Height, template.Width, bytes, template.OutputChartType);
            return response;
        }
        ///This method is not used and not tested.
        public override IUriResponse GetUri()
        {
            string itxml = ToITXML();
            OutputChartType original = m_OutputChartType;
            switch (m_OutputChartType)
            {
                case OutputChartType.FLASH_WITH_ACTIVEX_FIX:
                    m_OutputChartType = OutputChartType.FLASH;
                    break;
                case OutputChartType.GIF_WITH_JAVASCRIPT_INTERACTIVITY:
                    m_OutputChartType = OutputChartType.GIF;
                    break;
                case OutputChartType.JPEG_WITH_JAVASCRIPT_INTERACTIVITY:
                    m_OutputChartType = OutputChartType.JPEG;
                    break;
                case OutputChartType.PNG_WITH_JAVASCRIPT_INTERACTIVITY:
                    m_OutputChartType = OutputChartType.PNG;
                    break;
            }

            ChartTemplate template = GetChartTemplate();
            m_OutputChartType = original;
            string tUri = ChartingManager.ExtractSourceAttributeFromEmbededHTML(ChartingManager.GetChartEmbededHtmlByITXML(itxml, string.Empty, template, m_UseCache));

            ChartUriResponse response = new ChartUriResponse(template.Height, template.Width, tUri, original);
            return response;
        }

        public override IEmbeddedHTMLResponse GetHTML()
        {
            string itxml = string.Empty;
            if (IsVfastBarGraph || IsVfastIworks)
            {
                itxml = ToITXMLVFAST();
                return GetChartEmbeddedHTMLResponse(itxml);
            }
            if (iscombo)
            {
                itxml = ItXmlCombinedCombo;
            }
            //Infosys - 5/1/2010 - INDUSTRY_SNAPSHOT_SIZE_GROUPING chart - Begin
            else
            {
                if (BarChartAppearanceFile == IndustrySnapshotSizeGroupingChartFile)
                {
                    itxml = ToITXMLIndSnapshot();
                }
                //Infosys - 5/1/2010 - INDUSTRY_SNAPSHOT_SIZE_GROUPING chart - End
                else
                {
                    itxml = ToITXML();
                }
            }
            return GetChartEmbeddedHTMLResponse(itxml);
        }  

        private ChartEmbeddedHTMLResponse GetChartEmbeddedHTMLResponse (string itxml) 
        {  
            ChartTemplate template = GetChartTemplate();
            string tEmbededHTML = ChartingManager.GetChartEmbededHtmlByITXML(itxml, string.Empty, GetChartTemplate(), m_UseCache);

            ChartEmbeddedHTMLResponse response = new ChartEmbeddedHTMLResponse(template.Height, template.Width, tEmbededHTML, template.OutputChartType);
            return response;
   
        }


        

        internal override ChartTemplate GetChartTemplate()
        {
            if (IsVfastDjce)
            {
                if (DataSet.Series.CategoryNames.Count > 5)
                {
                    appearence_file = DEFAULT_VFAST_PALETTE_APPEARENCE_FILE;
                }
            }
            switch (ChartSize)
            {
                case ChartSize.Large:
                    return new ChartTemplate(appearence_file, LARGE_WIDTH, LARGE_HEIGHT, OutputChartType);
                default:
                    return new ChartTemplate(appearence_file, SmallWidth, SmallHeight, OutputChartType);
            }
        }

        public string ItXmlCombinedCombo
        {
            get
            {
                return itxmlcombined_combo;
            }

            set
            {
                itxmlcombined_combo = value;
            }
        }

        public string ComboGraphName
        {
            get
            {
                return combograph_name;
            }
            set
            {
                combograph_name = value;

            }
        }




        public string ToITXMLVFAST()
        {

            StringBuilder sb = new StringBuilder();

            //sb.Append(SetBackgroundColor());
            //sb.Append("<cit:bar-graph name=\"graph\" method=\"replace\">");
            sb.Append("<cit:bar-graph name=\"graph\">");
            //sb.Append("<cit:bar-graph name=\"graph\" common =\"top:10;left:10;width:210;height:300\">"); 
            // Add the color palette 
            
            sb.Append("<cit:graph-settings>");
            sb.Append(ColorPalette.ToITXML());
            sb.Append("<cit:drilldown highlight-mode=\"bold-outline\">");
            sb.Append("</cit:drilldown>");
            if (PercentSpaceVfast != 0 || PercentWidthVfast != 0)
            {  PercentSpaceVfast = (PercentSpaceVfast!= 0) ? PercentSpaceVfast : DEFAULT_SPACE_VFAST; 
                PercentWidthVfast = (PercentWidthVfast!=0) ? PercentWidthVfast : DEFAULT_WIDTH_VFAST;
                
                sb.Append(string.Format("<cit:bar-settings bar-style=\"gradient\" percent-space-between-bars=\"{0}\" percent-bar-width=\"{1}\" gradient-direction=\"right-left\" gradient-type=\"color\" gradient-color=\"#9999CC\"/>",PercentSpaceVfast,PercentWidthVfast));

            } 
            else  
            {   
                sb.Append("<cit:bar-settings bar-style=\"gradient\" percent-space-between-bars=\"1\" percent-bar-width=\"99\" gradient-direction=\"right-left\" gradient-type=\"color\" gradient-color=\"#9999CC\"/>");
            }
                

        
            sb.Append("</cit:graph-settings>");

            //// Set the Grid Colors 
            //if (!IsVfastIworks)
            //{
            //    sb.Append(SetGridColors());
            //}

            // Set the Scales
            sb.Append(SetScales());

            // Add Data
            sb.Append(ProcessData());

            // Add Legend
            
         

            // Add Title   
            sb.Append("</cit:bar-graph>");
            if(VstartDate != string.Empty || VendDate != string.Empty || Vdistribution!= string.Empty) 
            {
                sb.Append(ProcessTitleVFast());

            }
            
            return sb.ToString();

        }


        public string ToITXMLCombo()
        {
            iscombo = true;
            StringBuilder sb = new StringBuilder();
            sb.Append(SetBackgroundColor());
            //sb.Append("<cit:bar-graph name=\"graph\" method=\"replace\">"); 
            sb.Append(string.Format("<cit:bar-graph name=\"{0}\">", ComboGraphName));
            //sb.Append("<cit:bar-graph name=\"graph\" common =\"top:10;left:10;width:210;height:300\">"); 


            // Add the color palette
            sb.Append("<cit:graph-settings>");
            if (ComboGraphName == "LSide")
            {
                sb.Append(ColorPalette.ToITXML());
            }
            else
            {
                AbstractColorPalette m_ColorPalette = new ComboBarChartColorPalette();
                sb.Append(m_ColorPalette.ToITXML());
            }
            sb.Append("<cit:drilldown highlight-mode=\"bold-outline\">");
            sb.Append("</cit:drilldown>");
            sb.Append("</cit:graph-settings>");

            // Set the Grid Colors
            sb.Append(SetGridColors());

            // Set the Scales
            sb.Append(SetScales());

            // Add Data
            sb.Append(ProcessData());

            // Add Legend

            sb.Append(ProcessLegend());
            sb.Append("</cit:bar-graph>");

            // Add Title
            sb.Append(ProcessTitle());
            return sb.ToString();


        }

        //Infosys - 5/1/2010 - INDUSTRY_SNAPSHOT_SIZE_GROUPING chart - Begin
        public string ToITXMLIndSnapshot()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<cit:bar-graph name=\"graph\">");
            sb.Append(ProcessData());
            sb.Append("</cit:bar-graph>");
            return sb.ToString();
        }
        //Infosys - 5/1/2010 - INDUSTRY_SNAPSHOT_SIZE_GROUPING chart - End


        internal override string ToITXML()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(SetBackgroundColor());
            //sb.Append("<cit:bar-graph name=\"graph\" method=\"replace\">");
            sb.Append("<cit:bar-graph name=\"graph\">");
            //sb.Append("<cit:bar-graph name=\"graph\" common =\"top:10;left:10;width:210;height:300\">"); 
            // Add the color palette
            sb.Append("<cit:graph-settings>");
            sb.Append(ColorPalette.ToITXML());
            sb.Append("<cit:drilldown highlight-mode=\"bold-outline\">");
            sb.Append("</cit:drilldown>");
            sb.Append("</cit:graph-settings>");

            // Set the Grid Colors
            sb.Append(SetGridColors());

            // Set the Scales
            sb.Append(SetScales());

            // Add Data
            sb.Append(ProcessData());

            // Add Legend
            if (IsProcessLegend)
            {
                sb.Append(ProcessLegend());
            }
            sb.Append("</cit:bar-graph>");

            // Add Title  
            if (DescriptorTag != string.Empty || ValueKey != string.Empty)
            {
                sb.Append(ProcessTitleWithText());
            }
            else
            {
                sb.Append(ProcessTitle());
            }
            return sb.ToString();


        }

        #endregion

        #region Methods
        private string SetBackgroundColor()
        {
            return m_BackgroundColor != Color.White ? string.Format("<cit:canvas show-border=\"false\" fill-color=\"{0}\"/>", ColorTranslator.ToHtml(BackgroundColor)) : string.Empty;
        }

        private string SetScales()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<cit:value-scale position=\"left\" sync-with-primary-scale=\"false\">");
            if (IsVfastBarGraph || IsVfastIworks)
            {
                sb.AppendFormat("<cit:value-scale-labels rotate-labels=\"false\" value-format=\"override-maximum-decimal-places:true;override-always-show-maximum-decimal-places:true\" font=\"name:Arial Unicode MS;\"/>");
            }
            else
            {
                sb.AppendFormat("<cit:value-scale-labels rotate-labels=\"false\" value-format=\"override-maximum-decimal-places:true;override-always-show-maximum-decimal-places:true\" font=\"name:Arial Unicode MS;color:{0}\"/>", ColorTranslator.ToHtml(ScaleColor));
            }
            sb.Append("<cit:value-scale-title text=\"\" reverse-title-direction=\"false\" font=\"name:Arial Unicode MS;size:10;color:black\"/>");
            sb.Append("<cit:value-scale-divisions set-scale-values=\"automatically\" include-zero=\"true\" maximum-major-divisions=\"6\" use-log-scale=\"false\"/>");
            sb.Append("</cit:value-scale>");
            if (IsVfastBarGraph || IsVfastIworks)
            {
                sb.AppendFormat("<cit:category-scale position=\"below\" display-scale-labels=\"false\" limit-label-length=\"false\" enclose-labels=\"false\" line-between-labels=\"false\" manual-tick-location=\"false\" tick-location=\"edges\" first-label-to-be-displayed=\"1\" font=\"name:Arial Unicode MS;size:10;color:{0}\">", ColorTranslator.ToHtml(ScaleColor));
            }
            else
            {
                sb.AppendFormat("<cit:category-scale position=\"below\" display-scale-labels=\"true\" limit-label-length=\"false\" enclose-labels=\"false\" line-between-labels=\"false\" manual-tick-location=\"false\" tick-location=\"edges\" first-label-to-be-displayed=\"1\" font=\"name:Arial Unicode MS;size:10;color:{0}\">", ColorTranslator.ToHtml(ScaleColor));
            }
             if (CategoryScaleLabelRotation == MIN_ROTATION)
                sb.Append("<cit:adjust-labels perform-adjustments=\"always\" rotate-labels=\"false\" rotation=\"0\" rotate-wrap-width=\"75\" skip-labels=\"false\" show-one-label-for-every=\"2\"/>");
            else
                sb.AppendFormat("<cit:adjust-labels perform-adjustments=\"always\" rotate-labels=\"true\" rotation=\"{0}\" rotate-wrap-width=\"75\" skip-labels=\"false\" show-one-label-for-every=\"2\"/>", CategoryScaleLabelRotation);
            sb.Append("</cit:category-scale>");
            return sb.ToString();
        }

        private string SetGridColors()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<cit:grid>");
            sb.AppendFormat("<cit:outer-lines max-line=\"color:{0}\" min-line=\"color:{0}\" low-outer-line=\"color:{0}\" high-outer-line=\"color:{0}\"/>", ColorTranslator.ToHtml(MajorGridColor));
            sb.Append("<cit:inner-lines>");
            //<cit:inner-lines>
            //    <cit:horizontal-lines zero-line="show-line:false;color:red" minor-grid-line="color:#CCCCCC"/>
            //</cit:inner-lines>
            //sb.AppendFormat("<cit:vertical-lines major-grid-line=\"show-line:true;color:{0}\" minor-grid-line=\"show-line:true;color:{0}\"/>",ColorTranslator.ToHtml(MajorGridColor), ColorTranslator.ToHtml(GridColor));
            //sb.AppendFormat("<cit:horizontal-lines zero-line=\"color:{0}\" major-grid-line=\"color:{0}\" minor-grid-line=\"show-line:true;color:{0}\"/>",ColorTranslator.ToHtml(MajorGridColor),ColorTranslator.ToHtml(GridColor));
            if (IsVfastBarGraph)
            {
                sb.Append("<cit:horizontal-lines zero-line=\"show-line:false\" major-grid-line=\"line-style:dotted;color:#E0E0E0\" minor-grid-line=\"show-line:false;color:#A5A5A5\"/>");
                sb.Append("</cit:inner-lines>");
                sb.Append("<cit:ticks>");
                
				sb.Append("<cit:vertical-ticks major-tick=\"visible:false;length:4\"/>");
				sb.Append("<cit:horizontal-ticks major-tick=\"position:inside;color:#A5A5A5;length:4\" minor-tick=\"visible:false;color:#CCCCCC;length:3\"/>");
			    sb.Append("</cit:ticks>");
                sb.Append("<cit:background show-background=\"false\"/>");
                //sb.AppendFormat("<cit:horizontal-lines zero-line=\"show-line:false\" minor-grid-line=\"show-line:false;color:{0}\"/>", ColorTranslator.ToHtml(GridColor));
            }
            else
            {
                sb.AppendFormat("<cit:horizontal-lines zero-line=\"show-line:false\" minor-grid-line=\"show-line:true;color:{0}\"/>", ColorTranslator.ToHtml(GridColor));
                sb.Append("</cit:inner-lines>");
                sb.Append("<cit:ticks>");
                sb.AppendFormat("<cit:vertical-ticks major-tick=\"color:{0};length:4\" minor-tick=\"position:inside;color:{0};length:2\"/>", ColorTranslator.ToHtml(ScaleColor));
                sb.AppendFormat("<cit:horizontal-ticks major-tick=\"color:{0};length:4\" minor-tick=\"position:inside;color:{0};length:2\"/>", ColorTranslator.ToHtml(ScaleColor));
                sb.Append("</cit:ticks>");
                sb.AppendFormat("<cit:background color=\"{0}\"/>", ColorTranslator.ToHtml(BackgroundColor));

            }
            
            sb.Append("</cit:grid>");
            return sb.ToString();
        }

        private string ProcessLegend()
        {
            return Legend.ToITXMLBarchart();
        }

        private string ProcessTitle()
        {
            return Title.ToITXML();
        }

        private string ProcessData()
        {
            // Get the Bar-Series nodes
            //<cit:bar-series name="Series 1" number="1" />
            StringBuilder mainSB = new StringBuilder();
            StringBuilder outerSB = new StringBuilder();

            if (DataSet.Series != null)
            {
                mainSB.Append("<cit:data series-data-in-columns=\"true\">");
                for (int i = 1; i <= DataSet.Series.CategoryNames.Count; i++)
                {
                    string name = DataSet.Series.CategoryNames[i - 1];
                    outerSB.AppendFormat("<cit:bar-series name=\"{0}\" number=\"{1}\" />", name, i);
                    mainSB.AppendFormat("<cit:column name=\"{0}\"/>", name);
                }
                mainSB.Insert(0, outerSB.ToString());
                foreach (SeriesItem si in DataSet.Series.Items)
                {
                    mainSB.AppendFormat("<cit:row name=\"{0}\">", StringUtilitiesManager.XmlEncode(si.Name));
                    // Get the dict obj
                    foreach (DataItem item in si.DataItems)
                    {
                        mainSB.Append(item.ToITXML());
                    }
                    mainSB.Append("</cit:row>");
                }

                mainSB.Append("</cit:data>");

                return mainSB.ToString();

            }
            return "<cit:data />";
        }
        
        private string ProcessTitleWithText()
        {
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<cit:textbox name=\"title\" positioned-in-builder=\"true\" common=\"top:2;left:10;width:516;height:20\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>", DescriptorTag);
            sb.Append("<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" font=\"name:Arial Unicode MS;size:12;color:#000000\" fill-enabled=\"false\">");
            sb.Append("<cit:textbox-settings-layout shrink-text-to-fit=\"false\" vert-alignment=\"top\" left-margin=\"0\"/>");
            sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"valueKey\" positioned-in-builder=\"true\" common=\"top:2;left:190;width:210;height:22\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>",ValueKey);
            sb.Append("<cit:textbox-settings horiz-justification=\"right\" show-border=\"false\" fill-enabled=\"false\" font=\"name:Arial Unicode MS;size:10;color:#999999\">");
           // sb.AppendFormat(string.Format("<cit:textbox-settings horiz-justification=\"right\" show-border=\"false\" fill-enabled=\"false\" font=\"size:10;color:{0}\">", ColorTranslator.ToHtml(textColour)));
            sb.Append("<cit:textbox-settings-layout right-margin=\"0\"/>");
            sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");

            return sb.ToString();

        }

        private string ProcessTitleVFast() 
        {
            StringBuilder sb = new StringBuilder();
            if (ArExprtChrt != null)
            {
                if (ArExprtChrt.Count > 0)
                {
                    sb.Append("<cit:textbox name=\"graphLabel\">");
                    sb.AppendFormat("<cit:text content=\"{0}\"/>", ArExprtChrt[0].ToString());
                   // sb.Append("<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-color=\"#E5E5E5\" shadow-settings=\"show:true;style:glow\" font=\"name:Arial Unicode MS;size:12\">");
                    //sb.Append("<cit:textbox-settings-layout left-margin=\"5\"/>");
                    //sb.Append("</cit:textbox-settings>");
                    sb.Append("</cit:textbox>");

                }
            }
            sb.Append("<cit:textbox name=\"Distribution\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>",Vdistribution);
            //sb.Append("<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" font=\"name:Arial Unicode MS;size:10\" fill-enabled=\"false\">");
            //sb.Append("<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-width=\"true\" auto-height=\"true\" vert-alignment=\"top\" top-margin=\"0\" bottom-margin=\"0\" left-margin=\"5\" right-margin=\"0\"/>");
            //sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"StartDate\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>",VstartDate);
            //sb.Append("<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled=\"false\" font=\"name:Arial Unicode MS;size:10\">");
            //// sb.AppendFormat(string.Format("<cit:textbox-settings horiz-justification=\"right\" show-border=\"false\" fill-enabled=\"false\" font=\"size:10;color:{0}\">", ColorTranslator.ToHtml(textColour)));
            //sb.Append("<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-width=\"true\" auto-height=\"true\" vert-alignment=\"top\" top-margin=\"0\" bottom-margin=\"0\" left-margin=\"5\" right-margin=\"0\"/>");
            ////sb.Append("<cit:textbox-settings-layout right-margin=\"0\"/>");
            //sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");
            sb.Append("<cit:textbox name=\"EndDate\">");
            sb.AppendFormat("<cit:text content=\"{0}\"/>",VendDate); 
            //sb.Append("<cit:textbox-settings horiz-justification=\"right\" show-border=\"false\" fill-enabled=\"false\" font=\"name:Arial Unicode MS;size:10\">");
            //sb.Append("<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-width=\"true\" auto-height=\"true\" vert-alignment=\"top\" top-margin =\"0\" bottom-margin=\"0\" left-margin=\"0\" right-margin=\"5\"/>");
            //sb.Append("</cit:textbox-settings>");
            sb.Append("</cit:textbox>");

            if (ArExprtChrt != null)
            {
                if (ArExprtChrt.Count > 0)
                {
                    string copyrightText = "&#xa9;" + ArExprtChrt[1].ToString() + " Factiva";
                    sb.Append("<cit:textbox name=\"textbox2\">");
                    sb.AppendFormat("<cit:text content=\"{0}\"/>", copyrightText);
                    //sb.Append("<cit:textbox-settings horiz-justification=\"left\" show-border=\"false\" fill-enabled=\"false\" font=\"name:Arial Unicode MS;size:9;color:#595959\">");
                    //sb.Append("<cit:textbox-settings-layout shrink-text-to-fit=\"false\" auto-width=\"true\" auto-height=\"true\" vert-alignment=\"top\"/>");
                    //sb.Append("</cit:textbox-settings>");
                    sb.Append("</cit:textbox>");


                }
            }

            return sb.ToString();

        }  




  //      <cit:textbox name="Distribution" positioned-in-builder="true" common="top:127;width:97;height:11">
  //  <cit:text content="Distribution: Weekly"/>
  //  <cit:textbox-settings horiz-justification="left" show-border="false" fill-enabled="false" font="size:10">
  //    <cit:textbox-settings-layout shrink-text-to-fit="false" auto-width="true" auto-height="true" vert-alignment="top" top-margin="0" bottom-margin="0" left-margin="5" right-margin="0"/>
  //  </cit:textbox-settings>
  //</cit:textbox>
  //<cit:textbox name="StartDate" positioned-in-builder="true" common="top:110;width:59;height:11">
  //  <cit:text content="12/31/2005"/>
  //  <cit:textbox-settings horiz-justification="left" show-border="false" fill-enabled="false" font="size:10">
  //    <cit:textbox-settings-layout shrink-text-to-fit="false" auto-width="true" auto-height="true" vert-alignment="top" top-margin="0" bottom-margin="0" left-margin="5" right-margin="0"/>
  //  </cit:textbox-settings>
  //</cit:textbox>
  //<cit:textbox name="EndDate" positioned-in-builder="true" common="top:110;left:145;width:59;height:11">
  //  <cit:text content="12/31/2005"/>
  //  <cit:textbox-settings horiz-justification="right" show-border="false" fill-enabled="false" font="size:10">
  //    <cit:textbox-settings-layout shrink-text-to-fit="false" auto-width="true" auto-height="true" vert-alignment="top" top-margin="0" bottom-margin="0" left-margin="0" right-margin="5"/>
  //  </cit:textbox-settings>
  //</cit:textbox>


        #endregion
    }
}

