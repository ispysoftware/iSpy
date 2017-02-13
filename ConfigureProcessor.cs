using System;
using System.Drawing;
using System.Windows.Forms;
using iSpyApplication.Controls;

namespace iSpyApplication
{
    public partial class ConfigureProcessor : Form
    {
        private readonly CameraWindow _cameraControl;

        public ConfigureProcessor(CameraWindow cw)
        {
            InitializeComponent();
            RenderResources();
            _cameraControl = cw;
        }

        private void RenderResources()
        {
            chkKeepEdges.Text = LocRm.GetString("KeepEdges");
            label47.Text = LocRm.GetString("Tracking");
            label3.Text = LocRm.GetString("ObjectTrackingOptions");
            label48.Text = LocRm.GetString("MinimumWidth");
            label2.Text = LocRm.GetString("MinimumHeight");
            chkHighlight.Text = LocRm.GetString("Highlight");

            Text = LocRm.GetString("Configure");
            button1.Text = LocRm.GetString("OK");
        }

        private void ConfigureProcessorLoad(object sender, EventArgs e)
        {
            cdTracking.Color = pnlTrackingColor.BackColor = ColorTranslator.FromHtml(_cameraControl.Camobject.detector.color);
            chkKeepEdges.Checked = _cameraControl.Camobject.detector.keepobjectedges;
            numWidth.Value = _cameraControl.Camobject.detector.minwidth;
            numHeight.Value = _cameraControl.Camobject.detector.minheight;
            chkHighlight.Checked = _cameraControl.Camobject.detector.highlight;
        }

        private void Button1Click(object sender, EventArgs e)
        {
            _cameraControl.Camobject.detector.keepobjectedges = chkKeepEdges.Checked;
            _cameraControl.Camobject.detector.color = ColorTranslator.ToHtml(cdTracking.Color);
            _cameraControl.Camobject.detector.highlight = chkHighlight.Checked;

            //if (CameraControl.Camera != null && CameraControl.Camera.MotionDetector != null)
            //{
            //    switch (CameraControl.Camobject.detector.postprocessor)
            //    {
            //        case "Grid Processing":
            //            ((GridMotionAreaProcessing)CameraControl.Camera.MotionDetector.MotionProcessingAlgorithm).
            //                HighlightColor = ColorTranslator.FromHtml(CameraControl.Camobject.detector.color);
            //            break;
            //        case "Object Tracking":
            //            ((BlobCountingObjectsProcessing)
            //             CameraControl.Camera.MotionDetector.MotionProcessingAlgorithm).HighlightColor =
            //                ColorTranslator.FromHtml(CameraControl.Camobject.detector.color);
            //            break;
            //        case "Object Tracking (no overlay)":
            //            ((BlobCountingObjectsProcessing)
            //             CameraControl.Camera.MotionDetector.MotionProcessingAlgorithm).HighlightMotionRegions =
            //                false;
            //            break;
            //        case "Border Highlighting":
            //            ((MotionBorderHighlighting)CameraControl.Camera.MotionDetector.MotionProcessingAlgorithm).
            //                HighlightColor = ColorTranslator.FromHtml(CameraControl.Camobject.detector.color);
            //            break;
            //        case "Area Highlighting":
            //            ((MotionAreaHighlighting)CameraControl.Camera.MotionDetector.MotionProcessingAlgorithm).
            //                HighlightColor = ColorTranslator.FromHtml(CameraControl.Camobject.detector.color);
            //            break;
            //        case "None":
            //            break;
            //    }
            //}

            _cameraControl.Camobject.detector.minwidth = (int)numWidth.Value;
            _cameraControl.Camobject.detector.minheight = (int)numHeight.Value;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void pnlTrackingColor_Click(object sender, EventArgs e)
        {
            ShowTrackingColor();
        }

        private void ShowTrackingColor()
        {
            cdTracking.Color = pnlTrackingColor.BackColor;
            if (cdTracking.ShowDialog(this) == DialogResult.OK)
            {
                pnlTrackingColor.BackColor = cdTracking.Color;
                
            }
        }

    }
}
