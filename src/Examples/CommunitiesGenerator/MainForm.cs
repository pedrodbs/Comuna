// ------------------------------------------
// <copyright file="MainForm.cs" company="Pedro Sequeira">
// 
//     Copyright (c) 2018 Pedro Sequeira
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// </copyright>
// <summary>
//    Project: CommunitiesGenerator
//    Last updated: 06/18/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Comuna;
using Comuna.D3;
using Comuna.Graphviz;
using Newtonsoft.Json;
using QuickGraph.Graphviz.Dot;

namespace CommunitiesGenerator
{
    public partial class MainForm : Form
    {
        #region Static Fields & Constants

        private const int CONNECTION_CHANCE = 10;
        private const string D3_FILE_NAME = "graph.json";
        private const string GRAPHVIZ_FILE_NAME = "graph";
        private const int WAIT_TIMEOUT = 3000;
        private const GraphvizImageType GRAPHVIZ_IMAGE_TYPE = GraphvizImageType.Png;

        #endregion

        #region Fields

        private readonly Random _rand = new Random();

        #endregion

        #region Constructors

        public MainForm()
        {
            this.InitializeComponent();
            this.colorSchemeCBox.SelectedIndex = 0;
            this.colorSchemeCBox.SelectedText = string.Empty;
        }

        #endregion

        #region Private & Protected Methods

        private static void UpdateCommunities(CommunityTracker communityTracker)
        {
            communityTracker.CommunityAlg.Update();
            communityTracker.Update();
        }

        private void AlgorithmDoWork(object sender, DoWorkEventArgs e)
        {
            // retrieves arguments
            var args = (object[]) e.Argument;
            var communityTracker = (CommunityTracker) (e.Result = args[0]);
            var maxUpdates = (uint) args[1];
            var network = communityTracker.Network;

            // updates algorithm and tracker
            UpdateCommunities(communityTracker);

            // updates connections and tracker
            for (var t = 0; t < maxUpdates; t++)
            {
                // removes some edges
                foreach (var connection in network.Edges.ToList())
                    if (this._rand.Next(CONNECTION_CHANCE) == 0)
                        network.RemoveEdge(connection);

                // creates new connections
                for (var i = 0u; i < this.numNodesNumUD.Value; i++)
                for (var j = i + 1; j < this.numNodesNumUD.Value; j++)
                    if (!network.ContainsEdge(i, j) && this._rand.Next(CONNECTION_CHANCE) == 0)
                        network.AddEdge(new Connection(i, j, this._rand.NextDouble()));

                // updates algorithm and tracker
                UpdateCommunities(communityTracker);
            }
        }

        private void AlgorithmWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var communityTracker = (CommunityTracker) e.Result;

            // exports final community graph to image
            var fullPath = Path.GetFullPath(".");
            PaletteGenerator paletteGenerator;
            switch (this.colorSchemeCBox.SelectedIndex)
            {
                case 0:
                    paletteGenerator = TolPalettes.CreateTolDivPalette;
                    break;
                default:
                    paletteGenerator = TolPalettes.CreateTolRainbowPalette;
                    break;
            }

            var dotFile = communityTracker.CommunityAlg.ToGraphvizFile(
                fullPath, GRAPHVIZ_FILE_NAME, this.showLabelsCheckBox.Checked,
                GRAPHVIZ_IMAGE_TYPE, paletteGenerator, WAIT_TIMEOUT);

            communityTracker.Dispose();

            // checks dot
            if (!File.Exists(dotFile))
            {
                this.infoLabel.Text = "Could not generate file!";
                this.EnableInterface(true);
                return;
            }

            // checks and loads image
            var imageFile = $"{dotFile}.{GRAPHVIZ_IMAGE_TYPE.GetOutputFormatStr()}";
            this.pictureBox.Image?.Dispose();
            if (!File.Exists(imageFile))
            {
                this.infoLabel.Text = "Could not generate image file! Check that you have Graphviz installed.";
                this.EnableInterface(true);
                return;
            }

            Image img;
            using (var bmpTemp = new Bitmap(imageFile))
                img = new Bitmap(bmpTemp);
            this.pictureBox.Image = img;

            // informs of d3 json file
            this.infoLabel.Text = $"D3 json file saved to: {communityTracker.FilePath}";
            this.EnableInterface(true);
        }

        private void EnableInterface(bool enable)
        {
            this.groupBox1.Enabled = this.groupBox2.Enabled = this.groupBox3.Enabled = this.groupBox4.Enabled = enable;
        }

        private void GenerateBtnClick(object sender, EventArgs e)
        {
            // creates network nodes
            var network = new Network();
            for (var i = 0u; i < this.numNodesNumUD.Value; i++)
                network.AddVertex(i);

            // creates connections
            for (var i = 0u; i < this.numNodesNumUD.Value; i++)
            for (var j = i + 1; j < this.numNodesNumUD.Value; j++)
                if (this._rand.Next(CONNECTION_CHANCE) == 0)
                    network.AddEdge(new Connection(i, j, this._rand.NextDouble()));

            // creates community algorithm
            var communityAlg = new CommunityAlgorithm(
                network, (int) this.numPassesNumUD.Value, (double) this.minModularityNumUD.Value);

            // creates tracker
            var filePath = Path.GetFullPath(Path.Combine(".", D3_FILE_NAME));
            var maxUpdates = (uint) this.maxUpdatesNumUD.Value;
            var communityTracker = new CommunityTracker(communityAlg, filePath, maxUpdates, Formatting.Indented);

            // runs update algorithm
            this.EnableInterface(false);
            this.algorithmWorker.RunWorkerAsync(new object[] {communityTracker, maxUpdates});
        }

        #endregion
    }
}